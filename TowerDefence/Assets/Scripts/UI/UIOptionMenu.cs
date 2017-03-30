using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.IO;

public class UIOptionMenu : MonoBehaviour {

    [SerializeField]
    private Dropdown GaphicsPresets;
    [SerializeField]
    private Toggle Fullscreen;
    [SerializeField]
    private Dropdown ScreenResolution;
    [SerializeField]
    private Dropdown TextureQuality;
    [SerializeField]
    private Dropdown AnisotropicTextures;
    [SerializeField]
    private Dropdown AntiAliasing;
    [SerializeField]
    private Toggle SoftParticles;
    [SerializeField]
    private Dropdown Shadows;
    [SerializeField]
    private Dropdown ShadowResolution;
    [SerializeField]
    private Dropdown ShadowsCascades;
    [SerializeField]
    private Dropdown VSync;
    [SerializeField]
    private Dropdown LOD;
    [SerializeField]
    private Slider OveralVolume;
    [SerializeField]
    private Slider SoundEffects;
    [SerializeField]
    private Slider VoiceEffects;
    [SerializeField]
    private Dropdown MonoStereo;

    private GameSettings gameSettings;

    private Resolution[] resolutions;

    void Start()
    {
        gameSettings = new GameSettings();

        resolutions = Screen.resolutions;

        foreach (Resolution r in resolutions)
        {
            ScreenResolution.options.Add(new Dropdown.OptionData(r.ToString()));
        }

        gameSettings = LoadSettings();

        ApplySettings();
        UpdateUI();
        RefreshShownValues();
    }

    public void ShowUI(bool _ShowUi)
    {
        gameObject.SetActive(_ShowUi);
    }

    public void OnCloseOptions()
    {
        ShowUI(false);
    }

    public void OnGraphicPresetChange()
    {
        gameSettings.GaphicsPresets = GaphicsPresets.value;
        QualitySettings.SetQualityLevel(gameSettings.GaphicsPresets);
        UpdatePreset();
        RefreshShownValues();
    }

    public void OnFullscreen()
    {
        gameSettings.Fullscreen = Fullscreen.isOn;
        Screen.fullScreen = Fullscreen.isOn;
    }

    public void OnScreenResolutionChange()
    {
        gameSettings.ScreenResolution = ScreenResolution.value;
    }

    public void OnTextureQualityChange()
    {
        gameSettings.TextureQuality = TextureQuality.value;
    }

    public void OnAnisotropicTextureChange()
    {
        gameSettings.AnisotropicTextures = AnisotropicTextures.value;
    }

    public void OnAntiAliasingChange()
    {
        gameSettings.AntiAliasing = AntiAliasing.value;
    }

    public void OnSoftParticlesChange()
    {
        gameSettings.SoftParticles = SoftParticles.isOn;
    }

    public void OnShadowsChange()
    {
        gameSettings.Shadows = Shadows.value;
    }

    public void OnShadowResolutionChange()
    {
        gameSettings.ShadowResolution = ShadowResolution.value;
    }

    public void OnShadowCascadesChange()
    {
        gameSettings.ShadowsCascades = ShadowsCascades.value;
    }

    public void OnVSyncChange()
    {
        gameSettings.VSync = VSync.value;
    }

    public void OnLODChange()
    {
        gameSettings.LOD = LOD.value;
    }

    public void OnOveralVolumeChange()
    {
        gameSettings.OveralVolume = OveralVolume.value;
    }

    public void OnSoundEffectsChange()
    {
        gameSettings.SoundEffects = SoundEffects.value;
    }

    public void OnVoiceEffectChange()
    {
        gameSettings.VoiceEffects = VoiceEffects.value;
    }

    public void OnMonoStereoChange()
    {
        gameSettings.MonoStereo = MonoStereo.value;
    }

    public void OnAcceptSettings()
    {
        ApplySettings();
        SaveSettings();
    }

