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
    [SerializeField] private float DefaultWallJumpForce;
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
    private PlayerColor m_PlayerColor;
    
    [SerializeField]
    private Rigidbody m_Rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        m_PlayerColor = GetComponent<PlayerColor>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_PlayerColor.isHidden) return;

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
        if (m_isJumping && isThereFloor() )
            m_Rigidbody.AddForce(0, DefaultJumpForce, 0, ForceMode.VelocityChange);
        else if (isThereWall())
        {
            WallSliding();
            if (m_isJumping)
            {
                WallJump();
            }
        }

        
    }

    //Checks if there is floor under the player.
    private bool isThereFloor()
    {
        //TODO: Make it work
        foreach (var obj in Physics.OverlapSphere(transform.position - new Vector3(0, 1.2f, 0), 0.1f))
        {
            if (!obj.isTrigger)
                return true;
        }
        return false;
    }

    private bool isThereWall()
    {
        for (int i = -1; i <= 1; i+=2)
        { 
            if (Physics.Raycast(transform.position, Vector3.right * i, out RaycastHit hit, 0.6f))
                if (hit.collider.gameObject.CompareTag("Object"))
                { 
                        UnityEngine.Debug.Log("Wall touched");
                        return true;
                }
        }
        return false;
    }

    private void WallSliding()
    {
        Vector3 PlayerVelocity = m_Rigidbody.velocity;

        PlayerVelocity.y = Mathf.Clamp(PlayerVelocity.y, -WallSlidingSpeed, float.MaxValue);
        m_Rigidbody.velocity = PlayerVelocity;
    }

    private void WallJump()
    {
        if (Physics.Raycast(transform.position, Vector3.left, out RaycastHit hit, 0.6f))
            m_Rigidbody.velocity = new Vector3(transform.right.x * DefaultWallJumpForce , DefaultWallJumpForce, 0);
        else
            m_Rigidbody.velocity = new Vector3(-transform.right.x * DefaultWallJumpForce, DefaultWallJumpForce, 0);
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
