using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairScript : MonoBehaviour
{
  [SerializeField]
  private Sprite crosshairSearching_;
  [SerializeField]
  private Sprite crosshairPointing_;
  [SerializeField]
  private Sprite crosshairBlink_;
  [SerializeField]
  private float speed_ = 2.0f;


  private float blinkDuration_ = 2.0f;
  private float blinkLapse_ = 1.0f;


  private int numPaths_ = 0;
  private int pathCount_ = 0;
  private int rowTarget_ = 0;

  private SpriteRenderer sr_;
  private bool isSearching_ = false;
  private MissileScript parentReference_ = null;

  [NonSerialized]
  private List<PipeScript> pipes_ = null;
  private Transform targetTr_ = null;

  private void Start()
  {
    gameObject.SetActive(false);
    sr_ = GetComponent<SpriteRenderer>();
    parentReference_ = GetComponentInParent<MissileScript>();
  }

  public void StartSearching(ref List<PipeScript> pipes, int numPaths, float blinkDuration)
  {
    if(pipes_ == null)
    {
      pipes_ = pipes;
    }
    int randPipe = UnityEngine.Random.Range(0, pipes_.Count);
    transform.position = pipes_[randPipe].gameObject.transform.position;
    blinkDuration_ = blinkDuration;
    numPaths_ = numPaths;
    sr_.sprite = crosshairSearching_;
    isSearching_ = true;
  }

  private IEnumerator CountForLaunchMissile()
  {
    yield return new WaitForSeconds(blinkDuration_);
    parentReference_.Spawn(rowTarget_, transform.position);
    gameObject.SetActive(false);
  }

  private void AvanceToTarget()
  {
    Vector3 dir = transform.position - targetTr_.position;

    if(dir.magnitude < 1.0f)
    {
      dir.Normalize();
      transform.position += dir * Time.deltaTime * speed_;
    }else
    {
      targetTr_ = null;
      if(pathCount_ >= numPaths_)
      {
        isSearching_ = false;
        StartCoroutine(Blink());
        StartCoroutine(CountForLaunchMissile());
        pathCount_ = 0;
      }
      else
      {
        pathCount_++;
      }
      
    }
  }

  private void SearchTarget()
  {
    if(targetTr_ == null)
    {
      int randPipe = UnityEngine.Random.Range(0, pipes_.Count);
      targetTr_ = pipes_[randPipe].gameObject.transform;
      rowTarget_ = pipes_[randPipe].sr_.sortingOrder - 1;
    }
    else
    {
      AvanceToTarget();
    }
  }

  private IEnumerator Blink()
  {
    yield return new WaitForSeconds(blinkLapse_);
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
    }
  }
}
