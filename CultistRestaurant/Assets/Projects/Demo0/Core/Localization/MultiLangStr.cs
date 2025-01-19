using System.Collections.Generic;
using Sirenix.OdinInspector;
namespace Projects.Demo0.Core.Localization
{
[InlineProperty] [HideReferenceObjectPicker]
public class MultiLangStr
{
	public static LanguageType CurrentLanguage = LanguageType.Chinese;
	public Dictionary<LanguageType, string> Text = new();
	public override string ToString() => Text.TryGetValue(CurrentLanguage, out var value) ? value : $"Str of language {CurrentLanguage} Not Found";
}
}