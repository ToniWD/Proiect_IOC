using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class AudioInstruction
{
    public string name;      // identificator
    public AudioClip clip;   // fisierul audio
}

public class AudioManager : MonoBehaviour
{
    public List<AudioInstruction> instructiuni;
    public AudioSource audioSource;

    public AudioClip GetAudio(string name)
    {
        AudioInstruction instr = instructiuni.Find(i => i.name == name);
        if (instr != null && instr.clip != null)
        {
            return instr.clip;
        }
        else
        {
            Debug.LogWarning("Not found audio clip for: " + name);
            return null;
        }
    }

    public void PlaySingle(AudioClip clip)
    {
        if (clip == null) return;

        audioSource.Stop();       
        audioSource.clip = clip;
        audioSource.Play();      
    }

    public void PlaySingleAfterCurrent(AudioClip clip)
    {
        if (clip == null) return;

        StartCoroutine(PlayAfterCurrentCoroutine(clip));
    }

    private IEnumerator PlayAfterCurrentCoroutine(AudioClip clip)
    {
        while (audioSource.isPlaying)
        {
            yield return null;
        }

        audioSource.clip = clip;
        audioSource.Play();
    }
}
