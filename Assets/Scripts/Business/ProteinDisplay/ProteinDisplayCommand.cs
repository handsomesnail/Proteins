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