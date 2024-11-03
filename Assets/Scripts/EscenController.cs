using UnityEngine;
using UnityEngine.SceneManagement;

public class EscenController : MonoBehaviour
{
    public static EscenController instance; // Singleton
    public bool pasarNivel;
    public int indiceNivel;

    void Awake()
    {
        // Verificar si ya existe una instancia de este objeto
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Mantener este objeto al cambiar de escena
        }
        else
        {
            Destroy(gameObject); // Destruir duplicados
        }
    }

    void Update()
    {
        // Cambia de nivel si se cumple la condición
        if (pasarNivel)
        {
            CambiarNivel(indiceNivel);
            pasarNivel = false; // Reinicia para evitar múltiples cambios
        }
    }

    public void CambiarNivel(int indice)
    {
        SceneManager.LoadScene(indice);
    }

    // Método para configurar el índice del nivel y activar el cambio de nivel
    public void PrepararCambioNivel(int nuevoIndice)
    {
        indiceNivel = nuevoIndice;
        pasarNivel = true;
    }
}
