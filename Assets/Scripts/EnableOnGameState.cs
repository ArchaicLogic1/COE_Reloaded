using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnGameState : MonoBehaviour
{
    public AudioSource myAudio;
    public AudioClip bossTrack;
    EnableOnGameState instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    //  DontDestroyOnLoad(gameObject);
    //  if (instance == null) { instance = this; }
    //  else { Destroy(gameObject); }

        myAudio = GetComponent<AudioSource>();
        Debug.LogError(myAudio.name);
        CutsceneDirector.onGameStart += TurnOnAudio;
        Rotsputen.onBossBattleStart += SwapAudioToBossTrack;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void TurnOnAudio()
    {
        Debug.Log("audio is turned on");
        if(myAudio!=null)
        {
        myAudio.enabled = true;

        }
      
    }
    void SwapAudioToBossTrack()
    {
        myAudio.clip = bossTrack;
        myAudio.Play();
        Rotsputen.onBossBattleStart -= SwapAudioToBossTrack;
    }
}
