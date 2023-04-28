using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TriggerEnemy : MonoBehaviour
{
	[SerializeField]	private	Enemy	m_Enemy;

	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("Player")) return;
		m_Enemy.m_IsOff = false;
		Destroy(this.gameObject);
	}
}
