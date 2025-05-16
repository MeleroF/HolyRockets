using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.tvOS;

public class Shadow : MonoBehaviour
{
  private GameObject shadhowMaskLeft_ = null;
  private GameObject shadhowMaskRight_ = null;

  private SpriteRenderer sr_;

  [NonSerialized]
  public Remote remote_;
  private void SetMaskToExactPos()
  {
    shadhowMaskLeft_.transform.position =  new Vector3(sr_.bounds.center.x - sr_.bounds.size.x * 0.5f, shadhowMaskLeft_.transform.position.y);
    shadhowMaskRight_.transform.position = new Vector3(sr_.bounds.size.x * 0.5f + sr_.bounds.center.x, shadhowMaskRight_.transform.position.y);
  }

  // Start is called before the first frame update
  void Start()
  {
    sr_ = GetComponent<SpriteRenderer>();
    shadhowMaskLeft_ = transform.GetChild(0).gameObject;
    shadhowMaskRight_ = transform.GetChild(1).gameObject;

    remote_ = transform.GetChild(2).GetComponent<Remote>();

    SetMaskToExactPos();
  }

  // Update is called once per frame
  void Update()
  {
    
  }
}
