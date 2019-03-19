using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ZCore {

    public class ViewStringEventHandler : ViewBaseEventHandler, IVIewEventHandler<string> {

        [Serializable]
        public class VIewStringEvent : UnityEvent<string> { }

        [SerializeField]
        private VIewStringEvent viewStringEvent;

        private void Start() {
            base.delegateList = ConvertDelegateList<string>(viewStringEvent);
        }

        public void InvokeOnController(string parameter) {
            InvokeOnController<string>(parameter);
        }

    }

}
