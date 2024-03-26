using System.Collections.Generic;
using NSActiveZones2D;
using UnityEngine;

namespace ActiveZones2D.Scripts.NSActiveZones2D
{
   [RequireComponent(typeof(PolygonCollider2D))]
   public abstract class Objeto2D : Stages.Stages
   {
      /// <summary>
      /// Zona activa propia del objeto, sirve para indicarle al usuario si este objeto se puede seleccionar
      /// </summary>
      [Tooltip("Zona activa para este objeto, sirve para indicarle al usuario si este objeto se puede seleccionar")]
      private ZonaActiva zonaActivaParaInteractuarConEsteObjeto;

      /// <summary>
      /// Todas las zonas activas relacionadas con este objeto
      /// </summary>
      [Tooltip("Todas las zonas activas relacionadas con este objeto")]
      [SerializeField]
      protected ZonaActiva[] arrayZonasActivasConLasQuePuedeInteractuarEsteObjeto;

      /// <summary>
      /// Saber si el objeto esta esperando por interaccion o se esta moviendo
      /// </summary>
      private AccionActual accionActual;

      /// <summary>
      /// Para acceder facilmente por nombre a cualquiera de las zonas activas
      /// </summary>
      protected readonly Dictionary<string, ZonaActiva> dictionaryZonasActivas = new Dictionary<string, ZonaActiva>();

      /// <summary>
      /// Lista que contiene todas las zonas activas relacionadas con la etapa actual del objeto, de esta manera se pueden iluminar las zonas activas
      /// </summary>
      private readonly List<ZonaActiva> listZonasActivasInteractuablesEnLaAccionActualDelObjeto = new List<ZonaActiva>();

      /// <summary>
      /// Posicion inicial del objeto 2D en las posiciones X y Y solamente
      /// </summary>
      private Vector3 posicionInicialXY;

      private Vector3 posicionInicialZ;

      /// <summary>
      /// posicion hacia donde el objeto deberia moverses
      /// </summary>
      private Vector3 posicionObjetivo;

      /// <summary>
      /// Desface desde el pivot del objeto hasta la posicion del touch
      /// </summary>
      private Vector3 posicionDesfaceHastaTouch;

      /// <summary>
      /// Zona activa mas cercana
      /// </summary>
      private ZonaActiva refZonaActivaMasCercana;

      /// <summary>
      /// Para calcular colisiones con las zonas activas que son areas
      /// </summary>
      private PolygonCollider2D refPolygonCollider2D;

      protected bool activarZonasActivasRelacionadasConLaAccionActual = true;

      protected ZonaActiva refZonaActivaAnterior;

      protected bool objetoSePuedeMover = true;

      protected bool interactuable = true;

      private bool objetoEstaInicializado;

      /// <summary>
      /// Asignar la posicion activa
      /// </summary>
      public Vector3 PosicionObjetivo
      {
         get
         {
            return posicionObjetivo;
         }
         set
         {
            posicionObjetivo = value;
         }
      }

      /// <summary>
      /// Saber si el objeto esta esperando por interaccion o se esta moviendo al punto inicial o a otro punto
      /// </summary>
      public AccionActual AccionActual
      {
         get
         {
            return accionActual;
         }
      }

      /// <summary>
      /// Habilita o deshabilita el movimiento de un objeto con iteraccion por ejemplo cuando el objeto solo se quiere clickear esto debe ser falso
      /// </summary>
      public bool ObjetoSePuedeMover
      {
         get
         {
            return objetoSePuedeMover;
         }
         set
         {
            objetoSePuedeMover = value;
         }
      }

      public bool Interactuable
      {
         get
         {
            return interactuable;
         }
         set
         {
            interactuable = value;

            foreach(var tmpActiveZone in arrayZonasActivasConLasQuePuedeInteractuarEsteObjeto)
               tmpActiveZone.EsInteractuable = interactuable;
         }
      }

      protected virtual void Awake()
      {
         InicializarObjeto();
      }

      protected virtual void Update()
      {
         MoverseHaciaLaPosicionInicial();
         MoverseHaciaLaNuevaPosicion();
         MoverseHaciaZonaActiva();
      }

