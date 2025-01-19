using System;
using System.Collections;
using Audio;
using Projects.Demo0.Core.GameGlobal;
using Projects.Demo0.Core.Utils;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
namespace Projects.Demo0.Core.Mgr
{
public class CurtainUIMgr : MonoSingleton<CurtainUIMgr>
{
	public Animation m_Anim;
	public Text TitleText;
	public Text EndDescText;
	public Button m_ContinueBtn;
	public event Action OnContinueBtnClicked;
	[FormerlySerializedAs("IsCurtainMoving")] public bool IsMoving = false;
	void Start()
	{
		m_ContinueBtn.onClick.AddListener(CurtainOff);
		gameObject.SetActive(false);
	}
	void CurtainOff()
	{
		IsMoving = true;
		OnContinueBtnClicked?.Invoke();
		m_ContinueBtn.gameObject.SetActive(false);
		m_Anim.Play("CurtainOut");
		StartCoroutine(CurtainOffCoroutine());
	}
	IEnumerator CurtainOffCoroutine()
	{
		yield return new WaitUntil(() => !m_Anim.isPlaying);
		gameObject.SetActive(false);
		IsMoving = false;
	}
	public void CurtainIn()
	{
		IsMoving = true;
		gameObject.SetActive(true);
		m_Anim.Play("CurtainIn");
		StartCoroutine(CurtainIn_Impl());
	}
	public IEnumerator CurtainIn_Impl()
	{
		yield return new WaitUntil(() => !m_Anim.isPlaying);
		m_ContinueBtn.gameObject.SetActive(true);
		IsMoving = false;
	}
	public void ShowDayEnd(int count)
	{
		TitleText.text = $" {count + 1}{GameDocMgr.Instance.m_GameGlobalConfig.DayEndStr}";
		EndDescText.text = "";
		CurtainIn();
	}
	public void ShowStoryEnd(EndTextDesc desc)
	{
		TitleText.text = desc.m_Title.ToString();
		EndDescText.text = desc.m_Desc.ToString();
		CurtainIn();
	}
	public void ShowDeadEnd() => ShowStoryEnd(GameDocMgr.Instance.m_GameGlobalConfig.DeadEnd_Desc);
	public void ShowGoodEnd() => ShowStoryEnd(GameDocMgr.Instance.m_GameGlobalConfig.GoodEnd_Desc);
	public void ShowBadEnd() => ShowStoryEnd(GameDocMgr.Instance.m_GameGlobalConfig.BadEnd_Desc);
}
}