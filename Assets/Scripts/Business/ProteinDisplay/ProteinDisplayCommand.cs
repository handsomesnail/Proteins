using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZCore;

public class ProteinDisplayCommand : Command { }

public class ShowProteinCommand : ProteinDisplayCommand { }

public class ShowDisplayViewCommand : ProteinDisplayCommand { }

public class SetSelectedDisplayerCommand : ProteinDisplayCommand {

    public IDisplayerSelected Displayer { get; private set; }

    public SetSelectedDisplayerCommand(IDisplayerSelected displayer) {
        this.Displayer = displayer;
    }

}

public class GetSelectedDisplayerCommand: ProteinDisplayCommand { }

public class SetPolymerInfoDisplayerActiveCommand : ProteinDisplayCommand {
    public bool Active { get; private set; }

    public SetPolymerInfoDisplayerActiveCommand(bool active) {
        this.Active = active;
    }
}