using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenuScript : MonoBehaviour
{
  public CoverScreenScript cover_screen_;

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
}
