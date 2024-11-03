using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int PuntosTotales { get { return puntosTotales; } }
    public hud hud;

    private int puntosTotales;
    private static int MAXVIDAS = 3;
    private int vidas = 3;

    private const int NIVEL_OBJETIVO = 3; // Nivel 3 donde se verifica la condición
    private const int MONEDAS_OBJETIVO = 10; // Número de monedas necesarias

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Para mantener el GameManager entre escenas
        }
        else
        {
            Debug.Log("Más de un GameManager en escena");
            Destroy(gameObject);
        }
    }

   public void sumarPuntos(int puntosAsumar)
{
    puntosTotales += puntosAsumar;
    hud.actuallizarPuntos(PuntosTotales);

    // Verificar si estamos en el nivel 3 y hemos alcanzado las 20 monedas
    if (SceneManager.GetActiveScene().buildIndex == NIVEL_OBJETIVO && PuntosTotales >= MONEDAS_OBJETIVO)
    {
        CambiarNivel(4); // Cambia a la escena de Victoria (índice 4)
        return;
    }

    // Nueva verificación para cambiar a la siguiente escena cuando se alcanzan las 10 monedas
    if (puntosTotales >= 10)
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        // Verifica que la siguiente escena esté dentro del rango de índices válidos
        if (currentSceneIndex + 1 <= 3) // Cambiar solo a Nivel 1, Nivel 2, Nivel 3
        {
            CambiarNivel(currentSceneIndex + 1);
        }
    }
}

    private void CambiarNivel(int indice)
    {
        SceneManager.LoadScene(indice);
    }

    public void perderVidas()
    {
        vidas -= 1;
        if (vidas == 0)
        {
            // Reiniciar nivel
            SceneManager.LoadScene(5);
        }
        hud.desactivarVidas(vidas);
    }

    public bool RecuperarVIda()
    {
        if (vidas == MAXVIDAS)
        {
            return false;
        }

        hud.activarVida(vidas);
        vidas += 1;
        return true;
    }
}
