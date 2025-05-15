using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class PipeManager : MonoBehaviour
{
  public int openedPipes_ = 0;

  [SerializeField]
  private PipeScript pipePrefab_ = null;
  [SerializeField]
  private TextMeshProUGUI availablePipesText_ = null;
  [SerializeField]
  public int numRows_ = 4 ;
  [SerializeField]
  public int numCols_ = 10;
  [SerializeField]
  private float gapX_ = 0.10f;
  [SerializeField]
  private float gapY_ = 0.10f;

  private GameObject ship_;

  [NonSerialized]
  public List<PipeScript> pipes_ = new List<PipeScript>();

  char[] keyMapping = {
    '1','2','3','4','5','6','7','8','9','0',
      'q','w','e','r','t','y','u','i','o','p',
      'a','s','d','f','g','h','j','k','l','ñ',
        'z','x','c','v','b','n','m',',','.','-'
  };

  private Dictionary<char, int> charDictionary = new Dictionary<char, int>();

  private void GeneratePipes(ref Transform spawnPointPipes)
  {
    int nTotalPipes = numCols_ * numRows_;

    Renderer renderer = pipePrefab_.transform.GetChild(0).GetComponent<Renderer>();
    float pipeSize = renderer.bounds.size.x;

    Vector3 starterPos = spawnPointPipes ? spawnPointPipes.position : Vector3.zero;

    float offsetRow = 0.0f;

    for (int y = 0; y < numRows_; ++y)
    {
      SpriteRenderer customRendererRow = pipePrefab_.GetComponent<SpriteRenderer>();

      for (int x = 0; x < numCols_; ++x)
      {
        Vector3 position = new Vector3(0.0f, 0.0f, 0.0f);
        PipeScript tmpPipe;

        tmpPipe = Instantiate(pipePrefab_, position, new Quaternion(), ship_ ?  ship_.transform : null );
        tmpPipe.transform.position = (new Vector3(starterPos.x + offsetRow, starterPos.y)) + new Vector3((pipeSize + gapX_) * x, -(pipeSize + gapY_) * y);

        SpriteRenderer srTmpPipe = tmpPipe.GetComponentInChildren<SpriteRenderer>();
        SpriteMask spriteMaskPipe = tmpPipe.transform.GetChild(2).gameObject.GetComponent<SpriteMask>();

        srTmpPipe.sortingLayerID = SortingLayer.NameToID($"RocketRow{y + 1}");
        spriteMaskPipe.frontSortingLayerID = SortingLayer.NameToID($"RocketRow{y + 2}");
        spriteMaskPipe.backSortingLayerID = SortingLayer.NameToID($"RocketRow{y + 1}");


        spriteMaskPipe.sortingOrder = 1 + y;
        srTmpPipe.sortingOrder = 1 + y;

        pipes_.Add(tmpPipe);
      }
      if (y % 2 == 0)
      {
        offsetRow += 0.7f;
      }
    }
    
  }

  private void SearchForShip()
  {
    ship_ = GameObject.Find("Ship");
  }

  private void InitKeyMapping()
  {
    int counter = 0;
    foreach(char c in keyMapping)
    {
      charDictionary.Add(c, counter++);
    }
  }
  public void Init(ref Transform spawnPointPipes)
  {
    InitKeyMapping();
    SearchForShip();
    GeneratePipes(ref spawnPointPipes);
  }

  public void CloseAllPipes(int missilesPerWave)
  {
    for (int i = 0; i < pipes_.Count; i++)
    {
      if (pipes_[i].isLidOpen_)
      {
        pipes_[i].ClosePipe(ref openedPipes_);
        UpdatePipesOpenedHUD(missilesPerWave);
      }
    }
  }

  public void UpdatePipesOpenedHUD(int missilesPerWave)
  {
    availablePipesText_.text = "x" + (missilesPerWave - openedPipes_).ToString();
    if (openedPipes_ == missilesPerWave)
    {
      availablePipesText_.color = Color.red;
    }
    else
    {
      availablePipesText_.color = new Color(0.3f, 0.3f, 0.3f, 1.0f);
    }
  }

  public void DetectInput(int missilesPerWave)
  {

    foreach(char c in Input.inputString)
    {
      char character = char.ToLower(c);

      if (charDictionary.ContainsKey(character))
      {
        if (pipes_[charDictionary[character]].OpenPipe(ref openedPipes_, missilesPerWave))
        {
          UpdatePipesOpenedHUD(missilesPerWave);
        }
      } 
    }
  }
}
