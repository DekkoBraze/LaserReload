using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Danger : IDanger
{
    public void DestroyPlayerOrNot()
    {
        Manager.link.OnPlayerDestroy();
        Manager.playerLink.PlayerDestroy();
    }
}
