using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleModifier : MonoBehaviour
{
    public bool rotate_ = false;
    public bool scale_ = true;
    public bool start_with_random_angle_ = false;

    public float max_scale_ = 2.0f;
    public float speed_ = 1.0f;
    private Vector3 initial_scale_;
    private Vector3 initial_scale_sign_;
    // private float offset_ = 0.0f;

    public float angle_ = 10.0f;

    private void Start()
    {
        initial_scale_ = gameObject.transform.localScale;
        initial_scale_sign_ = new Vector3(Mathf.Sign(initial_scale_.x), Mathf.Sign(initial_scale_.y), 1.0f);
        if (start_with_random_angle_)
        {
            angle_ = Random.Range(2, 8);
            speed_ = Random.Range(0.2f, 1.5f);
        }
    }

    private void Update()
    {
        if (scale_)
        {
            Vector3 scale_ = initial_scale_ + initial_scale_sign_ * (Mathf.Cos(Time.fixedTime * speed_) + 1.0f) * 0.5f * max_scale_;
            gameObject.transform.localScale = new Vector3(scale_.x, scale_.y, 1);
        }
        if (rotate_) gameObject.transform.rotation = Quaternion.Euler(0, 0, Mathf.Cos(Time.fixedTime * speed_) * angle_);
    }
}
