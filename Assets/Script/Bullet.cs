using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 2f;
    public int damage = 1;
    public PlayerController playerController;

    private Vector2 targetPosition;
    private Rigidbody2D rb;
    public Vector2 direction;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // Obtiene la posición actual del jugador
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            targetPosition = player.transform.position;
        }

       
    }

    void FixedUpdate()
    {
        // Calcula la dirección hacia el jugador
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;

        // Mueve la bala hacia la dirección calculada
        rb.velocity = direction * speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerController.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
