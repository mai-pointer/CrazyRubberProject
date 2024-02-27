using System.Collections;
using UnityEngine;

namespace CrazyRubberProject
{
    public class TileObject : MonoBehaviour
    {
        [SerializeField] public bool[] availablePositions;
        [SerializeField] public bool canRotate;
        [SerializeField] public bool isAnimated;
        [SerializeField] public int size;
        [SerializeField] public int value;

        private TileManager tileManager;
        private float rotationSpeed = 100f;
        private float desiredDistance;
        private Vector3 initialPosition;
        private float moveSpeed;
        private Transform targetToFollow;

        private void OnEnable()
        {
            //if (isAnimated)
            //{
            //    GameObject gemContainer = GameObject.Find("GemContainer");
            //    transform.parent = gemContainer.transform.parent;
            //    tileManager = FindObjectOfType<TileManager>();
            //    targetToFollow = GameObject.Find("TileContainer").transform;
            //    desiredDistance = targetToFollow.position.z - transform.position.z;
            //    initialPosition = transform.position - targetToFollow.position;
            //    StartCoroutine(AnimateGems());
            //}
        }

        IEnumerator AnimateGems()
        {            

            while (true)
            {
                moveSpeed = tileManager.currentSpeed;
                transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.Self);

                Vector3 newPos = targetToFollow.position + initialPosition;
                transform.position = Vector3.MoveTowards(transform.position, newPos, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }
    }

}

    
