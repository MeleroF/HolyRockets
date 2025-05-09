using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UpcomingRocketScript : MonoBehaviour
{

  public delegate void RocketDestroid();
  public static event RocketDestroid OnRocketDestroid;

  public TrailRenderer trail_;

  public Vector2 speed_range_ = new Vector2(3.0f, 8.0f);
  public Vector2 deviation_range_ = new Vector2(-30.0f, 30.0f);

  private Vector3 rotation_;
  private Vector3 spawnPos_;

  private float speed_;
  private float deviation_;

  private int depth_;

  private void OnDestroy()
  {
      // TO DO
  }

  private void GetRandomValues()
  {
    depth_ = Random.Range(0, 2);
    speed_ = Random.Range(speed_range_.x, speed_range_.y);
    deviation_ = Random.Range(deviation_range_.x, deviation_range_.y);
  }

  public void Init(Vector3 position)
  {
    ApplyDepth();
    spawnPos_ = position;
    Spawn();
  }

  public void Spawn()
  {
    gameObject.SetActive(true);
    transform.position = spawnPos_;
    GetRandomValues();
    StartCoroutine(DesactivateRockets());
  }

  private IEnumerator DesactivateRockets()
  {
    yield return new WaitForSeconds(10.0f / speed_);
    gameObject.SetActive(false);
    OnRocketDestroid?.Invoke();
  }

  private void ChangeTrailCurve(float value)
  {
    AnimationCurve widthCurve = trail_.widthCurve;
    Keyframe[] keys = widthCurve.keys;

    keys[0].value = value;

    AnimationCurve newCurve = new AnimationCurve(keys);
    trail_.widthCurve = newCurve;
  }

  private void ApplyDepth()
  {
    if (depth_ == 0)
    {
        GetComponent<SpriteRenderer>().sortingOrder = -2;
        trail_.sortingOrder = -2;
        transform.localScale = new Vector2(0.1f,0.1f);
        ChangeTrailCurve(0.1f);
    }
    else
    {
        GetComponent<SpriteRenderer>().sortingOrder = -4;
        trail_.sortingOrder = -4;
        transform.localScale = new Vector2(0.05f, 0.05f);
        ChangeTrailCurve(0.05f);
    }
  }

  private void ApplyDeviation()
  {
    rotation_.z += deviation_ * Time.deltaTime;
    transform.rotation = Quaternion.Euler(rotation_);
  }

  private void MoveRocket()
  {
    transform.position += transform.up * speed_ * Time.deltaTime;
  }

  private void Update()
  {
      MoveRocket();
      ApplyDeviation();
  }
}
