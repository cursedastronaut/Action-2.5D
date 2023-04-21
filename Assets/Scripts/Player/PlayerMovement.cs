using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;

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
	[SerializeField]				private float maxVelocity;

    //Game Programming Variables
    [Header("Game Programming Variables")]
	[SerializeField] private bool ShowGPVariables = false;
	[IGP][SerializeField] private Vector2 m_MovementInput = Vector2.zero;
	[IGP][SerializeField] private bool m_isSprinting = false;
	[IGP][SerializeField] private bool m_canSprint = false;
	[IGP][SerializeField] private bool m_isJumping = false;
    [IGP][SerializeField] private bool m_isMoving = false;
    [IGP]
	[SerializeField]	private bool m_isWallJumping = false;
	[IGP][SerializeField]private float m_SprintDelay = 0.0f;
	[IGP][SerializeField] private float CoyoteTimeCD;
	[IGP][SerializeField] private PlayerColor m_PlayerColor;
	

	[SerializeField]
	private Rigidbody m_Rigidbody;

	private float ax;
	private Vector3 m_Velocity;

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
		{ m_SprintDelay = 0; m_isSprinting = false; m_isMoving = false; }
		if (m_canSprint && m_SprintDelay >= DefaultSprintDelay + DefaultSprintOffsetDelay)
			m_isSprinting = true;

		//Movement depending of input
		float currentSpeed = m_isSprinting == true ? DefaultSprintSpeed : DefaultSpeed;
		if (m_isWallJumping)
			m_Rigidbody.AddForce((transform.right * m_MovementInput.x * currentSpeed) * Time.deltaTime, ForceMode.Acceleration);
		else
            m_Rigidbody.AddForce((transform.right * m_MovementInput.x * currentSpeed) * Time.deltaTime, ForceMode.VelocityChange);
        ax = Mathf.Clamp(m_Rigidbody.velocity.x, -maxVelocity, maxVelocity);
		m_Rigidbody.velocity = new Vector3 (ax, m_Rigidbody.velocity.y, 0);
		if (IsThereFloor())
			m_isWallJumping = false;

		//Jump
		if (m_isJumping && IsThereFloor())
			m_Rigidbody.AddForce(0, DefaultJumpForce * Time.deltaTime, 0, ForceMode.VelocityChange);
		else if (IsThereWall())
		{
			WallSliding();
			if (m_isJumping && IsWallJumpable())
			{
				UnityEngine.Debug.Log("Wall jump");
				WallJump();
				m_isWallJumping = true;

			}
		}

		if (m_isMoving == false && IsThereFloor() && m_isJumping == false)
		{
			m_Rigidbody.velocity = Vector3.zero;
		}

	}

	//Checks if there is floor under the player.
	private bool IsThereFloor()
	{
		//TODO: Make it work
		if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1))
		{
			if (!hit.collider.isTrigger)
				return true;
		}
		return false;
	}

	private bool IsThereWall()
	{
		for (int i = -1; i <= 1; i += 2)
		{
			if (Physics.Raycast(transform.position, Vector3.right * i, out RaycastHit hit, 0.6f))
				if (hit.collider.isTrigger == false)
					if (hit.collider.gameObject.CompareTag("Object"))
					{
						UnityEngine.Debug.Log("Wall touched");
						return true;
					}
		}
		return false;
	} //

	private bool IsWallJumpable()
	{
		for (int i = -1; i <= 1; i += 2)
		{
			if (!Physics.Raycast(transform.position, Vector3.right * i, out RaycastHit hit, 0.6f))
				continue;

			if (!hit.collider.gameObject.GetComponent<Platform>())
				return true;
			else if (!hit.collider.GetComponent<Platform>().WallJumpAllowed)
				return false;
			else
				return true;

		}
		return true;
	}


	private void WallSliding()
	{
		Vector3 PlayerVelocity = m_Rigidbody.velocity;

		PlayerVelocity.y = Mathf.Clamp(PlayerVelocity.y, -WallSlidingSpeed, float.MaxValue);
		m_Rigidbody.velocity = PlayerVelocity;
	}

	private void WallJump()
	{
		RaycastHit hit = new();
        Vector3 wallNormal = Vector3.zero;
		if (Physics.Raycast(transform.position, Vector3.left, out RaycastHit leftHit, 0.6f))
		{
			wallNormal = leftHit.normal;
			hit = leftHit;
		}

		else if (Physics.Raycast(transform.position, Vector3.right, out RaycastHit rightHit, 0.6f))
		{
			wallNormal = rightHit.normal;
			hit = rightHit;
		}

		else
		{
			hit = new();
		}

		float direction = Mathf.Sign(Vector3.Dot(wallNormal, transform.up));
		m_Rigidbody.velocity = new Vector3(direction * DefaultWallJumpForce.x * wallNormal.x * Time.deltaTime, DefaultWallJumpForce.y * Time.deltaTime /2, 0);
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