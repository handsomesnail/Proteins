using HoloToolkit.Unity.InputModule;
using PolymerModel.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZCore;

public class AtomDisplayer : MonoBehaviour, IInputClickHandler, IDisplayerSelected {

    [SerializeField]
    public Material normal;
    [SerializeField]
    public Material highLight;

    private void Start() {
        
    }

    /// <summary>该原子位于蛋白质中的哪个氨基酸</summary>
    public AminoacidInProtein AminoacidInProtein {
        get; set;
    }

    /// <summary>该原子位于氨基酸中的哪个原子</summary>
    public AtomInAminoacid AtomInAminoacid {
        get; set;
    }

    public void OnInputClicked(InputClickedEventData eventData) {
        PolymerSelectMode selectMode = CoreAPI.PostCommand<MainConsoleModule, GetSelectModeCommand, PolymerSelectMode>(new GetSelectModeCommand());
        AminoacidDisplayer aminoacidDisplayer = transform.parent.GetComponent<AminoacidDisplayer>();
        if (selectMode == PolymerSelectMode.Atom) {
            CoreAPI.SendCommand<ProteinDisplayModule, SetSelectedDisplayerCommand>(new SetSelectedDisplayerCommand(this));
        }
        else {
            aminoacidDisplayer.OnInputClicked(eventData);
        }
    }

    public void OnSelected() {
        //设置高亮材质
        if (this == null)
            return;
        AminoacidDisplayer aminoacidDisplayer = transform.parent.GetComponent<AminoacidDisplayer>();
        aminoacidDisplayer.OnChildSelected();
        Renderer renderer = GetComponent<Renderer>();
        renderer.sharedMaterial = highLight;
        renderer.enabled = true;
    }

    public void OnUnSelected() {
        //取消高亮材质
        if (this == null)
            return;
        AminoacidDisplayer aminoacidDisplayer = transform.parent.GetComponent<AminoacidDisplayer>();
        aminoacidDisplayer.OnChildCancelSelected();
        Renderer renderer = GetComponent<Renderer>();
        renderer.sharedMaterial = normal;
        renderer.enabled = false;
    }

}
