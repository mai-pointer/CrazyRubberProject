using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Controlador : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI puntuacionTxt, dineroTxt;
    [SerializeField] private GameObject MenuMuerte;

    private float puntuacion = 0f;
    public static Controlador ins;
    private bool muerto;

    private void Awake()
    {
        ins = this;
    }

    private void Start()
    {
        Dinero();
    }

    private void Update()
    {
        if (!muerto)
        {
            puntuacion += Time.deltaTime * 10f;
            puntuacionTxt.text = puntuacion.ToString("F0");
        }
      
    }

    public void Dinero() 
    {
        dineroTxt.text = Save.Data.monedas.ToString();
    }

    public void Muerto() 
    {
        muerto = true;
        if (Save.Data.mejor_puntuacion < puntuacion) Save.Data.mejor_puntuacion = puntuacion;
        MenuMuerte.SetActive(true);
    }
}
