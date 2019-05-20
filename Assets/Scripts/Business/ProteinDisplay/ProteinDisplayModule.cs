using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZCore;

public class ProteinDisplayModule : Module {

    /// <summary>显示蛋白质模型 </summary>
    public void OnShowProteinCommand(ShowProteinCommand cmd) {
        GetController<ProteinDisplayController>().ShowProtein();
    }

    /// <summary>显示DisplayView </summary>
    public void OnShowDisplayViewCommand(ShowDisplayViewCommand cmd) {
        GetController<ProteinDisplayController>().ShowDisplayView();
    }

    /// <summary>设置选中的Displayer </summary>
    public void OnSetSelectedDisplayerCommand(SetSelectedDisplayerCommand cmd) {
        GetController<ProteinDisplayController>().SetSelectedDisplayer(cmd.Displayer);
    }

    public IDisplayerSelected OnGetSelectedDisplayerCommand(GetSelectedDisplayerCommand cmd) {
        return GetController<ProteinDisplayController>().GetSelectedDisplayer();
    }

    /// <summary>设置PolymerBoard的Active </summary>
    public void OnSetPolymerInfoDisplayerActiveCommand(SetPolymerInfoDisplayerActiveCommand cmd) {
        GetController<ProteinDisplayController>().SetPolymerInfoDisplayerActive(cmd.Active);
    }

}
