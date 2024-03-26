using System.Collections;
using UnityEngine;

public class PanelDesplegableSuperior : MonoBehaviour
{
   [SerializeField]
   private RectTransform rectTransformBorderTop;

   [SerializeField]
   private float timeAnimationPanel;

   private float actualTimeAnimationPanel;

   [SerializeField]
   private AnimationCurve acPanelInOut;

   private bool opened = true;

   public void ButtonOpenClosePanel()
   {
      opened = !opened;
      StopAllCoroutines();
      StartCoroutine(CouPanelAnimation());
   }

   private IEnumerator CouPanelAnimation()
   {
      var tmpPositionPanelTop = rectTransformBorderTop.anchoredPosition;

      if(opened)
      {
         while(actualTimeAnimationPanel < timeAnimationPanel)
         {
            actualTimeAnimationPanel += Time.deltaTime;
            tmpPositionPanelTop[1] = acPanelInOut.Evaluate(actualTimeAnimationPanel / timeAnimationPanel) * 130;
            rectTransformBorderTop.anchoredPosition = tmpPositionPanelTop;
            yield return null;
         }
      }
      else
      {
         while(actualTimeAnimationPanel > 0)
         {
            actualTimeAnimationPanel -= Time.deltaTime;
            tmpPositionPanelTop[1] = acPanelInOut.Evaluate(actualTimeAnimationPanel / timeAnimationPanel) * 130;
            rectTransformBorderTop.anchoredPosition = tmpPositionPanelTop;
            yield return null;
         }
      }
   }
}