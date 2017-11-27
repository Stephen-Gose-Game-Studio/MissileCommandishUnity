using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/******************************************************************************
* Weapon */
/** 
* Base class for all weapons such as Rockets and Bombs.
******************************************************************************/
public abstract class Weapon : Thing
  {
  protected float   mAngle;
  protected Vector3 mScreenPoint;
  protected float   mStep;
  protected Vector3 mTarget;

  public    long    directHitValue;
  public    long    proximityHitValue;
  public    long    particleHitValue;
  public    bool    dead;
  public    float   speed;
  public    float   minY;
  public    float   maxY;
  public    float   minX;
  public    float   maxX;

  /** This will increase as the game progresses inorder to make it more difficult. It
      will always be equal to the current level. If not, it will be updated. */
  protected long    mSpeedBonus;
  protected float   mAdjustedSpeed;

  /** Set z to be that of the original z value, else Z
  *   will try to approach camera's z, and go off screen. */
  public float z;


  /****************************************************************************
  * Unity Methods 
  ****************************************************************************/
  /****************************************************************************
  * Start */ 
  /**
  ****************************************************************************/
  public virtual void Start()
    {
    }

  /****************************************************************************
  * Update */ 
  /**
  ****************************************************************************/
  public virtual void Update()
    {
    }

  /****************************************************************************
  * Methods 
  ****************************************************************************/
  /****************************************************************************
  * checkAnimDone */ 
  /**
  * Checks if the animation is finished, "Done" state.
  ****************************************************************************/
  public virtual bool checkAnimDone()
    {
    bool t = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Done");
    return t;
    }

  public virtual void move()
    {
    }
  
  public virtual void playExplosionAnim()
    {
    }
  
  /****************************************************************************
  * findTargetBuilding */ 
  /**
  * Randomly sets a Building as a target for the weapon as long as the
  * Building is alive.
  ****************************************************************************/
  public Vector3 findTargetBuilding() { return findTargetBuilding(minY); }
  /**
  * Randomly sets a Building as a target for the weapon as long as the
  * Building is alive. Will try to reach lowest passed in Y value.
  * 
  * @param  targetY  Lowest Y destination point.
  ****************************************************************************/
  public Vector3 findTargetBuilding(float targetY)
    {
    /** Launcher list and City list to use for targets. */
    List<Building> buildings = new List<Building>();
    buildings.AddRange(Resources.FindObjectsOfTypeAll<Launcher>());
    buildings.AddRange(Resources.FindObjectsOfTypeAll<City>());

    int maxIndex    = buildings.Count;
    int minIndex    = 0;
    int targetIndex = Random.Range(minIndex, maxIndex);
    Vector3 target = new Vector3(buildings[targetIndex].transform.position.x, targetY, z);

    return target;//buildings[targetIndex].transform.position;
    }

  /****************************************************************************
  * tryDestroy */ 
  /**
  * Checks if the animation is finished, "Done" state, and destroys the object.
  ****************************************************************************/
  public virtual void tryDestroy()
    {
    if (checkAnimDone())
      Destroy (gameObject);
    }

  /****************************************************************************
  * tryDestroy */ 
  /**
  * Sets the weapon speed and applies the mod based on wave.
  *
  * @param  newSpeed  Speed to set the weapon to.
  ****************************************************************************/
  public void setWeaponSpeed(float newSpeed)
    {
    speed = newSpeed + MainGame.currentWave * 4;
    }

  /****************************************************************************
  * stopAudioSource */ 
  /**
  * Stops the Weapon's audio source.
  ****************************************************************************/
  public void stopAudioSource()
    {
    if (GetComponent<AudioSource>() != null)
      GetComponent<AudioSource>().Stop();
    }
  }
