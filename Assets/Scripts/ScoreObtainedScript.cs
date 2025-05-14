using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreObtainedScript : MonoBehaviour
{
    public TextMeshProUGUI text_fill_, text_outline_;

    public float time_to_dissappear_ = 1.0f;
    public float fading_time_ = 5.0f;
    public float score_text_color_time_ = 0.5f;
    public Vector3 tint_text_color_ = new Vector3(0.0f, 1.0f, 0.0f);
    public float move_speed_ = 1.0f;

    private bool dissappearing = false;
    private float alpha_ = 0.0f, alpha2_ = 0.0f;
    private Vector3 current_score_text_color_;
    private Vector3 base_text_color_ = new Vector3(1.0f, 1.0f, 1.0f);
    private float dissappearing_timer = 0.0f;
    
    public void SetScoreText(int value)
    {
        text_fill_.text = "+" + value.ToString();
        text_outline_.text = "+" + value.ToString();
    }

    private void FadeText()
    {
        text_fill_.color = new Color(text_fill_.color.r, text_fill_.color.g, text_fill_.color.b, 1 - alpha_);
        text_outline_.color = new Color(text_outline_.color.r, text_outline_.color.g, text_outline_.color.b, 1 - alpha_);

        alpha_ += Time.deltaTime / fading_time_;
    }

    private void FixColor()
    {
        if (alpha2_ + Time.unscaledDeltaTime / score_text_color_time_ > 1.0f)
        {
            alpha2_ = 1.0f;
        }
        else
        {
            alpha2_ += Time.unscaledDeltaTime / score_text_color_time_;
        }
        current_score_text_color_ = Vector3.Lerp(tint_text_color_, base_text_color_, alpha2_);

        text_fill_.color = new Color(current_score_text_color_.x, current_score_text_color_.y, current_score_text_color_.z);
    }

    private void MoveUp()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + move_speed_ * Time.unscaledDeltaTime, transform.position.z);
    }


    private void DissappearingCounter()
    {
        dissappearing_timer += Time.unscaledDeltaTime;

        if (dissappearing_timer > time_to_dissappear_)
            dissappearing = true;
    }

    private void Update()
    {
        MoveUp();

        if (!dissappearing)
            DissappearingCounter();

        if (dissappearing)
            FadeText();

        if (alpha2_ != 1.0f)
            FixColor();

        if (alpha_ >= 1.0f)
            Destroy(gameObject);
    }
}
