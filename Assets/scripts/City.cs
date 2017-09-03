using UnityEngine;
using System.Collections;

/******************************************************************************
* City */
/** 
* A city building.
******************************************************************************/
public class City : Building
  {
  /****************************************************************************
  * Unity Methods 
  ****************************************************************************/
  public override void Start()
    {
    base.Start();
    }
  
  void OnCollisionEnter2D(Collision2D coll)
    {
    playExplosionAnim();
    }
  }
