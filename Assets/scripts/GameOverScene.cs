using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

/******************************************************************************
* GameOverScene */
/** 
* Scene consisting of a basic background and some text denoting player lost.
* Allows for restarting the game.
******************************************************************************/
public class GameOverScene : BaseManager
  {
  public Button btnRestart;
  public int    mainGameSceneIndex = -1;

  /****************************************************************************
  * Unity Methods 
  ****************************************************************************/
  /****************************************************************************
  * Start */ 
  /**
  ****************************************************************************/
  void Start ()
    {
    btnRestart.GetComponent<Button>().onClick.AddListener(loadMainGameScene);
    }
  
  /****************************************************************************
  * Update */ 
  /**
  ****************************************************************************/
  public override void Update()
    {
    base.Update();
    }

  /****************************************************************************
  * Methods 
  ****************************************************************************/
  /****************************************************************************
  * loadMainGame */ 
  /**
  * UnityEvent that is triggered when the button is pressed. Loads the Main
  * Game Scene.
  * 
  * Note: Triggered on MouseUp after MouseDown on the same object.
  * see: https://docs.unity3d.com/ScriptReference/UI.Button-onClick.html
  ****************************************************************************/
  void loadMainGameScene()
    {
    SceneManager.LoadScene(mainGameSceneIndex);
    }
  }