      internal void InicializarObjeto()
      {
         if(objetoEstaInicializado)
            return;

         InicializarDictionaryZonasActivas();
         var tmpInitPosition = transform.position;
         posicionInicialXY = new Vector3(tmpInitPosition[0], tmpInitPosition[1], 0);
         posicionInicialZ = new Vector3(0, 0, tmpInitPosition[2]);
         refPolygonCollider2D = GetComponent<PolygonCollider2D>();
         refPolygonCollider2D.enabled = false;
         DesactivarColliderDeLaZonaActivaParaInteractuarConEsteObjeto();
         gameObject.layer = LayerMaskToLayer(SeleccionarYTransladarObjeto2D.Instance.LayerMaskSeleccionObjeto2D.value);
         objetoEstaInicializado = true;
      }

      protected void InicializarDictionaryZonasActivas()
      {
         dictionaryZonasActivas.Clear();

         foreach(var tmpActiveZoneOnWorld in arrayZonasActivasConLasQuePuedeInteractuarEsteObjeto)
         {
            var tmpNewKey = tmpActiveZoneOnWorld.name;

            if(dictionaryZonasActivas.ContainsKey(tmpNewKey))
               Debug.Log("La zona activa : " + tmpNewKey + " esta repetida en el array de zonas activas de este objeto.", this);
            else
            {
               dictionaryZonasActivas.Add(tmpActiveZoneOnWorld.name, tmpActiveZoneOnWorld);
               tmpActiveZoneOnWorld.delegateOnTab += TabSobreAlgunaZonaActivaDeEsteObjeto;
            }
         }
      }

