using UnityEngine;
using DG.Tweening;

public class MoveOnAwake : MonoBehaviour {

	private void Awake()
    {
        var startPosition = this.transform.position;
        this.transform.position += new Vector3(0, 100, 0);
        this.transform.DOMove(startPosition, 1, false);
    }
}
