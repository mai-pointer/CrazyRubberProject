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

        private Tile currentTile;

        private void Awake()
        {
            tileIndex = 0;  
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
    }
}