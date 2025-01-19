using UnityEngine;

public class SceneEffect : MonoBehaviour
{
    // 公共属性
    [SerializeField] 
    private int _isSunk; // 修改为私有字段

    public int isSunk
    {
        get => _isSunk;
        set
        {
            _isSunk = value;
            UpdateDistortState();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateDistortState(); // 初始化时更新状态
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 更新子节点的状态
    private void UpdateDistortState()
    {
        // 查找子节点中的distort节点
        Transform distortTransform = transform.Find("distort");
        if (distortTransform != null)
        {
            distortTransform.gameObject.SetActive(_isSunk == 1);
        }
    }

    // 公共方法来打开或关闭distort
    public void SetDistortActive(bool isActive)
    {
        _isSunk = isActive ? 1 : 0; // 根据参数设置_isSunk
        UpdateDistortState(); // 更新状态
    }
}
