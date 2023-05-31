using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 1;

    private bool isTouchingPlayer = false;
    private PlayerController player;
    private SpriteRenderer spriteRenderer;
    private bool _facingRight = true;
    private int moveDirection = 1;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            rb.velocity = direction * speed;
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
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.TakeDamage(damage); // Agrega el argumento 'damage' al llamar a TakeDamage()
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
            player.TakeDamage(damage); // Agrega el argumento 'damage' al llamar a TakeDamage()
            yield return new WaitForSeconds(0.5f);
        }
    }
    private void Flip()
    {
        _facingRight = !_facingRight;
        float localScaleX = transform.localScale.x;
        localScaleX = localScaleX * -1f;
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }
}

