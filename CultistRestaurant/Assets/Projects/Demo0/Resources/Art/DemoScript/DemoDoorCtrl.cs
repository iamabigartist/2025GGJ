using UnityEngine;
using Sirenix.OdinInspector;

[ExecuteInEditMode]
public class DemoDoorCtrl : SerializedMonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private bool _isOpen;
    
    [SerializeField]
    private GameObject doorOpenObject;    // door_open_0 子节点
    [SerializeField]
    private GameObject doorClosedObject;  // door_closed_0 子节点
    
    public bool isOpen
    {
        get { return _isOpen; }
        set
        {
            if (doorOpenObject != null && doorClosedObject != null)
            {
                _isOpen = value;
                doorOpenObject.SetActive(_isOpen);
                doorClosedObject.SetActive(!_isOpen);
                Debug.Log($"成功切换门的状态为: {_isOpen}");
            }
            else
            {
                Debug.LogError("门的开关状态对象未设置，请在Inspector中指定door_open_0和door_closed_0");
            }
        }
    }

    void Start()
    {
        InitializeComponents();
    }


    private void InitializeComponents()
    {
        if (!doorOpenObject || !doorClosedObject)
        {
            // 尝试自动查找子节点
            doorOpenObject = transform.Find("door_open_0")?.gameObject;
            doorClosedObject = transform.Find("door_closed_0")?.gameObject;
            
            if (!doorOpenObject || !doorClosedObject)
            {
                Debug.LogError("无法找到door_open_0或door_closed_0子节点，请确保命名正确或手动指定");
                return;
            }
        }
        
        // 初始化后立即更新门的状态显示
        isOpen = _isOpen;
    }
    
    private void ToggleDoor()
    {
        isOpen = !isOpen;
    }
    

    [Button("开关门"), GUIColor(0.4f, 1f, 0.4f)]
    private void ToggleDoorInInspector()
    {
        isOpen = !isOpen;
    }

}