using UnityEngine;

namespace CrazyRubberProject
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private GameObject[] decorationAreas;
        [SerializeField] private GameObject[] decorationAssets;
        [SerializeField] public GameObject anchorPoint;
        [SerializeField] private GameObject decoContainer;
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
                // Elegir aleatoriamente un área de decoración y un activo de decoración
                GameObject selectedArea = decorationAreas[Random.Range(0, decorationAreas.Length)];
                GameObject selectedAsset = decorationAssets[Random.Range(0, decorationAssets.Length)];

                // Obtener una posición aleatoria dentro del área de decoración
                Vector3 randomPosition = selectedArea.transform.position + new Vector3(Random.Range(-selectedArea.transform.localScale.x / 2f, selectedArea.transform.localScale.x / 2f),
                                                                                         0f,
                                                                                         Random.Range(-selectedArea.transform.localScale.z / 2f, selectedArea.transform.localScale.z / 2f));

                // Crear una instancia del activo de decoración en la posición aleatoria
                GameObject newDecoration = Instantiate(selectedAsset, randomPosition, Quaternion.identity);
                // Asegurarse de que el activo de decoración esté dentro del área de decoración
                newDecoration.transform.parent = decoContainer.transform;
            }
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