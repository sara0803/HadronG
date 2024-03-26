using System.Collections;
using System.Collections.Generic;
using NSManagerAumentos;
using Singleton;
using UnityEngine;

namespace ActiveZones2D.Scripts.NSManagerAumentos
{
   public class ManagerAumentos : Singleton<ManagerAumentos>
   {
      [SerializeField]
      private GameObject aumentoPrincipal;

      private Aumento refAumentoPrincipal;

      [SerializeField]
      private GameObject[] arrayGameObjectsAumentos;

      [SerializeField]
      private GameObjectParaOcultar[] arrayGameObjectParaEsconder;

      private Dictionary<string, Aumento> dictionaryAumentos;

      private Stack<Aumento> stackAumentos = new Stack<Aumento>();

      bool canShowAumento = true;

      public bool GetIsInMainAumento()
      {
         return refAumentoPrincipal == stackAumentos.Peek();
      }

      public bool CanShowAumento
      {
         get
         {
            return canShowAumento;
         }
         set
         {
            canShowAumento = value;
         }
      }
       
      private void Awake()
      {
         refAumentoPrincipal = new Aumento(aumentoPrincipal);
         stackAumentos.Push(refAumentoPrincipal);
         InicializarDictionaryAumentos();
      }

      private IEnumerator Start()
      {
         yield return null;
         refAumentoPrincipal.GuardarEstadoActualPropiedadEnableDelPolygonCollider();

         foreach(var tmpAumento in dictionaryAumentos)
            tmpAumento.Value.GuardarEstadoActualPropiedadEnableDelPolygonCollider();
      }

      private void InicializarDictionaryAumentos()
      {
         if(dictionaryAumentos == null)
         {
            dictionaryAumentos = new Dictionary<string, Aumento>();

            foreach(var tmpItem in arrayGameObjectsAumentos)
               dictionaryAumentos.Add(tmpItem.name, new Aumento(tmpItem));
         }
      }

      public void AddAumento(GameObject argAumento)
      {
         stackAumentos.Push(new Aumento(argAumento));
      }

      public void MostrarAumento(string argNombreAumentoQueSeQuiereMostrar, bool argEsconderElAumentoQueQuedaDetras = true)
      {
         if(dictionaryAumentos.ContainsKey(argNombreAumentoQueSeQuiereMostrar))
         {
            if(CanShowAumento)
            {
               var tmpAumentoPrevio = stackAumentos.Peek();
               tmpAumentoPrevio.SetInteracionDeTodosLosObjetos2DEnElAumento(false);

               if(argEsconderElAumentoQueQuedaDetras)
                  tmpAumentoPrevio.OcultarAumento();

               var tmpAumento = dictionaryAumentos[argNombreAumentoQueSeQuiereMostrar];
               tmpAumento.SetInteracionDeTodosLosObjetos2DEnElAumento();
               tmpAumento.MostrarAumento();
               stackAumentos.Push(tmpAumento);

               foreach(var tmpGameObjectForHide in arrayGameObjectParaEsconder)
                  tmpGameObjectForHide.Ocultar(tmpAumento.gameObjectAumento);
            }
         }
         else
            Debug.LogError("El aumento : " + argNombreAumentoQueSeQuiereMostrar + " agreguelo al array de aumentos.", this);
      }

      public bool EsconderUltimoAumentoMostrado()
      {
         var tmpAumentoActual = stackAumentos.Peek();

         if(tmpAumentoActual != refAumentoPrincipal)
         {
            tmpAumentoActual.SetInteracionDeTodosLosObjetos2DEnElAumento(false);
            StartCoroutine(CouHiddeLastAumento(tmpAumentoActual));
            stackAumentos.Pop();

            var tmpAumentoVisible = stackAumentos.Peek();
            tmpAumentoVisible.MostrarAumento();
            tmpAumentoVisible.SetInteracionDeTodosLosObjetos2DEnElAumento();

            foreach(var tmpGameObjectForHide in arrayGameObjectParaEsconder)
               tmpGameObjectForHide.Mostrar();

            return true;
         }

         return false;
      }

      public bool EstaEnAumentoPrincipal()
      {
         return stackAumentos.Peek() == refAumentoPrincipal;
      }

      public GameObject GetAumentoActual()
      {
         var tmpAumentoVisible = stackAumentos.Peek();
         return tmpAumentoVisible.gameObjectAumento;
      }

      private IEnumerator CouHiddeLastAumento(Aumento argAumentoParaOcultar)
      {
         yield return null;
         argAumentoParaOcultar.OcultarAumento();
      }
   }
}