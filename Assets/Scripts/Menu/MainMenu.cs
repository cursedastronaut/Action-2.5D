using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private List<Selectable> m_MenuItems;
    private int m_SelectedItemIndex = 0;
    private bool m_SubmitPressed;
    private Vector2 m_Navigation;

    public void Submit(InputAction.CallbackContext context)
    {
        Debug.Log("Button pressed");
        m_SubmitPressed = context.ReadValueAsButton();
    }

    public void Navigate(InputAction.CallbackContext context) 
    {
        m_Navigation = context.ReadValue<Vector2>() ;
        Debug.Log("navigation");
    }

    private void SelectMenuItem(int index)
    {
        // deselect all menu items
        foreach (var item in m_MenuItems)
        {
            item.interactable = true;
        }

        // select the current menu item
        m_MenuItems[index].Select();
        m_MenuItems[index].interactable = false;
    }
    
    void Start()
    {
        m_MenuItems = new List<Selectable>(FindObjectsOfType<Selectable>());
    }
    private void Update()
    {
        // move to the next menu item when the player presses the Down button on the controller
        if (m_Navigation.y < 0 && !m_SubmitPressed)
        {
            m_SelectedItemIndex = (m_SelectedItemIndex + 1) % m_MenuItems.Count;
            SelectMenuItem(m_SelectedItemIndex);
        }

        // move to the previous menu item when the player presses the Up button on the controller
        if (m_Navigation.y > 0 && !m_SubmitPressed)
        {
            m_SelectedItemIndex--;
            if (m_SelectedItemIndex < 0)
            {
                m_SelectedItemIndex = m_MenuItems.Count - 1;
            }
            SelectMenuItem(m_SelectedItemIndex);
        }

        // select the current menu item when the player presses the Submit button on the controller
        if (m_SubmitPressed)
        {
            m_MenuItems[m_SelectedItemIndex].Select();
            m_SubmitPressed = false;
        }
    }
}