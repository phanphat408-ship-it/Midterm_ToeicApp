using ToeicReadingApp.Debugging;

namespace ToeicReadingApp;

public class ToeicReadingAppConsts
{
    public const string LocalizationSourceName = "ToeicReadingApp";

    public const string ConnectionStringName = "Default";

    public const bool MultiTenancyEnabled = true;


    /// <summary>
    /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
    /// </summary>
    public static readonly string DefaultPassPhrase =
        DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "bb689bdcf0b6414788aaa293d2ca0cdf";
}
