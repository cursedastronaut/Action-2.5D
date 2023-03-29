using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerColor : MonoBehaviour
{
    //Game Design Variables
    [SerializeField] private Color[] AllColorsPlayerCanSwitchThrough;

    //Game Programming Variables
    public  int     ColorIndex          = 0;
    public  int     ColorLevel          = 0;
    private int     PreviousIndex       = 0;
    private bool    m_IsChanging        = false;
    private float   ColorChangingDelay = 0;

    [SerializeField] private Renderer m_Renderer;

    void Start()
    {
        m_Renderer = GetComponentInChildren<Renderer>();
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
        

        if (ColorIndex != PreviousIndex)
        {
            m_Renderer.material.color = AllColorsPlayerCanSwitchThrough[ColorIndex];
        }
        
        ColorIndex %= 6;

        PreviousIndex = ColorIndex;
    }

    public void ChangeColor(InputAction.CallbackContext ctx)
    {
        if (ColorChangingDelay >= 0.2f)
        { 
            ColorChangingDelay = 0;
            m_IsChanging = ctx.ReadValueAsButton();
        }
    }
}
