﻿using UnityEngine;
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

    /// <summary>
    /// The range when they will bully the victim.
    /// </summary>
    public const float BULLY_RANGE = 15;

    private Player _player;

    private bool _isBullying = false;
    private float _maxHorizontalOffset = 10;
    private float _currentHorizontalOffset = 0;
    private Vector3 _startPosition;
    private BoxCollider2D _boxCollider;
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private Sprite _idleSprite;
    [SerializeField]
    private Sprite _bullySprite;
    [SerializeField]
    private AudioAsset _bullySound;

    private void VictimBulliedEventHandler(Victim victim)
    {
        SetEmotion(Emotions.Happy);
        _player.Reputation += BULLY_REPUTATION_GOOD;
		GameController.Instance.PlayerStats.AddTainted();
    }

    private void VictimEscapedEventHandler(Victim victim)
    {
        SetEmotion(Emotions.Angry);
        _player.Reputation -= BULLY_REPUTATION_BAD;
        GameController.Instance.PlayerStats.AddSaved();
    }

    protected override void Update()
    {
        base.Update();
        CheckPlayerReputation();
    }

    protected override void Initialize()
    {
        //base.Initialize();

        _player = GameController.Instance.Player;
        _startPosition = this.transform.position;
        SetEmotion(Emotions.Happy);

        _spriteRenderer = GetComponent<SpriteRenderer>();

        _boxCollider = gameObject.AddComponent<BoxCollider2D>();
        _boxCollider.isTrigger = true;
        _boxCollider.size = new Vector3(BULLY_RANGE,BULLY_RANGE,BULLY_RANGE);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // When victim is in range, change victim to this one.
        var victim = other.GetComponent<Victim>();
        if (victim != null)
        {
            AddVictim(victim);
            BullyVictim(victim);
        }
    }

    /// <summary>
    /// Adds a victim to the list of potential victims.
    /// </summary>
    private void AddVictim(Victim item)
    {
        item.OnVictimBulliedEvent += VictimBulliedEventHandler;
        item.OnVictimEscapedEvent += VictimEscapedEventHandler;
    }

    private void BullyVictim(Victim victim)
    {
        //TODO: show bully animation
        //TODO: lookat bully
        if (_isBullying)
            return;

        _spriteRenderer.sprite = _bullySprite;

        AudioManager.Instance.Play(_bullySound, false);

        _isBullying = true;
        UIManager.Instance.CloudsManager.GetAvailableCloud().UseCloud(this.transform, 1, 0.5f);
        CancelInvoke("StopBullying");
        Invoke("StopBullying", 1);
    }

    private void StopBullying()
    {
        _spriteRenderer.sprite = _idleSprite;
        _isBullying = false;
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
        if (_player.Reputation != 100f && _player.Reputation > 0)
        {
            calculatedOffset = (100f - _player.Reputation) / 100f * _maxHorizontalOffset;
        }
        else if(_player.Reputation <= 0)
        {
            GameController.Instance.EndGame();
        }
        
        if (_currentHorizontalOffset != calculatedOffset)
        {
            _currentHorizontalOffset = calculatedOffset;
            Move();
        } 
    }


}
