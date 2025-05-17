using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class FallMarker : MissileScript
{
  [NonSerialized]
  public CrosshairScript crosshair_;
  [NonSerialized]
  public ParentController controller_;

  private void GetSpawnPointY()
  {
    Camera mainCamera = Camera.main;
    float halfHeight = mainCamera.orthographicSize;
    spawnPosY = mainCamera.transform.position.y + halfHeight;
    spawnPosY += sr_.bounds.size.y;
  }

  protected override void Awake()
  {
    sr_ = GetComponent<SpriteRenderer>();
    GetSpawnPointY();
    controller_ = GetComponent<ParentController>();
    controller_.Start();
    controller_.ChangeParentState(false);
    crosshair_ = GetComponentInChildren<CrosshairScript>();
    crosshair_.Init();

  }

  public void InitCrossHair(ref List<Sprite> crosshairSprites)
  {
    if (crosshairSprites == null) return;
    crosshair_.crosshairSearching_ = crosshairSprites[0];
    crosshair_.crosshairPointing_ = crosshairSprites[1];
    crosshair_.crosshairBlink_ = crosshairSprites[2];
  } 

  public override void Spawn(int assignedRow, Vector3 pos)
  {
    controller_.ChangeParentState(true);
    mustFall_ = true;
    sr_.sortingLayerName = $"RocketRow{assignedRow}";
    sr_.sortingOrder = assignedRow + 1; 
    transform.position = new Vector3(pos.x, spawnPosY);
}
}
