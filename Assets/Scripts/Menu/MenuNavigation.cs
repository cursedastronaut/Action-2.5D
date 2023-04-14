using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class MenuNavigation : MonoBehaviour
{
    [SerializeField] private List<Button> m_MenuItems;
    [SerializeField] private GameObject m_MenuItemVisual;

    private float m_SwitchTime;
    private int m_SelectedItemIndex = 0;
    private bool m_SubmitPressed;
    private bool m_CancelPressed;
    private Vector2 m_Navigation;

    public void Submit(InputAction.CallbackContext context)
    {
        Debug.Log("Submit pressed");
        m_SubmitPressed = context.ReadValueAsButton();
    }

    public void Cancel(InputAction.CallbackContext context)
    {
        Debug.Log("Cancel pressed");
        m_CancelPressed = context.ReadValueAsButton();
    }

    public void Navigate(InputAction.CallbackContext context) 
    {
        m_Navigation = context.ReadValue<Vector2>() ;
        //Debug.Log("navigation");
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

        // set the position and scale of the visual indicator
        if (m_MenuItemVisual != null)
        {
            m_MenuItemVisual.transform.position = m_MenuItems[index].transform.position;
        }
    }



    void Start()
    {
        //m_MenuItems = new List<Button>(FindObjectsOfType<Button>());
    }

    private void Update()
    {
            // move to the next menu item when the player presses the Down button on the controller
        if (m_Navigation.y < 0 && !m_SubmitPressed && m_SwitchTime <= 0)
        {
            m_SelectedItemIndex = (m_SelectedItemIndex + 1) % m_MenuItems.Count;
            SelectMenuItem(m_SelectedItemIndex);
            m_SwitchTime = 45;
        }
        else
        m_SwitchTime--;
        // move to the previous menu item when the player presses the Up button on the controller
        if (m_Navigation.y > 0 && !m_SubmitPressed && m_SwitchTime <= 0)
        {
            m_SelectedItemIndex--;
            if (m_SelectedItemIndex < 0)
            {
                m_SelectedItemIndex = m_MenuItems.Count - 1;
            }
            SelectMenuItem(m_SelectedItemIndex);
            m_SwitchTime = 45;
        }

    // select the current menu item when the player presses the Submit button on the controller
        if (m_SubmitPressed)
        {
            m_MenuItems[m_SelectedItemIndex].onClick.Invoke();
            m_SubmitPressed = false;
        }
    }

}