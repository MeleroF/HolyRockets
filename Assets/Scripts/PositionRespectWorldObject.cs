using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionRespectWorldObject : MonoBehaviour
{
  [SerializeField]
  private Transform worldTr_;

  private Camera maincamera_ = null;
  // Start is called before the first frame update
  void Start()
  {
    maincamera_ = Camera.main;
  }

  // Update is called once per frame
  void Update()
  {
    transform.position = maincamera_.WorldToScreenPoint(worldTr_.position);
  }
}
