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

        private int tileLength = 10;
        private int tileIndex;
        private int speed = 3;
        private List<Tile> currentTiles;

        public int difficultyLevel { get;  private set; }

        private Tile currentTile;

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
                SpawnTile(tileIndex);
                tileIndex++;
            }

            StartCoroutine(TilesMovement());
        }

        private void SpawnTile(int newTileIndex)
        {
            Tile newTile = Instantiate(tiles[Random.Range(0, tiles.Length)]);
            newTile.name = newTileIndex.ToString();
            newTile.transform.SetParent(tileContainer.transform);
            newTile.transform.localPosition = new Vector3(0, 0, -(newTileIndex * tileLength));
            newTile.transform.rotation = new Quaternion(0, 0, 0, 0);
            currentTiles.Add(newTile);
        }

        private void RemoveTile()
        {
            Tile tileToDelete = currentTiles[0];
            currentTiles.RemoveAt(0);
            Destroy(tileToDelete.gameObject);
        }

        private void UpdateTile(Tile myTile)
        {
            SpawnTile(tileIndex);
            RemoveTile();
            tileIndex++;
        }


        IEnumerator TilesMovement()
        {
            while (true)
            {
                MoveTiles();
                yield return null;
            }
        }

        void MoveTiles()
        {
            Vector3 newPosition = tileContainer.transform.position;
            newPosition.z += speed * Time.deltaTime;
            tileContainer.transform.position = newPosition;
        }

        private void OnDestroy()
        {
            Tile.onPlayerEntered -= UpdateTile;
        }

        private void DifficultyManager()
        {
            switch (tileIndex)
            {
                case int n when n >= 0 && n <= 1:
                    difficultyLevel = 0;
                    break;
                case int n when n >= 2 && n <= 10:
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