using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class InputHandler : MonoBehaviour
{
    //Keyboard
    public float dashPressed;
    public Vector2 lastDirection;
    public Vector2 direction;

    //Mouse
    public float attackPressed;

    private float x;
    private float y;

    void Update() {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        dashPressed = Input.GetAxisRaw("Jump");
        attackPressed = Input.GetAxisRaw("Fire1");
    }

    void FixedUpdate() {
        movementCalculation(x, y);    
    }

    public Vector2 getMouseRelativeToPlayer() {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
    }

    private void movementCalculation(float x, float y) {
        if ((x == 0 && y == 0) && direction.x != 0 || direction.y != 0) {
            lastDirection = direction;
        }

        direction = new Vector2(x, y).normalized;
    }


}
