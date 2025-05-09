using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UpcomingRocketManager;

public class MissileManager : MonoBehaviour
{
  [SerializeField]
  private MissileScript missilePrefab_;
  [SerializeField]
  private int numMaxMissiles_ = 10;

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

  public void SpawnMissiles(ref List<PipeScript> pipes, ref int numRows, ref int numCols)
  {
    bool missileAlreaySpawned = false;
    for(int i = 0; i < missiles.Count && !missileAlreaySpawned; ++i)
    {
      int randRow = UnityEngine.Random.Range(0, numRows);
      int randCol = UnityEngine.Random.Range(0, numCols);

      if (!missiles[i].activated_)
      {
        missiles[i].Spawn(randRow + 1, pipes[randRow * numCols + randCol].transform.position); 
        missileAlreaySpawned = true;
      }
    }
  }

}
