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
                // Elegir aleatoriamente un �rea de decoraci�n y un activo de decoraci�n
                GameObject selectedArea = decorationAreas[Random.Range(0, decorationAreas.Length)];
                GameObject selectedAsset = decorationAssets[Random.Range(0, decorationAssets.Length)];

                // Obtener una posici�n aleatoria dentro del �rea de decoraci�n
                Vector3 randomPosition = selectedArea.transform.position + new Vector3(Random.Range(-selectedArea.transform.localScale.x / 2f, selectedArea.transform.localScale.x / 2f),
                                                                                         0f,
                                                                                         Random.Range(-selectedArea.transform.localScale.z / 2f, selectedArea.transform.localScale.z / 2f));

                // Crear una instancia del activo de decoraci�n en la posici�n aleatoria
                GameObject newDecoration = Instantiate(selectedAsset, randomPosition, Quaternion.identity);
                // Asegurarse de que el activo de decoraci�n est� dentro del �rea de decoraci�n
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