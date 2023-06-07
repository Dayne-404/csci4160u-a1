using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour, IDamageable
{
    private Animator anim;
    private Collider2D coll;
    private SpriteRenderer spriteRenderer;
    public float despawnTimer = 15f;

    private bool isAlive = true;

    public float Health {
        get {
            return _health;
        }
        set {
            _health = value;

            if (_health <= 0) {
                isAlive = false;
                
                if (anim != null) {
                    anim.SetBool("IsAlive", isAlive);
                }

                spriteRenderer.sortingOrder = 0;
                coll.enabled = false;
            }
        }
    }

    float _health = 1f;

    void Awake() {
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate() { 
        if(!isAlive) {
            despawnTimer -= Time.deltaTime;
            spriteRenderer.color = new Color(1f, 1f, 1f, despawnTimer*0.1f);
            
            if(despawnTimer <= 0) {
                Destroy(gameObject);
            }
        }
    }

    private void DisableComponents() {
        if(spriteRenderer != null) spriteRenderer.sortingOrder = 0;
        if(coll != null) coll.enabled = false;
    }

    public void OnHit(float damage, Vector2 knockback) {
        Health -= damage;
    }

    void IDamageable.OnHit(float damage) {
        Health -= damage;
    }
}
