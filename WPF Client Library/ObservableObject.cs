using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WPF_Client_Library;

/// <summary>
/// Base class for an updatable UI element
/// </summary>
public abstract class ObservableObject : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    [Obsolete("Use PropertyChanged.Fody NuGet package instead")]
    protected virtual void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}