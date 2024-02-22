using UnityEngine;
using static CrazyRubberProject.Tile;

namespace CrazyRubberProject
{
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] public bool[] availablePositions;
        [SerializeField] public bool canRotate;
        [SerializeField] public int size;


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("HAS MUERTO");
            }
        }
    }
}
