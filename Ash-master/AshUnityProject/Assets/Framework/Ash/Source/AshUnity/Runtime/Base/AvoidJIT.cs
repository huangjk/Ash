using Ash.Core.Event;
using Ash.Core.Fsm;
using Ash.Core.Network;
using Ash.Core.Procedure;
using System;
using System.Collections.Generic;

namespace Ash.Runtime
{
    internal static class AvoidJIT
    {
        private static void NeverCalledMethod()
        {
            new Dictionary<int, EventHandler<GameEventArgs>>();
            new Dictionary<int, EventHandler<Packet>>();
            new Dictionary<int, FsmEventHandler<IProcedureManager>>();
        }
    }
}