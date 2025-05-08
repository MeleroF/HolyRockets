using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeScript : MonoBehaviour
{
  public Sprite lid_open_, lid_closed_;
  private int is_lid_open_;
  private bool isLidOpen_;
    
  public void ChangeStatePipe()
  {
    if(isLidOpen_)
    {
      GetComponentInChildren<SpriteRenderer>().sprite = lid_closed_;
      isLidOpen_ = false;
    }
    else
    {
      GetComponentInChildren<SpriteRenderer>().sprite = lid_open_;
      isLidOpen_ = true;
    }
  }
  
  void Start()
  {
    is_lid_open_ = Random.Range(0, 5);
    if (is_lid_open_ == 0)
      isLidOpen_ = true;
    else
      isLidOpen_ = false;
    ChangeStatePipe();
  }

  // Update is called once per frame
  void Update()
  {
        
  }
}
