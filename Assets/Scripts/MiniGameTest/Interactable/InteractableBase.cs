using System;
using ActiveZones2D.Scripts.NSActiveZones2D;
using MiniGameTest.Challenges;

namespace MiniGameTest.Interactable
{
    public abstract class InteractableBase : Objeto2D
    {
        public SpawnZoneType spawnZoneType;
        public override Enum ActualStage
        {
            protected get => actualStage;
            set => actualStage = value;
        }
        
        protected override void Awake()
        {
            base.Awake();
            AsignarZonaActivaParaInteractuarConEsteObjeto("ZonaActiva");
            ActivarInteraccionConEsteObjeto(true);
        }
        
    }
}