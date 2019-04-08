using PolymerModel.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZCore;

public enum DisplayMode {
    Spacefill = 0, //球模型
    BallStick, //球棍模型
}

/// <summary>选取模式</summary>
public enum SelectMode {
    Chain = 0, //选取链
    Residue, //选取残基
    Atom //选取原子
}

/// <summary>该Model中的数据相对于MainConsole和PdbLoader中的数据有所延迟</summary>
/// 只有在数据更新时才会更新View
public class ProteinDisplayModel : Model {

    public Protein DisplayedProteinData { get; set; } = null;

    public DisplayMode DisplayedDisplayMode { get; set; } = DisplayMode.BallStick;

    public SelectMode DIsplayedSelectMode { get; set; } = SelectMode.Atom;

}
