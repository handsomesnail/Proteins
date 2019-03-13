using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace ZCore {

    //****************命名规则******************
    //Module名 ExampleModule
    //Controller名 ExampleController
    //Model名 ExampleModel
    //View名 ExampleView
    //Service名 ServiceView
    //Command名 xxxxxCommand(继承自ExampleCommand)
    //注意：
    //1.每个模块内Example要一致，框架强制要求并根据名称字符串匹配
    //2.为了保持模块间良好封装性，要保证反射功能仅在Core工程内使用
    internal enum CoreType {
        Module = 0,
        Controller,
        View,
        Model,
        Service,
        Command,
        Exception,
    }

    public static class CoreAPI {
        public static void SendCommand<TModule>(Command cmd) where TModule : Module, new() {
            Core.SendCommand<TModule>(cmd);
        }
        public static object PostCommand<TModule>(Command cmd) where TModule : Module, new() {
            return Core.PostCommand<TModule>(cmd);
        }
        //TODO: 异步的带回调函数的发送指令
    }

    /// <summary>框架核心，仅框架层代码可访问</summary>
    internal static class Core {

        /// <summary>保持模块实例的引用</summary>
        private static readonly Dictionary<Type, Module> modulesDic;
        private static readonly Dictionary<Type, Controller> controllersDic;
        private static readonly Dictionary<Type, Model> modelsDic;
        private static readonly Dictionary<Type, View> viewsDic;
        private static readonly Dictionary<Type, Service> servicesDic;

        private static readonly GameObject core;
        private static readonly GameObject viewRoot;
        private static readonly GameObject controllers;

        private const string viewPath = "View/";

        /// <summary>主程序集(Assembly-CSharp)</summary>
        public static Assembly MainAssembly {
            get; private set;
        }

        static Core() {
            MainAssembly = Assembly.Load("Assembly-CSharp");
            modulesDic = new Dictionary<Type, Module>();
            controllersDic = new Dictionary<Type, Controller>();
            modelsDic = new Dictionary<Type, Model>();
            viewsDic = new Dictionary<Type, View>();
            servicesDic = new Dictionary<Type, Service>();
            GameObject core = new GameObject("Core");
            UnityEngine.Object.DontDestroyOnLoad(core);
            controllers = new GameObject("Controllers");
            controllers.transform.SetParent(core.transform);
            viewRoot = new GameObject("ViewRoot");
            viewRoot.transform.SetParent(core.transform);
        }

        public static void SendCommand<TModule>(Command cmd) where TModule : Module, new() {
            PostCommand<TModule>(cmd);
        }

        public static object PostCommand<TModule>(Command cmd) where TModule : Module, new() {
            //调用对应Module下的实例的OnxxxxCommand函数
            Type moduleType = typeof(TModule);
            TModule module = GetModule<TModule>();
            MethodInfo methodInfo = moduleType.GetMethod(string.Format("On{0}", cmd.GetType().Name), BindingFlags.Public | BindingFlags.Instance);
            if (methodInfo == null) {
                throw new CoreException(string.Format("[Core.PostCommand]Unhandled Command : {0} for {1}", cmd.GetType().Name, module.GetType().Name));
            }
            return methodInfo.Invoke(module, new object[] { cmd });
        }

        /// <summary>获取某个模块的名称</summary>
        public static string GetModuleName<TModule>() where TModule : Module, new() {
            Type moduleType = typeof(TModule);
            return GetModuleName(moduleType, CoreType.Module);
        }
        public static string GetModuleName(this Module module) {
            return GetModuleName(module.GetType(), CoreType.Module);
        }
        public static string GetModuleName(this Controller controller) {
            return GetModuleName(controller.GetType(), CoreType.Controller);
        }
        public static string GetModuleName(this Service service) {
            return GetModuleName(service.GetType(), CoreType.Service);
        }
        public static string GetModuleName(this View view) {
            return GetModuleName(view.GetType(), CoreType.View);
        }
        public static string GetModuleName(Type type, CoreType coreType) {
            if (!type.Name.EndsWith(coreType.ToString())) {
                throw new CoreException(string.Format("[Core.GetModuleName]The {0} : {1} is not end with \"{2}\" ", coreType.ToString().ToLower(), type.Name, coreType.ToString()));
            }
            int length = type.Name.Length;
            return type.Name.Substring(0, length - coreType.ToString().Length);
        }

        /// <summary>获取某个模块的实例(唯一入口)</summary>
        public static TModule GetModule<TModule>() where TModule : Module, new() {
            Type moduleType = typeof(TModule);
            Module module = null;
            if (!modulesDic.TryGetValue(moduleType, out module)) {
                if (!moduleType.Name.EndsWith("Module")) {
                    throw new CoreException(string.Format("[Core.GetModule]The module named {0} is not end with \"Module\" ", moduleType.Name));
                }
                module = new TModule();
                modulesDic.Add(moduleType, module);
            }
            return module as TModule;
        }


        /// <summary>获取某个模块的控制器</summary>
        public static TController GetController<TController>() where TController : Controller {
            Type controllerType = typeof(TController);
            return GetController(controllerType) as TController;
        }
        public static Controller GetController(Type controllerType) {
            Controller controller = null;
            if (!controllersDic.TryGetValue(controllerType, out controller)) {
                string controllerTypeName = controllerType.Name;
                if (!controllerTypeName.EndsWith("Controller")) {
                    throw new CoreException(string.Format("[Core.GetController]The controller named {0} is not end with \"Controller\" ", controllerTypeName));
                }
                if (!controllerType.IsSubclassOf(typeof(Controller))) {
                    throw new CoreException(string.Format("[Core.GetController]The controller class named {0} doesn't inherit from Core.Controller", controllerTypeName));
                }
                GameObject newController = new GameObject(controllerTypeName, controllerType);
                newController.transform.SetParent(controllers.transform);
                controller = newController.GetComponent(controllerTypeName) as Controller;
                controllersDic.Add(controllerType, controller);
            }
            return controller;
        }

        public static TModel GetModel<TModel>() where TModel : Model, new() {
            Type modelType = typeof(TModel);
            Model model = null;
            if (!modelsDic.TryGetValue(modelType, out model)) {
                if (!modelType.Name.EndsWith("Model")) {
                    throw new CoreException(string.Format("[Core.GetModel]The model named {0} is not end with \"Model\" ", modelType.Name));
                }
                model = new TModel();
                modelsDic.Add(modelType, model);
            }
            return model as TModel;
        }

        public static TService GetService<TService>() where TService : Service, new() {
            Type serviceType = typeof(TService);
            Service service = null;
            if (!servicesDic.TryGetValue(serviceType, out service)) {
                if (!serviceType.Name.EndsWith("Service")) {
                    throw new CoreException(string.Format("[Core.GetService]The service named {0} is not end with \"Service\" ", serviceType.Name));
                }
                service = new TService();
                servicesDic.Add(serviceType, service);
            }
            return service as TService;
        }

        public static TView GetView<TView>() where TView : View {
            Type viewType = typeof(TView);
            View view = null;
            if(!viewsDic.TryGetValue(viewType, out view)) {
                if (!viewType.Name.EndsWith("View")) {
                    throw new CoreException(string.Format("[Core.GetView]The view named {0} is not end with \"View\" ", viewType.Name));
                }
                GameObject viewGoPrefeb = null;
                viewGoPrefeb = Resources.Load<GameObject>(viewPath + viewType.Name);
                if(viewGoPrefeb == null) {
                    throw new CoreException(string.Format("[Core.GetView]Couldn't find the view asset named {0} at the path : {1}", viewType.Name, viewPath + viewType.Name));
                }
                if(viewGoPrefeb.GetComponent<TView>() == null) {
                    throw new CoreException(string.Format("[Core.GetView]The view asset named {0} isn't attached view script", viewType.Name));
                }
                GameObject viewGo = GameObject.Instantiate<GameObject>(viewGoPrefeb);
                viewGo.transform.SetParent(viewRoot.transform);
                view = viewGo.GetComponent<TView>();
                viewsDic.Add(viewType, view);
                view.OnCreated();
            }
            return view as TView;
        }

        public static void CloseView<TView>() where TView : View {
            Type viewType = typeof(TView);
            CloseView(viewType);
        }

        public static void CloseView(Type viewType) {
            View view = null;
            if(!viewsDic.TryGetValue(viewType, out view)) {
                if (!viewType.Name.EndsWith("View")) {
                    throw new CoreException(string.Format("[Core.CloseView]The view named {0} is not end with \"View\" ", viewType.Name));
                }
                if (!viewType.IsSubclassOf(typeof(View))) {
                    throw new CoreException(string.Format("[Core.CloseView]The view class named {0} doesn't inherit from Core.View", viewType.Name));
                }
                return;
            }
            UnityEngine.Object.Destroy(viewsDic[viewType].gameObject);
            viewsDic.Remove(viewType);
        }

    }

}
