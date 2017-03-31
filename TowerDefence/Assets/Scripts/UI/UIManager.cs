using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour {

    [Header ("UI")]
    [SerializeField]
    private Text livesText;

    [SerializeField]
    private Text buildModeText;

    [SerializeField]
    private Text waveTimer;

    [SerializeField]
    private Text waveNumber;

    [SerializeField]
    private Text BlockCurrencyText;

    [SerializeField]
    private Text ZomnCurrencyText;

    [Header ("Sprites")]
    [SerializeField]
    private Button fastWard;
    [SerializeField]
    private Sprite fastWardSprite;
    [SerializeField]
    private Sprite slowWardSprite;

    [SerializeField]
    private UIInGameMenu MenuUI;
    [SerializeField]
    private UIOptionMenu OptionsMenu;
    [SerializeField]
    private UITurretUpgrade turretUpgradeMenu;
    [SerializeField]
    private UITurretBuildMenu turretBuildMenu;
    [SerializeField]
    private UIBuildingBuildMenu BuildingsBuildMenu;

    [SerializeField]
    private Text speedText;

    void Start()
    {
        speedText.text = "x1";
    }

	void Update ()
    {
        buildModeText.enabled = BuildManager.BuildManagerInstance.GetBuildingMode();

        waveTimer.text = References.Refs.spawnerRef.GetWaveTimerText();

        waveNumber.text = References.Refs.spawnerRef.GetWaveText();

        BlockCurrencyText.text = PlayerData.playerData.GetCurrencyText();

        ZomnCurrencyText.text = PlayerData.playerData.GetZomnCurrencyText();

        livesText.text = GameManager.gameManager.GetLivesText();

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(MenuUI.isActiveAndEnabled)
            {
                MenuUI.ShowUI(false);
            }
            else if(OptionsMenu.isActiveAndEnabled)
            {
                OptionsMenu.ShowUI(false);
            }
            else
            {
                MenuUI.ShowUI(true);
            }
        }   
	}

    public void ExitGame()
    {
        HideAllUI();
        MenuUI.ShowUI(true);
        GameManager.gameManager.SetPause(true);
    }

    public void ShowOptionsUI()
    {
        HideAllUI();
        OptionsMenu.ShowUI(true);
    }

    public void ShowTurretUpgradeUI(bool _show)
    {
        turretUpgradeMenu.ShowUI(_show);
    }

    public bool isTurretUpgradeShowing()
    {
        return turretUpgradeMenu.isShowing;
    }

    public void ShowPlayerStatsUI()
    {
        HideAllUI();
    }

    public void MuteMusic()
    {
        if(AudioListener.volume == 0)
        {
            AudioListener.volume = 1;
        }
        else
        {
            AudioListener.volume = 0;
        }

    }

    public void ShowInfoUI()
    {
        HideAllUI();
    }

    public void SaveGame()
    {

    }

    public void SpeedUpGameTime()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 2;
            fastWard.GetComponent<Image>().sprite = slowWardSprite;
            speedText.text = "x2";
        }
        else
        {
            Time.timeScale = 1;
            fastWard.GetComponent<Image>().sprite = fastWardSprite;
            speedText.text = "x1";
        }
    }

    void HideAllUI()
    {
        MenuUI.ShowUI(false);
        OptionsMenu.ShowUI(false);
        turretBuildMenu.ShowUI(false);
        BuildingsBuildMenu.ShowUI(false);
    }
}
