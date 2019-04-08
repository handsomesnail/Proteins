using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PolymerInfoItem : MonoBehaviour, IPointerSpecificFocusable {
    
    [SerializeField]
    private GameObject frontPlate;
    [SerializeField]
    private GameObject infoPanel;

    public bool CanShow = true;

    public UnityEvent OnEnter;
    public UnityEvent OnExit;

    public void OnFocusEnter(PointerSpecificEventData eventData) {
        if (!CanShow)
            return;
        frontPlate.Active(true);
        infoPanel.Active(true);
        OnEnter.Invoke();
    }

    public void OnFocusExit(PointerSpecificEventData eventData) {
        frontPlate.Active(false);
        infoPanel.Active(false);
        OnExit.Invoke();
    }
}
