using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button exitButton;
    public Button shopButton;
    public Button muteSoundOn, muteSoundOff;
    public Button muteMusicOn, muteMusicOff;

    public GameObject quitConfirmationPanel; // Panel de confirmación de salida
    public GameObject instructionPanel;
    public bool sonidos = false;
    private bool isPaused = false;

    void Start()
    {
        Time.timeScale = 1;

        if (sonidos)
        {
            muteMusicOn.gameObject.SetActive(Save.Data.musica.estado);
            muteMusicOff.gameObject.SetActive(!Save.Data.musica.estado);

            muteSoundOn.gameObject.SetActive(Save.Data.sonidos.estado);
            muteSoundOff.gameObject.SetActive(!Save.Data.sonidos.estado);
        }
      
    }

    // Llamado cuando se presiona el botón de mutear
    public void ToggleSound(bool muteado)
    {
        Sonidos.Estado(TipoSonido.sonidos, muteado);

        muteSoundOn.gameObject.SetActive(muteado);
        muteSoundOff.gameObject.SetActive(!muteado);
    }
    public void TogglMusic(bool muteado)
    {
        Sonidos.Estado(TipoSonido.musica, muteado);

        muteMusicOn.gameObject.SetActive(muteado);
        muteMusicOff.gameObject.SetActive(!muteado);
    }


    public void PlayGame()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void PlayMenu()
    {
        SceneManager.LoadScene("UIMainMenu");
    }
    public void RetryGame()
    {
        SceneManager.LoadScene("MainScene");
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
        // Desactivar el botón de salir
        exitButton.gameObject.SetActive(false);
    }

    public void ShowInstructions()
    {
        instructionPanel.SetActive(true);
        // Desactivar el botón de salir
        exitButton.gameObject.SetActive(false);
    }
    public void QuitIntructions()
    {
        instructionPanel.SetActive(false);
        exitButton.gameObject.SetActive(true);
    }

    public void ConfirmQuit()
    {
        Application.Quit();
    }

    public void CancelQuit()
    {
        // Ocultar el panel de confirmación de salida y reanudar juego
        PauseGame();
        quitConfirmationPanel.SetActive(false);
        // Activar el botón de salir
        exitButton.gameObject.SetActive(true);
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
