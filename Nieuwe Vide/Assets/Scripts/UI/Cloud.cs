using UnityEngine;
using UnityEngine.UI;

public class Cloud : MonoBehaviour {
    private const float xOffset = -6.5f;
    private const float yOffset = -4.5f;

    public bool IsAvailable
    {
        get
        {
            return (_currentTransform == null);
        }
    }

    private Transform _currentTransform;
    private RectTransform _rectTransform;
    private Vector2 normalSize;

    /// <summary>
    /// Use this cloud and lock it to a transform.
    /// </summary>
    public void UseCloud(Transform obj, float duration = -1, float scaleMultiplier = -1)
    {
        _currentTransform = obj;
        this.gameObject.SetActive(true);
        if (duration != -1)
        {
            Invoke("DisableCloud", duration);
        }

        if (scaleMultiplier != -1)
        {
            this._rectTransform.sizeDelta = normalSize * scaleMultiplier;
        }

        if (_currentTransform != null)
        {
            _rectTransform.anchoredPosition = Camera.main.WorldToScreenPoint(_currentTransform.transform.position + new Vector3(xOffset, yOffset, 0));
        }
    }

    private void Awake()
    {
        if (_rectTransform == null)
            _rectTransform = GetComponent<RectTransform>();

        normalSize = this._rectTransform.sizeDelta;
    }

    private void DisableCloud()
    {
        this.gameObject.SetActive(false);
        this._currentTransform = null;
    }

    private void Update()
    {
        if (_currentTransform != null)
        {
            _rectTransform.anchoredPosition = Camera.main.WorldToScreenPoint(_currentTransform.transform.position + new Vector3(xOffset, yOffset, 0));
        }
        else if (this.gameObject.activeSelf)
        {
            this.gameObject.SetActive(false);
        }
    }
}
