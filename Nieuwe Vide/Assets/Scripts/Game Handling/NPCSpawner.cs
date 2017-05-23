using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnDirection
{
	Left, Right
}

public class NPCSpawner : MonoBehaviour {

	private SpawnDirection _spawnDirection;
	public float spawnWaitTime = 0;

	void Awake()
	{
		NPCFactory.Instance.Spawn();
	}
}