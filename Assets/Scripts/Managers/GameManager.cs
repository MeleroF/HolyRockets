using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UpcomingRocketManager;

public class GameManager : MonoBehaviour
{
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

  // Start is called before the first frame update
  void Start()
  {
    pipeManager_?.Init(ref pipeSpawnPoint_);
    missileManager_?.Init();
    upcomingRocketManager_?.Init(ref upcomingRocketSpawnPoint_);

    OnAllRocketsDestroid += CanSpawnMissiles;

  }

  private IEnumerator SetTimeForMissile()
  {
    yield return new WaitForSeconds(missileManager_.timeBetweenSpawns_);
    missileManager_.SpawnMissiles(ref pipeManager_.pipes_, ref pipeManager_.numRows_, ref pipeManager_.numCols_);
    StartCoroutine(SetTimeForMissile());
  }

  private void CanSpawnMissiles()
  {
    StartCoroutine(SetTimeForMissile());
  }

  // Update is called once per frame
  void Update()
  {
    pipeManager_?.DetectInput();
  }
}
