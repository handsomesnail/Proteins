using HoloToolkit.Unity.InputModule;
using PolymerModel.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AminoacidDisplayer : MonoBehaviour, IInputClickHandler, IDisplayerSelected {

    private void Start() {
        
    }

    public AminoacidInProtein AminoacidInProtein {
        get; set;
    }

    public void OnInputClicked(InputClickedEventData eventData) {
        Debug.Log("Aminoacid Click");
        SelectMode selectMode = SelectMode.Atom;//从数据获取SelectMode
        if (selectMode == SelectMode.Residue) {
            OnSelected();
            //设置DisplayMode数据
        }
        else {
            ChainDisplayer chainDisplayer = transform.parent.GetComponent<ChainDisplayer>();
            chainDisplayer.OnInputClicked(eventData);
        }
    }

    public void OnSelected() {
        foreach(Transform child in this.transform) {
            IDisplayerSelected displayer = child.GetComponent<IDisplayerSelected>();
            if (displayer != null) {
                displayer.OnSelected();
            }
        }
    }

    public void OnUnSelected() {
        foreach (Transform child in this.transform) {
            IDisplayerSelected displayer = child.GetComponent<IDisplayerSelected>();
            if (displayer != null) {
                displayer.OnUnSelected();
            }
        }
    }
}
