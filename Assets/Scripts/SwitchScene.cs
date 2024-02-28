using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitch : MonoBehaviour
{

    // Método para cambiar de escena
    public void CambiarAEscena(string escena)
    {

        // Carga la escena con el nombre proporcionado
        SceneManager.LoadScene(escena);
    }
}