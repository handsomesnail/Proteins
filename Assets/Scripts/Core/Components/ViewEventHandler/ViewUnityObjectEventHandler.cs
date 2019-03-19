using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace ZCore {

    public class ViewUnityObjectEventHandler : ViewBaseEventHandler, IVIewEventHandler<Object> {

        [Serializable]
        public class VIewUnityObjectEvent : UnityEvent<Object> { }

        [SerializeField]
        private VIewUnityObjectEvent viewUnityObjectEvent;

        private void Start() {
            base.delegateList = ConvertDelegateList<Object>(viewUnityObjectEvent);
        }

        public void InvokeOnController(Object parameter) {
            InvokeOnController<Object>(parameter);
        }

    }

}