using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradesMenu : MonoBehaviour
{
    private Player Player { get; set; } = null;
    private AudioManagement AudioManagement { get; set; } = null;
    private TextMeshProUGUI CoinCount { get; set; } = null;
    private Blaster Blaster { get; set; } = null;
    private Jetpack Jetpack { get; set; } = null;
    private Flamethrower Flamethrower { get; set; } = null;
    private GameObject PrimaryMenuGameObject { get; set; } = null;
    private GameObject PauseMenuGameObject { get; set; } = null;
    private Dictionary<string, Slider> Sliders { get; set; } = null;
    private Dictionary<string, Image> SlidersImages { get; set; } = null;
    private Gradient GearLevelGradient { get; set; } = null;

    private void Awake()
    {
        if (GameObject.Find("Player") is null)
        {
            Debug.LogError(
                "ERROR: <UpgradesMenu> - Player game object was not found in the game object hierarchy."
                );
            Application.Quit(1);
        }
        if ((Player = GameObject.Find("Player").GetComponent<Player>()) is null)
        {
            Debug.LogError(
                "ERROR: <UpgradesMenu> - Player game object is missing Player component."
                );
            Application.Quit(1);
        }
        
        if (GameObject.Find("Interface/MainCamera/Audio/Sounds") is null)
        {
            Debug.LogError(
                "ERROR: <UpgradesMenu> - Interface/MainCamera/Audio/Sounds game object was not found in the " +
                "game object hierarchy."
                );
            Application.Quit(1);
        }
        if ((AudioManagement = GameObject.Find(
                "Interface/MainCamera/Audio/Sounds"
                ).GetComponent<AudioManagement>()) is null)
        {
            Debug.LogError(
                "ERROR: <UpgradesMenu> - Interface/MainCamera/Audio/Sounds game object is missing " +
                "AudioManagement component."
                );
            Application.Quit(1);
        }
        
        if (GameObject.Find(
                "Interface/MainCamera/UICanvas/PauseMenu/" +
                "UpgradesMenu/PrimaryMenu/CoinsCount/Text (TMP)"
                ) is null)
        {
            Debug.LogError(
                "ERROR: <UpgradesMenu> - Interface/MainCamera/UICanvas/PauseMenu/UpgradesMenu/PrimaryMenu/" +
                "CoinsCount/Text (TMP) game object was not found in the game object hierarchy."
                );
            Application.Quit(1);
        }
        if ((CoinCount = GameObject.Find(
                "Interface/MainCamera/UICanvas/PauseMenu/" +
                "UpgradesMenu/PrimaryMenu/CoinsCount/Text (TMP)"
                ).GetComponent<TextMeshProUGUI>()) is null)
        {
            Debug.LogError(
                "ERROR: <UpgradesMenu> - Interface/MainCamera/UICanvas/PauseMenu/UpgradesMenu/PrimaryMenu/" +
                "CoinsCount/Text (TMP) game object is missing TextMeshProUGUI component."
                );
            Application.Quit(1);
        }
        
        if (GameObject.Find("Player/Blaster") is null)
        {
            Debug.LogError(
                "ERROR: <UpgradesMenu> - Player/Blaster game object was not found in the game object hierarchy."
            );
            Application.Quit(1);
        }
        if ((Blaster = GameObject.Find("Player/Blaster").GetComponent<Blaster>()) is null)
        {
            Debug.LogError(
                "ERROR: <UpgradesMenu> - Player/Blaster game object is missing Blaster component."
            );
            Application.Quit(1);
        }
        
        if (GameObject.Find("Player/Jetpack") is null)
        {
            Debug.LogError(
                "ERROR: <UpgradesMenu> - Player/Jetpack game object was not found in the game object hierarchy."
            );
            Application.Quit(1);
        }
        if ((Jetpack = GameObject.Find("Player/Jetpack").GetComponent<Jetpack>()) is null)
        {
            Debug.LogError(
                "ERROR: <UpgradesMenu> - Player/Jetpack game object is missing Jetpack component."
            );
            Application.Quit(1);
        }
        
        if (GameObject.Find("Player/Flamethrower") is null)
        {
            Debug.LogError(
                "ERROR: <UpgradesMenu> - Player/Flamethrower game object was not found in the game object " +
                "hierarchy."
            );
            Application.Quit(1);
        }
        if ((Flamethrower = GameObject.Find("Player/Flamethrower").GetComponent<Flamethrower>()) is null)
        {
            Debug.LogError(
                "ERROR: <UpgradesMenu> - Player/Flamethrower game object is missing Flamethrower component."
            );
            Application.Quit(1);
        }
        
        if ((PrimaryMenuGameObject = GameObject.Find(
                "Interface/MainCamera/UICanvas/PauseMenu/UpgradesMenu/PrimaryMenu"
                )) is null)
        {
            Debug.LogError(
                "ERROR: <UpgradesMenu> - Interface/MainCamera/UICanvas/PauseMenu/UpgradesMenu/PrimaryMenu " +
                "game object was not found in the game object hierarchy."
                );
            Application.Quit(1);
        }
        
        if ((PauseMenuGameObject = GameObject.Find("Interface/MainCamera/UICanvas/PauseMenu/PrimaryMenu")) is null)
        {
            Debug.LogError(
                "ERROR: <UpgradesMenu> - Interface/MainCamera/UICanvas/PauseMenu/PrimaryMenu game object was " +
                "not found in the game object hierarchy."
                );
            Application.Quit(1);
        }
        
        Sliders = new Dictionary<string, Slider>();
        
        if (GameObject.Find(
                "Interface/MainCamera/UICanvas/PauseMenu/" +
                "UpgradesMenu/PrimaryMenu/UpgradeOptions/Armor/Button/Slider"
                ) is null)
        {
            Debug.LogError(
                "ERROR: <UpgradesMenu> - Interface/MainCamera/UICanvas/PauseMenu/UpgradesMenu/PrimaryMenu/" +
                "UpgradeOptions/Armor/Button/Slider game object was not found in the game object hierarchy."
                );
            Application.Quit(1);
        }
        if (GameObject.Find(
                "Interface/MainCamera/UICanvas/PauseMenu/" +
                "UpgradesMenu/PrimaryMenu/UpgradeOptions/Armor/Button/Slider"
                ).GetComponent<Slider>() is null)
        {
            Debug.LogError(
                "ERROR: <UpgradesMenu> - Interface/MainCamera/UICanvas/PauseMenu/UpgradesMenu/PrimaryMenu/" +
                "UpgradeOptions/Armor/Button/Slider game object is missing Slider component."
                );
            Application.Quit(1);
        }
        Sliders.Add("armor", GameObject.Find(
            "Interface/MainCamera/UICanvas/PauseMenu/" +
            "UpgradesMenu/PrimaryMenu/UpgradeOptions/Armor/Button/Slider"
            ).GetComponent<Slider>());
        
        if (GameObject.Find(
                "Interface/MainCamera/UICanvas/PauseMenu/" +
                "UpgradesMenu/PrimaryMenu/UpgradeOptions/Blaster/Button/Slider"
                ) is null)
        {
            Debug.LogError(
                "ERROR: <UpgradesMenu> - Interface/MainCamera/UICanvas/PauseMenu/UpgradesMenu/PrimaryMenu/" +
                "UpgradeOptions/Blaster/Button/Slider game object was not found in the game object hierarchy."
                );
            Application.Quit(1);
        }
        if (GameObject.Find(
                "Interface/MainCamera/UICanvas/PauseMenu/" +
                "UpgradesMenu/PrimaryMenu/UpgradeOptions/Blaster/Button/Slider"
                ).GetComponent<Slider>() is null)
        {
            Debug.LogError(
                "ERROR: <UpgradesMenu> - Interface/MainCamera/UICanvas/PauseMenu/UpgradesMenu/PrimaryMenu/" +
                "UpgradeOptions/Blaster/Button/Slider game object is missing Slider component."
                );
            Application.Quit(1);
        }
        Sliders.Add("blaster", GameObject.Find(
            "Interface/MainCamera/UICanvas/PauseMenu/" +
            "UpgradesMenu/PrimaryMenu/UpgradeOptions/Blaster/Button/Slider"
            ).GetComponent<Slider>());
        
        if (GameObject.Find(
                "Interface/MainCamera/UICanvas/PauseMenu/" +
                "UpgradesMenu/PrimaryMenu/UpgradeOptions/Jetpack/Button/Slider"
                ) is null)
        {
            Debug.LogError(
                "ERROR: <UpgradesMenu> - Interface/MainCamera/UICanvas/PauseMenu/UpgradesMenu/PrimaryMenu/" +
                "UpgradeOptions/Jetpack/Button/Slider game object was not found in the game object hierarchy."
                );
            Application.Quit(1);
        }
        if (GameObject.Find(
                "Interface/MainCamera/UICanvas/PauseMenu/" +
                "UpgradesMenu/PrimaryMenu/UpgradeOptions/Jetpack/Button/Slider"
                ).GetComponent<Slider>() is null)
        {
            Debug.LogError(
                "ERROR: <UpgradesMenu> - Interface/MainCamera/UICanvas/PauseMenu/UpgradesMenu/PrimaryMenu/" +
                "UpgradeOptions/Jetpack/Button/Slider game object is missing Slider component."
                );
            Application.Quit(1);
        }
        Sliders.Add("jetpack", GameObject.Find(
            "Interface/MainCamera/UICanvas/PauseMenu/" +
            "UpgradesMenu/PrimaryMenu/UpgradeOptions/Jetpack/Button/Slider"
            ).GetComponent<Slider>());
        
        if (GameObject.Find(
                "Interface/MainCamera/UICanvas/PauseMenu/" +
                "UpgradesMenu/PrimaryMenu/UpgradeOptions/Flamethrower/Button/Slider"
                ) is null)
        {
            Debug.LogError(
                "ERROR: <UpgradesMenu> - Interface/MainCamera/UICanvas/PauseMenu/UpgradesMenu/PrimaryMenu/" +
                "UpgradeOptions/Flamethrower/Button/Slider game object was not found in the game object hierarchy."
                );
            Application.Quit(1);
        }
        if (GameObject.Find(
                "Interface/MainCamera/UICanvas/PauseMenu/" +
                "UpgradesMenu/PrimaryMenu/UpgradeOptions/Flamethrower/Button/Slider"
                ).GetComponent<Slider>() is null)
        {
            Debug.LogError(
                "ERROR: <UpgradesMenu> - Interface/MainCamera/UICanvas/PauseMenu/UpgradesMenu/PrimaryMenu/" +
                "UpgradeOptions/Flamethrower/Button/Slider game object is missing Slider component."
                );
            Application.Quit(1);
        }
        Sliders.Add("flamethrower", GameObject.Find(
            "Interface/MainCamera/UICanvas/PauseMenu/" +
            "UpgradesMenu/PrimaryMenu/UpgradeOptions/Flamethrower/Button/Slider"
            ).GetComponent<Slider>());

        SlidersImages = new Dictionary<string, Image>();
        
        if (GameObject.Find(
                "Interface/MainCamera/UICanvas/PauseMenu/" +
                "UpgradesMenu/PrimaryMenu/UpgradeOptions/Armor/Button/Slider/Fill Area/Fill"
                ) is null)
        {
            Debug.LogError(
                "ERROR: <UpgradesMenu> - Interface/MainCamera/UICanvas/PauseMenu/UpgradesMenu/PrimaryMenu/" +
                "UpgradeOptions/Armor/Button/Slider/Fill Area/Fill game object was not found in the game object " +
                "hierarchy."
                );
            Application.Quit(1);
        }
        if (GameObject.Find(
                "Interface/MainCamera/UICanvas/PauseMenu/" +
                "UpgradesMenu/PrimaryMenu/UpgradeOptions/Armor/Button/Slider/Fill Area/Fill"
                ).GetComponent<Image>() is null)
        {
            Debug.LogError(
                "ERROR: <UpgradesMenu> - Interface/MainCamera/UICanvas/PauseMenu/UpgradesMenu/PrimaryMenu/" +
                "UpgradeOptions/Armor/Button/Slider/Fill Area/Fill game object is missing Image component."
                );
            Application.Quit(1);
        }
        SlidersImages.Add("armor", GameObject.Find(
            "Interface/MainCamera/UICanvas/PauseMenu/" +
            "UpgradesMenu/PrimaryMenu/UpgradeOptions/Armor/Button/Slider/Fill Area/Fill"
            ).GetComponent<Image>());
        
        if (GameObject.Find(
                "Interface/MainCamera/UICanvas/PauseMenu/" +
                "UpgradesMenu/PrimaryMenu/UpgradeOptions/Blaster/Button/Slider/Fill Area/Fill"
                ) is null)
        {
            Debug.LogError(
                "ERROR: <UpgradesMenu> - Interface/MainCamera/UICanvas/PauseMenu/UpgradesMenu/PrimaryMenu/" +
                "UpgradeOptions/Blaster/Button/Slider/Fill Area/Fill game object was not found in the game object " +
                "hierarchy."
                );
            Application.Quit(1);
        }
        if (GameObject.Find(
                "Interface/MainCamera/UICanvas/PauseMenu/" +
                "UpgradesMenu/PrimaryMenu/UpgradeOptions/Blaster/Button/Slider/Fill Area/Fill"
                ).GetComponent<Image>() is null)
        {
            Debug.LogError(
                "ERROR: <UpgradesMenu> - Interface/MainCamera/UICanvas/PauseMenu/UpgradesMenu/PrimaryMenu/" +
                "UpgradeOptions/Blaster/Button/Slider/Fill Area/Fill game object is missing Image component."
                );
            Application.Quit(1);
        }
        SlidersImages.Add("blaster", GameObject.Find(
            "Interface/MainCamera/UICanvas/PauseMenu/" +
            "UpgradesMenu/PrimaryMenu/UpgradeOptions/Blaster/Button/Slider/Fill Area/Fill"
            ).GetComponent<Image>());
        
        if (GameObject.Find(
                "Interface/MainCamera/UICanvas/PauseMenu/" +
                "UpgradesMenu/PrimaryMenu/UpgradeOptions/Jetpack/Button/Slider/Fill Area/Fill"
                ) is null)
        {
            Debug.LogError(
                "ERROR: <UpgradesMenu> - Interface/MainCamera/UICanvas/PauseMenu/UpgradesMenu/PrimaryMenu/" +
                "UpgradeOptions/Jetpack/Button/Slider/Fill Area/Fill game object was not found in the game object " +
                "hierarchy."
                );
            Application.Quit(1);
        }
        if (GameObject.Find(
                "Interface/MainCamera/UICanvas/PauseMenu/" +
                "UpgradesMenu/PrimaryMenu/UpgradeOptions/Jetpack/Button/Slider/Fill Area/Fill"
                ).GetComponent<Image>() is null)
        {
            Debug.LogError(
                "ERROR: <UpgradesMenu> - Interface/MainCamera/UICanvas/PauseMenu/UpgradesMenu/PrimaryMenu/" +
                "UpgradeOptions/Jetpack/Button/Slider/Fill Area/Fill game object is missing Image component."
                );
            Application.Quit(1);
        }
        SlidersImages.Add("jetpack", GameObject.Find(
            "Interface/MainCamera/UICanvas/PauseMenu/" +
            "UpgradesMenu/PrimaryMenu/UpgradeOptions/Jetpack/Button/Slider/Fill Area/Fill"
            ).GetComponent<Image>());
        
        if (GameObject.Find(
                "Interface/MainCamera/UICanvas/PauseMenu/" +
                "UpgradesMenu/PrimaryMenu/UpgradeOptions/Flamethrower/Button/Slider/Fill Area/Fill"
                ) is null)
        {
            Debug.LogError(
                "ERROR: <UpgradesMenu> - Interface/MainCamera/UICanvas/PauseMenu/UpgradesMenu/PrimaryMenu/" +
                "UpgradeOptions/Flamethrower/Button/Slider/Fill Area/Fill game object was not found in the game " +
                "object hierarchy."
                );
            Application.Quit(1);
        }
        if (GameObject.Find(
                "Interface/MainCamera/UICanvas/PauseMenu/" +
                "UpgradesMenu/PrimaryMenu/UpgradeOptions/Flamethrower/Button/Slider/Fill Area/Fill"
                ).GetComponent<Image>() is null)
        {
            Debug.LogError(
                "ERROR: <UpgradesMenu> - Interface/MainCamera/UICanvas/PauseMenu/UpgradesMenu/PrimaryMenu/" +
                "UpgradeOptions/Flamethrower/Button/Slider/Fill Area/Fill game object is missing Image component."
                );
            Application.Quit(1);
        }
        SlidersImages.Add("flamethrower", GameObject.Find(
            "Interface/MainCamera/UICanvas/PauseMenu/" +
            "UpgradesMenu/PrimaryMenu/UpgradeOptions/Flamethrower/Button/Slider/Fill Area/Fill"
            ).GetComponent<Image>());
        
        GearLevelGradient = new Gradient();

        GradientColorKey[] colorKey;
        GradientAlphaKey[] alphaKey;
        
        colorKey = new GradientColorKey[5];
        colorKey[0].color = new Color32(255, 255, 255, 255);
        colorKey[0].time = 0f;
        colorKey[1].color = new Color32(150, 150, 150, 255);
        colorKey[1].time = 0.25f;
        colorKey[2].color = new Color32(0, 150, 255, 255);
        colorKey[2].time = 0.5f;
        colorKey[3].color = new Color32(200, 0, 255, 255);
        colorKey[3].time = 0.75f;
        colorKey[4].color = new Color32(255, 180, 0, 255);
        colorKey[4].time = 1f;

        alphaKey = new GradientAlphaKey[5];
        alphaKey[0].alpha = 1f;
        alphaKey[0].time = 0f;
        alphaKey[1].alpha = 1f;
        alphaKey[1].time = 0.25f;
        alphaKey[2].alpha = 1f;
        alphaKey[2].time = 0.5f;
        alphaKey[3].alpha = 1f;
        alphaKey[3].time = 0.75f;
        alphaKey[4].alpha = 1f;
        alphaKey[4].time = 1f;
        
        GearLevelGradient.SetKeys(colorKey, alphaKey);
    }

    private void Start()
    {
        CoinCount.text = "0";
        
        foreach (string gearLevelSlider in Sliders.Keys)
        {
            Sliders[gearLevelSlider].value = 1;
            SlidersImages[gearLevelSlider].color = GearLevelGradient.Evaluate(Sliders[gearLevelSlider].normalizedValue);
        }

        Sliders["armor"].value = 0;
        SlidersImages["armor"].color = GearLevelGradient.Evaluate(Sliders["armor"].normalizedValue);
        Sliders["blaster"].value = 0;
        SlidersImages["blaster"].color = GearLevelGradient.Evaluate(Sliders["blaster"].normalizedValue);
        Sliders["jetpack"].value = 0;
        SlidersImages["jetpack"].color = GearLevelGradient.Evaluate(Sliders["jetpack"].normalizedValue);
        Sliders["flamethrower"].value = 0;
        SlidersImages["flamethrower"].color = GearLevelGradient.Evaluate(Sliders["flamethrower"].normalizedValue);
    }

    public void RefreshLoadedPlayerData()
    {
        if (Player.PlayerData is null)
        {
            Debug.LogWarning("WARNING: <UpgradesMenu> - PlayerData is null.");
            return;
        }
        
        CoinCount.text = Player.PlayerData.CoinCount.ToString();
        Sliders["armor"].value = Player.PlayerData.ArmorLevel;
        SlidersImages["armor"].color = GearLevelGradient.Evaluate(Sliders["armor"].normalizedValue);
        Sliders["blaster"].value = Player.PlayerData.BlasterLevel;
        SlidersImages["blaster"].color = GearLevelGradient.Evaluate(Sliders["blaster"].normalizedValue);
        Sliders["jetpack"].value = Player.PlayerData.JetpackLevel;
        SlidersImages["jetpack"].color = GearLevelGradient.Evaluate(Sliders["jetpack"].normalizedValue);
        Sliders["flamethrower"].value = Player.PlayerData.FlamethrowerLevel;
        SlidersImages["flamethrower"].color = GearLevelGradient.Evaluate(Sliders["flamethrower"].normalizedValue);
    }
    
    public void UpgradeGear(string gearName)
    {
        gearName = gearName.ToLower();

        if (CoinCount.text == "0" || Player.PlayerData.CoinCount == 0)
        {
            AudioManagement.PlayOneShot("ErrorSound");
            return;
        }
        
        if (gearName == "armor")
        {
            if (Player.HealCoroutine is not null || Player.PlayerData.ArmorLevel == 4)
            {
                AudioManagement.PlayOneShot("ErrorSound");
                return;
            }
            
            Player.PlayerData.ArmorLevel += 1;
            Sliders["armor"].value += 1;
            SlidersImages["armor"].color = GearLevelGradient.Evaluate(Sliders["armor"].normalizedValue);
            
            Player.ReloadGearValue("armor");
        }
        else if (gearName == "blaster")
        {
            if (Blaster.ShootCoroutine is not null || Blaster.CoolingCoroutine is not null || 
                Blaster.OverheatCoroutine is not null || Player.PlayerData.BlasterLevel == 4)
            {
                AudioManagement.PlayOneShot("ErrorSound");
                return;
            }
            
            Player.PlayerData.BlasterLevel += 1;
            Sliders["blaster"].value += 1;
            SlidersImages["blaster"].color = GearLevelGradient.Evaluate(Sliders["blaster"].normalizedValue);
            
            Player.ReloadGearValue("blaster");
        }
        else if (gearName == "jetpack")
        {
            if (Jetpack.BurnFuelCoroutine is not null || Jetpack.RechargeFuelCoroutine is not null ||
                Player.PlayerData.JetpackLevel == 4)
            {
                AudioManagement.PlayOneShot("ErrorSound");
                return;
            }
            
            Player.PlayerData.JetpackLevel += 1;
            Sliders["jetpack"].value += 1;
            SlidersImages["jetpack"].color = GearLevelGradient.Evaluate(Sliders["jetpack"].normalizedValue);
            
            Player.ReloadGearValue("jetpack");
        }
        else if (gearName == "flamethrower")
        {
            if (Flamethrower.FlameCoroutine is not null || Flamethrower.CoolingCoroutine is not null ||
                Player.PlayerData.FlamethrowerLevel == 4)
            {
                AudioManagement.PlayOneShot("ErrorSound");
                return;
            }
            
            Player.PlayerData.FlamethrowerLevel += 1;
            Sliders["flamethrower"].value += 1;
            SlidersImages["flamethrower"].color = GearLevelGradient.Evaluate(Sliders["flamethrower"].normalizedValue);
            
            Player.ReloadGearValue("flamethrower");
        }
        else
        {
            Debug.LogWarning("WARNING: <UpgradesMenu> - Invalid gear name.");
            return;
        }
        
        Player.PlayerData.CoinCount -= 1;
        CoinCount.text = Player.PlayerData.CoinCount.ToString();
        RefreshLoadedPlayerData();
        AudioManagement.PlayOneShot("UpgradeButtonSound");
    }
    
    public void SwitchBackToPrimaryMenu()
    {
        AudioManagement.PlayOneShot("ButtonSound");
        PrimaryMenuGameObject.SetActive(false);
        PauseMenuGameObject.SetActive(true);
    }
}
