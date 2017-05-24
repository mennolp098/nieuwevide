using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour {
    [SerializeField] private Text _statText;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Image _victoryImage;
    [SerializeField] private Image _bullyImage;
    [SerializeField] private Color _bullyColor;
    [SerializeField] private Color _victoryColor;

    /// <summary>
    /// Shows the end screen.
    /// </summary>
    /// <param name="amountBullied">amount of people bullied in this game.</param>
    /// <param name="amountNotBullied">amount of people ignored in this game.</param>
    /// <param name="endReputation">reputation with the bullies.</param>
    public void Show(int amountBullied, int amountNotBullied, int endReputation)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -1000);
        this.gameObject.SetActive(true);

        _statText.text = "Tainted souls: " + amountBullied + '\n'
            + "Saved souls: " + amountNotBullied + '\n'
            + "Bad reputation: " + endReputation + "/100" + '\n';

        if(endReputation < 10 || amountNotBullied < 2)
        {
            _statText.text += "Well Done!";
            _victoryImage.gameObject.SetActive(true);
            _backgroundImage.color = _victoryColor;
        }
        else if(endReputation < 50)
        {
            _statText.text += "Hmm not bad!";
            _bullyImage.gameObject.SetActive(true);
            _backgroundImage.color = _bullyColor;
        }
        else
        {
            _statText.text += "That is not nice!";
            _bullyImage.gameObject.SetActive(true);
            _backgroundImage.color = _bullyColor;
        }

        rectTransform.DOAnchorPos(Vector2.zero, 2, true);
    }
}
