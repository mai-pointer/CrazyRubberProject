using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable]
public class RuedaItem
{
    public GameObject gameObjectRueda;
    public string nombreRueda;
    public int precio;
    public bool comprada;
    public int indice;
}

public class SelectorRueda : MonoBehaviour
{
    public Transform[] ruedas;
    public float sensibilidadDeslizamiento = 0.5f;
    public float separacionEntreRuedas = 15f;
    public float velocidadMovimiento = 5f;
    private Vector2 touchInicio;
    private Vector2 touchFin;
    public int indiceRuedaActual = 0;
    private bool enMovimiento = false;

    private int ruedaSeleccionada;

    public RuedaItem[] parametrosRuedas;

    public TextMeshProUGUI txtPrecio;
    public TextMeshProUGUI txtNombre;
    public Button botonSelect;
    public TextMeshProUGUI txtBoton;

    public int money = 121;

    public Image der, izq;

    public AudioClip cambioRuedaSound; // Variable para el sonido de cambio de rueda
    public AudioSource audioSource; // Variable para el AudioSource

    void Start()
    {
        money = Save.Data.monedas;

        // Posicionamos el GameObject de las ruedas en la posición inicial
        transform.position = new Vector3(-40f, transform.position.y, transform.position.z);

        CompararIndices(0);
    }

    private void Update()
    {

        if (Input.touchCount > 0 && !enMovimiento)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchInicio = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                touchFin = touch.position;
                float distanciaX = touchFin.x - touchInicio.x;

                if (Mathf.Abs(distanciaX) > sensibilidadDeslizamiento * Screen.width)
                {
                    int direccion = (distanciaX > 0) ? 1 : -1;
                    int siguienteIndice = Mathf.Clamp(indiceRuedaActual + direccion, 0, ruedas.Length - 1);

                    if (siguienteIndice != indiceRuedaActual)
                    {
                        // Reproducir el sonido de cambio de rueda al inicio (solo si la rueda actual no está en el primer índice)
                        if (indiceRuedaActual != 0 && cambioRuedaSound != null && audioSource != null)
                        {
                            audioSource.PlayOneShot(cambioRuedaSound);
                        }

                        StartCoroutine(MoverRuedas(siguienteIndice));
                    }
                }
            }
        }

        if (indiceRuedaActual == 0)
        {
            der.enabled = false;
        }
        else
        {
            der.enabled = true;
        }

        if (indiceRuedaActual == 4)
        {
            izq.enabled = false;
        }
        else
        {
            izq.enabled = true;
        }
    }

    public void CambiarSkin(int direccion)
    {
        int siguienteIndice = Mathf.Clamp(indiceRuedaActual + direccion, 0, ruedas.Length - 1);

        if (siguienteIndice != indiceRuedaActual)
        {
            // Reproducir el sonido de cambio de rueda al inicio (solo si la rueda actual no está en el primer índice)
            if (indiceRuedaActual != 0 && cambioRuedaSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(cambioRuedaSound);
            }

            StartCoroutine(MoverRuedas(siguienteIndice));
        }
    }

    private IEnumerator MoverRuedas(int siguienteIndice)
    {
        enMovimiento = true;
        Vector3 posicionDestino = new Vector3(-40f + siguienteIndice * separacionEntreRuedas, transform.position.y, transform.position.z);

        // Reproducir el sonido de cambio de rueda al comenzar a moverse
        if (cambioRuedaSound != null && audioSource != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(cambioRuedaSound);
        }

        while (transform.position != posicionDestino)
        {
            transform.position = Vector3.MoveTowards(transform.position, posicionDestino, velocidadMovimiento * Time.deltaTime);
            yield return null;
        }

        indiceRuedaActual = siguienteIndice;

        // Mostrar los datos de la rueda que coincide con el nuevo índice
        CompararIndices(indiceRuedaActual);

        enMovimiento = false;
    }

    private void CompararIndices(int indiceActual)
    {
        foreach (var rueda in parametrosRuedas)
        {
            if (rueda.indice == indiceActual)
            {
                txtNombre.text = rueda.nombreRueda;
                if (rueda.comprada)
                {
                    //comprar si es el mismo que esta almacenado en playerprefs y si es asi desabilitar el boton y poner seleccionado
                    if (PlayerPrefs.GetInt("RuedaSeleccionada", 0) == indiceActual)
                    {
                        botonSelect.interactable = false;
                        txtBoton.text = "Seleccionada";
                    }
                    else
                    {
                        botonSelect.interactable = true;
                        txtBoton.text = "Seleccionar";
                    }
                    txtPrecio.text = "Desbloqueada";
                }
                else
                {
                    botonSelect.interactable = true;
                    txtPrecio.text = money + "$ / " + rueda.precio.ToString() + "$";
                    txtBoton.text = "Comprar";
                }
                break;
            }
        }
    }

    public void ComprarSkin()
    {
        RuedaItem ruedaActual = parametrosRuedas[indiceRuedaActual];

        if (ruedaActual.comprada)
        {
            PlayerPrefs.SetInt("RuedaSeleccionada", ruedaActual.indice);
            botonSelect.interactable = false;
            txtBoton.text = "Seleccionada";
        }
        else
        {
            // Comprobar si el jugador tiene suficiente dinero para comprar la rueda
            if (money >= ruedaActual.precio)
            {
                // Restar el precio de la rueda al dinero del jugador
                money -= ruedaActual.precio;

                // Marcar la rueda como comprada
                ruedaActual.comprada = true;

                // Guardar el índice de la rueda seleccionada en PlayerPrefs
                PlayerPrefs.SetInt("RuedaSeleccionada", ruedaActual.indice);

                // Guardar el estado de compra de la rueda en PlayerPrefs
                PlayerPrefs.SetInt("RuedaComprada_" + ruedaActual.indice, 1);

                // Actualizar la interfaz de usuario
                txtPrecio.text = "Desbloqueada";
                botonSelect.interactable = false;
                txtBoton.text = "Seleccionada";

                // Mostrar mensaje de éxito
                Debug.Log("Rueda comprada con éxito.");
                Save.Data.monedas = money;
            }
            else
            {
                // Mostrar mensaje de que el jugador no tiene suficiente dinero
                Debug.Log("No tienes suficiente dinero para comprar esta rueda.");
            }
        }
    }

    void Awake()
    {
        // Verificar si es la primera vez que el jugador juega
        if (!PlayerPrefs.HasKey("PrimeraVezJugando"))
        {
            // Marcar la rueda 0 como comprada y seleccionada
            parametrosRuedas[0].comprada = true;
            PlayerPrefs.SetInt("RuedaSeleccionada", 0);
            PlayerPrefs.SetInt("RuedaComprada_0", 1);

            // Marcar que ya no es la primera vez que juega
            PlayerPrefs.SetInt("PrimeraVezJugando", 1);
        }

        // Cargar el estado de compra de las ruedas desde PlayerPrefs
        for (int i = 0; i < parametrosRuedas.Length; i++)
        {
            int comprada = PlayerPrefs.GetInt("RuedaComprada_" + i, 0);
            parametrosRuedas[i].comprada = (comprada == 1) ? true : false;
        }
    }
}
