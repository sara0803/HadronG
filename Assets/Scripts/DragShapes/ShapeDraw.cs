using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;
using UnityEngine.Events;

namespace DragShapes
{
   /*
    * Se deben poner los colliders como hijos del objeto que tiene esta clase como componente en sentido horario haciendo la figura
    * 
    */
   public class ShapeDraw : MonoBehaviour
   {
      [SerializeField]
      private LayerMask layerMaskShape;

      public UnityEvent OnFinishDrawShape;
      public UnityEvent OnFailDrawShape;

      private Collider2D nextColliderTouched;

      private readonly Dictionary<Collider2D, WrapperCollider> dictionaryCollider2D_WrapperCollider = new Dictionary<Collider2D, WrapperCollider>();

      private void OnEnable()
      {
         LeanTouch.OnFingerUpdate += OnFingerUpdate;
         LeanTouch.OnFingerUp += OnFingerUp;
         LeanTouch.OnFingerDown += OnFingerDown;
      }

      private void OnDisable()
      {
         LeanTouch.OnFingerUpdate -= OnFingerUpdate;
         LeanTouch.OnFingerUp -= OnFingerUp;
         LeanTouch.OnFingerDown -= OnFingerDown;
      }

      private void Awake()
      {
         for(int i = 0; i < transform.childCount; i++)
         {
            var tmpActualCollider = transform.GetChild(i).GetComponent<Collider2D>();

            var tmpIndexNext = i + 1;

            if(tmpIndexNext == transform.childCount)
               tmpIndexNext = 0;

            var tmpNextCollider = transform.GetChild(tmpIndexNext).GetComponent<Collider2D>();

            var tmpIndexPrevious = i - 1;

            if(tmpIndexPrevious == -1)
               tmpIndexPrevious = transform.childCount - 1;

            var tmpPreviousCollider = transform.GetChild(tmpIndexPrevious).GetComponent<Collider2D>();
            dictionaryCollider2D_WrapperCollider.Add(tmpActualCollider, new WrapperCollider { nextCollider = tmpNextCollider, previousCollider = tmpPreviousCollider });
         }
      }

      private void OnFingerDown(LeanFinger argLeanFinger)
      {
         var tmpCollider = Physics2D.OverlapCircle(argLeanFinger.GetWorldPosition(10), 0.2f, layerMaskShape);

         if(tmpCollider)
         {
            nextColliderTouched = tmpCollider;
            tmpCollider.gameObject.SetActive(false);
         }
      }

      private void OnFingerUp(LeanFinger argLeanFinger)
      {
         nextColliderTouched = null;

         foreach(Transform tmpCollider2D in transform)
            tmpCollider2D.gameObject.SetActive(true);
      }

      private void OnFingerUpdate(LeanFinger argLeanFinger)
      {
         var tmpCollider = Physics2D.OverlapCircle(argLeanFinger.GetWorldPosition(10), 0.2f, layerMaskShape);

         if(tmpCollider)
         {
            if(nextColliderTouched != null)
            {
               if(dictionaryCollider2D_WrapperCollider[nextColliderTouched].nextCollider == tmpCollider)
               {
                  nextColliderTouched = dictionaryCollider2D_WrapperCollider[nextColliderTouched].nextCollider;
                  tmpCollider.gameObject.SetActive(false);
                  CheckIfFinishDrawShape();
               }
               else if(dictionaryCollider2D_WrapperCollider[nextColliderTouched].previousCollider == tmpCollider)
               {
                  nextColliderTouched = dictionaryCollider2D_WrapperCollider[nextColliderTouched].previousCollider;
                  tmpCollider.gameObject.SetActive(false);
                  CheckIfFinishDrawShape();
               }
               else
               {
                  nextColliderTouched = null;
                  OnFailDrawShape.Invoke();
                  Debug.LogError("Se salto");
               }
            }
         }
      }

      private void CheckIfFinishDrawShape()
      {
         foreach(Transform tmpCollider2D in transform)
            if(tmpCollider2D.gameObject.activeSelf)
               return;

         nextColliderTouched = null;
         Debug.Log("Finish Draw Shape");
         OnFinishDrawShape.Invoke();
      }

      private class WrapperCollider
      {
         public Collider2D previousCollider;

         public Collider2D nextCollider;
      }
   }
}