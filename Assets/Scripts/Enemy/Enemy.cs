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
    [SerializeField]				private Transform[] m_Path;
	[SerializeField]				private float m_Speed;
    [SerializeField]				private bool m_TurnOff;
    [SerializeField]				protected float m_TurnOffSpeed = 5;
	[SerializeField][Tooltip(a)]	public int EnemyKind;
	[SerializeField]				private	AnimationCurve	FloatingAnimationCurve;
	[SerializeField]				private float			FloatingAnimationForce;
	[SerializeField]				private float			FloatingAnimationSpeed;
	[SerializeField]				private Transform		FloatingEnemyModel;





	//Game Programming Variables
	[Header("Game Programming Variables")]
    [SerializeField]		private bool		ShowGPVariables		= false;
    [SerializeField]		private Transform	m_DetectionBox;
    [SerializeField]		private GameObject	m_Light;
	[IGP][SerializeField]	private int			m_CurrentPath;
    [IGP][SerializeField]	private bool		m_IsOff;
    [IGP][SerializeField]	private float		m_TurnOffTime;


    private void Awake()
    {
        if (ShowGPVariables) { } //Avoid a warning.
								 // Get the game object that you want to disable
	}



    // Update is called once per frame
    void Update()
    {
        playerDetection();
		Movement();


        if (!m_TurnOff)
            return;

        m_TurnOffTime += Time.deltaTime;
        if (m_TurnOffTime < m_TurnOffSpeed)
            return;

        m_IsOff = !m_IsOff;
        m_TurnOffTime = 0;


        

        // Check if the game object is not null and m_IsOff is true
        if (m_DetectionBox != null && m_IsOff)
        {
            // Disable the Renderer component of the game object
            Renderer renderer = m_DetectionBox.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.enabled = false;
				m_Light.SetActive(false);
            }
        }
        else if (m_DetectionBox != null)
        {
            Renderer renderer = m_DetectionBox.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.enabled = true;
                m_Light.SetActive(true);
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
		Animation();


		if (Vector3.Distance(current, target) < 0.5f)
			m_CurrentPath = (m_CurrentPath + 1) % m_Path.Length;
	}

	private void Animation()
	{
		if (EnemyKind == 0 || EnemyKind == 1 || EnemyKind == 3)
			transform.LookAt(m_Path[m_CurrentPath]);
		if (EnemyKind == 0)
			FloatingEnemyModel.position = transform.position + new Vector3(0, FloatingAnimationCurve.Evaluate(Time.time * FloatingAnimationSpeed) , 0) * FloatingAnimationForce;
	}


	private const string a = "0: Floating Enemy" +
							"\n1: Security Enemy" +
							"\n2: Bottom Enemy" +
							"\n3: Background Enemy";
}
