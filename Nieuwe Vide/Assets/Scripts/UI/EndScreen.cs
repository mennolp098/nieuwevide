using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour {
    [SerializeField] private Text _statText;

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

        if(endReputation < 10)
        {
            _statText.text += "Well Done!";
        }
        else if(endReputation < 50)
        {
            _statText.text += "Hmm not bad!";
        }
        else
        {
            _statText.text += "That is not nice!";
        }

        //rectTransform.DOAnchorPos(Vector2.zero, 2, true);
    }
}
