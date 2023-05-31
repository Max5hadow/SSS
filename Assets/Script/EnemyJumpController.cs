using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJumpController : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 1;
    public float leftLimit = -5f; // Límite izquierdo
    public float rightLimit = 5f; // Límite derecho

    public float detectionRange = 5f; // Rango de detección del jugador

    public float jumpForce = 5f; // Fuerza del salto
    public float jumpInterval = 2f; // Intervalo entre saltos
    public float jumpHeight = 1f; // Altura del salto

    private bool isTouchingPlayer = false;
    private bool canJump = true; // Variable para controlar si puede saltar
    private PlayerController player;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private bool isJumping = false; // Variable para controlar si el enemigo está saltando
    private float jumpTimer = 0f; // Temporizador para controlar el intervalo entre saltos
    private bool _facingRight = true;
    private Vector3 initialPosition; // Posición inicial del enemigo
    private int moveDirection = 1; // Dirección del movimiento: 1 para derecha, -1 para izquierda

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialPosition = transform.position;
    }

    void FixedUpdate()
    {
        // Movimiento horizontal
        rb.velocity = new Vector2(moveDirection * speed, rb.velocity.y);

        // Cambio de dirección al alcanzar los límites
        if (transform.position.x <= initialPosition.x + leftLimit)
        {
            moveDirection = 1;
            Flip();
        }
        else if (transform.position.x >= initialPosition.x + rightLimit)
        {
            moveDirection = -1;
            Flip();
        }

        // Detectar al jugador en el rango de detección
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRange);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                ChasePlayer();
            }
        }

        // Si puede saltar y no está saltando, y ha pasado el intervalo entre saltos, realizar un salto
        if (canJump && !isJumping && Time.time >= jumpTimer)
        {
            StartCoroutine(Jump());
        }
    }

    void ChasePlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        Vector2 direction = (player.transform.position - transform.position).normalized;

        // Mover al enemigo en la dirección del jugador
        rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);

        // Cambiar la dirección del enemigo según la posición relativa del jugador
        if (direction.x > 0 && moveDirection == -1)
        {
            moveDirection = 1;
            Flip();
        }
        else if (direction.x < 0 && moveDirection == 1)
        {
            moveDirection = -1;
            Flip();
        }
    }

    private void Flip()
    {
        _facingRight = !_facingRight;
        float localScaleX = transform.localScale.x;
        localScaleX = localScaleX * -1f;
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.TakeDamage(damage);
            Vector2 direction = (transform.position - collision.transform.position).normalized;
            rb.AddForce(direction * speed, ForceMode2D.Impulse);
            StartCoroutine(StopMovement());
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = true; // Habilitar el salto cuando colisiona con el suelo
        }
    }

    IEnumerator Jump()
    {
        canJump = false; // Deshabilitar el salto durante el salto actual
        isJumping = true; // Marcar que el enemigo está saltando

        float originalY = transform.position.y;
        float targetY = originalY + jumpHeight;

        while (transform.position.y < targetY)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            yield return null;
        }

        isJumping = false; // Marcar que el enemigo ha terminado de saltar

        jumpTimer = Time.time + jumpInterval; // Establecer el tiempo para el próximo salto
    }

    IEnumerator StopMovement()
    {
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.5f);
        rb.velocity = Vector2.zero;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isTouchingPlayer = true;
            player = other.GetComponent<PlayerController>();
            StartCoroutine(DamagePlayer());
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isTouchingPlayer = false;
            StopCoroutine(DamagePlayer());
        }
    }

    IEnumerator DamagePlayer()
    {
        while (isTouchingPlayer)
        {
            player.TakeDamage(damage);
            yield return new WaitForSeconds(0.5f);
        }
    }

    // Draw gizmo to show detection range
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
