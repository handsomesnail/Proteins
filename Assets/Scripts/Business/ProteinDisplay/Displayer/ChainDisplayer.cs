using HoloToolkit.Unity.InputModule;
using PolymerModel.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainDisplayer : MonoBehaviour, IInputClickHandler, IDisplayerSelected {

    public Chain Chain {
        get; set;
    }

    public void OnInputClicked(InputClickedEventData eventData) {

    }

    public void OnSelected() {
    }

    public void OnUnSelected() {

    }
}
