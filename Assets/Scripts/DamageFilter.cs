using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static PipeScript;
public class DamageFilter : MonoBehaviour
{
  [SerializeField]
  float maxLimit_ = 0.5f;
  [SerializeField]
  float speed_ = 1.0f;

  private Image image_ = null;
  private float alpha_ = 0.0f;

  private bool isTintActive_ = false;

  // Start is called before the first frame update
  void Start()
  {
    image_ = GetComponent<Image>();
    if (image_ == null) return;
    OnRocketCollision += StartTint;
  }

  private void OnDestroy()
  {
    OnRocketCollision -= StartTint;
  }
  
  private void StartTint()
  {
    isTintActive_ = true;
  }

  private void TintScreen()
  {
    alpha_ += Time.deltaTime * speed_;
    alpha_ = Mathf.Clamp(alpha_, 0.0f, maxLimit_);
    
    if(alpha_ >= maxLimit_)
    {
      speed_ *= -1.0f;
    }
    if (alpha_ == 0.0f)
    {
      speed_ *= -1.0f;
      isTintActive_ = false;
    }
    image_.color = new Color(image_.color.r, image_.color.g, image_.color.b, alpha_);
  }

  // Update is called once per frame
  void Update()
  {
    if (isTintActive_)
      TintScreen();
  }
}
