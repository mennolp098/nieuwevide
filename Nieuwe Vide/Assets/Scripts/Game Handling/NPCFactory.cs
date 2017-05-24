using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFactory : MonoBehaviour {

	//The various skins for an Npc.
	public RuntimeAnimatorController[] npcSkins = new RuntimeAnimatorController[5];

	//Just a placeholder for Skins.
	public Sprite[] sprites = new Sprite[12];

	//The NPC Object to Build.
	private GameObject _npcGO;

	private SpriteRenderer _spriteRenderer;
	private Animator _npcAnimator;
	private BoxCollider2D _boxCollider2D;

	private int _orderInLayer = -1;

	/// <summary>
	/// Builds a new npc.
	/// </summary>
	/// <returns>The new npc.</returns>
	public GameObject BuildNewNpc<T>() where T : Human
	{
		_npcGO = new GameObject("NPC");
		
		_spriteRenderer = _npcGO.AddComponent<SpriteRenderer>() as SpriteRenderer;
		_npcAnimator = _npcGO.AddComponent<Animator>() as Animator;
		_boxCollider2D = _npcGO.AddComponent<BoxCollider2D>() as BoxCollider2D;
		T npcScript = _npcGO.AddComponent<T>();

		_spriteRenderer.sortingOrder = 1;
		_spriteRenderer.sprite = sprites[ Random.Range(0, sprites.Length) ] as Sprite;
		_npcAnimator.runtimeAnimatorController = npcSkins[ Random.Range(0, npcSkins.Length) ] as RuntimeAnimatorController;
		_boxCollider2D.isTrigger = true;

		return _npcGO;
	}

}