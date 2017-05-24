using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enviroment : MonoBehaviour {

    [SerializeField]
    private int layer = 0;

    [SerializeField]
    private Sprite[] background = new Sprite[2];

    [SerializeField]
    private Vector3 offset = new Vector3(0, 0, 0);

    private List<GameObject> placedBGs = new List<GameObject>();
    private GameObject prevPlacedBG;

    //Instantiate the first few bg objects
    void Start()
    {
        for(int i = 0; i < 15; i++)
        {
            GameObject bg = new GameObject();
            SpriteRenderer bgRenderer = bg.AddComponent<SpriteRenderer>();
            bgRenderer.sortingLayerID = layer;
            Sprite sprite = GetRandomBG();
            bgRenderer.sprite = sprite;
            bg.transform.parent = this.transform;
            if(prevPlacedBG == null)
            {
                bg.transform.position = new Vector3(0, 0, 0);
            }
            else
            {
                bg.transform.position = new Vector3(GetXPos(prevPlacedBG, bg), 0, 0) + offset;
            }
            bg.AddComponent<EnviromentMover>();
            placedBGs.Add(bg);
            prevPlacedBG = bg;
        }
    }

    void Update()
    {
        foreach(GameObject go in placedBGs)
        {
            //basically when object is out of screen. Check for resolution?
            if(go.transform.position.x < -20)
            {
                ChangeBG(go);
                go.transform.position = new Vector3(GetXPos(prevPlacedBG, go), 0, 0) + offset;
                prevPlacedBG = go;
            }
        }
    }

    private float GetXPos(GameObject prev, GameObject curr)
    {
        return (prev.transform.position.x + GetWidth(prev) * 0.5f + GetWidth(curr) * 0.5f);
    }

    private float GetWidth(GameObject go)
    {
        Sprite sprite = go.GetComponent<SpriteRenderer>().sprite;
        return sprite.rect.size.x / 100;
    }
    private Sprite GetRandomBG()
    {
        return background[Random.Range(0, background.Length)];
    }

    private void ChangeBG(GameObject _go)
    {
        _go.GetComponent<SpriteRenderer>().sprite = GetRandomBG();
    }
}
