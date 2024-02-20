using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDuracion : MonoBehaviour
{
    [SerializeField] private float duracionMovimiento = 1, distancia = 150;
    [SerializeField] private AnimationCurve animacion;

    [HideInInspector] public Interacciones interacciones;
    [HideInInspector] public float tiempo;


    private void Start()
    {
        StartCoroutine(FadeImage());
    }

    public IEnumerator Mover()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();

        float destinoY = rectTransform.anchoredPosition.y + distancia;
        Vector3 destino = new Vector3(rectTransform.anchoredPosition.x, destinoY, 0);

        Vector3 posicionInicial = rectTransform.localPosition;
        float tiempoPasado = 0f;

        while (tiempoPasado < duracionMovimiento)
        {
            if (rectTransform == null) break;

            float tiempo = tiempoPasado / duracionMovimiento;
            float velocidadActual = animacion.Evaluate(tiempo);

            Vector3 nuevaPosicion = Vector3.Lerp(posicionInicial, destino, velocidadActual);
            rectTransform.localPosition = nuevaPosicion;

            tiempoPasado += Time.deltaTime;
            yield return null;
        }

        if (rectTransform != null) rectTransform.localPosition = destino;

    }

    IEnumerator FadeImage()
    {
        Image image = GetComponent<Image>();
        float tiempoPasado = 0f;

        while (tiempoPasado < tiempo)
        {
            float fillAmount = Mathf.Lerp(1f, 0f, tiempoPasado / tiempo);
            image.fillAmount = fillAmount;

            tiempoPasado += Time.deltaTime;

            yield return null; 
        }

        image.fillAmount = 0f;

        interacciones.marcadores.Remove(this);
        Destroy(gameObject);
    }
}
