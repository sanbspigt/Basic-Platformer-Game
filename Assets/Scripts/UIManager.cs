using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public string music_Key = "MUSIC_ON";
    public string sound_Key = "SOUND_ON";

    public GameObject MainScreen;
    public GameObject GameplayScreen;
    public GameObject SettingsScreen;
    /// 
    [SerializeField] Toggle soundToggle,musicToggle;

    private Stack<GameObject> screenHistory 
        = new Stack<GameObject>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }        
    }

    private void Start()
    {
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            HandleBackFunctionality();
    }

    void HandleBackFunctionality()
    {
        if (screenHistory.Count > 0)
        {
            GameObject lastScreen = screenHistory.Pop();
            TransitionToScreen(lastScreen);
        }
        else
        { 
            //if you are in the gameplay - (Show Pause Menu)
            //else if you are in the Main Menu - (Quit Menu)
        }
    }

    void ShowPopup(GameObject screen)
    {
        screen.SetActive(true);
        screen.transform.DOScale(Vector2.one,0.4f)
            .From(Vector2.zero).SetEase(Ease.OutBack);
    }

    void HidePopup(GameObject screen)
    {
        screen.transform.DOScale(Vector2.zero, 0.3f)
            .From(Vector2.one).SetEase(Ease.InBack);
    }

    void TransitionToScreen(GameObject screen)
    {
        if (screenHistory.Count == 0 ||
            screenHistory.Peek() != screen)
        {
            screenHistory.Push(screen);//Saving current screen to history.
        }

        MainScreen.SetActive(false) ;
        SettingsScreen.SetActive(false);

        //screen.SetActive(true);
        // screen.GetComponent<CanvasGroup>()?.DOFade(1, 0.05f).From(0);
        ShowPopup(screen);

    }

    public void GoToMainMenu()
    {
        TransitionToScreen(MainScreen);
    }

    public void GoToGamePlay()
    {
        TransitionToScreen(GameplayScreen);
    }

    public void GoToSettings()
    {
        TransitionToScreen(SettingsScreen);
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
        if (soundToggle.isOn)
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

}
