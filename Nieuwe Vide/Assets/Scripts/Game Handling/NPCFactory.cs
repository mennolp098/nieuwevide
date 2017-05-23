using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFactory : MonoBehaviour {

	//The various skins for an Npc.
	public RuntimeAnimatorController[] npcSkins;

	//Just a placeholder for Skins.
	public Sprite[] sprites;

	//The NPC Object to Build.
	private GameObject _npcGO;

	private SpriteRenderer _spriteRenderer;
	private Animator _npcAnimator;
	private BoxCollider2D _boxCollider2D;

	private int _orderInLayer = -1 | 1;

	//A Singleton of this script.
	private static NPCFactory _instance = new NPCFactory();

	public static NPCFactory Instance
	{
		get { return _instance; }
	}

	void Awake()
	{
		if(Instance == this) { return; }
		else 
		{
			Destroy(this.gameObject);
		}
	}

	public void Spawn()
	{
		GameObject newlyBuiltNPC = BuildNewNpc();
		GameObject newNPC = Instantiate(newlyBuiltNPC, new Vector2(0, 0), Quaternion.identity) as GameObject;
	}

	/// <summary>
	/// Builds a new npc.
	/// </summary>
	/// <returns>The new npc.</returns>
	private GameObject BuildNewNpc()
	{
		_npcGO = new GameObject();

		_spriteRenderer = _npcGO.AddComponent<SpriteRenderer>();
		_npcAnimator = _npcGO.AddComponent<Animator>();
		_boxCollider2D = _npcGO.AddComponent<BoxCollider2D>();

		_spriteRenderer.sortingOrder = _orderInLayer;
		_spriteRenderer.sprite = sprites[ Random.Range(0, sprites.Length) ];
		_npcAnimator.runtimeAnimatorController = npcSkins[ Random.Range(0, npcSkins.Length) ];
		_boxCollider2D.isTrigger = true;

		return _npcGO;
	}

}