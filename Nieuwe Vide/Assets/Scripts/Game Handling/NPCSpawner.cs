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

	public float spawnWaitTime = 0;

	void Awake()
	{
		_npcFactory = GetComponent<NPCFactory>();
		SpawnNPC();
	}

	private void SpawnNPC()
	{
		GameObject newNPC;

		switch(_spawnDirection)
		{
		case SpawnDirection.Left:
			newNPC = _npcFactory.BuildNewNpc(new Vector2(-4, 0));
			break;
		case SpawnDirection.Right:
			newNPC = _npcFactory.BuildNewNpc(new Vector2(4, 0));
			break;
		}
	}
}