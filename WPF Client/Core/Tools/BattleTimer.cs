using System;
using System.Threading.Tasks;

namespace WPF_Client.Core.Tools;

internal static class BattleTimer
{
    private static Action _actions;

    static BattleTimer ()
    {
        do {
            _actions?.Invoke ();

            var delay = async () => await Task.Delay(20000);
            delay();

        } while (true);
    }

    internal static void AddAction(Action action)
    {
        ArgumentNullException.ThrowIfNull(action);

        _actions += action;
    }

    internal static void RemoveAction(Action action)
    {
        ArgumentNullException.ThrowIfNull(action);

        _actions -= action;
    }
}