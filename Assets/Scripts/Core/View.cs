using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZCore {

    /// <summary>View层 持有游戏物体的引用</summary>
    public class View : MonoBehaviour {

        /// <summary>View对象创建之后调用的函数(在GetView之前被调用, 替代Start的功能)</summary>
        internal protected virtual void OnCreated() { }

        protected TController GetController<TController>() where TController : Controller {
            Type controllerType = typeof(TController);
            if (this.GetModuleName() != Core.GetModuleName(controllerType, CoreType.Controller)) {
                throw new CoreException(string.Format("[View.GetController]The view : {0} couldn't call {1}", this.GetType().Name, controllerType.Name));
            }
            return Core.GetController<TController>();
        }

        public virtual void Close() {
            Type viewType = GetType();
            Core.CloseView(viewType);
        }

    }

}
