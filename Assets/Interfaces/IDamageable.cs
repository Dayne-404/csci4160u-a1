using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public interface IDamageable {
    float Health { set; get; }

    public void OnHit(float damage, Vector2 knockback);
    public void OnHit(float damage);
}