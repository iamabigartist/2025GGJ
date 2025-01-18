using Projects.Demo0.Core.Localization;
using Projects.Demo0.Core.Utils.MessageBubble;
namespace Projects.Demo0.Core
{

public class MessageBubbleCmd : Cmd
{
	public MultiLangStr Str = new();
	public override void Execute(CmdContext ctx) => MsgBbViewportMono.CurViewportAppendMsgBb(Str.ToString());
}

}