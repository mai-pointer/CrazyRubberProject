using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DefaultExecutionOrder(0)]
[DisallowMultipleComponent]

public class Sonidos : MonoBehaviour
{
    //VARIABLES 

    //PUBLICAS
    [Header("--Sonidos--")]
    [SerializeField] private DictionaryBG<AudioClip> _sonidos = new();
    [Header("--Musica--")]
    [SerializeField] private string autoPlay = "";
    [SerializeField] private DictionaryBG<AudioClip> _musica = new();
    [Header("--Botones--")]
    [SerializeField] private string sonidoBotones = "";
    //ESTATICAS
    private static List<SonidosCreados> musicaCreada = new();
    private static List<SonidosCreados> sonidoCreado = new();
    private static Transform padre, padreInmortal;
    //PRIVADAS
    public static DictionaryBG<AudioClip> sonidos = new();
    public static DictionaryBG<AudioClip> musica = new();

    public static Dictionary<string, Sonido> sonido { get => Save.Data.sonido; set => Save.Data.sonido = value; }

    private void OnValidate()
    {
        sonidos = _sonidos;
        musica = _musica;
    }

    private void Awake()
    {
        //Reproduce automaticamente la musica
        if (autoPlay != "") GetMusica(autoPlay, true);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += Cargado;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= Cargado;
    }

    //PUBLICAS

    //Metodos para cambiar el volumen y los estados
    #region sonidos volumen/estado
    public static void AlternarEstado(TipoSonido tipo)
    {
        Estado(tipo, !Save.Data.sonido[tipo.ToString()].estado);
    }

    public static void Estado(TipoSonido tipo, bool estado)
    {
        //Lo cambia
        Save.Data.sonido[tipo.ToString()].estado = estado;

        //Lo aplica a los sonidos existentes
        List<SonidosCreados> lista = new();

        if ("musica" == tipo.ToString()) lista = musicaCreada;
        else if ("sonidos" == tipo.ToString()) lista = sonidoCreado;

        foreach (var elemento in lista)
        {
            //Se mutea o desmutea
            elemento.sonido.volume = (!sonido[tipo.ToString()].estado)
            ? 0
            : ((float)sonido[tipo.ToString()].volumen / 100);
        }
    }
    public static void Volumen(TipoSonido tipo, int volumen)
    {
        //Errores
        if (volumen < 0)
        {
            volumen = 0;
            Debug.Log("[BL][Sonidos: 1] El volumen no puede ser menor que 0");
        }
        else if (volumen < 0)
        {
            volumen = 100;
            Debug.Log("[BL][Sonidos: 2] El volumen no puede ser mayor que 100");
        }

        //Lo cambia
        Save.Data.sonido[tipo.ToString()].volumen = volumen;

        //Lo aplica a los sonidos existentes
        List<SonidosCreados> lista = new();

        if ("musica" == tipo.ToString()) lista = musicaCreada;
        else if ("sonidos" == tipo.ToString()) lista = sonidoCreado;

        foreach (var elemento in lista)
        {
            elemento.sonido.volume = ((float)sonido[tipo.ToString()].volumen / 100);
        }

    }
    #endregion

