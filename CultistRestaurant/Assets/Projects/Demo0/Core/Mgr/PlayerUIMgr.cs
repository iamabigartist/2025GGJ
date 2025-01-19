using System;
using Projects.Demo0.Core.Utils;
using UnityEngine.UI;
namespace Projects.Demo0.Core.Mgr
{
public class PlayerUIMgr : MonoSingleton<PlayerUIMgr>
{
	public Button AcceptBtn;
	public Button RefuseBtn;
	public event Action<bool> OnServeBtnClicked;

	public Text HPText;
	public void SetServeBtnInteractive(bool active)
	{
		AcceptBtn.interactable = active;
		RefuseBtn.interactable = active;
	}
	public void SetHP(int hp)
	{
		// 根据HP数量重复爱心表情
		HPText.text = new('❤', hp);
	}
	void Start()
	{
		AcceptBtn.onClick.AddListener(() => OnServeBtnClicked?.Invoke(true));
		RefuseBtn.onClick.AddListener(() => OnServeBtnClicked?.Invoke(false));
	}
}
}