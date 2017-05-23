﻿using UnityEngine;

public class Victim : Human
{
    public delegate void OnVictimAction(Victim victim);
    public event OnVictimAction OnVictimBulliedEvent;
    public event OnVictimAction OnVictimEscapedEvent;

    private bool _isBullied;

    /// <summary>
    /// Trigger this when you want to bully this victim.
    /// </summary>
    public void BullyThis()
    {
        if (_isBullied)
            return;

        GetBullied();
    }

    protected override void Initialize()
    {
        base.Initialize();

        var collider = gameObject.AddComponent<BoxCollider2D>();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void BeforeDestroy()
    {
        base.BeforeDestroy();
        CheckEscaped();
    }

    private void CheckEscaped()
    {
        if (!this._isBullied)
        {
            Debug.Log(this.name + " escaped the bullies.");
            if (OnVictimEscapedEvent != null)
                OnVictimEscapedEvent(this);
        }
    }

    private void GetBullied()
    {
        Debug.Log(this.name + " is being bullied!");

        // Set boolean
        _isBullied = true;

        // Set emotion
        SetEmotion(Emotions.Sad);

        _speed = 0;

        //TODO: Play Animation Get Bullied.

        Invoke("MoveAgain", 1);

        // Send event
        if (OnVictimBulliedEvent != null)
            OnVictimBulliedEvent(this);
    }

    private void MoveAgain()
    {
        _speed = 3;
    }
}
