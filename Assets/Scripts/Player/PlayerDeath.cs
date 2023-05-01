using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
	public	bool	m_isDead;
	private Vector3 m_Checkpoint;
	
	// Start is called before the first frame update
	void Start()
	{
		m_Checkpoint = transform.position;
	}


	public void killPlayer()
	{
		m_isDead = true;
		StartCoroutine(DeathCoolDown());
		SingletonMediaPlayer.instance.PlaySoundEffect(28);
	}

	public void editCheckpoint(Vector3 input)
	{
		m_Checkpoint = input;
	}

    private IEnumerator DeathCoolDown()
	{
		yield return new WaitForSeconds(0);
        transform.position = m_Checkpoint;
		m_isDead = false;
    }
}
