using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator m_Animator;
    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (m_Animator != null)
        {
            
        }

        if (transform.GetComponent<PlayerMovement>().m_isMoving)
        {
            m_Animator.SetBool("IsMoving", true);
        }
        else
            m_Animator.SetBool("IsMoving", false);
    }
}
