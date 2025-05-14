using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenBounceScript : MonoBehaviour
{
  private RectTransform obj_;
  private Image img_;
  private float width_, height_;
  private Vector2 new_pos_;
  public float object_speed_;
  private bool go_up = true, go_right = true;
  private float half_width_, half_height_;

  void Start()
  {
    obj_ = gameObject.GetComponent<RectTransform>();
    img_ = gameObject.GetComponent<Image>();

    half_width_ = obj_.rect.width * 0.5f;
    half_height_ = obj_.rect.height * 0.5f;
    new_pos_.x = obj_.localPosition.x;
    new_pos_.y = obj_.localPosition.y;

    ChangeColor();
    ChangeSpeed();
  }

  void MoveObject()
  {
    float delta = object_speed_ * Time.unscaledDeltaTime;

    if (go_right)
      new_pos_.x += delta;
    else
      new_pos_.x -= delta;

    if (go_up)
      new_pos_.y += delta;
    else
      new_pos_.y -= delta;

    obj_.localPosition = new_pos_;
  }
  
  void ChangeColor()
  {
    img_.color = new Color(Random.Range(0.5f, 1.0f), Random.Range(0.5f, 1.0f), Random.Range(0.5f, 1.0f), img_.color.a);
  }

  void ChangeSpeed()
  {
    object_speed_ = Random.Range(80, 200);
  }

  void CheckBounds()
  {
    if (new_pos_.x - half_width_ <= -960.0f){
      go_right = true;
      ChangeColor();
      ChangeSpeed();
    }
    if (new_pos_.x + half_width_ >= 960.0f) {
      go_right = false;
      ChangeColor();
      ChangeSpeed();
    }
    if (new_pos_.y - half_height_ <= -540.0f){
      go_up = true;
      ChangeColor();
      ChangeSpeed();
    }
    if (new_pos_.y + half_height_ >= 540.0f){
      go_up = false;
      ChangeColor();
      ChangeSpeed();
    }
  }
  void Update()
  {
    MoveObject();
    CheckBounds();
  }
}
