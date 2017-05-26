using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
/****************************************************************************
* DEV NOTES
*   Animators: The way I handle animations is probably odd from the way they
* should be handled. To make it work my I have 3+ states that are manually
* made: Idle, SomeExplosion, ..., Done. 'Idle' is set to have no exit times
* so its transition will be ignored(hack?). 'Done' is checked for in order
* to discern that the object should be destroyed.
****************************************************************************/
public class MainGame : MonoBehaviour
  {
  GameObject   canvasGame;
  GameObject   canvasPause;
  GameObject   canvasWaveCleared;
  Vector3      camOriginalPosition;

  float gameOverTransitionTimer = 1.5f;

  int   enemyBlueBomberClonesInPlayCount{ get { return Resources.FindObjectsOfTypeAll<BlueBomber>().Length; } }
  bool  enemyBlueBombersClonesInPlay    { get { return enemyBlueBomberClonesInPlayCount > 1; } }

  int   enemyBombClonesInPlayCount      { get { return Resources.FindObjectsOfTypeAll<Bomb>().Length; } }
  bool  enemyBombClonesInPlay           { get { return enemyBombClonesInPlayCount > 1; } }

  int   enemyMIRVClonesInPlayCount      { get { return Resources.FindObjectsOfTypeAll<MIRV>().Length; } }
  bool  enemyMIRVClonesInPlay           { get { return enemyMIRVClonesInPlayCount > 1; } }

  int   enemyRocketClonesInPlayCount    { get { return Resources.FindObjectsOfTypeAll<EnemyRocket>().Length; } }
  bool  enemyRocketClonesInPlay         { get { return enemyRocketClonesInPlayCount > 1; } }
  
  int   playerRocketClonesInPlayCount   { get { return Resources.FindObjectsOfTypeAll<PlayerRocket>().Length; } }
  bool  playerRocketClonesInPlay        { get { return playerRocketClonesInPlayCount > 1; } }

  public static Enemy  enemy;
  public static Player player;

  protected     float  mTimer;

  public        City     city1;
  public        City     city2;
  public        City     city3;
  public        City     city4;
  public        Launcher launcherL;
  public        Launcher launcherC;
  public        Launcher launcherR;

  public        Text   textScore;
  public        Text   textPlayerMissiles;

  public        Button btnPause;
  public        Button btnResume;
  public        Button btnQuitPauseMenu;
  public        Button btnQuitWinMenu;
  public        Button btnContinue;
  public        int    mainGameOverSceneIndex;
  public        int    mainMenuSceneIndex;

  protected static long  mCurrentWave;
  protected static float mShakeCounter;

  public static int    buildingFiresLayer  { get { return 15; } }
  public static string buildingFireTag     { get { return "BuildingFire";  } }
  public static long   currentWave         { get { return mCurrentWave; } }
  public static int    enemyExplosionLayer { get { return 14; } }
  public static string enemyExplosionTag   { get { return "EnemyExplosion";  } }
  public static int    playerExplosionLayer{ get { return 13; } }
  public static string playerExplosionTag  { get { return "PlayerExplosion";  } }
  public static void   incrementShakeCounter(float val) { mShakeCounter += val; }
  public static void   decrementShakeCounter()
    {
    mShakeCounter -= Time.deltaTime;
    mShakeCounter = mShakeCounter < 0.0f ? 0.0f : mShakeCounter;
    }

  public static void incrementCurrentWave() { mCurrentWave++; }

  /****************************************************************************
  * Unity Methods 
  ****************************************************************************/
  void Start ()
    {
    mShakeCounter = 0.0f;
    mCurrentWave  = 1;
    /** Add listeners for buttons. */
    btnPause        .onClick.AddListener(handlePause);
    btnResume       .onClick.AddListener(handleResume);
    btnQuitPauseMenu.onClick.AddListener(loadMainMenu);
    btnQuitWinMenu  .onClick.AddListener(loadMainMenu);
    btnContinue     .onClick.AddListener(handleNextWave);

    /** Store canvas object for later since finding inactive objects is a pain. */
    canvasPause       = GameObject.Find("canvasPause")      .gameObject;
    canvasGame        = GameObject.Find("canvasGame")       .gameObject;
    canvasWaveCleared = GameObject.Find("canvasWaveCleared").gameObject;
    enemy             = ScriptableObject.FindObjectOfType<Enemy>();
    player            = ScriptableObject.FindObjectOfType<Player>();

    /** Hide the Pause menu. */
    canvasPause.SetActive(false);

    /** Hide win menu. */
    canvasWaveCleared.SetActive(false);

    /** Play area visible. */
    canvasGame.SetActive(true);

    camOriginalPosition = Camera.main.transform.localPosition;
    }

  void Update()
    {
    update();
    }
  
  /****************************************************************************
  * Methods 
  ****************************************************************************/
  /****************************************************************************
  * checkLose */
  /**
  * Checks to see if the player lost.
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
    canvasWaveCleared.SetActive(false);

    /** Hide the Pause UI. */
    canvasPause.SetActive(false);

    /** Show all UI components used for the main game. */
    canvasGame.SetActive(true);

    /** Activate rocket controllers. */
    enemy.enabled  = true;
    player.enabled = true;

    incrementCurrentWave();
    enemy .setRocketCount((long)(currentWave * .5 + 1) + 15);
    player.setRocketCount((long)(currentWave * .5) + 30);
    }

  /****************************************************************************
  * handlePause */
  /**
  * Handles actions needed when the Pause Button is pressed.
  ****************************************************************************/
  public void handlePause()
    {
    /** Hide the win screen. Not needed but doing it anyway. */
    canvasWaveCleared.SetActive(false);

    /** Hide all UI components used for the main game. */
    canvasGame.SetActive(false);

    /** Load in the Pause UI. */
    canvasPause.SetActive(true);

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
    canvasWaveCleared.SetActive(false);

    /** Hide the Pause UI. */
    canvasPause.SetActive(false);

    /** Show all UI components used for the main game. */
    canvasGame.SetActive(true);

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
    else if (Camera.main.transform.localPosition != camOriginalPosition)
      Camera.main.transform.localPosition = camOriginalPosition;
    }

  /****************************************************************************
  * handleWin */
  /**
  * Handles actions needed when the level is cleared.
  ****************************************************************************/
  public void handleWin()
    {
    /** Show the win screen. */
    canvasWaveCleared.SetActive(true);

    /** Load in the Pause UI. */
    canvasPause.SetActive(false);

    /** Hide all UI components used for the main game. */
    canvasGame.SetActive(false);

    /** Activate rocket controllers. */
    enemy.enabled  = false;
    player.enabled = false;

    /** Update the counts and score for the Wave Cleared screen. */
    GameObject.Find("txtWaveCleared")  .GetComponent<Text>().text = "Wave "   + mCurrentWave + " Cleared";
    GameObject.Find("txtCurrentScore") .GetComponent<Text>().text = "SCORE\n" + player.playerScore;
    GameObject.Find("txtLauncherCount").GetComponent<Text>().text = player.launcherCount().ToString();
    GameObject.Find("txtCityCount")    .GetComponent<Text>().text = player.cityCount().ToString();
    GameObject.Find("txtAmmoCount")    .GetComponent<Text>().text = player.currentRocketCount.ToString();
    }

  /****************************************************************************
  * loadGameOverMenu */ 
  /**
  * Loads the Game Over menu by Index.
  ****************************************************************************/
  public void loadGameOverMenu()
    {
    SceneManager.LoadScene(mainGameOverSceneIndex);
    }

  /****************************************************************************
  * loadMainMenu */ 
  /**
  * Loads the Main menu by Index.
  ****************************************************************************/
  public void loadMainMenu()
    {
    SceneManager.LoadScene(mainMenuSceneIndex);
    }

  /****************************************************************************
  * update */ 
  /**
  * Where the magic happens.
  ****************************************************************************/
  public void update()
    {

    /** Check to see if we should shake the camera. */
    handleShake();

    textScore.text          = "Score\n"       + player.playerScore;
    textPlayerMissiles.text = "Player Ammo\n" + player.currentRocketCount;

    /** Mouse position. */
    if (Input.GetKeyDown(KeyCode.M))
      print("Mouse ScreenToWorldPoint: " + Camera.main.ScreenToWorldPoint(Input.mousePosition));

//    /** Pause. */
//    if (Input.GetKey(KeyCode.S))
//      {
//      handlePause();
//      }

//    /** Resume. */
//    if (Input.GetKeyDown(KeyCode.W))
//      {
//      handleResume();
//      }  

    /** Win. */
    if(checkWin())
      {
      handleWin();
      }

    /** Check that Player lost. */
    else if(checkLose())
      {
      if(!enemyRocketClonesInPlay)
        {
        /** Using a delay so the game over transition does not seem as abrupt. */
        mTimer += Time.deltaTime;
        if(mTimer >= gameOverTransitionTimer)
          loadGameOverMenu();
        }
      }
    }
  }
