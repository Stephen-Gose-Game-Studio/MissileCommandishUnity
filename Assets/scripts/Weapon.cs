using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weapon : MonoBehaviour
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
  virtual public void Start()
    {
    }

  virtual public void Update()
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
  virtual public bool checkAnimDone()
    {
    bool t = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Done");
    return t;
    }

  virtual public void move()
    {
    }
  
  virtual public void playExplosionAnim()
    {
    }

  /** Randomly sets a Building as a target for the weapon as long as the Building is alive. */
  public Vector3 findTargetBuilding()
    {
    /** Launcher list and City list to use for targets. */
    List<Building> buildings = new List<Building>();
    buildings.AddRange(Resources.FindObjectsOfTypeAll<Launcher>());
    buildings.AddRange(Resources.FindObjectsOfTypeAll<City>());

    /** Find least and largets indeces. */
    int minIndex    = 0;
    int maxIndex    = buildings.Count-1;
    int targetIndex = 0;

    /** Find a non-dead building to target. */
    for(int i = 0; i < maxIndex - 1; i++)
      {
      targetIndex = Random.Range(minIndex, maxIndex);
      if (!buildings[targetIndex].dead)
        break;
      }
    
    return buildings[targetIndex].transform.position;
    }

  /****************************************************************************
  * tryDestroy */ 
  /**
  * Checks if the animation is finished, "Done" state, and destroys the object.
  ****************************************************************************/
  virtual public void tryDestroy()
    {
    if (checkAnimDone ())
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
  ****************************************************************************/
  public void stopAudioSource()
    {
    if (GetComponent<AudioSource>() != null)
      GetComponent<AudioSource>().Stop();
    }
  }
