using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PostUi : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _name;

    public void ChangePostUi(Post post)
    {
        _image.sprite = post.PostImage;
        _name.text = post.PostName;
    }
}
