using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Test : System.Attribute
{

}

public class PlayerColor : MonoBehaviour
{
	//Game Design Variables
	[Header("Game Design Variables")]
	[SerializeField][Tooltip(a)]private float			defColorCooldown;
	[SerializeField][Tooltip(b)]private float			defColorTimer;
	[SerializeField][Tooltip(c)]private float			RechargeMultiplier;
	[SerializeField][Tooltip(d)]private float			ScreenShakeLevel;

	//Game Programming Variables
	[Header("Game Programming Variables")]
	[SerializeField]			private bool			ShowGPVariables			= false;
	//[IGP,SerializeField]		private GameObject		UIColPalette;
	//[IGP,SerializeField]		private GameObject		UIColPaletteSelected;
	[IGP,SerializeField]		private RectTransform	UIGauge;
	[IGP,SerializeField]		private RectTransform	UITriangle;
	[IGP,SerializeField]		private PlayerCamera	m_Camera;
	[IGP,SerializeField]		public  bool			isHidden				= false;
	[IGP,SerializeField]		private bool			prev_isHidden			= false;

	//Color Change Variables
	[IGP,SerializeField]		public  int		ColorUnlocked		= 0;
	[IGP,SerializeField]		private int		PreviousIndex		= 0;
	[IGP,SerializeField]		private int		m_IsChanging		= 0;
	[IGP,SerializeField]		private float   ColorChangingDelay	= 0;

	//UI Color Palette
	[IGP,SerializeField]		private Vector3 m_UiColorPalette_initialPos;
	//UI Color Gauge
	[IGP,SerializeField]		private Vector2 m_UIGauge_maxSize;
	[IGP,SerializeField]		private Vector3 m_UIGauge_InitialPosition;
	[IGP,SerializeField]		private float m_ColorTimer;
	[IGP,SerializeField]		private float m_ColorGaugeDefXPos;

	//UI Color Triangle
	[IGP, SerializeField]		private float			m_TriangleAnimationTime;
	[IGP, SerializeField]		private float			m_TriangleAnimationProgress;
	[IGP, SerializeField]		private AnimationCurve	m_TriangleAnimation;
	[IGP, SerializeField]		private bool			m_TriangleShouldAnim		= false;
	[IGP]						private Quaternion		ui_TriangleStartRot = Quaternion.identity;
	[IGP,SerializeField]		private Renderer m_Renderer;
	void Start()
	{
		if (ShowGPVariables) { } //Avoid a warning.
		SingletonPlayerColor.instance.ModifyColorIndex(0);
		m_Renderer = GetComponentInChildren<Renderer>();
		Color temp = SingletonPlayerColor.instance.SelectableColors[0];
		m_Renderer.material.color = new Color(temp.r, temp.g, temp.b, 1.0f) ;
		//m_UiColorPalette_initialPos = UIColPalette.transform.position;
		m_UIGauge_maxSize = UIGauge.sizeDelta;
		m_UIGauge_InitialPosition = UIGauge.GetComponentInParent<Transform>().position;
		m_ColorTimer = defColorTimer;
		m_ColorGaugeDefXPos = UIGauge.position.x + UIGauge.sizeDelta.x/2;
	}
	
	void Update()
	{
		//If you are not using FixedUpdate(), avoid using Time.fixedDeltaTime
		ColorChangingDelay += Time.deltaTime;
		int NumberOfColors = SingletonPlayerColor.instance.SelectableColors.Length-1;
		
		if (m_IsChanging != 0)
		{
			SingletonPlayerColor.instance.AddToPlayerColor(m_IsChanging);
			m_TriangleShouldAnim = true;
			UIGauge.GetComponent<UIColor>().ChangeIndex(SingletonPlayerColor.instance.ColorIndex);
			m_IsChanging = 0;
		}
		//RectTransform rt = UIColPaletteSelected.GetComponent<RectTransform>();
		//rt.transform.localPosition = new Vector3(SingletonPlayerColor.instance.GetPlayerColor() * 96, rt.localPosition.y, 0);

		if (SingletonPlayerColor.instance.GetPlayerColor() > NumberOfColors)
			SingletonPlayerColor.instance.ModifyColorIndex(0);
		if (SingletonPlayerColor.instance.GetPlayerColor() < 0)
			SingletonPlayerColor.instance.ModifyColorIndex(ColorUnlocked > NumberOfColors ? NumberOfColors : ColorUnlocked);


		if (SingletonPlayerColor.instance.GetPlayerColor() != PreviousIndex || isHidden != prev_isHidden)
		{
			Color colorWant = SingletonPlayerColor.instance.SelectableColors[SingletonPlayerColor.instance.GetPlayerColor()];
			m_Renderer.material.color = new Color(colorWant.r, colorWant.g, colorWant.b, isHidden ? 0.5f : 1.0f );
			prev_isHidden = isHidden;
		}
		UIColorGauge();

		if (isHidden)
		{
			Debug.Log("uwu");
			HideCheck();
		}
		objectCheck();
		//UIColorPalette();
		UIColorTriangleUpdate();
		PreviousIndex = SingletonPlayerColor.instance.GetPlayerColor();
	}

