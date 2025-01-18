using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class SceneCreator : MonoBehaviour
{
    [SerializeField]
    private string spritePath = "Assets/Projects/Demo0/Resources/Art/Sprites/";
    
    [SerializeField]
    private string positionFilePath = "Assets/Projects/Demo0/Resources/Art/Sprites/position.txt";
    
    private struct SpriteData
    {
        public string group;
        public string layer;
        public Vector2 position;
    }
    
    private Dictionary<string, SpriteData> spritePositions = new Dictionary<string, SpriteData>();

    private void LoadPositionData()
    {
        spritePositions.Clear();
        if (File.Exists(positionFilePath))
        {
            string[] lines = File.ReadAllLines(positionFilePath);
            // 跳过标题行
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] parts = line.Split(',');
                if (parts.Length >= 4)
                {
                    string group = parts[0].Trim();
                    string layer = parts[1].Trim();
                    if (float.TryParse(parts[2], out float x) && float.TryParse(parts[3], out float y))
                    {
                        spritePositions[layer] = new SpriteData
                        {
                            group = group,
                            layer = layer,
                            position = new Vector2(x * 0.01f, y * -0.01f)
                        };
                    }
                }
            }
        }
    }

    private void ProcessDirectory(string directoryPath, Transform parentTransform)
    {
        // 创建目录对应的游戏对象（如果不是根目录）
        string dirName = new DirectoryInfo(directoryPath).Name;
        Transform currentParent = parentTransform;
        
        if (directoryPath != spritePath)
        {
            GameObject dirObj = new GameObject(dirName);
            dirObj.transform.SetParent(parentTransform);
            currentParent = dirObj.transform;

            // 解析目录名开头的数字作为层级顺序
            if (dirName.Contains("_"))
            {
                string orderStr = dirName.Split('_')[0];
                if (int.TryParse(orderStr, out int order))
                {
                    // 可以用来设置整个组的排序顺序
                    // 这里可以根据需要添加其他逻辑
                }
            }
        }

        // 处理当前目录下的所有PNG文件
        string[] pngFiles = Directory.GetFiles(directoryPath, "*.png");
        foreach (string pngPath in pngFiles)
        {
            string relativePath = pngPath.Replace(Application.dataPath, "Assets");
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(relativePath);
            
            if (sprite != null)
            {
                string spriteName = Path.GetFileNameWithoutExtension(pngPath);
                GameObject spriteObj = new GameObject(spriteName);
                spriteObj.transform.SetParent(currentParent);
                
                // 添加SpriteRenderer组件
                SpriteRenderer renderer = spriteObj.AddComponent<SpriteRenderer>();
                renderer.sprite = sprite;

                // 解析文件名开头的数字作为渲染顺序
                if (spriteName.Contains("_"))
                {
                    string orderStr = spriteName.Split('_')[0];
                    if (int.TryParse(orderStr, out int order))
                    {
                        renderer.sortingOrder = order;
                    }
                }

                // 设置位置
                if (spritePositions.TryGetValue(spriteName, out SpriteData data))
                {
                    spriteObj.transform.localPosition = new Vector3(data.position.x, data.position.y, 0);
                }
            }
        }

        // 递归处理所有子目录
        string[] subDirectories = Directory.GetDirectories(directoryPath);
        foreach (string subDir in subDirectories)
        {
            ProcessDirectory(subDir, currentParent);
        }
    }

    [Button("处理所有精灵图片", ButtonSizes.Large)]
    private void ProcessAllSprites()
    {
        LoadPositionData();
        ProcessDirectory(spritePath, this.transform);
    }

    [Button("清空所有子节点", ButtonSizes.Large)]
    private void ClearAllChildren()
    {
        // 在编辑器模式下
        #if UNITY_EDITOR
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
        #else
        // 在运行时模式下
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        #endif
    }

    [Button("设置所有精灵材质", ButtonSizes.Large)]
    private void SetAllSpritesMaterial()
    {
        #if UNITY_EDITOR
        // 加载指定材质
        string materialPath = "Assets/Projects/Demo0/Resources/Art/Render/MyUnlit.mat";
        Material material = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
        
        if (material == null)
        {
            Debug.LogError($"未能找到材质: {materialPath}");
            return;
        }

        // 获取所有子物体的SpriteRenderer组件
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);
        
        // 记录修改数量
        int modifiedCount = 0;

        // 设置材质
        foreach (SpriteRenderer renderer in spriteRenderers)
        {
            // 记录修改用于撤销
            Undo.RecordObject(renderer, "Change Sprite Material");
            
            renderer.material = material;
            modifiedCount++;
            
            // 标记为已修改
            EditorUtility.SetDirty(renderer);
        }

        // 保存场景
        if (modifiedCount > 0)
        {
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
            Debug.Log($"已修改 {modifiedCount} 个精灵的材质");
        }
        else
        {
            Debug.Log("未找到需要修改的精灵");
        }
        #endif
    }
}