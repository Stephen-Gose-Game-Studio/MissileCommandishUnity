using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

/******************************************************************************
* GameOver */
/** 
* Manages the GameOverScene
******************************************************************************/
public class GameOver : MonoBehaviour
  {
  /****************************************************************************
  * Methods 
  ****************************************************************************/
  /****************************************************************************
  * loadMainGame */
  /**
  * Load the main game scene.
  ****************************************************************************/
  public void loadMainGameScene()
    {
    SceneManager.LoadScene("MainGameScene");
    }
  }
