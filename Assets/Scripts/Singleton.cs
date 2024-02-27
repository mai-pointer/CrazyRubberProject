using UnityEngine;

[DefaultExecutionOrder(0)]
[DisallowMultipleComponent]

public class Singleton : MonoBehaviour
{
    void Awake()
    {
        Instanciar<Singleton>.Singletons(this, gameObject);
    }
}