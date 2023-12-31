using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SingletonPlayerColor : MonoBehaviour
{
	[HideInInspector]				public static	SingletonPlayerColor	instance;
	[SerializeField][Tooltip(a)]	public			Color[]					SelectableColors;

	[Header("Game Programming Variables")]
	[SerializeField]				private			bool					ShowGPVariables		= false;
	[IGP][SerializeField]			public			int						ColorIndex			= 0;
	[IGP][SerializeField]			public			GameObject				isBeingTeleported;

	private void Awake()
	{
		if (ShowGPVariables) { } //Avoid a warning.

		// If there is an instance, and it's not me, delete myself.
		if (instance != null && instance != this)
			Destroy(this);
		else
			instance = this;
	}

	public void AddToPlayerColor(int colorIndex)
	{
		ColorIndex += colorIndex;
		int tempindex= SingletonPlayerColor.instance.GetPlayerColor();
		int tempunlocked = GetComponentInParent<PlayerColor>().ColorUnlocked;
		if (tempindex > SelectableColors.Length-1 || tempindex > tempunlocked)
			SingletonPlayerColor.instance.ModifyColorIndex(0);
		if (tempindex < 0)
			SingletonPlayerColor.instance.ModifyColorIndex(tempunlocked);
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

	private const string a = "Where all colors the player and the platforms can switch through are set.";
}
