using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CombatManager : MonoBehaviour, IDamageable
{
    private Movement movement;
    private InputHandler inputHandler;
    private AnimationHandler animationHandler;
    private Collider2D collide;
    public playerHUD playerHud;

    public int attackState = 0;
    private float attackCoolCounter = 0f;
    private float health;
    private float respawnCounter;
    private bool isAlive;

    [SerializeField] Vector2 respawnPoint = Vector2.zero;
    [SerializeField] float respawnTimer;
    [SerializeField] float attackCooldown = 0.7f;
    [SerializeField] float maxHealth;
    [SerializeField] float attackMovementPause = 0.3f;

    public float Health {
        get {
            return health;
            
        }
        set {
            health = value;
            
            playerHud.SetHealth(health);
            if (health <= 0) {
                isAlive = false;
                animationHandler.setAnimTrigger("IsAlive");

                movement.enabled = false;
                collide.enabled = false;
                animationHandler.enabled = false;
            }
        }
    }

    void Awake() {
        inputHandler = GetComponent<InputHandler>();
        animationHandler= GetComponent<AnimationHandler>();
        movement = GetComponent<Movement>();
        collide = GetComponent<Collider2D>();
    }

    void FixedUpdate() 
    {
        if(isAlive) {
            if (movement.dashCounter <= 0) ComboAttack();
        } else {
            onDeath();
        }   
    }

    void Start() {
        Health = maxHealth;
        attackCoolCounter = attackCooldown;
        animationHandler.AttackState = attackState;
        respawnCounter = respawnTimer;
        playerHud.setMaxHealth(maxHealth);
        playerHud.setMaxCooldown(attackCooldown);
        isAlive = true;
    }

    private void ComboAttack() {
        //Combat Improved?? Tldr. No.. just gave me headaces
        if(attackCoolCounter > 0) {
            attackCoolCounter -= Time.deltaTime;
            playerHud.setCooldown(attackCoolCounter);
        }

        if (inputHandler.attackPressed > 0 && (
            (attackCoolCounter <= 0 && attackState == 0) || 
            (attackState < 4 && attackCoolCounter > 0 && attackCoolCounter <= attackCooldown*0.4))) {
            attackState++;
            animationHandler.setLastMouseDir();
            attackCoolCounter = attackCooldown;
        }

        if ((attackState != 0 && attackCoolCounter <= 0) || attackState >= 4) {
            attackCoolCounter = attackCooldown * 0.5f;
            attackState = 0;
        }

        if (attackCoolCounter > attackMovementPause && attackState != 0) {
            movement.slowModifier = 0.1f;
        } else {
            movement.slowModifier = 1f;
        }


        animationHandler.AttackState = attackState;        
    }

    public void onDeath() {
        if(!isAlive) {
            respawnCounter -= Time.deltaTime;

            if(respawnCounter <= 0) { 
                isAlive = true;
                collide.enabled = true;
                movement.enabled = true;
                animationHandler.enabled = true;

                animationHandler.setAnimTrigger("Respawn");

                transform.position = respawnPoint;
                Health = maxHealth;
                respawnCounter = respawnTimer;
            }
        }
    }

    public void OnHit(float damage, Vector2 knockback) {
        Health -= damage;
        animationHandler.setAnimTrigger("IsHurt");
    }

    public void OnHit(float damage) {
        animationHandler.setAnimTrigger("IsHurt");
        Health -= damage;
    }
}


/*
 * First Iteration of thhe combatManager Script
        if (attackCoolCounter > 0) {
            attackCoolCounter -= Time.deltaTime;

            if(attackCoolCounter < 0 && attackState > 0) {
                attackCoolCounter = attackCooldown;
                attackState = 0;
            }
        }

        if (ih.attackPressed > 0) {
            if(attackCoolCounter <= 0 && attackState == 0) {
                attackCoolCounter = attackCooldown;
                attackState++;
            } else if (attackCoolCounter > 0 && attackCoolCounter <= minComboTime && attackState < 3) {
                attackCoolCounter = attackCooldown;
                attackState++;
            }

            lastMouseDirection = ih.mouseRelativeToPlayer;
            ih.lastMoveDirection = ih.mouseRelativeToPlayer;
        }

        if (attackCoolCounter > attackMovementPause && attackState != 0) {
            movement.slowModifier = 0.1f;
        } else {
            movement.slowModifier = 1f;
        }
        */
