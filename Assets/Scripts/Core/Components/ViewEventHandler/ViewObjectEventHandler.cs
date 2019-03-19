using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ZCore {

    public class ViewObjectEventHandler : ViewBaseEventHandler, IVIewEventHandler<object> {

        [Serializable]
        public class VIewObjectEvent : UnityEvent<object> { }

        [SerializeField]
        private VIewObjectEvent viewObjectEvent;

        private void Start() {
            base.delegateList = ConvertDelegateList<object>(viewObjectEvent);
        }

        public void InvokeOnController(object parameter) {
            InvokeOnController<object>(parameter);
        }

    }

}
