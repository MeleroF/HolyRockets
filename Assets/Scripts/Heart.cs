using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{

  private Image image_;
  private Vector3 originalScale_;
  void Start()
  {
    image_ = GetComponent<Image>();
    originalScale_ = transform.localScale;
  }

   private IEnumerator RemoveHeart(Sprite emptyHeart, float timeBroken)
  {
    yield return new WaitForSeconds(timeBroken);
    image_.sprite = emptyHeart;
    transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));
  }
  public void Break(ref Sprite brokenHeart, ref Sprite emptyHeart, float timeBroken, float degreeRotation)
  {
    image_.sprite = brokenHeart;
    transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, degreeRotation)); 
    StartCoroutine(RemoveHeart(emptyHeart, timeBroken));
  }

  private IEnumerator NormalizeScale(float timeBig)
  {
    yield return new WaitForSeconds(timeBig);
    transform.localScale = originalScale_;
  }

  public void Heal(ref Sprite fullHeart, float plusScale, float timeBig)
  {
    image_.sprite = fullHeart;
    transform.localScale = new Vector3(originalScale_.x + plusScale, originalScale_.y + plusScale, originalScale_.z);
    StartCoroutine(NormalizeScale(timeBig));
  }
}
