using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using static UpcomingRocketManager;

public class MissileManager : MonoBehaviour
{
  [SerializeField]
  private List<RocketStats> missileStatPrefabs_;
  [SerializeField]
  public int maxPaths_ = 5;
  [SerializeField]
  public int maxMissiles_ = 15;

  [NonSerialized]
  public int realMaxMissiles_ = 0;

  private class MissileTypeClassifier
  {
    public int index_ = 0;
    public int lastSize_ = 0;
    public int numMissiles_ = 0;
    public int levelStart_ = 0;
    public List<MissileScript> missiles_ =  new List<MissileScript>();
  }

  [NonSerialized]
  private List<MissileTypeClassifier> missileTypes_ = new List<MissileTypeClassifier>();

  private void GetNumPrefabs(ref List<int> numMissilesPerPrefab_)
  {
    float percentageFactor = 1.0f / 100.0f;
    foreach(RocketStats prefab in missileStatPrefabs_)
    {
      int num = Mathf.CeilToInt(prefab.percentageSpawn_ * maxMissiles_ * percentageFactor);
      numMissilesPerPrefab_.Add(num);
      realMaxMissiles_ += num;
    }
  }

  private void GenerateMissiles()
  {
    List<int> numMissilesPerPrefab_ = new List<int>();
    GetNumPrefabs(ref numMissilesPerPrefab_);

    int prefabCount = 0;
    foreach(int numPrefab in numMissilesPerPrefab_)
    {
      MissileTypeClassifier classifier = new MissileTypeClassifier(); 
      for (int i = 0; i < numPrefab; ++i)
      {
        MissileScript newMissile = null;
        Quaternion rot = Quaternion.Euler(0f, 0f, 90f);
        if (missileStatPrefabs_[prefabCount].shadow_) 
        {
          Shadow shadow = Instantiate(missileStatPrefabs_[prefabCount].shadow_, new Vector3(), Quaternion.Euler(0f, 0f, 0.0f));
          newMissile = shadow.remote_;
    
        }
        else
        {
          newMissile = Instantiate(missileStatPrefabs_[prefabCount].prefab_, new Vector3(), rot);
        }
        
        newMissile.Init(ref missileStatPrefabs_[prefabCount].sprite_);
        if(i == 0)
        {
          classifier.levelStart_ = missileStatPrefabs_[prefabCount].levelStart;
        }
        if(newMissile is FallMarker newFallMarker)
        {
          newFallMarker.InitCrossHair(ref missileStatPrefabs_[prefabCount].crossHairSprites_);
        }
        classifier.missiles_.Add(newMissile);
      }
      
      classifier.lastSize_ = prefabCount == 0 ? prefabCount : numMissilesPerPrefab_[prefabCount - 1];
      classifier.index_ = prefabCount;
      classifier.numMissiles_ = numMissilesPerPrefab_[prefabCount];
      missileTypes_.Add(classifier);
      prefabCount++;
    }
  }
  

  public void Init()
  {
    GenerateMissiles();
  }

  private int GetRocketTypeWithBiggestProbabilty(ref int[] typeCounters)
  {
    float biggestProb = 0;
    int typeWithBiggestProb = 0;
    int type;
    for (type = 0; type < missileStatPrefabs_.Count; ++type)
    {
      if (missileStatPrefabs_[type].percentageSpawn_ > biggestProb)
      {
        biggestProb = missileStatPrefabs_[type].percentageSpawn_;
        typeWithBiggestProb = type;
      }
    }

    return typeWithBiggestProb;
  }

  private int GetRocketTypeByPercentage(ref int[] typeCounters, int currentLevel)
  {
    int selected = GetRocketTypeWithBiggestProbabilty(ref typeCounters);
    for(int i = 0; i < missileStatPrefabs_.Count; ++i)
    {
      if (typeCounters[i] >= missileTypes_[i].numMissiles_) continue;
      if (missileTypes_[i].levelStart_ > currentLevel) continue;
      bool isSelect = (UnityEngine.Random.value * 100) < missileStatPrefabs_[i].percentageSpawn_;
      if (isSelect)
      {
        return i;
      }
    }

    return selected;
  }

  private void SpawnByType(int type, 
                          ref int numMisssilesRemote,
                          ref HashSet<int> isMissileDisponible,
                          ref List<PipeScript> pipes,
                          ref int nmissileSpawned,
                          int nmaxMissilesWave,
                          int minPaths,
                          int maxPaths,
                          float maxSpeedFactor,
                          int numRows, int numCols)
  {
    MissileTypeClassifier missileCalifier = missileTypes_[type];
    for (int i = 0; i < missileCalifier.numMissiles_; ++i)
    {
      if (nmissileSpawned >= nmaxMissilesWave) break;
      if (isMissileDisponible.Contains(missileCalifier.lastSize_ + i)) continue;

      switch(missileStatPrefabs_[type].rocketType_)
      {
        case RocketType.FALLMARKER:
          FallMarker fallMarker = (FallMarker)missileCalifier.missiles_[i];
          if(!fallMarker.controller_.isEnabled_)
          {
            int numPaths = UnityEngine.Random.Range(minPaths, maxPaths);
            float speedFactor = UnityEngine.Random.Range(1.0f, maxSpeedFactor);
            fallMarker.crosshair_.StartSearching(ref pipes, numPaths, speedFactor);
            nmissileSpawned++;
          }
          break;
        case RocketType.REMOTE:
          Remote remote = (Remote)missileCalifier.missiles_[i];
          if(!remote.gameObject.active)
          {
            int randRow = UnityEngine.Random.Range(0, numRows);
            int numPaths = UnityEngine.Random.Range(minPaths, maxPaths);
            float speedFactor = UnityEngine.Random.Range(1.0f, maxSpeedFactor);
            remote.shadow_.StartStrolling(ref pipes, numPaths, speedFactor, randRow, numCols);
            nmissileSpawned++;
            numMisssilesRemote++;
          }
          break;
        default:
          break;
      }

      isMissileDisponible.Add(missileCalifier.lastSize_ + i);
      break;
    }
  }

  public void SpawnMissiles(ref int numMisssilesRemote, int numMissilesWave, ref List<PipeScript> pipes, 
                            int minPaths,float maxSpeedFactor, int currentLevel,
                            int numRows, int numCols)
  {
    int nmissileSpawned = 0;

    int[] typeCounter = new int[missileTypes_.Count];
    HashSet<int> isMissileDisponible = new HashSet<int>();

    while (nmissileSpawned < numMissilesWave && isMissileDisponible.Count < realMaxMissiles_)
    {
      int randType = GetRocketTypeByPercentage(ref typeCounter, currentLevel);
      SpawnByType(randType, 
        ref numMisssilesRemote,
        ref isMissileDisponible,
        ref pipes, 
        ref nmissileSpawned, 
        numMissilesWave, 
        minPaths, maxPaths_,
        maxSpeedFactor,
        numRows,numCols); 
    }
  }
}
