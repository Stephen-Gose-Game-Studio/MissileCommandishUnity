using UnityEngine;
using System.Collections;

public class City : Building
  {

  /****************************************************************************
  * Unity Methods 
  ****************************************************************************/
  void OnCollisionEnter2D(Collision2D coll)
    {
    playExplosionAnim();
    }
  }
