using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace ZCore {

    [AttributeUsage(AttributeTargets.Method)]
    public class ImplementedInControllerAttribute : Attribute {

        public bool IsCustomMethodName { get; private set; }

        public string MethodName { get; private set; }

        public ImplementedInControllerAttribute() {
            IsCustomMethodName = false;
        }

        public ImplementedInControllerAttribute(string methodName) {
            IsCustomMethodName = true;
            this.MethodName = methodName;
        }
    }

}
