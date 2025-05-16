using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class Remote : MissileScript
{
  [SerializeField]
  private Sprite blinkAspect_;

  [NonSerialized]
  public Shadow shadow_;
  private float differenceHeightShadow_;

  protected override void Awake()
  {
    sr_ = GetComponent<SpriteRenderer>();
    shadow_ = transform.parent.gameObject.GetComponent<Shadow>();
    differenceHeightShadow_ = transform.position.y - shadow_.transform.position.y;
  }

  public override void Spawn(int assignedRow, Vector3 pos)
  {
    sr_.sortingLayerName = $"RocketRow{assignedRow}";
    sr_.sortingOrder = assignedRow + 1;
  }
}
