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
  private int indextargetPipe_ = 0;

  private SpriteRenderer sr_;
  private bool isSearching_ = false;
  private MissileScript parentReference_ = null;

  [NonSerialized]
  private List<PipeScript> pipes_ = null;
  private Transform targetTr_ = null;

  public void Init()
  {
    gameObject.SetActive(false);
    sr_ = GetComponent<SpriteRenderer>();
    GameObject parent = gameObject.transform.parent.gameObject;
    parentReference_ = parent.GetComponent<MissileScript>();
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
    gameObject.SetActive(true);
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
    Vector3 dir = pipes_[indextargetPipe_].gameObject.transform.position - transform.position;

    if(dir.magnitude > 0.06f)
    {
      dir.Normalize();
      transform.position += dir * Time.deltaTime * speed_;
    }else
    {
      targetTr_ = null;
      pathCount_++;
      if (pathCount_ >= numPaths_)
      {
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
    if(gameObject.activeSelf) 
      Debug.Log("Crosshair actived");
    if(isSearching_)
    {
      SearchTarget();
    }
  }
}
