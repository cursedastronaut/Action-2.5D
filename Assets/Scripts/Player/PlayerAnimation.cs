using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator m_Animator;
    private PlayerMovement m_PlayerMovement;
    private void Awake()
    {
        m_Animator = GetComponentInChildren<Animator>();
        m_PlayerMovement = GetComponent<PlayerMovement>();
        if (m_Animator != null )
        {
            Debug.Log("Null");
        }

    }

    private void Update()
    {
        if (m_PlayerMovement.m_isMoving == true)
        {
            Debug.Log(m_PlayerMovement.m_isMoving == true);
            m_Animator.SetBool("IsMoving", true);
        }
        else
            m_Animator.SetBool("IsMoving", false);

        if (m_PlayerMovement.m_isSprinting != false) 
        { 
            m_Animator.SetBool("IsSprinting", true) ;
        }
        else 
            m_Animator.SetBool("IsSprinting", false);

        if (m_PlayerMovement.IsThereFloor() != false && GetComponent<Rigidbody>().velocity.y > 0)
        {
            m_Animator.SetBool("Jump", true);
        }
        else 
        {
            m_Animator.SetBool("Jump", false);
        }

        if (GetComponent<PlayerDeath>().m_isDead)
        {
            m_Animator.SetBool("Dead", true);
            
            StartCoroutine(DeathCoroutine());
                
            
        }
    }

    private IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(10);

        //
        transform.rotation = new Quaternion(0,0,0,0); 
        GetComponent<PlayerDeath>().m_isDead = false;
    }
}
