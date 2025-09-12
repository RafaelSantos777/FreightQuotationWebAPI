using System.Globalization;

namespace Shared;

public static class ApplicationCulture {

    public static void SetCulture() {
        CultureInfo cultureInfo = new("en-US");
        CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
    }

}
