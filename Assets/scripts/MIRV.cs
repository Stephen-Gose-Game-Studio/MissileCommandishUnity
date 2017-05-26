using UnityEngine;
using System.Collections;

public class MIRV : Rocket
  {
  public GameObject rocket1;
  public AudioClip  split;
  bool launchedWarheads;

	public override void Start ()
    {
    base.Start();
    setWeaponSpeed((float)Random.Range(5, 10));
    launchedWarheads = false;
	  }
	
  public override void Update ()
    {
    base.Update();
    launchWarheads();
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
      launchedWarheads = true;
      playExplosionAnim();
      }
    }

  void OnParticleCollision(GameObject other)
    {
    /** Increase player score when hit by player's Explosion. **/
    if (!dead)
      {
      launchedWarheads = true;
      MainGame.player.playerScore += particleHitValue;
      playExplosionAnim();
      }
    }

  /****************************************************************************
  * Methods 
  ****************************************************************************/
  /****************************************************************************
  * launchWarheads */ 
  /**
  * Launches multiple rockets when random value met. 
  ****************************************************************************/
  public void launchWarheads()
    {
    //TODO consider putting this on a timer instead of random value.
    /** Launch when target value rolled, and between a fair area to branch from--toppish third of play area. */
    int v = Random.Range(0, 30);
    if (!launchedWarheads && v == 5 && (transform.position.y < 40.0f && transform.position.y > 0.0f))
      {
      GetComponent<AudioSource>().clip    = split;
      GetComponent<AudioSource>().enabled = true;
      GetComponent<AudioSource>().Play();

      GameObject r  = Instantiate(rocket1);
      GameObject r2 = Instantiate(rocket1);
      GameObject r3 = Instantiate(rocket1);

      /** Set random speed. */
      float s = (float)Random.Range(5, 10);
      r .GetComponent<EnemyRocket>().launchFromPoint(transform.position, s);
      r2.GetComponent<EnemyRocket>().launchFromPoint(transform.position, s);
      r3.GetComponent<EnemyRocket>().launchFromPoint(transform.position, s);
      launchedWarheads = true;
      }
    }

  /****************************************************************************
  * launchFromRandomPoint */ 
  /**
  * Launch MIRV from random point.
  ****************************************************************************/
  public void launchFromRandomPoint()
    {
    /** Find random position, and set as target. */
    transform.position = new Vector3(Random.Range(minX, maxX), maxY, z);
    targetRandomPoint();
    }

  /****************************************************************************
  * targetRandomPoint */ 
  /**
  * Targets random point.
  ****************************************************************************/
  public void targetRandomPoint()
    {
    Vector3 targetPosition = new Vector3(Random.Range(minX, maxX), minY, z);

    setTarget (targetPosition);

    mScreenPoint       = transform.localPosition;
    mOffset            = new Vector2(mTarget.x - mScreenPoint.x, mTarget.y - mScreenPoint.y);
    mAngle             = Mathf.Atan2(mOffset.y, mOffset.x) * Mathf.Rad2Deg;
    transform.rotation = Quaternion.Euler(0, 0, mAngle);
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
      if (transform.position == mTarget)
        Destroy(gameObject);
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
