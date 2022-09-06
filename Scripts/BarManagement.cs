using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarManagement : MonoBehaviour
{
    private Slider Slider { get; set; }
    private Image Fill { get; set; }
    private Gradient CurrentGradient { get; set; }
    private Dictionary<string, Gradient> Gradients { get; set; }
    private Quaternion FixedRotation { get; set; }
    
    private void Awake()
    {
        if ((Slider = this.gameObject.GetComponent<Slider>()) is null)
        {
            Debug.LogError(
                "ERROR: <BarManagement> - " + this.gameObject.transform.parent.name + " game object is " +
                "missing Slider component."
                );
            Application.Quit(1);
        }

        if (this.gameObject.transform.Find("Fill") is null)
        {
            Debug.LogError(
                "ERROR: <BarManagement> - " + this.gameObject.transform.name + "/Fill game object was not " +
                "found in the game object hierarchy."
                );
            Application.Quit(1);
        }
        if ((Fill = this.gameObject.transform.Find("Fill").gameObject.GetComponent<Image>()) is null)
        {
            Debug.LogError(
                "ERROR: <BarManagement> - " + this.gameObject.transform.name + "/Fill is missing Image " +
                "component."
                );
            Application.Quit(1);
        }
        
        Gradients = new Dictionary<string, Gradient>();
        FixedRotation = transform.rotation;
        
        CreateGradients();
        SetGradient("Increasing");
    }
    
    private void LateUpdate()
    {
        // Fix rotation of bar slider.
        transform.rotation = FixedRotation;
    }

    private void CreateGradients()
    {
        Gradient increasingGradient = new Gradient();
        Gradient decreasingGradient = new Gradient();
        Gradient rechargingGradient = new Gradient();
        Gradient enemyHealthGradient = new Gradient();

        GradientColorKey[] colorKey;
        GradientAlphaKey[] alphaKey;
        
        colorKey = new GradientColorKey[3];
        colorKey[0].color = new Color32(0, 255, 0, 255);
        colorKey[0].time = 0f;
        colorKey[1].color = new Color32(255, 255, 0, 255);
        colorKey[1].time = 0.5f;
        colorKey[2].color = new Color32(255, 0, 0, 255);
        colorKey[2].time = 1f;

        alphaKey = new GradientAlphaKey[3];
        alphaKey[0].alpha = 1f;
        alphaKey[0].time = 0f;
        alphaKey[1].alpha = 1f;
        alphaKey[1].time = 0.5f;
        alphaKey[2].alpha = 1f;
        alphaKey[2].time = 1f;
        
        increasingGradient.SetKeys(colorKey, alphaKey);
        Gradients.Add("Increasing", increasingGradient);
        
        colorKey = new GradientColorKey[3];
        colorKey[0].color = new Color32(255, 0, 0, 255);
        colorKey[0].time = 0f;
        colorKey[1].color = new Color32(255, 255, 0, 255);
        colorKey[1].time = 0.5f;
        colorKey[2].color = new Color32(0, 255, 0, 255);
        colorKey[2].time = 1f;
        
        alphaKey = new GradientAlphaKey[3];
        alphaKey[0].alpha = 1f;
        alphaKey[0].time = 0f;
        alphaKey[1].alpha = 1f;
        alphaKey[1].time = 0.5f;
        alphaKey[2].alpha = 1f;
        alphaKey[2].time = 1f;
        
        decreasingGradient.SetKeys(colorKey, alphaKey);
        Gradients.Add("Decreasing", decreasingGradient);
        
        colorKey = new GradientColorKey[1];
        colorKey[0].color = new Color32(140, 0, 0, 255);
        colorKey[0].time = 0f;
        
        alphaKey = new GradientAlphaKey[1];
        alphaKey[0].alpha = 1f;
        alphaKey[0].time = 0f;
        
        rechargingGradient.SetKeys(colorKey, alphaKey);
        Gradients.Add("Recharging", rechargingGradient);
        
        colorKey = new GradientColorKey[1];
        colorKey[0].color = new Color32(255, 0, 0, 255); 
        colorKey[0].time = 0f;
        
        alphaKey = new GradientAlphaKey[1];
        alphaKey[0].alpha = 1f;
        alphaKey[0].time = 0f;
        
        enemyHealthGradient.SetKeys(colorKey, alphaKey);
        Gradients.Add("EnemyHealth", enemyHealthGradient);
    }

    public void SetMaxValue(float value)
    {
        Slider.maxValue = value;
        Fill.color = CurrentGradient.Evaluate(1f);
    }

    public void SetValue(float value)
    {
        Slider.value = value;
        Fill.color = CurrentGradient.Evaluate(Slider.normalizedValue);
    }

    public void SetGradient(string gradientName)
    {
        Gradient gradient;
        if (!Gradients.TryGetValue(gradientName, out gradient)) {
            Debug.LogWarning("WARNING: <BarManagement> - gradient: " + gradientName + " was not found.");
            return;
        }
        CurrentGradient = gradient;
        Fill.color = CurrentGradient.Evaluate(Slider.normalizedValue);
    }
}