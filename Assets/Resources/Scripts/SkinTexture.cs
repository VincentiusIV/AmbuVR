using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinTexture : MonoBehaviour {

    [SerializeField] private Texture2D unburned;
    [SerializeField] private Texture2D burned;
    [SerializeField] private int radius;
    [SerializeField] private float updateTime;
    [SerializeField] private PerlinValues pv;

    private Renderer rend;
    private Texture2D mix;
    private Texture2D savedMix;

    private Patient pt;

    private float nextUpdate;
    // Counter for the amount of pixels that are changed
    private int pixelCounter;

    // Use this for initialization
    void Start ()
    {
        rend = GetComponent<Renderer>();
        pt = GetComponent<Patient>();

        // Make a copy of the given unburned texture and use it
        savedMix = mix = Instantiate(unburned) as Texture2D;
        rend.material.mainTexture = mix;
    }

    public void Highlight(Vector2 textureCoord)
    {
        if (Time.time < nextUpdate)
            return;

        nextUpdate = Time.time + updateTime;

        mix = Instantiate(savedMix) as Texture2D;
        rend.material.mainTexture = mix;

        SetPixels(textureCoord, false);
    }

    public void SetPixels(Vector2 textureCoord, bool save, Vector3 worldPoint = new Vector3())
    {
        Vector2 pixelUV = textureCoord;
        pixelUV.x *= unburned.width;
        pixelUV.y *= unburned.height;

        int xPos = (int)pixelUV.x;
        int yPos = (int)pixelUV.y;

        for (int x = xPos - radius; x < xPos + radius; x++)
        {
            for (int y = yPos - radius; y < yPos + radius; y++)
            {
                if (GetPerlinHeight(x, y) > pv.burnHeight)
                    continue;

                Color col = burned.GetPixel(x, y);

                if(save && savedMix.GetPixel(x,y) != col)
                {
                    savedMix.SetPixel(x, y, col);
                    
                    pixelCounter++;
                }
                else if (mix.GetPixel(x, y) != col)
                    mix.SetPixel(x, y, col); 
            }
        }
        //float newTbsa = GetTBSA();
        if (save)
        {
            savedMix.Apply(true);
            pt.PlaceBurn(worldPoint);
        } 
        else mix.Apply(true);

        // actually apply all SetPixels, don't recalculate mip levels
        
    }

    private float GetPerlinHeight(int x, int y)
    {
        float frequency = 1;
        float amplitude = 1;
        float height = 0;

        for (int i = 0; i < pv.numOfLayers; i++)
        {
            float perlin = Mathf.PerlinNoise(x / pv.scale * frequency, y / pv.scale * frequency);
            height += perlin * amplitude;

            amplitude *= pv.ampPerLayer;
            frequency *= pv.frePerLayer;
        }

        return height;
    }


    public float GetTBSA()
    {
        float totalPixels = mix.width * mix.height;
        float tbsa = (pixelCounter / totalPixels) * 100;
        Debug.Log(tbsa+ "%");
        
        return tbsa;
    }
}

[System.Serializable]
public struct PerlinValues
{
    public float scale;
    public int numOfLayers;
    public float ampPerLayer;
    public float frePerLayer;
    public float burnHeight;
}
