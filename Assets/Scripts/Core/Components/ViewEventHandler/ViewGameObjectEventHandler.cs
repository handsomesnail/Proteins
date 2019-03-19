using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ZCore {

    public class ViewGameObjectEventHandler : ViewBaseEventHandler, IVIewEventHandler<GameObject> {

        [Serializable]
        public class VIewGameObjectEvent : UnityEvent<GameObject> { }

        [SerializeField]
        private VIewGameObjectEvent viewGameObjectEvent;

        private void Start() {
            base.delegateList = ConvertDelegateList<GameObject>(viewGameObjectEvent);
        }

        public void InvokeOnController(GameObject parameter) {
            InvokeOnController<GameObject>(parameter);
        }
    }

}
