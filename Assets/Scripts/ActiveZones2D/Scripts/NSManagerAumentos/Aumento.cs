#pragma warning disable
using ActiveZones2D.Scripts.NSActiveZones2D;
using NSActiveZones2D;
using UnityEngine;

namespace NSManagerAumentos
{
    public class Aumento
    {
        public readonly GameObject gameObjectAumento;

        private readonly Objeto2D[] arrayAbstractObject2D;

        public Aumento(GameObject argGameObjectAumento)
        {
            gameObjectAumento = argGameObjectAumento;
            arrayAbstractObject2D = gameObjectAumento.GetComponentsInChildren<Objeto2D>(true);
        }

        public void OcultarAumento()
        {
            gameObjectAumento.SetActive(false);
        }

        public void MostrarAumento()
        {
            gameObjectAumento.SetActive(true);
        }

        public void SetInteracionDeTodosLosObjetos2DEnElAumento(bool argInteractuable = true)
        {
            foreach (var tmpObject2D in arrayAbstractObject2D)
                tmpObject2D.Interactuable = argInteractuable;
        }

        public void GuardarEstadoActualPropiedadEnableDelPolygonCollider()
        {
            var tmpArrayActivesZonesOnWorld = gameObjectAumento.GetComponentsInChildren<ZonaActiva>(true);

            foreach (var tmpActiveZoneOnWorld in tmpArrayActivesZonesOnWorld)
                tmpActiveZoneOnWorld.GuardarEstadoActualPropiedadEnableDelPolygonCollider();
        }
    }
}