using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static PipeScript;
using static MissileDestroy;

public class LifeManager : MonoBehaviour
{
  public delegate void GameOver();
  public static event GameOver OnGameOver;

  [SerializeField]
  private Sprite fullHeart_;
  [SerializeField]
  private Sprite brokenHeart_;
  [SerializeField]
  private Sprite emptyHeart_;

  [SerializeField]
  private float timeBroken_ = 0.5f;
  [SerializeField]
  private float degreeRotation_ = 15.0f;
  [SerializeField]
  private float timeBig_ = 0.5f;
  [SerializeField]
  private float plusScale_ = 0.15f;

  private Heart[] hearts_;
  private int numLifes;
  private Transform shipTransform_;
  // Start is called before the first frame update

  private void GetHeartsFromCanvas(ref Canvas canvas)
  {
    int numChilds = canvas.transform.childCount;
    List<Heart> hearts = new List<Heart>();

    for (int i = 0; i < numChilds; i++) { 
      GameObject child = canvas.transform.GetChild(i).gameObject;
      Heart heartComp = child.GetComponent<Heart>();
      if(heartComp)
      {
        hearts.Add(heartComp);
      }
    }

    hearts_ = hearts.ToArray();
    numLifes = hearts_.Length;
  }

  public void Init(ref Canvas canvas)
  {
    GetHeartsFromCanvas(ref canvas);
    OnRocketCollision += BreakFirstHeart;
    OnHealMissileCatch += HealFirstHeart;
  }

  private void BreakFirstHeart()
  {
    if (numLifes > 0)
    {
      hearts_[numLifes - 1].Break(ref brokenHeart_, ref emptyHeart_, timeBroken_, degreeRotation_);
      numLifes--;
    }else
    {
      OnGameOver?.Invoke();
    }
      
  }
  private void HealFirstHeart()
  {
    if(numLifes < hearts_.Length)
    {
      hearts_[numLifes].Heal(ref fullHeart_, plusScale_, timeBig_);
      numLifes++;
    }
  }

}
