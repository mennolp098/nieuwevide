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
            else if (_reputation <= 0)
                _reputation = 0;
        }
    }

    private Victim _victimInRange;
    private BoxCollider2D _boxCollider;
    private float _reputation = 100;
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private GameObject _victimIndicator;
    [SerializeField]
    private Sprite _idleSprite;
    [SerializeField]
    private Sprite[] _bullySprites;

    protected override void Initialize()
    {
        //We are not going to set the emotion of the player.
        //base.Initialize();

        _spriteRenderer = GetComponent<SpriteRenderer>();

        // Create circle collider.
        _boxCollider = gameObject.AddComponent<BoxCollider2D>();
        _boxCollider.isTrigger = true;
        _boxCollider.size = new Vector3(Bully.BULLY_RANGE, Bully.BULLY_RANGE, Bully.BULLY_RANGE);
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
            _spriteRenderer.sprite = _bullySprites[Random.Range(0, _bullySprites.Length)];
            UIManager.Instance.CloudsManager.GetAvailableCloud().UseCloud(this.transform, 2);
            GameController.Instance.PlayerSpeed = 0;
            _victimInRange.BullyThis();
            _victimInRange = null;
            _victimIndicator.SetActive(false);
            CancelInvoke("StartMoving");
            Invoke("StartMoving", 1);
        }
    }

    private void StartMoving()
    {
        _spriteRenderer.sprite = _idleSprite;
        GameController.Instance.PlayerSpeed = 2;
    }
}
