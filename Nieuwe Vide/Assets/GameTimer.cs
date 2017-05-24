using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour {
    [SerializeField] private Text _timerText;
    [SerializeField][Range(0,180)] private float _timeToPlay;

    private float _currentTime = 0;
	
    private void Awake()
    {
        _currentTime = _timeToPlay;
        StartCoroutine(StartTimer());
    }

    private IEnumerator StartTimer()
    {
        while(_currentTime > 0)
        {
            _currentTime -= 1;
            var timeInMinutes = Mathf.Floor(_currentTime / 60);
            var timeInSeconds = _currentTime - (timeInMinutes * 60);

            _timerText.text = timeInMinutes + ":" + timeInSeconds;
            yield return new WaitForSeconds(1);
        }

        GameController.Instance.EndGame();
        yield break;
    }
}
