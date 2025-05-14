using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    public static HUDManager instance_;

    public RectTransform score_;
    public float score_value_;
    public float fake_score_;
    public float fake_score_delay_timer_;
    public float fake_score_delay_ = 0.005f;
    public Vector3 tint_score_text_color_ = new Vector3(0.0f,1.0f,0.0f);

    private TextMeshProUGUI score_fill_, score_outline_;
    public float score_text_color_time_ = 2.0f;
    private float alpha_ = 1.0f;

    private Vector3 current_score_text_color_;
    private Vector3 initial_score_text_color_;

    private void Awake()
    {
        instance_ = this;

        Transform fill = score_.GetChild(0);
        score_fill_ = fill.GetComponent<TextMeshProUGUI>();
        Transform outline = score_.GetChild(1);
        score_outline_ = outline.GetComponent<TextMeshProUGUI>();

        initial_score_text_color_.x = score_fill_.color.r;
        initial_score_text_color_.y = score_fill_.color.g;
        initial_score_text_color_.z = score_fill_.color.b;
    }

    private void FixColor()
    {
        if (alpha_ + Time.deltaTime / score_text_color_time_ > 1.0f)
        {
            alpha_ = 1.0f;
        } 
        else
        {
            alpha_ += Time.deltaTime / score_text_color_time_;
        }
        current_score_text_color_ = Vector3.Lerp(tint_score_text_color_, initial_score_text_color_, alpha_);
        score_fill_.color = new Color(current_score_text_color_.x, current_score_text_color_.y, current_score_text_color_.z);
    }

    private void Update()
    {
        if (fake_score_ != score_value_)
            UpdateScore();

        if (alpha_ != 1.0f)
            FixColor();
    }

    void UpdateScoreText()
    {
        score_fill_.text = fake_score_.ToString();
        score_outline_.text = fake_score_.ToString();
    }

    private void UpdateScore()
    {
        fake_score_delay_timer_ += Time.deltaTime;
        if (fake_score_delay_timer_ > fake_score_delay_)
        {
            fake_score_++;
            UpdateScoreText();
            fake_score_delay_timer_ = 0;
        }
    }
    

    public void ScoreUpdatedEffect()
    {
        score_.GetComponent<ShakeUIEffect>().ShakeObject();
        alpha_ = 0.0f;
    }

    public void UpdateScoreValue(int amount)
    {
        ScoreUpdatedEffect();
        fake_score_ = score_value_;
        score_value_ += amount;

        if (score_value_ < 0) score_value_ = 0;

        if (amount < 0)
        {
            fake_score_ = score_value_;
            UpdateScoreText();
        }
    }

}
