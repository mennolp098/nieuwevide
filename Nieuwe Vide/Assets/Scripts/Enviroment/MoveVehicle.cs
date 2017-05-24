using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveVehicle : MonoBehaviour {
	public float minSpeed = 0.2f;
	public float maxSpeed = 1f;
	private float oldSpeed = 0;
	private float speed = 0;
	private GameObject trafficlight;
	private TrafficLightAI tlai;
	// Use this for initialization
	void Start () {
		oldSpeed = speed = Random.Range(minSpeed,maxSpeed);
		trafficlight = GameObject.Find("trafficlight");

		tlai = trafficlight.GetComponent<TrafficLightAI>();

	}

	
	// Update is called once per frame
	void Update () {
		//move car
		Vector2 newPos = Vector2.right * speed * transform.localScale.x + (Vector2)transform.position;
		transform.position = newPos;


		if(tlai.Color.name == "red"){

			Debug.Log("red found");
			if(transform.position.x < trafficlight.transform.position.x + 2f && transform.position.x > trafficlight.transform.position.x - 2f){
				Debug.Log("position found");
				speed = 0;
			}

		}
		if(speed == 0 && tlai.Color.name == "green"){
			speed = oldSpeed;
		}




	}
}
