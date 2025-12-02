using UnityEngine;

public class ClickableFruit : MonoBehaviour
{
    void OnMouseDown()
    {
        if (FruitCollectorManager.Instance != null)
        {
            FruitCollectorManager.Instance.AttemptCollectFruit(this.gameObject);
        }
    }
}