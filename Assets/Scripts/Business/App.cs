using PolymerModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Util;
using System.Reflection;
using ZCore;
using UnityEngine.Events;

#if UNITY_WSA_10_0 && !UNITY_EDITOR
using Windows.Storage;
using Windows.UI.Popups;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.Storage.Pickers;
#endif

public class App : MonoBehaviour {

    public static App Instance {
        get; private set;
    }

    public Transform GameObjectPoolRoot {
        get; private set;
    }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public Text ErrorText;

    private void Start() {
        if(GameObjectPoolRoot == null) {
            GameObject go = new GameObject("GameObjectPoolRoot");
            GameObjectPoolRoot = go.transform;
            GameObjectPoolRoot.SetParent(this.gameObject.transform);
        }
        LoadData();
    }


    private async void LoadData() {
        await PolymerModelAPI.LoadDataAsync();
        Debug.Log("加载数据完成");
    }

    [ImplementedInController]
    public async void OnButtonClick() {
        CoreAPI.SendCommand<PdbLoaderModule, LoadLocalPdbFileCommand>(new LoadLocalPdbFileCommand(() => {
            CoreAPI.SendCommand<ProteinDisplayModule, ShowProteinCommand>(new ShowProteinCommand());
        }));
        //CoreAPI.SendCommand<ProteinDisplayModule, ShowDisplayViewCommand>(new ShowDisplayViewCommand());
    }

#if ENABLE_WINMD_SUPPORT
    private const int MAX_FILE_SIZE = 3145728;

        public async void MakeMessage() {
            MessageDialog msgDialog = new MessageDialog("我是一个提示内容") { Title = "提示标题" };
            msgDialog.Commands.Add(new UICommand("确定", uiCommand => { }));
            msgDialog.Commands.Add(new UICommand("取消", uiCommand => { }));
            await msgDialog.ShowAsync();
        }

        private void MakeToast() {
            string xml = $@"<toast launch='action=invite'>
  <visual>
    <binding template='ToastGeneric'>
      <text> zcc邀请你加入5号房间</text>
      <text>message</text>
      <text placement='attribution'>via zcc</text>
    </binding>
  </visual>
</toast>";
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);
            ToastNotification toast = new ToastNotification(xmlDocument);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        private async void OpenFile() {
            FileOpenPicker picker = new FileOpenPicker {
                //SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
                //ViewMode = PickerViewMode.List
            };
            picker.FileTypeFilter.Add(".pdb");
            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null) {
                ulong size = (await file.GetBasicPropertiesAsync()).Size;//字节大小
                if (size > MAX_FILE_SIZE) {
                    throw new Exception("OverSize");
                }
                else {
                    string text = await FileIO.ReadTextAsync(file, Windows.Storage.Streams.UnicodeEncoding.Utf8);
                }
            }
            else throw new Exception("Cancel");
        }
#endif

}
