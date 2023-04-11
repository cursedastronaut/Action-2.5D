using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
	
	//Game Design Variables
	[SerializeField][Tooltip(a)]	private GameObject	m_PortalMate;
	[SerializeField][Tooltip(b)]	private int			colorIndex;
	// Game Programming Variables
	[HideInInspector]	Renderer m_Renderer;

	// Start is called before the first frame update
	void Start()
	{
		m_Renderer = GetComponent<Renderer>();
	}

	// Update is called once per frame
	void Update()
	{
		m_Renderer.material.color = SingletonPlayerColor.instance.SelectableColors[colorIndex];
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") && SingletonPlayerColor.instance.GetPlayerColor() == colorIndex && !SingletonPlayerColor.instance.isBeingTeleported)
		{
			other.transform.position = m_PortalMate.transform.position;
			SingletonPlayerColor.instance.isBeingTeleported = this.gameObject;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player") && SingletonPlayerColor.instance.isBeingTeleported == m_PortalMate)
			SingletonPlayerColor.instance.isBeingTeleported = null;


	}




	private const string a	= "Drag here the other portal it will go out of.";
	private const string b	= "Color of the portal. Put the number of the color. (See SingletonPlayerColor in Player)";
}
