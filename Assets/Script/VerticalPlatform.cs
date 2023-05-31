using UnityEngine;

public class VerticalPlatform : MonoBehaviour
{
    public float amplitude = 1.0f; // amplitud del movimiento vertical de la plataforma
    public float frequency = 1.0f; // frecuencia del movimiento vertical de la plataforma
    public float movementOffset = 0f; // desplazamiento inicial de la plataforma en el eje vertical
    public bool isMovingUp = true; // indica si la plataforma se est� moviendo hacia arriba o no

    private Vector2 initialPosition;

    void Start()
    {
        // guarda la posici�n inicial de la plataforma
        initialPosition = transform.position;
    }

    void FixedUpdate()
    {
        // calcula la posici�n vertical de la plataforma utilizando una funci�n sinusoidal
        float verticalOffset = Mathf.Sin(Time.time * frequency) * amplitude;
        Vector2 newPosition = new Vector2(initialPosition.x, initialPosition.y + movementOffset + verticalOffset);

        // mueve la plataforma a la nueva posici�n
        transform.position = newPosition;

        // cambia la direcci�n de movimiento cuando la plataforma alcanza un l�mite
        if (transform.position.y >= initialPosition.y + amplitude && isMovingUp)
        {
            isMovingUp = false;
        }
        else if (transform.position.y <= initialPosition.y - amplitude && !isMovingUp)
        {
            isMovingUp = true;
        }
    }
}
