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
        
    }

}

    
