using UnityEngine;
using UnityEngine.Events;

public class ClickGameObject : MonoBehaviour
{

    [Header("Events")]
    public UnityEvent<string, ClickGameObject> doOnClick;
    public string value;

    public AudioClip audioClipOnMouseEnter;


    private float lastPlayTime = -Mathf.Infinity;
    public float cooldown = 5f;

    private GameObject audioPlayer;

    public bool selected = false;
    public static bool stop = false;

    private void Start()
    {
        stop = true;
        audioPlayer = GameObject.Find("AudioPlayer");
    }

    private void OnMouseEnter()
    {
        if (stop) return;
        Material mat = GetComponent<SpriteRenderer>().material;
        if (mat.HasProperty("_OutlineWidth"))
        {
            mat.SetFloat("_OutlineWidth", 7f);
        }

        if (cooldown == -1 || Time.time - lastPlayTime < cooldown)
            return;

        if (audioClipOnMouseEnter && !audioPlayer.GetComponent<AudioManager>().audioSource.isPlaying)
        {
            audioPlayer.GetComponent<AudioManager>().PlaySingle(audioClipOnMouseEnter);
            lastPlayTime = Time.time;
        }
    }

    public void Deselect()
    {
        Material mat = GetComponent<SpriteRenderer>().material;
        if (mat.HasProperty("_OutlineWidth"))
        {
            mat.SetFloat("_OutlineWidth", 0f);
        }
        selected = false;
    }

    private void OnMouseExit()
    {

        if (stop) return;
        if (selected) return;
        Material mat = GetComponent<SpriteRenderer>().material;
        if (mat.HasProperty("_OutlineWidth"))
        {
            mat.SetFloat("_OutlineWidth", 0f);
        }
    }

    private void OnMouseDown()
    {

        if (stop) return;
        Material mat = GetComponent<SpriteRenderer>().material;
        if (mat.HasProperty("_OutlineWidth"))
        {
            mat.SetFloat("_OutlineWidth", 7f);
        }

        selected = true;
        if (doOnClick != null)
        {
            doOnClick.Invoke(value, this);
        }

    }
}
