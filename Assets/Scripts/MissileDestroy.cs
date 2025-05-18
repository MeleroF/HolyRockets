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
      MissileScript missile = collision.gameObject.GetComponent<MissileScript>();

      if (sr.sortingLayerID != sm_.backSortingLayerID) return;

      if(missile.stats_.rocketSpeciality_ == RocketSpeciality.HEAL)
      {
        OnHealMissileCatch?.Invoke();
        AudioManager.instance_.PlaySFX(4);
      } 
      else
      {
        AudioManager.instance_.PlaySFX(3);
      }

      if(missile.target_.counterTargeted > 0)
        missile.target_.counterTargeted--;

      // Instantiate score obtained prefab.
      GameObject prefab = Resources.Load<GameObject>("ScoreObtained");
      Canvas canvas = FindObjectOfType<Canvas>();
      Vector3 screenPos = Camera.main.WorldToScreenPoint(collision.transform.position);
      screenPos.y += 150.0f;
      GameObject instance = Instantiate(prefab, canvas.transform);
      instance.GetComponent<RectTransform>().position = screenPos;
      instance.GetComponent<ScoreObtainedScript>().SetScoreText(missile.stats_.score_);

      // Update Score
      HUDManager.instance_.UpdateScoreValue(missile.stats_.score_);
      switch(missile.stats_.rocketType_)
      {
        case RocketType.FALLMARKER:
          ParentController controller = collision.gameObject.GetComponent<ParentController>();
          controller.ChangeParentState(false);
          break;
        case RocketType.REMOTE:
          Shadow shadow = collision.transform.parent.GetComponent<Shadow>();
          shadow.gameObject.SetActive(false);
          break;
      }
      OnMissileCatch?.Invoke();

    }
  }
 
}
