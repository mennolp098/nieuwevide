using UnityEngine;

/// <summary>
/// Manages all UI instances.
/// </summary>
public class UIManager : MonoBehaviour {

    /// <summary>
    /// Singleton.
    /// </summary>
    public static UIManager Instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.LogError("UIManager not created!");
                return null;
            }
            return _instance;
        }
    }

    /// <summary>
    /// Instance of endscreen.
    /// </summary>
    public EndScreen EndScreen
    {
        get
        {
            return _endScreen;
        }
    }

    public CloudsManager CloudsManager
    {
        get
        {
            return _cloudsManager;
        }
    }

    [SerializeField] private CloudsManager _cloudsManager;
    [SerializeField] private EndScreen _endScreen;

    private static UIManager _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);

        if (_instance == null)
            _instance = this;
    }


}
