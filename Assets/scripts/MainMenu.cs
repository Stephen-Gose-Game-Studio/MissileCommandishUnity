﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
  {
  public Button newGameButton;
  public int    mainGameSceneIndex;

  /****************************************************************************
  * Unity Methods 
  ****************************************************************************/
  void Start ()
    {
    newGameButton.GetComponent<Button>().onClick.AddListener(loadMainGameScene);
    PlayerPrefs.SetInt("previousSceneIndex", 0);
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
