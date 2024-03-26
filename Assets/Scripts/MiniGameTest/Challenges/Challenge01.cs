using System;
using System.Collections.Generic;
using GeneralsMiniGames;
using MiniGameTest.Interactable;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace MiniGameTest.Challenges
{
    [Serializable]
    public enum SpawnZoneType
    {
        Lago,
        Arbol,
        Pasto,
    }

    [Serializable]
    public class SpawnZoneKey
    {
        public PolygonCollider2D polygonCollider;
        public SpawnZoneType type;
    }
    
    [Serializable]
    public class SpawnZoneBounds
    {
        public SpawnZoneKey[] spawnZoneKeys;
        public float minDistance = 1.0f;

        private List<Vector2> spawnedPoints = new List<Vector2>();
        
        public GameObject SpawnObject(GameObject objectToSpawn, SpawnZoneType type)
        {
            var polygonCollider = GetPolygonColliderByType(type);
            Vector2 point = GetRandomPointInCollider(polygonCollider);
            return Object.Instantiate(objectToSpawn, point, Quaternion.identity);
        }

        private Vector2 GetRandomPointInCollider(PolygonCollider2D polygonCollider)
        {
            Bounds bounds = polygonCollider.bounds;
            Vector2 point = new Vector2(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y)
            );

            while (!polygonCollider.OverlapPoint(point) || IsPointTooClose(point))
            {
                point = new Vector2(
                    Random.Range(bounds.min.x, bounds.max.x),
                    Random.Range(bounds.min.y, bounds.max.y)
                );
            }
            
            spawnedPoints.Add(point);

            return point;
        }
        
        private PolygonCollider2D GetPolygonColliderByType(SpawnZoneType type)
        {
            foreach (SpawnZoneKey spawnZoneKey in spawnZoneKeys)
            {
                if (spawnZoneKey.type == type)
                {
                    return spawnZoneKey.polygonCollider;
                }
            }

            throw new Exception($"No existe una zona de spawn para el tipo {type}");
        }
        
        private bool IsPointTooClose(Vector2 point)
        {
            foreach (Vector2 existingPoint in spawnedPoints)
            {
                if (Vector2.Distance(existingPoint, point) < minDistance)
                {
                    return true;
                }
            }

            return false;
        }
    }
    
    [Serializable]
    public class GallinasConfiguration
    {
        public int time;
        public GallinaType gallinaType;
        public int gallinasAmount;
        public int distractoresAmount;
    }
    
    public class Challenge01 : ChallengeSetup
    {
        [Header("Configuracion")]
        public PanelCronometro cronometro;
        public Galpon galpon;
        public GallinasConfiguration[] gallinasConfigurations;

        [Header("Configuracion Zonas")]
        public SpawnZoneBounds spawnZoneBounds;

        [Header("Coleccion Interactuables")]
        public Gallina[] gallinas;
        public Distractores[] distractores;
        
        public GallinasConfiguration CurrentChallange => gallinasConfigurations[_currentChallenge];

        public int GallinasQuantity { get; set; }

        private int _currentChallenge;
        private List<GameObject> _distractoresSpawned = new List<GameObject>();
        private List<GameObject> _gallinasSpawned = new List<GameObject>();
        
        private void Start()
        {
            cronometro.gameObject.SetActive(false);
        }

        public override void SetupAndStartChallengeInScene()
        {
            cronometro.gameObject.SetActive(true);
            GallinasConfiguration configuration = gallinasConfigurations[_currentChallenge];
            cronometro.timeToCompleteChallenge = configuration.time;
            
            galpon.ShowGallina(configuration.gallinaType);
            SetUpGallinas();
            SetUpDistractores();
        }

        public override void ResetChallenge()
        {
            cronometro.gameObject.SetActive(true);
            RemoveGallinas();
            RemoveDistractores();
        }

        public override bool GetIfCurrentChallengeLevelAvailable()
        {
            return _currentChallenge < gallinasConfigurations.Length;
        }

        public override void PassToNextChallengeLevelAvaible()
        {
            _currentChallenge++;
        }

        #region SetUpInteractables

        private void SetUpGallinas()
        {
            GallinasConfiguration configuration = gallinasConfigurations[_currentChallenge];
            
            for (int i = 0; i < configuration.gallinasAmount; i++)
            {
                Gallina gallina = GetGallinaByType(configuration.gallinaType);
                GameObject gallinaSpawned =  spawnZoneBounds.SpawnObject(gallina.gameObject, gallina.spawnZoneType);
                _gallinasSpawned.Add(gallinaSpawned);
            }
        }
        
        private void SetUpDistractores()
        {
            GallinasConfiguration configuration = gallinasConfigurations[_currentChallenge];
            for (int i = 0; i < configuration.distractoresAmount; i++)
            {
                int randomIndex = Random.Range(0, distractores.Length);
                Distractores distractor = distractores[randomIndex];
                GameObject distractorSpawned =
                    spawnZoneBounds.SpawnObject(distractor.gameObject, distractor.spawnZoneType);
                _distractoresSpawned.Add(distractorSpawned);
            }

            for (int i = 0; i < configuration.distractoresAmount / 2; i++)
            {
                while (true)
                {
                    int randomGallina = Random.Range(0, gallinas.Length);
                    Gallina gallinaRandom = gallinas[randomGallina];
                    if (gallinaRandom.type != configuration.gallinaType)
                    {
                        Gallina gallina = GetGallinaByType(gallinaRandom.type);
                        GameObject gallinaSpawned =  spawnZoneBounds.SpawnObject(gallina.gameObject, gallina.spawnZoneType);
                        _gallinasSpawned.Add(gallinaSpawned);
                        break;
                    }
                }
                
            }
            
        }

        private Gallina GetGallinaByType(GallinaType type)
        {
            foreach (Gallina gallina in gallinas)
            {
                if (gallina.type == type)
                {
                    return gallina;
                }
            }

            throw new Exception($"No existe la referencia a la gallina de tipo {type}");
        }
        
        #endregion
        
        #region Kill Entities

        private void RemoveGallinas()
        {
            foreach (GameObject gallina in _gallinasSpawned)
            {
                Destroy(gallina.gameObject);
            }
        }

        private void RemoveDistractores()
        {
            foreach (GameObject distractor in _distractoresSpawned)
            {
                Destroy(distractor.gameObject);
            }
        }
        #endregion
        
    }
}