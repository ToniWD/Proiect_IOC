using System.Collections; // <--- LINIE NOUA OBLIGATORIE PENTRU "WAIT"
using TMPro;
using UnityEngine;  

public enum SeasonType
{
    Spring,
    Summer,
    Autumn,
    Winter
}

public class TreeLevelManager : MonoBehaviour
{
    public static SeasonType CurrentSeason;
    public static TreeLevelManager Instance;

    [Header("Setari Joc")]
    public int itemsNeededToWin = 5;

    private bool springDone = false;
    private bool summerDone = false;
    private bool autumnDone = false;
    private bool currentStageFinished = false;

    [Header("Audio Clips (Trage fisierele aici)")]
    public AudioSource audioSource;
    public AudioClip sunetPovesteStart; // <--- AICI PUNEM "NIVEL 8 START"
    public AudioClip instructiuniStart; // "nivel 8 instructiuni"
    public AudioClip sunetSucces;
    public AudioClip sunetEroare;
    public AudioClip sunetFinal;

    [Header("Sunete Anotimpuri")]
    public AudioClip sunetPrimavara;
    public AudioClip sunetVara;
    public AudioClip sunetToamna;
    public AudioClip sunetIarna;

    [Header("Legaturi UI")]
    public Transform decorationsContainer;
    public GameObject messagePanel;
    public TMP_Text messageText;

    [Header("Fundaluri")]
    public GameObject springBG;
    public GameObject summerBG;
    public GameObject autumnBG;
    public GameObject winterBG;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ChangeSeason(SeasonType.Spring);

        // Pornim secventa de start (Poveste -> Asteptare -> Instructiuni)
        StartCoroutine(PlayStartSequence());
    }

    // Aceasta este functia speciala care stie sa astepte
    IEnumerator PlayStartSequence()
    {
        // 1. Redam povestea de start
        if (sunetPovesteStart != null)
        {
            PlaySound(sunetPovesteStart);
            // Calculatorul asteapta exact cat dureaza sunetul
            yield return new WaitForSeconds(sunetPovesteStart.length);

            // Mica pauza de respiratie intre sunete (0.5 secunde)
            yield return new WaitForSeconds(0.5f);
        }

        // 2. Redam instructiunile
        PlaySound(instructiuniStart);
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(clip);
        }
    }

    public void PlayErrorSound()
    {
        PlaySound(sunetEroare);
    }

    void CheckProgress()
    {
        if (currentStageFinished || decorationsContainer == null) return;

        if (decorationsContainer.childCount >= itemsNeededToWin)
        {
            StageComplete();
        }
    }

    void StageComplete()
    {
        currentStageFinished = true;

        if (CurrentSeason == SeasonType.Spring) springDone = true;
        if (CurrentSeason == SeasonType.Summer) summerDone = true;
        if (CurrentSeason == SeasonType.Autumn) autumnDone = true;

        if (messagePanel != null) messagePanel.SetActive(true);

        PlaySound(sunetSucces);

        if (CurrentSeason == SeasonType.Winter)
        {
            if (messageText != null) messageText.text = "FELICITĂRI!\nAi completat ciclul vieții!";
            PlaySound(sunetFinal);
        }
        else
        {
            string nextSeasonName = "";
            if (CurrentSeason == SeasonType.Spring) nextSeasonName = "Vara";
            if (CurrentSeason == SeasonType.Summer) nextSeasonName = "Toamna";
            if (CurrentSeason == SeasonType.Autumn) nextSeasonName = "Iarna";

            if (messageText != null) messageText.text = "Minunat!\nAcum selectează " + nextSeasonName + "!";
        }
    }

    public void ChangeSeason(SeasonType newSeason)
    {
        if ((newSeason == SeasonType.Summer && !springDone) ||
            (newSeason == SeasonType.Autumn && !summerDone) ||
            (newSeason == SeasonType.Winter && !autumnDone))
        {
            Debug.Log("Inca nu ai terminat anotimpul curent!");
            return;
        }

        if (messagePanel != null) messagePanel.SetActive(false);
        currentStageFinished = false;
        ClearTreeInstant();
        CurrentSeason = newSeason;
        UpdateBackgrounds();

        // Evitam sa redam numele anotimpului ("Primavara") fix cand incepe jocul,
        // pentru ca s-ar suprapune cu povestea. Redam doar la schimbare manuala.
        if (Time.time > 1f)
        {
            switch (newSeason)
            {
                case SeasonType.Spring: PlaySound(sunetPrimavara); break;
                case SeasonType.Summer: PlaySound(sunetVara); break;
                case SeasonType.Autumn: PlaySound(sunetToamna); break;
                case SeasonType.Winter: PlaySound(sunetIarna); break;
            }
        }
    }

    void ClearTreeInstant()
    {
        if (decorationsContainer != null)
        {
            var children = new System.Collections.Generic.List<GameObject>();
            foreach (Transform child in decorationsContainer) children.Add(child.gameObject);
            decorationsContainer.DetachChildren();
            foreach (var child in children) Destroy(child);
        }
    }

    void UpdateBackgrounds()
    {
        if (springBG != null) springBG.SetActive(false);
        if (summerBG != null) summerBG.SetActive(false);
        if (autumnBG != null) autumnBG.SetActive(false);
        if (winterBG != null) winterBG.SetActive(false);

        switch (CurrentSeason)
        {
            case SeasonType.Spring: if (springBG != null) springBG.SetActive(true); break;
            case SeasonType.Summer: if (summerBG != null) summerBG.SetActive(true); break;
            case SeasonType.Autumn: if (autumnBG != null) autumnBG.SetActive(true); break;
            case SeasonType.Winter: if (winterBG != null) winterBG.SetActive(true); break;
        }
    }
}