using System;
using ActiveZones2D.Scripts.NSManagerAumentos;
using UnityEngine;

namespace NSManagerAumentos
{
   public class OcultarObjetoSiNoHayAumentosAbiertos : MonoBehaviour
   {
      #region members

      [SerializeField]
      private GameObject[] arrayGameObjectsParaOcultar;

      #endregion

      #region MonoBehaviour

      private void Update()
      {
         OcultarObjetos();
      }

      #endregion

      #region methods

      public void OcultarObjetos()
      {
         if(ManagerAumentos.SingletonExist)
            foreach(var tmpGameObject in arrayGameObjectsParaOcultar)
               tmpGameObject.SetActive(!ManagerAumentos.Instance.GetIsInMainAumento());
      }
      #endregion
   }
}