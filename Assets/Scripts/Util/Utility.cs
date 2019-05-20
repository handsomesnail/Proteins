using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

#if UNITY_WSA_10_0 && !UNITY_EDITOR
using Windows.UI.ViewManagement;
#endif

public static class Utility {

    private static System.Random random;

    static Utility() {
        random = new System.Random();
    }

    /// <summary>判断Win10平台下是否为触控</summary>
    public static bool IsWindows10UserInteractionModeTouch {
        get {
            bool isInTouchMode = false;
#if UNITY_WSA_10_0 && !UNITY_EDITOR
                UnityEngine.WSA.Application.InvokeOnUIThread(() => { isInTouchMode = UIViewSettings.GetForCurrentView().UserInteractionMode == UserInteractionMode.Touch; }, true); 
#endif
            return isInTouchMode;
        }
    }

    //该方法仅消耗3ms左右 相当于一次print 性能消耗忽略不计
    /// <summary>执行委托并获取目标代码的运行时间</summary>
    public static long GetTimeSpan(Action action) {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        action();
        stopwatch.Stop();
        return stopwatch.ElapsedTicks;
    }

    /// <summary>获取当前设备处理器的核心数(作为所开线程推荐上限)</summary>
    public static int ProcessorCoreCount {
        get {
            return SystemInfo.processorCount;
        }
    }

    public static int GetRandomInt() {
        return random.Next();
    }

    public static void Active(this GameObject gameObject, bool active) {
        if(gameObject.activeSelf != active) {
            gameObject.SetActive(active);
        }
    }

    /// <summary>合并该游戏物体下的所有Mesh并返回合并后的mesh </summary>
    public static Mesh CombineChildsMesh(this Transform parent) {
        MeshFilter[] meshFilters = parent.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combineInstances = new CombineInstance[meshFilters.Length]; //排除自己
        for (int i = 0; i < meshFilters.Length; i++) {
            combineInstances[i].mesh = meshFilters[i].sharedMesh;
            combineInstances[i].transform = meshFilters[i].transform.localToWorldMatrix; 
        }
        Mesh combinedMesh = new Mesh();  
        combinedMesh.CombineMeshes(combineInstances); 
        return combinedMesh;
    }


}
