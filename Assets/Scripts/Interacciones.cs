using CrazyRubberProject;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private int hijoSkin = 1, hijoEscudo = 0;

    private int cantMonedas = 1;

    [Header("Power ups")]
    [SerializeField] private Material fantasma;
    private PowerUp[] powerUps;
    private bool escudo;
    private bool inmortal;

    public GameObject psMuerte;
    private void Start()
    {
        BoxCollider boxcollider = GetComponent<BoxCollider>();
        Transform hijo = transform.GetChild(hijoSkin);

        List<Renderer> renderers = new();

        for (int i = 0; i < hijo.childCount; i++)
        {
            renderers.Add(hijo.GetChild(i).GetComponent<Renderer>());
        }

        powerUps = new PowerUp[]
        {
            new PowerUp("Fantasma", 5, (PowerUp elemento) => {
                foreach (var renderer in renderers)
                {
                    Material original = renderer.material;
                    renderer.material = fantasma;

                    //boxcollider.isTrigger = true;
                    inmortal = true;

                    StartCoroutine(Esperar(elemento, () =>
                    {
                        renderer.material = original;
                        //boxcollider.isTrigger = false;
                        inmortal = false;

                    }));
                }
            }),
            new PowerUp("Multiplicador", 10, (PowerUp elemento) => {
                transform.GetChild(1).GetComponent<Outline>().enabled = true;
                cantMonedas *= 2;

                StartCoroutine(Esperar(elemento, () =>
                {
                    transform.GetChild(1).GetComponent<Outline>().enabled = false;
                    cantMonedas /= 2;
                }));
            }),
            new PowerUp("Escudo", 25, (PowerUp elemento) => {
                GameObject escudoGO = transform.GetChild(hijoEscudo).gameObject;
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
        //MONEDAS
        if (other.CompareTag(tagMoneda))
        {
            Save.Data.monedas += cantMonedas * other.GetComponent<TileObject>().value;
            Controlador.ins.Dinero();
            Destroy(other.gameObject);
            Sonidos.GetSonido("Moneda");
            //**** Particulas/Animacion ****
        }

        //POWER UPS
        foreach (var elemento in powerUps)
        {
            if (other.CompareTag(elemento.tag))
            {
                if (elemento.usado) continue;

                Destroy(other.gameObject);
                Sonidos.GetSonido("Powerup");

                elemento.usado = true;
                elemento.funcion(elemento);
            }
        }

        //MUERTE
        if (other.CompareTag(tagMuerte))
        {
            if (escudo)
            {
                GameObject escudoGO = transform.GetChild(hijoEscudo).gameObject;

                escudoGO.SetActive(false);
                escudo = false;

                Destroy(other.gameObject);

                Destruir(powerUps[2].marcador);
                powerUps[2].usado = false;

                return;
            }

            if (!inmortal)
            {
                //Muerte
                Sonidos.GetSonido("Muerte");
                Controlador.ins.Muerto();
                Instantiate(psMuerte, transform.parent);
                Destroy(gameObject);
                //**** Particulas/Animacion ****
            }
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag(tagMuerte))
    //    {
    //        if (escudo)
    //        {
    //            GameObject escudoGO = transform.GetChild(hijoEscudo).gameObject;

    //            escudoGO.SetActive(false);
    //            escudo = false;

    //            Destroy(collision.gameObject);

    //            Destruir(powerUps[2].marcador);
    //            powerUps[2].usado = false;

    //            return;
    //        }

    //        //Muerte
    //        Controlador.ins.Muerto();
    //        Destroy(gameObject);
    //        //**** Particulas/Animacion ****
    //        Debug.Log("MUERTO");
    //    }
    //}

    public void Destruir(UIDuracion marcador)
    {
        int index = marcadores.IndexOf(marcador);
        if (index != -1)
        {
            for (int i = 0; i < index; i++)
            {
                StartCoroutine(marcadores[i].Mover(-1));
            }
        }

        marcadores.Remove(marcador);
        Destroy(marcador.gameObject);
    }

    private IEnumerator Esperar(PowerUp elemento, Action funcion)
    {
        foreach (var item in marcadores)
        {
            StartCoroutine(item.Mover());
        }

        GameObject marcador = Instantiate(prefabDuracion, posicionDuracion, Quaternion.identity);
        marcador.transform.SetParent(GameObject.Find("Canvas").transform, false);

        marcador.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = elemento.tag;

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
    public class PowerUp
    {
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
