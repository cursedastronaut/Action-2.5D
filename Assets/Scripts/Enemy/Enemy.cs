using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
	//Game Design Variables
	[Header("Game Design Variables")]
	[SerializeField]	private Transform[]		m_Path;

	//Game Programming Variables
	[Header("Game Programming Variables")]
	[SerializeField]	private bool			ShowGPVariables = false;
	[SerializeField]	private	Transform		m_DetectionBox;
	[NaughtyAttributes.ShowIf("ShowGPVariables")][SerializeField] 
		private float			initY				= 0;
	[NaughtyAttributes.ShowIf("ShowGPVariables")][SerializeField]
		private bool			isCalculatingPath   = false;
	[NaughtyAttributes.ShowIf("ShowGPVariables")][SerializeField]
		private Transform		m_Target;
	[NaughtyAttributes.ShowIf("ShowGPVariables")][SerializeField]
		private int				m_CurrentPath;
	[NaughtyAttributes.ShowIf("ShowGPVariables")][SerializeField]
		private NavMeshAgent	m_Agent;


	private void Awake()
	{
		if (ShowGPVariables) { } //Avoid a warning.
		m_Agent = GetComponent<NavMeshAgent>();
		initY = transform.position.y;
	}



	// Update is called once per frame
	void Update()
	{
		playerDetection();
		//If the enemy arrived to its desired destination
		if (!m_Agent.hasPath && !isCalculatingPath)
		{
			m_CurrentPath++;
			if (m_Path.Length <= m_CurrentPath)
			{
				m_CurrentPath = 0;
			}
			m_Agent.SetDestination( new Vector3(m_Path[m_CurrentPath].position.x, initY, m_Path[m_CurrentPath].position.z));
			isCalculatingPath = true;
		}
		//In case path calculation takes more than one frame, we use this variable
		if (m_Agent.hasPath && isCalculatingPath)
			isCalculatingPath = false;

		transform.position = new Vector3(transform.position.x, initY, transform.position.z);
	}
	
	private void playerDetection()
	{
		Collider[] objArray = Physics.OverlapBox(m_DetectionBox.position, m_DetectionBox.localScale);
		foreach (var obj in objArray)
		{
			if (obj.CompareTag("Player") && !obj.GetComponent<PlayerColor>().isHidden)
				obj.GetComponent<PlayerDeath>().killPlayer();
		}
	}
}
