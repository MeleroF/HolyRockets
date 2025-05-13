using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeUIEffect : MonoBehaviour
{
    public float angle_fixing_speed_ = 1.0f;
    public float scale_fixing_speed_ = 0.1f;

    public Vector2 angle_range_ = new Vector2(-10.0f, 10.0f);
    public Vector2 scale_range_ = new Vector2(1.0f, 1.5f);

    private RectTransform tr_;

    private Vector3 new_rotation_;
    private Vector3 new_scale_;
    private float angle_ = 0.0f;
    private float xy_scale_ = 1.0f;

    public void ShakeObject()
    {
        angle_ = Random.Range(angle_range_.x, angle_range_.y);
        xy_scale_ = Random.Range(scale_range_.x, scale_range_.y);
    }

    private void FixAngle()
    {
        float real_fixing_speed = angle_fixing_speed_ * Time.deltaTime;
        if (angle_ > 0)
        {
            if (angle_ - real_fixing_speed < 0)
            {
                angle_ = 0;
            }
            else
            {
                angle_ -= real_fixing_speed;
            }
        }
        if (angle_ < 0)
        {
            if (angle_ + real_fixing_speed > 0)
            {
                angle_ = 0;
            } 
            else
            {
                angle_ += real_fixing_speed;
            }
        }
    }

    private void FixScale()
    {
        float real_fixing_speed = scale_fixing_speed_ * Time.deltaTime;
        if (xy_scale_ - real_fixing_speed < 1.0f)
        {
            xy_scale_ = 1.0f;
        }
        else
        {
            xy_scale_ -= real_fixing_speed;
        }
    }

    private void Start()
    {
        new_rotation_ = transform.rotation.eulerAngles;
        new_scale_ = transform.localScale;
        tr_ = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (angle_ != 0)
            FixAngle();
        if (xy_scale_ != 1)
            FixScale();

        new_rotation_.z = angle_;
        new_scale_.x = xy_scale_;
        new_scale_.y = xy_scale_;

        tr_.localScale = new_scale_;
        tr_.rotation = Quaternion.Euler(new_rotation_);
    }
}
