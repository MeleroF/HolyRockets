using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PipeScript;

public class Ship : MonoBehaviour
{

  [SerializeField]
  private float leanDuration_ = 1.0f;
  [SerializeField]
  private float leanSpeed_ = 3.0f;
  [SerializeField]
  private float leanDegrees_ = 15.0f;
  [SerializeField]
  private float swingTimeOut_ = 1.0f;
  [SerializeField]
  private float offsetPosLean_ = 10.0f;
  [SerializeField]
  private float goDownSpeed_ = 2.0f;

  private float alphaLerp_ = 0.0f;
  private float alphaDown_ = 0.0f;
  private Quaternion prevRotation_ = Quaternion.identity;
  private Quaternion targetRotation_ = Quaternion.identity;
  private float targetDegree_ = 0.0f;
  private int zeroCounter_ = 0;

  private Vector3 camInitPos_ = Vector3.zero;
  private Vector3 shipInitPos_ = Vector3.zero;

  private Vector3 camCurrentPos_ = Vector3.zero;
  private Vector3 shipCurrentPos_ = Vector3.zero;

  private Vector3 camTargetPos_ = Vector3.zero;
  private Vector3 shipTargetPos_ = Vector3.zero;

  private TurbulenceEffect turbulenceEffect_ = null;
  private ShakeEffect camera_;

  private enum ShipState
  {
    NONE,
    LEANING,
    SWINGING
  }

  private bool isLeaned_ = false;
  private bool isDown_ = false;
  private ShipState shipState_ = ShipState.NONE;

  // Start is called before the first frame update
  void Start()
  {
    camera_ = Camera.main.gameObject.GetComponent<ShakeEffect>();
    turbulenceEffect_ = GetComponent<TurbulenceEffect>();

    camInitPos_ = camera_.transform.position;
    shipInitPos_ = transform.position;

    camTargetPos_ = new Vector3(camera_.transform.position.x, camera_.transform.position.y + offsetPosLean_);
    shipTargetPos_ = new Vector3(transform.position.x, transform.position.y + offsetPosLean_);
    
    OnRocketCollision += TakeDamage;
    targetRotation_ = Quaternion.Euler(0.0f, 0.0f, leanDegrees_);
  }

  private void OnDestroy()
  {
    OnRocketCollision -= TakeDamage;
  }

  private void TakeDamage()
  {
    alphaLerp_ = 0.0f;
    alphaDown_ = 0.0f;
    shipState_ = ShipState.LEANING;
    prevRotation_ = transform.rotation;
    camCurrentPos_ = camera_.GetPrevPos();
    isLeaned_ = false;
    isDown_ = false;
    shipCurrentPos_ = turbulenceEffect_.GetStartPos();
  }
  private void Lean()
  {
    alphaLerp_ += Time.deltaTime * leanSpeed_;
    transform.rotation = Quaternion.Slerp(prevRotation_, targetRotation_, alphaLerp_);
    if(alphaLerp_ >= 1.0f)
    {
      alphaLerp_ = 0.0f;
      targetDegree_ = leanDegrees_;
      isLeaned_ = true;
    }

  }
  private void Swing()
  {
    alphaLerp_ += Time.deltaTime * leanSpeed_;

    turbulenceEffect_.UpdateStartPos(Vector3.Lerp(shipTargetPos_, shipInitPos_, alphaLerp_));

    camera_.UpdatePrevPos(Vector3.Lerp(camTargetPos_, camInitPos_, alphaLerp_));

    transform.rotation = Quaternion.Euler(0.0f, 0.0f, targetDegree_ * Mathf.Cos(Time.deltaTime));
    targetDegree_ -= Time.deltaTime * swingTimeOut_;
    if(targetDegree_ <= 0.0f)
    {
      zeroCounter_++;
      if(zeroCounter_ == 1)
      {
        targetDegree_ = 0.0f;
      }else
      {
        alphaLerp_ = 0.0f;
        shipState_ = ShipState.NONE;
        zeroCounter_ = 0;
      }
    }
  }

  private void GoDown()
  {
    alphaDown_ += Time.deltaTime * goDownSpeed_;

    alphaDown_ = Mathf.Clamp01(alphaDown_);
    turbulenceEffect_.UpdateStartPos(Vector3.Lerp(shipCurrentPos_, shipTargetPos_, alphaDown_));

    camera_.UpdatePrevPos(Vector3.Lerp(camCurrentPos_, camTargetPos_, alphaDown_));

    if(alphaDown_ == 1.0f)
    {
      alphaDown_ = 0.0f;
      isDown_ = true;
      shipState_ = ShipState.SWINGING;
    }
  }
  // Update is called once per frame
  void Update()
  {
    switch(shipState_)
    {
      case ShipState.LEANING:
        if(!isLeaned_)
          Lean();
        GoDown();
        break;
      case ShipState.SWINGING:
        Swing();
        break;
      default:
        break;
    } 
  }
}
