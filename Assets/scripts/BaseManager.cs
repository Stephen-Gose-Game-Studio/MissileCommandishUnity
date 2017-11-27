using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/****************************************************************************
* BaseManager */ 
/**
* Base class for all Manager scripts.
****************************************************************************/
public abstract class BaseManager : MonoBehaviour
  {
  /**************************************************************************
  * Update */ 
  /**
  **************************************************************************/
  public virtual void Update ()
    {
    /** Esc pressed. Quit the application. */
    if(Input.GetKeyDown(KeyCode.Escape))
      Application.Quit();
    }
  }