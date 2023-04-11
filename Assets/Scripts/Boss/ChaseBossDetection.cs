using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChaseBossDetection : MonoBehaviour
{
	
	private void OnTriggerEnter(Collider collider)
	{
		Debug.Log("test");
		if (collider.gameObject.CompareTag("Player")) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		else if (collider.gameObject.CompareTag("Object")) Destroy(collider.gameObject);
	}
}
