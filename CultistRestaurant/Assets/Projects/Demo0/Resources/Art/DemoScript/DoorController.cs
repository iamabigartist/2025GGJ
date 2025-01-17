using UnityEngine;

[ExecuteInEditMode]
public class DoorController : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private bool _isOpen;
    
    [SerializeField]
    private UnityEngine.UI.Button doorButton;
    
    public bool isOpen
    {
        get { return _isOpen; }
        set
        {
            string newSpritePath = value ? 
                "Assets/Projects/Demo0/Resources/Art/Restaurant/door_open.png" : 
                "Assets/Projects/Demo0/Resources/Art/Restaurant/door_closed.png";
                
            Debug.Log($"尝试加载精灵图，路径: {newSpritePath}");
            
            Sprite newSprite = Resources.Load<Sprite>(newSpritePath);
            if (newSprite != null)
            {
                _isOpen = value;
                spriteRenderer.sprite = newSprite;
                Debug.Log($"成功切换门的状态为: {_isOpen}");
            }
            else
            {
                Debug.LogError($"切换门的状态失败，无法加载精灵图: {newSpritePath}");
            }
        }
    }

    void Start()
    {
        InitializeComponents();
    }

    void OnEnable()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            
            if (doorButton != null)
            {
                doorButton.onClick.AddListener(ToggleDoor);
            }
            else
            {
                Debug.LogWarning("未设置门的按钮组件，请在Inspector中指定Button");
            }
            
            // 初始化后立即更新门的状态显示
            isOpen = _isOpen;
        }
    }
    
    private void ToggleDoor()
    {
        isOpen = !isOpen;
    }
    
    private void OnDisable()
    {
        if (doorButton != null)
        {
            doorButton.onClick.RemoveListener(ToggleDoor);
        }
    }



}