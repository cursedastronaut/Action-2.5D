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
    private bool    m_IsChanging        = false;
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

        
        if (m_IsChanging)
        {
            ColorIndex += 1;
            m_IsChanging = false;
        }

        if (ColorIndex > ColorUnlocked || ColorIndex >= AllColorsPlayerCanSwitchThrough.Length)
            ColorIndex = 0;

        if (ColorIndex != PreviousIndex)
        {
            m_Renderer.material.color = AllColorsPlayerCanSwitchThrough[ColorIndex];
        }
        UIColorPalette();
        PreviousIndex = ColorIndex;
    }

    //Reads input corresponding to the color change.
    public void ChangeColor(InputAction.CallbackContext ctx)
    {
        if (ColorChangingDelay >= 0.2f)
        { 
            ColorChangingDelay = 0;
            m_IsChanging = ctx.ReadValueAsButton();
        }
    }

    //Handles Color Palette UI (to show all colors you call loop through)
    private void UIColorPalette()
    {
        UI.transform.position = new Vector3(initialPos.x - (UI.GetComponent<RectTransform>().sizeDelta.y * (ColorUnlocked+1)) * UI.transform.localScale.x, initialPos.y, initialPos.z);
    }
}
