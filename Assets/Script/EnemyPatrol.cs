using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
	public float speed = 1f;
	public float minX;
	public float maxX;
	public float waitingTime = 2f;

	private GameObject _target;
	private Animator _animator;
	private Weapon _weapon;
    private bool _facingRight = true;
    public GameObject player;
    private Vector2 targetPosition;
    public float detectionRange = 5f; // Rango de detección del jugador
    private PlayerController playerController;
	public int damage = 1;



    void Awake()
	{
		_animator = GetComponent<Animator>();
        _weapon = GetComponentInChildren<Weapon>();
		playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}

    // Start is called before the first frame update
    void Start()
	{
		UpdateTarget();
		StartCoroutine("PatrolToTarget");
	}

	// Update is called once per frame
	void Update()
	{
		if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Shooting"))
		{
			CanShoot();
        }
    }

    void FixedUpdate()
    {
        bool playerDetected = false;
        // Detectar al jugador en el rango de detección
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRange);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                // Desactiva la animación de ataque
                _animator.SetTrigger("Shoot");
            }
        }
        if (!playerDetected)
        {
            // Update animator
            _animator.SetBool("Idle", true);
        }

    }

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.CompareTag("Player"))
		{
			playerController.TakeDamage(damage);
		}
	}
	private void UpdateTarget()
	{
		// If first time, create target in the left
		if (_target == null) {
			_target = new GameObject("Target");
			_target.transform.position = new Vector2(minX, transform.position.y);
			transform.localScale = new Vector3(-1, 1, 1);
			return;
		}

		// If we are in the left, change target to the right
		if (_target.transform.position.x == minX) {
			_target.transform.position = new Vector2(maxX, transform.position.y);
			transform.localScale = new Vector3(1, 1, 1);
		}

		// If we are in the right, change target to the left
		else if (_target.transform.position.x == maxX) {
			_target.transform.position = new Vector2(minX, transform.position.y);
			transform.localScale = new Vector3(-1, 1, 1);
		}
	}

	private IEnumerator PatrolToTarget()
	{
		// Coroutine to move the enemy
		while (Vector2.Distance(transform.position, _target.transform.position) > 0.05f) {

			// Update animator
			_animator.SetBool("Idle", false);


			// let's move to the target
			Vector2 direction = _target.transform.position - transform.position;
			float xDirection = direction.x;

			transform.Translate(direction.normalized * speed * Time.deltaTime);

			// IMPORTANT
			yield return null;
		}

		// At this point, i've reached the target, let's set our position to the target's one
		Debug.Log("Target reached");
		transform.position = new Vector2(_target.transform.position.x, transform.position.y);
		UpdateTarget();

		// Update animator
		_animator.SetBool("Idle", true);
	



		// And let's wait for a moment
		Debug.Log("Waiting for " + waitingTime + " seconds");
        yield return new WaitForSeconds(waitingTime); // IMPORTANT

		
		// Obtiene la posición actual del jugador
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		if (player != null)
        {
            targetPosition = player.transform.position;
			// Shoot
			_animator.SetTrigger("Shoot");
		}

        // once waited, let's restore the patrol behaviour
        Debug.Log("Waited enough, let's update the target and move again");
		StartCoroutine("PatrolToTarget");
    }
	
    void CanShoot()
    {
        if (_weapon != null && player != null)
        {

			_weapon.Shoot();
		
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