using NaughtyAttributes.Test;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Platform : MonoBehaviour
{
	//Game Design Variables
	[Header("Game Design Variables")]
	[SerializeField]	private bool		shouldMove			= false;
	[SerializeField]	private bool		shouldSwitchColors	= false;
	[SerializeField]	public	bool		shouldAllowHiding	= true;
	[SerializeField]	public int[]		colorIndex;
	[SerializeField,SWC,Tooltip(a)]	public float		timeBetweenColorSwitch;
	[SerializeField,SM,Tooltip(b)]	private	Transform[]	m_Path;
	[SerializeField,SM,Tooltip(c)]	private	float		m_Speed;


	// Game Programming Variables
	[Header("Game Programming Variables")]
	[SerializeField]		private bool			ShowGPVariables		= false;
	[IGP,SerializeField]	private	Renderer		m_Renderer;
	[IGP,SerializeField]	private int				m_Index				= 0;
	[IGP,SerializeField]	private float			m_Timer				= 0;
	[IGP,SerializeField]	private int				m_CurrentPath		= 0;
	[IGP,SerializeField]	private int				m_NextPath			= 0;
	[IGP,SerializeField]	private int				m_CurrentStep		= 0;
	[IGP,SerializeField]	private float			m_StepTimer			= 0;
	[IGP,SerializeField]	private bool			m_IsPlayerColliding = false;
	[IGP,SerializeField]	private Transform		m_Player;

	//Called elsewhere variables
	[IGP]	public int currentColor = 0;

	// Start is called before the first frame update
	void Start()
	{
		if (ShowGPVariables) { } //Avoid a warning.
		m_Renderer = GetComponent<Renderer>();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (SingletonPlayerColor.instance.GetPlayerColor() == colorIndex[m_Index])
			GetComponent<BoxCollider>().isTrigger = true;
		else
			GetComponent<BoxCollider>().isTrigger = false;


		if (shouldSwitchColors) SwitchColor();
		if (shouldMove)			Movement();

		//Changes material color to the corresponding color in the Universal Color Array.
		m_Renderer.material.color = SingletonPlayerColor.instance.SelectableColors[colorIndex[m_Index]];
		currentColor = colorIndex[m_Index];
	}
	private void SwitchColor()
	{
		if (m_Timer >= timeBetweenColorSwitch)
		{
			m_Index++;
			if (m_Index >= colorIndex.Length)
				m_Index = 0;
			m_Timer = 0;
		}
		m_Timer += Time.deltaTime;
	}

	private void Movement()
	{

		Vector3 target = m_Path[m_CurrentPath].position;
		Vector3 current = transform.position;
		Vector3 previous = transform.position;
		transform.position = Vector3.MoveTowards(current, target, m_Speed * Time.deltaTime);
		//movePlayerWithPlatform(previous);

		if (Vector3.Distance(current, target) < 0.5f)
			m_CurrentPath = (m_CurrentPath + 1) % m_Path.Length;
		/*
				//If the platform arrived to its desired destination
				if (m_CurrentStep >= numberOfSteps)
				{
					m_CurrentPath++; m_CurrentStep= 0;
					if (m_Path.Length <= m_CurrentPath)
						m_CurrentPath = 0;
				}

				if (m_NextPath == m_CurrentPath)
				{
					m_NextPath = m_CurrentPath+1 >= m_Path.Length ? 0 : m_CurrentPath+1;
				}

				if (m_StepTimer >= TimeBetweenSteps)
				{
					m_CurrentStep++;
					m_StepTimer = 0;
					transform.position += ((m_Path[m_NextPath].position - m_Path[m_CurrentPath].position) / numberOfSteps) ;
					movePlayerWithPlatform();
				}
				m_StepTimer += Time.fixedDeltaTime;*/

	}
	private void movePlayerWithPlatform(Vector3 previous)
	{
		if (m_IsPlayerColliding)
		{
			m_Player.position = (m_Player.position - 
			previous)
			+ transform.position;
		}
	}


	private void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.CompareTag("Player"))
		{
			m_IsPlayerColliding = true;
			m_Player = collision.collider.transform;
		}
	}
	private void OnCollisionExit(Collision collision)
	{
		if (collision.collider.CompareTag("Player"))
			m_IsPlayerColliding = false;
	}


	private const string a = "Time it takes before the platform changes color.";
	private const string b = "Place here the Empties that are positionned to the path you wish.";
	private const string c = "Speed of the platform";
}
