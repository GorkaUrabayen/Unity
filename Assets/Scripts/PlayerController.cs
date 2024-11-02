using System.Collections;
using Unity.VisualScripting;
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


    void Start()
    {
        mirigidbody2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        ProcesarMovimiento();


    }


    void ProcesarMovimiento()
    {
        if(!puedeMoverse) return;
        
        float movimientoHorizontal = Input.GetAxis("Horizontal");
        mirigidbody2D.linearVelocity = new Vector2(movimientoHorizontal * velocity, mirigidbody2D.linearVelocity.y);

        GestionarOrientacion(movimientoHorizontal);

        // Actualiza la animación de correr solo cuando hay movimiento horizontal
        animator.SetBool("IsRunning", movimientoHorizontal != 0f);

        // para porcesar el salto
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, longitudraycast, capaSuelo);
        enSuelo = hit.collider != null;

        if (enSuelo && Input.GetKeyDown(KeyCode.Space))
        {
            mirigidbody2D.AddForce(new Vector2(0f, fuerzaSalto), ForceMode2D.Impulse);
        }
        animator.SetBool("enSuelo", enSuelo);
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
        Vector2 direccionGolpe;
        if (mirigidbody2D.linearVelocity.x > 0)
        {
            direccionGolpe = new Vector2(1, -1);
        }
        else
        {
            direccionGolpe = new Vector2(-1, 1);
        }
        mirigidbody2D.AddForce(direccionGolpe * fuerzaGolpe, ForceMode2D.Impulse);
        StartCoroutine(esperarYActivarMovimientos());
    }
    IEnumerator esperarYActivarMovimientos()
    {
        yield return new WaitForSeconds(1f);
        while(!enSuelo)
        {
            yield return null;
        }
        puedeMoverse = true;
    }
}