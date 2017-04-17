using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SkinTexture : MonoBehaviour {

    [SerializeField] private Texture2D unburned;
    [SerializeField] private Texture2D burnedFirst;

    [SerializeField] private int radius;
    [SerializeField] private float updateTime;
    [SerializeField] private PerlinValues pv;

    [SerializeField]private Renderer rend;
    [SerializeField] private Renderer sharedRenderer;

    private Texture2D mix;
    private Texture2D savedMix;
    private Patient pt;

    public bool drawCubes = true;

    private float nextUpdate;
    // Counter for the amount of pixels that are changed
    private int pixelCounter;
    private Vector2 currentTC;
    private TBSA_Controller tbsa;

    private float[,] noiseMap;

    // Use this for initialization
    private void Start()
    {
        BootSequence();
    }

    public void BootSequence ()
    {
        tbsa = GameObject.FindWithTag("TBSA").GetComponent<TBSA_Controller>();

        if (rend == null)
            rend = GetComponent<Renderer>();
        pt = GetComponent<Patient>();

        ResetTexture();

        CalcNoiseMap();

        if (sharedRenderer != null)
        {
            sharedRenderer.material = rend.material;
        }
            
    }

    public void Highlight(Vector2 textureCoord)
    {
        if (Time.time < nextUpdate || textureCoord == currentTC)
            return;
        
        currentTC = textureCoord;
        nextUpdate = Time.time + updateTime;

        mix = Instantiate(savedMix) as Texture2D;
        rend.material.mainTexture = mix;

        SetPixels(textureCoord, false);
    }

    public void SetPixels(Vector2 textureCoord, bool save, bool amplify = true, Vector3 worldPoint = new Vector3())
    {
        Vector2 pixelUV = textureCoord;
        if(amplify)
        {
            pixelUV.x *= unburned.width;
            pixelUV.y *= unburned.height;
        }

        int xPos = (int)pixelUV.x;
        int yPos = (int)pixelUV.y;

        for (int x = xPos - radius; x < xPos + radius; x++)
        {
            for (int y = yPos - radius; y < yPos + radius; y++)
            {
                if (x < 0 || x > pixelUV.x || y < 0 || y > pixelUV.y)
                    continue;
                if (noiseMap[x,y] > pv.burnHeight)
                    continue;

                Color col = burnedFirst.GetPixel(x, y);

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
            if(worldPoint != new Vector3() && drawCubes)
                pt.PlaceBurn(worldPoint);
        } 
        else mix.Apply(true);
        Debug.Log("TBSA is now: " + GetTBSA());
    }

    public void ResetTexture()
    {
        // Make a copy of the given unburned texture and use it
        savedMix = mix = Instantiate(unburned) as Texture2D;
        rend.material.mainTexture = mix;
    }

    private void CalcNoiseMap()
    {
        noiseMap = new float[unburned.width, unburned.height];
        for (int x = 0; x < unburned.width; x++)
            for (int y = 0; y < unburned.height; y++)
                noiseMap[x, y] = GetPerlinHeight(x,y);
    }

    private float GetPerlinHeight(int x, int y)
    {
        float xFrequency = 1;
        float yFrequency = 1;
        float amplitude = 1;
        float height = 0;

        for (int i = 0; i < pv.numOfLayers; i++)
        {
            float perlin = Mathf.PerlinNoise(x / pv.scale * xFrequency, y / pv.scale * yFrequency);
            height += perlin * amplitude;

            amplitude *= pv.ampPerLayer;
            xFrequency *= pv.xFrePerLayer;
            yFrequency *= pv.yFrePerLayer;
        }
        return height;
    }

    public float GetTBSA()
    {
        if (mix == null)
            return 0f;
        float totalPixels = mix.width * mix.height;
        float tbsa = (pixelCounter / totalPixels) * 100;
        return tbsa;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12))
            SaveToPNG();
    }

    void SaveToPNG()
    {
        Debug.Log("Saving to PNG...");
        byte[] bytes = savedMix.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "SavedMix.png", bytes);
        Debug.Log("TBSA: " + GetTBSA());
    }
}

[System.Serializable]
public struct PerlinValues
{
    public float scale;
    public int numOfLayers;
    public float ampPerLayer;
    public float xFrePerLayer, yFrePerLayer;
    public float burnHeight;
}
