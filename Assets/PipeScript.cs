using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeScript : MonoBehaviour
{
    public Sprite lid_open_, lid_closed_;
    private int is_lid_open_;

    void Start()
    {
        is_lid_open_ = Random.Range(0, 5);
        if (is_lid_open_ == 0)
        {
            GetComponentInChildren<SpriteRenderer>().sprite = lid_open_;
        } else
        {
            GetComponentInChildren<SpriteRenderer>().sprite = lid_closed_;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
