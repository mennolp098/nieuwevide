using UnityEngine;
using DG.Tweening;

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

	public int Direction
	{
		get
		{
			return _direction;
		}
		set
		{
			_direction = value;
			_width = _direction;
		}
	}

    protected bool _isMoving;
    protected int _direction = -1;

    [SerializeField]
    protected float _speed = 1;

    protected float _width;
    protected float _height;
    protected Vector2 _startScale;
    private float _startY;
    protected bool _gettingBigger = false;

    private SpriteRenderer _spriteRenderer;
    protected Sprite _sadSprite;
    protected Sprite _normalSprite;
    protected AudioAsset _sadSound;

    public void SetSprites(Sprite sadSprite, Sprite normalSprite)
    {
        _sadSprite = sadSprite;
        _normalSprite = normalSprite;
    }

    private void Start()
    {
        _sadSound = Resources.Load("SadSound", typeof(AudioAsset)) as AudioAsset;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _width = _spriteRenderer.bounds.size.x;
        _height = _spriteRenderer.bounds.size.y;
        _isMoving = true;
        _startScale = this.transform.localScale;
        _startY = this.transform.position.y;
        Bop();

        //TODO: Play animation Walk

        Initialize();
    }

    protected virtual void Initialize()
    {
    }

    protected virtual void Update()
    {
        if (_isMoving)
            Move();
    }

    protected virtual void Bop()
    {
        var newScale = _startScale * 0.9f;
        var jumpY = _startY + 0.1f;
        if(_gettingBigger)
        {
            newScale = _startScale;
            jumpY = _startY;
            _gettingBigger = false;
        }
        else
        {
            _gettingBigger = true;
        }
        transform.DOScaleY(newScale.y, 0.25f);
        transform.DOMoveY(jumpY, 0.25f).OnComplete(() => Bop());
    }

    /// <summary>
    /// Movement of the character.
    /// </summary>
    protected virtual void Move()
    {
        transform.position += new Vector3(1, 0, 0) * _direction * _speed * Time.deltaTime;

        CheckDestroy();
    }

    protected virtual void CheckDestroy()
    {
        if (this.transform.position.x <= -10 && this._direction == -1 ||
        this.transform.position.x >= 10 && this._direction == 1)
        {
            BeforeDestroy();
            Destroy(this.gameObject);
        }
    }

    protected virtual void BeforeDestroy()
    {

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
                //TODO: Display angry emotion.
                break;
            case Emotions.Happy:
                //TODO: Display happy emotion.
                break;
            case Emotions.Sad:
                _spriteRenderer.sprite = _sadSprite;
                AudioManager.Instance.Play(_sadSound);
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
