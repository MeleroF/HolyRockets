using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UpcomingRocketManager;
using static UpcomingRocketScript;

public class UpcomingRocketManager : MonoBehaviour
{
  public delegate void AllRocketsDestroid();
  public static event AllRocketsDestroid OnAllRocketsDestroid;

  [SerializeField]
  private UpcomingRocketScript upcomingRocketPrefab_;
  [SerializeField]
  private int numUpcomingRockets_ = 15;
  [SerializeField]
  private float spawnGap_ = 0.5f;

  private int rocketsDestroid = 0;
  private List<UpcomingRocketScript> rocketList_ = new List<UpcomingRocketScript>();

  // Start is called before the first frame update

  private void GenerateUpcomingRockets(ref Transform spawnTr)
  {
    for(int x = 0; x < numUpcomingRockets_; ++x)
    {
      Vector3 spawnPos = spawnTr.position + new Vector3(x * spawnGap_, 0.0f);
      UpcomingRocketScript newRocket = Instantiate(upcomingRocketPrefab_, spawnPos, new Quaternion());
      newRocket.Init(spawnPos);
      rocketList_.Add(newRocket); 
    }
  }

  private void DetectRocketsDestroid()
  {
    Debug.Log("Hola");
    if (rocketsDestroid >= numUpcomingRockets_)
    {
      Debug.Log("Buenas");
      OnAllRocketsDestroid?.Invoke();
      rocketsDestroid = 0;
    }
    rocketsDestroid++;
  }

  public void Init(ref Transform spawnTr)
  {
    OnRocketDestroid += DetectRocketsDestroid;
    GenerateUpcomingRockets(ref spawnTr);
  }
}
