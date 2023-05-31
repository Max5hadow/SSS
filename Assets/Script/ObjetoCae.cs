using UnityEngine;

public class ObjetoCae : MonoBehaviour
{
    public float fallSpeed = 5f; // Velocidad de caída del enemigo
    public int damage = 10; // Cantidad de daño que el enemigo inflige al jugador

    private PlayerController jugadorController;
    private bool isFalling = false;
    private bool isPlayerDetected = false;

    private void Start()
    {
        jugadorController = FindObjectOfType<PlayerController>(); // Buscar el objeto del jugador por su etiqueta
    }

    private void Update()
    {
        if (!isFalling && !isPlayerDetected)
        {
            // Verificar si el jugador está debajo y cerca en la coordenada X del enemigo
            if (jugadorController.transform.position.y < transform.position.y &&
                Mathf.Abs(jugadorController.transform.position.x - transform.position.x) < 2f)
            {
                isFalling = true;
            }
        }
        else if (isFalling)
        {
            // Calcular el movimiento de caída
            Vector2 fallMovement = new Vector2(0f, -fallSpeed * Time.deltaTime);
            transform.Translate(fallMovement);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificar si el enemigo ha hecho contacto con el jugador
        if (other.CompareTag("Player"))
        {
            isPlayerDetected = true;

            // Llamar a la función TakeDamage() del script PlayerController para aplicar el daño al jugador
            jugadorController.TakeDamage(damage);

            // Desaparecer el enemigo
            Destroy(gameObject);
        }

        // Verificar si el enemigo ha hecho contacto con el suelo
        if (isFalling && other.CompareTag("Ground"))
        {
            // Desaparecer el enemigo
            Destroy(gameObject);
        }
    }
}
