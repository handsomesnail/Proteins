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

public class MainConsoleView : View {

    private static Vector3 oriOffsetToMainCamera = new Vector3(0.04f, -0.03f, 1); //初始化时的相对于主摄像机的offset
    private static Vector3 oriTargetOffsetToMainCamera = new Vector3(0, -0.03f, 1.2f); //初始化后移动的目标offset
    private static Vector3 oriLocalScale = new Vector3(0.4f, 0.4f, 0.4f); //初始化时的localScale
    private static Vector3 oriRotationOffset = new Vector3(30, -30, 0);

    private BoundingBoxRig boundingBoxRig;
    private HandDraggable handDraggable;
    private Billboard billboard;
    private Tagalong tagalong;

    [Header("Resource Reference")]
    [SerializeField]
    private Dialog HelpDialogPrefeb;
    private GameObject HelpDialog;

    [Header("UI Reference")]
    [SerializeField]
    private InteractiveSet DisplayModeRadialSet;

    private bool isFollow = true;
    public bool IsFollow {
        get { return isFollow; }
        set {
            isFollow = value;
            billboard.enabled = value;
            tagalong.enabled = value;
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
    public void OnSelectedDisplayMode() { }

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

    #endregion

    #region MonoBehaviour

    private void Awake() {
        boundingBoxRig = GetComponent<BoundingBoxRig>();
        handDraggable = GetComponent<HandDraggable>();
        billboard = GetComponent<Billboard>();
        tagalong = GetComponent<Tagalong>();
    }

    private void Start() {
        //只有在不启动自动跟随的情况下 控制台是可拖动的(在Adjust模式下)
        handDraggable.enabled = false; //非Adjust模式启动
        //在Follow模式下 跟随的脚本在动画播放完毕后启动
        billboard.enabled = false;
        tagalong.enabled = false;
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
            "You can call the CONSOLE to modify options at any time by using navigation gestures. If you want to know more about navigation gestures, please refer to the manual of Hololens");
    }

    /// <summary>冻结(即无法交互该View)</summary>
    public void Freeze() {
        gameObject.layer = 0;//Default
        if (IsFollow) {
            billboard.enabled = false;
            tagalong.enabled = false;
        }
    }

    /// <summary>恢复交互</summary>
    public void Recovery() {
        gameObject.layer = 2;//Ignore Raycast
        if (IsFollow) {
            billboard.enabled = true;
            tagalong.enabled = true;
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

}
