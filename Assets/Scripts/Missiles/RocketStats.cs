using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RocketStats", menuName = "scriptableObjects/RocketStats", order = 1)]
public class RocketStats : ScriptableObject
{
  public MissileScript prefab_;

  [Header("Stats")]
  public RocketSpeciality rocketSpeciality_ = RocketSpeciality.NONE;
  public RocketType rocketType_ = RocketType.FALLMARKER;
  public float fallSpeed_ = -10.0f;
  public float percentageSpawn_ = 50.0f;
  public int levelStart = 0;
  public Shadow shadow_ = null;

  [Header("Visuals")]
  public Sprite sprite_;
  public List<Sprite> crossHairSprites_;

  [Header("Audio")]
  public AudioClip collisionAudio_;
  public AudioClip cathedAudio_;
}

public enum RocketType
{
  FALLMARKER,
  REMOTE
}

public enum RocketSpeciality
{
  NONE,
  HEAL
}