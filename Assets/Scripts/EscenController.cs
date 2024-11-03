using UnityEngine;
using UnityEngine.SceneManagement;
public class EscenController : MonoBehaviour
{
     public bool pasarNivel;
    private int indiceNivel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         if(pasarNivel)
        {
            CambiarNivel(indiceNivel);
        }
    }
    public void CambiarNivel(int indice)
    {
        SceneManager.LoadScene(indice);
    }
    
   

}
       




