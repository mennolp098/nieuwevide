using UnityEngine;

public class Human : MonoBehaviour {
    public bool IsMoving
    {
        get
        {
            return isMoving;
        }
    }

    private int _direction = 1;
    protected bool isMoving;

	void Update ()
    {
        Move();
	}

    protected virtual void Move()
    {

    }
}
