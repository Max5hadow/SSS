using UnityEngine;

public class VerticalPlatform : MonoBehaviour
{
    public float amplitude = 1.0f; // amplitud del movimiento vertical de la plataforma
    public float frequency = 1.0f; // frecuencia del movimiento vertical de la plataforma
    public float movementOffset = 0f; // desplazamiento inicial de la plataforma en el eje vertical
    public bool isMovingUp = true; // indica si la plataforma se está moviendo hacia arriba o no

    private Vector2 initialPosition;

    void Start()
    {
        // guarda la posición inicial de la plataforma
        initialPosition = transform.position;
    }

    void FixedUpdate()
    {
        // calcula la posición vertical de la plataforma utilizando una función sinusoidal
        float verticalOffset = Mathf.Sin(Time.time * frequency) * amplitude;
        Vector2 newPosition = new Vector2(initialPosition.x, initialPosition.y + movementOffset + verticalOffset);

        // mueve la plataforma a la nueva posición
        transform.position = newPosition;

        // cambia la dirección de movimiento cuando la plataforma alcanza un límite
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
