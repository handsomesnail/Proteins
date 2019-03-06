using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZCore {

    //基本不持有数据 只提供方法
    /// <summary>处理交互 变更Model数据的同时更新View</summary>
    public class Controller : MonoBehaviour {

        protected virtual void Start() { }

        protected TModel GetModel<TModel>() where TModel : Model, new() {
            Type modelType = typeof(TModel);
            if (this.GetModuleName() != Core.GetModuleName(modelType, CoreType.Model)) {
                throw new CoreException(string.Format("[Controller.GetModel]The controller : {0} couldn't call {1}", this.GetType().Name, modelType.Name));
            }
            return Core.GetModel<TModel>();
        }

        protected TService GetService<TService>() where TService : Service, new() {
            Type serviceType = typeof(TService);
            if (this.GetModuleName() != Core.GetModuleName(serviceType, CoreType.Service)) {
                throw new CoreException(string.Format("[Controller.GetService]The controller : {0} couldn't call {1}", this.GetType().Name, serviceType.Name));
            }
            return Core.GetService<TService>();
        }

        protected TView GetView<TView>() where TView : View {
            Type viewType = typeof(TView);
            if (this.GetModuleName() != Core.GetModuleName(viewType, CoreType.View)) {
                throw new CoreException(string.Format("[Controller.GetView]The controller : {0} couldn't call {1}", this.GetType().Name, viewType.Name));
            }
            return Core.GetView<TView>();
        }

    }

}
