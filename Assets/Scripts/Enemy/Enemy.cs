using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private Transform m_Target;
    [SerializeField] private Transform[] m_Path;
    private int m_CurrentPath;
    private NavMeshAgent m_Agent;
    private SphereCollider m_RangeSphere;


    private void Awake()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_RangeSphere = GetComponent<SphereCollider>();

    }



    // Update is called once per frame
    void Update()
    {
        if (m_Target)
        {
            m_Agent.destination = new Vector3(m_Target.position.x, transform.position.y, m_Target.position.x) ;
        }

        if (!m_Agent.hasPath)
        {
            if (m_Path.Length > 0)
            {
                m_CurrentPath %= m_Path.Length;
                m_Agent.destination = new Vector3(m_Path[m_CurrentPath++].position.x, transform.position.y, m_Path[m_CurrentPath].position.z) ;
            }

            else
            {
                Vector3 random = Random.insideUnitSphere;
                m_Agent.destination = transform.position + new Vector3(random.x, transform.position.y, random.z) * m_RangeSphere.radius;
            }
        }
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
