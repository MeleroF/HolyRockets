using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Remote : MissileScript
{
  public float blinkLapse_ = 0.5f;

  [SerializeField]
  private Vector3 gravity = Vector3.one;
  [SerializeField]
  private Sprite originalAspect_;
  [SerializeField]
  private Sprite blinkAspect_;

  [SerializeField]
  private float blinkTime_ = 1.5f;

  [NonSerialized]
  public Shadow shadow_;
  private float differenceHeightShadow_;

  [NonSerialized]
  public bool isFalling_ = false;
  private Quaternion starterRotation_ = Quaternion.identity;
  private Quaternion fallRotation_ = Quaternion.identity;
  private Quaternion targetRotation_ = Quaternion.identity;
  private float currentBlinkLapse_ = 0.0f;
  private float alpha_ = 0.0f;
  private Vector3 speedWithGravity = Vector3.zero;
 

  protected override void Awake()
  { 
  }

  private IEnumerator Blink()
  {
    yield return new WaitForSeconds(currentBlinkLapse_);

    if(sr_.sprite == originalAspect_)
      sr_.sprite = blinkAspect_;
    else
      sr_.sprite = originalAspect_;
    StartCoroutine(Blink());
  }

  public override void Spawn(int assignedRow, Vector3 pos)
  {
    targetRotation_ = Quaternion.Euler(0f, 0f, 90f);
    sr_.sortingLayerName = $"RocketRow{assignedRow + 1}";
    sr_.sortingOrder = assignedRow + 2;
    currentBlinkLapse_ = blinkLapse_;
    alpha_ = 0.0f;
    isFalling_ = false;
    speedWithGravity = Vector3.zero;
    transform.rotation = starterRotation_;
    transform.position = new Vector3(shadow_.transform.position.x, shadow_.transform.position.y + differenceHeightShadow_);
    StartCoroutine(Blink());
  }

  private IEnumerator BlinkTime()
  {
    yield return new WaitForSeconds(blinkTime_);
    isFalling_ = true;
    shadow_.maskGettingCloser_ = true;
    shadow_.initShadowMaskLeftPos = shadow_.shadhowMaskLeftTr_.position;
    shadow_.initShadowMaskRightPos = shadow_.shadhowMaskRightTr_.position;
    fallRotation_ = transform.localScale.x < 0 ? Quaternion.Euler(0.0f, 0.0f, -90.0f) : targetRotation_;
  }

  public void IsTimeToFall()
  {
    currentBlinkLapse_ *= 0.5f;

    StartCoroutine(BlinkTime());
  }

  public override void Init(ref Sprite sprite)
  {
    shadow_ = transform.parent.gameObject.GetComponent<Shadow>();
    differenceHeightShadow_ = transform.position.y - shadow_.transform.position.y;
    sr_ = GetComponent<SpriteRenderer>();
    sr_.sprite = sprite;
  }

  private void FallWithGravity()
  {
    speedWithGravity = gravity * Time.deltaTime;
    alpha_ += Time.deltaTime * Mathf.Abs(speedWithGravity.y) * 150.0f;
    alpha_ = Mathf.Clamp01(alpha_);
    transform.rotation = Quaternion.Slerp(starterRotation_, fallRotation_, alpha_);
    transform.position += speedWithGravity;
  }

  protected override void Update()
  {
    base.Update();
    if(isFalling_)
    {
      FallWithGravity();
    }
  }
}
