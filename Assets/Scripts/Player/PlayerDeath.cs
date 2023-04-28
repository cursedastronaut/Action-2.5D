using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
	public bool m_isDead;
	private Vector3 m_Checkpoint;
	
	// Start is called before the first frame update
	void Start()
	{
		m_Checkpoint = transform.position;
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public void killPlayer()
	{
		m_isDead = true;
		SingletonMediaPlayer.instance.PlaySoundEffect("player_death");
		StartCoroutine(DeathCoolDown());
	}

	public void editCheckpoint(Vector3 input)
	{
		m_Checkpoint = input;
	}

    private IEnumerator DeathCoolDown()
	{
		yield return new WaitForSeconds(5);
        transform.position = m_Checkpoint;
		m_isDead = false;
    }
}
