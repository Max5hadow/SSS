using System.Collections;
using UnityEngine;

public class SueloFalso : MonoBehaviour
{
    public float tiempoDeContacto = 1.5f;
    public Color colorDeParpadeo = Color.red;
    public Transform contactoSuperior; // Punto de contacto superior del suelo falso

    private bool sueloActivo = true;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && sueloActivo)
        {
            // Obtener la posición del jugador y la plataforma en el eje Y
            float jugadorPosY = collision.transform.position.y;
            float plataformaPosY = transform.position.y;

            // Verificar si el jugador ha colisionado por encima de la plataforma
            if (jugadorPosY > plataformaPosY)
            {
                // Desactivar el suelo falso
                sueloActivo = false;

                // Iniciar el temporizador de desaparición
                StartCoroutine(DesaparecerSuelo());
            }
        }
    }

    private IEnumerator DesaparecerSuelo()
    {
        // Cambiar el color de parpadeo
        spriteRenderer.color = colorDeParpadeo;

        // Esperar un tiempo antes de desaparecer
        yield return new WaitForSeconds(tiempoDeContacto);

        // Desactivar el renderizado del sprite
        spriteRenderer.enabled = false;

        // Desactivar la detección de colisiones
        boxCollider.enabled = false;
    }
}
