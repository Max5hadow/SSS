using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public int health = 3;
    public int maxHealth;
    public float invulnerabilityTime = 1f;
    public Color damageColor = Color.red;
    private Animator _animator;
    public int damage = 1;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool isInvulnerable = false;
    public float jumpForce = 7f; // fuerza de salto del jugador
    public float airJumpForce = 5f; // fuerza de salto en el aire del jugador
    private bool _facingRight = true;
    private int respawnCount = 0;
    public int maxRespawns = 3;
    public ReaparicionesIndicador reaparicionesIndicador;
    public LifeIndicator lifeIndicator;
    private bool isGrounded; // indica si el jugador está tocando el suelo
    private int airJumpsLeft = 1; // cantidad de saltos en el aire que quedan
    private Vector2 _movement;
    public float runSpeed = 8f;
    public KeyCode runKey = KeyCode.LeftShift;
    public float maxRunTime = 5f;
    private float currentRunTime = 0f;
    public float waitTimeAfterRun = 2f;
    private bool isWaiting = false;
    private bool isCrouching = false;
    public KeyCode crouchKey = KeyCode.DownArrow;
    private bool isSprinting = false;
    public float sprintSpeedMultiplier = 2f;
    private string sprintParameter = "IsSprinting";
    private Vector3 respawnPosition; // Posición de respawn actual
    private Vector3 initialPosition; // Posición inicial del jugador

    void Start()
    {
        maxHealth = health;
        currentRunTime = maxRunTime;
        spriteRenderer = GetComponent<SpriteRenderer>();
        lifeIndicator.UpdateLife(health, maxHealth);
        initialPosition = transform.position;
        respawnPosition = initialPosition;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        _movement = new Vector2(horizontalInput, 0f);
        float moveSpeed = speed;


        if (Input.GetKey(runKey) && !isWaiting && currentRunTime > 0f && !isSprinting)
        {
            moveSpeed *= sprintSpeedMultiplier;
            currentRunTime -= Time.deltaTime;

            if (currentRunTime <= 0f)
            {
                currentRunTime = 0f;
                isWaiting = true;
                moveSpeed = speed; // Restablecer la velocidad de movimiento normal
            }
            else
            {
                isSprinting = true;
            }
        }
        else
        {
            if (currentRunTime < maxRunTime)
            {
                currentRunTime += Time.deltaTime;
                isWaiting = false;
            }

            if (isSprinting)
            {
                // Realiza acciones adicionales cuando se desactiva el sprint, como reducir la velocidad de movimiento
                moveSpeed = speed; // Restablecer la velocidad de movimiento normal
                isSprinting = false;
            }
        }
        if (isSprinting != _animator.GetBool(sprintParameter))
        {
            _animator.SetBool(sprintParameter, isSprinting);
        }

        // Flip character
        if (horizontalInput < 0f && _facingRight == true)
        {
            Flip();
        }
        else if (horizontalInput > 0f && _facingRight == false)
        {
            Flip();
        }
        if (isCrouching)
        {
            // Aplica una velocidad de movimiento reducida mientras está agachado
            transform.position += (Vector3)_movement * moveSpeed * 0.5f * Time.deltaTime;
        }
        else
        {
            // Aplica la velocidad de movimiento normal
            transform.position += (Vector3)_movement * moveSpeed * Time.deltaTime;
        }

        if (Input.GetKey(crouchKey))

        {
            isCrouching = true;
            // Aquí puedes realizar acciones adicionales, como cambiar el tamaño del collider del personaje para simular el agachamiento.
        }
        else
        {
            isCrouching = false;
            // Aquí puedes realizar acciones adicionales para restaurar el tamaño original del collider del personaje.
        }
        // salto del jugador
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                // salto desde el suelo
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                isGrounded = false;
                airJumpsLeft = 1;
            }
            else if (airJumpsLeft > 0)
            {
                // salto en el aire
                rb.velocity = new Vector2(rb.velocity.x, airJumpForce);
                airJumpsLeft--;
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKey(runKey) && !isWaiting && currentRunTime > 0f)
        {
            moveSpeed = runSpeed;
            currentRunTime -= Time.deltaTime;

            if (currentRunTime <= 0f)
            {
                currentRunTime = 0f;
                isWaiting = true;
                // Realizar acciones adicionales, como reducir la velocidad de movimiento
                moveSpeed = speed; // Restablecer la velocidad de movimiento normal
            }
        }
        else
        {
            if (currentRunTime < maxRunTime)
            {
                currentRunTime += Time.deltaTime;
                isWaiting = false;
                // Realizar acciones adicionales, como permitir que el jugador vuelva a correr
            }
        }

        if (isWaiting)
        {
            waitTimeAfterRun -= Time.deltaTime;

            if (waitTimeAfterRun <= 0f)
            {
                waitTimeAfterRun = 0f;
                isWaiting = false;
                // Aquí puedes realizar alguna acción cuando el tiempo de espera termine, como permitir que el jugador vuelva a correr.
            }
        }

        if (isInvulnerable)
        {
            return;
        }
        if (health <= 0)
        {
            if (respawnCount < maxRespawns)
            {
                Respawn();
            }
            else
            {
                Die();
            }
            return;
        }
    }

    void FixedUpdate()
    {
        float horizontalVelocity = _movement.normalized.x * speed;
        rb.velocity = new Vector2(horizontalVelocity, rb.velocity.y);
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
        _animator.SetBool("Idle", _movement == Vector2.zero);
        _animator.SetBool("IsGrounded", isGrounded);
        _animator.SetBool("IsCrouching", isCrouching);
        _animator.SetBool("IsSprinting", isSprinting);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // verifica si el jugador está tocando el suelo
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;
            airJumpsLeft = 1;

        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (isInvulnerable)
        {
            return;
        }

        health -= damageAmount;
        StartCoroutine(InvulnerabilityCoroutine());
        StartCoroutine(DamageColorCoroutine());

        if (health <= 0)
        {
            if (respawnCount < maxRespawns)
            {
                Respawn();
            }
            else
            {
                Die();
            }
        }

        lifeIndicator.UpdateLife(health, maxHealth);
    }

    IEnumerator InvulnerabilityCoroutine()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityTime);
        isInvulnerable = false;
    }

    IEnumerator DamageColorCoroutine()
    {
        spriteRenderer.color = damageColor;
        yield return new WaitForSeconds(invulnerabilityTime);
        spriteRenderer.color = Color.white;
    }

    void Respawn()
    {
        respawnCount++;
        health = maxHealth; // Restablecer la salud al máximo al reaparecer
        lifeIndicator.UpdateLife(health, maxHealth);
        reaparicionesIndicador.ActualizarReaparicionesRestantes(maxRespawns - respawnCount);
        transform.position = respawnPosition;   
    }

    public void SetCheckpoint(Vector3 checkpointPosition)
    {
        respawnPosition = checkpointPosition;
    }

    // Método para restablecer la posición de respawn a la posición inicial
    public void ResetRespawnPosition()
    {
        respawnPosition = initialPosition;
    }

    void Die()
    {
        DeathScreenManager deathScreenManager = FindObjectOfType<DeathScreenManager>();

        if (deathScreenManager != null)
        {
            deathScreenManager.ShowDeathScreen();
        }
    }
}