using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SequenceCharacters : MonoBehaviour {
    [SerializeField] Text _text;
	
    IEnumerator Sequence()
    {
        string allCharacters = _text.text;
        _text.text = "";
        for (int i = 0; i < allCharacters.Length; i++)
        {
            yield return new WaitForSeconds(Time.maximumDeltaTime*0.25f);
            _text.text += allCharacters[i];
        }

        yield break;
    }

    private void OnEnable()
    {
        StartCoroutine(Sequence());
    }
}
