using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerColor : MonoBehaviour
{
    //Game Design Variables
    [SerializeField] private Color[] AllColorsPlayerCanSwitchThrough;
    [SerializeField] private float defColorCooldown;
    [SerializeField] private float defColorTimer;

    //Game Programming Variables
    [SerializeField] private GameObject UI;
    [SerializeField] private RectTransform UIGauge;

    //Color Change Variables
    public  int     ColorIndex          = 0;
    public  int     ColorUnlocked       = 0;
    private int     PreviousIndex       = 0;
    private int     m_IsChanging        = 0;
    private float   ColorChangingDelay = 0;

    //UI Color Palette
    private Vector3 m_UiColorPalette_initialPos;
    //UI Color Gauge
    private Vector2 m_UIGauge_maxSize;
    private Vector3 m_UIGauge_InitialPosition;
    private float m_ColorTimer;

    [SerializeField] private Renderer m_Renderer;

    void Start()
    {
        m_Renderer = GetComponentInChildren<Renderer>();
        m_Renderer.material.color = AllColorsPlayerCanSwitchThrough[0];
        m_UiColorPalette_initialPos = UI.transform.position;
        m_UIGauge_maxSize = UIGauge.sizeDelta;
        m_UIGauge_InitialPosition = UIGauge.GetComponentInParent<Transform>().position;
        m_ColorTimer = defColorTimer;
    }
    
    void Update()
    {
        //If you are not using FixedUpdate(), avoid using Time.fixedDeltaTime
        ColorChangingDelay += Time.deltaTime;
        int NumberOfColors = AllColorsPlayerCanSwitchThrough.Length-1;

        UIColorGauge();
        if (m_IsChanging != 0)
        {
            ColorIndex += m_IsChanging;
            m_IsChanging = 0;
        }

        if (ColorIndex > ColorUnlocked || ColorIndex > NumberOfColors)
            ColorIndex = 0;
        if (ColorIndex < 0)
            ColorIndex = ColorUnlocked > NumberOfColors ? NumberOfColors : ColorUnlocked;

        if (ColorIndex != PreviousIndex)
        {
            m_Renderer.material.color = AllColorsPlayerCanSwitchThrough[ColorIndex];
        }
        UIColorPalette();
        PreviousIndex = ColorIndex;
    }

    //Reads input corresponding to the color change.
    public void NextColor(InputAction.CallbackContext ctx)
    {
        if (ColorChangingDelay >= 0.2f && m_ColorTimer <= 0)
        { 
            ColorChangingDelay = 0;
            m_IsChanging = ctx.ReadValueAsButton() ? 1 : 0;
        }
    }

    public void PreviousColor(InputAction.CallbackContext ctx)
    {
        if (ColorChangingDelay >= 0.2f && m_ColorTimer <= 0)
        {
            ColorChangingDelay = 0;
            m_IsChanging = ctx.ReadValueAsButton() ? -1 : 0;
        }
    }

    //Handles Color Palette UI (to show all colors you call loop through)
    private void UIColorPalette()
    {
        UI.transform.position = new Vector3(m_UiColorPalette_initialPos.x - (UI.GetComponent<RectTransform>().sizeDelta.y * (ColorUnlocked+1)) * UI.transform.localScale.x, m_UiColorPalette_initialPos.y, m_UiColorPalette_initialPos.z);
    }

    private void UIColorGauge()
    {
        Debug.Log(m_ColorTimer);
        if (m_ColorTimer < 0)
            ColorIndex = 0;
        if (ColorIndex == 0)
        {
            if (m_ColorTimer < 0)
                m_ColorTimer -= Time.deltaTime;
            if (m_ColorTimer <= -defColorCooldown)
                m_ColorTimer = defColorTimer;
        }
        else
        {
            m_ColorTimer -= Time.deltaTime;
        }
        float newScale = (m_ColorTimer * (100 / (m_ColorTimer < 0 ? defColorCooldown : defColorTimer))) * (m_UIGauge_maxSize.x / 100);
        UIGauge.sizeDelta = new Vector2(
            Mathf.Abs(newScale),
            UIGauge.sizeDelta.y);    
    }
}
