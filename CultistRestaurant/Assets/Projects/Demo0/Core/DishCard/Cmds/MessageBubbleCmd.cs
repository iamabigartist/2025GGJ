using Projects.Demo0.Core.Localization;
namespace Projects.Demo0.Core
{

public class MessageBubbleCmd : Cmd
{
	public MultiLangStr Str = new();
	public override void Execute(CmdContext ctx)
	{
		throw new System.NotImplementedException();
	}
}

}