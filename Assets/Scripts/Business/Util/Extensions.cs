using HoloToolkit.UX.Dialog;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions {

    /// <summary>创建一个Dialog 一直yield返回直到Dialog响应</summary>
    public static IEnumerator LaunchDialog(this ZCore.View view,Dialog dialogPrefab, DialogButtonType buttons, string title, string message) {
        Dialog dialog = Dialog.Open(dialogPrefab.gameObject, buttons, title, message);
        while (dialog.State < DialogState.InputReceived) {
            yield return null;
        }
        yield break;
    }

}
