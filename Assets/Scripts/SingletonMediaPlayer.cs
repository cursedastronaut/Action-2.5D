using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class SingletonMediaPlayer : MonoBehaviour
{

	[HideInInspector]	public	static	SingletonMediaPlayer	instance;
	[HideInInspector]	private			AudioSource				m_AudioSource;
	[SerializeField]	private			AudioClip[]				m_AudioClips;
	[SerializeField]	private			float[]					m_AudioVolume;

	public string[] AudioNames = {/*
		"amb_boss"				,
		"amb_ing�_loop"			,
		"amb_paint_loop"		,
		"amb_tuto"				,
		"amb_zapground_loop"	,
		"boss_rage"				,
		"boss_spawn"			,
		"change_color"			,
		"change_color_fail"		,
		"checkpoint"			,
		"conveyor_loop"			,
		"door_opening"			,
		"elev_ding"				,
		"elev_doors_close"		,
		"elev_robo_death"		,
		"enemy_clicclac"		,
		"enemy_laser_loop"		,
		"enemy_move_loop"		,
		"jump_impact"			,
		"menu_click"			,
		"menu_hover"			,
		"player_death"			,
		"player_hide"			,
		"player_jump"			,
		"player_uncover"		,
		"player_walk"			,
		"portal"				,
		"projo_on"				,
		"robot_close_loop"		,
		"walljump"				,
		"zap_death"*/
	};

	private void Awake()
	{
	 // If there is an instance, and it's not me, delete myself.

		m_AudioSource = GetComponent<AudioSource>();
		if (instance != null && instance != this)
		{
			Destroy(this);
		}
		else
		{
			instance = this;
		}
		string[] characters = System.IO.Directory.GetFiles(Application.dataPath + "/Sounds");
		string[] charactersFinal = new string[characters.Length/2];
		for (int i = 0, j = 0; i < characters.Length; i++)
		{
			if (!characters[i].Contains(".meta"))
            {
				charactersFinal[j] = characters[i];
				j++;
            }
		}
		AudioNames = new string[charactersFinal.Length];
		for (int i = 0; i < charactersFinal.Length; i++)
		{
			AudioNames[i] = System.IO.Path.GetFileNameWithoutExtension(charactersFinal[i]);
		}
	}

	// Start is called before the first frame update
	void Start()
    {
	}

    public void PlaySoundEffect(int index)
	{

		m_AudioSource.PlayOneShot(m_AudioClips[index]);
		SelectVolume(index);
	}

	private float SelectVolume(int index)
	{
		return m_AudioVolume[index];
	}

}
