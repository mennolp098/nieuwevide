﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentMover : MonoBehaviour {

    public float speedMultiplier = 0;
    private float speed = 0;
    private GameController gc;

    //too lazy for a delegate
    void Start()
    {
        gc = GameController.Instance;
    }

    void Update () {
        this.speed = gc.PlayerSpeed * speedMultiplier;
        this.transform.position += new Vector3(-Time.deltaTime * speed, 0, 0);
	}
}
