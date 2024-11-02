using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour // Cambiado a AudioManager
{
        public static AudioManager Instance{get;private set;}
        private AudioSource audioSource;

      

    private void Awake(){
        if(Instance == null)
        {
            Instance = this;
        }else
        {
            Debug.Log("Cuidado! MAs de un AudioManager en escena");
             Destroy(gameObject);
        }
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void ReproducirSonido(AudioClip audio)
    {
        audioSource.PlayOneShot(audio); // Reproducir el sonido pasado como parámetro
    }
}
