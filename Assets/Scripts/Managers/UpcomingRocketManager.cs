using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UpcomingRocketManager;
using static UpcomingRocketScript;

public class UpcomingRocketManager : MonoBehaviour
{
  public delegate void AllRocketsDestroid(int numRockets);
  public static event AllRocketsDestroid OnAllRocketsDestroid;

  [SerializeField]
  private UpcomingRocketScript upcomingRocketPrefab_;
  [SerializeField]
  private float spawnGap_ = 0.5f;

  private int numMaxRockets_ = 15;
  private int rocketsDestroid = 0;
  private int numRocketsToSpawn_ = 0;

  [NonSerialized]
  private List<UpcomingRocketScript> rocketList_ = new List<UpcomingRocketScript>();

  // Start is called before the first frame update

  private void GenerateUpcomingRockets(ref Transform spawnTr)
  {
    for(int x = 0; x < numMaxRockets_; ++x)
    {
      Vector3 spawnPos = spawnTr.position + new Vector3(x * spawnGap_, 0.0f);
      UpcomingRocketScript newRocket = Instantiate(upcomingRocketPrefab_, spawnPos, new Quaternion());
      newRocket.Init(spawnPos);
      rocketList_.Add(newRocket); 
    }
  }

  private void DetectRocketsDestroid()
  {
    rocketsDestroid++;
    if (rocketsDestroid >= numRocketsToSpawn_)
    {
      OnAllRocketsDestroid?.Invoke(numRocketsToSpawn_);
      rocketsDestroid = 0;
    }
  }

  public void SummonRockets(int numRockets)
  {
    int nrocketSpawned = 0;
    int randRocket = 0;
    int whileLoopLimitCounter = 0;
    int maxWhileLoops = numMaxRockets_ * 3;
    numRocketsToSpawn_ = numRockets;
    while (nrocketSpawned < numRockets && whileLoopLimitCounter < maxWhileLoops) { 
      randRocket = UnityEngine.Random.Range(0, numMaxRockets_);
      if (!rocketList_[randRocket].gameObject.activeSelf)
      {
        rocketList_[randRocket].Spawn();
        nrocketSpawned++;
      }
      whileLoopLimitCounter++;
    }
  }
  public void Init(ref Transform spawnTr, int numMaxRockets)
  {
    numMaxRockets_ = numMaxRockets;
    OnRocketDestroid += DetectRocketsDestroid;
    GenerateUpcomingRockets(ref spawnTr);
  }

  private void OnDestroy()
  {
    OnRocketDestroid -= DetectRocketsDestroid;
  }
}
