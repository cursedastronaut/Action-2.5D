using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    private Vector3 m_Checkpoint;
    
    // Start is called before the first frame update
    void Start()
    {
        m_Checkpoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void killPlayer()
    {
        Debug.Log(m_Checkpoint);
        transform.position = m_Checkpoint;
    }

    public void editCheckpoint(Vector3 input)
    {
        m_Checkpoint = input;
    }
}
