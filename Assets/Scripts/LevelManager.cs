using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelManager : MonoBehaviour
{

  public delegate void LevelChange();
  public static event LevelChange OnLevelChange;

  private int currentLevel_ = 0;
 
  // Start is called before the first frame update
  void Start()
  {
    
  }



}
