using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.UX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZCore;

public class MainConsoleController : Controller, ISpeechHandler, INavigationHandler {

    #region ImplementedInController

    public void OnClickHelpButton() {
        StartCoroutine(LaunchHelpDialog());
    }

    public void OnClickAdjustButton() {
        MainConsoleView view = GetView<MainConsoleView>();
        MainConsoleModel model = GetModel<MainConsoleModel>();
        BoundingBoxRig boundingBoxRig = view.GetComponent<BoundingBoxRig>();
        boundingBoxRig.Activate();
        model.State = MainConsoleStateEnum.Manipulation;
        view.StartAdjust();
    }

    public void OnClickDoneButton() {
        MainConsoleView view = GetView<MainConsoleView>();
        MainConsoleModel model = GetModel<MainConsoleModel>();
        BoundingBoxRig boundingBoxRig = view.GetComponent<BoundingBoxRig>();
        boundingBoxRig.Deactivate();
        model.State = MainConsoleStateEnum.Default;
        view.DoneAdjust();
    }

    public void OnClickCloseButton() {
        HideMainConsole();
    }

    public void OnClickSelectFileButton() {
        MainConsoleView view = GetView<MainConsoleView>();
        CoreAPI.SendCommand<PdbLoaderModule, LoadLocalPdbFileCommand>(new LoadLocalPdbFileCommand((id) => {
            view.SetPDBFileNameText(id);
            CoreAPI.SendCommand<ProteinDisplayModule, ShowProteinCommand>(new ShowProteinCommand());
        }));
    }

    public void OnClickDownloadFileButton() {
        Debug.Log("DownloadFile");
    }

    public void OnClickExampleButton() {
        string pdbName = "2nc3";
        MainConsoleView view = GetView<MainConsoleView>();
        CoreAPI.SendCommand<PdbLoaderModule, LoadDefaultPdbFileCommand>(new LoadDefaultPdbFileCommand(pdbName, () => {
            view.SetPDBFileNameText(pdbName);
            CoreAPI.SendCommand<ProteinDisplayModule, ShowProteinCommand>(new ShowProteinCommand());
        }));
    }

    public void OnSelectedDisplayMode() {
        MainConsoleView view = GetView<MainConsoleView>();
        MainConsoleModel model = GetModel<MainConsoleModel>();
        DisplayMode displayMode = default(DisplayMode);
        string keyword = view.GetSelectedDisplayMode();
        if (keyword == "BallStick") {
            displayMode = DisplayMode.BallStick;
        }
        else if (keyword == "SpaceFill") {
            displayMode = DisplayMode.Spacefill;
        }
        else if(keyword == "Sticks") {
            displayMode = DisplayMode.Sticks;
        }
        else throw new System.Exception(string.Format("Unknown radial keyword : {0}", keyword));
        model.DisplayMode = displayMode;
        CoreAPI.SendCommand<ProteinDisplayModule, ShowProteinCommand>(new ShowProteinCommand());
    }

    public void OnSelectedSelectMode() {
        MainConsoleView view = GetView<MainConsoleView>();
        MainConsoleModel model = GetModel<MainConsoleModel>();
        PolymerSelectMode selectMode = default(PolymerSelectMode);
        string keyword = view.GetSelectedSelectMode();
        if (keyword == "Chain") {
            selectMode = PolymerSelectMode.Chain;
        }
        else if (keyword == "Residue") {
            selectMode = PolymerSelectMode.Residue;
        }
        else if(keyword == "Atom") {
            selectMode = PolymerSelectMode.Atom;
        }
        else throw new System.Exception(string.Format("Unknown radial keyword : {0}", keyword));
        model.SelectMode = selectMode;
    }

    public void OnChangedFollowAround(bool changed) {
        MainConsoleView view = GetView<MainConsoleView>();
        MainConsoleModel model = GetModel<MainConsoleModel>();
        model.IsFollow = changed;
        view.IsFollow = changed;
    }

    private GameObject FpsDisplayGameObject;
    public void OnChangedShowFps(bool changed) {
        MainConsoleModel model = GetModel<MainConsoleModel>();
        model.IsShowFps = changed;
        if (FpsDisplayGameObject == null)
            FpsDisplayGameObject = GameObject.FindGameObjectWithTag("FPSDisplay");
        FpsDisplayGameObject.Active(changed);
    }

    public void OnChangedSoundVolume(float value) { }

    public void OnClickAboutButton() {
        StartCoroutine(LaunchOptionHelpDialog(4));
    }

