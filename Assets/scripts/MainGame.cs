using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

/******************************************************************************
* MainGame */
/** 
* Where the magic happens.
******************************************************************************/
public class MainGame : MonoBehaviour
  {
  /** Properties. */
  protected GameObject mCanvasGame;
  protected GameObject mCanvasPause;
  protected GameObject mCanvasWaveCleared;
  protected Vector3    mCamOriginalPosition;
  protected float      mTimer;
  protected float      mTransitionTimer = 1.5f;
  protected bool       mWinBonusApplied;

  protected static long  mCurrentWave;
  protected static float mShakeCounter;

  public City     city1;
  public City     city2;
  public City     city3;
  public City     city4;
  public Launcher launcherL;
  public Launcher launcherC;
  public Launcher launcherR;
  public Text     textScore;
  public Text     textPlayerMissiles;
  public int      mainGameOverSceneIndex;
  public int      mainMenuSceneIndex;

  public static Enemy  enemy;
  public static Player player;

  /** Accessors. */
  protected int  enemyBlueBomberClonesInPlayCount{ get { return Resources.FindObjectsOfTypeAll<BlueBomber>().Length; } }
  protected bool enemyBlueBombersClonesInPlay    { get { return enemyBlueBomberClonesInPlayCount > 1; } }
  protected int  enemyBombClonesInPlayCount      { get { return Resources.FindObjectsOfTypeAll<Bomb>().Length; } }
  protected bool enemyBombClonesInPlay           { get { return enemyBombClonesInPlayCount > 1; } }
  protected int  enemyMIRVClonesInPlayCount      { get { return Resources.FindObjectsOfTypeAll<MIRV>().Length; } }
  protected bool enemyMIRVClonesInPlay           { get { return enemyMIRVClonesInPlayCount > 1; } }
  protected int  enemyRocketClonesInPlayCount    { get { return Resources.FindObjectsOfTypeAll<EnemyRocket>().Length; } }
  protected bool enemyRocketClonesInPlay         { get { return enemyRocketClonesInPlayCount > 1; } }
  protected int  playerRocketClonesInPlayCount   { get { return Resources.FindObjectsOfTypeAll<PlayerRocket>().Length; } }
  protected bool playerRocketClonesInPlay        { get { return playerRocketClonesInPlayCount > 1; } }
  
  public static int    buildingFiresLayer  { get { return 15; } }
  public static string buildingFireTag     { get { return "BuildingFire";  } }
  public static long   currentWave         { get { return mCurrentWave; } }
  public static int    enemyExplosionLayer { get { return 14; } }
  public static string enemyExplosionTag   { get { return "EnemyExplosion";  } }
  public static int    playerExplosionLayer{ get { return 13; } }
  public static string playerExplosionTag  { get { return "PlayerExplosion";  } }

  /** Simple methods. */
  public void incrementTimer() { mTimer += Time.deltaTime; }
  
  public static void incrementCurrentWave()           { mCurrentWave++; }
  public static void incrementPlayerScore(long val)   { player.playerScore += val; }
  public static void incrementShakeCounter(float val) { mShakeCounter += val; }
  public static void decrementShakeCounter()
    {
    mShakeCounter -= Time.deltaTime;
    mShakeCounter = mShakeCounter < 0.0f ? 0.0f : mShakeCounter;
    }
  
  /****************************************************************************
  * Unity Methods 
  ****************************************************************************/
  /****************************************************************************
  * Start */ 
  /**
  ****************************************************************************/
  public void Start ()
    {
    mShakeCounter = 0.0f;
    mCurrentWave  = 1;

    /** Store canvas object for later since finding inactive objects is a pain. */
    mCanvasPause       = GameObject.Find("canvasPause")      .gameObject;
    mCanvasGame        = GameObject.Find("canvasGame")       .gameObject;
    mCanvasWaveCleared = GameObject.Find("canvasWaveCleared").gameObject;

    /** Store Enemy and Player objects. */
    enemy  = ScriptableObject.FindObjectOfType<Enemy>();
    player = ScriptableObject.FindObjectOfType<Player>();

    /** Hide the Pause menu. */
    mCanvasPause.SetActive(false);

    /** Hide win menu. */
    mCanvasWaveCleared.SetActive(false);

    /** Play area visible. */
    mCanvasGame.SetActive(true);

    /** Save camera's original position for resetting after a shake. */
    mCamOriginalPosition = Camera.main.transform.localPosition;
    }

  /****************************************************************************
  * Update */ 
  /**
  ****************************************************************************/
  public void Update()
    {

    /** Check to see if we should shake the camera. */
    handleShake();

    textScore.text          = "Score\n"       + player.playerScore;
    textPlayerMissiles.text = "Player Ammo\n" + player.currentRocketCount;

    /** Win. */
    if(checkWin())
      {
      /** Using a delay so the transition does not seem as abrupt. */
      incrementTimer();
      if(mTimer >= mTransitionTimer)
        handleWin();
      }

    /** Check that Player lost. */
    else if(checkLose())
      {
      if(!enemyRocketClonesInPlay)
        {
        /** Using a delay so the transition does not seem as abrupt. */
        incrementTimer();
        if(mTimer >= mTransitionTimer)
          loadGameOverMenu();
        }
      }

    /** Do other things. */
    else
      {
      /** Reset the win bonus between win screens. */
      mWinBonusApplied = false;
      }
    }
  
  /****************************************************************************
  * Methods 
  ****************************************************************************/
  /****************************************************************************
  * checkLose */
  /**
  * Player loses if they have no buildings or launchers.
  ****************************************************************************/
  public bool checkLose()
    {
    bool a = !player.checkHasLaunchers();
    bool b = !player.checkHasBuildings();
    return a || b;
    }

  /****************************************************************************
  * checkWin */
  /**
  * Checks to see if the player won, Player beats the level if they have at 
  * least one city and one launcher left, and there are no more enemy threats.
  * Also makes sure that there are no clones flying through the air
  * before transition.
  ****************************************************************************/
  public bool checkWin()
    {
    bool enemyNoThreatsLeft = enemy.currentThreatCount <= 0;

    return !(enemyBombClonesInPlay     || enemyRocketClonesInPlay || enemyBlueBombersClonesInPlay || enemyMIRVClonesInPlay ||
             playerRocketClonesInPlay) && enemyNoThreatsLeft      && player.checkHasBuildings()   && player.checkHasLaunchers();
    }

  /****************************************************************************
  * handleNextWave */
  /**
  * Handles actions needed when the Continuing to the next wave.
  ****************************************************************************/
  public void handleNextWave()
    {
    /** Hide the win screen. */
    mCanvasWaveCleared.SetActive(false);

    /** Hide the Pause UI. */
    mCanvasPause.SetActive(false);

    /** Show all UI components used for the main game. */
    mCanvasGame.SetActive(true);

    /** Activate rocket controllers. */
    enemy.enabled  = true;
    player.enabled = true;

    incrementCurrentWave();

    enemy .setRocketCount((long)(currentWave * 1.0f) + 10);
    player.setRocketCount((long)(currentWave * 0.75f) + 30);
    }

  /****************************************************************************
  * handlePause */
  /**
  * Handles actions needed when the Pause Button is pressed.
  ****************************************************************************/
  public void handlePause()
    {
    /** Hide the win screen. Not needed but doing it anyway. */
    mCanvasWaveCleared.SetActive(false);

    /** Hide all UI components used for the main game. */
    mCanvasGame.SetActive(false);

    /** Load in the Pause UI. */
    mCanvasPause.SetActive(true);

    /** Deactivate rocket controllers. */
    enemy.enabled  = false;
    player.enabled = false;
    }
  
  /****************************************************************************
  * handleResume */
  /**
  * Handles actions needed when the Resume Button is pressed.
  ****************************************************************************/
  public void handleResume()
    {
    /** Hide the win screen. */
    mCanvasWaveCleared.SetActive(false);

    /** Hide the Pause UI. */
    mCanvasPause.SetActive(false);

    /** Show all UI components used for the main game. */
    mCanvasGame.SetActive(true);

    /** Activate rocket controllers. */
    enemy.enabled  = true;
    player.enabled = true;
    }

  /****************************************************************************
  * handleShake */
  /**
  * Handles camera shake.
  ****************************************************************************/
  public void handleShake()
    {

    /** Shake shake shake. */
    if (mShakeCounter > 0.0f)
      {
      float shakeAmount = 0.75f;
      Camera.main.transform.localPosition = Random.insideUnitCircle * shakeAmount;
      decrementShakeCounter();
      }
    
    /** Reset camera after shaking. */
    else if (Camera.main.transform.localPosition != mCamOriginalPosition)
      Camera.main.transform.localPosition = mCamOriginalPosition;
    }

  /****************************************************************************
  * handleWin */
  /**
  * Handles actions needed when the level is cleared.
  ****************************************************************************/
  public void handleWin()
    {
    /** Reset timer. */
    mTimer = 0;

    /** Show the win screen. */
    mCanvasWaveCleared.SetActive(true);

    /** Load in the Pause UI. */
    mCanvasPause.SetActive(false);

    /** Hide all UI components used for the main game. */
    mCanvasGame.SetActive(false);

    /** Activate rocket controllers. */
    enemy.enabled  = false;
    player.enabled = false;

    long launcherBonus = player.launcherCount()    * 500;
    long cityBonus     = player.cityCount()        * 250;
    long missileBonus  = player.currentRocketCount * 20;

    if(!mWinBonusApplied)
      {
      incrementPlayerScore(launcherBonus + cityBonus + missileBonus);
      mWinBonusApplied = true;
      }

    /** Update the counts and score for the Wave Cleared screen. */
    GameObject.Find("txtWaveCleared")  .GetComponent<Text>().text = "Wave "   + mCurrentWave + " Cleared";
    GameObject.Find("txtCurrentScore") .GetComponent<Text>().text = "SCORE\n" + player.playerScore;
    GameObject.Find("txtLauncherCount").GetComponent<Text>().text = player.launcherCount().ToString()    + " x 500 = " + launcherBonus.ToString();
    GameObject.Find("txtCityCount")    .GetComponent<Text>().text = player.cityCount().ToString()        + " x 250 = " + cityBonus.ToString();
    GameObject.Find("txtAmmoCount")    .GetComponent<Text>().text = player.currentRocketCount.ToString() + " x 20 = "  + missileBonus.ToString();
    }

  /****************************************************************************
  * keyBoardCommands */ 
  /**
  * Keyboard commands for testing the game.
  ****************************************************************************/
  protected void checkKeyBoardCommand()
    {
    /** Mouse position. */
    if (Input.GetKeyDown(KeyCode.M))
      print("Mouse ScreenToWorldPoint: " + Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }
    
  /****************************************************************************
  * loadGameOverMenu */ 
  /**
  * Loads the Game Over menu by Index.
  ****************************************************************************/
  public void loadGameOverMenu()
    {
    SceneManager.LoadScene("GameOverScene");
    }

  /****************************************************************************
  * loadMainMenu */ 
  /**
  * Loads the Main menu by Index.
  ****************************************************************************/
  public void loadMainMenu()
    {
    SceneManager.LoadScene("MainMenuScene");
    }
  }
