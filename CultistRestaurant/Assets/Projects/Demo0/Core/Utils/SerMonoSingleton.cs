using Sirenix.OdinInspector;
using UnityEngine;
namespace Projects.Demo0.Core.Utils
{
public class SerMonoSingleton<T> : SerializedMonoBehaviour where T : SerMonoSingleton<T>
{
	public static T Instance { get; private set; }
	protected virtual void Awake() => Instance = this as T;
}

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
	public static T Instance { get; private set; }
	protected virtual void Awake() => Instance = this as T;
}

}