using Singleton;
using UnityEngine;

public class CameraShake : Singleton<CameraShake>
{
   [SerializeField]
   private float duracionSacudida = 0.5f; 
   [SerializeField]
   private float intensidad = 0.1f; 
   private Transform camaraTransform;
   private Vector3 posicionInicial;
   private float tiempoInicioSacudida;
   private bool sacudiendo = false;

   private void Start()
   {
      camaraTransform = Camera.main.transform; 
      posicionInicial = camaraTransform.localPosition; 
   }

   private void Update()
   {
      if (sacudiendo)
      {
         if (Time.time - tiempoInicioSacudida < duracionSacudida)
         {
            Vector3 sacudida = Random.insideUnitSphere * intensidad;
            camaraTransform.localPosition = posicionInicial + sacudida;
         }
         else
         {
            camaraTransform.localPosition = posicionInicial;
            sacudiendo = false;
         }
      }
   }

   // Función para iniciar la sacudida desde el exterior
   public void ShakeCamera()
   {
      tiempoInicioSacudida = Time.time;
      sacudiendo = true;
   }
}
