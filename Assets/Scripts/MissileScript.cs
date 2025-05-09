using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileScript : MonoBehaviour
{

  [SerializeField]
  private float fallSpeed_ = 2.0f;

  [NonSerialized]
  public bool activated_ = false;

  private float spawnPosY = 0.0f;
  private float initialSpawnPosX = 0.0f;
  private float screenWidth = 0.0f;

  private SpriteRenderer sr;

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
    gameObject.SetActive(false);
  }

  // Update is called once per frame
  public void Spawn(int assignedRow, Vector3 pos)
  {
    gameObject.SetActive(true);
    activated_ = true;
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
    if(activated_)
    {
      LetFallMissile();
    }
  }
}
