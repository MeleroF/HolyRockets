using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
  private bool isPaused = false;
  public bool safety_options_on_ = false;
  public GameObject pause_icon_, play_icon_, background_, home_button_, yes_button_, no_button_, bouncy_logo_;
  public ScaleModifier audio_settings_sm_;
  public ScaleModifier scale_modifier_;
  private float pause_initial_speed_, pause_initial_angle_, audio_setting_initial_angle_;

  private void Start()
  {
    pause_initial_speed_ = scale_modifier_.speed_;
    pause_initial_angle_ = scale_modifier_.angle_;
    audio_setting_initial_angle_ = audio_settings_sm_.angle_;
  }

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Return))
    {
      TogglePause();
    }
  }

  public void ToggleSafetyOptions()
  {
    if (safety_options_on_)
    {
      yes_button_.SetActive(false);
      no_button_.SetActive(false);
    }
    else
    {
      yes_button_.SetActive(true);
      no_button_.SetActive(true);
    }
    safety_options_on_ = !safety_options_on_;
  }

  public void TogglePause()
  {
    if (isPaused)
    {
      // Resuming
      pause_icon_.SetActive(true);
      play_icon_.SetActive(false);
      background_.SetActive(false);
      bouncy_logo_.SetActive(false);
      home_button_.gameObject.SetActive(false);
      scale_modifier_.speed_ = pause_initial_speed_;
      scale_modifier_.angle_ = pause_initial_angle_;
      audio_settings_sm_.angle_ = audio_setting_initial_angle_;
      Time.timeScale = 1;
    }
    else
    {
      // Pausing
      pause_icon_.SetActive(false);
      play_icon_.SetActive(true);
      background_.SetActive(true);
      bouncy_logo_.SetActive(true);
      home_button_.gameObject.SetActive(true);
      scale_modifier_.speed_ = 8;
      scale_modifier_.angle_ = 10;
      audio_settings_sm_.angle_ = 0;
      Time.timeScale = 0;
    }
    isPaused = !isPaused;
  }
}
