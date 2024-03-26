#pragma warning disable
using System.Collections;
using ActiveZones2D.Scripts.NSActiveZones2D;
using UnityEngine;
using UnityEngine.Events;

namespace NSActiveZones2D
{
   [RequireComponent(typeof(PolygonCollider2D), typeof(SpriteRenderer))]
   public class ZonaActiva : MonoBehaviour
   {
      #region members

      [SerializeField]
      private bool enabledInAwake;

      [Tooltip("La zona activa es un punto, o un area sobre el mundo?"), SerializeField]
      private bool esArea;

      [Tooltip("Punto de colocacion de la zona activa en el que se hubicara el objeto dueño de esta zona activa"), SerializeField]
      private Transform tranformPuntoColocacion;

      [Tooltip("Configura puntos extra para la detección de este elemento en partes diferentes al centro del target"), SerializeField]
      private Transform[] arrayPuntosColocacionAdicionales;

      [Tooltip("Distancia minima para moverse al punto de colocacion de la zona activa a la que se esta transladando el objeto"), SerializeField]
      private float minDistanciaParaMoverseAlPuntoColocacion = 0.75f;

      private int estaActiva;

      [Header("Uso de la zona activa"), SerializeField]
      private bool zonaActivaEstaEnUso;

      [SerializeField]
      private bool zonaActivaPuedeSerUsadaCuandoYaEstaSiendoUsada;

      [Header("Visualizacion Zona Activa"), SerializeField]
      private float multiplicadorBrilloAnimacion = 1f;

      private float multiplicadorBrilloAnterior = -1;

      private SpriteRenderer spriteRendererImagenZonaActiva;

      private PolygonCollider2D refPolygonCollider2D;

      private ContactFilter2D refContactFilter2D;

      private Collider2D[] arrayCollidersOverlap = new Collider2D[25];

      public UnityAction<string> delegateOnTab;

      [SerializeField]
      private bool animarZonaActiva = true;

      [Header("Interactividad de la zona activa"), SerializeField]
      private bool esInteractuable = true;

      private bool estadoAnteriorPropiedadEnablePolygonCollider = true;

      #endregion

      #region propierties

      public bool EstaActiva
      {
         get
         {
            return estaActiva > 0;
         }
      }

      public PolygonCollider2D RefPolygonCollider2D
      {
         get
         {
            return refPolygonCollider2D;
         }
      }

      public bool ESArea
      {
         get
         {
            return esArea;
         }
      }

      public Transform TranformPuntoColocacion
      {
         get
         {
            return tranformPuntoColocacion;
         }
      }

      public Transform[] ArrayAdditionalAnchorPoints
      {
         get
         {
            return arrayPuntosColocacionAdicionales;
         }

         set
         {
            arrayPuntosColocacionAdicionales = value;
         }
      }

      public bool ZonaActivaEstaDisponible
      {
         get
         {
            if(zonaActivaEstaEnUso)
               return zonaActivaPuedeSerUsadaCuandoYaEstaSiendoUsada;

            return true;
         }
      }

      public bool ZonaActivaEstaEnUso
      {
         set
         {
            zonaActivaEstaEnUso = value;
         }
      }

      public bool EsInteractuable
      {
         get
         {
            return esInteractuable;
         }
         set
         {
            esInteractuable = value;

            if(!refPolygonCollider2D)
               refPolygonCollider2D = GetComponent<PolygonCollider2D>();

            if(esInteractuable)
            {
               if(multiplicadorBrilloAnterior != -1f)
                  multiplicadorBrilloAnimacion = multiplicadorBrilloAnterior;

               refPolygonCollider2D.enabled = estadoAnteriorPropiedadEnablePolygonCollider;
            }
            else
            {
               if(multiplicadorBrilloAnterior == -1f)
                  multiplicadorBrilloAnterior = multiplicadorBrilloAnimacion;

               estadoAnteriorPropiedadEnablePolygonCollider = refPolygonCollider2D.enabled;
               refPolygonCollider2D.enabled = false;
               multiplicadorBrilloAnimacion = 0;
            }
         }
      }

      internal bool EsOpcionParaColocarObjeto { get; set; }

      public float MultiplicadorBrilloAnimacion
      {
         set
         {
            multiplicadorBrilloAnimacion = value;
         }
      }

      #endregion

      #region MonoBehaviour

      private void Awake()
      {
         GetReferencias();
         SetActiveZoneAsArea();

         gameObject.layer = LayerMaskToLayer(SeleccionarYTransladarObjeto2D.Instance.LayerMaskSeleccionObjeto2D.value);
         gameObject.SetActive(EstaActiva || enabledInAwake);
      }

      private void OnEnable()
      {
         StartCoroutine(CouAnimationZonaActiva());
      }

      private void OnDisable()
      {
         StopCoroutine(CouAnimationZonaActiva());
      }

      #endregion

      #region Methods

      private void GetReferencias()
      {
         spriteRendererImagenZonaActiva = GetComponent<SpriteRenderer>();

         if(!refPolygonCollider2D)
            refPolygonCollider2D = GetComponent<PolygonCollider2D>();
      }

      private void SetActiveZoneAsArea()
      {
         if(esArea)
         {
            refContactFilter2D.SetLayerMask(SeleccionarYTransladarObjeto2D.Instance.LayerMaskSeleccionObjeto2D);
            refContactFilter2D.useTriggers = false;
            refContactFilter2D.useDepth = false;
         }
      }

