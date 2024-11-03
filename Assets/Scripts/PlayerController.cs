using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool mirandoDerecha = true;
    private BoxCollider2D boxCollider2D;
    private Animator animator;
    private bool enSuelo;
    private Rigidbody2D mirigidbody2D;
    public float fuerzaSalto = 10f;
    public float longitudraycast = 30f;
    public LayerMask capaSuelo;
    public float desplazamientoRaycastX = 0.5f;
    public float velocity = 5f;
    public float fuerzaGolpe;
    private bool puedeMoverse = true;
    private bool atacando;

    private float tiempoFueraDelSuelo = 0f; // Para contar el tiempo fuera del suelo
    private const float TIEMPO_LIMITE = 5f; // Tiempo límite para morir si no toca el suelo

    void Start()
    {
        mirigidbody2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        
        // Asegura que el Rigidbody no rote para evitar atravesar el suelo
        mirigidbody2D.freezeRotation = true;
    }

    void Update()
    {
        ProcesarMovimiento();
        ComprobarTiempoFueraDelSuelo(); // Comprobar el tiempo fuera del suelo
    }

    void ProcesarMovimiento()
    {
        if (!puedeMoverse) return;

        float movimientoHorizontal = Input.GetAxis("Horizontal");
        mirigidbody2D.linearVelocity = new Vector2(movimientoHorizontal * velocity, mirigidbody2D.linearVelocity.y);

        GestionarOrientacion(movimientoHorizontal);

        animator.SetBool("IsRunning", movimientoHorizontal != 0f);

        // Raycast para verificar si el jugador está en el suelo
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, longitudraycast, capaSuelo);
        enSuelo = hit.collider != null;

        if (enSuelo)
        {
            tiempoFueraDelSuelo = 0f; // Reiniciar el temporizador al tocar el suelo
        }

        if (enSuelo && Input.GetKeyDown(KeyCode.Space))
        {
            mirigidbody2D.AddForce(new Vector2(0f, fuerzaSalto), ForceMode2D.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.Z) && !atacando && enSuelo)
        {
            Atacando();
        }
        animator.SetBool("enSuelo", enSuelo);
        animator.SetBool("Atacando", atacando);
    }

    private void ComprobarTiempoFueraDelSuelo()
    {
        if (!enSuelo)
        {
            tiempoFueraDelSuelo += Time.deltaTime; // Incrementar el tiempo fuera del suelo

            if (tiempoFueraDelSuelo >= TIEMPO_LIMITE)
            {
                MuerteDelJugador(); // Llamar al método que maneja la muerte
            }
        }
    }

    private void MuerteDelJugador()
    {
        // Aquí puedes implementar la lógica para la muerte del jugador
        Debug.Log("El jugador ha muerto por estar demasiado tiempo en el aire");
        GameManager.Instance.perderVidas(); // Llama al método del GameManager para restar vidas
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * longitudraycast);
    }

    void GestionarOrientacion(float movimientoHorizontal)
    {
        if ((mirandoDerecha && movimientoHorizontal < 0) || (!mirandoDerecha && movimientoHorizontal > 0))
        {
            mirandoDerecha = !mirandoDerecha;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    public void aplicarGolpe()
    {
        puedeMoverse = false;
        Vector2 direccionGolpe = mirigidbody2D.linearVelocity.x > 0 ? new Vector2(1, -1) : new Vector2(-1, 1);
        mirigidbody2D.AddForce(direccionGolpe * fuerzaGolpe, ForceMode2D.Impulse);
        StartCoroutine(esperarYActivarMovimientos());
    }

    IEnumerator esperarYActivarMovimientos()
    {
        yield return new WaitForSeconds(1f);
        while (!enSuelo)
        {
            yield return null;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, longitudraycast, capaSuelo);
            enSuelo = hit.collider != null;
        }
        puedeMoverse = true;
    }

    public void Atacando()
    {
        atacando = true;
        mirigidbody2D.linearVelocity = Vector2.zero;  // Detiene cualquier movimiento para evitar que atraviese el suelo
        puedeMoverse = false;                   // Desactiva el movimiento mientras ataca
        StartCoroutine(ReactivarMovimientoTrasAtaque());
    }

    private IEnumerator ReactivarMovimientoTrasAtaque()
    {
        yield return new WaitForSeconds(0.5f); // Ajusta el tiempo según la duración de la animación de ataque
        atacando = false;
        puedeMoverse = true;
    }

    public void desactivarAtaque()
    {
        atacando = false;
    }
}
