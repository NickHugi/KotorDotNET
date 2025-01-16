using Avalonia.Platform.Storage;

namespace Kotor.DevelopmentKit.Core;

public static class FilePickerTypes
{
    public static readonly FilePickerFileType TwoDA = new FilePickerFileType("2DA File")
    {
        Patterns = ["*.2da"],
    };
}
