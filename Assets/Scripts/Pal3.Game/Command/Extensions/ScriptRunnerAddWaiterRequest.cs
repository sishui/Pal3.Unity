// ---------------------------------------------------------------------------------------------
//  Copyright (c) 2021-2024, Jiaqi Liu. All rights reserved.
//  See LICENSE file in the project root for license information.
// ---------------------------------------------------------------------------------------------

namespace Pal3.Game.Command.Extensions
{
    using Core.Command;
    using Script.Waiter;

    public sealed class ScriptRunnerAddWaiterRequest : ICommand
    {
        public ScriptRunnerAddWaiterRequest(IScriptRunnerWaiter waiter)
        {
            Waiter = waiter;
        }

        public IScriptRunnerWaiter Waiter { get; }
    }
}