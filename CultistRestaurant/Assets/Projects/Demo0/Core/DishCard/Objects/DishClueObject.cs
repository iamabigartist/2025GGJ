using UnityEngine;
namespace Projects.Demo0.Core.DishCard.Objects
{
[RequireComponent(typeof(SpriteRenderer), typeof(PolygonCollider2D))]
public class DishClueObject : MonoBehaviour
{
	public static (DishClueObject clueObj, GameObject gameObj) Create(DishClueDoc clueDoc, DishClueState state, ClueAnimationDoc animationDoc)
	{
		var curStateDoc = clueDoc.StateDoc.Find(doc => doc.State.Equals(state));
		var prefab = curStateDoc.CluePrefab;
		var gameObj = Instantiate(prefab);
		gameObj.name = $"Clue_{prefab.name}";
		var collider = gameObj.AddComponent<PolygonCollider2D>();
		var clueObj = gameObj.AddComponent<DishClueObject>();
		clueObj.m_Doc = clueDoc;
		clueObj.m_StateDoc = curStateDoc;
		var animation = gameObj.GetComponent<Animation>();
		foreach (var clip in animationDoc.Clips) { animation.AddClip(clip, clip.name); }
		return (clueObj, gameObj);
	}
	public DishClueStateDoc m_StateDoc;
	public DishClueDoc m_Doc;
	void OnMouseDown() => StartCoroutine(m_StateDoc.InteractCmdList.ExecuteCmdList(new() { TargetGameObj = gameObject }));
	void OnDestroy() => StopAllCoroutines();
}
}