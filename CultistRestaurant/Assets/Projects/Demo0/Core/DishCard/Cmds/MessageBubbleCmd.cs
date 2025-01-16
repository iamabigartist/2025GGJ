using System.Collections.Generic;
using Projects.Demo0.Core.Localization;
namespace Projects.Demo0.Core
{

public class MessageBubbleCmd : Cmd
{
	public Dictionary<LanguageType, string> Text = new();
}
}