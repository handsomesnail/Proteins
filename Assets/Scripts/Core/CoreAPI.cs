using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZCore {
    public static class CoreAPI {

        public static void SendCommand<TModule, TCommand>(TCommand cmd) where TModule : Module, new() where TCommand : Command {
            Core.SendCommand<TModule, TCommand>(cmd);
        }

        public static void SendCommand<TModule>(Command cmd) where TModule : Module, new() {
            Core.SendCommand<TModule>(cmd);
        }

        public static TResult PostCommand<TModule, TCommand, TResult>(TCommand cmd) where TModule : Module, new() where TCommand : Command {
            return Core.PostCommand<TModule, TCommand, TResult>(cmd);
        }

        public static object PostCommand<TModule>(Command cmd) where TModule : Module, new() {
            return Core.PostCommand<TModule>(cmd);
        }

        //TODO: 异步的带回调函数的发送指令

        public static void PostCommandAsync<TModule, TCommand, TResult>(TCommand cmd, Action<TResult> callBack) where TModule : Module, new() where TCommand : Command {
            Core.PostCommandAsync<TModule, TCommand, TResult>(cmd, callBack);
        }

        public static void PostCommandAsync<TModule>(Command cmd, Action<object> callBack) where TModule : Module, new() {
            Core.PostCommandAsync<TModule>(cmd, callBack);
        }

    }
}