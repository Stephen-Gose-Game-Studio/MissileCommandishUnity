using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour
  {
  /** Player's missile count. */           protected long       mCurrentPlayerRocketCount;
  /** Player's prevoious missile count. */ protected long       mPreviousWaveRocketCount;

  /** Player Rocket Object. */             public    GameObject playerRocket;
  /** Player's score. Global. */           public    long       playerScore;

  public long currentRocketCount  { get { return mCurrentPlayerRocketCount;} }
  public long previousRocketCount { get { return mPreviousWaveRocketCount; } }
  
  /****************************************************************************
  * Unity Methods 
  ****************************************************************************/
  void Start ()
    {
    playerScore               = 0;
    mCurrentPlayerRocketCount = mPreviousWaveRocketCount = 30;
    }

  void Update ()
    {
    Vector3 target = new Vector3();

    /** Launch a missile on LMB press, and Player has ammo. */
    if((Input.GetMouseButtonDown(0)) && mCurrentPlayerRocketCount > 0)
      {
      target = Camera.main.ScreenToWorldPoint(Input.mousePosition);

      /** Don't launch unless player tapped/clicked in play area. */
      if (target.x < 89.0f && target.x > -89.0f && target.y < 50.0f && target.y > -33.0f)
        {
        Launcher launcherToUse = null;
        
        /** Find the nearest available launcher to use. */
        float shortestDist = 10000.0f;
        Launcher[] ll = Resources.FindObjectsOfTypeAll<Launcher>();
        foreach (Launcher l in ll)
          {
          float d = Vector3.Distance (target, l.transform.position);
          if (shortestDist > d && l.GetComponent<Launcher>().isUsable)
            {
            launcherToUse = l;
            shortestDist  = d;
            }
          }
        
        /** Launch a rocket. */
        if(launcherToUse != null)
          {
          mCurrentPlayerRocketCount--;
          playerRocket.transform.position = launcherToUse.transform.position;
          Instantiate(playerRocket);
          }
        }
      }
    }
  
  /****************************************************************************
  * Methods
  ****************************************************************************/
  /****************************************************************************
  * cityCount */ 
  /**
  * Returns the remaining number of cities.
  ****************************************************************************/
  public int cityCount()
    {
    int i = 0;
    foreach (City c in Resources.FindObjectsOfTypeAll<City> ())
      i += !c.dead ? 1 : 0;
    return i;
    }

  /****************************************************************************
  * checkHasBuildings */ 
  /**
  * Checks if there are Buildings remaining. This can be done much better, but
  * meh.
  ****************************************************************************/
  public bool checkHasBuildings()
    {
    return cityCount() > 0;
    }

  /****************************************************************************
  * checkHasLaunchers */ 
  /**
  * Checks if there are Launchers remaining. This can be done much better, but
  * meh.
  ****************************************************************************/
  public bool checkHasLaunchers()
    {
    return launcherCount() > 0;
    }

  /****************************************************************************
  * launcherCount */ 
  /**
  * Returns the remaining number of launchers.
  ****************************************************************************/
  public int launcherCount()
    {
    int i = 0;
    foreach (Launcher l in Resources.FindObjectsOfTypeAll<Launcher> ())
      i += !l.dead ? 1 : 0;

    return i;
    }

  /****************************************************************************
  * updateRocketCounts */
  /***
  * Updates the rocket counts with the passed in count.
  ****************************************************************************/
  public void setRocketCount(long rocketCount)
    {
    mCurrentPlayerRocketCount = mPreviousWaveRocketCount = rocketCount;
    }
  }

