using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace ToeicReadingApp.Localization;

public static class ToeicReadingAppLocalizationConfigurer
{
    public static void Configure(ILocalizationConfiguration localizationConfiguration)
    {
        localizationConfiguration.Sources.Add(
            new DictionaryBasedLocalizationSource(ToeicReadingAppConsts.LocalizationSourceName,
                new XmlEmbeddedFileLocalizationDictionaryProvider(
                    typeof(ToeicReadingAppLocalizationConfigurer).GetAssembly(),
                    "ToeicReadingApp.Localization.SourceFiles"
                )
            )
        );
    }
}
