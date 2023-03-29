using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    //Game Design Variables
    [SerializeField] private Transform[] m_Path;

    //Game Programming Variables
    private float initY = 0;
    private bool isCalculatingPath = false;
    private Transform m_Target;
    private int m_CurrentPath;
    private NavMeshAgent m_Agent;
    private SphereCollider m_RangeSphere;


    private void Awake()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_RangeSphere = GetComponent<SphereCollider>();
        initY = transform.position.y;
    }



    // Update is called once per frame
    void Update()
    {
        if (m_Target)
        {
            m_Agent.destination = new Vector3(m_Target.position.x, initY, m_Target.position.x) ;
        }

        if (!m_Agent.hasPath && !isCalculatingPath)
        {
            Debug.Log("l"+ (m_Path.Length-1));
            Debug.Log("c"+ m_CurrentPath);
            if (m_Path.Length-1 <= m_CurrentPath)
            {
                m_CurrentPath = 0;
            }
            m_Agent.SetDestination( new Vector3(m_Path[m_CurrentPath++].position.x, initY, m_Path[m_CurrentPath].position.z));
            isCalculatingPath = true;
        }
        if (m_Agent.hasPath && isCalculatingPath)
            isCalculatingPath = false;

        transform.position = new Vector3(transform.position.x, initY, transform.position.z);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            m_Target = other.transform;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            m_Target = null;
        }
    }

}