      private int LayerMaskToLayer(int argBitmask)
      {
         var tmpResult = argBitmask > 0? 0 : 31;

         while(argBitmask > 1)
         {
            argBitmask = argBitmask >> 1;
            tmpResult++;
         }

         return tmpResult;
      }

      /// <summary>
      /// Guarda el estado actual del collider, activado/desactivado
      /// </summary>
      internal void SavePreviousPolygonColliderEnableStatus()
      {
         if(!refPolygonCollider2D)
            refPolygonCollider2D = GetComponent<PolygonCollider2D>();

         estadoAnteriorPropiedadEnablePolygonCollider = refPolygonCollider2D.enabled;
      }

      internal void OnTab()
      {
         if(EstaActiva && esInteractuable)
            delegateOnTab(name);
      }

      /// <summary>
      /// Calcula la distancia que hay hasta esta zona activa
      /// </summary>
      /// <param name="argPositionTouch">Posicion desde la cual se evalua la distancia</param>
      /// <param name="argPolygonCollider2D">Collider2D del objeto para chekear si esta sobre el area de la zona activa y retornar distancia 0 en caso de que se encuentre encima</param>
      /// <returns>Distancia que hay hasta el punto de la zona activa.</returns>
      internal float GetDistanciaHastaEstaZonaActiva(Vector2 argPositionTouch, PolygonCollider2D argPolygonCollider2D)
      {
         if(esArea)
         {
            var tmpQuantityCollisions = refPolygonCollider2D.OverlapCollider(refContactFilter2D, arrayCollidersOverlap);

            for(int i = 0; i < tmpQuantityCollisions; i++)
               if(arrayCollidersOverlap[i] == argPolygonCollider2D)
                  return 0f;

            return Mathf.Infinity;
         }

         var tmpDistanceToActiveZone = Vector2.Distance(argPositionTouch, tranformPuntoColocacion.position);

         // Si el arreglo es nulo, entonces su longitud es 0
         //int length = arrayPuntosColocacionAdicionales?.Length ?? 0;
         int length = 0;
         if(arrayPuntosColocacionAdicionales != null)
         {
            length = arrayPuntosColocacionAdicionales.Length;
         }

         float tmpMinDistance = Mathf.Infinity;

         for(int i = 0; i < length; i++)
         {
            var tmpPuntoColocacionActual = arrayPuntosColocacionAdicionales[i];

            if(tmpPuntoColocacionActual == null)
               continue;

            var tmpDistanciaAlPuntoColocacionActual = Vector2.Distance(argPositionTouch, tmpPuntoColocacionActual.position);

            if(tmpDistanciaAlPuntoColocacionActual < tmpMinDistance)
               tmpMinDistance = tmpDistanciaAlPuntoColocacionActual;
         }

         tmpDistanceToActiveZone = Mathf.Min(tmpDistanceToActiveZone, tmpMinDistance);

         if(tmpDistanceToActiveZone <= minDistanciaParaMoverseAlPuntoColocacion)
            return tmpDistanceToActiveZone;

         return Mathf.Infinity;
      }

      /// <summary>
      /// Calcula la posicion de la zona activa
      /// </summary>
      /// <param name="argTransform">Posicion actual desde donde se calcula</param>
      /// <returns>Posicion de la zona activa calculada</returns>
      internal Transform GetTransformDeEstaZonaActivaParaColocarse(Transform argTransform)
      {
         if(esArea)
            return argTransform;

         return tranformPuntoColocacion;
      }

      /// <summary>
      /// Activa la animacion de la zona activa
      /// </summary>
      /// <param name="argActivate">Activar?</param>
      public void ActivarEstaZonaActiva(bool argActivate = true)
      {
         estaActiva += argActivate? 1 : -1;
         gameObject.SetActive(EstaActiva);
      }

      /// <summary>
      /// Activa la animacion de la zona activa, sirve para la zona activa propia se active si o si
      /// </summary>
      /// <param name="argActivate">Activar?</param>
      public void ForzarActivarEstaZonaActiva(bool argActivate = true)
      {
         estaActiva = argActivate? 1 : 0;
         gameObject.SetActive(EstaActiva);
      }

      internal void GuardarEstadoActualPropiedadEnableDelPolygonCollider()
      {
         if(!refPolygonCollider2D)
            refPolygonCollider2D = GetComponent<PolygonCollider2D>();

         estadoAnteriorPropiedadEnablePolygonCollider = refPolygonCollider2D.enabled;
      }

      private float GetMultiplicadorBrilloAnimacion()
      {
         return (EsOpcionParaColocarObjeto)? SeleccionarYTransladarObjeto2D.Instance.MultiplicadorBrilloAnimacionZonaActivaObjetivo : 0;
      }

      #endregion

      #region Courutines

      private IEnumerator CouAnimationZonaActiva()
      {
         if(!animarZonaActiva)
            yield break;

         while(true)
         {
            spriteRendererImagenZonaActiva.color = EstaActiva? new Color(1, 1, 1, SeleccionarYTransladarObjeto2D.Instance.MultiplicadorAnimacionZonaActiva * multiplicadorBrilloAnimacion + GetMultiplicadorBrilloAnimacion()) : new Color(1, 1, 1, 0);
            yield return null;
         }
      }

      #endregion
   }
}