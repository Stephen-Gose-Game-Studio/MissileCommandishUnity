using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/******************************************************************************
* TextBonusMultiplier */
using UnityEngine.UI;


/** 
* Text that displays a multiplier for when a multiplier has been applied.
******************************************************************************/
public class TextBonusMultiplier : MonoBehaviour
  {
  /** Timer. */          protected float mTimer;
  /** Text component. */ public    Text  textComponent;               

  /** Text displayed. */                public string multiplierText;
  /** Time before object destroyed . */ public float  destroyTimer;

  /** Simple methods. */
  public void incrementTimer() { mTimer += Time.deltaTime; }

  /****************************************************************************
  * Unity Methods 
  ****************************************************************************/
  /****************************************************************************
  * Start */ 
  /**
  ****************************************************************************/
	void Start ()
    {
    textComponent = Instantiate(textComponent);
    //textComponent.text = "poop";
    textComponent.transform.position = gameObject.transform.position;
    textComponent.transform.SetParent(GameObject.FindGameObjectWithTag("CanvasGame").GetComponent<Canvas>().transform);
    //TODO CH  NEED TO SET SCALE AND TEXT SIZE
	  }
	
  /****************************************************************************
  * Update */ 
  /**
  ****************************************************************************/
	void Update ()
    {
    incrementTimer();
	  }

  /****************************************************************************
  * Methods 
  ****************************************************************************/
  /****************************************************************************
  * tryDestroy */ 
  /**
  * Checks if the animation is timer has met the target destroy time, then
  * destroys the gameobject.
  ****************************************************************************/
  public void tryDestroy()
    {
    if (mTimer >= destroyTimer)
      Destroy (gameObject);
    }
  }
