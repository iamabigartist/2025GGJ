using Projects.Demo0.Core.Mgr;
using UnityEngine;
namespace Projects.Demo0.Core.DishCard.Objects
{
[RequireComponent(typeof(SpriteRenderer), typeof(PolygonCollider2D))]
public class DishClueObject : MonoBehaviour
{
	public static (DishClueObject clueObj, GameObject gameObj) Create(DishClueDoc clueDoc, DishClueStateDoc stateDoc)
	{
		var prefab = stateDoc.CluePrefab;
		var gameObj = Instantiate(prefab);
		gameObj.name = $"Clue_{prefab.name}";
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