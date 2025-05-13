using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PipeScript;

public class ShakeEffect : MonoBehaviour
{
  [SerializeField]
  private float shakeTimeOut_ = 1.0f;
  [SerializeField]
  private float shakeForce_ = 1.0f;

  private float currentshakeForce_ = 0.0f;
  float prevXAxisVl = 0.0f;
  float prevYAxisVl = 0.0f;

  // Start is called before the first frame update
  void Start()
  {
    OnRocketCollision += ShakeStart;
  }

  private void ShakeStart()
  {
    prevXAxisVl = transform.position.x;
    prevYAxisVl = transform.position.y;
    currentshakeForce_ = shakeForce_;
  }

  private void ShakeEjecution()
  {
    float shakeValue = Mathf.Sin(Time.deltaTime * 1.0f) * currentshakeForce_;
    transform.position = new Vector3(prevXAxisVl + shakeValue,
                                    prevYAxisVl + shakeValue, 
                                    transform.position.z);
    currentshakeForce_ -= Time.deltaTime * shakeTimeOut_;
    Debug.Log(currentshakeForce_);
    if(currentshakeForce_ <= 0.0f || currentshakeForce_ > 0.0f && currentshakeForce_ < 0.1f)
    {
      currentshakeForce_ = 0.0f;
    }
  }

  // Update is called once per frame
  void Update()
  {
    if (currentshakeForce_ != 0.0f)
      ShakeEjecution();
  }
}
