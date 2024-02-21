using UnityEngine;

namespace CrazyRubberProject
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private GameObject[] decorationAreas;
        [SerializeField] private GameObject[] decorationAssets;
        [SerializeField] public GameObject anchorPoint;
        [SerializeField] private int numberOfProps;

        public delegate void PlayerEntered(Tile myTile);
        public static event PlayerEntered onPlayerEntered;

        private void OnEnable()
        {
            DecorateTile();
        }

        private void DecorateTile()
        {
            for (int i = 0; i < numberOfProps; i++)
            {
                // Elegir aleatoriamente un �rea de decoraci�n y un activo de decoraci�n
                GameObject selectedArea = decorationAreas[Random.Range(0, decorationAreas.Length)];
                GameObject selectedAsset = decorationAssets[Random.Range(0, decorationAssets.Length)];

                // Crear una instancia del activo de decoraci�n en la posici�n aleatoria
                GameObject newDecoration = Instantiate(selectedAsset, GetRandomPointOnPlane(selectedArea), Quaternion.identity);
                // Asegurarse de que el activo de decoraci�n est� dentro del �rea de decoraci�n
                newDecoration.transform.parent = selectedArea.transform;
            }
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

    }
}