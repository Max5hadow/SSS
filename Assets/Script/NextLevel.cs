using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class NextLevel : MonoBehaviour
{
    public string nextLevelName; // Nombre de la escena del pr�ximo nivel
    public Animator transitionAnimator; // Referencia al componente Animator para reproducir la animaci�n de transici�n

    private bool levelCompleted = false; // Variable para asegurar que la transici�n se reproduzca solo una vez

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !levelCompleted)
        {
            levelCompleted = true;
            StartCoroutine(LoadNextLevel());
        }
    }

    private IEnumerator LoadNextLevel()
    {
        // Reproducir la animaci�n de transici�n
        if (transitionAnimator != null)
        {
            Debug.Log("AAA");
            transitionAnimator.SetTrigger("Start");
        }

        // Esperar un tiempo suficiente para que la animaci�n se reproduzca
        yield return new WaitForSeconds(0.7f); // Ajusta el tiempo de espera seg�n la duraci�n de tu animaci�n
        Debug.Log("Wait end");
        // Cargar la siguiente escena
        SceneManager.LoadScene(nextLevelName);
    }
}
