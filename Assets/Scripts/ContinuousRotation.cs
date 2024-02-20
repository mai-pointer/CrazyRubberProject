using UnityEngine;

public class ContinuousRotation : MonoBehaviour
{
    public float rotationSpeed = 30f; // Velocidad de rotación en grados por segundo

    void Update()
    {
        // Rotar el objeto continuamente en el eje local Y
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.Self);
    }
}
