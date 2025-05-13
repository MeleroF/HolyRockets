using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsMenuScript : MonoBehaviour
{
  public AudioSource music_source, sfx_source_;
  public Scrollbar music_scrollbar, sfx_scrollbar;
  private bool is_window_enabled_ = false;

  public void ModifyMusicVolume()
  {
    music_source.volume = music_scrollbar.value;
  }

  public void ModifySFXVolume()
  {
    sfx_source_.volume = sfx_scrollbar.value;
  }

  public void TriggerWindow()
  {
    gameObject.SetActive(!is_window_enabled_);
    is_window_enabled_ = !is_window_enabled_;
  }
}
