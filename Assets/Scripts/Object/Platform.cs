using NaughtyAttributes.Test;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;

public class Platform : MonoBehaviour
{
	//Game Design Variables
	[SerializeField]	private bool		shouldMove			= false;
	[SerializeField]	private bool		shouldSwitchColors	= false;
	[SerializeField]	public int[]		colorIndex;
	[NaughtyAttributes.ShowIf("shouldSwitchColors")]
	[SerializeField]	public float		timeBetweenColorSwitch;
	[NaughtyAttributes.ShowIf("shouldMove")]
	[SerializeField]	private	Transform[]	m_Path;
	[NaughtyAttributes.ShowIf("shouldMove")]
	[SerializeField]	private	int			numberOfSteps;
	[NaughtyAttributes.ShowIf("shouldMove")]
	[SerializeField]	private	float		TimeBetweenSteps;


	// Game Programming Variables
	private	Renderer		m_Renderer;
	private int				m_Index				= 0;
	private float			m_Timer				= 0;
	private int				m_CurrentPath		= 0;
	private int				m_NextPath			= 0;
	private int				m_CurrentStep		= 0;
	private float			m_StepTimer			= 0;
	//Called eslewhere variables
	[HideInInspector] public int currentColor = 0;

	// Start is called before the first frame update
	void Start()
	{
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
		currentColor = m_Index;
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
			transform.position += ((m_Path[m_NextPath].position -
			m_Path[m_CurrentPath].position) / numberOfSteps) ;
		}
		m_StepTimer += Time.fixedDeltaTime;
		

		
	}
}
