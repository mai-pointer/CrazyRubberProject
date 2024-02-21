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
            // Obtén la posición, escala y rotación correspondientes a la selección
            Vector3 posicion = posiciones[seleccion];
            Vector3 escala = escalas[seleccion];
            Quaternion rotacion = ruedas[seleccion].transform.rotation;

            // Instancia el prefab con la posición, escala y rotación ajustadas
            GameObject ruedita = Instantiate(ruedas[seleccion], posicion, rotacion, transform);
            ruedita.transform.localScale = escala;
        }
        else
        {
            Debug.LogError("La selección de rueda está fuera de rango.");
        }
    }

    void Update()
    {

    }
}
