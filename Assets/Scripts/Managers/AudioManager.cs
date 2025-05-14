using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
  public static AudioManager instance_;

  static public float music_volume_ = 1.0f;
  static public float sfx_volume_ = 1.0f;
  public AudioSource music_source, sfx_source_;
  public GameObject audio_settings_window_;
  public Scrollbar music_scrollbar, sfx_scrollbar;
  private bool is_window_enabled_ = false;

  [SerializeField]
  public AudioClip[] music_;

  [SerializeField]
  public AudioClip[] sfx_;

  private void Start()
  {
    instance_ = this;

    music_source.volume = music_volume_;
    music_scrollbar.value = music_volume_;

    sfx_source_.volume = sfx_volume_;
    sfx_scrollbar.value = sfx_volume_;

    if (music_.Length != 0)
    {
      music_source.clip = music_[0];
      music_source.Play(); 
    }
  }

  public void PlaySFX(AudioClip clip)
  {
    sfx_source_.PlayOneShot(clip);
  }

  public void ModifyMusicVolume()
  {
    music_source.volume = music_scrollbar.value;
    music_volume_ = music_scrollbar.value;
  }

  public void ModifySFXVolume()
  {
    sfx_source_.volume = sfx_scrollbar.value;
    sfx_volume_ = sfx_scrollbar.value;
  }

  public void TriggerAudioSettingsWindow()
  {
    audio_settings_window_.SetActive(!is_window_enabled_);
    is_window_enabled_ = !is_window_enabled_;
  }
}
