using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using TMPro;
using static LifeManager;

public class GameOverMenuScript : MonoBehaviour
{
  public CoverScreenScript cover_screen_;

  [SerializeField]
  private GameObject game_over_window_, background_, pause_button_, home_button_, audio_settings_button_, audio_settings_window, congrats_message_, regular_message_;

  [SerializeField]
  private HUDManager HUD_;

  private TextMeshProUGUI score_text_, score_text_shadow_;

  public void Start()
  {
    OnGameOver += ShowWindow;
  }

  public void OnDestroy()
  {
    OnGameOver -= ShowWindow;
  }

  public void RestartGame()
  {
    AudioManager.instance_.PlaySFX(0);
    cover_screen_.new_scene_index_ = 1;
    cover_screen_.gameObject.SetActive(true);
  }

  public void BackToMenu()
  {
    AudioManager.instance_.PlaySFX(0);
    cover_screen_.new_scene_index_ = 0;
    cover_screen_.gameObject.SetActive(true);
  }
  
  private void UpdateHighScore()
  {
    PlayerPrefs.SetInt("HighScore", (int)HUD_.score_value_);
  }

  public void GetScore()
  {
    if (HUD_.score_value_ > PlayerPrefs.GetInt("HighScore")){
      UpdateHighScore();
      regular_message_.SetActive(false);
      congrats_message_.SetActive(true);
      score_text_ = congrats_message_.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
      score_text_shadow_ = congrats_message_.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
      score_text_.text = HUD_.score_value_.ToString() + "!";
      score_text_shadow_.text = HUD_.score_value_.ToString() + "!";
    }
    else
    {
      regular_message_.SetActive(true);
      congrats_message_.SetActive(false);
      score_text_ = regular_message_.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
      score_text_.text = HUD_.score_value_.ToString() + ".";
    }
  }

  public void ShowWindow()
  {
    AudioManager.instance_.PlaySFX(1);
    AudioManager.instance_.music_source_.Stop();
    AudioManager.instance_.PlayMusic(1);
    Time.timeScale = 0;
    GetScore();
    game_over_window_.SetActive(true);
    background_.SetActive(true);
    pause_button_.SetActive(false);
    home_button_.SetActive(false);
  }

}
