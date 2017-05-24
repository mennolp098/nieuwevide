using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayAudioOnStart : MonoBehaviour {
    [SerializeField] private AudioAsset audioAsset;
    [SerializeField] private bool loop;
    [SerializeField] private bool fadeOnSceneSwitch;

    private Scene currentScene;

	void Start () {
        currentScene = SceneManager.GetActiveScene();
        AudioManager.Instance.Play(audioAsset, loop, false);
        if(fadeOnSceneSwitch)
        {
            SceneManager.sceneUnloaded += SceneUnloaded;
        }
	}

    private void SceneUnloaded(Scene arg0)
    {
         AudioManager.Instance.FadeOut(audioAsset);
    }
}
