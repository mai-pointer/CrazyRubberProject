using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public partial class Data
{
    public float mejor_puntuacion;
    public int monedas;
    public Dictionary<string, Sonido> sonido = new() //Sonidos
    {
        { "vibracion", new() },
        { "musica", new() },
        { "sonidos", new() },
    };
}
