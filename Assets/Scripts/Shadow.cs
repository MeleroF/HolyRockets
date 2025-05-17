using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.tvOS;

public class Shadow : MonoBehaviour
{
  [SerializeField]
  private float strollSpeed_ = 1.0f;

  [SerializeField]
  private float speedAproach_ = 1.0f;

  [NonSerialized]
  public Transform shadhowMaskLeftTr_ = null;
  [NonSerialized]
  public Transform shadhowMaskRightTr_ = null;

  [NonSerialized]
  public Vector3 initShadowMaskLeftPos = Vector3.zero;
  [NonSerialized]
  public Vector3 initShadowMaskRightPos = Vector3.zero;

  private SpriteRenderer sr_ = null;
  private List<PipeScript> pipes_ = null;

  private int selectedRow_ = 0;
  private int numColsPipes_ = 0;

  private int pathCounter_ = 0;
  private int targetPaths_ = 0;

  private float spawnPosXLeft = 0;
  private float spawnPosXRight = 0;

  [NonSerialized]
  public bool maskGettingCloser_ = false;

  private Transform targetTr_ = null;

  private Transform leftTr_ = null;
  private Transform rightTr_ = null;

  private float alpha_ = 0.0f;
  private bool strollCompleted_ = true;

  [NonSerialized]
  public Remote remote_;
  private void SetMaskToExactPos()
  {
    shadhowMaskLeftTr_.position =  new Vector3(transform.position.x - sr_.bounds.size.x * 0.5f, transform.position.y);
    shadhowMaskRightTr_.position = new Vector3(sr_.bounds.size.x * 0.5f + transform.position.x, transform.position.y);
  }

  private void GetBothPosSpawn()
  {
    Camera camera = Camera.main;
    float halfHeight = camera.orthographicSize;
    float halfWidth = halfHeight * camera.aspect;
    spawnPosXLeft = camera.transform.position.x - (halfWidth + sr_.bounds.size.x);
    spawnPosXRight = camera.transform.position.x + (halfWidth + sr_.bounds.size.x);
  }

  // Start is called before the first frame update
  private void Awake()
  {
    sr_ = GetComponent<SpriteRenderer>();
    sr_.sortingOrder = 2;
    shadhowMaskLeftTr_ = transform.GetChild(0).transform;
    shadhowMaskRightTr_ = transform.GetChild(1).transform;
    gameObject.SetActive(false);
    GetBothPosSpawn();
    remote_ = transform.GetChild(2).GetComponent<Remote>();

    SetMaskToExactPos();
  }


  private void AproachMasks()
  {
    float maxAproach = 0.50f;
    alpha_ += Time.deltaTime * speedAproach_ ;
    alpha_ = Mathf.Clamp(alpha_, 0.0f, maxAproach);
    shadhowMaskLeftTr_.position = Vector3.Lerp(initShadowMaskLeftPos, transform.position, alpha_);
    shadhowMaskRightTr_.position = Vector3.Lerp(initShadowMaskRightPos, transform.position, alpha_);
    if(alpha_ == maxAproach)
    {
      maskGettingCloser_ = false;
    }
  }

  // Update is called once per frame
  void Update()
  {
    if(!strollCompleted_)
      Stroll();
    if (maskGettingCloser_)
      AproachMasks();
  }

  private void AdvanceToTarget()
  {
    Vector3 dir = targetTr_.transform.position - transform.position;
    if (dir.magnitude > 0.06f)
    {
      dir.Normalize();
      transform.position += dir * Time.deltaTime * strollSpeed_;
    }
    else
    {
      if (targetTr_ == leftTr_)
        targetTr_ = rightTr_;
      else
        targetTr_ = leftTr_;

      pathCounter_++;
      if(pathCounter_ <= targetPaths_)
      {
        remote_.sr_.flipX = !remote_.sr_.flipX;
      }
      if (pathCounter_ == targetPaths_)
        GenereateFinalTarget();
    }
  }

  private void GenereateFinalTarget()
  {
    int randCol = UnityEngine.Random.Range(0, numColsPipes_);
    targetTr_ = pipes_[selectedRow_ * numColsPipes_ + randCol].transform;
  }
  private void Stroll()
  {
    if (pathCounter_ <= targetPaths_)
    {
      AdvanceToTarget();
    }else
    {
      strollCompleted_ = true;
      remote_.IsTimeToFall();
    }
  }

  public void StartStrolling(ref List<PipeScript> pipes, int numPaths, float speedFactor, 
                             int selectedRow, int numCols)
  {
    if(pipes_ == null)
    {
      pipes_ = pipes;
    }
    pathCounter_ = 0;
    targetPaths_ = numPaths;

    numColsPipes_ = numCols;
    selectedRow_ = selectedRow;

    gameObject.SetActive(true);

    remote_.Spawn(selectedRow, Vector3.one);
    remote_.sr_.flipX = false;

    strollCompleted_ = false;

    leftTr_ = pipes_[selectedRow * numCols + 0].transform;
    rightTr_ = pipes_[selectedRow * numCols + (numCols - 1)].transform;
    float posRow = leftTr_.position.y;
    bool spawnLeft = UnityEngine.Random.value < 0.5f;
    transform.position = new Vector3(spawnLeft ? spawnPosXLeft : spawnPosXRight, posRow);
    SetMaskToExactPos();
    targetTr_ = spawnLeft ? rightTr_ : leftTr_;
    if(targetTr_ == rightTr_)
    {
      remote_.sr_.flipX = true;
    }
  }
}
