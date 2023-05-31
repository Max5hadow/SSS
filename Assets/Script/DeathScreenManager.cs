using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DeathScreenManager : MonoBehaviour
{
    public Animator deathScreenAnimator; // Referencia al Animator de la pantalla de muerte
    public Animator menuScreenAnimator; // Referencia al Animator de la pantalla del menú

    public void ShowDeathScreen()
    {
        StartCoroutine(ShowDeathScreenWithAnimation());
    }

    private System.Collections.IEnumerator ShowDeathScreenWithAnimation()
    {
        // Activar la animación de la pantalla de muerte
        deathScreenAnimator.SetTrigger("Start");

        // Esperar un breve período de tiempo para que la animación se reproduzca
        yield return new WaitForSeconds(1f);

        // Almacenar el nombre de la escena anterior
        SceneManagerHelper.previousSceneName = SceneManager.GetActiveScene().name;

        // Cargar la escena de la pantalla de muerte
        SceneManager.LoadScene("DeathScreen");
    }

    public void Retry()
    {
        // Cargar la escena anterior por su nombre almacenado
        SceneManager.LoadScene(SceneManagerHelper.previousSceneName);
    }

    public void BackToMenu()
    {
        StartCoroutine(BackToMenuWithAnimation());
    }

    private System.Collections.IEnumerator BackToMenuWithAnimation()
    {
        // Activar la animación de la pantalla del menú
        menuScreenAnimator.SetTrigger("Start");

        // Esperar un breve período de tiempo para que la animación se reproduzca
        yield return new WaitForSeconds(1f);

        // Cargar la escena del menú principal por su nombre
        SceneManager.LoadScene("Menú");
    }
}
