using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDanger
{
    void DestroyPlayerOrNot();
    // Сделать отдельный массив опасных спрайтов, номера которых будут соответствовать соответствующим тайлам в безопасном массиве
}