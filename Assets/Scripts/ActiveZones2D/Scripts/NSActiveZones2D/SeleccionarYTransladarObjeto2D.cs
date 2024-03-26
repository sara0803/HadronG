using System.Collections;
using Lean.Touch;
using NSActiveZones2D;
using Singleton;
using UnityEngine;

namespace ActiveZones2D.Scripts.NSActiveZones2D
{
   public class SeleccionarYTransladarObjeto2D : Singleton<SeleccionarYTransladarObjeto2D>
   {
      [Tooltip("Layer en donde solo estan objetos que usen la clase AbstractObject2D")]
      [SerializeField]
      private LayerMask layerMaskSeleccionObjeto2D;

      [Tooltip("Camara desde donde se seleccionan los objetos")]
      [SerializeField]
      private Camera refCamera;

      [Tooltip("Radio alrededor del touch que se tiene encuenta para seleccionar los objetos")]
      [SerializeField]
      private float radioSeleccionAlrededorDelPuntoDelTouch = 0.2f;

      [Tooltip("Cuanto se tiene que mover el mouse para que se detecte como movimiento")]
      [SerializeField]
      private float umbralMovimientoMouseParaMoverObjeto = 0.05f;

      /// <summary>
      /// Objeto actualmente seleccioando
      /// </summary>
      private Objeto2D refObjeto2DSelected;

      /// <summary>
      /// ZonaActiva actualmente seleccioanda
      /// </summary>
      private ZonaActiva refZonaActivaSelected;

      private float touchScreenDelta;

      [Header("Zonas activas")]
      [SerializeField]
      private float velocidadAnimacionZonaActiva;

      [SerializeField]
      private float multiplicadorBrilloAnimacionZonaActivaObjetivo = 0.5f;

      [SerializeField, Tooltip("Velocidad con la que se mueve un objeto hacia el mouse que se esta arrastrando ")]
      private float velocidadMovimientoHaciaMouse = 0.5f;

      [SerializeField]
      private float velocidadMovimientoGeneral;

      /// <summary>
      /// Factor de animacion de las zonas activas
      /// </summary>
      private float multiplicadorAnimacionZonaActiva;

      public LayerMask LayerMaskSeleccionObjeto2D
         => layerMaskSeleccionObjeto2D;

      public float MultiplicadorAnimacionZonaActiva
         => multiplicadorAnimacionZonaActiva;

      public float MultiplicadorBrilloAnimacionZonaActivaObjetivo
         => multiplicadorBrilloAnimacionZonaActivaObjetivo;

      public float VelocidadMovimientoHaciaMouse
         => velocidadMovimientoHaciaMouse;

      public float VelocidadMovimientoGeneral
         => velocidadMovimientoGeneral;

      private void Awake()
      {
         StartCoroutine(CouAnimacionZonaActiva());
      }

      private void OnEnable()
      {
         LeanTouch.OnFingerDown += OnFingerDown;
         LeanTouch.OnFingerUpdate += OnFingerUpdate;
         LeanTouch.OnFingerTap += OnFingerTab;
         LeanTouch.OnFingerUp += OnFingerUp;
      }

      private void OnDisable()
      {
         LeanTouch.OnFingerDown -= OnFingerDown;
         LeanTouch.OnFingerUpdate -= OnFingerUpdate;
         LeanTouch.OnFingerTap -= OnFingerTab;
         LeanTouch.OnFingerUp -= OnFingerUp;
      }

      private void OnFingerDown(LeanFinger argLeanFinger)
      {
         if(argLeanFinger.IsOverGui)
            return;

         touchScreenDelta = 0f;
         var tmpPositionOnWorld = refCamera.ScreenToWorldPoint(argLeanFinger.ScreenPosition);
         var tmpArrayCollision2D = Physics2D.OverlapCircleAll(tmpPositionOnWorld, radioSeleccionAlrededorDelPuntoDelTouch, layerMaskSeleccionObjeto2D);

         Collider2D tmpCollider2DMoreNear = null;
         var tmpMinDistance = float.MaxValue;

         foreach(var tmpCollider2D in tmpArrayCollision2D)
            if(tmpCollider2D.transform.position[2] < tmpMinDistance)
            {
               tmpMinDistance = tmpCollider2D.transform.position[2];
               tmpCollider2DMoreNear = tmpCollider2D;
            }

         if(tmpCollider2DMoreNear)
         {
            refObjeto2DSelected = tmpCollider2DMoreNear.GetComponent<Objeto2D>();

            if(refObjeto2DSelected && refObjeto2DSelected.AccionActual == AccionActual.WaitingForAction && refObjeto2DSelected.Interactuable)
            {
               refObjeto2DSelected.PosicionObjetivo = tmpPositionOnWorld;
               refObjeto2DSelected.ObjetoSeleccionado();
            }
            else
               refObjeto2DSelected = null;

            refZonaActivaSelected = tmpCollider2DMoreNear.GetComponent<ZonaActiva>();
         }
      }

      private void OnFingerUpdate(LeanFinger argLeanFinger)
      {
         touchScreenDelta += argLeanFinger.ScreenDelta.magnitude;

         if(touchScreenDelta > umbralMovimientoMouseParaMoverObjeto)
            if(refObjeto2DSelected && refObjeto2DSelected.Interactuable && refObjeto2DSelected.ObjetoSePuedeMover)
            {
               refObjeto2DSelected.PosicionObjetivo = refCamera.ScreenToWorldPoint(argLeanFinger.ScreenPosition);
               refObjeto2DSelected.ObjetoEstaSeleccionadoYSeEstaTransladando();
            }
      }

      private void OnFingerTab(LeanFinger argLeanFinger)
      {
         if(argLeanFinger.IsOverGui)
            return;

         if(refObjeto2DSelected && refObjeto2DSelected.Interactuable && refObjeto2DSelected.AccionActual is AccionActual.WaitingForAction)
         {
            refObjeto2DSelected.TabSobreEsteObjeto();
            return;
         }

         if(refZonaActivaSelected)
            refZonaActivaSelected.OnTab();
      }

      private void OnFingerUp(LeanFinger argLeanFinger)
      {
         if(refObjeto2DSelected)
            refObjeto2DSelected.ObjetoDeseleccionado();

         refObjeto2DSelected = null;
         refZonaActivaSelected = null;
      }

      private IEnumerator CouAnimacionZonaActiva()
      {
         var tmpAngle = 0f;

         while(true)
         {
            tmpAngle += (180f / velocidadAnimacionZonaActiva) * Time.deltaTime;
            tmpAngle %= 180f;
            multiplicadorAnimacionZonaActiva = Mathf.Sin(tmpAngle * Mathf.Deg2Rad);
            yield return null;
         }
      }
   }
}