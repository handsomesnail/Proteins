using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZCore;

/// <summary>主控制台(可随时呼出的操作菜单)</summary>
public class MainConsoleModule : Module {

    public void OnRegisterHoldHandlerCommand(RegisterHoldHandlerCommand cmd) {
        InputManager.Instance.AddGlobalListener(GetController<MainConsoleController>().gameObject);
    }

   /// <summary>显示主控制台</summary>
    public void OnShowMainConsoleCommand(ShowMainConsoleCommand cmd) {
        GetController<MainConsoleController>().ShowMainConsole();
    }

    /// <summary>关闭主控制台(隐藏)</summary>
    public void OnCloseMainConsoleCommand(CloseMainConsoleCommand cmd) {
        GetController<MainConsoleController>().HideMainConsole();
    }

    /// <summary>获取当前Display模式 </summary>
    public DisplayMode OnGetDisplayModeCommand(GetDisplayModeCommand cmd) {
        return GetController<MainConsoleController>().GetDisplayMode();
    }

    /// <summary>获取当前Select模式 </summary>
    public SelectMode OnGetSelectModeCommand(GetSelectModeCommand cmd) {
        return GetController<MainConsoleController>().GetSelectMode();
    }




}
