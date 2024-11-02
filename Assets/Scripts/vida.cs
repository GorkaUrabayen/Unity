using UnityEngine;

public class vida : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D ohter)
    {
        if (ohter.gameObject.CompareTag("Player"))
        {
            bool vidaRecuperada = GameManager.Instance.RecuperarVIda();
            if (vidaRecuperada)
            {
              
                Destroy(this.gameObject);
                 
            }

        }
    }
}
