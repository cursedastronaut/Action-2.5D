using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator m_Animator;
    private PlayerMovement m_PlayerMovement;
    [SerializeField] private GameObject m_PlayerGameObject;

    private Quaternion m_InitialRotation;
    private Quaternion m_OldRotation;
    public Quaternion m_LastRotation;
    private Vector3 m_InitialPosition;

    [SerializeField] private ParticleSystem m_PlayerParticleSystem;


    private void Awake()
    {
        m_Animator = GetComponentInChildren<Animator>();
        m_PlayerMovement = GetComponent<PlayerMovement>();
        m_PlayerGameObject.transform.Rotate(new Vector3(0, -90, 0));
        if (m_Animator != null)
        {
            //Debug.Log("Null");
        }
        m_InitialRotation = m_PlayerGameObject.transform.rotation;
        m_InitialPosition = m_PlayerGameObject.transform.localPosition;

    }

    private void Update()
    {
        m_Animator.SetBool("IsMoving", m_PlayerMovement.m_isMoving);
        m_Animator.SetBool("IsSprinting", m_PlayerMovement.m_isSprinting);
        if (m_PlayerMovement.IsThereFloor() == true && GetComponent<Rigidbody>().velocity.y < -0.1f)
        {
            m_Animator.SetBool("Jump", true);
            StartCoroutine(ParticuleCoroutine());
            StartCoroutine(JumpCoroutine());
        }
        else 
        {
            m_Animator.SetBool("Jump", false);
        }

        if (GetComponent<PlayerDeath>().m_isDead)
        {
            StartCoroutine(DeathCoroutine());
            m_Animator.SetBool("Dead", GetComponent<PlayerDeath>().m_isDead);
            
        }
        else
        {
            m_Animator.SetBool("Dead", false) ;
        }
        m_OldRotation = m_PlayerGameObject.transform.rotation;
        SetRotationAnimator();

    }

    private IEnumerator DeathCoroutine()
    {
        //Debug.Log("Sympa");
        yield return new WaitForSeconds(3);

        m_PlayerGameObject.transform.rotation = m_InitialRotation;
        
    }

    private IEnumerator JumpCoroutine()
    {
        //Debug.Log("Saut");
        yield return new WaitForSeconds(1);
        m_PlayerGameObject.transform.localPosition = m_InitialPosition;
    }

    private IEnumerator WalkCoroutine()
    {
        yield return new WaitForSeconds(1);
        m_PlayerGameObject.transform.rotation = m_OldRotation * m_LastRotation;
    }

    private IEnumerator ParticuleCoroutine()
    {
        m_PlayerParticleSystem.Play();
        yield return new WaitForSeconds(0.5f);
        m_PlayerParticleSystem.Stop(true);
    }

    private void SetRotationAnimator()
    {
        Vector3 velocity = GetComponent<Rigidbody>().velocity;
        float sign = Mathf.Sign(velocity.x); // get the sign of the x-velocity

        Quaternion newRotation;
        if (sign > 0)
        { // if moving to the right
            if (m_LastRotation == Quaternion.Euler(0, 90, 0))
                return;
            else if (m_LastRotation == Quaternion.Euler(0, -90, 0))
            {
                newRotation = m_OldRotation * Quaternion.Euler(0, 180, 0);
            }
            else
                newRotation = m_OldRotation * Quaternion.Euler(0, 90, 0); // rotate 90 degrees to the right

            m_LastRotation = Quaternion.Euler(0, 90, 0);

            m_PlayerGameObject.transform.rotation = newRotation;
        }
        else if (sign < 0)
        { // if moving to the left
            if (m_LastRotation == Quaternion.Euler(0, -90, 0))
                return;
            else if (m_LastRotation == Quaternion.Euler(0, 90, 0))
                newRotation = m_OldRotation * Quaternion.Euler(0, -180, 0);
            else
                newRotation = m_OldRotation * Quaternion.Euler(0, -90, 0); // rotate 90 degrees to the right

            m_LastRotation = Quaternion.Euler(0, -90, 0);
            m_PlayerGameObject.transform.rotation = newRotation;
        }
        else
            return;
    }
}
