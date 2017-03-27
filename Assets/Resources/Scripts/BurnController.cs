using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Place & combine burn meshes
// 
public class BurnController : MonoBehaviour {

    [SerializeField] private GameObject burnWoundPrefab;

    private List<GameObject> burnWounds = new List<GameObject>();

    public void PlaceBurn(Vector3 pos)
    {
        // set burn degree
        GameObject newBurn = Instantiate(burnWoundPrefab, pos, Quaternion.identity) as GameObject;
        burnWounds.Add(newBurn);
        // combine mesh?
    }
}
