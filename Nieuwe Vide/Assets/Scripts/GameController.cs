using UnityEngine;

public class GameController : MonoBehaviour {
    /// <summary>
    /// Singleton
    /// </summary>
    public static GameController Instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.LogError("GameController not created!");
                return null;
            }

            return _instance;
        }
    }

    /// <summary>
    /// NPCFactory in this game.
    /// </summary>
    public NPCFactory NPCFactory
    {
        get
        {
            return _npcFactory;
        }
    }

    /// <summary>
    /// Player in this game.
    /// </summary>
    public Player Player
    {
        get
        {
            return _player;
        }
    }

    /// <summary>
    /// Returns false wheter this game is ended.
    /// </summary>
    public bool IsEnded
    {
        get
        {
            return _isEnded;
        }
    }
    /// <summary>
    /// Returns the speed the level is moving at. 
    /// </summary>
    public float PlayerSpeed
    {
        get
        {
            return _playerSpeed;
        }
        set
        {
            _playerSpeed = value;
        }
    }

    public PlayerStats PlayerStats
    {
        get
        {
            return _playerStats;
        }
    }

    private static GameController _instance;

    [SerializeField] private NPCFactory _npcFactory;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private Player _player;
    [SerializeField] private PlayerStats _playerStats;

    private bool _isEnded = false;
    private float _playerSpeed = 2;

    public void EndGame()
    {
        if (_isEnded)
            return;

        _isEnded = true;

        _uiManager.EndScreen.Show(PlayerStats.taintedSouls, PlayerStats.savedSouls, Mathf.FloorToInt(_player.Reputation));
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);

        if (_instance == null)
            _instance = this;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            EndGame();
        }
    }
}
