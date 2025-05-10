using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UpcomingRocketManager;

public class MissileManager : MonoBehaviour
{
  [SerializeField]
  private MissileScript missilePrefab_;
  
  public int numMaxMissiles_ = 15;

  [NonSerialized]
  public float timeBetweenSpawns_ = 4.0f;
  [NonSerialized]
  private List<MissileScript> missiles = new List<MissileScript>();

  private void GenerateMissiles()
  {
    for(int i = 0; i < numMaxMissiles_; ++i)
    {
      Quaternion rot = Quaternion.Euler(0f, 0f, 90f);
      MissileScript newMissile = Instantiate(missilePrefab_, new Vector3(), rot);
      missiles.Add(newMissile);
    }
  }
  

  public void Init()
  {
    GenerateMissiles();
  }

  public void SpawnMissiles(int numMissilesWave, ref List<PipeScript> pipes)
  {
    int nmissileSpawned = 0;
    for(int i = 0; i < missiles.Count && nmissileSpawned < numMissilesWave; ++i)
    {
      if (!missiles[i].gameObject.activeSelf)
      {
        missiles[i].crosshair_.StartSearching(ref pipes, 4, 5.0f);
        nmissileSpawned++;
      }
    }
  }

}
