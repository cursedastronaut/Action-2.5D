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
	[Header("Game Design Variables")]
	[SerializeField, Tooltip(h)]	private float DefaultSpeed;
	[SerializeField][Tooltip(a)]	private float DefaultSprintSpeed;
	[SerializeField][Tooltip(b)]	private float DefaultJumpForce;
	[SerializeField][Tooltip(c)]	private Vector2 DefaultWallJumpForce;
	[SerializeField][Tooltip(d)]	private float DefaultSprintDelay;
	[SerializeField][Tooltip(e)]	private float DefaultSprintOffsetDelay;
	[SerializeField][Tooltip(f)]	private float DeadZone;
	[SerializeField][Tooltip(g)]	private float WallSlidingSpeed;
<<<<<<< HEAD
=======
	[SerializeField]				private float maxVelocity;
	[SerializeField]				private float CoyoteTime;
>>>>>>> 74b7a24e64fd73ed48aec2e9cb81af62f05aa7a5

    //Game Programming Variables
    [Header("Game Programming Variables")]
	[SerializeField] private bool ShowGPVariables = false;
	[IGP][SerializeField] private Vector2 m_MovementInput = Vector2.zero;
	[IGP][SerializeField] private bool m_isSprinting = false;
	[IGP][SerializeField] private bool m_canSprint = false;
	[IGP][SerializeField] private bool m_isJumping = false;
<<<<<<< HEAD
	[IGP][SerializeField] private float m_SprintDelay = 0.0f;
=======
	[IGP]
	[SerializeField]	private bool m_isWallJumping = false;
	[IGP][SerializeField]private float m_SprintDelay = 0.0f;
	[IGP][SerializeField] private float CoyoteTimeCD;
>>>>>>> 74b7a24e64fd73ed48aec2e9cb81af62f05aa7a5
	[IGP][SerializeField] private PlayerColor m_PlayerColor;
	

	[SerializeField]
	private Rigidbody m_Rigidbody;

	// Start is called before the first frame update
	void Start()
	{
		if (ShowGPVariables) { }
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
<<<<<<< HEAD
		transform.position += (transform.right * m_MovementInput.x * currentSpeed) * Time.deltaTime;
=======
		if (m_isWallJumping)
			m_Rigidbody.AddForce((transform.right * m_MovementInput.x * currentSpeed) * Time.deltaTime , ForceMode.Acceleration);
		else
            m_Rigidbody.AddForce((transform.right * m_MovementInput.x * currentSpeed) * Time.deltaTime, ForceMode.VelocityChange);
        ax = Mathf.Clamp(m_Rigidbody.velocity.x, -maxVelocity, maxVelocity);
		m_Rigidbody.velocity = new Vector3 (ax, m_Rigidbody.velocity.y, 0);
		if (isThereFloor())
			m_isWallJumping = false;
>>>>>>> 74b7a24e64fd73ed48aec2e9cb81af62f05aa7a5

		//Jump
		if (m_isJumping && isThereFloor())
			m_Rigidbody.AddForce(0, DefaultJumpForce * Time.deltaTime, 0, ForceMode.VelocityChange);
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
		for (int i = -1; i <= 1; i += 2)
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
<<<<<<< HEAD
		if (Physics.Raycast(transform.position, Vector3.left, out RaycastHit hit, 0.6f))
			m_Rigidbody.velocity = new Vector3(transform.right.x * DefaultWallJumpForce, DefaultWallJumpForce, 0);
		else
			m_Rigidbody.velocity = new Vector3(-transform.right.x * DefaultWallJumpForce, DefaultWallJumpForce, 0);
=======
        Vector3 wallNormal = Vector3.zero;
        if (Physics.Raycast(transform.position, Vector3.left, out RaycastHit leftHit, 0.6f))
            wallNormal = leftHit.normal;
        else if (Physics.Raycast(transform.position, Vector3.right, out RaycastHit rightHit, 0.6f))
            wallNormal = rightHit.normal;

        float direction = Mathf.Sign(Vector3.Dot(wallNormal, transform.up));
        m_Rigidbody.velocity = new Vector3(direction * DefaultWallJumpForce.x * wallNormal.x * Time.deltaTime, DefaultWallJumpForce.y * Time.deltaTime /2, 0);


    }

	private bool Coyote()
	{
		if (isThereWall() || isThereFloor())
		{

		}

		return true;
>>>>>>> 74b7a24e64fd73ed48aec2e9cb81af62f05aa7a5
	}
	public void Move(InputAction.CallbackContext context)
	{
		m_MovementInput = context.ReadValue<Vector2>();
	}

	public void Jump(InputAction.CallbackContext context)
	{
		m_isJumping = context.ReadValueAsButton();
	}

	private const string a = "The Speed when the Player Sprints";
	private const string b = "The Force applied to the Player when they jump";
	private const string c = "The Force applied to the Player when they WallJump";
	private const string d = "The time before the Player has to fully push his joystick to one side to sprint";
	private const string e = "The time it takes for the player to sprint";
	private const string f = "The Zone in which player can move but sprint function won't apply";
	private const string g = "The Speed at which the Player shall slide down a wall.";
	private const string h = "The Speed when the Player Walks";
}
