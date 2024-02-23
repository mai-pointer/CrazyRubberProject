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
    
            // Instancia el prefab con la posici�n, escala y rotaci�n ajustadas
            GameObject ruedita = Instantiate(ruedas[seleccion], transform.position, Quaternion.identity, transform);
        }
        else
        {
            Debug.LogError("La selecci�n de rueda est� fuera de rango.");
        }

        transform.GetChild(1).gameObject.AddComponent<Rotacion>();

        Outline outline = gameObject.AddComponent<Outline>();
        outline.OutlineColor = new Color(1.0f, 0.5f, 0.0f, 1.0f);
        outline.OutlineWidth = 7;
        outline.enabled = false;
    }
}