using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SingletonPlayerColor : MonoBehaviour
{
    public int ColorIndex = 0;
    public static SingletonPlayerColor instance;
    [SerializeField] public Color[] SelectableColors;
    public GameObject isBeingTeleported;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    public void AddToPlayerColor(int colorIndex)
    {
        ColorIndex += colorIndex;
    }

    public void ModifyColorIndex(int colorIndex)
    {
        ColorIndex = colorIndex;
    }

    // Start is called before the first frame update
    public int GetPlayerColor()
    {
        return ColorIndex;
    }
}
