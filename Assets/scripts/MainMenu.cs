using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

/******************************************************************************
* MainMenu */
/** 
* Manages events in the MainMenuScene.
******************************************************************************/
public class MainMenu : MonoBehaviour
  {
  public AudioMixer mainMixer;

  protected GameObject mCanvasMainMenu;
  protected GameObject mCanvasOptions;

  /****************************************************************************
  * Unity Methods 
  ****************************************************************************/
  /****************************************************************************
  * Start */ 
  /**
  ****************************************************************************/
  public void Start()
    {
    /** Store canvas object for later since finding inactive objects is a pain. */
    mCanvasMainMenu = GameObject.Find("canvasMainMenu").gameObject;
    mCanvasOptions  = GameObject.Find("canvasOptions") .gameObject;

    mCanvasMainMenu.SetActive(true);
    mCanvasOptions.SetActive(false);
    }

  /****************************************************************************
  * Methods 
  ****************************************************************************/
  /****************************************************************************
  * handleBack */ 
  /**
  * Displays the Main Menu canvas.
  ****************************************************************************/
  public void handleBack()
    {
    mCanvasMainMenu.SetActive(true);
    mCanvasOptions.SetActive(false);
    }

  /****************************************************************************
  * handleOptions */ 
  /**
  * Displays the Options canvas.
  ****************************************************************************/
  public void handleOptions()
    {
    mCanvasMainMenu.SetActive(false);
    mCanvasOptions.SetActive(true);
    }

  /****************************************************************************
  * loadMainGame */ 
  /**
  * Loads the main game.
  ****************************************************************************/
  public void loadMainGameScene()
    {
    SceneManager.LoadScene("MainGameScene");
    }

  /****************************************************************************
  * setVolume */ 
  /**
  * Sets the game's volume.
  * @param  volume  Value to set mixer volume.
  ****************************************************************************/
  public void setVolume(float volume)
    {
    mainMixer.SetFloat("volume", volume);
    }
  }
