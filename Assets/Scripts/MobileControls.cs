using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobileControls : MonoBehaviour
{

    public GameObject crouchButton;
    public GameObject fireButton;
    // Esto se encarga de mostrar u ocultar el canvas táctil en función de la plataforma
     void Start()
     {
        #if !UNITY_ANDROID
            gameObject.SetActive(false); // Oculta el canvas táctil en PC
        #endif
    }

    // Hacemos que este script no se destruya al cambiar de escena
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    // Movimiento
    public void MoveLeftDown()
    {
        InputTranslator.customHorizontal = -1f;
        Debug.Log("Botón LEFT pulsado");
    }

    public void MoveRightDown()
    {
        InputTranslator.customHorizontal = 1f;
        Debug.Log("Botón RIGHT pulsado");
    }

    public void MoveRelease()
    {
        InputTranslator.customHorizontal = 0f;
        Debug.Log("Botón LEFT/RIGHT soltado");
    }

    // Salto
    public void JumpDown()
    {
        InputTranslator.customJump = true;
        Debug.Log("Botón JUMP pulsado");
    }

    public void JumpUp()
    {
        InputTranslator.customJump = false;
        Debug.Log("Botón JUMP soltado");
    }

    // Fuego
    public void FireDown()
    {
        InputTranslator.customFire = true;
        Debug.Log("Botón FIRE pulsado");
    }

    public void FireUp()
    {
        InputTranslator.customFire = false;
        Debug.Log("Botón FIRE soltado");
    }

    // Agacharse
    public void CrouchDown()
    {
        InputTranslator.customVertical = -1f;
        InputTranslator.customCrouch = true;
        Debug.Log("Botón CROUCH pulsado");
    }

    public void CrouchUp()
    {
        InputTranslator.customVertical = 0f;
        InputTranslator.customCrouch = false;
        Debug.Log("Botón CROUCH soltado");
    }
    
    // Actualiza el estado de los botones según el estado de Mario
    void Update()
    {
        if (Mario.instance == null) return;
        bool enableFire = Mario.instance.IsInFireMode();
        SetButtonState(fireButton, enableFire);
    }

    // Método para habilitar o deshabilitar los botones y cambiar su apariencia
    void SetButtonState(GameObject buttonObj, bool isEnabled)
    {
        // Fondo (componente Image del botón padre)
        Image background = buttonObj.GetComponent<Image>();
        if (background != null)
        {
            background.color = isEnabled ? Color.white : new Color(0.4f, 0.4f, 0.4f, 1f); // gris opaco
        }

        // Escala para dar feedback visual
        buttonObj.transform.localScale = isEnabled ? Vector3.one : new Vector3(0.9f, 0.9f, 1f);

        // Desactivar funcionalidad real (opcional)
        Button btn = buttonObj.GetComponent<Button>();
        if (btn != null)
        {
            btn.interactable = isEnabled;
        }
    }
}
