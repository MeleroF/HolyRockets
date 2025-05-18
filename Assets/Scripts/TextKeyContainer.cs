using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextKeyContainer : MonoBehaviour
{
  [SerializeField]
  private float blinkFinishTime_ = 5.5f;

  [SerializeField]
  private float blinkStartTime_ = 2.5f;

  [SerializeField]
  private float blinkLapse_ = 0.5f;

  [NonSerialized]
  public TextMeshProUGUI fillText_ = null;

  [NonSerialized]
  public TextMeshProUGUI outlineText_ = null;

  // Start is called before the first frame update
  private void Awake()
  {
    StartCoroutine(StartBlinking());
    StartCoroutine(StopBlinking());
  }

  private void Blink()
  {
    fillText_.enabled = !fillText_.enabled;
    outlineText_.enabled = !outlineText_.enabled;
  }

  private IEnumerator StopBlinking()
  {
    yield return new WaitForSeconds(blinkFinishTime_);
    gameObject.SetActive(false);
  }

  private IEnumerator BlinkCoroutine()
  {
    yield return new WaitForSeconds(blinkLapse_);
    Blink();
    StartCoroutine(BlinkCoroutine()); 
  }

  private IEnumerator StartBlinking()
  {
    yield return new WaitForSeconds(blinkStartTime_);
    Blink();
    StartCoroutine(BlinkCoroutine());
  }

  // Update is called once per frame
  void Update()
  {
        
  }
}
