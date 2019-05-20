using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZCore;

public enum MainConsoleStateEnum {
    Default = 0,
    Manipulation, //正在调整
}

public class MainConsoleModel : Model {

    /// <summary>当前勾选的Display模式</summary>
    public DisplayMode DisplayMode { get; set; } = DisplayMode.BallStick;

    /// <summary>当前勾选的Select模式 </summary>
    public PolymerSelectMode SelectMode { get; set; } = PolymerSelectMode.Atom;

    public MainConsoleStateEnum State { get; set; } = MainConsoleStateEnum.Default;

    /// <summary>控制台是否自动跟随(和Adjust模式和控制移动转向有关)</summary>
    public bool IsFollow { get; set; } = true;

    public bool IsShowFps { get; set; } = true;

    public bool IsHelpDialogLaunched { get; set; } = false;

    public bool IsOptionHelpDialogLaunched { get; set; } = false;

}
