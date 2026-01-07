using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))] 
public class SeasonButton : MonoBehaviour
{
    [Header("Ce anotimp este acest buton?")]
    public SeasonType mySeason;

    
    public TreeLevelManager manager;

    private void OnMouseDown()
    {
        
        if (manager != null)
        {
            
            manager.ChangeSeason(mySeason);
        }
    }
}