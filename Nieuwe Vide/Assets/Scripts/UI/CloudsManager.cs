using UnityEngine;
using UnityEngine.UI;

public class CloudsManager : MonoBehaviour {
    [SerializeField] private Cloud[] clouds;
	
    /// <summary>
    /// Returns a available cloud.
    /// </summary>
    /// <returns>Available cloud</returns>
    public Cloud GetAvailableCloud()
    {
        for (int i = 0; i < clouds.Length; i++)
        {
            if(clouds[i].IsAvailable)
            {
                return clouds[i];
            }
        }

        Debug.Log("No clouds available");
        return null;
    }
}
