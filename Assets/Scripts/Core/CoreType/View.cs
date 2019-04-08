using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace ZCore {

    /// <summary>View层 持有游戏物体的引用</summary>
    public abstract class View : MonoBehaviour {

        private Controller Controller { get; set; }

        /// <summary>View对象创建之后调用的函数(在GetView之前被调用, 替代Start的功能)</summary>
        internal protected virtual void OnCreated() {

        }

        internal Controller GetController() {
            if (Controller == null) {
                string controllerName = string.Format("{0}Controller", this.GetModuleName());
                Type controllerType = Core.MainAssembly.GetType(controllerName);
                if (controllerType == null) {
                    throw new CoreException(string.Format("[View.GetController]Couldn't find the controller class named {0}", controllerName));
                }
                Controller = Core.GetController(controllerType);
            }
            return Controller;
        }

        internal TController GetController<TController>() where TController : Controller {
            Type controllerType = typeof(TController);
            if (this.GetModuleName() != Core.GetModuleName(controllerType, CoreType.Controller)) {
                throw new CoreException(string.Format("[View.GetController]The view : {0} couldn't call {1}", this.GetType().Name, controllerType.Name));
            }
            if(Controller == null) {
                Controller = Core.GetController<TController>();
            }
            return Controller as TController;
        }

        public virtual void Close() {
            Type viewType = GetType();
            Core.CloseView(viewType);
        }

    }

}
