using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorObject : MonoBehaviour
{
    //Game Design Variables
    [SerializeField] public int    colorIndex;

    // Game Programming Variables
    Renderer m_Renderer;
    private bool m_Bool;

    // Start is called before the first frame update
    void Start()
    {
        m_Renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (SingletonPlayerColor.instance.GetPlayerColor() == colorIndex)
            GetComponent<BoxCollider>().isTrigger = true;
        else
            GetComponent<BoxCollider>().isTrigger = false;

        m_Renderer.material.color = SingletonPlayerColor.instance.SelectableColors[colorIndex];
    }

   
}
