using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeScript : MonoBehaviour
{
  public delegate void RocketCollisioned();
  public static event RocketCollisioned OnRocketCollision;

  public Sprite lidOpen_, lidClosed_;

  [NonSerialized]
  public bool isLidOpen_ = false;

  [NonSerialized]
  public SpriteRenderer sr_;
  private BoxCollider2D collider_;

  private int rocketLayer_ = 0;

  private float timeOpen_ = 0.8f;

  public void OpenPipe(ref int openedPipes, int missilesPerWave)
  {
    if(!isLidOpen_)
    {
      if (openedPipes < missilesPerWave)
      {
        OpenPipe(ref openedPipes);
      }
    }
    else
    {
      ClosePipe(ref openedPipes);
    }
  }

  private void OpenPipe(ref int openedPipes)
  {
    sr_.sprite = lidOpen_;
    isLidOpen_ = true;
    collider_.enabled = false;
    openedPipes++;
  }

  public void ClosePipe(ref int openedPipes)
  {
    sr_.sprite = lidClosed_;
    isLidOpen_ = false;
    collider_.enabled = true;
    openedPipes--;
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

      // Instantiate explosion.
      GameObject prefab = Resources.Load<GameObject>("Explosion");
      GameObject instance = Instantiate(prefab, collision.transform.position, Quaternion.identity);

      OnRocketCollision?.Invoke();
      controller.ChangeParentState(false);
    }
  }

}
