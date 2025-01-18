using Projects.Demo0.Core.GameGlobal;
using Projects.Demo0.Core.Utils;
namespace Projects.Demo0.Core.Mgr
{
public class GameDocMgr : MonoSingleton<GameDocMgr>
{
	public GameGlobalConfig m_GameGlobalConfig;
	public ClueAnimationDoc m_ClueAnimationDoc;
}
}