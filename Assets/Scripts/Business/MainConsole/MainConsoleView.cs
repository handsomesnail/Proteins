using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZCore;
using DG.Tweening;
using HoloToolkit.Unity;
using HoloToolkit.Unity.UX;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Examples.InteractiveElements;
using HoloToolkit.UX.Dialog;
using HoloToolkit.UI.Keyboard;

public class MainConsoleView : View {

    private static Vector3 oriOffsetToMainCamera = new Vector3(0.04f, -0.03f, 1); //初始化时的相对于主摄像机的offset
    private static Vector3 oriTargetOffsetToMainCamera = new Vector3(0, -0.03f, 1.2f); //初始化后移动的目标offset
    private static Vector3 oriLocalScale = new Vector3(0.4f, 0.4f, 0.4f); //初始化时的localScale
    private static Vector3 oriRotationOffset = new Vector3(30, -30, 0);

    private BoundingBoxRig boundingBoxRig;
    private HandDraggable handDraggable;
    //private Billboard billboard;
    //private Tagalong tagalong;
    private SolverHandler solverHandler;

    [Header("Resource Reference")]
    [SerializeField]
    private Dialog HelpDialogPrefeb;
    [SerializeField]
    private Dialog TipDialogPrefeb;

    private GameObject HelpDialog;

    [Header("UI Reference")]
    [SerializeField]
    private InteractiveSet DisplayModeRadialSet;
    [SerializeField]
    private InteractiveSet SelectModeRadialSet;
    [SerializeField]
    private TextMesh pdbFileNameText;
    [SerializeField]
    private KeyboardInputField keyboardInputField;

    private bool isFollow = true;
    public bool IsFollow {
        get { return isFollow; }
        set {
            isFollow = value;
            solverHandler.UpdateSolvers = value;
        }
    }


    #region ImplementedInController

    [ImplementedInController]
    public void OnClickCloseButton() { }

    [ImplementedInController]
    public void OnClickAdjustButton() { }

    [ImplementedInController]
    public void OnClickHelpButton() { }

    [ImplementedInController]
    public void OnClickDoneButton() { }

    [ImplementedInController]
    public void OnClickSelectFileButton() { }

    [ImplementedInController]
    public void OnClickDownloadFileButton() { }

    [ImplementedInController]
    public void OnClickExampleButton() { }

    [ImplementedInController]
    public void OnSelectedDisplayMode() { }

    [ImplementedInController]
    public void OnSelectedSelectMode() { }

    [ImplementedInController]
    public void OnChangedFollowAround(bool changed) { }

    [ImplementedInController]
    public void OnChangedShowFps(bool changed) { }

    [ImplementedInController]
    public void OnChangedSoundVolume(float value) { }

    [ImplementedInController]
    public void OnClickAboutButton() { }

    [ImplementedInController]
    public void OnClickExitButton() { }

    [ImplementedInController]
    public void OnClickDisplayModeHelpButton() { }

    [ImplementedInController]
    public void OnClickSelectModeHelpButton() { }

    [ImplementedInController]
    public void OnClickFollowAroundHelpButton() { }

    [ImplementedInController]
    public void OnClickShowFPSHelpButton() { }

    [ImplementedInController]
    public void OnClickDownloadButton() { }

    #endregion

    #region MonoBehaviour

    private void Awake() {
        boundingBoxRig = GetComponent<BoundingBoxRig>();
        handDraggable = GetComponent<HandDraggable>();
        solverHandler = GetComponent<SolverHandler>();
    }

    private void Start() {
        //只有在不启动自动跟随的情况下 控制台是可拖动的(在Adjust模式下)
        handDraggable.enabled = false; //非Adjust模式启动
        //在Follow模式下 跟随的脚本在动画播放完毕后启动
        solverHandler.UpdateSolvers = false;
    }

