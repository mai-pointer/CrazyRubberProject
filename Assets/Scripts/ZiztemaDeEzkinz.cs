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

        Vector3 position = transform.position; // Posici�n por defecto

        // Verificar si la rueda seleccionada es la n�mero 4 
        if (PlayerPrefs.GetInt("RuedaSeleccionada") == 4)
        {
            // Ajustar la posici�n en x si es la rueda n�mero 4
            position.x = -1.75f;
        }

        if (PlayerPrefs.GetInt("RuedaSeleccionada") == 0)
        {
            // Ajustar la posici�n en x si es la rueda n�mero 4
            position.x = 0.65f;
        }

        // Obt�n la rotaci�n del prefab que deseas instanciar
        Quaternion rotation = ruedas[PlayerPrefs.GetInt("RuedaSeleccionada")].transform.rotation;

        // Instancia el prefab con la misma rotaci�n
        GameObject ruedita = Instantiate(ruedas[PlayerPrefs.GetInt("RuedaSeleccionada")], position, rotation, transform);
    }

    void Update()
    {

    }
}

