using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerHUD : MonoBehaviour
{
    public Slider healthSlider;
    public Slider attackCooldownSlider;

    public void setMaxCooldown(float cooldown) {
        attackCooldownSlider.maxValue = cooldown;
    }

    public void setCooldown(float cooldown) {
        attackCooldownSlider.value = cooldown;
    }

    public void setMaxHealth(float health) {
        healthSlider.maxValue = health;
        healthSlider.value = health;
    }

    public void SetHealth(float health) {
        healthSlider.value = health;
    }
}
