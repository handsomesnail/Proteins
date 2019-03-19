using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ZCore {

    public class ViewFloatEventHandler : ViewBaseEventHandler, IVIewEventHandler<float> {

        [Serializable]
        public class VIewFloatEvent : UnityEvent<float> { }

        [SerializeField]
        private VIewFloatEvent viewFloatEvent;

        private void Start() {
            base.delegateList = ConvertDelegateList<float>(viewFloatEvent);
        }

        public void InvokeOnController(float parameter) {
            InvokeOnController<float>(parameter);
        }

    }

}
