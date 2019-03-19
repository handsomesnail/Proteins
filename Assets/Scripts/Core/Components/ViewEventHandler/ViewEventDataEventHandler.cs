using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ZCore {

    public class ViewEventDataEventHandler : ViewBaseEventHandler, IVIewEventHandler<BaseEventData> {

        [Serializable]
        public class VIewEventDataEvent : UnityEvent<BaseEventData> { }

        [SerializeField]
        private VIewEventDataEvent viewEventDataEvent;

        private void Start() {
            base.delegateList = ConvertDelegateList<BaseEventData>(viewEventDataEvent);
        }

        public void InvokeOnController(BaseEventData parameter) {
            InvokeOnController<BaseEventData>(parameter);
        }
    }

}
