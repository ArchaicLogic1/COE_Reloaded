using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
public class endgameMenuManager : TextTyper
{
    [SerializeField] TMP_Text score, nameInput;
    int scoreTotal;
    int placeHolderScore;
   [SerializeField]float scoreTallySpeed;
    Animator myanim;
    [SerializeField] AudioClip counterSound;
    [SerializeField] Button exitButton;
    [SerializeField] TMP_InputField uiNameInputElement;


    // Start is called before the first frame update
    void Start()
    {
        uiNameInputElement.characterLimit = 5;
        myanim = GetComponent<Animator>();
        scoreTotal = GameManager.instance.score;
        StartCoroutine(TextTyper3000(sentences, 0, TextToType));
        exitButton.onClick.AddListener(StoreScore);
        
       
    }

    // Update is called once per frame
    void Update()
    {
        score.text = placeHolderScore.ToString();
       
    }
    public void restartGame()
    {


        AssistantToTheSceneManager.instance.QuitToMenu();

        //Application.Quit();
    }
    public void startCounter()
    {
        StartCoroutine(CountUp());
    }
    IEnumerator CountUp()
    {




        myAudio.clip = counterSound;
        myAudio.pitch = 1;
        myAudio.Play();

        while (placeHolderScore < GameManager.instance.score)
        {
            if (scoreTotal >= 100000)
            {
                scoreTotal -= 100000;
                placeHolderScore += 100000;

            }
            if (scoreTotal >= 10000)
            {
                scoreTotal -= 10000;
                placeHolderScore += 10000;
            }
            if(scoreTotal>=1000)
            {
                scoreTotal-= 1000;
                placeHolderScore += 1000;
            }
            
            if(scoreTotal>=100)
            {
                scoreTotal -= 100;
                placeHolderScore += 100;
            }
            if(scoreTotal>=1)
            {
                scoreTotal--;
                
               placeHolderScore++;
            }
         
       
            yield return new WaitForSecondsRealtime(scoreTallySpeed);
          
            if (placeHolderScore >= GameManager.instance.score)
            {
                placeHolderScore = GameManager.instance.score;
               
            }    
                

        }
       
        myAudio.enabled = false;
        Debug.Log("soundShouldStop");
    }
    public void StoreScore()
    {
        

             LeaderBoardPersistence.instance.CreateNewNameandScore(GameManager.instance.score, nameInput.text);
      

        
    }
 
}
