using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enviroment : MonoBehaviour {

    [System.Serializable]
    private struct ObjectData
    {
        public Sprite ObjectSprite;
        public Vector2 Offset;
    }

    /*
    private struct PlacedObjectData
    {
        public GameObject GameObject;
        public Vector2 Offset;
    }*/

    [SerializeField]
    private int totalObjects = 15;

    [SerializeField]
    private int layer = 0;

    [SerializeField]
    private Color _color = new Color(1, 1, 1, 1);

    [SerializeField]
    private float layerSpeedMultiplier = 0;

    [SerializeField]
    private ObjectData[] objects = new ObjectData[2];

    [SerializeField]
    private Vector3 offset = new Vector3(0, 0, 0);

    [SerializeField]
    private Vector2 xRNGRange = new Vector2(0, 0);

    private List<GameObject> placedBGs = new List<GameObject>();
    private GameObject prevPlacedBG;

    
    //Instantiate the first few bg objects
    void Start()
    {
        for(int i = 0; i < totalObjects; i++)
        {
            GameObject bg = new GameObject();
            SpriteRenderer bgRenderer = bg.AddComponent<SpriteRenderer>();
            bgRenderer.color = _color;
            bgRenderer.sortingOrder = layer;
            var data = GetRandomObject();
            Sprite sprite = data.ObjectSprite;
            Vector3 addedOffset = data.Offset;

            bgRenderer.sprite = sprite;
            bg.transform.parent = this.transform;
            if(prevPlacedBG == null)
            {
                bg.transform.position = new Vector3(0, 0, 0) + offset + addedOffset;
            }
            else
            {
                bg.transform.position = new Vector3(GetXPos(prevPlacedBG, bg) + XRng(), 0, 0) + offset + addedOffset;
            }
            EnviromentMover em = bg.AddComponent<EnviromentMover>();
            em.speedMultiplier = layerSpeedMultiplier;
            placedBGs.Add(bg);
            bg.name = this.name+ " " + sprite.name;
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
                Vector3 addedOffset = ChangeSprite(go);
                go.transform.position = new Vector3(GetXPos(prevPlacedBG, go) + XRng(), 0, 0) + offset + addedOffset;
                go.name = this.name + " " + go.GetComponent<SpriteRenderer>().sprite.name;        //I'm so lazy. Could probably find a better naming method.
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
    private ObjectData GetRandomObject()
    {
        return objects[Random.Range(0, objects.Length)];
    }
    private float XRng()
    {
        return (xRNGRange.x + Random.value * xRNGRange.y) - xRNGRange.x;
    }

    /// <summary>
    /// Change the sprite, Returns offset of sprite.
    /// </summary>
    /// <param name="_go"></param>
    /// <returns></returns>
    private Vector2 ChangeSprite(GameObject _go)
    {
        var data = GetRandomObject();
        
        _go.GetComponent<SpriteRenderer>().sprite = data.ObjectSprite;

        return data.Offset;
    }
}
