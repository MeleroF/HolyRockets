using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileDestroy : MonoBehaviour
{
  private int rocketLayer_ = 0;
  private SpriteMask sm_;

  // Start is called before the first frame update
  void Start()
  {
    rocketLayer_ = LayerMask.NameToLayer("Rocket");
    sm_ = GetComponent<SpriteMask>();
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.gameObject.layer == rocketLayer_)
    {
      SpriteRenderer sr = collision.gameObject.GetComponent<SpriteRenderer>();

      if (sr.sortingLayerID != sm_.backSortingLayerID) return;


      collision.gameObject.SetActive(false);
    }
  }
 
}
