using GeneralsMiniGames;

namespace MiniGameTest.Interactable
{

    public class Distractores : InteractableBase
    {
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
            AttemptsCounter.Instance.AddAttempt();
        }

        protected override void TabSobreAlgunaZonaActivaDeEsteObjeto(string argNombreZonaActivaSobreLaQueSeHizoTab)
        {
            
        }

        protected override void EsteObjetoSeEstaTransladandoMientrasSeSelecciona()
        {
            
        }
        
    }
}