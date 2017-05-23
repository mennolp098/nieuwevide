using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnDirection
{
	None, Left, Right
}

public class NPCSpawner : MonoBehaviour {

	private SpawnDirection _spawnDirection;
	private NPCFactory _npcFactory;

	public float spawnWaitTime = 3;
	private float _spawnWaitCountDown;
	private System.Random _random = new System.Random();
	private int _randomNpc;

	void Awake()
	{
		_npcFactory = GetComponent<NPCFactory>();
	}

	void Start()
	{
		_spawnWaitCountDown = spawnWaitTime;
	}

	void Update()
	{
		if(_spawnWaitCountDown <= 0)
		{
			NewRandomNPC();
			NewSpawnDirection();

			if(_randomNpc <= 4)
				SpawnNPC<Human>();
			else if(_randomNpc >= 5)
				SpawnNPC<Victim>();
			
			_spawnWaitCountDown = spawnWaitTime;
		}
		_spawnWaitCountDown -= Time.deltaTime;
	}

	private void SpawnNPC<T>() where T : Human
	{
		Vector2 position = new Vector2();
		GameObject newNPC = _npcFactory.BuildNewNpc<T>();

		switch(_spawnDirection)
		{
		case SpawnDirection.Left:
			position = new Vector2(-7, 0);
			newNPC.GetComponent<T>().Direction = 1;
			break;
		case SpawnDirection.Right:
			position = new Vector2(7, 0);
			newNPC.GetComponent<T>().Direction = -1;
			break;
		}

		newNPC.transform.position = position;
	}

	private int NewRandomNPC()
	{
		_randomNpc = _random.Next(0, 10);
		return _randomNpc;
	}

	private SpawnDirection NewSpawnDirection()
	{
		int randomValue = _random.Next(0, 10);

		if(randomValue <= 4)
			_spawnDirection = SpawnDirection.Left;
		else if(randomValue >= 5)
			_spawnDirection = SpawnDirection.Right;

		return _spawnDirection;
	}
}