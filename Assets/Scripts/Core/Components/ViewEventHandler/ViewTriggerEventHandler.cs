using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ZCore {

    public class ViewTriggerEventHandler : ViewBaseEventHandler, IViewEventHandler {

        [Serializable]
        public class VIewVoidEvent : UnityEvent { }

        [SerializeField]
        private VIewVoidEvent viewVoidEvent;

        private void Start() {
            base.delegateList = ConvertDelegateList(viewVoidEvent);
        }

        public void InvokeOnController() {
            foreach (Delegate action in delegateList) {
                (action as Action)();
            }
        }

    }

}