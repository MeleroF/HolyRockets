using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MissileScript : MonoBehaviour
{

  public RocketStats stats_;
  protected float spawnPosY = 0.0f;
  protected bool mustFall_ = false;

  [NonSerialized]
  public SpriteRenderer sr_;

  [NonSerialized]
  public PipeScript target_ = null;

  // Start is called before the first frame update
  protected abstract void Awake();

  public virtual void Init(ref Sprite sprite)
  {
    sr_.sprite = sprite;
  }

  // Update is called once per frame
  public abstract void Spawn(int assignedRow, Vector3 pos);

  private void LetFallMissile()
  {
    transform.position = new Vector3(transform.position.x, transform.position.y + Time.deltaTime * stats_.fallSpeed_, transform.position.z);
  }

  protected virtual void Update()
  {
    if(mustFall_)
      LetFallMissile();
  }

 
}
