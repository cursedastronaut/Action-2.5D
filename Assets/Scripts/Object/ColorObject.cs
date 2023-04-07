using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorObject : MonoBehaviour
{
    //Game Design Variables
    [SerializeField] public int[]   colorIndex;
    [SerializeField] public float   timeBetweenColorSwitch;

    // Game Programming Variables
    Renderer m_Renderer;
    private int     m_Index = 0;
    private float   m_Timer = 0;

    //Called eslewhere variables
    public int currentColor = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_Renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (SingletonPlayerColor.instance.GetPlayerColor() == colorIndex[m_Index])
            GetComponent<BoxCollider>().isTrigger = true;
        else
            GetComponent<BoxCollider>().isTrigger = false;

        if (m_Timer >= timeBetweenColorSwitch)
            SwitchColor();
        m_Timer += Time.deltaTime;
       

        m_Renderer.material.color = SingletonPlayerColor.instance.SelectableColors[colorIndex[m_Index]];
        currentColor = m_Index;
    }

   private void SwitchColor()
    {
        m_Index++;
        if (m_Index >= colorIndex.Length)
            m_Index = 0;
        m_Timer = 0;
    }
}
