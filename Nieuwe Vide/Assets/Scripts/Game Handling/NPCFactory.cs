using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFactory : MonoBehaviour {
    [System.Serializable]
    public struct NPCData
    {
        public Sprite normalSprite;
        public Sprite sadSprite;
    }


    //The various skins for an Npc.
    public NPCData[] npcData;

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
        _npcGO.transform.localScale = new Vector3(0.35f, 0.35f, 1);

		_spriteRenderer = _npcGO.AddComponent<SpriteRenderer>() as SpriteRenderer;
		//_npcAnimator = _npcGO.AddComponent<Animator>() as Animator;
		_boxCollider2D = _npcGO.AddComponent<BoxCollider2D>() as BoxCollider2D;
		T npcScript = _npcGO.AddComponent<T>();

		_spriteRenderer.sortingOrder = 1;
        //_npcAnimator.runtimeAnimatorController = npcSkins[ Random.Range(0, npcSkins.Length) ] as RuntimeAnimatorController;
        NPCData data = npcData[Random.Range(0, npcData.Length)];
        _spriteRenderer.sprite = data.normalSprite;
        npcScript.SetSprites(data.normalSprite, data.sadSprite);

        _boxCollider2D.isTrigger = true;

		return _npcGO;
	}

}