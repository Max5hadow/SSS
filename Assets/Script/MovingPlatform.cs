using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 2f; // velocidad de la plataforma
    public float leftLimit = 0f; // límite izquierdo de la plataforma
    public float rightLimit = 5f; // límite derecho de la plataforma
    private bool movingRight = true; // indica si la plataforma se está moviendo hacia la derecha o no

    void FixedUpdate()
    {
        // mueve la plataforma entre los límites izquierdo y derecho utilizando la función PingPong
        float xPosition = Mathf.PingPong(Time.time * speed, rightLimit - leftLimit) + leftLimit;
        transform.position = new Vector2(xPosition, transform.position.y);

        // cambia la dirección de movimiento cuando la plataforma alcanza un límite
        if (transform.position.x >= rightLimit && movingRight)
        {
            movingRight = false;
        }
        else if (transform.position.x <= leftLimit && !movingRight)
        {
            movingRight = true;
        }
    }
}
