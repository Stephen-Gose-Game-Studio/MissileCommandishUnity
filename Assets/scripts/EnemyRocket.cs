using UnityEngine;
using System.Collections;

public class EnemyRocket : Rocket
  {

  /****************************************************************************
  * Unity Methods 
  ****************************************************************************/
  public override void Start()
    {
    base.Start();

    setWeaponSpeed((float)Random.Range(10, 20));
    }

  public override void Update ()
    {
    base.Update();
    move();
    tryDestroy();
    }

  void OnCollisionEnter2D(Collision2D coll)
    {
    /** Increase playerScore. */
    if (!dead)
      {
      /** Increase player score when hit by player's Rocket. **/
      if (coll.gameObject.tag == "PlayerRocket")
        MainGame.player.playerScore += directHitValue;
     
      /** Increase player score when hit by player's Explosion. **/
      else if (coll.gameObject.tag == "PlayerExplosion")
        MainGame.player.playerScore += proximityHitValue;
      
      /** Hit the ground. */
      else if (coll.gameObject.tag == "Ground")
        MainGame.incrementShakeCounter(0.5f);

      playExplosionAnim();
      }
    }

  void OnParticleCollision(GameObject other)
    {
    /** Increase player score when hit by player's Explosion. **/
    if (!dead)
      {
      MainGame.player.playerScore += particleHitValue;
      playExplosionAnim();
      }
    }

  /****************************************************************************
  * Methods 
  ****************************************************************************/
  /****************************************************************************
  * targetRandomPoint */ 
  /**
  * Targets random point.
  ****************************************************************************/
  public void targetRandomPoint()
    {
    Vector3 targetPosition = new Vector3(Random.Range(minX, maxX), minY, z);

    setTarget (targetPosition);
    //mTarget = findTargetBuilding();

    mScreenPoint           = transform.localPosition;
    mOffset                = new Vector2(mTarget.x - mScreenPoint.x, mTarget.y - mScreenPoint.y);
    mAngle                 = Mathf.Atan2(mOffset.y, mOffset.x) * Mathf.Rad2Deg;
    transform.rotation     = Quaternion.Euler(0, 0, mAngle);
    }

  /****************************************************************************
  * launchFromRandomPoint */ 
  /**
  * Launch rocket from random point.
  ****************************************************************************/
  public void launchFromRandomPoint()
    {
    /** Find random position, and set as target. */
    transform.position = new Vector3(Random.Range(minX, maxX), maxY, z);
    targetRandomPoint();
    }

  /****************************************************************************
  * launchFromPoint */ 
  /**
  * Launch rocket from fixed point. Allow for speed change in case it is
  * needed (MIRVs utilize this).
  * @param  launchFrom  Point to launch from.
  * @param  newSpeed    New speed factor.
  ****************************************************************************/
  public void launchFromPoint(Vector3 launchFrom, float newSpeed)
    {
    if(speed != newSpeed && newSpeed > 0)
      setWeaponSpeed(newSpeed);
    
    transform.position = launchFrom;
    targetRandomPoint();
    }

  /****************************************************************************
  * move */ 
  /**
  * Moves the rocket if not dead.
  ****************************************************************************/
  public override void move()
    {
    /** Update if the enemy class is enabled (game not paused). */
    if(MainGame.FindObjectOfType<Enemy>().enabled && !dead)
      {
      /** Destroy rocket if it did not hit anything and reaches target point. */
      if(transform.position == mTarget)
        playExplosionAnim();
      else
        {
        mStep              = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, mTarget, mStep);
        }
      }
    }

  /****************************************************************************
  * override playExplosionAnim */ 
  /**
  * Plays the Explosion Animation. Sets the layer to the EnemyExplosion layer.
  * and Tag.
  ****************************************************************************/
  public override void playExplosionAnim()
    {
    base.playExplosionAnim();
    gameObject.layer = MainGame.enemyExplosionLayer;
    gameObject.tag   = MainGame.enemyExplosionTag;
    }
  }
