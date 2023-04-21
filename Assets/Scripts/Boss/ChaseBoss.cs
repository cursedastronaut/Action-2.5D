
using UnityEngine;

public class ChaseBoss : MonoBehaviour
{

	//Game Design Variables
	[Header("Game Design Variables")]
	[SerializeField] private float m_Speed;	
	[SerializeField] private bool  m_GoesLeft;

	//Game Programming Variables
	[Header("Game Programming Variables")]
	private bool isStopped = false;

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		if (!isStopped)
			transform.position += new Vector3((m_GoesLeft ? 1 : -1) * m_Speed * Time.fixedDeltaTime, 0, 0); ;
    }
}
