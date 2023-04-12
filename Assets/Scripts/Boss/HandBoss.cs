using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandBoss : MonoBehaviour
{
	//UNCOMMENT
	//private byte left = 0; private byte right = 1;

	[Header("Game Programming Variables")]
	[SerializeField]		private bool ShowGPVariables = false;
	[IGP, SerializeField]	private GameObject[] Hands;
    // Start is called before the first frame update
    void Start()
    {
        if (ShowGPVariables) { } //Avoid a warning.
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
