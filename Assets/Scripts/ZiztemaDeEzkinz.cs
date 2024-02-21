using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZiztemaDeEzkinz : MonoBehaviour
{
    public GameObject[] ruedas;
    public Vector3[] escalas;
    public Vector3[] posiciones;

    void Start()
    {
        if (!PlayerPrefs.HasKey("RuedaSeleccionada"))
        {
            PlayerPrefs.SetInt("RuedaSeleccionada", 0);
        }

        int seleccion = PlayerPrefs.GetInt("RuedaSeleccionada");

        if (seleccion >= 0 && seleccion < ruedas.Length)
        {
            // Obt�n la posici�n, escala y rotaci�n correspondientes a la selecci�n
            Vector3 posicion = posiciones[seleccion];
            Vector3 escala = escalas[seleccion];
            Quaternion rotacion = ruedas[seleccion].transform.rotation;

            // Instancia el prefab con la posici�n, escala y rotaci�n ajustadas
            GameObject ruedita = Instantiate(ruedas[seleccion], posicion, rotacion, transform);
            ruedita.transform.localScale = escala;
        }
        else
        {
            Debug.LogError("La selecci�n de rueda est� fuera de rango.");
        }
    }

    void Update()
    {

    }
}
