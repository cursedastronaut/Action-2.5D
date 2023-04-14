using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIColor : MonoBehaviour
{

	[SerializeField] private Texture[] m_Textures;

    public void ChangeIndex(int color)
	{
		GetComponent<RawImage>().texture = m_Textures[color];
	}
}