    //Metodos para instanciar un sonido o vibracion
    #region instanciar
    public static void GetVibracion()
    {
        if (Save.Data.sonido["vibracion"].estado)
        {
            if (SystemInfo.supportsVibration)
            {
                Handheld.Vibrate();
                return;
            }
            Debug.Log("El dispositivo actual no soporta la vibracion");
        }
    }
    public static AudioSource GetSonido(string nombre, bool bucle = false)
    {
        int volumen = Save.Data.sonido["sonidos"].volumen;
        if (!Save.Data.sonido["sonidos"].estado)
        {
            volumen = 0;
        }
        AudioSource audio = Creador(sonidos, nombre, volumen, bucle);
        sonidoCreado.Add(new SonidosCreados(audio));
        return audio;
    }
    public static AudioSource GetMusica(string nombre, bool bucle = false)
    {
        int volumen = Save.Data.sonido["musica"].volumen;
        if (!Save.Data.sonido["musica"].estado)
        {
            volumen = 0;
        }
        AudioSource audio = Creador(musica, nombre, volumen, bucle);
        musicaCreada.Add(new SonidosCreados(audio));
        return audio;
    }
    private static AudioSource Creador(DictionaryBG<AudioClip> lista, string nombre, int volumen, bool bucle)
    {
        //Comprueba que exista el audio
        if (!lista.Inside(nombre))
        {
            //Ese sonido no esta dentro del array
            Debug.Log($"[BL][Sonidos: 3] No existe el sonido {nombre} dentro de Sounds");
            return null;
        }
        //Crea un contenedor padre
        if (padre == null)
        {
            padre = new GameObject($"Sonidos").transform;
            padre.position = Vector3.zero;
        }
   
        //Instancia el audio
        GameObject instancia = new GameObject($"Sonido-{nombre}");
        instancia.transform.position = Vector3.zero;

        //Crea el audioi source
        AudioSource audioSource = instancia.AddComponent<AudioSource>();
        audioSource.clip = lista.Get(nombre);
        audioSource.volume = ((float)volumen / 100);

        instancia.transform.SetParent(padre);


        //Lo activa en bucle
        if (bucle) audioSource.loop = true;
        else Destroy(instancia, lista.Get(nombre).length);

        //Lo activa y devuelve
        audioSource.Play();
        return audioSource;
    }
    #endregion

    //PRIVADAS

    //Asigna a los botones sonidos
    #region botones
    private void Cargado(Scene scene, LoadSceneMode mode)
    {
        sonidos = _sonidos;
        musica = _musica;

        if (sonidoBotones != "")
        {
            Button[] botones = FindObjectsOfType<Button>(includeInactive: true);
            foreach (Button boton in botones)
            {
                boton.onClick.AddListener(() => { GetSonido(sonidoBotones); });
            }
        }
    }
    #endregion
}


//Clases que se usan en la ventana de SONIDOS
#region sonidos
[Serializable]
public class Sonido
{
    public bool estado = true;
    public int volumen = 100;
}
[Serializable]
public class SonidosCreados
{
    public AudioSource sonido;
    public bool inmortal;

    public SonidosCreados(AudioSource sonido)
    {
        this.sonido = sonido;
    }
}
public enum TipoSonido
{
    vibracion,
    musica,
    sonidos
}
#endregion

//DICCIONARIO 
#region diccionario
[Serializable]
public class DictionaryBG<T>
{
    //VARIABLES
    [SerializeField] public List<Elementos> data = new List<Elementos>();

    //Contenido que va en la lista elementos
    #region contenido   
    [Serializable]
    public class Elementos
    {
        [SerializeField] public string indice;
        [SerializeField] public T valor;

        public Elementos(string indice, T valor)
        {
            this.valor = valor;
            this.indice = indice;
        }
    }
    #endregion

    //Coger informacion del diccionario
    #region get
    //Usando un indice string
    public T Get(string indice)
    {
        T valor = default(T);

        data.ForEach((elemento) =>
        {
            if (indice.ToLower() == elemento.indice.ToLower())
            {
                valor = elemento.valor;
            }
        });

        return valor;
    }
    //Usando un indice int
    public T Get(int indice) => data[indice].valor;
    #endregion

    //Añade elementos al diccionario
    #region add
    public void Add(string indice, T valor)
    {
        data.Add(new Elementos(indice, valor));
    }
    #endregion

    //Te dice si contiene un valor dentro del diccionario
    #region inside
    //Usando un indice string
    public bool Inside(string indice)
    {
        bool dentro = false;
        data.ForEach((element) =>
        {
            if (indice.ToLower() == element.indice.ToLower())
            {
                dentro = true;
            }
        });
        return dentro;
    }
    //Usando un indice int
    public bool Inside(int indice)
    {
        return (indice >= 0 && indice < data.Count);
    }
    #endregion

    //Te da todos los elementos
    #region foreach
    public void ForEach(Action<string, T> func)
    {
        data.ForEach((elemento) =>
        {
            func(elemento.indice, elemento.valor);
        });
    }
    #endregion

    //Te da la lonngitud de la lista
    #region length
    public int Length() => data.Count;
    #endregion

}
#endregion