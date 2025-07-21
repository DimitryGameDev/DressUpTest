using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Отвечает за очистку всех видов макияжа (тени, румяна, помада) при нажатии на спонж.
/// </summary>
public class MakeupResetter : MonoBehaviour
{
    [Header("Makeup Objects to Clear")]
    [Tooltip("Все объекты макияжа, которые нужно отключить при очистке")]  
    public List<GameObject> makeupObjects;

   

    /// <summary>
    /// Метод для OnClick кнопки-спонжа.
    /// Отключает все объекты макияжа
    /// </summary>
    public void ClearAllMakeup()
    {
        // Скрываем все объекты макияжа
        foreach (var obj in makeupObjects)
        {
            if (obj != null)
                obj.SetActive(false);
        }
     
       
    }
}

