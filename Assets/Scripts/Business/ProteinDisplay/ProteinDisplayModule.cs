using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZCore;

public class ProteinDisplayModule : Module {

    public void OnShowProteinCommand(ShowProteinCommand cmd) {
        GetController<ProteinDisplayController>().ShowProtein();
    }

    public void OnShowDisplayViewCommand(ShowDisplayViewCommand cmd) {
        GetController<ProteinDisplayController>().ShowDisplayView();
    }

    public void OnShowInfoInBoardCommand(ShowInfoInBoardCommand cmd) {
        GetController<ProteinDisplayController>().ShowInfoInBoard(cmd.AtomDisplayer);
    }

    public void OnSetDisplayModeCommand(SetDisplayModeCommand cmd) {
        GetController<ProteinDisplayController>().SetDisplayMode(cmd.DisplayMode);
    }

}
