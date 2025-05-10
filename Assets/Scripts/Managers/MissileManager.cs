using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UpcomingRocketManager;

public class MissileManager : MonoBehaviour
{
  [SerializeField]
  private MissileScript missilePrefab_;
  
  public int maxPaths_ = 5;
  public int numMaxMissiles_ = 15;


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

  public void SpawnMissiles(int numMissilesWave, ref List<PipeScript> pipes, int minPaths, int maxPaths, float maxSpeedFactor)
  {
    int nmissileSpawned = 0;
    for(int i = 0; i < missiles.Count && nmissileSpawned < numMissilesWave; ++i)
    {
      if (!missiles[i].controller_.isEnabled_)
      {
        int numPaths = UnityEngine.Random.Range(minPaths, maxPaths);
        float speedFactor = UnityEngine.Random.Range(1.0f, maxSpeedFactor);
        missiles[i].crosshair_.StartSearching(ref pipes, numPaths, speedFactor);
        nmissileSpawned++;
      }
    }
  }

}
