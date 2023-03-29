using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerColor : MonoBehaviour
{
    private Color m_PlayerColor;
    private float ChangeCD;
    
    public int ColorIndex;
    private int PreviousIndex;
    private bool m_IsChanging;
    private bool m_IsChanging_bckp;

    [SerializeField] private Renderer m_Renderer;

    void Start()
    {
        m_Renderer = GetComponentInChildren<Renderer>();
        ChangeCD = 0;
    }

    
    void Update()
    {
        ChangeCD += Time.fixedDeltaTime;

        if (m_IsChanging)
        {
            ColorIndex += 1;
            m_IsChanging = false;
        }
        

        if (ColorIndex != PreviousIndex)
        {
            SetColor();
        }
        m_IsChanging_bckp = m_IsChanging;
        
        ColorIndex %= 6;

        PreviousIndex = ColorIndex;
    }

    private void SetColor()
    {
        switch (ColorIndex)
        {
            case 0:
                Debug.Log("Black");
                m_PlayerColor =new Color (0,0,0);
                break;

            case 1:
                Debug.Log("Color 1");
                m_PlayerColor = new Color(0, 1, 0);
                break;

            case 2:
                Debug.Log("Color 2");
                m_PlayerColor = new Color(0, 0, 1);
                break;

            case 3:
                Debug.Log("Color 3");
                m_PlayerColor = new Color(1, 0, 0);
                break;

            case 4:
                Debug.Log("Color 4");
                m_PlayerColor = new Color(1, 1, 0);
                break;

            case 5:
                Debug.Log("Color 5");
                m_PlayerColor = new Color(1, 0, 1);
                break;

            case 6:
                Debug.Log("Color 6");
                m_PlayerColor = new Color(0, 1, 1);
                break;

        }
                m_Renderer.material.color = m_PlayerColor;
    }

    public void ChangeColor(InputAction.CallbackContext ctx)
    {
        if (ChangeCD >= 5)
        { 
            ChangeCD = 0;
            m_IsChanging = ctx.ReadValueAsButton();
        }
    }
}
