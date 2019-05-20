using HoloToolkit.Unity.InputModule;
using PolymerModel.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZCore;

public class ChainDisplayer : MonoBehaviour, IInputClickHandler, IDisplayerSelected {

    public Chain Chain {
        get; set;
    }

    public void OnInputClicked(InputClickedEventData eventData) {
        PolymerSelectMode selectMode = CoreAPI.PostCommand<MainConsoleModule, GetSelectModeCommand, PolymerSelectMode>(new GetSelectModeCommand());
        //Chain是可选取的最顶层，若事件传导到ChainDisplayer则一定是SelectMode.Chain 有可能选择的Bond传导上去了
        //if(selectMode!= SelectMode.Chain) {
        //    throw new System.Exception("Logic Error");
        //}
        CoreAPI.SendCommand<ProteinDisplayModule, SetSelectedDisplayerCommand>(new SetSelectedDisplayerCommand(this));
    }

    public void OnSelected() {
        if (this == null)
            return;
        foreach (Transform child in this.transform) {
            IDisplayerSelected displayer = child.GetComponent<IDisplayerSelected>();
            if (displayer != null) {
                displayer.OnSelected();
            }
        }
    }

    public void OnUnSelected() {
        if (this == null)
            return;
        foreach (Transform child in this.transform) {
            IDisplayerSelected displayer = child.GetComponent<IDisplayerSelected>();
            if (displayer != null) {
                displayer.OnUnSelected();
            }
        }
    }
}
