using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEditor.Progress;

namespace CrazyRubberProject
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private GameObject[] decorationAreas;
        [SerializeField] private GameObject[] decorationAssets;
        [SerializeField] private GameObject[] obstacleAnchors;
        [SerializeField] private Obstacle[] obstacles;
        [SerializeField] public GameObject anchorPoint;
        [SerializeField] private int numberOfProps;

        private TileManager tileManager;
        private List<List<GameObject>> obstacleAnchorPoints = new List<List<GameObject>>();
        private List<List<bool>> occupiedPositions = new List<List<bool>>();

        public delegate void PlayerEntered(Tile myTile);
        public static event PlayerEntered onPlayerEntered;

        private void OnEnable()
        {
            tileManager = FindObjectOfType<TileManager>();
            ObtainAnchors();
            DecorateTile();
            SetObstacles();
        }

        private void DecorateTile()
        {
            for (int i = 0; i < numberOfProps; i++)
            {
                // Elegir aleatoriamente un área de decoración y un activo de decoración
                GameObject selectedArea = decorationAreas[Random.Range(0, decorationAreas.Length)];
                GameObject selectedAsset = decorationAssets[Random.Range(0, decorationAssets.Length)];

                // Crear una instancia del activo de decoración en la posición aleatoria
                GameObject newDecoration = Instantiate(selectedAsset, GetRandomPointOnPlane(selectedArea), Quaternion.identity);
                // Asegurarse de que el activo de decoración esté dentro del área de decoración
                newDecoration.transform.parent = selectedArea.transform;
            }
        }

        private void SetObstacles()
        {

            for (int i = 0; i < tileManager.difficultyLevel; i++)
            {
                Obstacle selectedObstacle = obstacles[Random.Range(0, obstacles.Length)];

                List<int> abailablePos = AbailablePositionCheck(selectedObstacle);

                int randomCol;
                int randomRow;

                do
                {
                    int randomColNum = Random.Range(0, abailablePos.Count);
                    randomCol = abailablePos[randomColNum];
                    randomRow = Random.Range(0, 8);

                } while (PosAbailable(selectedObstacle, randomRow, randomCol));
                

                PositionHandler(selectedObstacle, randomRow, randomCol);

                Obstacle newObstacle = Instantiate(selectedObstacle, obstacleAnchorPoints[randomCol][randomRow].transform.position, RotateObstacles(selectedObstacle, randomCol));
                newObstacle.transform.parent = obstacleAnchorPoints[randomCol][randomRow].transform.parent;
            }
        }

        private Quaternion RotateObstacles(Obstacle obstacle, int randomCol)
        {
            Quaternion rotation = Quaternion.Euler(0, 180, 0);
            Quaternion noRotation = Quaternion.Euler(0, 0, 0);

            Quaternion[] rotations = { rotation, noRotation };
            Quaternion selectedRotation = Quaternion.identity;
            if (!obstacle.canRotate)
            {
                if (randomCol == 4)
                {
                    selectedRotation = rotation;
                }
            }
            else
            {
                selectedRotation = rotations[Random.Range(0, rotations.Length)];
            }

            return selectedRotation;
        }

        private Vector3 GetRandomPointOnPlane(GameObject plane)
        {
            var planeBounds = plane.GetComponent<Renderer>().bounds;

            float randomX = Random.Range(planeBounds.min.x, planeBounds.max.x);
            float randomZ = Random.Range(planeBounds.min.z, planeBounds.max.z);

            return new Vector3(randomX, plane.transform.position.y, randomZ);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                onPlayerEntered?.Invoke(this);
            }
        }

        private void ObtainAnchors()
        {
            for (int i = 0; i < obstacleAnchors.Length; i++)
            {
                List<GameObject> myList = new List<GameObject>();
                List<bool> myPosList = new List<bool>();

                for (int j = 0; j < obstacleAnchors[i].transform.childCount; j++)
                {
                    Transform childTransform = obstacleAnchors[i].transform.GetChild(j);
                    GameObject childGameObject = childTransform.gameObject;

                    myList.Add(childGameObject);
                    myPosList.Add(false);
                }

                obstacleAnchorPoints.Add(myList);
                occupiedPositions.Add(myPosList);
            }

        }

        private List<int> AbailablePositionCheck(Obstacle myObstacle)
        {
            List<int> myPositions = new List<int>();
            for (int i = 0; i < myObstacle.availablePositions.Length; i++)
            {
                if (myObstacle.availablePositions[i] == true)
                {
                    myPositions.Add(i);
                }
            }

            return myPositions;
        }

        private void PositionHandler(Obstacle obstacle, int randomRow, int randomCol)
        {
            occupiedPositions[randomCol][randomRow] = true;

            if (obstacle.size == 1)
            {

                if (randomCol != 0)
                {
                    occupiedPositions[randomCol - 1][randomRow] = true;
                }
                if (randomCol != 4)
                {
                    occupiedPositions[randomCol + 1][randomRow] = true;
                }
                if (randomRow != 0)
                {
                    occupiedPositions[randomCol][randomRow - 1] = true;
                }
                if (randomRow != 8)
                {
                    occupiedPositions[randomCol][randomRow + 1] = true;
                }

            }
            else if (obstacle.size == 2)
            {
                for (int i = 0; i <= 4; i++)
                {
                    occupiedPositions[i][randomRow] = true;
                }

                if (randomRow != 0)
                {
                    occupiedPositions[randomCol][randomRow - 1] = true;
                    occupiedPositions[randomCol + 1][randomRow - 1] = true;
                    occupiedPositions[randomCol - 1][randomRow - 1] = true;

                }
                if (randomRow != 8)
                {
                    occupiedPositions[randomCol][randomRow + 1] = true;
                    occupiedPositions[randomCol + 1][randomRow + 1] = true;
                    occupiedPositions[randomCol - 1][randomRow + 1] = true;
                }

            }
            else
            {

                for (int i = 0; i <= 4; i++)
                {
                    occupiedPositions[i][randomRow] = true;
                }

                if (randomRow != 0)
                {
                    for (int i = 0; i <= 4; i++)
                    {
                        occupiedPositions[i][randomRow - 1] = true;
                    }

                }
                if (randomRow != 8)
                {
                    for (int i = 0; i <= 4; i++)
                    {
                        occupiedPositions[i][randomRow + 1] = true;
                    }
                }
            }
        }

        private bool PosAbailable(Obstacle obstacle, int randomRow, int randomCol)
        {
            foreach (List<bool> list in occupiedPositions)
            {
                Debug.Log("Lsta: " + list.Count);
            }

            if (occupiedPositions[randomCol][randomRow]) { return true; }

            if (obstacle.size == 1)
            {

                if (randomCol != 0)
                {
                    if (occupiedPositions[randomCol - 1][randomRow]) { return true; }
                }
                if (randomCol != 4)
                {
                    if (occupiedPositions[randomCol + 1][randomRow]) { return true; }
                }
                if (randomRow != 0)
                {
                    if (occupiedPositions[randomCol][randomRow - 1]) { return true; }
                }
                if (randomRow != 8)
                {
                    if (occupiedPositions[randomCol][randomRow + 1]) { return true; }
                }

            }
            else if (obstacle.size == 2)
            {
                for (int i = 0; i <= 4; i++)
                {
                    if (occupiedPositions[i][randomRow] == true)
                    {
                        return true;
                    }
                }

                if (randomRow != 0)
                {
                    if (occupiedPositions[randomCol][randomRow - 1]) { return true; }
                    if (occupiedPositions[randomCol][randomRow - 1]) { return true; }
                    if (occupiedPositions[randomCol - 1][randomRow - 1]) { return true; }

                }
                if (randomRow != 8)
                {
                    if (occupiedPositions[randomCol][randomRow + 1]) { return true; }
                    if (occupiedPositions[randomCol][randomRow + 1]) { return true; }
                    if (occupiedPositions[randomCol - 1][randomRow + 1]) { return true; }
                }

            }
            else
            {

                for (int i = 0; i <= 4; i++)
                {
                    if (occupiedPositions[i][randomRow] == true)
                    {
                        return true;
                    }
                }

                if (randomRow != 0)
                {
                    for (int i = 0; i <= 4; i++)
                    {
                        if (occupiedPositions[i][randomRow-1] == true)
                        {
                            return true;
                        }
                    }

                }
                if (randomRow != 8)
                {
                    for (int i = 0; i <= 4; i++)
                    {
                        if (occupiedPositions[i][randomRow+1] == true)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;

        }

    }
}