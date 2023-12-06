using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField]
    List<UIScreenInfo> screenInfos = new List<UIScreenInfo>();
       
    Dictionary<ScreenTypes, UIScreenInfo> screens = new Dictionary<ScreenTypes, UIScreenInfo>();
      
    public enum ScreenTypes
    { 
        MAIN_MENU,
        GAMEPLAY,
        SETTINGS,
        PAUSE,
        COMPLETED,
        GAME_OVER,
        QUIT,
        SHOP,
        INVENTORY,
        ACHIEVEMENTS,
        LEVEL_SELECTION,
        LEADERBOARD,
        RATE_US
    }

    public string music_Key = "MUSIC_ON";
    public string sound_Key = "SOUND_ON";

   
    /// 
    [SerializeField] Toggle soundToggle,musicToggle;

    private Stack<UIScreenInfo> screenHistory 
        = new Stack<UIScreenInfo>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        foreach (UIScreenInfo info in screenInfos)
        {
            screens[info.scrType] = info;
        }
    }


    private void Start()
    {        
        MakeUIUpdates();
    }

    

   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            HandleBackFunctionality();
    }

    void HandleBackFunctionality()
    {
        if (screenHistory.Count > 0)
        {
            UIScreenInfo lastScreen = screenHistory.Pop();
            TransitionToScreen(lastScreen);
        }
        else
        { 
            //if you are in the gameplay - (Show Pause Menu)
            //else if you are in the Main Menu - (Quit Menu)
        }
    }

    void ShowPopup(UIScreenInfo screen)
    {
        if (screens.TryGetValue(screen.scrType, out UIScreenInfo currtInfo))
        {
            currtInfo.screenRef.SetActive(true);
            currtInfo.screenRef.transform.
                DOScale(1, currtInfo.animDuration).
                From(0).SetEase(currtInfo.animType);
        }       
    }

    void HidePopup(UIScreenInfo screen)
    {
        if (screens.TryGetValue(screen.scrType, out UIScreenInfo currtInfo))
        {
            currtInfo.screenRef.transform.
                DOScale(0, currtInfo.animDuration).
                From(1).SetEase(currtInfo.animType)
                .OnComplete(()=> {
                    currtInfo.screenRef.SetActive(false);
                });
        }
    }

    void TransitionToScreen(UIScreenInfo screen)
    {
        if (screenHistory.Count == 0 ||
            screenHistory.Peek() != screen)
        {
            screenHistory.Push(screen);//Saving current screen to history.
        }

        ShowPopup(screen);
    }

    public void ShowOrHideMainMenu(bool isOpen)
    {
        UIScreenInfo info = screens[ScreenTypes.MAIN_MENU];
        if (isOpen)
            TransitionToScreen(info);
        else
            HidePopup(info);
    }

    public void ShowOrHideGamePlay(bool isOpen)
    {
        UIScreenInfo info = screens[ScreenTypes.GAMEPLAY];
        if (isOpen)
            TransitionToScreen(info);
        else
            HidePopup(info);
    }

    public void ShowOrHideSettings(bool isOpen)
    {
        UIScreenInfo info = screens[ScreenTypes.SETTINGS];
        if (isOpen)                 
            TransitionToScreen(info);      
        else
            HidePopup(info);
    }

    public void SetSound()
    {
        if (soundToggle.isOn)
        {
            SoundManager.instance.SetSfxVolume(1);
            SaveDataManager.instance.Save(sound_Key,1);
        }
        else
        {
            SoundManager.instance.SetSfxVolume(0);
            SaveDataManager.instance.Save(sound_Key, 0);
        }

        MakeUIUpdates();
    }

    public void SetMusic()
    {
        if (musicToggle.isOn)
        {
            SoundManager.instance.SetMusicVolume(1);
            SaveDataManager.instance.Save(music_Key, 1);           
        }
        else
        {
            SoundManager.instance.SetMusicVolume(0);
            SaveDataManager.instance.Save(music_Key, 0);
        }

        MakeUIUpdates();
    }

    void MakeUIUpdates()
    {
        if (SaveDataManager.instance.GetInt(music_Key) > 0)
        {
            musicToggle.isOn = true;
            SoundManager.instance.SetMusicVolume(1);
        }
        else
        {
            musicToggle.isOn = false;
            SoundManager.instance.SetMusicVolume(0);
        }

        if (SaveDataManager.instance.GetInt(sound_Key) > 0)
        {
            soundToggle.isOn = true;
            SoundManager.instance.SetSfxVolume(1);
        }
        else
        {
            soundToggle.isOn = false;
            SoundManager.instance.SetSfxVolume(0);
        }
    }
}
