using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    //Game Design Variables
    [Header("Game Design Variables")]
    [SerializeField] private Transform[] m_Path;
	[SerializeField] private float m_Speed;
    [SerializeField] private bool m_TurnOff;
    [SerializeField] protected float m_TurnOffSpeed = 5;

    //Game Programming Variables
    [Header("Game Programming Variables")]
    [SerializeField]		private bool		ShowGPVariables		= false;
    [SerializeField]		private Transform	m_DetectionBox;
    [IGP][SerializeField]	private int			m_CurrentPath;
    [IGP][SerializeField]	private bool		m_IsOff;
    [IGP][SerializeField]	private float		m_TurnOffTime;


    private void Awake()
    {
        if (ShowGPVariables) { } //Avoid a warning.
    }



    // Update is called once per frame
    void Update()
    {
        playerDetection();/*
        //If the enemy arrived to its desired destination
        if (!m_Agent.hasPath && !isCalculatingPath)
        {
            m_CurrentPath++;
            if (m_Path.Length <= m_CurrentPath)
            {
                m_CurrentPath = 0;
            }
            m_Agent.SetDestination(new Vector3(m_Path[m_CurrentPath].position.x, initY, m_Path[m_CurrentPath].position.z));
            isCalculatingPath = true;
        }*/
		Movement();


        if (!m_TurnOff)
            return;

        m_TurnOffTime += Time.fixedDeltaTime;
        if (m_TurnOffTime < m_TurnOffSpeed)
            return;

        m_IsOff = !m_IsOff;
        m_TurnOffTime = 0;


        // Get the game object that you want to disable
        GameObject detectionBox = transform.GetChild(1).gameObject;
        Light light = transform.GetChild(2).GetComponent<Light>();

        // Check if the game object is not null and m_IsOff is true
        if (detectionBox != null && m_IsOff)
        {
            // Disable the Renderer component of the game object
            Renderer renderer = detectionBox.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.enabled = false;
                light.enabled = false;
            }
        }
        else if (detectionBox != null)
        {
            Renderer renderer = detectionBox.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.enabled = true;
                light.enabled = true;
            }
        }
    }

    private void playerDetection()
    {
        Collider[] objArray = Physics.OverlapBox(m_DetectionBox.position, m_DetectionBox.localScale);
        if (m_IsOff == false)
            foreach (var obj in objArray)
            {
                if (obj.CompareTag("Player") && !obj.GetComponent<PlayerColor>().isHidden)
                    obj.GetComponent<PlayerDeath>().killPlayer();
            }
    }
	
	private void Movement()
	{
		Vector3 target = m_Path[m_CurrentPath].position;
		Vector3 current = transform.position;

		transform.position = Vector3.MoveTowards(current, target, m_Speed * Time.deltaTime);

		if (Vector3.Distance(current, target) < 0.5f)
			m_CurrentPath = (m_CurrentPath + 1) % m_Path.Length;
	}
}
