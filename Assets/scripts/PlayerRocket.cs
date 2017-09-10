using UnityEngine;
using System.Collections;

/******************************************************************************
* PlayerRocket */
/** 
* Rockets that Players launch from launchers.
******************************************************************************/
public class PlayerRocket : Rocket
  {
  /** Final position of the rocket. */ protected Vector3 mFinalPosition;
  /** Mouse position. */               protected Vector3 mMouse;

  /** Score multiplier text. */ public GameObject multiplierText;

  /****************************************************************************
  * Unity Method
  ****************************************************************************/
  /****************************************************************************
  * Start */ 
  /**
  ****************************************************************************/
  public override void Start()
    {
    base.Start();

    /** Reset the weapon speed. */
    setWeaponSpeed(60.0f);

    /** Use mouse position and set as target. */
    mMouse             = Input.mousePosition;
    setTarget(Camera.main.ScreenToWorldPoint(mMouse));

    mScreenPoint       = Camera.main.WorldToScreenPoint(transform.localPosition);
    mOffset            = new Vector2(mMouse.x - mScreenPoint.x, mMouse.y - mScreenPoint.y);
    mAngle             = Mathf.Atan2(mOffset.y, mOffset.x) * Mathf.Rad2Deg;
    transform.rotation = Quaternion.Euler(0, 0, mAngle);
    GetComponent<Rigidbody2D>().freezeRotation = true;
    playRocketLaunchSound();
    }

  /****************************************************************************
  * Update */ 
  /**
  ****************************************************************************/
  public override void Update ()
    {
    base.Update();
    move();
    tryDestroy();
    }
 
  /****************************************************************************
  * OnCollisionEnter2D */ 
  /**
  ****************************************************************************/
  void OnCollisionEnter2D(Collision2D coll)
    {
    if (!dead)
      {
      playExplosionAnim();
      }
    }

  /****************************************************************************
  * Methods 
  ****************************************************************************/
  /****************************************************************************
  * move */ 
  /**
  * Moves the rocket if not dead.
  ****************************************************************************/
  public override void move()
    {
    /** Update if the enemy class is enabled (game not paused). */
    if (MainGame.FindObjectOfType<Player>().enabled && !dead)
      {
      /** Destroy rocket if it did not hit anything and reaches target point. */
      if (transform.position == mTarget)
        playExplosionAnim();

      /** Update new position. */
      else
        {
        mStep = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, mTarget, mStep);
        }
      }
    }

  /****************************************************************************
  * playExplosionAnim */ 
  /**
  * Plays the Explosion Animation. Sets the layer to the PlayerExplosion layer.
  ****************************************************************************/
  public override void playExplosionAnim()
    {
    base.playExplosionAnim();

    gameObject.layer = MainGame.playerExplosionLayer;
    gameObject.tag   = MainGame.playerExplosionTag;
    }
  }
