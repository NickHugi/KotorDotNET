using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

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
                desktop.MainWindow = new LoadFromERFWindow(@"C:\Program Files (x86)\Steam\steamapps\common\swkotor\modules\danm13.rim");
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
