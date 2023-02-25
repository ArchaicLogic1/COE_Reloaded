using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AssistantToTheSceneManager : MonoBehaviour
{
   public static AssistantToTheSceneManager instance;
    public AsyncOperation loadCutscene;
    public delegate void CutsceneLoaded();
    public static CutsceneLoaded onCutsceneLoaded;

    public delegate void RestartGame();
    public static RestartGame OnRestartGame;
    // Start is called before the first frame update
    void Start()
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
       

    }

    // Update is called once per frame
    void Update()
    {
  
        
        if(loadCutscene!=null && loadCutscene.isDone)
        {
            if(onCutsceneLoaded!=null)
            {
                onCutsceneLoaded();
                

            }
        }
    }
    public void loadGame()
    {
       loadCutscene= SceneManager.LoadSceneAsync("cutscene", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("Level_1", LoadSceneMode.Additive);

        
    }
    public void QuitToMenu()
    {
        if(OnRestartGame!=null)
        {
        OnRestartGame();

        }
        SceneManager.LoadScene(0, LoadSceneMode.Single);
        


    }
   
}
