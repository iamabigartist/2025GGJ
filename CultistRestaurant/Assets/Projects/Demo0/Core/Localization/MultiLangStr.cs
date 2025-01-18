using System.Collections.Generic;
using Sirenix.OdinInspector;
namespace Projects.Demo0.Core.Localization
{
[InlineProperty] [HideLabel] [HideReferenceObjectPicker]
public class MultiLangStr
{
	public Dictionary<LanguageType, string> Text = new();
}
}