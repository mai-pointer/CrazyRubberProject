using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skins : MonoBehaviour
{
    public GameObject[] ruedas;

    void Awake()
    {
        if (!PlayerPrefs.HasKey("RuedaSeleccionada"))
        {
            PlayerPrefs.SetInt("RuedaSeleccionada", 0);
        }

        int seleccion = PlayerPrefs.GetInt("RuedaSeleccionada");

        if (seleccion >= 0 && seleccion < ruedas.Length)
        {
    
            // Instancia el prefab con la posición, escala y rotación ajustadas
            GameObject ruedita = Instantiate(ruedas[seleccion], transform.position, Quaternion.identity, transform);
        }
        else
        {
            Debug.LogError("La selección de rueda está fuera de rango.");
        }

        transform.GetChild(1).gameObject.AddComponent<Rotacion>();
    }
}
