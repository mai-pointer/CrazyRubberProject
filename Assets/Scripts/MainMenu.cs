using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public AudioSource audioSource;
    public Button exitButton;
    public Button shopButton;
    public Button muteButton;
    public Button soundButton;
    public GameObject quitConfirmationPanel; // Panel de confirmación de salida
    private bool isMuted = false;
    private bool isPaused = false;

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
        // Mostrar el panel de confirmación de salida y pausar juego
        PauseGame();
        quitConfirmationPanel.SetActive(true);
    }

    public void ConfirmQuit()
    {
        Application.Quit();
    }

    public void CancelQuit()
    {
        // Ocultar el panel de confirmación de salida
        quitConfirmationPanel.SetActive(false);
    }

    public void PauseGame()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            Time.timeScale = 0f; // Pausar el juego estableciendo el tiempo a cero
        }
        else
        {
            Time.timeScale = 1f; // Reanudar el juego estableciendo el tiempo a su valor normal
        }
    }
}
