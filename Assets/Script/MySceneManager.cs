using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class MySceneManager : MonoBehaviour
{
    public Animator loadingScreenAnimator; // Referencia al Animator de la pantalla de carga

    public void LoadLevel(string levelName)
    {
        StartCoroutine(LoadLevelWithAnimation(levelName));
    }

    private IEnumerator LoadLevelWithAnimation(string levelName)
    {
        // Activar la animación de la pantalla de carga
        loadingScreenAnimator.SetTrigger("Start");

        // Esperar un breve período de tiempo para que la animación se reproduzca
        yield return new WaitForSeconds(1f);

        // Cargar el nivel
        SceneManager.LoadScene(levelName);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
