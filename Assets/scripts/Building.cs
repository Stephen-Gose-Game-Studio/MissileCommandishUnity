using UnityEngine;
using System.Collections;

/****************************************************************************
* Building *
/**
* Base class for Cities and Launchers.
****************************************************************************/
public class Building : MonoBehaviour
  {
  public bool   dead;
  public string bldgName;

  /****************************************************************************
  * Unity Methods 
  ****************************************************************************/
  /****************************************************************************
  * Start */ 
  /**
  ****************************************************************************/
  public virtual void Start()
    {
    dead = false;
    }

  /****************************************************************************
  * Update */ 
  /**
  ****************************************************************************/
  public virtual void Update()
    {
    if (dead)
      {
      /** Make sure the animation is playing. If not, make it play. The
      *   animation is stopped when the main game scene is disabled for pause
      *   or continue screen, so we want to be sure it resumes. */
      if (!this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Burning"))
        this.GetComponent<Animator>().Play("Burning");
      //tryDestroy();
      }
    }

  /****************************************************************************
  * Methods 
  ****************************************************************************/
  /****************************************************************************
  * checkAnimDone */ 
  /**
  * Checks if the animation is finished, "Done" state.
  ****************************************************************************/
  public bool checkAnimDone()
    {
    return false;//this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Done");
    }

  /****************************************************************************
  * playExplosionAnim */ 
  /**
  * Plays the Explosion Animation.
  ****************************************************************************/
  public virtual void playExplosionAnim()
    {
    if (!dead)
      {
      dead = true;
      MainGame.incrementShakeCounter(0.5f);
      this.GetComponent<Animator>().Play("Burning");
      GetComponent<AudioSource>().Play();
      gameObject.layer = MainGame.buildingFiresLayer;
      gameObject.tag   = MainGame.buildingFireTag;
      }
    }

  /****************************************************************************
  * tryDestroy */ 
  /**
  * Checks if the animation is finished, "Done" state, and destroys the object.
  ****************************************************************************/
  public void tryDestroy()
    {
//    if (checkAnimDone ())
    Destroy (gameObject);
    } 
  }

