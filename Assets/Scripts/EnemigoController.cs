using System.Collections;
using UnityEngine;

public class EnemigoController : MonoBehaviour
{
    public float velocidadMovimiento = 2f;
    public float cooldownAtaque = 1f;
    private bool puedeAtacar = true;
    private bool mirandoDerecha = true;
    private bool estamuerto = false;
    private Animator animator;
    private Rigidbody2D mirigidbody2D;
    private SpriteRenderer spriteRenderer;
    private bool recibiendoDanio;
    public LayerMask capaSuelo;
    public float longitudRaycastSuelo = 1f;
    public float tiempoDireccion = 3f;
    public float fuerzaGolpe;

    private int contadorGolpes = 0; // Contador de golpes recibidos

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        mirigidbody2D = GetComponent<Rigidbody2D>();

        StartCoroutine(MovimientoAlternante());
    }

    void Update()
    {
        ActualizarAnimacion();
    }

    private IEnumerator MovimientoAlternante()
    {
        while (!estamuerto) // Detener movimiento si el enemigo está muerto
        {
            MoverEnDireccion();
            yield return new WaitForSeconds(tiempoDireccion);
            CambiarDireccion();
        }
    }

    private void MoverEnDireccion()
    {
        if (!recibiendoDanio && !estamuerto) // No mover si está muerto
        {
            float direccion = mirandoDerecha ? 1 : -1;
            mirigidbody2D.linearVelocity = new Vector2(direccion * velocidadMovimiento, mirigidbody2D.linearVelocity.y);
        }
    }

    private void CambiarDireccion()
    {
        if (!estamuerto) // Cambiar dirección solo si está vivo
        {
            mirandoDerecha = !mirandoDerecha;
            transform.localScale = new Vector3(mirandoDerecha ? 1 : -1, 1, 1);
        }
    }

    private void ActualizarAnimacion()
{
    // Si el enemigo está muerto, no actualices otras animaciones
    if (estamuerto) return;

    animator.SetBool("correr", Mathf.Abs(mirigidbody2D.linearVelocity.x) > 0);
}


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && puedeAtacar)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.perderVidas();
            }
            else
            {
                Debug.LogWarning("GameManager instance is null. Make sure GameManager is initialized properly.");
            }

            puedeAtacar = false;
            StartCoroutine(ReactivarAtaque());

            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.aplicarGolpe();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D colision)
    {
        if (colision.CompareTag("Espada"))
        {
            Vector2 direccionDanio = (transform.position - colision.transform.position).normalized;
            RecibeDanio(direccionDanio, 1);
        }
    }

    private IEnumerator ReactivarAtaque()
    {
        yield return new WaitForSeconds(cooldownAtaque);
        puedeAtacar = true;
    }

    public void RecibeDanio(Vector2 direccion, int cantidadDanio)
    {
        if (!recibiendoDanio && !estamuerto)
        {
            contadorGolpes++; // Incrementar el contador de golpes
            recibiendoDanio = true;

            // Aplicamos la fuerza de retroceso en la dirección opuesta a la espada
            Vector2 rebote = direccion * fuerzaGolpe;
            mirigidbody2D.AddForce(rebote, ForceMode2D.Impulse);

            // Activamos una animación de daño si existe
            if (animator != null)
            {
                animator.SetTrigger("recibirDanio");
            }

            // Verificamos si el enemigo ha recibido suficientes golpes
            if (contadorGolpes >= 2)
            {
                Morir(); // Llamar al método de muerte si se han recibido dos golpes
            }
            else
            {
                StartCoroutine(RecuperarDeDanio(1f)); // Ajusta el tiempo de recuperación según necesites
            }
        }
    }

 private void Morir()
{
    estamuerto = true; // Marcar al enemigo como muerto
    animator.SetBool("muerto", true); // Activar animación de muerte
    
    // Detener el movimiento del enemigo
    mirigidbody2D.linearVelocity = Vector2.zero; // Establece la velocidad en cero
    mirigidbody2D.useFullKinematicContacts = true; // Evita que se mueva por física

    // Detén cualquier corrutina de movimiento si está en curso
    StopAllCoroutines(); // Esto detendrá el MovimientoAlternante y cualquier otra corrutina

    // Destruir el enemigo después de la animación de muerte
    Invoke("DestruirEnemigo", 2f); // Ajusta el tiempo según la duración de la animación de muerte
}

private void DestruirEnemigo()
{
    Destroy(gameObject); // Destruye el enemigo después de la animación de muerte
}


    private IEnumerator RecuperarDeDanio(float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        recibiendoDanio = false; // Permitimos al enemigo moverse nuevamente
    }
}
