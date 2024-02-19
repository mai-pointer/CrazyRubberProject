using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(0)]
[DisallowMultipleComponent]
public class Save : MonoBehaviour
{
    //VARIABELS
    [SerializeField] private Data data = new Data();
    [HideInInspector] public bool mensajes = false;

    //ESTATICA
    [HideInInspector] public static Data Data { get => miInstancia.data; set => miInstancia.data = value; }
    private static Save miInstancia;

    // Recoge los datos y los carga desde PlayerPrefs
    private void Awake()
    {
        // Convierte el script en Singleton
        #region singleton
        if (miInstancia == null)
        {
            miInstancia = this;
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion

        //Carga los datos
        Cargar();
    }

    //Carga los datos si los hay
    #region cargar_datos
    public void Cargar()
    {
        string jsonString = PlayerPrefs.GetString("data");
        if (!string.IsNullOrEmpty(jsonString))
        {
            data = JsonUtility.FromJson<Data>(jsonString);
            if (mensajes)
            {
                Debug.Log("Datos cargados correctamente");
            }
        }
        else
        {
            // No se ha encontrado ningún dato en PlayerPrefs
            Debug.LogWarning("No se han encontrado datos guardados");
        }
    }
    #endregion

    // Al salir de la aplicación, guarda los datos en PlayerPrefs
    #region guardar_datos
    private void OnApplicationPause(bool pause)
    {
        if (pause) Guardar();
    }
    private void OnApplicationQuit()
    {
        Guardar();
    }
    public void Guardar()
    {
        string jsonString = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("data", jsonString);

        if (mensajes)
        {
            Debug.Log("Datos guardados en PlayerPrefs");
        }
    }
    #endregion

    // Elimina los datos de PlayerPrefs
    #region eliminar_datos
    public void Eliminar()
    {
        data = new Data();
        PlayerPrefs.DeleteKey("data");
        if (mensajes) Debug.Log("Datos eliminados correctamente de PlayerPrefs");
    }
    #endregion
}