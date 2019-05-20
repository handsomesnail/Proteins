using HoloToolkit.Unity.InputModule;
using PolymerModel.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZCore;

public class AminoacidDisplayer : MonoBehaviour, IInputClickHandler, IDisplayerSelected {

    public AminoacidInProtein AminoacidInProtein {
        get; set;
    }

    public Material Normal { get; set; }

    public Material HighLight { get; set; }

    public void OnInputClicked(InputClickedEventData eventData) {
        PolymerSelectMode selectMode = CoreAPI.PostCommand<MainConsoleModule, GetSelectModeCommand, PolymerSelectMode>(new GetSelectModeCommand());
        if (selectMode == PolymerSelectMode.Residue) {
            OnSelected();
            CoreAPI.SendCommand<ProteinDisplayModule, SetSelectedDisplayerCommand>(new SetSelectedDisplayerCommand(this));
        }
        else {
            ChainDisplayer chainDisplayer = transform.parent.GetComponent<ChainDisplayer>();
            chainDisplayer.OnInputClicked(eventData);
        }
    }

    public void OnSelected() {
        if (this == null)
            return;
        Renderer renderer = GetComponent<Renderer>();
        renderer.sharedMaterial = HighLight;
    }

    public void OnUnSelected() {
        if (this == null)
            return;
        Renderer renderer = GetComponent<Renderer>();
        renderer.sharedMaterial = Normal;
    }

    /// <summary>残基的某个子物体被选中</summary>
    public void OnChildSelected() {
        if (this == null)
            return;
        //禁用残基Renderer 开启所有子物体Renderer
        GetComponent<Renderer>().enabled = false;
        foreach (Transform child in this.transform) {
            IDisplayerSelected displayer = child.GetComponent<IDisplayerSelected>();
            if (displayer != null) {
                if (displayer is AtomDisplayer) {
                    child.GetComponent<Renderer>().enabled = true;
                }
                else if (displayer is BondDisplayer) {
                    foreach (Transform bond in child) {
                        bond.GetComponent<Renderer>().enabled = true;
                    }
                }
                else throw new System.Exception();
            }
        }
    }

    public void OnChildCancelSelected() {
        if (this == null)
            return;
        //启用残基Renderer 禁用所有子物体Renderer
        GetComponent<Renderer>().enabled = true;
        foreach (Transform child in this.transform) {
            IDisplayerSelected displayer = child.GetComponent<IDisplayerSelected>();
            if (displayer != null) {
                if (displayer is AtomDisplayer) {
                    child.GetComponent<Renderer>().enabled = false;
                }
                else if (displayer is BondDisplayer) {
                    foreach (Transform bond in child) {
                        bond.GetComponent<Renderer>().enabled = false;
                    }
                }
                else throw new System.Exception();
            }
        }
    }
}
