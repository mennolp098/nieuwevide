using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Hover : MonoBehaviour {
    private float _startY;

    private bool _isGoingUp = false;
	// Use this for initialization
	void Start () {
        _startY = this.transform.position.y;
        GoHover();
	}

    private void GoHover()
    {
        var moveY = _startY + 2;
        if(_isGoingUp)
        {
            _isGoingUp = false;
            moveY = _startY;
        }else
        {
            _isGoingUp = true;
        }
        this.transform.DOMoveY(_startY, 2).OnComplete(() => GoHover());
    }
}
