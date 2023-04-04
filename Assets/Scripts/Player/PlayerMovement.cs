using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    //Game Design Variables
    [SerializeField] private float DefaultSpeed;
    [SerializeField] private float DefaultSprintSpeed;
    [SerializeField] private float DefaultJumpForce;
    [SerializeField] private float DefaultSprintDelay;
    [SerializeField] private float DefaultSprintOffsetDelay;
    [SerializeField] private float DeadZone;
    [SerializeField] private float DistanceObject;
    [SerializeField] private float WallSlidingSpeed;

    //Game Programming Variables
    private Vector2 m_MovementInput = Vector2.zero;
    private bool m_isSprinting = false;
    private bool m_canSprint = false;
    private bool m_isJumping = false;
    private float m_SprintDelay = 0.0f;
    private bool m_isWallSliding;
    public bool m_isHide = false;
    [SerializeField]
    private Rigidbody m_Rigidbody;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        bool shouldSprint = m_SprintDelay < DefaultSprintDelay;
        if ((m_MovementInput.x > 0.9f && shouldSprint) || (m_MovementInput.x < -0.9f && shouldSprint))
            m_canSprint = true;
        if (m_MovementInput.x > DeadZone || m_MovementInput.x < -DeadZone)
            m_SprintDelay += Time.deltaTime;
        if (m_MovementInput.x < DeadZone && m_MovementInput.x > -DeadZone)
        { m_SprintDelay = 0; m_isSprinting = false; }
        if (m_canSprint && m_SprintDelay >= DefaultSprintDelay + DefaultSprintOffsetDelay)
            m_isSprinting = true;

        //Movement depending of input
        float currentSpeed = m_isSprinting == true ? DefaultSprintSpeed : DefaultSpeed;
        transform.position += (transform.right * m_MovementInput.x * currentSpeed) * Time.deltaTime;

        //Jump
        if (m_isJumping && isThereFloor() || m_isJumping && isThereWall() )
            m_Rigidbody.AddForce(0, DefaultJumpForce, 0, ForceMode.VelocityChange);
    }

    //Checks if there is floor under the player.
    private bool isThereFloor()
    {
        //TODO: Make it work
        foreach (var obj in Physics.OverlapSphere(transform.position - new Vector3(0, 1.2f, 0), 0.1f))
        {
            return true;
        }
        return false;
    }

    private bool isThereWall()
    {
        for (int i = -1; i <= 1; i+=2)
        { 
            if (Physics.Raycast(transform.position, Vector3.right * i, out RaycastHit hit, 0.9f))
            {
                if (hit.collider.gameObject.CompareTag("Obstacle"))
                {
                    return true;
            
                }
            }
        }
        return false;
    }

    public void Move(InputAction.CallbackContext context)
    {
        m_MovementInput = context.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        m_isJumping = context.ReadValueAsButton();
    }

    

}
