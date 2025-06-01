using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputTranslator
{
    // Controles táctiles (modificados por botones UI en Android)
    public static float customVertical = 0f;
    public static float customHorizontal = 0f;
    public static bool customJump = false;
    public static bool customFire = false;
    public static bool customCrouch = false;

    // Movimiento horizontal (teclado, mando o táctil)
    public static float Horizontal
    {
        //Getter para obtener el valor del movimiento horizontal
        get
        {
            return customHorizontal != 0f ? customHorizontal : Input.GetAxis("Horizontal");
        }
    }

    public static float Vertical
    {
        get
        {
            return customVertical != 0f ? customVertical : Input.GetAxisRaw("Vertical");
        }
    }

    // Salto (tecla, botón de mando o botón táctil)
    public static bool Jump
    {
        get
        {
            return Input.GetButtonDown("Jump") || customJump;
        }
    }

    // Disparo (tecla, botón de mando o botón táctil)
    public static bool Fire
    {
        get
        {
            return Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Fire1") || customFire;
        }
    }

    public static bool Crouch
    {
        get
        {
            return Input.GetAxisRaw("Vertical") < -0.5f || customCrouch;
        }
    }
}

