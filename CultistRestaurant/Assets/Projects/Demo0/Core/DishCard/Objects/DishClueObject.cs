using UnityEngine;
namespace Projects.Demo0.Core.DishCard.Objects
{
[RequireComponent(typeof(SpriteRenderer), typeof(PolygonCollider2D))]
public class DishClueObject : MonoBehaviour
{
	public static (DishClueObject clueObj, GameObject gameObj) Create(DishClueDoc doc, DishClueState state)
	{
		var curStateDoc = doc.StateDoc.Find(doc => doc.State.Equals(state));
		var prefab = curStateDoc.CluePrefab;
		var gameObj = Instantiate(prefab);
		gameObj.name = $"Clue_{prefab.name}";
		var collider = gameObj.AddComponent<PolygonCollider2D>();
		var clueObj = gameObj.AddComponent<DishClueObject>();
		clueObj.m_Doc = doc;
		clueObj.m_StateDoc = curStateDoc;
		return (clueObj, gameObj);
	}
	public DishClueStateDoc m_StateDoc;
	public DishClueDoc m_Doc;
	void OnMouseDown() => StartCoroutine(m_StateDoc.InteractCmdList.ExecuteCmdList(new() { TargetGameObj = gameObject }));
	void OnDestroy() => StopAllCoroutines();
}
}