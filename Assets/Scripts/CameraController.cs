using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform jugador;
    float distanciaCamaraAlBorde;

    void Start()
    {
        // Calcula la distancia al borde basada en el tamaño ortográfico y el aspecto de la cámara
        distanciaCamaraAlBorde = Camera.main.orthographicSize * Camera.main.aspect;
    }

    void Update()
    {
        // Si el jugador está a la derecha del borde visible de la cámara
        if (jugador.position.x > (transform.position.x + distanciaCamaraAlBorde))
        {
            // Mueve la cámara a la derecha en la distancia al borde
            transform.position += new Vector3(distanciaCamaraAlBorde, 0, 0);
        }
        // Si el jugador está a la izquierda del borde visible de la cámara
        else if (jugador.position.x < (transform.position.x - distanciaCamaraAlBorde))
        {
            // Mueve la cámara a la izquierda en la distancia al borde
            transform.position -= new Vector3(distanciaCamaraAlBorde, 0, 0);
        }
         if (jugador.position.y > (transform.position.y + Camera.main.orthographicSize))
        {
            // Mueve la cámara hacia arriba en la distancia al borde
            transform.position += new Vector3(0, Camera.main.orthographicSize, 0);
        }
        // Si el jugador está por debajo del borde visible de la cámara
        else if (jugador.position.y < (transform.position.y - Camera.main.orthographicSize))
        {
            // Mueve la cámara hacia abajo en la distancia al borde
            transform.position -= new Vector3(0, Camera.main.orthographicSize, 0);
        }
    }
}
