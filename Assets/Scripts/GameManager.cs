using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manages the game. (score, Ui, collectables data)
/// </summary>
public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public enum GameState { MainMenu, CutScene, Paused, InGame, QuitGame }
    public static GameState gameState;
    [SerializeField] GameObject player;
    [SerializeField] public int score;
    [SerializeField] TMP_Text scoreText, Timer;
   public static GameManager instance;
    [SerializeField] Image healthUI, healthbarDamageTell;
        [SerializeField] float playerHealth=1000, maxHealth=1000;

    public delegate void DamageTaken();
    public static event DamageTaken onDamageTaken;
    [SerializeField]Canvas myCanvas;
    [SerializeField]List<GameObject> floatingPoints = new List<GameObject>();
    int floatingPointListIndexCounter;
    float healthBarDamping = .02f;
    [SerializeField]GameObject gameOver;
    private void Awake()
    {
        instance = this;
     //   DontDestroyOnLoad(gameObject);
    // if (instance == null)
    // {
    //     instance = this;
    //
    // }
    // else
    // {
    //     Destroy(gameObject);
    // }

        gameState = new GameState();
    }
    void Start()
    {
        gameState = GameState.InGame;
        playerHealth = 1000;
        maxHealth = 1000;
        // EnablePlayerController();
        healthUI.fillAmount = playerHealth / maxHealth;
        if (gameState != GameState.InGame) 
        {
         //   myCanvas.enabled = false;
            Time.timeScale = 0;
        }

        CutsceneDirector.onGameStart += StartGame;
        //  PauseMenuManager.onResumeGame += EnablePlayerController;


    }

    // Update is called once per frame
    void Update()
    {
       scoreText.text = score.ToString();

        if (healthbarDamageTell.fillAmount != playerHealth / maxHealth)
        {
            healthbarDamageTell.fillAmount = Mathf.Lerp(healthbarDamageTell.fillAmount, playerHealth/ maxHealth, healthBarDamping);
        }

    }
    private void FixedUpdate()
    {
       // Timer.text = Time.time.ToString("HH:mm:ss");
    }
    void StartGame()
    {


       
     
        //  EnablePlayerController();
        Time.timeScale = 1;
    }
    private void EnablePlayerController()
    {
        Debug.Log("current gamestate :" + gameState);
        bool _isInGame = gameState == GameState.InGame ? true : false;
        player.SetActive(_isInGame);

    }
    public  void TakeDamage(int damageAmt)
    {
        Debug.Log(healthUI.fillAmount);
        Debug.Log(damageAmt);
        Debug.Log("takeDamage called");
      playerHealth-= damageAmt;
        if (playerHealth <= 0)
        {
            Time.timeScale = 0;
            gameOver.SetActive(true);
        }
        healthUI.fillAmount =  (playerHealth / maxHealth);
        Debug.Log(playerHealth / maxHealth);
        Debug.Log(healthUI.fillAmount);
        if (onDamageTaken!=null)
        {

        onDamageTaken();
        }
        

    }
    public void AwardPlayerPoints(int pointValue, Vector2 location)
    {
        if(floatingPointListIndexCounter>floatingPoints.Count-1)
        {
            floatingPointListIndexCounter = 0;
           
        }
        score += pointValue;
       
        
        // To Do : run coroutine to count up points to make it more visually exciting
      
        GameObject floatingPt = floatingPoints[floatingPointListIndexCounter];
       floatingPt.SetActive(true);
        TMP_Text PtText = floatingPt.GetComponentInChildren<TMP_Text>();
        PtText.text = pointValue.ToString();
        floatingPoints[floatingPointListIndexCounter].transform.position = location;
        floatingPointListIndexCounter++;
        StartCoroutine(DeactivateFloatingPoint(floatingPt));



    }
    IEnumerator DeactivateFloatingPoint(GameObject floatingPoint)
    {
        yield return new WaitForSeconds(3f);
        floatingPoint.SetActive(false);
    }
   
}
