using System.Collections;
using UnityEngine;

public class PuzzlePieceLevel6 : MonoBehaviour
{
    public Vector3 barPoz;

    public bool inPlace = false;

    public AudioClip audioClipInPlace;
    public AudioClip audioClipOnMouseEnter;
    public AudioClip audioClipMistake;

    private float lastPlayTime = -Mathf.Infinity;
    public float cooldown = 5f;

    private GameObject audioPlayer;

    public int orderPiece;

    private void Start()
    {
        this.GetComponent<Draggable>().snapPoz = transform.position;

        this.transform.position = barPoz;

        audioPlayer = GameObject.Find("AudioPlayer");
    }

    public void setInPlace()
    {

        if(orderPiece > Level6.currentPiece)
        {
            if (audioClipMistake) audioPlayer.GetComponent<AudioManager>().PlaySingleAfterCurrent(audioClipMistake);
            this.transform.position = barPoz;
            return;
        }
        Debug.Log("In place");
        inPlace = true;

        this.GetComponent<Draggable>().dragOn = false;

        this.GetComponent<BoxCollider2D>().enabled = false;

        this.GetComponent<SpriteRenderer>().sortingOrder -= 1;

        if (audioClipInPlace) audioPlayer.GetComponent<AudioManager>().PlaySingleAfterCurrent(audioClipInPlace);

        cooldown = -1;

        StartCoroutine(remove(audioClipInPlace.length));
    }

    IEnumerator remove(float lenght)
    {
        yield return new WaitForSeconds(lenght);
        Level6.removePuzzlePiece();
    }

    public void MouseEnter()
    {
        if (cooldown == -1 || Time.time - lastPlayTime < cooldown)
            return;

        if (audioClipOnMouseEnter && !audioPlayer.GetComponent<AudioManager>().audioSource.isPlaying)
        {
            audioPlayer.GetComponent<AudioManager>().PlaySingle(audioClipOnMouseEnter);
            lastPlayTime = Time.time;
        }
    }
}
