using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SingletonPlayerColor : MonoBehaviour
{
	[HideInInspector]		public static SingletonPlayerColor instance;

	[SerializeField][Tooltip("Where all colors the player and the platforms can switch through are set.")]
		public Color[] SelectableColors;
	[Header("Game Programming Variables")]
	[SerializeField]		private bool ShowGPVariables = false;
	[IGP][SerializeField]	public int ColorIndex = 0;
	[IGP][SerializeField]	public GameObject isBeingTeleported;

	private void Awake()
	{
		if (ShowGPVariables) { } //Avoid a warning.
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
		if (SingletonPlayerColor.instance.GetPlayerColor() > SelectableColors.Length-1)
			SingletonPlayerColor.instance.ModifyColorIndex(0);
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
