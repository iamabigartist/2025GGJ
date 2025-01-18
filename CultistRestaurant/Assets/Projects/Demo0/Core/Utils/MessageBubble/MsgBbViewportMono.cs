using System.Collections;
using UnityEngine;
using UnityEngine.UI;
namespace Projects.Demo0.Core.Utils.MessageBubble
{
public class MsgBbViewportMono : MonoBehaviour
{
	public static void CurViewportAppendMsgBb(string msg) => FindAnyObjectByType<MsgBbViewportMono>().AppendBb(msg);

	public RectTransform MsgBbParent;
	public GameObject MsgBbPrefab;
	public AnimationCurve ScrollSpeedCurve = new();
	public float SpeedRatio = 50f;
	public float RemainScrollHeight = 0;
	public bool NeedScroll => RemainScrollHeight > 0;
	void SetMsgParentY() => MsgBbParent.anchoredPosition = new(0, -RemainScrollHeight);
	void Update()
	{
		if (NeedScroll)
		{
			float scrollSpeed = ScrollSpeedCurve.Evaluate(RemainScrollHeight);
			RemainScrollHeight -= scrollSpeed * Time.deltaTime * SpeedRatio;
			if (RemainScrollHeight < 0) { RemainScrollHeight = 0; }
			SetMsgParentY();
		}
	}
	public void AppendBb(string msg) => AppendBb_Impl(msg);
	void AppendBb_Impl(string msg)
	{
		var msgBb = Instantiate(MsgBbPrefab, MsgBbParent);
		var textMono = msgBb.GetComponentInChildren<Text>();
		textMono.text = msg;
		LayoutRebuilder.ForceRebuildLayoutImmediate(MsgBbParent);
		RemainScrollHeight += msgBb.GetComponent<RectTransform>().rect.height;
		SetMsgParentY();
	}

	[ContextMenu("TesAddRndBb")]
	public void TestAddRndBb() => StartCoroutine(TestAddRndBb_Impl());
	public IEnumerator TestAddRndBb_Impl()
	{
		if (!Application.isPlaying) { yield break; }
		for (int i = 0; i < 3; i++)
		{
			AppendBb($"RndMsg_{i}");
			yield return new WaitForSeconds(0.3f);
			AppendBb($"HELLO werwertwert\n\n");
			yield return new WaitForSeconds(0.1f);
			AppendBb($"年后放假啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊\n\n");
			yield return new WaitForSeconds(0.5f);
			AppendBb($"尸体在说话u\n\n");
			yield return new WaitForSeconds(2f);
		}
	}
}
}