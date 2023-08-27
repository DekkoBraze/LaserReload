using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;
using UnityEngine.UI;

public class RewardAdsManager : MonoBehaviour
{
    public YandexGame sdk;

    public void FulscreenAd()
    {
        sdk._FullscreenShow();
    }

    public void AdButton()
    {
        sdk._RewardedShow(1);
        //Messenger.Broadcast(GameEvent.OFF_MUSIC);
    }

    public void AdButtonCul()
    {
        Manager.link.SkipLevel();
    }
}
