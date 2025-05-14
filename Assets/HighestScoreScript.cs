using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighestScoreScript : MonoBehaviour
{
  public int score_;

  void Awake()
  {
    PlayerPrefs.GetInt("score", 0);
  }
}
