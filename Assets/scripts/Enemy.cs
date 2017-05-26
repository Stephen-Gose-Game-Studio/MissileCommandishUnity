using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Enemy : MonoBehaviour
  {

  /** Object for enemy missile. */                   public    GameObject enemyRocket;
  /** Object for enemy bomber. */                    public    GameObject blueBomber;
  /** Object for enemy mirv. */                      public    GameObject mirv;
  /** Text Object for enemy missile count. */        public    Text       textEnemyThreat;

  /** Number of rockets the enemy has. */            protected long       mCurrentEnemyThreatCount;
  /** Number of rockets the enemy previously had. */ protected long       mPreviousWaveThreatCount;

  public long currentThreatCount  { get { return mCurrentEnemyThreatCount; } }
  public long previousThreatCount { get { return mPreviousWaveThreatCount; } }
  
  /****************************************************************************
  * Unity Methods 
  ****************************************************************************/
  void Start()
    {
    mPreviousWaveThreatCount = mCurrentEnemyThreatCount = 10;
    textEnemyThreat.text = "Enemy Threats\n" + mCurrentEnemyThreatCount;
    }

  void Update()
    {
    /** Randomly launch an EnemyRocket. */
    int x = (int)Random.Range(0f, 125.0f);
    textEnemyThreat.text = "Enemy Threats\n" + mCurrentEnemyThreatCount;

    /** Fire a missile. */
    if(mCurrentEnemyThreatCount > 0 && ((x == 1 && this.enabled) ||
      (MainGame.player.currentRocketCount <= 0                   ||
      !MainGame.player.checkHasBuildings()                       ||
      !MainGame.player.checkHasLaunchers())))
      {
      /** Randomly launch a Blue Bomber or MIRV; otherwise launch regular missile. */
      int roll = (int)Random.Range(0f, 16.0f);
      /** Launch BlueBomber. */
      if (roll == 1)
        {
        Instantiate(blueBomber);
        }
      /** Launch MIRV. */
      else if (roll == 15 || Input.GetKeyDown(KeyCode.G))
        {
        GameObject merv = Instantiate(mirv);
        merv.GetComponent<MIRV>().launchFromRandomPoint();
        }
      /** Launch Enemy Rocket. */
      else
        {
        GameObject rocket = Instantiate(enemyRocket);
        rocket.GetComponent<EnemyRocket>().launchFromRandomPoint();
        }
      
      mCurrentEnemyThreatCount--;
      }
    }

  /****************************************************************************
  * Methods 
  ****************************************************************************/
  /****************************************************************************
  * updateRocketCounts */
  /***
  * Updates the rocket counts with the passed in count.
  ****************************************************************************/
  public void setRocketCount(long rocketCount)
    {
    mCurrentEnemyThreatCount = mPreviousWaveThreatCount = rocketCount;
    }
  }
