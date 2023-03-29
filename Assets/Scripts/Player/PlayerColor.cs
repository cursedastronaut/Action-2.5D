using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerColor : MonoBehaviour
{
    //Game Design Variables
    [SerializeField] private Color[] AllColorsPlayerCanSwitchThrough;

    //Game Programming Variables
    [SerializeField] private GameObject UI;
    public  int     ColorIndex          = 0;
    public  int     ColorUnlocked       = 0;
    private int     PreviousIndex       = 0;
    private int     m_IsChanging        = 0;
    private float   ColorChangingDelay = 0;

    private Vector3 initialPos;

    [SerializeField] private Renderer m_Renderer;

    void Start()
    {
        m_Renderer = GetComponentInChildren<Renderer>();
        m_Renderer.material.color = AllColorsPlayerCanSwitchThrough[0];
        initialPos = UI.transform.position;
    }
    
    void Update()
    {
        //If you are not using FixedUpdate(), avoid using Time.fixedDeltaTime
        ColorChangingDelay += Time.deltaTime;
        int NumberOfColors = AllColorsPlayerCanSwitchThrough.Length-1;
        Debug.Log(NumberOfColors);

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
        if (ColorChangingDelay >= 0.2f)
        { 
            ColorChangingDelay = 0;
            m_IsChanging = ctx.ReadValueAsButton() ? 1 : 0;
        }
    }

    public void PreviousColor(InputAction.CallbackContext ctx)
    {
        if (ColorChangingDelay >= 0.2f)
        {
            ColorChangingDelay = 0;
            m_IsChanging = ctx.ReadValueAsButton() ? -1 : 0;
        }
    }

    //Handles Color Palette UI (to show all colors you call loop through)
    private void UIColorPalette()
    {
        UI.transform.position = new Vector3(initialPos.x - (UI.GetComponent<RectTransform>().sizeDelta.y * (ColorUnlocked+1)) * UI.transform.localScale.x, initialPos.y, initialPos.z);
    }
}
