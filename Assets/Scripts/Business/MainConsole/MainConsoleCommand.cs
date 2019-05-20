using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZCore;

public class MainConsoleCommand : Command { }

public class ShowMainConsoleCommand : MainConsoleCommand { }

public class CloseMainConsoleCommand: MainConsoleCommand { }

public class RegisterHoldHandlerCommand : MainConsoleCommand { }

public class UnRegisterHoldHandlerCommand : MainConsoleCommand { }

public class GetDisplayModeCommand : MainConsoleCommand { }

public class GetSelectModeCommand : MainConsoleCommand { }

public class ShowHelpDialogCommand : MainConsoleCommand { }
