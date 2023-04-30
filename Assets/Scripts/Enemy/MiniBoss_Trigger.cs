using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBoss_Trigger : MonoBehaviour
{
	public bool triggered = false;
	private void OnCollisionEnter(Collision collision)
	{
		Debug.Log("awawa" + collision.collider.gameObject);
		if (triggered) return;
		if (!collision.collider.CompareTag("Player")) return;
		GetComponentInParent<MiniBoss>().m_MiniBossAwake = true;
		GetComponent<Platform>().shouldMove = true;
		triggered = true;
	}
}
