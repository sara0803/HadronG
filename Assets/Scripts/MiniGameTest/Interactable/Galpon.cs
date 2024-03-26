using System;
using ActiveZones2D.Scripts.NSActiveZones2D;
using UnityEngine;

namespace MiniGameTest.Interactable
{
    [Serializable]
    public class GallinaTypeRender
    {
        public GallinaType type;
        public GameObject gameObject;
        
    }
    
    [Serializable]
    public class GallinaGalponRender
    {
        public GallinaTypeRender[] gallinaRenders;
        private GallinaType _currentType;

        public void ShowGallina(GallinaType gallinaType)
        {
            GetGallinaRenderByType(_currentType).gameObject.SetActive(false);
            GetGallinaRenderByType(gallinaType).gameObject.SetActive(true);
            _currentType = gallinaType;
        }

        private GallinaTypeRender GetGallinaRenderByType(GallinaType type)
        {
            foreach (GallinaTypeRender gallinaTypeRender in gallinaRenders)
            {
                if (gallinaTypeRender.type == type)
                {
                    return gallinaTypeRender;
                }
            }

            throw new Exception($"No Gallina Render for type {type}");
        }
    }
    
    public class Galpon : Objeto2D
    {
        public SpriteRenderer spriteRenderer;
        public GallinaGalponRender gallinaTypeSprite;
        
        public override Enum ActualStage
        {
            protected get => actualStage;
            set => actualStage = value;
        }
        
        protected override void ComenzoSeleccionDeEsteObjeto()
        {
            
        }

        public override void SeSoltoEsteObjetoSobreAlgunaZonaActiva(string argNombreZonaActivaSobreLaQueSeSoltoEsteObjeto)
        {
            
        }

        protected override void SeSoltoEsteObjetoSinEstarCercaAUnaZonaActiva()
        {
            
        }

        protected override void ObjetoAlcanzoLaPosicionPreviaDespuesDeSoltarseSobreNada()
        {
            
        }

        public override void TabSobreEsteObjeto()
        {
            
        }

        protected override void TabSobreAlgunaZonaActivaDeEsteObjeto(string argNombreZonaActivaSobreLaQueSeHizoTab)
        {
            
        }

        protected override void EsteObjetoSeEstaTransladandoMientrasSeSelecciona()
        {
            
        }

        public void ShowGallina(GallinaType type)
        {
            gallinaTypeSprite.ShowGallina(type);
        }
 
    }
}