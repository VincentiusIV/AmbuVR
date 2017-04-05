using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterEggController : MonoBehaviour {

    public GameObject go;
    public int amountOfKidsX;
    public int amountOfKidsY;
    public int distance;

	public void ItsJustAPrank()
    {
        for (int x = 0; x < amountOfKidsX; x++)
        {
            for (int y = 0; y < amountOfKidsY; y++)
            {
                Vector3 position = transform.position + new Vector3(distance * x, 0f, distance * y);
                Instantiate(go, position, Quaternion.identity);
            }
        }
    }
}
