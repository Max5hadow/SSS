using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 2f; // velocidad de la plataforma
    public float leftLimit = 0f; // l�mite izquierdo de la plataforma
    public float rightLimit = 5f; // l�mite derecho de la plataforma
    private bool movingRight = true; // indica si la plataforma se est� moviendo hacia la derecha o no

    void FixedUpdate()
    {
        // mueve la plataforma entre los l�mites izquierdo y derecho utilizando la funci�n PingPong
        float xPosition = Mathf.PingPong(Time.time * speed, rightLimit - leftLimit) + leftLimit;
        transform.position = new Vector2(xPosition, transform.position.y);

        // cambia la direcci�n de movimiento cuando la plataforma alcanza un l�mite
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
