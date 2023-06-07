using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    private Animator animator;
    public InputHandler inputHandler;
    private Movement move;

    private bool facingRight = true;
    private Vector2 mouseDir;

    private int attackState;

    public int AttackState {
        get {
            return attackState;
        }
        
        set {
            attackState = value;
            animator.SetInteger("AttackState", attackState);
        }
    }

    void Awake() {
        animator = GetComponent<Animator>();
        move = GetComponent<Movement>();
    }

    void FixedUpdate() {
        CheckForFlip();
        setVelocity();
        setLastDir();
    }

    public void setAnimTrigger(string trigger) {
        animator.SetTrigger(trigger);
    }

    private void setVelocity() {
        animator.SetFloat("VelocityX", inputHandler.direction.x);
        animator.SetFloat("VelocityY", inputHandler.direction.y);
        animator.SetFloat("VelocityMagnitude", inputHandler.direction.magnitude);
    }

    public void setIsDashing(bool isDashing) {
        animator.SetBool("IsDashing", isDashing);
    }

    public void setLastDir() {
        Vector2 lastDirection = inputHandler.lastDirection;

        animator.SetFloat("LastDirX", lastDirection.x);
        animator.SetFloat("LastDirY", lastDirection.y); 
    }

    public void setLastMouseDir() {
        mouseDir = inputHandler.getMouseRelativeToPlayer();
        inputHandler.lastDirection = mouseDir;

        animator.SetFloat("MouseX", mouseDir.x);
        animator.SetFloat("MouseY", mouseDir.y);
    }

    private void CheckForFlip() {
        if (inputHandler.direction.x < 0 && AttackState <= 0 ||
            (inputHandler.lastDirection.x < 0 && move.dashCounter > 0) ||
            (mouseDir.x < 0 && AttackState > 0)) {
            if (facingRight) Flip();
        } else if (inputHandler.direction.x > 0 && AttackState <= 0 ||
            (inputHandler.lastDirection.x > 0 && move.dashCounter > 0) ||
            (inputHandler.lastDirection.x > 0 && AttackState > 0)) {
            if (!facingRight) Flip();
        }
    }

    private void Flip() {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        facingRight = !facingRight;
    }
}
