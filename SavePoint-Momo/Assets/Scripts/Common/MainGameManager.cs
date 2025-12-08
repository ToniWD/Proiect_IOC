using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameManager : MonoBehaviour
{
    public static MainGameManager instance;

    public bool musicPause = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            var audio = GameObject.Find("AudioPlayer").GetComponent<AudioManager>();
            audio.PlaySingle(audio.GetAudio("intro"));
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void play(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void musicSwitch()
    {
        if (!musicPause)
        {
            this.GetComponent<AudioSource>().Pause();
            musicPause = true;
        }
        else
        {
            this.GetComponent<AudioSource>().UnPause();
            musicPause = false;
        }
    }
}
