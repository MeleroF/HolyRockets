using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GoToPosition : MonoBehaviour
{
    public delegate void OnStartMoving();
    public event OnStartMoving onStartMoving;

    public Transform target_;
    public bool has_arrived_ = false;
    private float speed_ = 0.001f;
    private float timer_ = 0.0f;
    public float travel_distance_ = 1;
    private Vector3 applied_travel_distance_;

    private void Start()
    {
        Vector3 forward_vector_ = (target_.position - gameObject.transform.position).normalized;
        applied_travel_distance_ = forward_vector_ * travel_distance_;
    }

    void Update()
    {
        if (!has_arrived_)
        {
            if (timer_ >= speed_)
            {
                gameObject.transform.position += applied_travel_distance_;
            }
            if (Vector2.Distance(gameObject.transform.position, target_.position) < travel_distance_)
            {
                has_arrived_ = true;
                gameObject.transform.position = target_.position;
            }
            timer_ += Time.deltaTime;
        }
    }
}
