using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : UiPanel {
    [SerializeField] Slider masterVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider effectsVolumeSlider;


    protected override void OnInitialise() { }

    protected override void OnOpen() { }

    protected override void OnClose() { }
}