using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZCore {
    internal class CoreException : Exception {
        public CoreException(string message, Exception innerException) : base(message, innerException) { }
        public CoreException(string message) : base(message) { }
        public CoreException() : base() { }
    }

}
