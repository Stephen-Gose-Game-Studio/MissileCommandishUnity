using UnityEngine;
using System.Collections;

/******************************************************************************
* BlueBomber */
/** 
* A Blue Bomber that flies across the top of the screen, and drops bombs on
* targets.
******************************************************************************/
public class BlueBomber : Weapon
  {
  public GameObject daBomb;
  public AudioClip  bombDrop;
  public AudioClip  bomberExplosion;

  protected Vector3 mBombTarget;
  protected int     mBombCount;

  /****************************************************************************
  * Unity Methods 
  ****************************************************************************/
  /****************************************************************************
  * Start */ 
  /**
  ****************************************************************************/
  public override void Start()
    {
    setWeaponSpeed((float)Random.Range(6, 10));

    dead = false;
    /** Find random position, and set as entry point. */
    transform.position = new Vector3(minX,Random.Range(minY, maxY),z);

    /** Set target for bomber to fly to. */
    mTarget = new Vector3(maxX,transform.position.y,z);

    /** Set target for bomb to drop on. We want a really low target Y. */
    mBombTarget = findTargetBuilding(-51);
    mBombCount  = 1;
    }

  /****************************************************************************
  * Update */ 
  /**
  ****************************************************************************/
  public override void Update()
    {
    base.Update();

    dropBomb();
    move();
    tryDestroy();
    }
	
  /****************************************************************************
  * OnCollisionEnter2D */ 
  /**
  ****************************************************************************/
  void OnCollisionEnter2D(Collision2D coll)
    {
    /** Increase playerScore. */
    if (!dead)
      {
      /** Increase player score when hit by player's Rocket. **/
      if (coll.gameObject.tag == "PlayerRocket")
        MainGame.incrementPlayerScore(directHitValue);

      /** Increase player score when hit by player's Explosion. **/
      else if (coll.gameObject.tag == "PlayerExplosion")
        MainGame.incrementPlayerScore(proximityHitValue);
      
      playExplosionAnim();
      }
    }

  /****************************************************************************
  * OnParticleCollision */ 
  /**
  ****************************************************************************/
  void OnParticleCollision(GameObject other)
    {
    /** Increase player score when hit by player's Explosion. **/
    if (!dead)
      {
      MainGame.incrementPlayerScore(particleHitValue);

      playExplosionAnim();
      }
    }
  
  /****************************************************************************
  * Methods 
  ****************************************************************************/
  /****************************************************************************
  * dropBomb */ 
  /**
  * Release bomb on target. 
  ****************************************************************************/
  public void dropBomb()
    {
    /** Update if the enemy class is enabled (game not paused). */
    if(!dead && mBombCount > 0 && (transform.position.x <= mBombTarget.x + 1 && transform.position.x >= mBombTarget.x - 1))
      {
      GetComponent<AudioSource>().clip    = bombDrop;
      GetComponent<AudioSource>().enabled = true;
      GetComponent<AudioSource>().Play();

      mBombCount--;

      GameObject bomb         = Instantiate(daBomb);
      bomb.transform.position = transform.position;
      bomb.GetComponent<Bomb>().setTarget(mBombTarget);
      }
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
      mStep = speed * Time.deltaTime;
      transform.position = Vector3.MoveTowards(transform.position, mTarget, mStep);
      }

    /** Reached destination, destroy. */
    if (transform.position == mTarget)// || dead)
      Destroy (gameObject);
    }

  /****************************************************************************
  * playExplosionAnim */ 
  /**
  * Plays the Explosion Animation. Sets the layer to the EnemyExplosion layer.
  * and Tag.
  ****************************************************************************/
  public override void playExplosionAnim()
    {
    dead = true;
    this.GetComponent<Animator>().Play("Explosion");

    stopAudioSource();
    GetComponent<AudioSource>().clip    = bomberExplosion;
    GetComponent<AudioSource>().enabled = true;
    GetComponent<AudioSource>().Play();

    gameObject.layer = MainGame.enemyExplosionLayer;
    gameObject.tag   = MainGame.enemyExplosionTag;
    }
  }
