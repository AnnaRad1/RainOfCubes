using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Platform : MonoBehaviour
{
    public Renderer Renderer { get; private set; }

    private void Awake()
    {
        Renderer = GetComponent<Renderer>();
    }
}