    public void ApplySettings()
    {
        QualitySettings.SetQualityLevel(gameSettings.GaphicsPresets);
        Screen.fullScreen = gameSettings.Fullscreen;
        Screen.SetResolution(resolutions[gameSettings.ScreenResolution].width, resolutions[gameSettings.ScreenResolution].height, Screen.fullScreen);
        QualitySettings.masterTextureLimit = gameSettings.TextureQuality;
        QualitySettings.anisotropicFiltering = (AnisotropicFiltering)gameSettings.AnisotropicTextures;
        QualitySettings.antiAliasing = gameSettings.AntiAliasing;
        QualitySettings.softParticles = gameSettings.SoftParticles;
        QualitySettings.shadows = (ShadowQuality)gameSettings.Shadows;
        QualitySettings.shadowCascades = gameSettings.ShadowsCascades;
        QualitySettings.shadowResolution = (ShadowResolution)gameSettings.ShadowResolution;
        QualitySettings.vSyncCount = gameSettings.VSync;
        QualitySettings.maximumLODLevel = gameSettings.LOD;

        RefreshShownValues();
    }

    public void UpdatePreset()
    {
        gameSettings.Fullscreen = Fullscreen.isOn = Screen.fullScreen;
        gameSettings.ScreenResolution = ScreenResolution.value;
        gameSettings.TextureQuality = TextureQuality.value = QualitySettings.masterTextureLimit;
        gameSettings.AnisotropicTextures = AnisotropicTextures.value = (int)QualitySettings.anisotropicFiltering;
        gameSettings.AntiAliasing = AntiAliasing.value = QualitySettings.antiAliasing;
        gameSettings.SoftParticles = SoftParticles.isOn = QualitySettings.softParticles;
        gameSettings.Shadows = Shadows.value = (int)QualitySettings.shadows;
        gameSettings.ShadowsCascades = ShadowsCascades.value = QualitySettings.shadowCascades;
        gameSettings.ShadowResolution = ShadowResolution.value = (int)QualitySettings.shadowResolution;
        gameSettings.VSync = VSync.value = QualitySettings.vSyncCount;
        gameSettings.LOD = LOD.value = QualitySettings.maximumLODLevel;
    }

    public void UpdateUI()
    {
        GaphicsPresets.value = gameSettings.GaphicsPresets;
        Fullscreen.isOn = gameSettings.Fullscreen;
        ScreenResolution.value = gameSettings.ScreenResolution;
        TextureQuality.value = gameSettings.TextureQuality;
        AnisotropicTextures.value = gameSettings.AnisotropicTextures;
        AntiAliasing.value = gameSettings.AntiAliasing;
        SoftParticles.isOn = gameSettings.SoftParticles;
        Shadows.value = gameSettings.Shadows;
        ShadowsCascades.value = gameSettings.ShadowsCascades;
        ShadowResolution.value = gameSettings.ShadowResolution;
        VSync.value = gameSettings.VSync;
        LOD.value = gameSettings.LOD;

        OveralVolume.value = gameSettings.OveralVolume;
        SoundEffects.value = gameSettings.SoundEffects;
        VoiceEffects.value = gameSettings.VoiceEffects;
        MonoStereo.value = gameSettings.MonoStereo;
    }

    public void SaveSettings()
    {
        string jsonData = JsonUtility.ToJson(gameSettings, true);
        File.WriteAllText(Application.persistentDataPath + "/gamesettings.json", jsonData);
    }

    public GameSettings LoadSettings()
    {
        GameSettings _gamesettings = JsonUtility.FromJson<GameSettings>(File.ReadAllText(Application.persistentDataPath + "/gamesettings.json"));

        return _gamesettings;
    }

    public void RefreshShownValues()
    {
        GaphicsPresets.RefreshShownValue();
        ScreenResolution.RefreshShownValue();
        TextureQuality.RefreshShownValue();
        AnisotropicTextures.RefreshShownValue();
        AntiAliasing.RefreshShownValue();
        Shadows.RefreshShownValue();
        ShadowResolution.RefreshShownValue();
        ShadowsCascades.RefreshShownValue();
        VSync.RefreshShownValue();
        LOD.RefreshShownValue();
        MonoStereo.RefreshShownValue();
    }
}
