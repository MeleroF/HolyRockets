using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  [SerializeField]
  private PipeManager pipeManager_;
  [SerializeField]
  private Transform pipeSpawnPoint_;

  // Start is called before the first frame update
  void Start()
  {
    pipeManager_.Init(ref pipeSpawnPoint_);
  }

  // Update is called once per frame
  void Update()
  {
    pipeManager_.DetectInput();
  }
}
