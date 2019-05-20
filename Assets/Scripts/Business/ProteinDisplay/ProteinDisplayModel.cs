using PolymerModel.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZCore;

public enum DisplayMode {
    Spacefill = 0, //球模型
    BallStick, //球棍模型
    Sticks, //棍状模型
}

/// <summary>选取模式</summary>
public enum PolymerSelectMode {
    Chain = 0, //选取链
    Residue, //选取残基
    Atom //选取原子
}

/// <summary>该Model中的数据相对于MainConsole和PdbLoader中的数据有所延迟</summary>
/// 只有在数据更新时才会更新View
public class ProteinDisplayModel : Model {

    /// <summary>当前显示的蛋白质数据</summary>
    public Protein DisplayedProteinData { get; set; } = null;

    /// <summary>当前选中的Displayer</summary>
    public IDisplayerSelected SelectedDisplayer { get; set; } = null;

    /// <summary>当前显示的模式</summary>
    public DisplayMode DisplayedDisplayMode { get; set; } = DisplayMode.BallStick;


}
