using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personaje : MonoBehaviour
{
    [Header("Caminos")]
    [SerializeField] private float distanciaCaminos = 50f;

    [Header("Movimiento")]
    [SerializeField] private float tiempoMov = .5f;
    [SerializeField] private AnimationCurve animacion;
    private bool moviendo;

    [Header("Salto")]
    [SerializeField] private float fuerzaSalto = 100f;
    [SerializeField] private float alturaPersonaje = 1f;
    private Rigidbody rb;

    [Header("Agachar")]
    [SerializeField] private float alturaAgachado = 1f;
    private BoxCollider boxcolider;

    [Header("Controles")]
    [SerializeField] private float deslizamientoMin = 50f;
    [SerializeField] private bool movil, pc;

    private Vector2 inicialPos;

    void Update()
    {
        rb = GetComponent<Rigidbody>();
        boxcolider = GetComponent<BoxCollider>();

        if (movil) Movil();
        if(pc) PC();
    }

    private void Dirigir(int index)
    {
        switch (index)
        {
            case 0:
                //Salto
                RaycastHit hit;
                
                if (Physics.Raycast(transform.position, Vector3.down, out hit, alturaPersonaje))
                {
                    rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
                }
                break;
            case 1:
                //Agachar
                rb.AddForce(Vector3.down * fuerzaSalto * 2, ForceMode.Impulse);
                boxcolider.size = new Vector3(transform.localScale.x, alturaAgachado, transform.localScale.z);
                break;
            case 2:
                //Derecha
                if (moviendo) return;
                if (transform.position.x == distanciaCaminos) return;

                StartCoroutine(
                    Mover(new Vector3(
                        transform.position.x + distanciaCaminos,
                        transform.position.y,
                        0)
                    )
                );
                break;
            case 3:
                //Izquierda
                if (moviendo) return;
                if (transform.position.x == -distanciaCaminos) return;

                StartCoroutine(
                    Mover(new Vector3(
                        transform.position.x - distanciaCaminos,
                        transform.position.y,
                        0)
                    )
                );
                break;
        }
    }

    private IEnumerator Mover(Vector3 destino)
    {

        Vector3 posicionInicial = transform.localPosition;
        float tiempoPasado = 0f;
        moviendo = true;

        while (tiempoPasado < tiempoMov)
        {
            float tiempo = tiempoPasado / tiempoMov;
            float velocidadActual = animacion.Evaluate(tiempo);

            Vector3 nuevaPosicion = Vector3.Lerp(posicionInicial, destino, velocidadActual);
            transform.localPosition = nuevaPosicion;

            tiempoPasado += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = destino;
        moviendo = false;

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;

        //Altura
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * alturaPersonaje);

        //Caminos
        float altura = .5f;
        float tamaño = .25f;

        Gizmos.DrawWireSphere(
            new Vector3(+distanciaCaminos, altura, 0)
            , tamaño
        );
        Gizmos.DrawWireSphere(
            new Vector3(0, altura, 0)
            , tamaño
        );
        Gizmos.DrawWireSphere(
            new Vector3(-distanciaCaminos, altura, 0)
            , tamaño
        );
    }

    private void Movil()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            //INICIO
            if (touch.phase == TouchPhase.Began)
            {
                inicialPos = touch.position;
            }
            //FIN
            if (touch.phase == TouchPhase.Ended)
            {
                Vector2 finalPos = touch.position;
                Vector2 apuntado = finalPos - inicialPos;

                if (apuntado.magnitude < deslizamientoMin) return;
                apuntado.Normalize();

                if (apuntado.y > 0.5f)
                {
                    Dirigir(0);
                }
                else if (apuntado.y < -0.5f)
                {
                    Dirigir(1);
                }
                else if (apuntado.x > 0.5f)
                {
                    Dirigir(2);
                }
                else if (apuntado.x < -0.5f)
                {
                    Dirigir(3);
                }
            }
        }
    }
    private void PC()
    {
        KeyCode[] teclas = {
            KeyCode.W,
            KeyCode.S,
            KeyCode.D,
            KeyCode.A
        };

        for (int i = 0; i < teclas.Length; i++)
        {
            if (Input.GetKeyDown(teclas[i]))
            {
                Dirigir(i);
            }
        }
    }
}
