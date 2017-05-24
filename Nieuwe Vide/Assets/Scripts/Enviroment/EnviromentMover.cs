using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentMover : MonoBehaviour {

    public float speed = 3;
	void Update () {
        this.transform.position += new Vector3(-Time.deltaTime * speed, 0, 0);
	}
}
