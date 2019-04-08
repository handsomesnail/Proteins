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
        Debug.Log("SelectFile");
    }

    public void OnClickDownloadFileButton() {
        Debug.Log("DownloadFile");
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
        else throw new System.Exception(string.Format("Unknown radial keyword : {0}", keyword));
        model.DisplayMode = displayMode;
        //Set ProteinDisplayModule Mode 
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
        Debug.Log("About");
    }

    public void OnClickExitButton() {
        Application.Quit();
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
    }

    public void HideMainConsole() {
        MainConsoleView view = GetView<MainConsoleView>();
        view.gameObject.Active(false); //GameObject在OnDisable动画播放完毕后再SetActive false
        InputManager.Instance.AddGlobalListener(this.gameObject);
    }

    public DisplayMode GetDisplayMode() {
        MainConsoleModel model = GetModel<MainConsoleModel>();
        return model.DisplayMode;
    }

    public SelectMode GetSelectMode() {
        MainConsoleModel model = GetModel<MainConsoleModel>();
        return model.SelectMode;
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

    #endregion

}
