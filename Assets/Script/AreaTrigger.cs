using UnityEngine;
using UnityEngine.UI;

public class AreaTrigger : MonoBehaviour
{
    public Text text; // Referencia al objeto de texto que mostrará el mensaje

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Comprueba si el objeto que entró en el área es el jugador
        {
            text.gameObject.SetActive(true); // Activa el objeto de texto
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Comprueba si el objeto que salió del área es el jugador
        {
            text.gameObject.SetActive(false); // Desactiva el objeto de texto
        }
    }
}