    private void OnEnable() {
        transform.DOKill();
        Freeze();
        Camera mainCamera = Camera.main;
        //设置Position
        transform.position = mainCamera.transform.position + oriOffsetToMainCamera.z * mainCamera.transform.forward
            + oriOffsetToMainCamera.y * mainCamera.transform.up + oriOffsetToMainCamera.x * mainCamera.transform.right;
        Vector3 targetPos = mainCamera.transform.position + oriTargetOffsetToMainCamera.z * mainCamera.transform.forward
            + oriTargetOffsetToMainCamera.y * mainCamera.transform.up + oriTargetOffsetToMainCamera.x * mainCamera.transform.right;

        //设置Rotation
        Quaternion oriQuaternion = mainCamera.transform.rotation * Quaternion.Euler(oriRotationOffset);
        Quaternion targetQuaternion = mainCamera.transform.rotation;
        Vector3 oriEulerAngles = new Vector3(oriQuaternion.eulerAngles.x, oriQuaternion.eulerAngles.y, 0);
        Vector3 targetEulerAngles = new Vector3(targetQuaternion.eulerAngles.x, targetQuaternion.eulerAngles.y, 0);
        transform.eulerAngles = oriEulerAngles;
        transform.DORotate(targetEulerAngles, 1.0f).SetEase(Ease.OutQuart);

        //设置Scale
        transform.localScale = oriLocalScale;
        transform.DOMove(targetPos, 1.0f).SetEase(Ease.OutQuart).OnComplete(() => {
            Recovery();
        });
    }

    private void OnDisable() {
        Freeze();
        transform.DOKill();
    }

    #endregion

    public IEnumerator LaunchHelpDialog() {
        yield return this.LaunchDialog(HelpDialogPrefeb, DialogButtonType.OK, "HELP",
            "You can call the console to modify options at any time by using navigation gestures. If you want to know more about navigation gestures, please refer to the manual of Hololens");
    }

    public IEnumerator LaunchDisplayModeHelpDialog() {
        yield return this.LaunchDialog(TipDialogPrefeb, DialogButtonType.OK, "DISPLAY MODE",
            "You can view the structure in different display styles,  while Spacefill, BallStick and Sticks are supported up to now");
    }

    public IEnumerator LaunchSelectModeHelpDialog() {
        yield return this.LaunchDialog(TipDialogPrefeb, DialogButtonType.OK, "SELECT MODE",
            "You can select different levels of the structure to get the detail,  while Chain, Residue and Atom are supported up to now");
    }

    public IEnumerator LaunchFollowAroundHelpDialog() {
        yield return this.LaunchDialog(TipDialogPrefeb, DialogButtonType.OK, "FOLLOW AROUND",
            "If you check this option, the console will always keep a fixed distance from you, otherwise, it will always stay somewhere until you move it");
    }

    public IEnumerator LaunchShowFPSHelpDialog() {
        yield return this.LaunchDialog(TipDialogPrefeb, DialogButtonType.OK, "SHOW FPS",
            "If you check this option, the FPS value will always be displayed in the center of view");
    }

    public IEnumerator LaunchAboutDialog() {
        yield return this.LaunchDialog(TipDialogPrefeb, DialogButtonType.OK, "ABOUT",
            "It's an application of molecular visualization based on MR");
    }

    /// <summary>冻结(即无法交互该View)</summary>
    public void Freeze() {
        gameObject.layer = 0;//Default
        if (IsFollow) {
            solverHandler.UpdateSolvers = false;
        }
    }

    /// <summary>恢复交互</summary>
    public void Recovery() {
        gameObject.layer = 2;//Ignore Raycast
        if (IsFollow) {
            solverHandler.UpdateSolvers = true;
        }
    }

    public void StartAdjust() {
        //不能freeze 因为要点击Done按钮完成Adjust过程
        if (!IsFollow) {
            handDraggable.enabled = true;
        }
    }

    public void DoneAdjust() {
        if (!IsFollow) {
            handDraggable.enabled = false;
        }
    }

    public string GetSelectedDisplayMode() {
        int index = DisplayModeRadialSet.SelectedIndices[0];
        return DisplayModeRadialSet.Interactives[index].Keyword;
    }

    public string GetSelectedSelectMode() {
        int index = SelectModeRadialSet.SelectedIndices[0];
        return SelectModeRadialSet.Interactives[index].Keyword;
    }

    public string GetInputPdbId() {
        return keyboardInputField.text;
    }

    public void SetPDBFileNameText(string name) {
        pdbFileNameText.text = string.Format("({0})",name);
    }

}
