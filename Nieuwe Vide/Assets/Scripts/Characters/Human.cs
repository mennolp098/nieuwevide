﻿using UnityEngine;

public class Human : MonoBehaviour
{
    public enum Emotions
    {
        Happy = 0,
        Sad,
        Angry
    }

    /// <summary>
    /// Is this character currently moving?
    /// </summary>
    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }
    }

    protected bool _isMoving;
    protected int _direction = -1;
    [SerializeField]
    protected float _speed;

    protected float _width;
    protected float _height;

    private void Start()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        _width = spriteRenderer.bounds.size.x;
        _height = spriteRenderer.bounds.size.y;
        _isMoving = true;

        //TODO: Play animation Walk

        Initialize();
    }

    protected virtual void Initialize()
    {
        SetEmotion(Random.Range(0, 3));
    }

    protected virtual void Update()
    {
        if (_isMoving)
            Move();
    }

    /// <summary>
    /// Movement of the character.
    /// </summary>
    protected virtual void Move()
    {
        transform.position += new Vector3(1, 0, 0) * _direction * _speed * Time.deltaTime;
    }

    /// <summary>
    /// Sets the emotion of this human.
    /// </summary>
    /// <param name="emotion">The emotion it should display.</param>
    protected void SetEmotion(Emotions emotion)
    {
        switch (emotion)
        {
            case Emotions.Angry:
                Debug.Log(this.name + " is now Angry!");
                //TODO: Display angry emotion.
                break;
            case Emotions.Happy:
                Debug.Log(this.name + " is now Happy!");
                //TODO: Display happy emotion.
                break;
            case Emotions.Sad:
                Debug.Log(this.name + " is now Sad!");
                //TODO: Display sad emotion.
                break;
        }
    }

    /// <summary>
    /// Sets the emotion of this human.
    /// </summary>
    /// <param name="emotion">The emotion it should display as intiger in range of enum.</param>
    protected void SetEmotion(int emotion)
    {
        SetEmotion((Emotions)emotion);
    }
}
