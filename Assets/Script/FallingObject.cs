using UnityEngine;

public class FallingObject : MonoBehaviour
{
    public float fallSpeed = 3f; // velocidad de caída del objeto
    public float riseSpeed = 1f; // velocidad de elevación del objeto
    private Rigidbody2D rb; // referencia al Rigidbody2D del objeto
    private Vector2 initialPosition; // posición inicial del objeto
    private bool isFalling = true; // indica si el objeto está cayendo o no

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
    }

    void Update()
    {
        // verifica si el objeto ha llegado al fondo
        if (transform.position.y < initialPosition.y)
        {
            // aplica la gravedad al objeto
            rb.gravityScale = 1f;
        }

        // verifica si el objeto ha vuelto a su posición inicial después de caer
        if (transform.position.y >= initialPosition.y && !isFalling)
        {
            // detiene el objeto y comienza a elevarse lentamente
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0f;
            transform.position = Vector2.MoveTowards(transform.position, initialPosition, Time.deltaTime * riseSpeed);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // detiene el objeto y comienza a elevarse lentamente
            isFalling = false;
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0f;
            transform.position = new Vector2(transform.position.x, collision.contacts[0].point.y + GetComponent<SpriteRenderer>().bounds.size.y / 2f);
        }
    }
}
