using UnityEngine;
using UnityEngine.InputSystem;

public class TriangleClick : MonoBehaviour
{
    
    public AudioSource audioSourceNegative;
    public AudioSource audioSourcePositive;

    private SpriteRenderer sr;
    public Sprite[] triangles;
    public Sprite clickableSprite;
    public bool isDestroyable;
    private string spriteName;
    
    public float duration = 0.1f;        
    public float magnitude = 0.01f;     
    
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr == null) Debug.LogError("SpriteRenderer missing!");
        int fish = Random.Range(0, triangles.Length);
        sr.sprite = triangles[fish];

       
            isDestroyable = true;
        
    }

    public void ForceClickable()
    {
        sr.sprite = clickableSprite;
        isDestroyable = true;
    }
    
    void Update()
    {
        if (GameManager.instance.isGameOver)
        {
            return;
        }
        
        if (Mouse.current == null) return;

        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                if (isDestroyable)
                {
                    Debug.Log("Clicked " + gameObject.name);
                    
                    GameManager.instance.StopAllSounds();
                    GameManager.instance.generalAudioSource.PlayOneShot(GameManager.instance.audioSourceTriangle);
                    
                    GameManager.instance.RemoveTriangle();
                    
                    Destroy(gameObject);
                    
                    
                }
                else
                {
                    StartCoroutine(Shake());
                    
                    
                    GameManager.instance.StopAllSounds();
                    GameManager.instance.generalAudioSource.PlayOneShot(GameManager.instance.audioSourceWrong);
                }
            }
        }
    }
    
    private System.Collections.IEnumerator PlaySoundAndDestroy()
    {
        sr.enabled = false;
        GetComponent<Collider2D>().enabled = false;

        GameManager.instance.RemoveTriangle();

        audioSourcePositive.PlayOneShot(audioSourcePositive.clip);

        yield return new WaitForSeconds(audioSourcePositive.clip.length);

        Destroy(gameObject);
    }


    
    private System.Collections.IEnumerator Shake()
    {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0f;

        while (elapsed < 0.1f)
        {
            float x = Random.Range(-1f, 1f) * 0.15f;
            float y = Random.Range(-1f, 1f) * 0.15f;

            transform.localPosition = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
