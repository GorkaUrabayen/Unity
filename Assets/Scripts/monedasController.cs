using System;
using UnityEngine;

public class MonedasController : MonoBehaviour
{
    public int valor = 1; // Valor de la moneda
    public AudioClip sonidoMonedas; // Sonido que se reproducirá al recoger la moneda

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica si el objeto que colisiona tiene la etiqueta "Player"
        if (collision.CompareTag("Player"))
        {
            // Aumenta la puntuación en el GameManager
            GameManager.Instance.sumarPuntos(valor);

            // Registra en la consola la referencia del objeto
            Debug.Log($"Moneda recogida: {this.gameObject.name}");

            // Destruye la moneda
            Destroy(this.gameObject);

            // Reproduce el sonido de la moneda
            AudioManager.Instance.ReproducirSonido(sonidoMonedas);
        }
    }
}
