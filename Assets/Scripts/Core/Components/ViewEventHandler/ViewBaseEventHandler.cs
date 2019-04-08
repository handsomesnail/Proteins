using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using ZCore;


namespace ZCore {

    public interface IViewEventHandler {
        void InvokeOnController();
    }

    public interface IVIewEventHandler<T> {
        void InvokeOnController(T parameter);
    }

    public abstract class ViewBaseEventHandler : MonoBehaviour {

        protected Delegate[] delegateList;

        /// <summary>将序列化的UnityEvent转化为委托链</summary>
        internal static Delegate[] ConvertDelegateList(UnityEvent unityEvent) {
            int eventCount = unityEvent.GetPersistentEventCount();
            Delegate[] delegateList = new Delegate[eventCount];
            for (int i = 0; i < eventCount; i++) {
                UnityEngine.Object target = unityEvent.GetPersistentTarget(i);
                string methodName = unityEvent.GetPersistentMethodName(i);
                View view = target as View;
                if (view == null) {
                    throw new CoreException(string.Format("[ViewBaseEventHandler.ConvertDelegateList]Target object : {0} is not a Core.View component", target.name));
                }
                MethodInfo viewMethodInfo = view.GetType().GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
                if (viewMethodInfo == null) {
                    throw new CoreException(string.Format("[ViewBaseEventHandler.ConvertDelegateList]NotImplemented viewEvent method : {0} in {1}", methodName, view.GetType().Name));
                }
                ImplementedInControllerAttribute attributeInfo = viewMethodInfo.GetCustomAttribute(typeof(ImplementedInControllerAttribute)) as ImplementedInControllerAttribute;
                if (attributeInfo == null) {
                    throw new CoreException(string.Format("[ViewBaseEventHandler.ConvertDelegateList]The viewEvent method : {0} in {1} doesn't has ImplementedInControllerAttribute", viewMethodInfo.Name, view.GetType().Name));
                }
                Controller controller = view.GetController();
                MethodInfo methodInfo = controller.GetType().GetMethod(attributeInfo.IsCustomMethodName ? attributeInfo.MethodName : methodName, BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
                if (methodInfo == null) {
                    throw new CoreException(string.Format("[ViewBaseEventHandler.ConvertDelegateList]NotImplemented viewEvent method : {0} in {1}", attributeInfo.MethodName, controller.GetType().Name));
                }
                Delegate action = methodInfo.CreateDelegate(typeof(Action), controller);
                delegateList[i] = action;
            }
            return delegateList;
        }

        internal static Delegate[] ConvertDelegateList<TParameter>(UnityEvent<TParameter> unityEvent) {
            int eventCount = unityEvent.GetPersistentEventCount();
            Delegate[] delegateList = new Delegate[eventCount];
            for (int i = 0; i < eventCount; i++) {
                UnityEngine.Object target = unityEvent.GetPersistentTarget(i);
                string methodName = unityEvent.GetPersistentMethodName(i);
                View view = target as View;

                if (view == null) {
                    throw new CoreException(string.Format("[ViewBaseEventHandler.ConvertDelegateList]Target object : {0} is not a Core.View component", target.name));
                }
                MethodInfo viewMethodInfo = view.GetType().GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(TParameter)}, null);
                if (viewMethodInfo == null) {
                    throw new CoreException(string.Format("[ViewBaseEventHandler.ConvertDelegateList]NotImplemented viewEvent method : {0} in {1}", viewMethodInfo.Name, view.GetType().Name));
                }
                ImplementedInControllerAttribute attributeInfo = viewMethodInfo.GetCustomAttribute(typeof(ImplementedInControllerAttribute)) as ImplementedInControllerAttribute;
                if (attributeInfo == null) {
                    throw new CoreException(string.Format("[ViewBaseEventHandler.ConvertDelegateList]The viewEvent method : {0} in {1} doesn't has ImplementedInControllerAttribute", viewMethodInfo.Name, view.GetType().Name));
                }
                Controller controller = view.GetController();
                MethodInfo methodInfo = controller.GetType().GetMethod(attributeInfo.IsCustomMethodName ? attributeInfo.MethodName : methodName, BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(TParameter) }, null);
                if (methodInfo == null) {
                    throw new CoreException(string.Format("[ViewBaseEventHandler.ConvertDelegateList]NotImplemented viewEvent method : {0} in {1}", methodInfo.Name, controller.GetType().Name));
                }
                Delegate action = methodInfo.CreateDelegate(typeof(Action<TParameter>), controller);
                delegateList[i] = action;
            }
            return delegateList;
        }

        internal void InvokeOnController<T>(T parameter) {
            foreach (Delegate action in delegateList) {
                (action as Action<T>)(parameter);
            }
        }

    }

}
