using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileDestroy : MonoBehaviour
{
  public delegate void MissileCatched();
  public static event MissileCatched OnMissileCatch;

  public delegate void HealMissileCatched();
  public static event HealMissileCatched OnHealMissileCatch;

  private int rocketLayer_ = 0;
  private SpriteMask sm_;

  // Start is called before the first frame update
  void Start()
  {
    rocketLayer_ = LayerMask.NameToLayer("Rocket");
    sm_ = GetComponent<SpriteMask>();
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.gameObject.layer == rocketLayer_)
    {
      SpriteRenderer sr = collision.gameObject.GetComponent<SpriteRenderer>();
      ParentController controller = collision.gameObject.GetComponent<ParentController>();
      MissileScript missile = collision.gameObject.GetComponent<MissileScript>();

      if (sr.sortingLayerID != sm_.backSortingLayerID) return;

      OnMissileCatch?.Invoke();

      if(missile.stats_.rocketSpeciality_ == RocketSpeciality.HEAL)
      {
        OnHealMissileCatch?.Invoke();
      }

      // Instantiate score obtained prefab.
      GameObject prefab = Resources.Load<GameObject>("ScoreObtained");
      Canvas canvas = FindObjectOfType<Canvas>();
      Vector3 screenPos = Camera.main.WorldToScreenPoint(collision.transform.position);
      screenPos.y += 150.0f;
      GameObject instance = Instantiate(prefab, canvas.transform);
      instance.GetComponent<RectTransform>().position = screenPos;
      instance.GetComponent<ScoreObtainedScript>().SetScoreText(100);

      // Update Score
      HUDManager.instance_.UpdateScoreValue(100);

      controller.ChangeParentState(false);
    }
  }
 
}
