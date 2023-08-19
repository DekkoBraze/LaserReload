using UnityEngine;
using System.Runtime.InteropServices;

public class AdsYandex : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    [DllImport("__Internal")]
    private static extern void ShowFullscreen();

    [DllImport("__Internal")]
    private static extern void ShowRewarded();

    public void Show1()
    {
        ShowFullscreen();
        Messenger.Broadcast(GameEvent.OFF_MUSIC);
    }

    public void Show2()
    {
        ShowRewarded();
        Messenger.Broadcast(GameEvent.OFF_MUSIC);
    }

    public void AdsCoints()
    {
        Manager.link.SkipLevel();
    }
}
