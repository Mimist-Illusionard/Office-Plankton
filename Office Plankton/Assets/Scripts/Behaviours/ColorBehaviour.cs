using UnityEngine;


public class ColorBehaviour : MonoBehaviour
{
    [SerializeField] private Color _firstColor;
    [SerializeField] private Color _secondColor;

    private Material _materialInstance;
    private Renderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();

        _materialInstance = new Material(_renderer.material);
        _renderer.material = _materialInstance;
    }

    public void ChangeToFirstColor()
    {
        _materialInstance.color = _firstColor;
    }

    public void ChangeToSecondColor()
    {
        _materialInstance.color = _secondColor;
    }
}
