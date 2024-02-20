using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Personaje))]
public class Interacciones : MonoBehaviour
{
    [Header("Marcador tiempo")]
    [SerializeField] private GameObject prefabDuracion;
    [SerializeField] private Vector2 posicionDuracion = new Vector2(-300f, -850f);
    [HideInInspector] public List<UIDuracion> marcadores = new();


    [Header("Principal")]
    [SerializeField] private string tagMuerte = "Muerte";
    [SerializeField] private string tagMoneda = "Moneda";

    private int cantMonedas = 1;

    [Header("Power ups")]
    private PowerUp[] powerUps;
    private bool escudo;

    private void Start()
    {
        BoxCollider boxcollider = GetComponent<BoxCollider>();
        Renderer renderer = transform.GetChild(0).GetComponent<Renderer>();

        powerUps = new PowerUp[]
        {
            new PowerUp("Fantasma", 5, (PowerUp elemento) => {
                Color color = renderer.material.color;
                color.a = 0.75f; //Opacidad
                renderer.material.color = color;

                boxcollider.isTrigger = true;

                StartCoroutine(Esperar(elemento, () =>
                {
                    color.a = 1f;
                    renderer.material.color = color;

                    boxcollider.isTrigger = false;
                    
                }));
            }),
            new PowerUp("Multiplicador", 10, (PowerUp elemento) => {
                cantMonedas *= 2;

                StartCoroutine(Esperar(elemento, () =>
                {
                    cantMonedas /= 2;
                }));
            }),
            new PowerUp("Escudo", 25, (PowerUp elemento) => {
                GameObject escudoGO = transform.GetChild(1).gameObject;
                escudoGO.SetActive(true);
                escudo = true;

                StartCoroutine(Esperar(elemento, () =>
                {
                    escudo = false;
                    escudoGO.SetActive(false);
                }));
            })
        };
    }

    //Detectores

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagMoneda))
        {
            //Monedas
            Save.Data.monedas += cantMonedas;
        }

        //POWER UPS
        foreach (var elemento in powerUps)
        {
            if (other.CompareTag(elemento.tag))
            {
                if (elemento.usado) continue;

                elemento.usado = true;
                elemento.funcion(elemento);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(tagMuerte))
        {
            if (escudo)
            {
                GameObject escudoGO = transform.GetChild(1).gameObject;

                escudoGO.SetActive(false);
                escudo = false;

                Destroy(collision.gameObject);
                powerUps[2].marcador.Destruir();
                powerUps[2].usado = false;

                return;
            }

            //Muerte
            Debug.Log("MUERTO");
        }
    }

    private IEnumerator Esperar(PowerUp elemento, Action funcion)
    {
        foreach (var item in marcadores)
        {
            StartCoroutine(item.Mover());
        }

        GameObject marcador = Instantiate(prefabDuracion, posicionDuracion, Quaternion.identity);
        marcador.transform.SetParent(GameObject.Find("Canvas").transform, false);
        UIDuracion marcadorScript = marcador.GetComponent<UIDuracion>();
        marcadorScript.tiempo = elemento.duracion;
        marcadorScript.interacciones = this;
        StartCoroutine(marcadorScript.Mover());

        marcadores.Add(marcadorScript);
        elemento.marcador = marcadorScript;

        yield return new WaitForSeconds(elemento.duracion);

        elemento.usado = false;
        funcion?.Invoke();
    }

    [Serializable]
    public class PowerUp{
        public string tag;
        public float duracion;
        public bool usado;
        public UIDuracion marcador;
        public Action<PowerUp> funcion;


        public PowerUp(string tag, float duracion, Action<PowerUp> funcion)
        {
            this.tag = tag;
            this.duracion = duracion;
            this.funcion = funcion;
        }
    }
}
