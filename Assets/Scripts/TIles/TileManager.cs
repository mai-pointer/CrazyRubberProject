using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrazyRubberProject
{
    public class TileManager : MonoBehaviour
    {
        [SerializeField] private Tile[] tiles;
        [SerializeField] private int numberOfTiles;
        [SerializeField] private GameObject tileContainer;
        [SerializeField] private AnimationCurve speedCurve;
        [SerializeField] private float maxSpeed;
        [SerializeField] private float duration;

        private int tileLength = 10;
        private int tileIndex;
        private float currentSpeed;
        private List<Tile> currentTiles;

        
        private float elapsedTime = 0f;

        public int difficultyLevel { get;  private set; }

        private void Awake()
        {
            difficultyLevel = 0;
            tileIndex = -2;  
            currentTiles = new List<Tile>();
            Tile.onPlayerEntered += UpdateTile;
        }
        void Start()
        {
            for (int i = 0; i < numberOfTiles; i++)
            {
                SpawnTile();
                tileIndex++;
            }

            StartCoroutine(TilesMovement());
        }

        private void SpawnTile()
        {
            Tile newTile = Instantiate(tiles[Random.Range(0, tiles.Length)]);
            newTile.name = tileIndex.ToString();
            newTile.transform.SetParent(tileContainer.transform);
            newTile.transform.localPosition = new Vector3(0, 0, -(tileIndex * tileLength));
            newTile.transform.rotation = new Quaternion(0, 0, 0, 0);
            currentTiles.Add(newTile);
            DifficultyManager();
        }

        private void RemoveTile()
        {
            Tile tileToDelete = currentTiles[0];
            currentTiles.RemoveAt(0);
            Destroy(tileToDelete.gameObject);
        }

        private void UpdateTile(Tile myTile)
        {
            SpawnTile();
            RemoveTile();
            tileIndex++;
        }


        IEnumerator TilesMovement()
        {
            while (elapsedTime < duration)
            {
                // Evaluar el valor de la curva de animación en función del tiempo transcurrido
                float curveValue = speedCurve.Evaluate(elapsedTime / duration);

                // Calcular la velocidad actual utilizando el valor de la curva
                currentSpeed = curveValue * maxSpeed;

                Vector3 newPosition = tileContainer.transform.position;
                newPosition.z += currentSpeed * Time.deltaTime;
                tileContainer.transform.position = newPosition;

                // Incrementar el tiempo transcurrido
                elapsedTime += Time.deltaTime;

                yield return null;
            }

            // Asegurarse de que la velocidad máxima se alcanza exactamente al final de la animación
            transform.Translate(Vector3.forward * maxSpeed * Time.deltaTime);
        }

        private void OnDestroy()
        {
            Tile.onPlayerEntered -= UpdateTile;
        }

        private void DifficultyManager()
        {
            switch (tileIndex)
            {
                case int n when n >= -2 && n <= 3:
                    difficultyLevel = 0;
                    break;
                case int n when n >= 4 && n <= 10:
                    difficultyLevel = 1;
                    break;
                case int n when n >= 11 && n <= 50:
                    difficultyLevel = 2;
                    break;
                case int n when n >= 51 && n <= 100:
                    difficultyLevel = 3;
                    break;
                default:
                    difficultyLevel = 4;
                    break;

            }
        }
    }
}