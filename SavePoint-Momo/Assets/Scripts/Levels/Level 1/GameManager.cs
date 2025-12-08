using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool isGameOver;
    
    public AudioSource audioSourceEnd;
    public AudioClip endGameClip;
    
    public AudioSource audioSourceIntro;  
    public AudioClip introClip;

    public AudioClip[] rulesClips;
    
    
    public AudioClip audioSourceTriangle;
    public AudioClip audioSourceWrong;
    
    
    public AudioSource generalAudioSource;
    
    [SerializeField]
    private GameObject introPanel; 
    [SerializeField]
    private GameObject gamePanel;
    
    private int triangleCount = 0;
    [SerializeField]
    private ShapeGenerator shapeGenerator;
    
    public Camera mainCamera;
    
    private void Awake()
    {
        instance = this;
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    public void StopAllSounds()
    {
        generalAudioSource.Stop();
        
    }

    public void RemoveTriangle()
    {
        triangleCount++;

        if (triangleCount == shapeGenerator.blueCount)
        {
            EndGame();
        }
    }
    
    private void Start()
    {
        isGameOver = true;

        StartCoroutine(PlayIntroAndRules());
    }


    private System.Collections.IEnumerator PlayIntroAndRules()
    {
    
        if (introPanel != null)
            introPanel.SetActive(true);

        if (audioSourceIntro != null && introClip != null)
            audioSourceIntro.PlayOneShot(introClip);

        yield return new WaitForSeconds(introClip.length + 0.2f);


        foreach (var clip in rulesClips)
        {
            if (clip != null)
            {
                audioSourceIntro.PlayOneShot(clip);
                yield return new WaitForSeconds(clip.length + 0.2f);
            }
        }

        
        if (introPanel != null)
            introPanel.SetActive(false);

        if (gamePanel != null)
        {
            ShapeGenerator.instance.SpawnFish();
            gamePanel.SetActive(true);
        }

        isGameOver = false;
    }



    private void EndGame()
    {
        isGameOver = true;
        Debug.Log("Ai câștigat! Nu mai sunt triunghiuri!");
        
        
        if (audioSourceEnd != null && endGameClip != null)
        {
            StopAllSounds();
            
            generalAudioSource.PlayOneShot(endGameClip);
        }

        StartCoroutine(LoadNextLevelAfterSound());
    }
    
    private System.Collections.IEnumerator LoadNextLevelAfterSound()
    {
        yield return new WaitForSeconds(endGameClip.length);
        
        StopAllCoroutines();

        MainGameManager.play("Level 2");
    }
    
}