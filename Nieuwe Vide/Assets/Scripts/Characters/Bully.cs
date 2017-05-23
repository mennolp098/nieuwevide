using UnityEngine;
using System.Collections.Generic;
using System;
using DG.Tweening;

public class Bully : Human {
    /// <summary>
    /// Add this when you bully a person.
    /// </summary>
    public const float BULLY_REPUTATION_GOOD = 10;

    /// <summary>
    /// Min this when you don't bully a person.
    /// </summary>
    public const float BULLY_REPUTATION_BAD = 5;

    public const float BULLY_RANGE = 5;

    [SerializeField]
    private Player _currentPlayer;
    [SerializeField]
    private Victim[] victims;

    private float _maxHorizontalOffset = 10;
    private float _currentHorizontalOffset = 0;
    private Vector3 _startPosition;
    private CircleCollider2D _circleCollider;

    /// <summary>
    /// Adds a victim to the list of potential victims.
    /// </summary>
    public void AddVictim(Victim item)
    {
        item.OnVictimBulliedEvent += VictimBulliedEventHandler;
        item.OnVictimEscapedEvent += VictimEscapedEventHandler;
    }

    /// <summary>
    /// Removes a victim to the list of potential victims.
    /// </summary>
    public void RemoveVictim(Victim item)
    {
        item.OnVictimBulliedEvent -= VictimBulliedEventHandler;
        item.OnVictimEscapedEvent -= VictimEscapedEventHandler;
    }

    /// <summary>
    /// Sets the player for this instance.
    /// </summary>
    /// <param name="item"></param>
    public void SetPlayer(Player item)
    {
        _currentPlayer = item;
    }

    private void VictimBulliedEventHandler(Victim victim)
    {
        SetEmotion(Emotions.Happy);
        _currentPlayer.Reputation += BULLY_REPUTATION_GOOD;
    }

    private void VictimEscapedEventHandler(Victim victim)
    {
        SetEmotion(Emotions.Angry);
        _currentPlayer.Reputation -= BULLY_REPUTATION_BAD;
    }

    protected override void Update()
    {
        base.Update();
        CheckPlayerReputation();
    }

    protected override void Initialize()
    {
        for (int i = 0; i < victims.Length; i++)
        {
            AddVictim(victims[i]);
        }

        //base.Initialize();
        _startPosition = this.transform.position;
        SetEmotion(Emotions.Happy);

        _circleCollider = gameObject.AddComponent<CircleCollider2D>();
        _circleCollider.isTrigger = true;
        _circleCollider.radius = BULLY_RANGE;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // When victim is in range, change victim to this one.
        var victim = other.GetComponent<Victim>();
        if (victim != null)
        {
            BullyVictim(victim);
        }
    }

    private void BullyVictim(Victim victim)
    {
        //TODO: show bully animation
        //TODO: lookat bully

        Debug.Log(this.name + " is now bullying: " + victim.name);
    }

    protected override void Move()
    {
        //base.Move();
        this.transform.DOMove(_startPosition + new Vector3(_currentHorizontalOffset, 0, 0), 10);
    }

    private void CheckPlayerReputation()
    {
        // Reputation goes from 0 - 100
        // So offset on 100 reputation should be 0
        // and on 0 reputation it should be max horizontal offset.
        var calculatedOffset = 0.0f;
        if (_currentPlayer.Reputation != 100f)
        {
            calculatedOffset = (100f - _currentPlayer.Reputation) / 100f * _maxHorizontalOffset;
        }
        
        if (_currentHorizontalOffset != calculatedOffset)
        {
            Debug.Log("Horizontal offset has been changed to: " + _currentHorizontalOffset);
            _currentHorizontalOffset = calculatedOffset;
            Move();
        } 
    }


}
