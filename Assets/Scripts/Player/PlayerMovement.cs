using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    //Game Design Variables
    [SerializeField] private float DefaultSpeed;
    [SerializeField] private float DefaultSprintSpeed;
    [SerializeField] private float DefaultJumpForce;

    //Game Programming Variables
    private Vector2     m_MovementInput = Vector2.zero;
    private bool        m_isSprinting   = false;
    private bool        m_isJumping     = false;
    [SerializeField]
    private Rigidbody m_Rigidbody;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //Movement depending of input
        float currentSpeed = m_isSprinting == true ? DefaultSprintSpeed : DefaultSpeed;
        transform.position += (transform.right * m_MovementInput.x * currentSpeed) * Time.deltaTime;

        //Jump
        if (m_isJumping && isThereFloor())
            m_Rigidbody.AddForce(0, DefaultJumpForce, 0);
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

    public void Move(InputAction.CallbackContext context)
    {
        m_MovementInput = context.ReadValue<Vector2>();
    }
    public void Sprint(InputAction.CallbackContext context)
    {
        m_isSprinting = context.ReadValueAsButton();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        m_isJumping = context.ReadValueAsButton();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Collision");
            
        }

    }

}
