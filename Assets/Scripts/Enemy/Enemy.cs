using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    //Game Design Variables
    [SerializeField] private Transform[] m_Path;

    //Game Programming Variables
    [SerializeField] private Transform m_DetectionBox;
    private float           initY               = 0;
    private bool            isCalculatingPath   = false;
    private Transform       m_Target;
    private int             m_CurrentPath;
    private NavMeshAgent    m_Agent;


    private void Awake()
    {
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
            if (obj.CompareTag("Player"))
                Debug.Log("Player detected");
        }
    }
}
