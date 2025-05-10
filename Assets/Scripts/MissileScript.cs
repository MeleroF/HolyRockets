using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileScript : MonoBehaviour
{

  [SerializeField]
  private float fallSpeed_ = 2.0f;

  private float spawnPosY = 0.0f;
  private float initialSpawnPosX = 0.0f;
  private float screenWidth = 0.0f;

  private SpriteRenderer sr;
  [NonSerialized]
  public CrosshairScript crosshair_;
  [NonSerialized]
  public ParentController controller_;

  private void GetSpawnPointY()
  {
    Camera mainCamera = Camera.main;
    float halfHeight = mainCamera.orthographicSize;
    spawnPosY = mainCamera.transform.position.y + halfHeight;
    float aspect = (float)Screen.width / Screen.height;
    float halfWidth = halfHeight * aspect;
    initialSpawnPosX = mainCamera.transform.position.x - halfWidth;
    screenWidth = halfWidth * 2f;
  }

  // Start is called before the first frame update
  void Start()
  {
    GetSpawnPointY();
    sr = GetComponent<SpriteRenderer>();
    controller_ = GetComponent<ParentController>();
    controller_.Start();
    controller_.ChangeParentState(false);
    crosshair_ = GetComponentInChildren<CrosshairScript>();
    crosshair_.Init();

  }

  // Update is called once per frame
  public void Spawn(int assignedRow, Vector3 pos)
  {
    controller_.ChangeParentState(true);
    sr.sortingLayerName = $"RocketRow{assignedRow}";
    sr.sortingOrder = assignedRow + 1; 
    transform.position = new Vector3(pos.x, spawnPosY);
  }

  private void LetFallMissile()
  {
    transform.position = new Vector3(transform.position.x, transform.position.y + Time.deltaTime * fallSpeed_, transform.position.z);
  }

  private void Update()
  {
    LetFallMissile();
  }

 
}
