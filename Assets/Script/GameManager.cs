using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public CanvasGroup pauseMenuCanvasGroup;
    public Animator loadingScreenAnimator; // Referencia al Animator de la pantalla de carga

    private bool isPaused = false;

    private void Start()
    {
        pauseMenuCanvasGroup.alpha = 0;
    }

    private void Update()
    {
        if (isPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    public void LoadLevel(string levelName)
    {
        StartCoroutine(LoadLevelWithAnimation(levelName));
    }

    private System.Collections.IEnumerator LoadLevelWithAnimation(string levelName)
    {
        // Reanudar el juego despu�s de cargar el nivel
        isPaused = false;
        pauseMenuCanvasGroup.alpha = 0;
        pauseMenuCanvasGroup.blocksRaycasts = false;
        
        // Activar la animaci�n de la pantalla de carga
        loadingScreenAnimator.SetTrigger("Start");

        // Esperar un breve per�odo de tiempo para que la animaci�n se reproduzca
        yield return new WaitForSeconds(1f); // Ajusta el tiempo seg�n la duraci�n de tu animaci�n

        // Cargar el nivel
        SceneManager.LoadScene(levelName);
    }

    public void TogglePauseMenu()
    {
        if (isPaused)
        {
            isPaused = false;
            pauseMenuCanvasGroup.alpha = 0;
            pauseMenuCanvasGroup.blocksRaycasts = false;
        }
        else
        {
            isPaused = true;
            pauseMenuCanvasGroup.alpha = 1;
            pauseMenuCanvasGroup.blocksRaycasts = true;
        }
    }
}
