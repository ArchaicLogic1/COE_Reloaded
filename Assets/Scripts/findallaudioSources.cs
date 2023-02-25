using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class findallaudioSources : MonoBehaviour
{

    AudioSource[] myAudioSources;
  [SerializeField] AudioMixerGroup mainMixer;
    // Start is called before the first frame update
    void Start()
    {
        myAudioSources = FindObjectsOfType<AudioSource>();
        foreach(AudioSource audioSource in myAudioSources)
        {
            audioSource.outputAudioMixerGroup = mainMixer;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
