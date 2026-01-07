using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))] // Folosim Polygon pentru forma de felie de pizza
public class SeasonButton : MonoBehaviour
{
    [Header("Ce anotimp este acest buton?")]
    public SeasonType mySeason;

    // Referinta catre manager ca sa il anuntam
    public TreeLevelManager manager;

    private void OnMouseDown()
    {
        // 1. Verificam daca managerul exista
        if (manager != null)
        {
            // 2. Ii spunem sa schimbe totul la anotimpul MEU
            manager.ChangeSeason(mySeason);
        }
    }
}