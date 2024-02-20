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
        // Reproducir la canción en bucle
        audioSource.loop = true;
        audioSource.Play();
    }

    // Llamado cuando se presiona el botón de mutear
    public void ToggleMute()
    {
        isMuted = !isMuted; // Cambiar el estado de silencio (mute)
        if (isMuted)
        {
            // Si está silenciado, detener la reproducción
            audioSource.Pause();
            // Desactivar el botón de mutear y activar el botón de sonido
            muteButton.gameObject.SetActive(false);
            soundButton.gameObject.SetActive(true);
        }
        else
        {
            // Si no está silenciado, reanudar la reproducción
            audioSource.UnPause();
            // Desactivar el botón de sonido y activar el botón de mutear
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
