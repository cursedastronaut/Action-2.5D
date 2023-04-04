using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerColor : MonoBehaviour
{
    //Game Design Variables
    [SerializeField] private float defColorCooldown;
    [SerializeField] private float defColorTimer;

    //Game Programming Variables
    [SerializeField] private GameObject UIColPalette;
    [SerializeField] private GameObject UIColPaletteSelected;
    [SerializeField] private RectTransform UIGauge;
    public  bool    isHidden            = false;
    private bool    prev_isHidden       = false;

    //Color Change Variables
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
        SingletonPlayerColor.instance.ModifyColorIndex(0);
        m_Renderer = GetComponentInChildren<Renderer>();
        Color temp = SingletonPlayerColor.instance.SelectableColors[0];
        m_Renderer.material.color = new Color(temp.r, temp.g, temp.b, 1.0f) ;
        m_UiColorPalette_initialPos = UIColPalette.transform.position;
        m_UIGauge_maxSize = UIGauge.sizeDelta;
        m_UIGauge_InitialPosition = UIGauge.GetComponentInParent<Transform>().position;
        m_ColorTimer = defColorTimer;
    }
    
    void Update()
    {
        //If you are not using FixedUpdate(), avoid using Time.fixedDeltaTime
        ColorChangingDelay += Time.deltaTime;
        int NumberOfColors = SingletonPlayerColor.instance.SelectableColors.Length-1;
        UIColorGauge();
        if (m_IsChanging != 0)
        {
            SingletonPlayerColor.instance.AddToPlayerColor(m_IsChanging);
           
            m_IsChanging = 0;
        }
        RectTransform rt = UIColPaletteSelected.GetComponent<RectTransform>();
        rt.transform.localPosition = new Vector3(SingletonPlayerColor.instance.GetPlayerColor() * 96, rt.localPosition.y, 0);

        if (SingletonPlayerColor.instance.GetPlayerColor() > ColorUnlocked || SingletonPlayerColor.instance.GetPlayerColor() > NumberOfColors)
            SingletonPlayerColor.instance.ModifyColorIndex(0);
        if (SingletonPlayerColor.instance.GetPlayerColor() < 0)
            SingletonPlayerColor.instance.ModifyColorIndex(ColorUnlocked > NumberOfColors ? NumberOfColors : ColorUnlocked);
        
        
        if (SingletonPlayerColor.instance.GetPlayerColor() != PreviousIndex || isHidden != prev_isHidden)
        {
            Color colorWant = SingletonPlayerColor.instance.SelectableColors[SingletonPlayerColor.instance.GetPlayerColor()];
            m_Renderer.material.color = new Color(colorWant.r, colorWant.g, colorWant.b, isHidden ? 0.5f : 1.0f );
            prev_isHidden = isHidden;
        }

        if (isHidden)
            HideCheck();
        UIColorPalette();
        PreviousIndex = SingletonPlayerColor.instance.GetPlayerColor();
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
    private void UIColorPalette()
    {
        UIColPalette.transform.position = new Vector3(m_UiColorPalette_initialPos.x - (UIColPalette.GetComponent<RectTransform>().sizeDelta.y * (ColorUnlocked+1)) * UIColPalette.transform.localScale.x, m_UiColorPalette_initialPos.y, m_UiColorPalette_initialPos.z);
    }

    private void UIColorGauge()
    {
        if (m_ColorTimer < 0)
            SingletonPlayerColor.instance.ModifyColorIndex(0);
        if (SingletonPlayerColor.instance.GetPlayerColor() == 0)
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

    public void Hide(InputAction.CallbackContext context)
    {
        /*if (!context.ReadValueAsButton())
        { return; }*/
        HideCheck();
    }

    private void HideCheck()
    {
        foreach (var obj in Physics.OverlapBox(transform.position, new Vector3(0.1f, 0.1f, 0.1f)))
        {
            if (obj.tag == "Object" /*&&
                obj.GetComponent<ColorObject>().colorIndex == SingletonPlayerColor.instance.GetPlayerColor()*/) //If ObjectColor == PlayerColor
            {
                isHidden = true;
                return;
            }
        }
        isHidden = false;
    }
}
