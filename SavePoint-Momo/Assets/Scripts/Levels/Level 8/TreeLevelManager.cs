using System.Collections;
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
    public int itemsNeededToWin = 4; // Atentie: In inspector ai pus 4!

    // Variabile de stare
    public bool springDone = false;
    public bool summerDone = false;
    public bool autumnDone = false;
    public bool currentStageFinished = false;

    [Header("Audio Clips")]
    public AudioSource audioSource;
    public AudioClip sunetPovesteStart;
    public AudioClip instructiuniStart;
    public AudioClip sunetSucces;
    public AudioClip sunetEroare;
    public AudioClip sunetFinal;
    public AudioClip sunetPrimavara;
    public AudioClip sunetVara;
    public AudioClip sunetToamna;
    public AudioClip sunetIarna;

    [Header("Legaturi UI")]
    public Transform decorationsContainer;
    public GameObject messagePanel;
    public TMP_Text messageText;
    public GameObject springBG;
    public GameObject summerBG;
    public GameObject autumnBG;
    public GameObject winterBG;

    void Awake()
    {
        Instance = this;
        Debug.Log("Scriptul TreeLevelManager a pornit!"); // VERIFICARE 1
    }

    void Start()
    {
        Debug.Log("Start Level - Setam Primavara"); // VERIFICARE 2
        ChangeSeason(SeasonType.Spring);
        StartCoroutine(PlayStartSequence());
    }

    void Update()
    {
        // Rulam verificarea in fiecare cadru
        CheckProgress();
    }

    void CheckProgress()
    {
        // 1. Verificam daca avem container
        if (decorationsContainer == null)
        {
            Debug.LogError("LIPSA CONTAINER! Trage ActiveDecorations in Inspector!");
            return;
        }

        // 2. Numaram obiectele (SPIONUL)
        int numarObiecte = decorationsContainer.childCount;

        // Afisam mesajul doar daca se schimba ceva, ca sa nu umplem consola degeaba,
        // DAR il afisam fortat daca ai >= 4 obiecte.
        if (numarObiecte >= 1)
        {
            // Debug.Log("Numar curent: " + numarObiecte + " / Tinta: " + itemsNeededToWin);
        }

        // 3. Verificam victoria
        if (!currentStageFinished && numarObiecte >= itemsNeededToWin)
        {
            Debug.Log("!!! VICTORIE DETECTATA !!! Avem " + numarObiecte + " flori.");
            StageComplete();
        }
    }

    void StageComplete()
    {
        Debug.Log("Executam StageComplete()...");
        currentStageFinished = true;

        if (CurrentSeason == SeasonType.Spring) springDone = true;
        if (CurrentSeason == SeasonType.Summer) summerDone = true;
        if (CurrentSeason == SeasonType.Autumn) autumnDone = true;

        // ACTIVAM MESAJUL VIZUAL
        if (messagePanel != null)
        {
            messagePanel.SetActive(true);
            Debug.Log("Am activat Panoul de Mesaj (WinMessage).");
        }
        else
        {
            Debug.LogError("NU GASESC Message Panel! Trage WinMessage in Inspector!");
        }

        // REDAM AUDIO
        PlaySound(sunetSucces);

        // SETAM TEXTUL
        if (CurrentSeason == SeasonType.Winter)
        {
            if (messageText != null) messageText.text = "FELICITĂRI!\nAi completat ciclul vieții!";
            PlaySound(sunetFinal);
            // ---> AICI SE APELEAZA SUNETUL FINAL <---
            //AICI E FINALUL (TONY)

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
            Debug.Log("Inca nu ai terminat anotimpul curent! Ai nevoie de " + itemsNeededToWin + " obiecte.");
            PlayErrorSound();
            return;
        }

        if (messagePanel != null) messagePanel.SetActive(false);
        currentStageFinished = false;
        ClearTreeInstant();
        CurrentSeason = newSeason;
        UpdateBackgrounds();

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

    // ... (restul functiilor raman la fel, le pun mai jos comprimate)
    IEnumerator PlayStartSequence()
    {
        if (sunetPovesteStart != null) { PlaySound(sunetPovesteStart); yield return new WaitForSeconds(sunetPovesteStart.length + 0.5f); }
        PlaySound(instructiuniStart);
    }
    public void PlaySound(AudioClip clip) { if (clip != null && audioSource != null) audioSource.PlayOneShot(clip); }
    public void PlayErrorSound() { PlaySound(sunetEroare); }
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