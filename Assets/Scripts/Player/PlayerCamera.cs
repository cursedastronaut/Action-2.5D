using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
	[SerializeField]	private Transform		player;
	[SerializeField]	private	float			offsetY;
                        private float			initialZPosition;
	[Header("Shaking Effect")]
	[SerializeField]	private AnimationCurve	CamShakeX;
	[SerializeField]	private AnimationCurve	CamShakeY;
	[SerializeField]	private float			CamShakeForce;
	[SerializeField]	private float			CamShakeSpeed;
	[HideInInspector]	public	float			ShakeTime			= 0;
	// Start is called before the first frame update
	void Start()
    {
        initialZPosition = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.position.x, player.position.y + offsetY, initialZPosition);
		if (ShakeTime > 0)
		{
			transform.position += new Vector3(CamShakeX.Evaluate(Time.time * CamShakeSpeed), CamShakeY.Evaluate(Time.time * CamShakeSpeed), 0) * CamShakeForce;
			ShakeTime -= Time.deltaTime;
		}
	}
}
