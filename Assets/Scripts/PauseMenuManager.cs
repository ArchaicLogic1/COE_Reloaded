using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class PauseMenuManager : MonoBehaviour
{
    [SerializeField]  TestInputActions playerControls;
    [SerializeField] Button resumeButton, optionsButton, quitButton;

    [SerializeField] GameObject  PauseMenuCanvas;
    InputAction pauseGame;
    public delegate void OptionsClick();
    public static event OptionsClick onOptionsClick;
    public delegate void ResumeGame();
    public static event ResumeGame onResumeGame;
    bool isPaused;
    // Start is called before the first frame update
    private void Awake()
    {
       
        playerControls = new TestInputActions();
        
        resumeButton.onClick.AddListener(Resume);
       optionsButton.onClick.AddListener(Options);

        quitButton.onClick.AddListener(QuitGame);
        PauseMenuCanvas.SetActive( false);
    }
    private void OnEnable()
    {
       

        pauseGame = playerControls.Player.Pause;
        pauseGame.Enable();
        pauseGame.performed += PauseGame;
    }
  
    private void OnDisable()
    {
        pauseGame.Disable();
    }



    public void PauseGame(InputAction.CallbackContext context)
    {
       

            if (!isPaused && GameManager.gameState == GameManager.GameState.InGame)
            {

                GameManager.gameState = GameManager.GameState.Paused;
                Debug.Log(GameManager.gameState);
                Time.timeScale = 0;
                PauseMenuCanvas.SetActive(true);
                isPaused = true;
            }
            else
            {
                Resume();
            }
        
        // subscribe to the input actions required to pause and then trigger pause 
    }
   public  void Resume()
    {
        isPaused = false;
        GameManager.gameState = GameManager.GameState.InGame;
        if(onResumeGame!=null)
        {
            onResumeGame();
        }
        
        PauseMenuCanvas.SetActive(false);
        Time.timeScale = 1;
        
    }
    public void Options()
    {
        
        Debug.Log("optionsTRggeredinPause");
        if(onOptionsClick!=null)
        {
            onOptionsClick();
        }
      
    }
    public void AreYouSure()
    {
        // call pop up manager to popup are You sure?
        // subscribe quitgame to that onclick event
        // the are you sure would sort infront of paused game in event they say cancel
    }
 
   public  void QuitGame()
    {
        GameManager.gameState = GameManager.GameState.MainMenu;

        AssistantToTheSceneManager.instance.QuitToMenu();
       
      
    }
   
}
