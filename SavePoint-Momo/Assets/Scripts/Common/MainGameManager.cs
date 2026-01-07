using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameManager : MonoBehaviour
{
    public static MainGameManager instance;

    public bool musicPause = false;

    public GameObject exitMenu;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            var audio = GameObject.Find("AudioPlayer").GetComponent<AudioManager>();

            StartCoroutine(audio.PlayAudioList(new List<string> { "intro", /*"hint replay instr",*/ "start instr 2" }));
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

    public void openCloseMenu()
    {
        exitMenu.SetActive(!exitMenu.activeSelf);
    }

    public void exitApp()
    {
        Application.Quit();
    }
}
