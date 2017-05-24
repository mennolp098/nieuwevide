using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class TrafficLightAI : MonoBehaviour {
	public List<Sprite> sprites;
	public List<Sprite> carSprites;
	public GameObject spriteRenderPrefab;
	private SpriteRenderer carRenderer;
	public int score = 100;
	private SpriteRenderer sr;
	private float carTime;
	private float switchTime;
	private LightColor color;

	private LightColor red;
	private LightColor yellow;
	private LightColor green;

	public LightColor Color{
		get{return color;}
	}
	void Start () {
		
		red = new LightColor("red",0, 5f);
		yellow = new LightColor("yellow", 1, 1f);
		green = new LightColor("green", 2, 8f);
		color = red;
		sr = GetComponent<SpriteRenderer>();

		sr.sprite = sprites[red.index];
		carTime = 0f;
		switchTime = 0f;
		//activeCars = new List<GameObject>();
		carRenderer = spriteRenderPrefab.GetComponent<SpriteRenderer>();
	}
	// Update is called once per frame
	void Update () {
		//generate cars
		generateCar();
		switchLight();

	}
	private void generateCar(){
		carTime += Time.deltaTime;
		if(carTime > 1f){
			carTime = 0f;

			if(UnityEngine.Random.value < 0.2f){
				addCar();
			}

		}
	}
	private void switchLight(){
		switchTime += Time.deltaTime;
		if(switchTime > color.time){
			switch(color.name){
			case "red":
				color = green;
				break;
			case "yellow":
				color = red;
				break;
			case "green":
				color = yellow;
				break;
			}
			sr.sprite = sprites[color.index];
			switchTime = 0f;
		}
	}
	private void addCar(){

		int index = (int) Mathf.Floor(UnityEngine.Random.value*carSprites.Count);
		Debug.Log(index);
		carRenderer.sprite = carSprites[index];
		GameObject go = (GameObject) GameObject.Instantiate(spriteRenderPrefab);
		Destroy(go, 3);

		if(UnityEngine.Random.value > 0.5f){
			//left
			go.transform.position = new Vector2(-13f,-4.5f);
		}else{
			//right
			go.transform.position = new Vector2(10f,-4.5f);
			Vector2 newScale =  go.transform.localScale;
			newScale.x *=-1;
			go.transform.localScale = newScale;
		//	Debug.Log("right");
		}
	}
}
public class LightColor
{
	public string name;
	public int index;
	public float time;
	public LightColor(string n, int i, float t){
		name = n;
		index = i;
		time = t;
	}

}
