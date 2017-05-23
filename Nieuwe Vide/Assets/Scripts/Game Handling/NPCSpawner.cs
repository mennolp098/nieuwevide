using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnDirection
{
	Left, Right
}

public class NPCSpawner : MonoBehaviour {

	private SpawnDirection _spawnDirection;
	private NPCFactory _npcFactory;

	public float spawnWaitTime = 6;
	private float _spawnWaitCountDown;

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
			_spawnDirection = (SpawnDirection)Random.Range(0, 1);
			SpawnNPC();
			_spawnWaitCountDown = spawnWaitTime;
		}
		_spawnWaitCountDown -= Time.deltaTime;
	}

	private void SpawnNPC()
	{
		Vector2 position = new Vector2();

		switch(_spawnDirection)
		{
		case SpawnDirection.Left:
			position = new Vector2(-4, 0);
			break;
		case SpawnDirection.Right:
			position = new Vector2(4, 0);
			break;
		}

		GameObject newNPC = _npcFactory.BuildNewNpc();
		newNPC.transform.position = position;
	}
}