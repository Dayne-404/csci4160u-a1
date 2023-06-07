using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private DetectionZone detectionZone;
    private Collider2D coll;

    private bool IsAlive = true;
    private float attackCounter = 0;
    private float stunCounter = 0;

    [SerializeField] private Vector2 direction;
    [SerializeField] private float health;
    [SerializeField] private float damage;
    [SerializeField] private float knockBackForce;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float stunDuration;
    [SerializeField] private float despawnTimer;
    [SerializeField] private float attackTimer;

    public float Health {
        get {
            return health;
        }
        set {
            if (value < health) {
                anim.SetTrigger("IsHurt");
            }

            health = value;

            if (health <= 0) {
                IsAlive = false;
                anim.SetTrigger("IsDead");
            }
        }
    }

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        detectionZone = GetComponentInChildren<DetectionZone>();
    }

    void Start() {
        attackCounter = attackTimer;
    }

    void FixedUpdate() {
        if(IsAlive) {
            animate();
            move();
        } else {
            die();
        }
    }

    private void move() {
        anim.SetBool("IsMoving", false);

        if(stunCounter > 0) {
            stunCounter -= Time.deltaTime;
        }

        if (detectionZone.det != null && stunCounter <= 0) { 
            float dist = Vector2.Distance(detectionZone.det.transform.position, transform.position);

            if (dist > 1) {
                direction = (detectionZone.det.transform.position - transform.position).normalized;
                rb.velocity = (direction * moveSpeed);
                anim.SetBool("IsMoving", true);
            } else { //Enemy in range
                if (attackCounter > 0) {
                    attackCounter -= Time.deltaTime;
                }

                if (attackCounter <= 0) { //Attack Player
                    attackCounter = attackTimer;
                    attack(detectionZone.det.gameObject);
                    anim.SetTrigger("IsAttacking");
                }
            } 
        } 
    }

    private void attack(GameObject obj) {
        IDamageable damageableObject = obj.GetComponent<IDamageable>();

        if(damageableObject != null) {
            damageableObject.OnHit(damage);
            Debug.Log("Munch");
        } else {
            Debug.Log("Not Damageable!");
        }
    }

    private void animate() {
        anim.SetFloat("VelocityX", direction.x);
        anim.SetFloat("VelocityY", direction.y);

        if (!spriteRenderer.flipX && direction.x < 0) {
            spriteRenderer.flipX = true;
        } else if (spriteRenderer.flipX && direction.x > 0) {
            spriteRenderer.flipX = false;
        }
    }

    private void disableComponents() {
        if(spriteRenderer != null) spriteRenderer.sortingOrder = 0;
        if (coll != null) coll.enabled = false;
    }

    public void OnHit(float damage, Vector2 knockback) {
        Health -= damage;
        attackCounter = attackTimer;
        stunCounter = stunDuration;
        rb.AddForce(knockback);
    }

    public void OnHit(float damage) {
        Health -= damage;
    }

    private void die() {
        disableComponents();
        
        despawnTimer -= Time.deltaTime;
        spriteRenderer.color = new Color(1f, 1f, 1f, despawnTimer * 0.1f);

        if (despawnTimer <= 0) {
            Destroy(gameObject);
        }
    }
}
