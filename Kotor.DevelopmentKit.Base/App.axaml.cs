using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.NET.Encapsulations;

namespace Kotor.DevelopmentKit.Base
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new LoadFromERFWindow()
                {
                    DataContext = Encapsulation.LoadFromPath(@"C:\Program Files (x86)\Steam\steamapps\common\swkotor\modules\danm13.rim")
                };
            };

            base.OnFrameworkInitializationCompleted();
        }
    }
}
