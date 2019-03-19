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

        //[Serializable]
        //public class TEvent : UnityEvent<T> { }

        //[SerializeField]
        //private TEvent Event; //必须在运行前绑定

        protected Delegate[] delegateList;

        /// <summary>将序列化的UnityEvent转化为委托链</summary>
        internal static Delegate[] ConvertDelegateList(UnityEventBase unityEvent) {
            int eventCount = unityEvent.GetPersistentEventCount();
            Delegate[] delegateList = new Delegate[eventCount];
            for (int i = 0; i < eventCount; i++) {
                UnityEngine.Object target = unityEvent.GetPersistentTarget(i);
                string methodName = unityEvent.GetPersistentMethodName(i);
                View view = target as View;
                if (view == null) {
                    throw new CoreException(string.Format("[ViewBaseEventHandler.ConvertDelegateList]Target object : {0} is not a Core.View component", target.name));
                }
                Controller controller = view.GetController();
                MethodInfo methodInfo = controller.GetType().GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance);
                if (methodInfo == null) {
                    throw new CoreException(string.Format("[ViewBaseEventHandler.ConvertDelegateList]NotImplemented viewEvent method : {0} in {1}", methodInfo.Name, controller.GetType().Name));
                }
                ParameterInfo[] parameterInfos = methodInfo.GetParameters();
                if (parameterInfos.Length != 0) { //无参
                    throw new CoreException(string.Format("[ViewBaseEventHandler.ConvertDelegateList]The illegal method : {0}.{1} It's not supported to pass parameter", controller.GetType().Name, methodInfo.Name));
                }
                if (methodInfo.ReturnType != typeof(void)) { //无返回值
                    throw new CoreException(string.Format("[ViewBaseEventHandler.ConvertDelegateList]The illegal method : {0}.{1} It's not supported to return value", controller.GetType().Name, methodInfo.Name));
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
                Controller controller = view.GetController();
                MethodInfo methodInfo = controller.GetType().GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance);
                if (methodInfo == null) {
                    throw new CoreException(string.Format("[ViewBaseEventHandler.ConvertDelegateList]NotImplemented viewEvent method : {0} in {1}", methodInfo.Name, controller.GetType().Name));
                }
                ParameterInfo[] parameterInfos = methodInfo.GetParameters();
                if (parameterInfos.Length != 1 || parameterInfos[0].ParameterType != typeof(TParameter)) { //只有一个对应类型参
                    throw new CoreException(string.Format("[ViewBaseEventHandler.ConvertDelegateList]The illegal method : {0}.{1} its parameter is not match the Type : {2}", controller.GetType().Name, methodInfo.Name, typeof(TParameter).Name));
                }
                if (methodInfo.ReturnType != typeof(void)) { //无返回值
                    throw new CoreException(string.Format("[ViewBaseEventHandler.ConvertDelegateList]The illegal method : {0}.{1} It's not supported to return value", controller.GetType().Name, methodInfo.Name));
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
