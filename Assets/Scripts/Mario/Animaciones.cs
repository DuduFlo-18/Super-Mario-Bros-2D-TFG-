using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// Este script interactua con el Animator de Mario para controlar las animaciones del personaje (Animator es una parte gráfica de Unity que permite gestionar animaciones de forma sencilla con variables y condiciones).
public class Animaciones : MonoBehaviour
{
    //Objecto del Animator (Parte Grafica en Unity) que controla las animaciones del personaje Mario
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Grounded(bool isGrounded)
    {
        animator.SetBool("Grounded", isGrounded);
    }

    public void Velocity(float velocityX)
    {
        animator.SetFloat("VelocityX", Mathf.Abs(velocityX));
    }

    public void Jumping(bool isJumping)
    {
        animator.SetBool("Jumping", isJumping);
    }

    public void Skid(bool isSkidding)
    {
        animator.SetBool("Skid", isSkidding);
    }

    public void Dead()
    {
        animator.SetTrigger("Dead");
    }

// Cambia el estado de Mario (0: Normal, 1: Super, 2: Fuego)
    public void NewState(int state)
    {
        animator.SetInteger("State", state);
    }

    public void PowerUp()
    {
        animator.SetTrigger("PowerUp");
    }

    public void Hit()
    {
        animator.SetTrigger("Hit");
    }

    public void Shoot()
    {
        animator.SetTrigger("Shoot");
    }

    public void InvincibleMode(bool activate)
    {
        animator.SetBool("Invincible", activate);
    }

    public void Hurt(bool activate)
    {
        animator.SetBool("Hurt", activate);
    }

    public void Crouch(bool activate)
    {
        animator.SetBool("Crouched", activate);
    }

    public void Climb(bool activate)
    {
        animator.SetBool("Climb", activate);
    }

    public void Pause()
    {
        animator.speed = 0;
    }

    public void Continue()
    {
        animator.speed = 1;
    }

    // Reinicia el estado del animator (todos los parámetros y triggers)
    public void Reset()
    {
        animator.SetBool("Grounded", false);
        animator.SetBool("Jumping", false);
        animator.SetFloat("VelocityX", 0);
        animator.SetBool("Skid", false);
        animator.SetBool("Invincible", false);
        animator.SetBool("Hurt", false);
        animator.SetBool("Crouched", false);
        animator.SetBool("Climb", false);

        animator.ResetTrigger("PowerUp");
        animator.ResetTrigger("Hit");
        animator.ResetTrigger("Shoot");
        animator.ResetTrigger("Dead");

        animator.SetInteger("State", 0);
        animator.Play("States");
    }



}
