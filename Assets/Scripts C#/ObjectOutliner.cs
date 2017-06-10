using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Renderer))]
public class ObjectOutliner : MonoBehaviour
{
    public GameObject player;
    public Shader shader1;
    public Shader shader2;
    public float outlineSize = 0.01f;
    public float distanceToAct = 2;
    public Color outlineColor = Color.black;
    private bool alreadyNear = false;

    bool pingPong = false;
    Renderer rend;
    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();
        shader1 = rend.material.shader;
        shader2 = Shader.Find("TSF/Base1");
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(gameObject.transform.position, player.transform.position);
        if (distance <= distanceToAct)
        {
            if (!alreadyNear)
            {
                alreadyNear = true;
                rend.material.shader = shader2;
                rend.material.SetFloat("_Outline", outlineSize);
                rend.material.SetColor("_OutlineColor", outlineColor);
            }
        }
        else
        {
            alreadyNear = false;
            rend.material.shader = shader1;
        }
    }
}