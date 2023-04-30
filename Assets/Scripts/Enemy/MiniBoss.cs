using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBoss : MonoBehaviour
{
	[Header("Game Design Variables")]
	[SerializeField]		private float[]			YpositionForEachStep;

	[Header("Game Programming Variables")]
	[SerializeField]		private bool			ShowGPVariables		= false;
	[IGP, SerializeField]	private bool			m_MiniBossAwake		= false;
	[IGP, SerializeField]	private Transform		m_Player;
	[IGP, SerializeField]	private GameObject[]	m_Enemies;
	[IGP, SerializeField]	private Platform		m_Platform;
	[IGP, SerializeField]	private List<Steps>		allSteps;						//Function array
	private delegate void Steps();
	// Start is called before the first frame update
	void Start()
	{
		if (ShowGPVariables) { } //Avoid a warning
		allSteps = new List<Steps>();
		allSteps.Add(Step1);
		allSteps.Add(Step1);
	}

	// Update is called once per frame
	void Update()
	{
		if (!m_Platform.shouldMove && !m_MiniBossAwake) return;

		for (int i = 0; i < allSteps.Count; i++)
			if (m_Player.position.y >= YpositionForEachStep[i])
				allSteps[i]();
	}

	private void Step1() {
	}

	private void Step2()
	{
	}

	private void Step3()
	{
	}

	private void Step4()
	{
	}

}
