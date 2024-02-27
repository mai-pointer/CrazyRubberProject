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
            if (isAnimated)
            {
            //    Debug.Log($"Local scale: {transform.localScale}, Global scale: {transform.lossyScale}");
            //    GetComponent<Animator>().enabled = true;
            //    Debug.Log($"Local scale: {transform.localScale}, Global scale: {transform.lossyScale}");
            //    transform.localScale = Vector3.one;
            //    Debug.Log($"Local scale: {transform.localScale}, Global scale: {transform.lossyScale}");
            }
        }

        
    }

}

    
