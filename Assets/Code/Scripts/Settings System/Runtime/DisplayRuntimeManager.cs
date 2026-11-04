using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using YNQ.Settings.SettingField;

public class DisplayRuntimeManager : ASettingsRuntimeManager
{
    [SerializeField] private Vector2 brightnessBounds;
    [SerializeField] private VolumeProfile brightnessProfile;
    [SerializeField] private SelectVector2SettingField resolutionSetting;
    [SerializeField] private SelectWindowModeSettingField windowModeSetting;
    [SerializeField] private IntSettingField brightnessSetting;

    protected override void Connect()
    {
        resolutionSetting.OnValueChanged += ChangeResolution;
        windowModeSetting.OnValueChanged += ChangeWindowMode;
        brightnessSetting.OnValueChanged += SetBrightness;
    }

    protected override void Disconnect()
    {
        resolutionSetting.OnValueChanged -= ChangeResolution;
        windowModeSetting.OnValueChanged -= ChangeWindowMode;
        brightnessSetting.OnValueChanged -= SetBrightness;
    }

    private void ChangeResolution(Vector2 resolution)
    {
        var resolutionInt = new Vector2Int((int)resolution.x, (int)resolution.y);
        Screen.SetResolution(resolutionInt.x, resolutionInt.y, windowModeSetting.Value == WindowMode.FullScreen);
    }

    private void ChangeWindowMode(WindowMode windowMode)
    {
        Screen.fullScreenMode = windowMode switch
        {
            WindowMode.FullScreen => FullScreenMode.ExclusiveFullScreen,
            WindowMode.Windowed => FullScreenMode.Windowed,
            WindowMode.WindowedBorderless => FullScreenMode.MaximizedWindow,
            _ => FullScreenMode.Windowed
        };
    }

    private void SetBrightness(int brightnessLevel)
    {
        brightnessProfile.TryGet(out LiftGammaGain liftGammaGain);
        var levelNormalised = (float)(brightnessLevel - brightnessSetting.MinValue) / brightnessSetting.MaxValue;
        var value = Mathf.Lerp(brightnessBounds.x, brightnessBounds.y, levelNormalised);

        liftGammaGain.gain.Override(new Vector4(0, 0, 0, value));
    }
}
