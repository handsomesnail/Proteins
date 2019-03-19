using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ZCore {

    public class ViewBoolEventHandler : ViewBaseEventHandler, IVIewEventHandler<bool> {

        [Serializable]
        public class VIewBoolEvent : UnityEvent<bool> { }

        [SerializeField]
        private VIewBoolEvent viewBoolEvent;

        private void Start() {
            base.delegateList = ConvertDelegateList<bool>(viewBoolEvent);
        }

        public void InvokeOnController(bool parameter) {
            InvokeOnController<bool>(parameter);
        }
    }

}
