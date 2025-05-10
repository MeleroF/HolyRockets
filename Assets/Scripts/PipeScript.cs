using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeScript : MonoBehaviour
{
  public delegate void RocketCollisioned();
  public static event RocketCollisioned OnRocketCollision;

  public Sprite lidOpen_, lidClosed_;
  private bool isLidOpen_ = false;

  [NonSerialized]
  public SpriteRenderer sr_;
  private BoxCollider2D collider_;

  private int rocketLayer_ = 0;

  private float timeOpen_ = 0.8f;

  public void OpenPipe()
  {
    if(!isLidOpen_)
    {
      sr_.sprite = lidOpen_;
      isLidOpen_ = true;
      collider_.enabled = false;
      StartCoroutine(CountForClosePipe());
    }
  }

  private IEnumerator CountForClosePipe()
  {
    yield return new WaitForSeconds(timeOpen_);

    sr_.sprite = lidClosed_;
    isLidOpen_ = false;
    collider_.enabled = true;
   
  }

  private void Start()
  {
    sr_ = GetComponentInChildren<SpriteRenderer>();
    collider_ = GetComponent<BoxCollider2D>();
    rocketLayer_ = LayerMask.NameToLayer("Rocket");
  }


  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.gameObject.layer == rocketLayer_)
    {
      SpriteRenderer sr = collision.gameObject.GetComponent<SpriteRenderer>();
      ParentController controller = collision.gameObject.GetComponent<ParentController>();
      if (sr.sortingLayerID != sr_.sortingLayerID) return;

      OnRocketCollision?.Invoke();
      controller.ChangeParentState(false);
    }
  }

}
