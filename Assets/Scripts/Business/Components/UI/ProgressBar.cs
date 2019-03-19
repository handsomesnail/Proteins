using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {

    /// <summary>当前进度</summary>
    [Range(0,1)]
    public float Value = 0;

    [SerializeField]
    private Slider slider;

    [SerializeField]
    private Text text;

    public UnityEvent unityEvent;

    private void Start() {
        Init();
    }

    private void Update() {
        if (slider.value < Value * 100) {
            slider.value += 1;
            text.text = string.Format("{0}%", slider.value);
        }
    }

    public void Init() {
        slider.minValue = 0;
        slider.wholeNumbers = true;
        slider.maxValue = 100;
        slider.value = 0;
        Value = 0;
        text.text = "0%";
    }

}
