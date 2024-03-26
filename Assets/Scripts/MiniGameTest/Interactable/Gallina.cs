using System;
using System.Collections;
using GeneralsMiniGames;
using iText.Layout;
using MiniGameTest.Challenges;
using TMPro;
using UnityEngine;

namespace MiniGameTest.Interactable
{

    [Serializable]
    public enum GallinaType
    {
        Blanca,
        BlancaManchas,
        Marron,
        Negra,
        otros,
    }

    [Serializable]
    public class GallinaConfig
    {
        public enum GallinaState { Aleteando, Caminando, Sentada,  None }

        
        public GameObject gallinaAleteando;
        public GameObject gallinaCaminando;
        public GameObject gallinaSentada;
        public GameObject gallinaPlumas;

        private GallinaState _currentState = GallinaState.Caminando;
        
        public void ShowState(GallinaState newState)
        {
            
            GetGameObject(_currentState).SetActive(false);
            GetGameObject(newState)?.SetActive(true);
            _currentState = newState;
        }
        
        public void ShowPlumas(bool state)
        {
            gallinaPlumas.gameObject.SetActive(state);
        }

        private GameObject GetGameObject(GallinaState gallinaState)
        {
            switch (gallinaState)
            {
                case GallinaState.Aleteando:
                    return gallinaAleteando;
                case GallinaState.Caminando:
                    return gallinaCaminando;
                case GallinaState.Sentada:
                    return gallinaSentada;
                case GallinaState.None:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gallinaState), gallinaState, null);
            }
        }
        
    }

    public class Gallina : InteractableBase
    {
        public GallinaType type;
        public GallinaConfig gallinaConfig;
        private Challenge01 _challenge01;
        
        private bool _canTouch;

        protected override void Awake()
        {
            
            
            base.Awake();
            _canTouch = true;
            gallinaConfig.ShowPlumas(false);
            gallinaConfig.ShowState(GallinaConfig.GallinaState.Caminando);
        }

        private void Start()
        {
           
  
            //StartCoroutine(ActivateAndDeactivateAtrapar());
            _challenge01 = SelectChallenge.Instance.RefChallengeSetupSelected as Challenge01;
            Vector3 gallinaPosition = gallinaConfig.gallinaCaminando.transform.position;
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(gallinaPosition);
        


        }
        IEnumerator ActivateAndDeactivateAtrapar()
        {
            GameObject atrapar = GameObject.FindGameObjectWithTag("Atrapar");

            if (atrapar != null)
            {
                
                atrapar.SetActive(true);

                
                yield return new WaitForSeconds(5f);

                
                atrapar.SetActive(false);
            }
            else
            {
                Debug.LogError("No se encontró ningún GameObject con la etiqueta 'Atrapar'.");
            }
        }

        private void MoveCursorToGallina(Vector3 gallinaPosition)
        {
            Vector3 gallinaScreenPosition = Camera.main.WorldToScreenPoint(gallinaPosition);
            transform.position = gallinaScreenPosition;
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
            if (!_canTouch) return;

            ChallengeSetup currentChallenge = SelectChallenge.Instance.RefChallengeSetupSelected;
            if (currentChallenge is Challenge01 challenge)
            {
                if (challenge.CurrentChallange.gallinaType == type)
                {
                    _challenge01.GallinasQuantity++;
                    StartCoroutine(KillGallina());
                }
                else
                {
                    AttemptsCounter.Instance.AddAttempt();
                }
            }

        }

        protected override void TabSobreAlgunaZonaActivaDeEsteObjeto(string argNombreZonaActivaSobreLaQueSeHizoTab)
        {

        }

        protected override void EsteObjetoSeEstaTransladandoMientrasSeSelecciona()
        {

        }

        private IEnumerator KillGallina()
        {
            

            _canTouch = false;

            gallinaConfig.ShowState(GallinaConfig.GallinaState.Aleteando);

            yield return new WaitForSeconds(2f);

            gallinaConfig.ShowState(GallinaConfig.GallinaState.None);
            gallinaConfig.ShowPlumas(true);

          
            string gallinasText = (_challenge01.GallinasQuantity).ToString();

            GameObject contadorPlumaObj = gallinaConfig.gallinaPlumas.transform.Find("ContadorPluma").gameObject;
       
            Vector3 startPosition = contadorPlumaObj.transform.position;
            GameObject tableroContador = GameObject.FindGameObjectWithTag("Board");
            GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
         
            Vector3 endPosition = tableroContador.transform.position;

            TextMeshPro textMeshPro = contadorPlumaObj.GetComponent<TextMeshPro>();
            if (contadorPlumaObj != null)
            {

                if (textMeshPro != null)
                {
                    textMeshPro.text = (_challenge01.GallinasQuantity).ToString();
                    CambiarTextoEnDatosTablero();
                }
                else
                {
                    Debug.LogError("No se encontró el componente TextMeshPro dentro de 'contador pluma'.");
                }

            }

            
            GameObject flyingNumberGO = new GameObject("FlyingNumber");
            TextMeshProUGUI flyingNumberText = flyingNumberGO.AddComponent<TextMeshProUGUI>();
            flyingNumberText.text = gallinasText;
            flyingNumberText.fontSize =69;
            flyingNumberText.color = textMeshPro.color;
            flyingNumberText.transform.SetParent(canvas.transform);

           
            Vector3 startOffset = new Vector3(0, 0, 0); 
            Vector3 endOffset = new Vector3(0, 100, 0); 
            float moveDuration = 1.5f; 

            
            StartCoroutine(MoveNumberToCounter(flyingNumberText, startPosition + startOffset, endPosition + endOffset, moveDuration));

          
            yield return new WaitForSeconds(1f);

           
            gallinaConfig.ShowPlumas(false);

        
            
            Destroy(flyingNumberGO, 2f);
        }
        private void CambiarTextoEnDatosTablero()
        {
            GameObject tableroContador = GameObject.FindGameObjectWithTag("Board");

            try
            {
                TextMeshProUGUI textMeshPro = tableroContador.GetComponent<TextMeshProUGUI>();
                if (textMeshPro != null)
                {
                    textMeshPro.text = (_challenge01.GallinasQuantity).ToString();
                }
                else
                {
                    Debug.LogError("El objeto 'datosTableroContador' no tiene el componente TextMeshPro.");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Se produjo una excepción: {ex}");
            }
        }
        private IEnumerator MoveNumberToCounter(TextMeshProUGUI numberText, Vector3 startPosition, Vector3 endPosition, float duration)
        {
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                numberText.transform.position = Vector3.Lerp(startPosition, endPosition, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            numberText.transform.position = endPosition;
        }


    }
}