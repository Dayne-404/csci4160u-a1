using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //Componenets
    private Rigidbody2D rb;
    private InputHandler inputHandler;
    private AnimationHandler animationHandler;
    private CombatManager combatManager;

    //Dash
    private float activeMoveSpeed;
    private float dashCoolCounter;

    public float dashSpeed;
    public float dashLength = 0.5f;
    public float dashCooldown = 1f;
    public float dashCounter;

    //Attack
    public float slowModifier = 1f;

    //Mouse
    private Vector2 lastMouseDirection;

    [SerializeField] private float speed = 0.5f;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        animationHandler= GetComponent<AnimationHandler>();
        inputHandler = GetComponent<InputHandler>();
        combatManager = GetComponent<CombatManager>();
    }

    void Start()
    {
        activeMoveSpeed = speed;
    }

    private void FixedUpdate() 
    {
        if(combatManager.attackState <= 0 || combatManager.attackState >= 4) Dash();
        MovePlayer();
        animationHandler.setIsDashing(dashCounter > 0);
    }

    private void MovePlayer() {
        if(activeMoveSpeed > speed) {
            rb.velocity = lastMouseDirection * activeMoveSpeed;
        } else {
            rb.velocity = activeMoveSpeed * slowModifier * inputHandler.direction;
        }
    }

    private void Dash() {
        if (inputHandler.dashPressed > 0) {
            if (dashCounter <= 0 && dashCoolCounter <= 0) {
                lastMouseDirection = inputHandler.getMouseRelativeToPlayer().normalized;
                animationHandler.setLastMouseDir();
                activeMoveSpeed = dashSpeed;
                dashCounter = dashLength;
            }
        }

        if (dashCounter > 0) {
            dashCounter -= Time.deltaTime;

            if (dashCounter <= 0) {
                activeMoveSpeed = speed;
                dashCoolCounter = dashCooldown;
            }
        }

        if (dashCoolCounter > 0) {
            dashCoolCounter -= Time.deltaTime;
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.layer == 3) {
            Physics2D.IgnoreLayerCollision(7, 3);
        }
    }
}
