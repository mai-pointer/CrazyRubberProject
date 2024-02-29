using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F60FPS : MonoBehaviour
{
    void Start()
    {
        // Establecer la frecuencia de actualización a 60 Hz
        Application.targetFrameRate = 60;
    }
}
