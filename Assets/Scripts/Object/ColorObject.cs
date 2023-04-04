using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorObject : MonoBehaviour
{
    [SerializeField] public int    colorIndex;
    [SerializeField] private Color  color;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (SingletonPlayerColor.instance.GetPlayerColor() == colorIndex)
            GetComponent<BoxCollider>().enabled = false;
        else
            GetComponent<BoxCollider>().enabled = true;
    }

    
}
