using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairScript : MonoBehaviour
{
  
  [SerializeField]
  private float searchSpeed_ = 2.0f;
  [SerializeField]
  private float blinkLapseSpeed_ = 0.2f;
  [SerializeField]
  private float blinkSpeed_ = 0.1f;

  private float blinkDuration_ = 3.0f;
  private float blinkLapse_ = 1.0f;


  private int numPaths_ = 0;
  private int pathCount_ = 0;
  private int rowTarget_ = 0;
  private int indextargetPipe_ = 0;

  private SpriteRenderer sr_;
  private bool isSearching_ = false;
  private FallMarker parentReference_ = null;

  [NonSerialized]
  private List<PipeScript> pipes_ = null;
  private Transform targetTr_ = null;
  private Transform followTr_ = null;

  [NonSerialized]
  public Sprite crosshairSearching_;
  [NonSerialized]
  public Sprite crosshairPointing_;
  [NonSerialized]
  public Sprite crosshairBlink_;

  public void Init()
  {
    gameObject.SetActive(false);
    sr_ = GetComponent<SpriteRenderer>();
    GameObject parent = gameObject.transform.parent.gameObject;
    parentReference_ = parent.GetComponent<FallMarker>();
  }

  public void StartSearching(ref List<PipeScript> pipes, int numPaths, float speedFactor)
  {
    if(pipes_ == null)
    {
      pipes_ = pipes;
    }
    searchSpeed_ = Mathf.Min(17.0f, searchSpeed_ + (speedFactor * 0.4f));
    int randPipe = UnityEngine.Random.Range(0, pipes_.Count);
    transform.position = pipes_[randPipe].gameObject.transform.position;
    blinkSpeed_ *= speedFactor * 0.8f;
    blinkLapseSpeed_ *= speedFactor * 1.25f;
    numPaths_ = numPaths;
    followTr_ = null;
    gameObject.SetActive(true);
    sr_.sprite = crosshairSearching_;
    isSearching_ = true;
  }

  private IEnumerator CountForLaunchMissile()
  {
    yield return new WaitForSeconds(Mathf.Max( blinkDuration_ - blinkSpeed_, 0.8f));
    parentReference_.Spawn(rowTarget_, transform.position);
    gameObject.SetActive(false);

  }

  private void AvanceToTarget()
  {
    Vector3 dir = pipes_[indextargetPipe_].gameObject.transform.position - transform.position;

    if(dir.magnitude > 0.06f)
    {
      dir.Normalize();
      transform.position += dir * Time.deltaTime * searchSpeed_;
    }else
    {
      followTr_ = targetTr_;
      targetTr_ = null;
      pathCount_++;
      if (pathCount_ >= numPaths_)
      {
        parentReference_.target_ = followTr_.gameObject.GetComponent<PipeScript>();
        parentReference_.target_.counterTargeted++;
        isSearching_ = false;
        StartCoroutine(Blink());
        StartCoroutine(CountForLaunchMissile());
        pathCount_ = 0;
      }
      
    }
  }

  private void SearchTarget()
  {
    if(targetTr_ == null)
    {
      indextargetPipe_ = UnityEngine.Random.Range(0, pipes_.Count);
      targetTr_ = pipes_[indextargetPipe_].gameObject.transform;
      rowTarget_ = pipes_[indextargetPipe_].sr_.sortingOrder;
    }
    else
    {
      AvanceToTarget();
    }
  }

  private IEnumerator Blink()
  {
    yield return new WaitForSeconds(Mathf.Max(blinkLapse_ - blinkLapseSpeed_, 0.2f));
    if (gameObject.activeSelf)
    {
      if (sr_.sprite == crosshairPointing_)
      {
        sr_.sprite = crosshairBlink_;
      }
      else
      {
        sr_.sprite = crosshairPointing_;
      }
    
      StartCoroutine(Blink());
    }
  }

  private void Update()
  {
    if(isSearching_)
    {
      SearchTarget();
    }else if (followTr_)
    {
      transform.position = followTr_.position;
    }
  }
}
