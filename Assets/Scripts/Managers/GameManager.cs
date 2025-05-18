using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UpcomingRocketManager;
using static MissileDestroy;
using static PipeScript;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
  [SerializeField]
  private TextKeyContainer keyTextContainer_;
  [SerializeField]
  private Canvas canvas_;
  [SerializeField]
  private LifeManager lifeManager_;
  [SerializeField]
  private UpcomingRocketManager upcomingRocketManager_;
  [SerializeField]
  private MissileManager missileManager_;
  [SerializeField]
  private PipeManager pipeManager_;
  [SerializeField]
  private Transform pipeSpawnPoint_;
  [SerializeField]
  private Transform upcomingRocketSpawnPoint_;

  private int maxMissilesWave_ = 0;
  private int missilesWave_ = 0;
  private int missileCounter_ = 0;

  private int numMissilesRemoteWave = 0;
  private int numWaves_ = 0;
  private int waveCounter_ = 0;
  [SerializeField]
  private int currentLevel_ = 0;
  private int minPathsCrosshair = 0;

  private KeywordVisualizer keyVisualizer_ = null;

  private float maxSpeedFactor_ = 1.0f;

  // Start is called before the first frame update
  void Start()
  {
    pipeManager_?.Init(ref pipeSpawnPoint_);
    missileManager_?.Init();
    upcomingRocketManager_?.Init(ref upcomingRocketSpawnPoint_, missileManager_.realMaxMissiles_);
    lifeManager_?.Init(ref canvas_);
    keyVisualizer_ = canvas_.gameObject.GetComponent<KeywordVisualizer>();
    keyVisualizer_.Init(ref pipeManager_.pipes_, ref canvas_, ref keyTextContainer_);

    UpdateSettingsForLevel();
    SummonRocketsInLevel();

    OnAllRocketsDestroid += CanSpawnMissiles;
    OnRocketCollision += CountMissilesCatched;
    OnMissileCatch += CountMissilesCatched;
  }

  private void SummonRocketsInLevel()
  {
    missilesWave_ = UnityEngine.Random.Range(1, maxMissilesWave_);
    pipeManager_.UpdatePipesOpenedHUD(missilesWave_);
    upcomingRocketManager_.SummonRockets(missilesWave_);
  }

  private void UpdateSettingsForLevel()
  {
    currentLevel_++;
    float difficulty = Mathf.Log(currentLevel_, 2) + 1.0f;
    maxMissilesWave_ = Mathf.CeilToInt(difficulty * 1.5f);
    numWaves_ = Mathf.CeilToInt(difficulty);
    minPathsCrosshair = Mathf.Max(0, missileManager_.maxPaths_ - Mathf.FloorToInt(difficulty));
    maxSpeedFactor_ = Mathf.Min(10, difficulty);
  }

  private void CountMissilesCatched()
  {
    missileCounter_++;
    if (missileCounter_ >= missilesWave_ - numMissilesRemoteWave)
    {
      SummonRocketsInLevel();
      missileCounter_ = 0;
      numMissilesRemoteWave = 0;
      waveCounter_++;
      pipeManager_.CloseAllPipes(missilesWave_);
      if (waveCounter_ >= numWaves_)
      {
        UpdateSettingsForLevel();
        waveCounter_ = 0;
      }
    }
  }

  private void CanSpawnMissiles(int numMissiles)
  {
    missileManager_.SpawnMissiles(
                              ref numMissilesRemoteWave,
                              numMissiles,
                              ref pipeManager_.pipes_, 
                              minPathsCrosshair, 
                              maxSpeedFactor_, currentLevel_,
                              pipeManager_.numRows_,
                              pipeManager_.numCols_);
  }

  // Update is called once per frame
  void Update()
  {
    pipeManager_?.DetectInput(missilesWave_);
  }

  private void OnDestroy()
  {
    OnAllRocketsDestroid -= CanSpawnMissiles;
    OnRocketCollision -= CountMissilesCatched;
    OnMissileCatch -= CountMissilesCatched;
  }
}