    public void OnClickExitButton() {
        Application.Quit();
    }

    public void OnClickDisplayModeHelpButton() {
        StartCoroutine(LaunchOptionHelpDialog(0));
    }

    public void OnClickSelectModeHelpButton() {
        StartCoroutine(LaunchOptionHelpDialog(1));
    }

    public void OnClickFollowAroundHelpButton() {
        StartCoroutine(LaunchOptionHelpDialog(2));
    }

    public void OnClickShowFPSHelpButton() {
        StartCoroutine(LaunchOptionHelpDialog(3));
    }

    public void OnClickDownloadButton() {
        MainConsoleView view = GetView<MainConsoleView>();
        string input = view.GetInputPdbId();
        CoreAPI.SendCommand<PdbLoaderModule, LoadNetworkPdbFileCommand>(new LoadNetworkPdbFileCommand(input, ()=> {
            view.SetPDBFileNameText(input);
            CoreAPI.SendCommand<ProteinDisplayModule, ShowProteinCommand>(new ShowProteinCommand());
        }));
    }

    #endregion

    #region Interface

    public void OnSpeechKeywordRecognized(SpeechEventData eventData) {
        string recognizedText = eventData.RecognizedText;
        if(recognizedText.ToLower() == "console") {
            ShowMainConsole();
        }
    }

    public void OnNavigationStarted(NavigationEventData eventData) { }

    public void OnNavigationUpdated(NavigationEventData eventData) { }

    public void OnNavigationCompleted(NavigationEventData eventData) {
        ShowMainConsole();
    }

    public void OnNavigationCanceled(NavigationEventData eventData) { }


    #endregion

    #region Public Methods

    public void ShowMainConsole() {
        MainConsoleView view = GetView<MainConsoleView>();
        MainConsoleModel model = GetModel<MainConsoleModel>();
        view.gameObject.Active(true);
        InputManager.Instance.RemoveGlobalListener(this.gameObject);
        CoreAPI.SendCommand<ProteinDisplayModule, SetPolymerInfoDisplayerActiveCommand>(new SetPolymerInfoDisplayerActiveCommand(false));
    }

    public void HideMainConsole() {
        MainConsoleView view = GetView<MainConsoleView>();
        view.gameObject.Active(false); //GameObject在OnDisable动画播放完毕后再SetActive false
        InputManager.Instance.AddGlobalListener(this.gameObject);
        CoreAPI.SendCommand<ProteinDisplayModule, SetPolymerInfoDisplayerActiveCommand>(new SetPolymerInfoDisplayerActiveCommand(true));
    }

    public DisplayMode GetDisplayMode() {
        MainConsoleModel model = GetModel<MainConsoleModel>();
        return model.DisplayMode;
    }

    public PolymerSelectMode GetSelectMode() {
        MainConsoleModel model = GetModel<MainConsoleModel>();
        //Sticks模式不支持选取单个原子
        if (model.DisplayMode == DisplayMode.Sticks && model.SelectMode == PolymerSelectMode.Atom)
            return PolymerSelectMode.Residue;
        else return model.SelectMode;
    }

    #endregion

    #region Private/Protected Methods

    private IEnumerator LaunchHelpDialog() {
        MainConsoleModel model = GetModel<MainConsoleModel>();
        if (model.IsHelpDialogLaunched)
            yield break;
        model.IsHelpDialogLaunched = true;
        MainConsoleView view = GetView<MainConsoleView>();
        view.Freeze();
        yield return view.LaunchHelpDialog();
        view.Recovery();
        model.IsHelpDialogLaunched = false;
    }

    private IEnumerator LaunchOptionHelpDialog(int optionId) {
        MainConsoleModel model = GetModel<MainConsoleModel>();
        if (model.IsOptionHelpDialogLaunched)
            yield break;
        model.IsOptionHelpDialogLaunched = true;
        MainConsoleView view = GetView<MainConsoleView>();
        view.Freeze();
        switch (optionId) {
            case 0: yield return view.LaunchDisplayModeHelpDialog(); break;
            case 1: yield return view.LaunchSelectModeHelpDialog(); break;
            case 2: yield return view.LaunchFollowAroundHelpDialog(); break;
            case 3: yield return view.LaunchShowFPSHelpDialog(); break;
            case 4: yield return view.LaunchAboutDialog();break;
            default: throw new System.Exception();
        }
        view.Recovery();
        model.IsOptionHelpDialogLaunched = false;
    }

    #endregion

}
