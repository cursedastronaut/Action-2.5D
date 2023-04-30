using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBoss : MonoBehaviour
{
	[Header("Game Design Variables")]
	[SerializeField]		private float[]			YpositionForEachStep;
	[SerializeField]		private float			FlickeringTime;

	[Header("Game Programming Variables")]
	[SerializeField]		private bool			ShowGPVariables		= false;
	[IGP, SerializeField]	public	bool			m_MiniBossAwake		= false;
	[IGP, SerializeField]	private GameObject[]	m_Enemies;
	[IGP, SerializeField]	private int[]			m_EnemiesStates;
	[IGP, SerializeField]	private float[][]		m_EnemiesFlickeringTime;
	[IGP, SerializeField]	private GameObject		m_Platform;
	[IGP, SerializeField]	private Vector3			m_Platform_Init;
	[IGP, SerializeField]	private List<Steps>		allSteps;						//Function array
	[IGP, SerializeField]	private int				currentLevel		= 0;
	private delegate void Steps();
	// Start is called before the first frame update
	void Start()
	{
		m_Platform_Init = m_Platform.transform.position;
		if (ShowGPVariables) { } //Avoid a warning
		allSteps = new List<Steps>();
		allSteps.Add(Step1);
		allSteps.Add(Step2);
		allSteps.Add(Step3);
		allSteps.Add(Step4);
		allSteps.Add(Step5);
		allSteps.Add(Step6);
		allSteps.Add(Step7);
		m_EnemiesStates = new int[m_Enemies.Length];
		m_EnemiesFlickeringTime = new float[m_Enemies.Length][];
		for (int i = 0; i < m_Enemies.Length; i++)
		{ 
			m_EnemiesFlickeringTime[i] = new float[2];
			m_Enemies[i].GetComponent<Enemy>().m_IsVictorBoss = true;
			m_Enemies[i].GetComponent<Enemy>().m_Light.SetActive(false);
		}
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (!m_MiniBossAwake) return;
		for (int i = 0; i < m_Enemies.Length; i++)
			EnemyUpdate(i);
		for (int i = 0; i < allSteps.Count; i++)
		{
			if (m_Platform.transform.position.y >= YpositionForEachStep[i] && m_Platform.transform.position.y < YpositionForEachStep[i+1])
			{ allSteps[i](); return; }
		}

		

		
	}

	private void EnemyUpdate(int index) {
		switch (m_EnemiesStates[index])
		{
			case 0:
				m_Enemies[index].GetComponent<Enemy>().m_IsOff = true; break;
			case 1:
				Flicker(index); break;
			case 2:
				m_Enemies[index].GetComponent<Enemy>().m_IsOff = false; break;
		}
	}

	private void TurnOn(int index) {
		if (m_EnemiesStates[index] == 0)
			m_EnemiesStates[index] = 1;
	}

	private void TurnOff(int index)
	{
		m_EnemiesStates[index] = 0;
		m_Enemies[index].GetComponent<Enemy>().m_Light.SetActive(false);
	}

	private void Flicker(int index)
	{
		if (m_EnemiesFlickeringTime[index][1] >= 0.04f)
		{
			m_Enemies[index].GetComponent<Enemy>().m_Light.SetActive(!m_Enemies[index].GetComponent<Enemy>().m_Light.activeSelf);
			m_EnemiesFlickeringTime[index][1] = 0;
		}
		m_Enemies[index].GetComponent<Enemy>().m_IsOff = true;
		m_EnemiesFlickeringTime[index][0] += Time.fixedDeltaTime;
		m_EnemiesFlickeringTime[index][1] += Time.fixedDeltaTime;
		if (m_EnemiesFlickeringTime[index][0] < FlickeringTime) return;

		m_EnemiesFlickeringTime[index][0] = 0;
		m_EnemiesStates[index] = 2;
		m_Enemies[index].GetComponent<Enemy>().m_Light.SetActive(true);
	}

	private void TurnAllOff()
	{
		for (int j = 0; j < m_Enemies.Length; j++)
						TurnOff(j);
	}

	private void Step1() {
		if (currentLevel != 1)
			TurnAllOff();
		currentLevel = 1;
		TurnOff(0);
		TurnOff(1);
		TurnOff(2);
		TurnOn (3);
	}

	private void Step2()
	{
		if (currentLevel != 2)
			TurnAllOff();
		currentLevel = 2;
		int i = 0;
		TurnOff(i++);
		TurnOn (i++);
		TurnOff(i++);
		TurnOff(i++);
	}

	private void Step3()
	{
		if (currentLevel != 3)
			TurnAllOff();
		currentLevel = 3;
		int i = 0;
		TurnOff(i++);
		TurnOn(i++);
		TurnOn(i++);
		TurnOff(i++);
	}

	private void Step4()
	{
		if (currentLevel != 4)
			TurnAllOff();
		currentLevel = 4;
		int i = 0;
		TurnOn(i++);
		TurnOff(i++);
		TurnOff(i++);
		TurnOn(i++);
	}

	private void Step5()
	{
		if (currentLevel != 5)
			TurnAllOff();
		currentLevel = 5;
		int i = 0;
		TurnOn(i++);
		TurnOff(i++);
		TurnOn(i++);
		TurnOn(i++);
	}

	private void Step6()
	{
		if (currentLevel != 6)
			TurnAllOff();
		currentLevel = 6;
		int i = 0;
		TurnOff(i++);
		TurnOn(i++);
		TurnOn(i++);
		TurnOn(i++);
	}

	private void Step7()
	{
		if (currentLevel != 7)
			TurnAllOff();
		currentLevel = 7;
		int i = 0;
		TurnOn(i++);
		TurnOn(i++);
		TurnOn(i++);
		TurnOff(i++);
	}

	public void Reset()
	{
		m_Platform.transform.position = m_Platform_Init;
		m_Platform.GetComponent<Platform>().shouldMove = false;
		m_Platform.GetComponent<MiniBoss_Trigger>().triggered = false;
		m_MiniBossAwake = false;
		currentLevel = 0;
		
		TurnAllOff();
		Start();
	}

}
