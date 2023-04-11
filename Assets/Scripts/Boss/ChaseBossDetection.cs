using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseBossDetection : MonoBehaviour
{
	
	private void OnTriggerEnter(Collider collider)
	{
		Debug.Log("test");
		if (collider.gameObject.CompareTag("Player")) collider.gameObject.GetComponent<PlayerDeath>().killPlayer();
		else if (collider.gameObject.CompareTag("Object")) Destroy(collider.gameObject);
	}
}
