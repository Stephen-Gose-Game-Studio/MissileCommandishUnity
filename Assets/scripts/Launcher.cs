using UnityEngine;
using System.Collections;

public class Launcher : Building
  {

  /** Can be launched from. */ public bool isUsable;

  /****************************************************************************
  * Unity Methods 
  ****************************************************************************/
	void Start ()
    {
    isUsable = true;
	  }

  void OnCollisionEnter2D(Collision2D coll)
    {
    isUsable = false;
    playExplosionAnim();
    }
  }
