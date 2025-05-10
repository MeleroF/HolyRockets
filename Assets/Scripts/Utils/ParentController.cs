using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ParentController : MonoBehaviour
{
  private SpriteRenderer sr_;
  private Collider2D collider_;
  private MonoBehaviour[] scripts_;

  [NonSerialized]
  public bool isEnabled_ = false;
    // Start is called before the first frame update
  public void Start()
  {
    sr_ = GetComponent<SpriteRenderer>();
    collider_ = GetComponent<Collider2D>();
    scripts_ = GetComponents<MonoBehaviour>().Where(m => m != this).ToArray();
  }

  public void ChangeParentState(bool state)
  {
    if(sr_)
    {
      sr_.enabled = state;
    }
    if (collider_)
    {
      collider_.enabled = state;
    }
  
    foreach(MonoBehaviour s in scripts_)
    {
      if(s != null)
      {
        s.enabled = state;
      }
    }
    
    isEnabled_ = state;
  }

}
