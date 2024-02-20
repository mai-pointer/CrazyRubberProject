using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace CrazyRubberProject
{
    public class TileManager : MonoBehaviour
    {
        [SerializeField] private Tile[] tiles;
        [SerializeField] private int numberOfTiles;
        [SerializeField] private GameObject tileContainer;

        private int tileLength = 10;
        private int tileIndex = 0;
        private int speed = 3;
        private bool hasSpawn = false;

        private Tile currentTile;

        private void Awake()
        {
            Tile.onPlayerEntered += SelectCurrentTile;
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

        // Update is called once per frame
        void Update()
        {
            if (CheckPlayerProximity() && !hasSpawn)
            {
                SpawnTile(tileIndex);
                tileIndex++;
            }
        }

        private void SpawnTile(int newTileIndex)
        {
            Tile newTile = Instantiate(tiles[Random.Range(0, tiles.Length)]);
            newTile.transform.position = new Vector3(0, 0, -(newTileIndex * tileLength));
            newTile.transform.SetParent(tileContainer.transform);

        }

        private void SelectCurrentTile(Tile myTile)
        {
            currentTile = myTile;
            hasSpawn = false;
        }

        private bool CheckPlayerProximity()
        {        
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (currentTile != null && player != null)
            {
                float playerPos = player.transform.position.z;
                float tileEndPoint = currentTile.anchorPoint.transform.position.z;

                if (tileEndPoint - playerPos < 1)
                {
                    hasSpawn = true;    
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
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
            Tile.onPlayerEntered -= SelectCurrentTile;
        }
    }
}