using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EasingTest : MonoBehaviour
{
    [Header("Animation Player")]
    [SerializeField]
    private Button playButton;
    private Image playButtonIconImage;
    [SerializeField]
    private Sprite playSprite;
    [SerializeField]
    private Sprite pauseSprite;
    [SerializeField]
    private Button loopButton;
    private Image loopButtonIconImage;
    [SerializeField]
    private Sprite loopOnSprite;
    [SerializeField]
    private Sprite loopOffSprite;
    [SerializeField]
    private TMPro.TMP_InputField progressInputField;
    [SerializeField]
    private Slider progressSlider;

    [Header("Animation Speed")]
    [SerializeField]
    private Slider durationSlider;
    [SerializeField]
    private TMPro.TextMeshProUGUI durationText;

    private const float minDuration = 0.25f;
    private const float maxDuration = 5.0f;

    [Header("Ease Selection")]
    [SerializeField]
    private Button previousEaseButton;
    [SerializeField]
    private Button nextEaseButton;
    [SerializeField]
    private TMPro.TMP_Dropdown easeDropdown;
    [Space]
    [Header("Ease Presentations")]
    [Space]
    [Header("Line Chart")]
    [SerializeField]
    private LineRenderer chartLineRenderer;
    [SerializeField]
    private LineRenderer xAxisLineRenderer;
    [SerializeField]
    private LineRenderer yAxisLineRenderer;
    [SerializeField]
    private RectTransform progressMark;
    [SerializeField]
    private TMPro.TextMeshProUGUI valueText;

    private float lineChartWidth = 4.0f;
    private float lineChartHeight = 2.0f;

    private const int linePositionCount = 100;
    private const float linePositionCountInv = 1.0f / linePositionCount;
    private readonly Vector3[] linePositions = new Vector3[linePositionCount + 1];

    [Header("Position")]
    [SerializeField]
    private Transform positionEasingFromTransform;
    [SerializeField]
    private Transform positionEasingToTransform;
    [SerializeField]
    private Transform positionEasingTransform;
    [Header("Scale")]
    [SerializeField]
    private Transform scaleEasingTransform;
    [Header("Rotation")]
    [SerializeField]
    private Transform rotationEasingTransform;
    [Header("Color")]
    [SerializeField]
    private Image colorEasingImage;
    [SerializeField]
    private Image transparencyEasingImage;
    [Header("Sound")]
    [SerializeField]
    private AudioSource soundAudioSource;
    [SerializeField]
    private Button playSoundButton;
    private TMPro.TextMeshProUGUI playSoundButtonText;
    [SerializeField]
    private Button soundVolumeButton;
    private TMPro.TextMeshProUGUI soundVolumeButtonText;
    [SerializeField]
    private Button soundPitchButton;
    private TMPro.TextMeshProUGUI soundPitchButtonText;

    private bool isPlaying = true;
    private bool loop = true;

    private Ease ease = Ease.None;
    private Ease? previousEase;

    private float progress = 0.0f;
    private float? previousProgress;

    private float time;
    private float duration = 1.0f;
    private bool progressForward = true;

    private bool easeSoundVolume = true;
    private bool easeSoundPitch = true;


    private void Awake()
    {
        if (chartLineRenderer)
        {
            if (chartLineRenderer.TryGetComponent(out RectTransform chartRectTransform))
            {
                lineChartWidth = chartRectTransform.rect.width / chartRectTransform.localScale.x;
                lineChartHeight = chartRectTransform.rect.height / chartRectTransform.localScale.y;
            }

            chartLineRenderer.positionCount = linePositions.Length;
        }
        if (xAxisLineRenderer)
        {
            xAxisLineRenderer.SetPosition(1, new Vector3(lineChartWidth, 0.0f, 0.0f));
        }
        if (yAxisLineRenderer)
        {
            yAxisLineRenderer.SetPosition(1, new Vector3(0.0f, lineChartHeight, 0.0f));
        }

        if (previousEaseButton)
        {
            previousEaseButton.onClick.AddListener(OnPreviousEaseButtonPressed);
        }
        if (nextEaseButton)
        {
            nextEaseButton.onClick.AddListener(OnNextEaseButtonPressed);
        }
        if (easeDropdown)
        {
            easeDropdown.ClearOptions();
            easeDropdown.AddOptions(new List<string>(System.Enum.GetNames(typeof(Ease))));
            easeDropdown.SetValueWithoutNotify((int)ease);
            easeDropdown.onValueChanged.AddListener(OnEaseDropdownValueChanged);
        }

        if (playButton)
        {
            var icon = playButton.transform.Find("Icon");
            if (icon && icon.TryGetComponent(out Image image))
            {
                playButtonIconImage = image;
                playButtonIconImage.sprite = playSprite;
            }
            playButton.onClick.AddListener(OnPlayButtonPressed);
        }
        if (loopButton)
        {
            var icon = loopButton.transform.Find("Icon");
            if (icon && icon.TryGetComponent(out Image image))
            {
                loopButtonIconImage = image;
                loopButtonIconImage.sprite = loopOffSprite;
            }
            loopButton.onClick.AddListener(OnLoopButtonPressed);
        }

        if (progressInputField)
        {
            progressInputField.onValueChanged.AddListener(OnProgressInputValueChanged);
        }
        if (progressSlider)
        {
            progressSlider.minValue = 0.0f;
            progressSlider.maxValue = 1.0f;
            progressSlider.onValueChanged.AddListener(OnProgressSliderValueChanged);
        }
        if (durationSlider)
        {
            durationSlider.minValue = minDuration;
            durationSlider.maxValue = maxDuration;
            durationSlider.onValueChanged.AddListener(OnDurationSliderValueChanged);
            durationSlider.value = duration;
        }

        if (playSoundButton)
        {
            playSoundButtonText = playSoundButton.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            playSoundButton.onClick.AddListener(OnPlaySoundButtonPressed);
        }
        if (soundVolumeButton)
        {
            soundVolumeButtonText = soundVolumeButton.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            soundVolumeButtonText.SetText(easeSoundVolume ? "Volume: ON" : "Volume: OFF");
            soundVolumeButton.onClick.AddListener(OnSoundVolumeButtonPressed);
        }
        if (soundPitchButton)
        {
            soundPitchButtonText = soundPitchButton.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            soundPitchButtonText.SetText(easeSoundPitch ? "Pitch: ON" : "Pitch: OFF");
            soundPitchButton.onClick.AddListener(OnSoundPitchButtonPressed);
        }

        OnPlayStateChanged();
    }
    private void Update()
    {
        if (isPlaying)
        {
            if (!loop)
            {
                progressForward = true;
            }

            time += progressForward ? Time.deltaTime : -Time.deltaTime;
            if (time > duration)
            {
                if (loop)
                {
                    progressForward = false;
                }
                else
                {
                    isPlaying = false;
                    OnPlayStateChanged();
                    time = duration;
                }
            }
            else if (time < 0.0f)
            {
                if (loop)
                {
                    progressForward = true;
                }
            }

            progress = Mathf.Clamp01(time / duration);
        }

        if (!previousEase.HasValue || previousEase.Value != ease)
        {
            previousEase = ease;
            OnEaseChanged();
        }
        if (!previousProgress.HasValue || previousProgress.Value != progress)
        {
            previousProgress = progress;
            OnProgressChanged();
        }
    }
    private void OnDestroy()
    {
        if (previousEaseButton)
        {
            previousEaseButton.onClick.RemoveAllListeners();
        }
        if (nextEaseButton)
        {
            nextEaseButton.onClick.RemoveAllListeners();
        }
        if (easeDropdown)
        {
            easeDropdown.onValueChanged.RemoveAllListeners();
        }

        if (playButton)
        {
            playButton.onClick.RemoveAllListeners();
        }
        if (loopButton)
        {
            loopButton.onClick.RemoveAllListeners();
        }
        if (progressInputField)
        {
            progressInputField.onValueChanged.RemoveAllListeners();
        }
        if (progressSlider)
        {
            progressSlider.onValueChanged.RemoveAllListeners();
        }
        if (durationSlider)
        {
            durationSlider.onValueChanged.RemoveAllListeners();
        }

        if (playSoundButton)
        {
            playSoundButton.onClick.RemoveAllListeners();
        }
        if (soundVolumeButton)
        {
            soundVolumeButton.onClick.RemoveAllListeners();
        }
        if (soundPitchButton)
        {
            soundPitchButton.onClick.RemoveAllListeners();
        }
    }

    #region UI Controls Events

    private void OnPreviousEaseButtonPressed()
    {
        var count = System.Enum.GetValues(typeof(Ease)).Length;
        int easeInt = (int)ease;
        if (--easeInt < 0)
        {
            easeInt = count + easeInt;
        }
        ease = (Ease)easeInt;
        easeDropdown.SetValueWithoutNotify((int)ease);
    }
    private void OnNextEaseButtonPressed()
    {
        var count = System.Enum.GetValues(typeof(Ease)).Length;
        int easeInt = (int)ease;
        if (++easeInt >= count)
        {
            easeInt = easeInt - count;
        }
        ease = (Ease)easeInt;
        easeDropdown.SetValueWithoutNotify((int)ease);
    }
    private void OnEaseDropdownValueChanged(int option)
    {
        ease = (Ease)option;
    }

    private void OnDurationSliderValueChanged(float value)
    {
        var oldDuration = duration;
        duration = Mathf.Clamp(value, minDuration, maxDuration);
        durationText.SetText($"{value.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)} s");
        time *= duration / oldDuration;
    }

    private void OnPlayButtonPressed()
    {
        isPlaying = !isPlaying;
        OnPlayStateChanged();
        if (isPlaying && progress >= 1.0f)
        {
            time = 0.0f;
        }
    }
    private void OnLoopButtonPressed()
    {
        loop = !loop;
        loopButtonIconImage.sprite = loop ? loopOnSprite : loopOffSprite;
    }
    private void OnProgressInputValueChanged(string value)
    {
        if (!string.IsNullOrEmpty(value) && float.TryParse(value.Replace(',', '.'), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float newProgress))
        {
            progress = Mathf.Clamp01(newProgress);
        }
    }
    private void OnProgressSliderValueChanged(float value)
    {
        progress = Mathf.Clamp01(value);
    }

    private void OnPlaySoundButtonPressed()
    {
        if (soundAudioSource.isPlaying)
        {
            soundAudioSource.Stop();
        }
        else
        {
            soundAudioSource.Play();
        }
        playSoundButtonText.SetText(soundAudioSource.isPlaying ? "Stop sound" : "Play sound");
    }
    private void OnSoundVolumeButtonPressed()
    {
        easeSoundVolume = !easeSoundVolume;
        if (!easeSoundVolume)
        {
            soundAudioSource.volume = 1.0f;
        }
        soundVolumeButtonText.SetText(easeSoundVolume ? "Volume: ON" : "Volume: OFF");
    }
    private void OnSoundPitchButtonPressed()
    {
        easeSoundPitch = !easeSoundPitch;
        if (!easeSoundPitch)
        {
            soundAudioSource.pitch = 1.0f;
        }
        soundPitchButtonText.SetText(easeSoundPitch ? "Pitch: ON" : "Pitch: OFF");
    }

    #endregion UI Controls Events

    private void OnPlayStateChanged()
    {
        playButtonIconImage.sprite = isPlaying ? pauseSprite : playSprite;
        progressInputField.interactable = progressSlider.interactable = !isPlaying;
    }
    private void OnEaseChanged()
    {
        // Redraw line chart
        for (int i = 0; i < linePositionCount; ++i)
        {
            float valueX = i * linePositionCountInv;
            linePositions[i] = new Vector3(valueX * lineChartWidth, Easing.Ease(valueX, ease) * lineChartHeight, 0.0f);
        }
        linePositions[^1] = new Vector3(lineChartWidth, Easing.Ease(1.0f, ease) * lineChartHeight, 0.0f);
        chartLineRenderer.SetPositions(linePositions);

        easeDropdown.SetValueWithoutNotify((int)ease);

        OnProgressChanged(); // refresh current ease values
    }
    private void OnProgressChanged()
    {
        // Get eased value for current progress, where progress is 0.0f-1.0f value calculated in the Update() method as time/duration
        float value = Easing.Ease(progress, ease);

        // Update player controls
        progressSlider.SetValueWithoutNotify(progress);
        progressInputField.SetTextWithoutNotify(progress.ToString(System.Globalization.CultureInfo.InvariantCulture));

        // Update line chart progress marker
        progressMark.anchoredPosition = new Vector3(progress * lineChartWidth * chartLineRenderer.transform.localScale.x, value * lineChartHeight * chartLineRenderer.transform.localScale.y, 0.0f);

        // Show value
        valueText.SetText(value.ToString());

        // Ease position - smoothly interpolate between two positions using the selected easing
        positionEasingTransform.localPosition = Vector3.LerpUnclamped(positionEasingFromTransform.localPosition, positionEasingToTransform.localPosition, value);
        // Ease rotation - rotate from 0 to 180 degrees around Z axis
        rotationEasingTransform.localRotation = Quaternion.SlerpUnclamped(Quaternion.identity, Quaternion.Euler(0.0f, 0.0f, 180.0f), value);
        // Ease scale - scale uniformly in all dimensions
        scaleEasingTransform.localScale = Vector3.one * value;

        // Ease color - transition from white to red
        colorEasingImage.color = Color.LerpUnclamped(Color.white, Color.red, value);
        // Ease transparency - fade out from fully visible (1.0) to invisible (0.0)
        Color color = transparencyEasingImage.color;
        color.a = Mathf.LerpUnclamped(1.0f, 0.0f, value); // lerp from 1.0 to 0.0 to create fade out
        transparencyEasingImage.color = color;

        // Ease sound volume - fade in from quiet (0.1) to full volume (1.0)
        if (easeSoundVolume)
        {
            soundAudioSource.volume = Mathf.LerpUnclamped(0.1f, 1.0f, value); // lerp from 0.1 to 1.0 so the sound won't be fully muted in the sample
        }
        // Ease sound pitch - shift from low pitch (0.1) to high pitch (2.0)
        if (easeSoundPitch)
        {
            soundAudioSource.pitch = Mathf.LerpUnclamped(0.1f, 2.0f, value); // lerp from 0.1 to 2.0 to create the feeling of sound slowing down and speeding up
        }
    }
}