	private void objectCheck()
	{
		foreach (var obj in Physics.OverlapBox(transform.position, new Vector3(0.01f, 0.01f, 0.01f)))
		{
			if (obj.tag == "Object" && SingletonPlayerColor.instance.GetPlayerColor() != obj.GetComponent<Platform>().currentColor) //If ObjectColor == PlayerColor
			{
				GetComponent<PlayerDeath>().killPlayer();
				return;
			}
			else if (obj.tag == "Portal")
			{
				
			}
		}
	}

	//Reads input corresponding to the color change.
	public void NextColor(InputAction.CallbackContext ctx)
	{
		if (ColorChangingDelay >= 0.2f && m_ColorTimer >= 0)
		{ 
			ColorChangingDelay = 0;
			m_IsChanging = ctx.ReadValueAsButton() ? 1 : 0;
		}
	}

	public void PreviousColor(InputAction.CallbackContext ctx)
	{
		if (ColorChangingDelay >= 0.2f && m_ColorTimer >= 0)
		{
			ColorChangingDelay = 0;
			m_IsChanging = ctx.ReadValueAsButton() ? -1 : 0;
		}
	}

	//Handles Color Palette UI (to show all colors you call loop through)
	/*private void UIColorPalette()
	{
		UIColPalette.transform.position = new Vector3(m_UiColorPalette_initialPos.x - (UIColPalette.GetComponent<RectTransform>().sizeDelta.y * (ColorUnlocked+1)) * UIColPalette.transform.localScale.x, m_UiColorPalette_initialPos.y, m_UiColorPalette_initialPos.z);
	}*/

	private void UIColorGauge()
	{
		bool isDrained = true;
		if (m_ColorTimer < 0)
			m_IsChanging = -SingletonPlayerColor.instance.GetPlayerColor();
		if (SingletonPlayerColor.instance.GetPlayerColor() == 0)
		{
			if (m_ColorTimer < defColorTimer)
			{
				m_ColorTimer += Time.deltaTime * RechargeMultiplier;
				isDrained = false;
			}
		}
		else
		{
			isDrained = true;
			m_ColorTimer -= Time.deltaTime; 
		}
		float newScale = (m_ColorTimer * (100 / defColorTimer)) * (m_UIGauge_maxSize.x / 100);
		UIGauge.sizeDelta = new Vector2(
			Mathf.Abs(newScale),
			UIGauge.sizeDelta.y);
		UIGauge.transform.position = new Vector3(m_ColorGaugeDefXPos - UIGauge.sizeDelta.x / 2, UIGauge.transform.position.y);
		if (m_ColorTimer / defColorTimer * 100 < ScreenShakeLevel && m_ColorTimer / defColorTimer * 100 > ScreenShakeLevel-5 && isDrained)
			m_Camera.ShakeTime = 0.2f;
	}

	public void Hide(InputAction.CallbackContext context)
	{
		if (isHidden)
		{
			isHidden = false;
			return;
		}
		else
			HideCheck();
	}

	private void HideCheck()
	{
		foreach (var obj in Physics.OverlapBox(transform.position, new Vector3(0.1f, 0.1f, 0.1f)))
		{
			if (obj.tag == "Object" &&
				((obj.TryGetComponent(out Platform plat) && plat.currentColor == SingletonPlayerColor.instance.GetPlayerColor() && plat.shouldAllowHiding)) || 
				((obj.TryGetComponent(out BackgroundHide bckg) && bckg.colorIndex == SingletonPlayerColor.instance.GetPlayerColor() && bckg.shouldAllowHiding)))
			{
				isHidden = true;
				return;
			}
		}
		isHidden = false;
	}



	void UIColorTriangleUpdate()
	{
		if (!m_TriangleShouldAnim) return;

		int playerColor = SingletonPlayerColor.instance.GetPlayerColor();
		m_TriangleAnimationProgress += Time.deltaTime / m_TriangleAnimationTime;
		Quaternion rotation = Quaternion.Slerp(ui_TriangleStartRot, Quaternion.Euler(0, 0, 120 * playerColor), m_TriangleAnimation.Evaluate(m_TriangleAnimationProgress));
		UITriangle.transform.rotation = rotation;

		if (m_TriangleAnimationProgress >= 1) 
		{
			m_TriangleShouldAnim = false;
			m_TriangleAnimationProgress = 0;
			ui_TriangleStartRot = UITriangle.transform.rotation;
		}
	}
	private const string a = "The time in seconds it takes to return to a full gauge.";
	private const string b = "The time in seconds it takes for the gauge to fully drain.";
	private const string c = "defColorTimer multiplied by THIS gives the time it takes for the color gauge to fully fill back up.";
	private const string d = "At what percentage of the color bar should the screen shake. (Between 0 and 100)";
}
