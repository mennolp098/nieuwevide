using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFactory : MonoBehaviour {

	//The various skins for an Npc.
	public RuntimeAnimatorController[] npcSkins = new RuntimeAnimatorController[5];

	//Just a placeholder for Skins.
	public Sprite sprite;

	//The NPC Object to Build.
	private GameObject _npcGO;

	private SpriteRenderer _spriteRenderer;
	private Animator _npcAnimator;
	private BoxCollider2D _boxCollider2D;
	private Victim _victimScript;
//	private Rigidbody2D _rigidbody2D;

	private int _orderInLayer = -1 | 1;

	/// <summary>
	/// Builds a new npc.
	/// </summary>
	/// <returns>The new npc.</returns>
	public GameObject BuildNewNpc(Vector2 spawnPosition)
	{
		_npcGO = new GameObject();
		_npcGO.name = "Victim NPC";

		_npcGO.transform.position = spawnPosition;

		_spriteRenderer = _npcGO.AddComponent<SpriteRenderer>() as SpriteRenderer;
		_npcAnimator = _npcGO.AddComponent<Animator>() as Animator;
		_boxCollider2D = _npcGO.AddComponent<BoxCollider2D>() as BoxCollider2D;
//		_rigidbody2D = _npcGO.AddComponent<Rigidbody2D>() as Rigidbody2D;

		_spriteRenderer.sortingOrder = _orderInLayer;
		_spriteRenderer.sprite = sprite as Sprite;
		_npcAnimator.runtimeAnimatorController = npcSkins[ Random.Range(0, npcSkins.Length) ] as RuntimeAnimatorController;
		_boxCollider2D.isTrigger = true;

		return _npcGO;
	}

}