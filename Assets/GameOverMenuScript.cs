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
  private GameObject game_over_window_, background_, pause_button_, home_button_, audio_settings_button_, audio_settings_window;

  [SerializeField]
  private HUDManager HUD_;

  [SerializeField]
  private TextMeshProUGUI score_text_;

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
    cover_screen_.new_scene_index_ = 1;
    cover_screen_.gameObject.SetActive(true);
  }

  public void BackToMenu()
  {
    cover_screen_.new_scene_index_ = 0;
    cover_screen_.gameObject.SetActive(true);
  }

  public void GetScore()
  {
    score_text_.text = HUD_.score_value_.ToString() + ".";
  }

  public void ShowWindow()
  {
    Time.timeScale = 0;
    GetScore();
    game_over_window_.SetActive(true);
    background_.SetActive(true);
    pause_button_.SetActive(false);
    home_button_.SetActive(false);
    audio_settings_button_.SetActive(false);
    audio_settings_window.SetActive(false);
  }

}
