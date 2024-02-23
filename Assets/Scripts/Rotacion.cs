using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotacion : MonoBehaviour
{
    [SerializeField] private float velocidadRotacion = 250;

    void Update()
    {
        transform.Rotate(Vector3.left, velocidadRotacion * Time.deltaTime);
    }
}
