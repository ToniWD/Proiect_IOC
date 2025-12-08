using UnityEngine;

public class Clickable : MonoBehaviour
{
    public static bool startGame = false;
    public int cod = 0;
    public bool found = false;
    public Level5 mainSc;

    private void Start()
    {
        mainSc = GameObject.Find("Level5").gameObject.GetComponent<Level5>();
        Material mat = GetComponent<SpriteRenderer>().material;
        if (mat.HasProperty("_OutlineWidth"))
        {
            mat.SetFloat("_OutlineWidth", 0f);
            mat.SetColor("_OutlineColor", Color.black);
        }
    }

    private void OnMouseDown()
    {
        if (!startGame || found) return;

        int a = mainSc.found(cod, this.gameObject);

        if (cod != 1) GetComponent<SpriteRenderer>().color = Color.red;
        else
        {
            var audio = GameObject.Find("AudioPlayer").GetComponent<AudioManager>();
            GetComponent<SpriteRenderer>().color = Color.green;
            if (a != 1 && !audio.audioSource.isPlaying)
            {
                if (Random.value > 0.5f)
                {
                    audio.PlaySingleAfterCurrent(audio.GetAudio("fruct gasit 1"));
                }
                else
                {
                    audio.PlaySingleAfterCurrent(audio.GetAudio("fruct gasit 2"));
                }
            }
        }

        found = true;
        Debug.Log("Click");
    }

    private void OnMouseEnter()
    {
        Material mat = GetComponent<SpriteRenderer>().material;
        if (mat.HasProperty("_OutlineWidth"))
        {
            mat.SetFloat("_OutlineWidth", 2f);
            mat.SetColor("_OutlineColor", Color.green);
        }
    }

    private void OnMouseExit()
    {
        Material mat = GetComponent<SpriteRenderer>().material;
        if (mat.HasProperty("_OutlineWidth"))
        {
            mat.SetFloat("_OutlineWidth", 0f);
            mat.SetColor("_OutlineColor", Color.black);
        }
    }
}
