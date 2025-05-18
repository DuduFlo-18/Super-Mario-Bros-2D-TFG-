using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Implementamos dos clases para la logica del cursor en el menú, junto a los métodos necesarios
public class MenuCursor : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public GameObject icon;

    public void OnSelect(BaseEventData eventData)
    {
        icon.SetActive(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        icon.SetActive(false);
    }
}
