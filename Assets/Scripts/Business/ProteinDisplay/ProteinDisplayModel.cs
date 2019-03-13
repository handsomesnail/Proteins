using PolymerModel.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZCore;

public enum DisplayMode {
    Spacefill = 0, //球模型
    BallStick, //球棍模型
}

public class ProteinDisplayModel : Model {

    public Protein DisplayedProteinData { get; set; }

}
