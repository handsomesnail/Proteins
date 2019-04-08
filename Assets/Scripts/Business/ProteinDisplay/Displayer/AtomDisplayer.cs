using HoloToolkit.Unity.InputModule;
using PolymerModel.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZCore;

public class AtomDisplayer : MonoBehaviour, IInputClickHandler, IDisplayerSelected {

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
        Debug.Log("AtomClick");
        SelectMode selectMode = SelectMode.Atom;//从数据获取SelectMode
        if(selectMode == SelectMode.Atom) {
            OnSelected();
            //设置DisplayMode数据
        }
        else {
            AminoacidDisplayer aminoacidDisplayer = transform.parent.GetComponent<AminoacidDisplayer>();
            aminoacidDisplayer.OnInputClicked(eventData);
        }
    }

    public void OnSelected() {
        //设置高亮材质
    }

    public void OnUnSelected() {
        //取消高亮材质
    }



    //private void OnMouseEnter() {
    //    CoreAPI.SendCommand<ProteinDisplayModule, ShowInfoInBoardCommand>(new ShowInfoInBoardCommand(this));
    //}

    //private void OnMouseExit() {
    //    CoreAPI.SendCommand<ProteinDisplayModule, ShowInfoInBoardCommand>(new ShowInfoInBoardCommand(null));
    //}


}
