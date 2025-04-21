using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [Header("오디오 설정 UI")]
    public Toggle bgmMute;
    public Toggle sfxMute;
    public Slider bgmSlider;
    public Slider sfxSlider;

    [Header("버튼")]
    public GameObject settingPnl;
    public Button openSettingButt;
    public Button closeSettingButt;
    public Button exitButt;

    // Start is called before the first frame update
    void Start()
    {
        //Toggle 이벤트 등록
        bgmMute.onValueChanged.AddListener((isOn)=> SoundManager.instance.SetBGMMute(isOn));
        sfxMute.onValueChanged.AddListener((isOn) => SoundManager.instance.SetSFXMute(isOn));

        //Slider 이벤트 등록
        bgmSlider.onValueChanged.AddListener((value) => SoundManager.instance.SetBGMVolume(value));
        sfxSlider.onValueChanged.AddListener((value) => SoundManager.instance.SetSFXVolume(value));

        //Button 이벤트 등록
        openSettingButt.onClick.AddListener(() => settingPnl.SetActive(true));
        closeSettingButt.onClick.AddListener(() => settingPnl.SetActive(false));
        exitButt.onClick.AddListener(ExitGame);

    }

    void ExitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
