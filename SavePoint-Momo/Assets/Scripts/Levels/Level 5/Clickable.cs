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
    }

    private void OnMouseDown()
    {
        if (!startGame || found) return;

        mainSc.found(cod, this.gameObject);

        if(cod!=1)GetComponent<SpriteRenderer>().color = Color.red;
        else GetComponent<SpriteRenderer>().color = Color.green;

        found = true;
        Debug.Log("Click");
    }
}
