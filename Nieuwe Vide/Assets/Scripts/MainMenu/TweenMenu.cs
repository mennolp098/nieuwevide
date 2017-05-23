using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TweenMenu : MonoBehaviour {
    [SerializeField]
    private RectTransform[] tweenObjects;
	
    void Awake()
    {
        for (int i = 0; i < tweenObjects.Length; i++)
        {
            //tweenObjects[i].DOAnchorPos(tweenObjects[i].anchoredPosition - new Vector2(1000, 0), 0);
            tweenObjects[i].anchoredPosition -= new Vector2(1000, 0);
        }

        StartCoroutine(MoveButtons());
    }

    IEnumerator MoveButtons()
    {
        for (int i = 0; i < tweenObjects.Length; i++)
        {
            tweenObjects[i].DOAnchorPos(tweenObjects[i].anchoredPosition + new Vector2(1000, 0), 1);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
