using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CoverScreenScript : MonoBehaviour
{
    public bool hidden_ = true;
    public bool changes_the_scene_ = false;
    public float change_scene_time_ = 1.5f;
    public float duration_ = 1.0f;

    private float alpha_ = 0.0f;
    private Image img_;
    private float timer_ = 0.0f;

    private void Awake()
    {
        img_ = GetComponent<Image>();
        if (hidden_)
        {
            alpha_ = 0.0f;
        }
        else
        {
            alpha_ = 1.0f;
        }
    }

    private void Transition()
    {
        if (hidden_)
        {
            if (alpha_ != 1.0f)
            {
                if (alpha_ + Time.deltaTime / duration_ > 1.0f)
                {
                    alpha_ = 1.0f;
                }
                else
                {
                    alpha_ += Time.deltaTime / duration_;
                }
            }
        }
        else
        {
            if (alpha_ != 0.0f)
            {
                if (alpha_ - Time.deltaTime / duration_ < 0.0f)
                {
                    alpha_ = 0.0f;
                    gameObject.SetActive(false);
                }
                else
                {
                    alpha_ -= Time.deltaTime / duration_;
                }
            }
        }

        img_.color = new Color(img_.color.r, img_.color.g, img_.color.b, alpha_);
    }

    private void ChangeScenesCounter(){
        timer_ += Time.deltaTime;
        if (timer_ > change_scene_time_)
        {
            SceneManager.LoadScene(1);
        }
    }

    private void Update()
    {
        if (changes_the_scene_)
            ChangeScenesCounter();

        Transition();
    }
}
