using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScript : MonoBehaviour
{
    public float speed_ = 1.0f;
    private Vector3 new_pos_;
    private float limit_;

    void Start()
    {
        new_pos_ = transform.position;
        limit_ = transform.localScale.x;
    }

    void FixedUpdate()
    {
        if (new_pos_.x >= limit_)
        {
            new_pos_.x = 0;
        }
        transform.position = new_pos_;
        new_pos_.x += speed_;
    }
}
