using UnityEngine;

public class Player : Human {
    private const string VICTIM_INDICATOR_PATH = "VictimIndicator";

    public Victim Victim
    {
        get
        {
            return _victimInRange;
        }
        set
        {
            Debug.Log("[Player] Victim has been set to: " + value.name);
            _victimInRange = value;
        }
    }

    public float Reputation
    {
        get
        {
            return _reputation;
        }
        set
        {
            _reputation = value;

            // Reputation should not be higher then 100
            if (_reputation >= 100)
                _reputation = 100;
        }
    }

    private Victim _victimInRange;
    private CircleCollider2D _circleCollider;
    private float _reputation = 100;

    [SerializeField]
    private GameObject _victimIndicator;

    protected override void Initialize()
    {
        //We are not going to set the emotion of the player.
        //base.Initialize();

        // Create circle collider.
        _circleCollider = gameObject.AddComponent<CircleCollider2D>();
        _circleCollider.isTrigger = true;
        _circleCollider.radius = Bully.BULLY_RANGE;
    }

    protected override void Update ()
    {
        //base.Update();

        // Check player inputs.
        PlayerInputs();
	}

    private void OnTriggerStay2D(Collider2D other)
    {
        // When victim is in range, change victim to this one.
        var victim = other.GetComponent<Victim>();
        if (victim != null && !victim.IsBullied)
        {
            _victimIndicator.SetActive(true);
            Debug.Log(victim.name + " is now in range to bully");
            this.Victim = victim;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // When victim is out of range and no other victims found.
        var victim = other.GetComponent<Victim>();
        if (victim != null)
        {
            _victimIndicator.SetActive(false);
        }
    }

    private void PlayerInputs()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            TryBully();
        }
    }

    private void TryBully()
    {
        if(_victimInRange != null)
        {
            Debug.Log("Player is now bullying: " + _victimInRange.name);
            UIManager.Instance.CloudsManager.GetAvailableCloud().UseCloud(this.transform, 2);

            _victimInRange.BullyThis();
            _victimInRange = null;
            _victimIndicator.SetActive(false);
        }
    }
}
