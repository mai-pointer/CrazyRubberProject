using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public AudioSource audioSource;
    public Button exitButton;
    public Button muteButton;
    public Button soundButton;
    private bool isMuted = false;

    void Start()
    {
        // Reproducir la canci�n en bucle
        audioSource.loop = true;
        audioSource.Play();
    }

    // Llamado cuando se presiona el bot�n de mutear
    public void ToggleMute()
    {
        isMuted = !isMuted; // Cambiar el estado de silencio (mute)
        if (isMuted)
        {
            // Si est� silenciado, detener la reproducci�n
            audioSource.Pause();
            // Desactivar el bot�n de mutear y activar el bot�n de sonido
            muteButton.gameObject.SetActive(false);
            soundButton.gameObject.SetActive(true);
        }
        else
        {
            // Si no est� silenciado, reanudar la reproducci�n
            audioSource.UnPause();
            // Desactivar el bot�n de sonido y activar el bot�n de mutear
            soundButton.gameObject.SetActive(false);
            muteButton.gameObject.SetActive(true);
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void PlayShop()
    {
        SceneManager.LoadScene("Tienda");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
