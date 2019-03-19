using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZCore {

    /// <summary>模块，作为业务之间通信调用接口</summary>
    public abstract class Module {

        protected Controller GetController() {
            string controllerName = string.Format("{0}Controller", this.GetModuleName());
            Type controllerType = Core.MainAssembly.GetType(controllerName);
            if (controllerType == null) {
                throw new CoreException(string.Format("[Module.GetController]Couldn't find the controller class named {0}", controllerName));
            }
            return Core.GetController(controllerType);
        }

        protected TController GetController<TController>() where TController : Controller {
            Type controllerType = typeof(TController);
            if(this.GetModuleName() != Core.GetModuleName(controllerType, CoreType.Controller)) {
                throw new CoreException(string.Format("[Module.GetController]The module : {0} couldn't call {1}", this.GetType().Name, controllerType.Name));
            }
            return Core.GetController<TController>();
        }

    }

}
