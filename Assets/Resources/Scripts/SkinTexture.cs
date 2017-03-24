using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinTexture : MonoBehaviour {

    [SerializeField] private Texture2D unburned;
    [SerializeField] private Texture2D burned;
    [SerializeField] private int radius;

    private Renderer rend;
    private Texture2D mix;
    private Camera cam;

    // Counter for the amount of pixels that are changed
    private int pixelCounter;

    // Use this for initialization
    void Start ()
    {
        rend = GetComponent<Renderer>();
        cam = Camera.main;
        // Make a copy of the given unburned texture and use it
        mix = Instantiate(unburned) as Texture2D;
        rend.material.mainTexture = mix;

        SetPixels(new Vector2(512 / mix.width, 512 / mix.height));
    }

    public void SetPixels(Vector2 textureCoord)
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
                Color col = burned.GetPixel(x, y);

                if(mix.GetPixel(x,y) != col)
                {
                    mix.SetPixel(x, y, col);
                    pixelCounter++;
                }  
            }
        }
        float newTbsa = GetTBSA();
        mix.Apply(true);

        // actually apply all SetPixels, don't recalculate mip levels
        
    }

    public float GetTBSA()
    {
        float totalPixels = mix.width * mix.height;
        float tbsa = (pixelCounter / totalPixels) * 100;
        Debug.Log(tbsa+ "%");
        
        return tbsa;
    }
}
