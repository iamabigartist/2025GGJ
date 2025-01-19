using Projects.Demo0.Core.Mgr;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Linq;
using UnityEditor;
namespace Projects.Demo0.Core.DishCard.Objects
{
[RequireComponent(typeof(SpriteRenderer), typeof(PolygonCollider2D))]
public class DishClueObject : SerializedMonoBehaviour
{
	public static (DishClueObject clueObj, GameObject gameObj) Create(DishClueDoc clueDoc, DishClueStateDoc stateDoc)
	{
		GameObject gameObj = new();
		var curSprite = stateDoc.ClueStateSprite ?? GameDocMgr.Instance.m_GameGlobalConfig.DefaultDishSprite;
		var spriteRenderer = gameObj.AddComponent<SpriteRenderer>();
		spriteRenderer.sprite = curSprite;
		
		spriteRenderer.sortingLayerName = "elements"; //所有菜品单元都在这个层
		// 从sprite名称中提取第一个数字作为sorting order
		var firstNumber = new string(curSprite.name.Where(c => char.IsDigit(c)).Take(1).ToArray());
		if (int.TryParse(firstNumber, out int orderInLayer))
		{
			spriteRenderer.sortingOrder = orderInLayer;
		}
		
		gameObj.name = $"Clue_{curSprite.name}"; 
		var collider = gameObj.AddComponent<PolygonCollider2D>();
		var clueObj = gameObj.AddComponent<DishClueObject>();
		clueObj.m_Doc = clueDoc;
		clueObj.m_StateDoc = stateDoc;
		var animation = gameObj.GetComponent<Animation>();
		foreach (var clip in GameDocMgr.Instance.m_ClueAnimationDoc.Clips) { animation.AddClip(clip, clip.name); }
		return (clueObj, gameObj);
	}
	public DishClueStateDoc m_StateDoc;
	public DishClueDoc m_Doc;
	void OnMouseDown() => StartCoroutine(m_StateDoc.InteractCmdList.ExecuteCmdList(new() { TargetGameObj = gameObject }));
	void OnDestroy() => StopAllCoroutines();

	public bool WrongClue => m_StateDoc.State is { Correct    : false };
	public bool PollutedClue => m_StateDoc.State is { Polluted: true };
	public bool PerfectClue => m_StateDoc.State is { Correct  : true, Polluted: false };
}
}