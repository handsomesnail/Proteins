using HoloToolkit.Unity.InputModule;
using PolymerModel.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZCore;

public class BondDisplayer : MonoBehaviour, IDisplayerSelected, IInputClickHandler {

    public BondType BondType;

    public AtomDisplayer AttachedAtomDisplayer;

    public Material normal { get; set; }

    public Material highLight { get; set; }

    public void OnInputClicked(InputClickedEventData eventData) {
        PolymerSelectMode selectMode = CoreAPI.PostCommand<MainConsoleModule, GetSelectModeCommand, PolymerSelectMode>(new GetSelectModeCommand());
        DisplayMode displayMode = CoreAPI.PostCommand<MainConsoleModule, GetDisplayModeCommand, DisplayMode>(new GetDisplayModeCommand());
        //只有棍模型下的棍支持选取
        if (displayMode != DisplayMode.Sticks) {
            return;
        }
        AminoacidDisplayer aminoacidDisplayer = transform.parent.GetComponent<AminoacidDisplayer>();
        aminoacidDisplayer.OnInputClicked(eventData);

    }

    public void OnSelected() {
        if (this == null)
            return;
        AminoacidDisplayer aminoacidDisplayer = transform.parent.GetComponent<AminoacidDisplayer>();
        aminoacidDisplayer.OnChildSelected();
        foreach (Transform child in transform) {
            Renderer renderer = GetComponent<Renderer>();
            renderer.sharedMaterial = highLight;
            renderer.enabled = true;
        }
    }

    public void OnUnSelected() {
        if (this == null)
            return;
        AminoacidDisplayer aminoacidDisplayer = transform.parent.GetComponent<AminoacidDisplayer>();
        aminoacidDisplayer.OnChildCancelSelected();
        foreach (Transform child in transform) {
            Renderer renderer = GetComponent<Renderer>();
            renderer.sharedMaterial = normal;
            renderer.enabled = false;
        }
    }
}
