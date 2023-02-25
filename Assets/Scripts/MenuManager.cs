using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager: MonoBehaviour
{
    public static MenuManager instance;

    [Header ("Buttons")]
    [SerializeField] Button playButton;
    [SerializeField] Button optionsButton;
    [SerializeField] Button leaderBoardButton;
    [SerializeField] Button leaderBoardBackButton;
    [SerializeField] Button optionsBackButton;

    [Header("Audio")]
    [SerializeField] List<AudioClip> menuSfx = new List<AudioClip>();
    public static List<AudioClip> menuSFX;
      [SerializeField]AudioSource myTitleMenuThemeMusic;

    [Header("Menus")]
    [SerializeField] GameObject titleMenu, OptionsMenu, LeaderBoardMenu, endGameMenu;
   
    public GameObject   CurrentMenu,previousMenu;
    bool GameStarted = false;
  
    AudioSource myAudioSource;
    public delegate void PlayeButtonPressed();
    public static PlayeButtonPressed onPlayButtonPressed;


    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    //  DontDestroyOnLoad(gameObject);
    //    if (instance == null)
    //    {
    //        instance = this;
    //
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
        SubscribeToButtons();
        titleMenu.SetActive(true);
        menuSFX = new List<AudioClip>(menuSfx);
        GameManager.gameState = GameManager.GameState.MainMenu;
        CurrentMenu = titleMenu;


        myAudioSource = GetComponent<AudioSource>();

        AssistantToTheSceneManager.onCutsceneLoaded += CloseTitleMenu;
       
    }
   
   

    private void SubscribeToButtons()
    {
        playButton.onClick.AddListener(StartGame);
        optionsButton.onClick.AddListener(OpenOptionsMenu);
        leaderBoardButton.onClick.AddListener(OpenLeaderBoard);
        optionsBackButton.onClick.AddListener(OpenPreviousMenu);
        leaderBoardBackButton.onClick.AddListener(OpenPreviousMenu);
        PauseMenuManager.onOptionsClick += OpenOptionsMenu;
    }
    void CloseTitleMenu()
    {
        titleMenu.SetActive(false);
        AssistantToTheSceneManager.onCutsceneLoaded -= CloseTitleMenu;
    }
    void  StartGame()
    {
        // close title menu start game 
        myAudioSource.PlayOneShot(menuSfx[2]);
        Debug.Log("play sword clangg in menuManager");
        GameStarted = true;
       AssistantToTheSceneManager.instance.loadGame();
      

        StartCoroutine(AudioManager.StartFade(myTitleMenuThemeMusic,.5f,0));
        if(onPlayButtonPressed!=null)
        {
        onPlayButtonPressed();

        }
        // play cutscene and 
        // loadscene
        // when scene is loaded timeScale=1
        // kickOff gamePlay
    }
    public  void OpenOptionsMenu()
    {
        Debug.Log("options opened general");
        myAudioSource.PlayOneShot(menuSfx[1]);
        previousMenu = CurrentMenu; // store ref of previous menu for when back button is used
        OpenMenu(OptionsMenu);
    }
   
    void OpenMenu(GameObject menu)
    {
       
        if (CurrentMenu!=null)
        {
            CurrentMenu.SetActive(false);// disable menu clean up
        }
       
        CurrentMenu = menu;
        menu.SetActive(true);
    }
   void  OpenPreviousMenu()
    {
        myAudioSource.PlayOneShot(menuSfx[0]);
        if (!GameStarted)
        {
             if(CurrentMenu!=null)
             {
                 CurrentMenu.SetActive(false);
    
             }
        CurrentMenu = previousMenu;
        OpenMenu(CurrentMenu);
        }

        else if(GameStarted)
        {
            CurrentMenu.SetActive(false);
        }

    }
    void OpenLeaderBoard()
    {
        myAudioSource.PlayOneShot(menuSfx[1]);
        previousMenu = CurrentMenu;
        OpenMenu(LeaderBoardMenu);
        titleMenu.SetActive(false);
    }
    void OpenTitleMenu()
    {
        titleMenu.SetActive(true);

    }

    public void QueueEndGameMenu()
    {
        endGameMenu.SetActive(true);

    }
    public void ExitApp()
    {
        Application.Quit();
    }

}
