using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeScript : MonoBehaviour
{
  public Sprite lidOpen_, lidClosed_;
  private bool isLidOpen_ = false;
  [SerializeField]
  private float timeOpen_ = 0.8f;
  private float timeCounter = -1.0f;
    
  public void OpenPipe()
  {
    if(!isLidOpen_)
    {
      GetComponentInChildren<SpriteRenderer>().sprite = lidOpen_;
      isLidOpen_ = true;
      timeCounter = 0.0f;
    }
  }

  private void CountForClosePipe()
  {
    timeCounter += Time.deltaTime;
    if(timeCounter >= timeOpen_)
    {
      GetComponentInChildren<SpriteRenderer>().sprite = lidClosed_;
      isLidOpen_ = false;
      timeCounter = -1.0f;
    }
  }

  // Update is called once per frame
  void Update()
  {
    if(timeCounter >= 0.0f)
    {
      CountForClosePipe();
    }
  }
}
