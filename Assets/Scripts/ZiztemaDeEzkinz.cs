using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZiztemaDeEzkinz : MonoBehaviour
{
    public GameObject[] ruedas;

    void Awake()
    {
        if (!PlayerPrefs.HasKey("RuedaSeleccionada"))
        {
            PlayerPrefs.SetInt("RuedaSeleccionada", 0);
        }

        Vector3 position = transform.position; // Posición por defecto

        // Verificar si la rueda seleccionada es la número 4 
        if (PlayerPrefs.GetInt("RuedaSeleccionada") == 4)
        {
            // Ajustar la posición en x si es la rueda número 4
            position.x = -1.75f;
        }

        if (PlayerPrefs.GetInt("RuedaSeleccionada") == 0)
        {
            // Ajustar la posición en x si es la rueda número 4
            position.x = 0.65f;
        }

        // Obtén la rotación del prefab que deseas instanciar
        Quaternion rotation = ruedas[PlayerPrefs.GetInt("RuedaSeleccionada")].transform.rotation;

        // Instancia el prefab con la misma rotación
        GameObject ruedita = Instantiate(ruedas[PlayerPrefs.GetInt("RuedaSeleccionada")], position, rotation, transform);
    }

    void Update()
    {

    }
}

