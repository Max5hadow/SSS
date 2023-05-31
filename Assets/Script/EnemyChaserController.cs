using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaserController : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 1;
    public float leftLimit = -5f; // Límite izquierdo
    public float rightLimit = 5f; // Límite derecho

    public float detectionRange = 5f; // Rango de detección del jugador

    private bool isTouchingPlayer = false;
    private PlayerController player;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
      private bool isAttacking = false;
    private Vector3 initialPosition; // Posición inicial del enemigo
    private int moveDirection = 1; // Dirección del movimiento: 1 para derecha, -1 para izquierda
    private Animator animator;
    private bool _facingRight = true;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialPosition = transform.position;
        animator = GetComponent<Animator>();

    }

    void FixedUpdate()
    {
        bool playerDetected = false;
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
        if (!playerDetected)
        {
            // Desactiva la animación de ataque
            animator.SetBool("IsAttacking", false);
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

        // Activa la animación de ataque
        animator.SetBool("IsAttacking", true);
    }
    private void Flip()
    {
        _facingRight = !_facingRight;
        float localScaleX = transform.localScale.x;
        localScaleX = localScaleX * -1f;
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }
  
    void LateUpdate()
    {
        animator.SetBool("IsAttacking", isAttacking);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.TakeDamage(damage); // Agregar el argumento 'damage'
            Vector2 direction = (transform.position - collision.transform.position).normalized;
            rb.AddForce(direction * speed, ForceMode2D.Impulse);
            StartCoroutine(StopMovement());
        }
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
            player.TakeDamage(damage); // Agregar el argumento 'damage'
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