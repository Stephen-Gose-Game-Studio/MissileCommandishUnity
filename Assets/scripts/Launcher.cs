using UnityEngine;
using System.Collections;

/******************************************************************************
* Launcher */
/** 
* Building that PlayerRockets launch from. If not usable, PlayerRockets cannot
* launch from it.
******************************************************************************/
public class Launcher : Building
  {

  /** Can be launched from. */ public bool isUsable;

  /****************************************************************************
  * Unity Methods 
  ****************************************************************************/
  public override void Start ()
    {
    base.Start();
    isUsable = true;
	  }

  void OnCollisionEnter2D(Collision2D coll)
    {
    isUsable = false;
    playExplosionAnim();
    }
  }
