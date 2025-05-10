using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeScript : MonoBehaviour
{
  public delegate void RocketCollisioned();
  public static event RocketCollisioned OnRocketCollision;

  public Sprite lidOpen_, lidClosed_;
  private bool isLidOpen_ = false;
  [SerializeField]

  private SpriteRenderer sr_;
  private BoxCollider2D collider_;

  private int rocketLayer_ = 0;

  private float timeOpen_ = 0.8f;
  private float timeCounter = -1.0f;

  public void OpenPipe()
  {
    if(!isLidOpen_)
    {
      sr_.sprite = lidOpen_;
      isLidOpen_ = true;
      timeCounter = 0.0f;
      collider_.enabled = false;
    }
  }

  private void CountForClosePipe()
  {
    timeCounter += Time.deltaTime;
    if(timeCounter >= timeOpen_)
    {
      sr_.sprite = lidClosed_;
      isLidOpen_ = false;
      timeCounter = -1.0f;
      collider_.enabled = true;
    }
  }

  private void Start()
  {
    sr_ = GetComponentInChildren<SpriteRenderer>();
    collider_ = GetComponent<BoxCollider2D>();
    rocketLayer_ = LayerMask.NameToLayer("Rocket");
  }

  // Update is called once per frame
  void Update()
  {
    if(timeCounter >= 0.0f)
    {
      CountForClosePipe();
    }
  }
  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.gameObject.layer == rocketLayer_)
    {
      SpriteRenderer sr = collision.gameObject.GetComponent<SpriteRenderer>();

      if (sr.sortingLayerID != sr_.sortingLayerID) return;

      OnRocketCollision?.Invoke();
      collision.gameObject.SetActive(false);
    }
  }

}
