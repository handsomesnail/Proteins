using PolymerModel.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BondDisplayer : MonoBehaviour, IDisplayerSelected {

    public BondType BondType;

    public void OnSelected() {
        //设置高亮材质
    }

    public void OnUnSelected() {
        //取消高亮材质
    }
}
