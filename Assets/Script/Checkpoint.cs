using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private PlayerController playerController;
    private bool isChecked = false;
    private SpriteRenderer spriteRenderer;

    public Color checkedColor = Color.green;
    public float respawnDelay = 2f;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isChecked)
        {
            isChecked = true;
            spriteRenderer.color = checkedColor;

            // Establecer la posición de respawn del jugador en la posición del checkpoint
            playerController.SetCheckpoint(transform.position);

            // Realizar acciones adicionales si es necesario, como reproducir un sonido o activar efectos visuales

            // Desactivar el collider del checkpoint después de un breve retraso
            DisableCollider();
        }
    }

    private void DisableCollider()
    {
        GetComponent<Collider2D>().enabled = false;
    }
}
