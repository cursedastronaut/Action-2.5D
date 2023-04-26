using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SingletonMediaPlayer : MonoBehaviour
{

	[HideInInspector]	public	static	SingletonMediaPlayer	instance;
	[HideInInspector]	private			AudioSource				m_AudioSource;
	[SerializeField]	private			AudioClip[]				m_AudioClips;
	[HideInInspector]
	public string[] AudioNames = {
		"amb_ing�_loop"			,
		"amb_paint_loop"		,
		"amb_tuto"				,
		"amb_zapground_loop"	,
		"boss_rage"				,
		"boss_spawn"			,
		"checkpoint"			,
		"conveyor_loop"			,
		"door_opening"			,
		"elev_ding"				,
		"elev_doors_close"		,
		"elev_robo_death"		,
		"enemy_clicclac"		,
		"enemy_laser_loop"		,
		"enemy_move_loop"		,
		"player_death"			,
		"player_jump"			,
		"player_walk"			,
		"portal"				,
		"projo_on"				,
		"robot_close_loop"		,
		"walljump"				,
		"zap_death"
	};

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

	// Start is called before the first frame update
	void Start()
    {
		m_AudioSource = GetComponent<AudioSource>();
	}

    public void PlaySoundEffect(string index)
	{
		m_AudioSource.PlayOneShot(m_AudioClips[ArrayUtility.IndexOf(AudioNames, index)]);
	}

}
