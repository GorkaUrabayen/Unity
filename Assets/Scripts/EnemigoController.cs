using System.Collections;
using UnityEngine;

public class EnemigoController : MonoBehaviour
{
    public float velocidadMovimiento = 2f;
    public float cooldownAtaque = 1f;
    private bool puedeAtacar = true;
    private bool mirandoDerecha = true;

    private Animator animator;
    private Rigidbody2D mirigidbody2D;
    private SpriteRenderer spriteRenderer;

    public LayerMask capaSuelo;
    public float longitudRaycastSuelo = 1f;
    public float tiempoDireccion = 3f; // Tiempo en segundos que el enemigo va en cada dirección

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        mirigidbody2D = GetComponent<Rigidbody2D>();

        // Iniciar el movimiento alternante
        StartCoroutine(MovimientoAlternante());
    }

    void Update()
    {
        ActualizarAnimacion();
    }

    private IEnumerator MovimientoAlternante()
    {
        while (true) // Loop infinito para alternar indefinidamente
        {
            MoverEnDireccion();
            yield return new WaitForSeconds(tiempoDireccion); // Espera el tiempo especificado
            CambiarDireccion(); // Cambia de dirección después de esperar
        }
    }

    private void MoverEnDireccion()
    {
        // Dirección del movimiento dependiendo de si mira a la derecha o no
        float direccion = mirandoDerecha ? 1 : -1; // Cambiar el signo para que funcione correctamente
        mirigidbody2D.linearVelocity = new Vector2(direccion * velocidadMovimiento, mirigidbody2D.linearVelocity.y);
    }

    private void CambiarDireccion()
    {
        // Alterna la dirección y voltea el sprite
        mirandoDerecha = !mirandoDerecha;

        // Cambia la escala para voltear el sprite
        if (mirandoDerecha)
        {
            transform.localScale = new Vector3(1, 1, 1); // Mirando a la derecha
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1); // Mirando a la izquierda
        }
    }

    private void ActualizarAnimacion()
    {
        // Activa la animación de correr si el enemigo se está moviendo horizontalmente
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

    private IEnumerator ReactivarAtaque()
    {
        yield return new WaitForSeconds(cooldownAtaque);
        puedeAtacar = true;
    }

    void OnDrawGizmos()
    {
        // Visualiza el raycast para detectar suelo o borde
        Gizmos.color = Color.red;
        Vector3 posicionRaycast = transform.position + new Vector3((mirandoDerecha ? 1 : -1) * 0.5f, 0, 0);
        Gizmos.DrawLine(posicionRaycast, posicionRaycast + Vector3.down * longitudRaycastSuelo);
    }
}
