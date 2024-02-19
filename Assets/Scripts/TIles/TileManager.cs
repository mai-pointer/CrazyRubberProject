using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField] private Tile[] tiles;
    [SerializeField] private int numberOfTiles;
    [SerializeField] private GameObject tileCointainer;

    private int tileLength = 10;
    private int tileIndex = 0;
    private Transform rubberPos;

    private Tile currentTile;
    void Start()
    {
        for (int i = 0; i< numberOfTiles; i++)
        {
            SpawnTile();
            tileIndex++;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnTile()
    {
        Tile newTile = Instantiate(tiles[Random.Range(0, tiles.Length)]);

        newTile.transform.position = new Vector3(0, 0, tileIndex * tileLength);

    }
}
