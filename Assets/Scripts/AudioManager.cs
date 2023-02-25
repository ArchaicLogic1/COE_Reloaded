using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] List<AudioClip> hitSoundfx = new List<AudioClip>();
    [SerializeField] List<AudioClip> miscAudioFX = new List<AudioClip>();
    AudioSource myaudioSource;
    public static AudioManager instance;
    void Start()
    {
        instance = this;
 // if (instance == null)
 // {
 //     instance = this;
 //
 // }
 // else
 // {
 //     Destroy(gameObject);
 // }
        myaudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {   float currentTime = 0;
        float start = audioSource.volume;
       
       
            while (currentTime < duration)
            {
                currentTime += Time.unscaledDeltaTime;
                audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
                yield return null;
            }
            yield break;
    
      
      
        
    }
    public void PlayHitSFX()
    {
        myaudioSource.pitch = Random.Range(.9f, 1.1f);
        myaudioSource.PlayOneShot(hitSoundfx[Random.Range(0,hitSoundfx.Count)]);
    }
    public void playCoinPickup()
    {
        myaudioSource.PlayOneShot(miscAudioFX[0]);
        myaudioSource.pitch = Random.Range(.95f, 1f);
    }
}
