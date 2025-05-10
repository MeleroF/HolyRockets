using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileDestroy : MonoBehaviour
{
  public delegate void MissileCatched();
  public static event MissileCatched OnMissileCatch;

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
      ParentController controller = collision.gameObject.GetComponent<ParentController>();

      if (sr.sortingLayerID != sm_.backSortingLayerID) return;

      OnMissileCatch?.Invoke();

      controller.ChangeParentState(false);
    }
  }
 
}
