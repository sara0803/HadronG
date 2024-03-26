using UnityEngine;

namespace DeviceConfiguration
{
   public class CameraSizeSetup : MonoBehaviour
   {
      [SerializeField]
      private float orthographicSizeTablets = 5;

      [SerializeField]
      private float horizontalOffset;

      private void Awake()
      {
         var tmpAspectRatio = (float)Screen.width / Screen.height;

         if(Mathf.Abs(tmpAspectRatio - 1.6f) <= 0.05f)
         {
            Camera.main.orthographicSize = orthographicSizeTablets;
            Camera.main.transform.position += new Vector3(horizontalOffset, 0);
         }
      }
   }
}