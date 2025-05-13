using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance_;

    [SerializeField] public AudioSource musicSource_;
    [SerializeField] AudioSource SFXSource_;

    [SerializeField]
    public AudioClip[] music_;

    [SerializeField]
    public AudioClip[] sfx_;

    private void Start()
    {
        instance_ = this;

        if (music_.Length != 0)
        {
            musicSource_.clip = music_[0];
            musicSource_.Play(); 
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource_.PlayOneShot(clip);
    }
}
