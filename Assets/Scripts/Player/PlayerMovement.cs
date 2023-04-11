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
	[SerializeField][Tooltip("The Default Speed of the Player")]
		private float DefaultSpeed;
	[SerializeField][Tooltip("The Speed when the Player Sprints")]
		private float DefaultSprintSpeed;
	[SerializeField][Tooltip("The Force applied to the Player when they jump")]
		private float DefaultJumpForce;
	[SerializeField][Tooltip("The Force applied to the Player when they WallJump")]
		private float DefaultWallJumpForce;
	[SerializeField][Tooltip("The time before the Player has to fully push his joystick to one side to sprint")]
		private float DefaultSprintDelay;
	[SerializeField][Tooltip("The time it takes for the player to sprint")]
		private float DefaultSprintOffsetDelay;
	[SerializeField][Tooltip("The Zone in which player can move but sprint function won't apply")]
		private float DeadZone;
	[SerializeField][Tooltip("The Speed at which the Player shall slide down a wall.")]
		private float WallSlidingSpeed;
	

	//Game Programming Variables
	[Header("Game Programming Variables")]
	[SerializeField]	private bool ShowGPVariables = false;
	[NaughtyAttributes.ShowIf("ShowGPVariables")] [SerializeField]
		private Vector2 m_MovementInput = Vector2.zero;
	[NaughtyAttributes.ShowIf("ShowGPVariables")] [SerializeField]
		private bool m_isSprinting = false;
	[NaughtyAttributes.ShowIf("ShowGPVariables")] [SerializeField] 
		private bool m_canSprint = false;
	[NaughtyAttributes.ShowIf("ShowGPVariables")] [SerializeField] 
		private bool m_isJumping = false;
	[NaughtyAttributes.ShowIf("ShowGPVariables")] [SerializeField]
		private float m_SprintDelay = 0.0f;
	[NaughtyAttributes.ShowIf("ShowGPVariables")] [SerializeField]
		private PlayerColor m_PlayerColor;
	
	[SerializeField]
	private Rigidbody m_Rigidbody;

	// Start is called before the first frame update
	void Start()
	{
		if (ShowGPVariables) {}
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
