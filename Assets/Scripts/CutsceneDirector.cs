using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class CutsceneDirector : MonoBehaviour
{
    [SerializeField] Button ContinueButton;
    public delegate void GameStart();
    public static GameStart onGameStart;
    [SerializeField]Canvas mycanvas;
    Animator myAnimator;
    AudioSource myAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
        myAnimator = GetComponent<Animator>();
        MenuManager.onPlayButtonPressed += ActivateCutscene;
        ContinueButton.onClick.AddListener(StartGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ActivateCutscene()
    {
        if(mycanvas!=null)
        {
        mycanvas.enabled = true;
        myAnimator.SetTrigger("PlayCutscene");

        }

    }
    void StartGame()
    {
        // fadeOut
        // once screen is gone unload scene
        // 
        myAudioSource.PlayOneShot(MenuManager.menuSFX[2]);
        Debug.Log("play parry claggg in cutscene director");
        myAnimator.SetTrigger("StartGame");
        // dirty
        mycanvas.enabled = false;
        GameManager.gameState = GameManager.GameState.InGame;
        if(onGameStart!=null)
        {
        onGameStart();
        }
        UnloadCutscene(); // move to animation event for better transition
    }
    public void UnloadCutscene()
    {
        // SceneManager.UnloadSceneAsync("cutscene");
        gameObject.SetActive(false);
        
    }
}
