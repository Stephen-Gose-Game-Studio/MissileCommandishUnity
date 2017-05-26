using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameOverScene : MonoBehaviour
  {
  public Button btnRestart;
  public int    mainGameSceneIndex = -1;

  /****************************************************************************
  * Unity Methods 
  ****************************************************************************/
  void Start ()
    {
    btnRestart.GetComponent<Button>().onClick.AddListener(loadMainGameScene);
    PlayerPrefs.SetInt("previousSceneIndex", 2);
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
