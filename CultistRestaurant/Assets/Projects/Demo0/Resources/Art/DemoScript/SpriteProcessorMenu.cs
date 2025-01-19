using UnityEngine;
using UnityEditor;
using System.Linq;

public class SpriteProcessorMenu
{
    [MenuItem("Tools/处理选中Sprite的渲染设置")]
    static void ProcessSelectedSprite()
    {
        var selectedObject = Selection.activeGameObject;
        if (selectedObject == null) 
        {
            Debug.LogWarning("请先选择一个游戏对象");
            return;
        }

        var spriteRenderer = selectedObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null || spriteRenderer.sprite == null)
        {
            Debug.LogWarning("选中的对象需要有SpriteRenderer组件和有效的Sprite");
            return;
        }

        var sprite = spriteRenderer.sprite;
        var assetPath = AssetDatabase.GetAssetPath(sprite);

        // 从sprite名称中提取第一个数字作为sorting order
        var firstNumber = new string(sprite.name.Where(c => char.IsDigit(c)).Take(1).ToArray());
        if (int.TryParse(firstNumber, out int orderInLayer))
        {
            Undo.RecordObject(spriteRenderer, "Change Sprite Order");
            spriteRenderer.sortingOrder = orderInLayer;
        }
        
        // 如果sprite资产路径包含elements，设置其sortingLayer
        if (assetPath.Contains("elements"))
        {
            Undo.RecordObject(spriteRenderer, "Change Sprite Layer");
            spriteRenderer.sortingLayerName = "elements";
        }
    }
}