using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ZCore;

public class PolymerInfoItem : MonoBehaviour, IPointerSpecificFocusable {
    
    [SerializeField]
    private GameObject frontPlate;
    [SerializeField]
    private GameObject infoPanel;
    [SerializeField]
    private DisplayerType displayerType;

    public bool CanShow {
        get {
            IDisplayerSelected displayer = CoreAPI.PostCommand<ProteinDisplayModule, GetSelectedDisplayerCommand, IDisplayerSelected>(new GetSelectedDisplayerCommand());
            if (displayer == null)
                return false;
            PolymerSelectMode selectMode = CoreAPI.PostCommand<MainConsoleModule, GetSelectModeCommand, PolymerSelectMode>(new GetSelectModeCommand());
            switch (displayerType) {
                case DisplayerType.Protein: return true;
                case DisplayerType.Chain: return selectMode >= PolymerSelectMode.Chain; //恒为true
                case DisplayerType.Residue: return selectMode >= PolymerSelectMode.Residue;
                case DisplayerType.Atom: return selectMode >= PolymerSelectMode.Atom;
                default: return false;
            }
        }
    }

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
