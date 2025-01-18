using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

[ExecuteInEditMode]
public class DemoDoorCtrl : SerializedMonoBehaviour
{
    [SerializeField]
    private bool _isOpen;
    
    [SerializeField]
    private Transform sceneRoot; // 1_sceneObj的引用
    [SerializeField]
    private SpriteRenderer[] openSprites;
    [SerializeField]
    private SpriteRenderer[] closeSprites;
    
    public bool isOpen
    {
        get { return _isOpen; }
        set
        {
            _isOpen = value;
            UpdateSpritesVisibility();
            Debug.Log($"成功切换门的状态为: {_isOpen}");
        }
    }

    void Start()
    {
        InitializeComponents();
    }


    private void InitializeComponents()
    {
        if (!sceneRoot)
        {
            // 尝试在场景中查找1_sceneObj
            sceneRoot = GameObject.Find("1_sceneObj")?.transform;
            if (!sceneRoot)
            {
                Debug.LogError("无法找到1_sceneObj，请确保场景中存在此对象或手动指定");
                return;
            }
        }
        
        // 获取所有相关的SpriteRenderer
        CollectSprites();
        
        // 初始化后立即更新状态显示
        UpdateSpritesVisibility();
    }
    
    private void CollectSprites()
    {
        if (sceneRoot == null) return;

        // 获取所有子物体的SpriteRenderer组件
        SpriteRenderer[] allSprites = sceneRoot.GetComponentsInChildren<SpriteRenderer>(true);
        
        // 使用System.Linq来过滤
        openSprites = allSprites.Where(sr => 
            sr != null && 
            sr.gameObject.name.EndsWith("Open", System.StringComparison.OrdinalIgnoreCase)
        ).ToArray();
        
        closeSprites = allSprites.Where(sr => 
            sr != null && 
            sr.gameObject.name.EndsWith("Close", System.StringComparison.OrdinalIgnoreCase)
        ).ToArray();
        
        Debug.Log($"找到 {openSprites.Length} 个Open精灵和 {closeSprites.Length} 个Close精灵");
    }
    
    private void UpdateSpritesVisibility()
    {
        if (openSprites == null || closeSprites == null)
        {
            CollectSprites();
        }
        
        // 更新所有Open精灵的可见性
        foreach (var sprite in openSprites)
        {
            if (sprite != null)
            {
                sprite.gameObject.SetActive(_isOpen);
            }
        }
        
        // 更新所有Close精灵的可见性
        foreach (var sprite in closeSprites)
        {
            if (sprite != null)
            {
                sprite.gameObject.SetActive(!_isOpen);
            }
        }
    }

    [Button("开关门"), GUIColor(0.4f, 1f, 0.4f)]
    private void ToggleDoorInInspector()
    {
        isOpen = !isOpen;
    }
    
    [Button("重新收集Sprite"), GUIColor(1f, 0.4f, 0.4f)]
    private void RefreshSprites()
    {
        CollectSprites();
        UpdateSpritesVisibility();
    }
}