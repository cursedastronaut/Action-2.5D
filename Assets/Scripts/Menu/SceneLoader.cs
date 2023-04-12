using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string Scene;
    public void OnClick()
    {
        Debug.Log("uzu");
        SceneManager.LoadScene(Scene, 0);
    }

    

}
