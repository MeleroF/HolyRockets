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
  private Vector3 prevPos = Vector3.zero;

  // Start is called before the first frame update
  void Start()
  {
    OnRocketCollision += ShakeStart;
    prevPos = transform.position;
  }

  private void OnDestroy()
  {
    OnRocketCollision -= ShakeStart;
  }


  private void ShakeStart()
  {
    prevPos = transform.position;
    currentshakeForce_ = shakeForce_;
  }

  public void UpdatePrevPos(Vector3 newPos)
  {
    prevPos = new Vector3(newPos.x, newPos.y, transform.position.z);
  }

  public Vector3 GetPrevPos()
  {
    return prevPos;
  }

  private void ShakeEjecution()
  {
    float shakeValue = Mathf.Sin(Time.deltaTime * 1.0f) * currentshakeForce_;
    transform.position = new Vector3(prevPos.x + shakeValue,
                                    prevPos.y + shakeValue, 
                                    transform.position.z);
    currentshakeForce_ -= Time.deltaTime * shakeTimeOut_;
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
    else
    {
      transform.position = prevPos;
    }
  }
}
