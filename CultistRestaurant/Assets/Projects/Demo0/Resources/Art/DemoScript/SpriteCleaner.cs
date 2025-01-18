using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class SpriteCleaner : MonoBehaviour
{
    [SerializeField]
    private string spritePath = "Assets/Projects/Demo0/Resources/Art/Sprites/";

    [Button("清理所有精灵边界")]
    private void CleanAllSprites()
    {
        if (!Directory.Exists(spritePath))
        {
            Debug.LogError($"路径不存在: {spritePath}");
            return;
        }

        // 获取所有png文件
        string[] pngFiles = Directory.GetFiles(spritePath, "*.png", SearchOption.AllDirectories);
        
        foreach (string pngPath in pngFiles)
        {
            string assetPath = pngPath.Replace('\\', '/');
            TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            
            if (importer != null)
            {
                // 设置为单个精灵模式
                importer.spriteImportMode = SpriteImportMode.Single;
                
                // 获取精灵的边界
                TextureImporterSettings settings = new TextureImporterSettings();
                importer.ReadTextureSettings(settings);
                
                // 设置精灵边界紧贴像素
                settings.spriteMeshType = SpriteMeshType.FullRect;
                settings.spriteExtrude = 0;
                settings.spritePivot = new Vector2(0.5f, 0.5f);
                settings.spritePixelsPerUnit = 100;
                settings.spriteGenerateFallbackPhysicsShape = false;
                settings.spriteBorder = Vector4.zero;
                
                // 应用设置
                importer.SetTextureSettings(settings);
                
                // 设置精灵的矩形完全覆盖图片
                SpriteMetaData[] spritesheet = new SpriteMetaData[1];
                TextureImporterPlatformSettings platformSettings = importer.GetDefaultPlatformTextureSettings();
                Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
                
                spritesheet[0] = new SpriteMetaData
                {
                    name = Path.GetFileNameWithoutExtension(assetPath),
                    rect = new Rect(0, 0, texture.width, texture.height),
                    pivot = new Vector2(0.5f, 0.5f),
                    border = Vector4.zero,
                    alignment = (int)SpriteAlignment.Center
                };
                
                importer.spritesheet = spritesheet;
                
                // 保存修改
                EditorUtility.SetDirty(importer);
                importer.SaveAndReimport();
                
                Debug.Log($"已处理: {assetPath}");
            }
        }
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("所有精灵边界清理完成！");
    }
}