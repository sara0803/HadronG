#pragma warning disable
using System;
using UnityEngine;

namespace NSManagerAumentos
{
    [Serializable]
    public class GameObjectParaOcultar
    {
        [SerializeField] private GameObject gameObjectParaOcultar;

        [SerializeField] private GameObject[] arrayAumentosQueSonExceptionYNoOcultan;

        private bool estadoAnteriorEnabledDelGameObject;

        public void Ocultar(GameObject argAumentoActualmenteMostrado)
        {
            estadoAnteriorEnabledDelGameObject = gameObjectParaOcultar.activeSelf;

            foreach (var tmpAumentoException in arrayAumentosQueSonExceptionYNoOcultan)
                if (tmpAumentoException == argAumentoActualmenteMostrado)
                    return;

            gameObjectParaOcultar.SetActive(false);
        }

        public void Mostrar()
        {
            gameObjectParaOcultar.SetActive(estadoAnteriorEnabledDelGameObject);
        }
    }
}