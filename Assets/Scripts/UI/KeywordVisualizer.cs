using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class KeywordVisualizer : MonoBehaviour
{
  private Canvas canvas_;

  private class TextContainerAndTr
  {
    public TextKeyContainer container_;
    public Transform pipeTr_;

    public TextContainerAndTr()
    {
      container_ = null;
      pipeTr_ = null;
    }

    public TextContainerAndTr(TextKeyContainer container, Transform pipeTr)
    {
      container_ = container;
      pipeTr_ = pipeTr;
    }
  }

  private Camera maincamera_;

  private List<TextContainerAndTr> keyTextContainer_ = new List<TextContainerAndTr>();

  private void UpdateKeyText(ref TextKeyContainer textContainer, char key)
  {
    for(int i = 0; i < textContainer.transform.childCount; ++i)
    {
      GameObject child = textContainer.transform.GetChild(i).gameObject;
      
      TextMeshProUGUI textMeshPro = child.GetComponent<TextMeshProUGUI>();

      if (i == 0)
        textContainer.fillText_ = textMeshPro;
      else
        textContainer.outlineText_ = textMeshPro;

      textMeshPro.text = key.ToString();
    }
  }

  public void Init(ref List<PipeScript> pipes, ref Canvas canvas, ref TextKeyContainer keyTextcontainer)
  {
    canvas_ = canvas;
    maincamera_ = Camera.main;

    foreach(PipeScript pipe in pipes)
    {
      Vector2 screenPipeLoc = maincamera_.WorldToScreenPoint(pipe.transform.position);
      TextKeyContainer newText = Instantiate(keyTextcontainer, canvas.transform);
      newText.GetComponent<RectTransform>().position = screenPipeLoc;
      UpdateKeyText(ref newText, char.ToUpper(pipe.key_));
      keyTextContainer_.Add(new TextContainerAndTr(newText, pipe.transform));
    }

  }

  private void Update()
  {
    foreach(TextContainerAndTr textContainer in keyTextContainer_)
      textContainer.container_.transform.position = maincamera_.WorldToScreenPoint(textContainer.pipeTr_.position);
    
  }
}
