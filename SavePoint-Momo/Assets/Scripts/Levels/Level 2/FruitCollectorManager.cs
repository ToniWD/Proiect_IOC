using UnityEngine;
using UnityEngine.UI;

public class FruitCollectorManager : MonoBehaviour
{
    [Header("Configurare Nivel")]
    public int targetFruits = 5;      
    private int currentFruits = 0;   
    private bool levelCompleted = false;

    [Header("Referințe UI")]
    public Text counterDisplay;      
    public GameObject winMessageObject; 
    public GameObject warningTextObject; 
    public static FruitCollectorManager Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateUI();
        if (winMessageObject) winMessageObject.SetActive(false);
        if (warningTextObject) warningTextObject.SetActive(false);
    }

    public void AttemptCollectFruit(GameObject fruitObject)
    {
        if (levelCompleted) return;

        if (currentFruits >= targetFruits)
        {
            Debug.Log("Miau! Ai ales prea multe fructe. Cosul e deja plin!");
            ShowWarning(); 
            return; 
        }

        currentFruits++;

        fruitObject.SetActive(false);

        UpdateUI();
        Debug.Log("Fruct colectat! Total: " + currentFruits);

        if (currentFruits == targetFruits)
        {
            WinLevel();
        }
    }

    void UpdateUI()
    {
        if (counterDisplay != null)
        {
            counterDisplay.text = currentFruits.ToString() + " / " + targetFruits.ToString();
        }
    }

    void WinLevel()
    {
        levelCompleted = true;
        Debug.Log("Nivel Complet! Iepurasul e fericit.");

        if (winMessageObject)
            winMessageObject.SetActive(true);
    }

    void ShowWarning()
    {
        if (warningTextObject)
        {
            warningTextObject.SetActive(true);
            CancelInvoke("HideWarning"); 
            Invoke("HideWarning", 2.0f); 
        }
    }

    void HideWarning()
    {
        if (warningTextObject) warningTextObject.SetActive(false);
    }
}