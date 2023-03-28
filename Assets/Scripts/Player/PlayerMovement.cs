using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //Game Design Variables
    [SerializeField] private float DefaultSpeed;
    [SerializeField] private float DefaultSprintSpeed;

    //Game Programming Variables
    private Vector2 m_MovementInput = Vector2.zero;
    private bool    m_isSprinting     = false;

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
    }

    public void Move(InputAction.CallbackContext context)
    {
        m_MovementInput = context.ReadValue<Vector2>();
    }
    public void Sprint(InputAction.CallbackContext context)
    {
        m_isSprinting = context.ReadValueAsButton();
    }
}
