using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class SliderAnimator : MonoBehaviour, IPointerClickHandler
{
   [SerializeField]
   private float amountMovement = 100f;

   [SerializeField]
   private float timeToCompleteMovement = 1.5f;

   private float actualTimeToCompleteMovement;

   private Vector3 initTransformPosition;

   private bool unfolded = true;

   [SerializeField]
   private Animator refAnimator;

   private static readonly int Opened = Animator.StringToHash("Opened");

   private void Awake()
   {
      initTransformPosition = (transform as RectTransform).anchoredPosition;
   }

   private void OnEnable()
   {
      StartCoroutine(CouMoveSlider(3f));
   }

   public void OnPointerClick(PointerEventData eventData)
   {
      StopAllCoroutines();
      StartCoroutine(CouMoveSlider(0));
   }

   IEnumerator CouMoveSlider(float argTimeWaitUnFoll)
   {
      yield return new WaitForSeconds(argTimeWaitUnFoll);
      var tmpTargetPosition = unfolded? initTransformPosition + Vector3.up * amountMovement : initTransformPosition - Vector3.up * amountMovement;
      refAnimator.SetBool(Opened, !unfolded);

      while(actualTimeToCompleteMovement < timeToCompleteMovement)
      {
         (transform as RectTransform).anchoredPosition = Vector3.Lerp(initTransformPosition, tmpTargetPosition, Mathf.Pow(actualTimeToCompleteMovement / timeToCompleteMovement,0.35f));
         actualTimeToCompleteMovement += Time.deltaTime;
         yield return null;
      }

      (transform as RectTransform).anchoredPosition = tmpTargetPosition;
      initTransformPosition = tmpTargetPosition;
      unfolded = !unfolded;
      actualTimeToCompleteMovement = 0;
         
      if(unfolded)
         StartCoroutine(CouMoveSlider(5));
   }
}