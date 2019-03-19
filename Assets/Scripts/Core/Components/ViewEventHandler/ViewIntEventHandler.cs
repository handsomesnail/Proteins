using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ZCore {

    public class ViewIntEventHandler : ViewBaseEventHandler, IVIewEventHandler<int> {

        [Serializable]
        public class VIewIntEvent : UnityEvent<int> { }

        [SerializeField]
        private VIewIntEvent viewIntEvent;

        private void Start() {
            base.delegateList = ConvertDelegateList<int>(viewIntEvent);
        }

        public void InvokeOnController(int parameter) {
            InvokeOnController<int>(parameter);
        }

    }
    
}
