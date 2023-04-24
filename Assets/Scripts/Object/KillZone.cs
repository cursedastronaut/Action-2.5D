using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
	private void OnCollisionEnter(Collision collision)
	{
		collision.gameObject.GetComponent<PlayerDeath>().killPlayer();
	}
}
