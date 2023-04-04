using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]    private Transform player;
                        private float initialZPosition;
    // Start is called before the first frame update
    void Start()
    {
        initialZPosition = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.position.x, player.position.y, initialZPosition);
    }
}
