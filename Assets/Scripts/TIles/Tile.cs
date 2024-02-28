using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

namespace CrazyRubberProject
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private GameObject[] decorationAreas;
        [SerializeField] private GameObject[] floorArea;
        [SerializeField] private GameObject[] decorationAssets;
        [SerializeField] private GameObject[] floorDecorationAssets;
        [SerializeField] private GameObject[] obstacleAnchors;
        [SerializeField] private TileObject[] obstacles;
        [SerializeField] private TileObject[] collectibles;
        [SerializeField] private TileObject[] powerUps;
        [SerializeField] public GameObject anchorPoint;
        [SerializeField] private int numberOfProps;

        private TileManager tileManager;
        private List<List<GameObject>> obstacleAnchorPoints;
        private List<List<bool>> occupiedPositions;
        private int gemAmount;
        private int powerUpAmount;
        private int[] powerUpAmountChoices = new int[] { 0, 0, 0, 0, 0, 1 };
        private int columnAmount = 4;
        private int rowAmount = 8;

        public delegate void PlayerEntered(Tile myTile);
        public static event PlayerEntered onPlayerEntered;

        private void OnEnable()
        {
            obstacleAnchorPoints = new List<List<GameObject>>();
            occupiedPositions = new List<List<bool>>();
            tileManager = FindObjectOfType<TileManager>();
            gemAmount = Random.Range(1, 5);
            powerUpAmount = powerUpAmountChoices[Random.Range(0, powerUpAmountChoices.Length)];

            ObtainAnchors();
            DecorateTile(decorationAssets, decorationAreas);
            //DecorateTile(floorDecorationAssets, floorArea);
            SetObjects(obstacles, tileManager.difficultyLevel);
            SetObjects(collectibles, gemAmount);
            if (powerUpAmount > 0)
            {
                SetObjects(powerUps, powerUpAmount);
            }
        }

        private void DecorateTile(GameObject[]myDecoAssets, GameObject[]myDecoAreas)
        {
            for (int i = 0; i < numberOfProps; i++)
            {
                GameObject selectedArea = myDecoAreas[Random.Range(0, myDecoAreas.Length)];
                GameObject selectedAsset = myDecoAssets[Random.Range(0, myDecoAssets.Length)];

                GameObject newDecoration = Instantiate(selectedAsset, GetRandomPointOnPlane(selectedArea), Quaternion.identity);
                newDecoration.transform.parent = selectedArea.transform;
            }
        }

        private void SetObjects(TileObject[] objects, int amount)
        {

            for (int i = 0; i < amount; i++)
            {
                TileObject selectedObj = objects[Random.Range(0, objects.Length)];

                List<int> abailablePos = AbailablePositionCheck(selectedObj);

                int randomCol;
                int randomRow;
                int maxAttemps = 10;
                int currentAttemps = 0;
                do
                {
                    int randomColNum = Random.Range(0, abailablePos.Count);
                    randomCol = abailablePos[randomColNum];
                    randomRow = Random.Range(0, 8);
                    currentAttemps++;

                } while (PosAbailable(selectedObj, randomRow, randomCol) && currentAttemps<maxAttemps);
                

                PositionHandler(selectedObj, randomRow, randomCol);

                TileObject newObj = Instantiate(selectedObj, obstacleAnchorPoints[randomCol][randomRow].transform.position, RotateObj(selectedObj, randomCol));
                newObj.transform.parent = obstacleAnchorPoints[randomCol][randomRow].transform;
            }
        }


        private Quaternion RotateObj(TileObject obstacle, int randomCol)
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
                Debug.Log(other.name);
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

        private List<int> AbailablePositionCheck(TileObject myObstacle)
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

        private void PositionHandler(TileObject obstacle, int randomRow, int randomCol)
        {
            occupiedPositions[randomCol][randomRow] = true;

            if (obstacle.size == 1)
            {

                if (randomCol != 0)
                {
                    occupiedPositions[randomCol - 1][randomRow] = true;
                }
                if (randomCol != columnAmount)
                {
                    occupiedPositions[randomCol + 1][randomRow] = true;
                }
                if (randomRow != 0)
                {
                    occupiedPositions[randomCol][randomRow - 1] = true;
                }
                if (randomRow != rowAmount)
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
                if (randomRow != rowAmount)
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
                if (randomRow != rowAmount)
                {
                    for (int i = 0; i <= 4; i++)
                    {
                        occupiedPositions[i][randomRow + 1] = true;
                    }
                }
            }
        }

        private bool PosAbailable(TileObject obstacle, int randomRow, int randomCol)
        {
            if (occupiedPositions[randomCol][randomRow]) { return true; }

            if (obstacle.size == 1)
            {

                if (randomCol != 0)
                {
                    if (occupiedPositions[randomCol - 1][randomRow]) { return true; }
                }
                if (randomCol != columnAmount)
                {
                    if (occupiedPositions[randomCol + 1][randomRow]) { return true; }
                }
                if (randomRow != 0)
                {
                    if (occupiedPositions[randomCol][randomRow - 1]) { return true; }
                }
                if (randomRow != rowAmount)
                {
                    if (occupiedPositions[randomCol][randomRow + 1]) { return true; }
                }

            }
            else if (obstacle.size == 2)
            {
                for (int i = 0; i <= columnAmount; i++)
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
                if (randomRow != rowAmount)
                {
                    if (occupiedPositions[randomCol][randomRow + 1]) { return true; }
                    if (occupiedPositions[randomCol][randomRow + 1]) { return true; }
                    if (occupiedPositions[randomCol - 1][randomRow + 1]) { return true; }
                }

            }
            else
            {

                for (int i = 0; i <= columnAmount; i++)
                {
                    if (occupiedPositions[i][randomRow] == true)
                    {
                        return true;
                    }
                }

                if (randomRow != 0)
                {
                    for (int i = 0; i <= columnAmount; i++)
                    {
                        if (occupiedPositions[i][randomRow-1] == true)
                        {
                            return true;
                        }
                    }

                }
                if (randomRow != 8)
                {
                    for (int i = 0; i <= columnAmount; i++)
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