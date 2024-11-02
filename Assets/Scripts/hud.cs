using TMPro;
using UnityEngine;

public class hud : MonoBehaviour
{

    public TextMeshProUGUI puntos;    // Reference to the TextMeshProUGUI component
    public GameObject[] vidas;
    void Start()
    {
        // Check if gameManager is assigned
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager is not assigned in the hud script!");
        }

        // Check if puntos is assigned
        if (puntos == null)
        {
            Debug.LogError("TextMeshProUGUI 'puntos' is not assigned in the hud script!");
        }
    }
    public void actuallizarPuntos(int puntosTotales)
    {
        puntos.text = puntosTotales.ToString();
    }

    public void desactivarVidas(int indice)
    {
        vidas[indice].SetActive(false);

    }
    public void activarVida(int indice)
    {
        vidas[indice].SetActive(true);
    }


}
