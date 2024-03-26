using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GeneralsMiniGames
{
   public class FollowShapeDraw : MonoBehaviour
   {
      [SerializeField]
      private float speed = 2.0f, fadeSpeed = 10;

      [SerializeField]
      private SpriteRenderer refSpriteRendererImageFollow;

      [SerializeField]
      private List<Transform> pathPoints;
       
      private int currentPointIndex;

      private float followLength;

      public UnityEvent OnShapeDrawComplete;

      public void SetPathPoint(Transform[] arrayPointToSetup)
      {
         pathPoints.Clear();
         foreach (var tmpPoint in arrayPointToSetup)
            pathPoints.Add(tmpPoint);
      }

      private void OnEnable()
      {
         if (refSpriteRendererImageFollow != null)
         {
            var spriteColor = refSpriteRendererImageFollow.color;
            spriteColor.a = 0f;
            refSpriteRendererImageFollow.color = spriteColor;
         }
      }

      public void SetupFollowShape()
      {
         currentPointIndex = 0;

         if (pathPoints.Count > 0)
         {
            transform.position = pathPoints[0].position;
            followLength = Vector3.Distance(transform.position, pathPoints[currentPointIndex].position);
         }

         if (refSpriteRendererImageFollow != null)
         {
            var spriteColor = refSpriteRendererImageFollow.color;
            spriteColor.a = 0f;
            refSpriteRendererImageFollow.color = spriteColor;
         }
      }

      [ContextMenu("Start Journey")]
      public void StartFollowShape()
      {
         StartCoroutine(FollowPath());
      }

      private IEnumerator FollowPath()
      {
         // Fade In
         while (refSpriteRendererImageFollow != null && refSpriteRendererImageFollow.color.a < 1f)
         {
            var spriteColor = refSpriteRendererImageFollow.color;
            spriteColor.a += Time.deltaTime * (1f / fadeSpeed);
            refSpriteRendererImageFollow.color = spriteColor;
            yield return null;
         }

         while (currentPointIndex < pathPoints.Count)
         {
            Vector3 startPoint = transform.position;
            var tmpPointIndex = currentPointIndex + 1;
            tmpPointIndex = Mathf.Clamp(tmpPointIndex, 0, pathPoints.Count - 1);
            Vector3 targetPosition = pathPoints[tmpPointIndex].position;
            followLength = Vector3.Distance(startPoint, targetPosition);
            float followTime = followLength / speed;
            float startTime = Time.time;

            while (Time.time - startTime < followTime)
            {
               float distanceCovered = (Time.time - startTime) * speed;
               float fractionOfJourney = distanceCovered / followLength;
               transform.position = Vector3.Lerp(startPoint, targetPosition, fractionOfJourney);
               yield return null;
            }

            transform.position = targetPosition;
            currentPointIndex++;
         }

         // Fade Out
         while (refSpriteRendererImageFollow != null && refSpriteRendererImageFollow.color.a > 0f)
         {
            Color spriteColor = refSpriteRendererImageFollow.color;
            spriteColor.a -= Time.deltaTime * (1f / fadeSpeed);
            refSpriteRendererImageFollow.color = spriteColor;
            yield return null;
         }

         OnShapeDrawComplete.Invoke();
         gameObject.SetActive(false);
      }
   }
}
