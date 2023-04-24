using NaughtyAttributes.Test;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class BackgroundHide : MonoBehaviour
{
	//Game Design Variables
	[Header("Game Design Variables")]
	[SerializeField]	public	bool		shouldAllowHiding	= true;
	[SerializeField]	public int			colorIndex;


	// Game Programming Variables
	[Header("Game Programming Variables")]
	[SerializeField]		private bool			ShowGPVariables		= false;
	[IGP,SerializeField]	private	Renderer		m_Renderer;
	[IGP,SerializeField]	private bool			m_IsPlayerColliding = false;
	[IGP,SerializeField]	private Transform		m_Player;

	// Start is called before the first frame update
	void Start()
	{
		if (ShowGPVariables) { } //Avoid a warning.
		m_Renderer = GetComponent<Renderer>();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		//Changes material color to the corresponding color in the Universal Color Array.
		m_Renderer.material.color = SingletonPlayerColor.instance.SelectableColors[colorIndex];
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
			m_IsPlayerColliding = true;
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
			m_IsPlayerColliding = false;
	}


	private const string a = "Time it takes before the platform changes color.";
	private const string b = "Place here the Empties that are positionned to the path you wish.";
	private const string c = "Speed of the platform";
}
