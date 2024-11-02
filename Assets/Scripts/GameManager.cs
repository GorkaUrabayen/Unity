using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{

    public static GameManager Instance {get;private set;}
   
   public int PuntosTotales { get { return puntosTotales;}}
   public hud hud;
   
    private int puntosTotales ;
    private static int MAXVIDAS=3;
    private int vidas = 3;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }else
        {
            Debug.Log("Mas de un gameManager en esneca");
             Destroy(gameObject);
        }
    }

    public void sumarPuntos(int puntosAsumar)
    {
        puntosTotales += puntosAsumar;
        hud.actuallizarPuntos(PuntosTotales);
    }

 
    public void perderVidas()
    {
        vidas -= 1;
        if(vidas == 0)
        {
            //reunicar nivel
            SceneManager.LoadScene(0);
        }
        hud.desactivarVidas(vidas);


    }
     public bool RecuperarVIda()
    {
       if(vidas == MAXVIDAS)
       {
        return false;
       }

        hud.activarVida(vidas);
        {
        vidas += 1;
        }
        return true;
    }
   


    
}
