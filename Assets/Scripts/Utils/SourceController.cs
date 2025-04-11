using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourceController : MonoBehaviour
{
    [SerializeField] private AudioSource Source;
    [SerializeField] private AudioClip[] AudioClips;
    
    private List<AudioSource> _sourcesList = new List<AudioSource>();

    public void PlaySound()
    {
        if (Source.isPlaying)
        {
            var newSourceInst = Instantiate(new GameObject().AddComponent<AudioSource>());
            newSourceInst.volume = Source.volume;
            newSourceInst.playOnAwake = false;
            newSourceInst.outputAudioMixerGroup = Source.outputAudioMixerGroup;
            newSourceInst.clip = AudioClips[Random.Range(0, AudioClips.Length)];
            newSourceInst.Play();
            _sourcesList.Add(newSourceInst);
        }
        else
        {
            Debug.Log("sdsdsdsd");
            Source.clip = AudioClips[Random.Range(0, AudioClips.Length)];
            Source.Play();
        }

        RemoveNotPlaying();
    }

    private void RemoveNotPlaying()
    {
        List<AudioSource> toDestroy = new List<AudioSource>();
        for (int i = 0; i < _sourcesList.Count; i++)
        {
            if (!_sourcesList[i].isPlaying)
            {
                toDestroy.Add(_sourcesList[i]);
            }
        }

        foreach (var el in toDestroy)
        {
            _sourcesList.Remove(el);
            Destroy(el.gameObject);
        }
    }
}
