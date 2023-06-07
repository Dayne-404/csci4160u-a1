using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxController : MonoBehaviour
{
    public float swordDamage = 1;
    public BoxCollider2D hitbox;
    public float knockBack = 100f;

    void FixedUpdate()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision) {
        IDamageable damageableObject = collision.GetComponent<IDamageable>();
        Vector2 mouseDirection = GetComponentInParent<InputHandler>().getMouseRelativeToPlayer();

        if(damageableObject != null) {
            damageableObject.OnHit(swordDamage, knockBack * mouseDirection);
            //damageableObject.OnHit(swordDamage);
        } 
    }
}
