using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ZCore {

    public class ViewVector2EventHandler : ViewBaseEventHandler, IVIewEventHandler<Vector2> {

        [Serializable]
        public class VIewVector2Event : UnityEvent<Vector2> { }

        [SerializeField]
        private VIewVector2Event viewVector2Event;

        private void Start() {
            base.delegateList = ConvertDelegateList<Vector2>(viewVector2Event);
        }

        public void InvokeOnController(Vector2 parameter) {
            InvokeOnController<Vector2>(parameter);
        }

    }

}