      private void DesactivarColliderDeLaZonaActivaParaInteractuarConEsteObjeto()
      {
         if(zonaActivaParaInteractuarConEsteObjeto)
            zonaActivaParaInteractuarConEsteObjeto.GetComponent<PolygonCollider2D>().enabled = false;
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

      private void MoverseHaciaLaPosicionInicial()
      {
         if(accionActual == AccionActual.MovingToInitPosition)
         {
            transform.position = Vector3.MoveTowards(transform.position, posicionInicialXY + posicionInicialZ, SeleccionarYTransladarObjeto2D.Instance.VelocidadMovimientoGeneral * Time.deltaTime);

            if(Vector3.Distance(transform.position, posicionInicialXY + posicionInicialZ) < 0.05f)
            {
               accionActual = AccionActual.WaitingForAction;
               transform.position = posicionInicialXY + posicionInicialZ;
               ObjetoAlcanzoLaPosicionPreviaDespuesDeSoltarseSobreNada();
            }
         }
      }

      private void MoverseHaciaZonaActiva()
      {
         if(accionActual == AccionActual.MovingToActiveZone)
         {
            transform.position = Vector3.MoveTowards(transform.position, posicionInicialXY + posicionInicialZ, SeleccionarYTransladarObjeto2D.Instance.VelocidadMovimientoGeneral * Time.deltaTime);

            if(Vector3.Distance(transform.position, posicionInicialXY + posicionInicialZ) < 0.005f)
            {
               accionActual = AccionActual.WaitingForAction;
               SeSoltoEsteObjetoSobreAlgunaZonaActiva(refZonaActivaAnterior.name);
            }
         }
      }

      private void MoverseHaciaLaNuevaPosicion()
      {
         if(accionActual == AccionActual.MovingToNewPosition)
         {
            transform.position = Vector3.MoveTowards(transform.position, posicionInicialXY + posicionInicialZ, SeleccionarYTransladarObjeto2D.Instance.VelocidadMovimientoGeneral * Time.deltaTime);

            if(Vector3.Distance(transform.position, posicionInicialXY + posicionInicialZ) < 0.005f)
               accionActual = AccionActual.WaitingForAction;
         }
      }

      /// <summary>
      /// Consigue la zona activa mas cercana
      /// </summary>
      private ZonaActiva GetZonaActivaMasCercana()
      {
         ZonaActiva tmpZonaActivaMoreNear = null;
         var tmpMinDistance = Mathf.Infinity;

         foreach(var tmpActiveZone in listZonasActivasInteractuablesEnLaAccionActualDelObjeto)
         {
            var tmpMinDistanceNew = tmpActiveZone.GetDistanciaHastaEstaZonaActiva(posicionObjetivo + posicionDesfaceHastaTouch, refPolygonCollider2D);

            if(tmpMinDistanceNew < tmpMinDistance)
            {
               tmpZonaActivaMoreNear = tmpActiveZone;
               tmpMinDistance = tmpMinDistanceNew;
            }
         }

         return tmpZonaActivaMoreNear;
      }

      private void ActualizarColliderDeZonaActivaParaInteractuarConEsteObjeto()
      {
         var tmpPolygonCollider2D = zonaActivaParaInteractuarConEsteObjeto.GetComponent<PolygonCollider2D>();
         tmpPolygonCollider2D.enabled = false;
         refPolygonCollider2D.points = tmpPolygonCollider2D.points;

         var tmpTransformActiveZoneOwn = zonaActivaParaInteractuarConEsteObjeto.TranformPuntoColocacion;
         var tmpNewPosition = new Vector3(tmpTransformActiveZoneOwn.position[0], tmpTransformActiveZoneOwn.position[1], 0);
         refPolygonCollider2D.transform.position = tmpNewPosition + posicionInicialZ;
         transform.rotation = tmpTransformActiveZoneOwn.rotation;
         posicionInicialXY = tmpNewPosition;
      }

      protected abstract void ComenzoSeleccionDeEsteObjeto();

      public abstract void SeSoltoEsteObjetoSobreAlgunaZonaActiva(string argNombreZonaActivaSobreLaQueSeSoltoEsteObjeto);

      protected abstract void SeSoltoEsteObjetoSinEstarCercaAUnaZonaActiva();

      protected abstract void ObjetoAlcanzoLaPosicionPreviaDespuesDeSoltarseSobreNada();

      public abstract void TabSobreEsteObjeto();

      protected abstract void TabSobreAlgunaZonaActivaDeEsteObjeto(string argNombreZonaActivaSobreLaQueSeHizoTab);

      protected abstract void EsteObjetoSeEstaTransladandoMientrasSeSelecciona();

      public void ObjetoSeleccionado()
      {
         posicionDesfaceHastaTouch = transform.position - posicionObjetivo;
         ComenzoSeleccionDeEsteObjeto();
      }

      public void ObjetoEstaSeleccionadoYSeEstaTransladando()
      {
         if(activarZonasActivasRelacionadasConLaAccionActual)
         {
            foreach(var tmpActiveZone in listZonasActivasInteractuablesEnLaAccionActualDelObjeto)
               tmpActiveZone.ActivarEstaZonaActiva();

            activarZonasActivasRelacionadasConLaAccionActual = false;
         }

         accionActual = AccionActual.InMovement;

         if(refZonaActivaAnterior)
         {
            refZonaActivaAnterior.ZonaActivaEstaEnUso = false;
            refZonaActivaAnterior.EsOpcionParaColocarObjeto = false;
         }

         if(refZonaActivaMasCercana)
            refZonaActivaMasCercana.EsOpcionParaColocarObjeto = false;

         refZonaActivaMasCercana = GetZonaActivaMasCercana(); //Cual es la zona activa mas cercana?

         var tmpPosition = transform.position;

         if(refZonaActivaMasCercana != null)
         {
            refZonaActivaMasCercana.EsOpcionParaColocarObjeto = true;

            if(refZonaActivaMasCercana.ESArea)
               transform.position = Vector3.Lerp(tmpPosition, posicionObjetivo + posicionDesfaceHastaTouch, SeleccionarYTransladarObjeto2D.Instance.VelocidadMovimientoHaciaMouse * Time.deltaTime);
            else
            {
               var tmpTransformObjective = refZonaActivaMasCercana.GetTransformDeEstaZonaActivaParaColocarse(transform);
               transform.position = Vector3.Lerp(tmpPosition, tmpTransformObjective.position, SeleccionarYTransladarObjeto2D.Instance.VelocidadMovimientoHaciaMouse * Time.deltaTime);
               transform.rotation = Quaternion.Lerp(transform.rotation, tmpTransformObjective.rotation, 0.25f * Time.deltaTime);
            }
         }
         else
            transform.position = Vector3.MoveTowards(tmpPosition, posicionObjetivo + posicionDesfaceHastaTouch, SeleccionarYTransladarObjeto2D.Instance.VelocidadMovimientoHaciaMouse * Time.deltaTime);

         EsteObjetoSeEstaTransladandoMientrasSeSelecciona();
      }

      public void ObjetoDeseleccionado()
      {
         if(!objetoSePuedeMover)
            return;

         if(!activarZonasActivasRelacionadasConLaAccionActual)
         {
            foreach(var tmpActiveZone in listZonasActivasInteractuablesEnLaAccionActualDelObjeto)
               tmpActiveZone.ActivarEstaZonaActiva(false);

            activarZonasActivasRelacionadasConLaAccionActual = true;
         }

         listZonasActivasInteractuablesEnLaAccionActualDelObjeto.Clear();

         if(refZonaActivaMasCercana != null)
         {
            if(refZonaActivaAnterior)
               refZonaActivaAnterior.ZonaActivaEstaEnUso = false;

            refZonaActivaAnterior = refZonaActivaMasCercana;
            refZonaActivaMasCercana.ZonaActivaEstaEnUso = true;
            posicionInicialXY = refZonaActivaMasCercana.GetTransformDeEstaZonaActivaParaColocarse(transform).position;
            posicionInicialXY[2] = 0;
            accionActual = AccionActual.MovingToNewPosition;
            SeSoltoEsteObjetoSobreAlgunaZonaActiva(refZonaActivaMasCercana.name);
         }
         else
         {
            accionActual = AccionActual.MovingToInitPosition;
            SeSoltoEsteObjetoSinEstarCercaAUnaZonaActiva();

            if(refZonaActivaAnterior)
               refZonaActivaAnterior.ZonaActivaEstaEnUso = true;
         }

         refZonaActivaMasCercana = null;
      }

      protected void MoverEsteObjetoHastaZonaActiva(string argNombreDeLaZonaActiva, bool argIgnorarQueEsteEnUso)
      {
         if(dictionaryZonasActivas.TryGetValue(argNombreDeLaZonaActiva, out var tmpActiveActive))
         {
            if(!tmpActiveActive.ZonaActivaEstaDisponible && !argIgnorarQueEsteEnUso)
               return;

            if(refZonaActivaAnterior)
               refZonaActivaAnterior.ZonaActivaEstaEnUso = false;

            refZonaActivaAnterior = tmpActiveActive;
            refZonaActivaAnterior.ZonaActivaEstaEnUso = true;
            posicionInicialXY = refZonaActivaAnterior.GetTransformDeEstaZonaActivaParaColocarse(transform).position;
            posicionInicialXY[2] = 0;
            accionActual = AccionActual.MovingToActiveZone;
            refZonaActivaMasCercana = null;
         }
         else
            Debug.LogError("ZonaActiva : " + argNombreDeLaZonaActiva + " |del objeto : " + name + " No existe");
      }

      public void ActivarZonaActiva(string argNombreDeLaZonaActiva, bool argActivar)
      {
         if(dictionaryZonasActivas.TryGetValue(argNombreDeLaZonaActiva, out var tmpActiveZone))
         {
            tmpActiveZone.GetComponent<PolygonCollider2D>().enabled = true;
            tmpActiveZone.ActivarEstaZonaActiva(argActivar);
         }
         else
            Debug.LogError("ZonaActiva : " + argNombreDeLaZonaActiva + " |del objeto : " + name + " No existe");
      }

      //Activar todas la zonas activas
      public void ActivarTodasLasZonas(bool argActivar = true)
      {
         if(arrayZonasActivasConLasQuePuedeInteractuarEsteObjeto != null)
         {
            foreach(var tmpActiveZone in arrayZonasActivasConLasQuePuedeInteractuarEsteObjeto)
            {
               tmpActiveZone.GetComponent<PolygonCollider2D>().enabled = true;
               tmpActiveZone.ActivarEstaZonaActiva(argActivar);
            }
         }
         else
            Debug.LogError("no hay elementos que activar");
      }

      public void ForzarActivarZonaActiva(string argNombreDeLaZonaActiva, bool argActivar)
      {
         if(dictionaryZonasActivas.TryGetValue(argNombreDeLaZonaActiva, out var tmpActiveZone))
            tmpActiveZone.ForzarActivarEstaZonaActiva(argActivar);
         else
            Debug.LogError("ZonaActiva : " + argNombreDeLaZonaActiva + " del objeto : " + name + " No existe");
      }

      /// <summary>
      /// Activa la interaccion de este objeto para que se pueda mover, primero debe asignarsele una zona activa con AsignarZonaActivaParaInteractuarConEsteObjeto()
      /// </summary>
      /// <param name="argActivar"></param>
      public void ActivarInteraccionConEsteObjeto(bool argActivar)
      {
         if(!refPolygonCollider2D)
            refPolygonCollider2D = GetComponent<PolygonCollider2D>();

         refPolygonCollider2D.enabled = argActivar;

         if(zonaActivaParaInteractuarConEsteObjeto)
         {
            zonaActivaParaInteractuarConEsteObjeto.ForzarActivarEstaZonaActiva(argActivar);

            if(argActivar)
               ActualizarColliderDeZonaActivaParaInteractuarConEsteObjeto();
         }
         else
            Debug.LogError("El objeto : " + gameObject.name + " No tiene zona activa propia, asigne una zona activa propia usando el metodo SetZonaActivaParaInteractuarConEsteObjeto(NombreZonaActiva)");
      }

      /// <summary>
      /// Usar para asignar una zona activa que se quiera usar como marco de seleccion o resaltador
      /// </summary>
      /// <param name="argNameActiveZoneOnWorld"></param>
      public void AsignarZonaActivaParaInteractuarConEsteObjeto(string argNameActiveZoneOnWorld = null)
      {
         if(zonaActivaParaInteractuarConEsteObjeto)
         {
            zonaActivaParaInteractuarConEsteObjeto.GetComponent<PolygonCollider2D>().enabled = true;
            zonaActivaParaInteractuarConEsteObjeto.ForzarActivarEstaZonaActiva(false);
         }

         if(argNameActiveZoneOnWorld == null)
         {
            if(zonaActivaParaInteractuarConEsteObjeto)
            {
               zonaActivaParaInteractuarConEsteObjeto.GetComponent<PolygonCollider2D>().enabled = false;
               zonaActivaParaInteractuarConEsteObjeto = null;
            }
         }
         else if(dictionaryZonasActivas.TryGetValue(argNameActiveZoneOnWorld, out var tmpActiveZone))
         {
            zonaActivaParaInteractuarConEsteObjeto = tmpActiveZone;
            DesactivarColliderDeLaZonaActivaParaInteractuarConEsteObjeto();
         }
         else
            Debug.LogError("Zona activa con el nombre : " + argNameActiveZoneOnWorld + " no existe en el array de zonas activas del objeto: " + gameObject.name);
      }

      public void AgregarZonaActivaAListaZonasActivasInteractuablesEnLaAccionActualDelObjeto(string argNombreZonaActivaParaAgregar, bool argAgregar)
      {
         if(dictionaryZonasActivas.TryGetValue(argNombreZonaActivaParaAgregar, out var tmpActiveZone))
         {
            if(argAgregar)
            {
               if(!listZonasActivasInteractuablesEnLaAccionActualDelObjeto.Contains(tmpActiveZone) && tmpActiveZone.ZonaActivaEstaDisponible)
                  listZonasActivasInteractuablesEnLaAccionActualDelObjeto.Add(tmpActiveZone);
            }
            else
            {
               if(listZonasActivasInteractuablesEnLaAccionActualDelObjeto.Contains(tmpActiveZone))
                  listZonasActivasInteractuablesEnLaAccionActualDelObjeto.Remove(tmpActiveZone);
            }
         }
         else
            Debug.LogError("La zona activa con el nombre : " + argNombreZonaActivaParaAgregar + " no existe en el array de zonas activas del objeto: " + gameObject.name);
      }

      public void SetZonaActivaEnUso(string argNombreZonaActiva, bool argEstaEnUso = true)
      {
         if(dictionaryZonasActivas.TryGetValue(argNombreZonaActiva, out var tmpActiveZone))
            tmpActiveZone.ZonaActivaEstaEnUso = argEstaEnUso;
      }

      public void MoverObjetoHaciaPosition(Vector2 argPositionObjetivo)
      {
         posicionInicialXY = argPositionObjetivo;
         accionActual = AccionActual.MovingToNewPosition;
      }
   }

   public enum AccionActual
   {
      WaitingForAction,
      InMovement,
      MovingToInitPosition,
      MovingToNewPosition,
      MovingToActiveZone
   }
}