using System;
using System.Collections.Generic;
using System.Text;
using ReactiveUI;
using Avalonia.ReactiveUI;

namespace MineCloudApp.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        public string AppName => "MineCloud";
    }
}
