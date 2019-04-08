using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZCore;

public class ProteinDisplayCommand : Command { }

public class ShowProteinCommand : ProteinDisplayCommand { }

public class ShowDisplayViewCommand : ProteinDisplayCommand { }

public class ShowInfoInBoardCommand : ProteinDisplayCommand {

    public AtomDisplayer AtomDisplayer { get; private set; }

    public ShowInfoInBoardCommand(AtomDisplayer atomDisplayer) {
        this.AtomDisplayer = atomDisplayer;
    }

}

public class SetDisplayModeCommand : ProteinDisplayCommand {
    public DisplayMode DisplayMode { get; private set; }

    public SetDisplayModeCommand(DisplayMode displayMode) {
        this.DisplayMode = DisplayMode;
    }
}