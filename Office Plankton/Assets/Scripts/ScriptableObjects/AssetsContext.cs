using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = "AssetsContext", menuName = "Data/AssetsContext")]
public class AssetsContext : ScriptableObject
{
    [SerializeField] private GameObject[] _assets;
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private ScriptableObject[] _scriptableObjects;

    public GameObject GetAsset(string assetName)
    {
        return _assets.FirstOrDefault(asset => asset.gameObject.name == assetName);
    }

    public Sprite GetSprite(string spriteName)
    {
        return _sprites.FirstOrDefault(sprite => sprite.name == spriteName);
    }

    public ScriptableObject GetScriptableObject(string spriteName)
    {
        return _scriptableObjects.FirstOrDefault(scriptableObject => scriptableObject.name == spriteName);
    }
}
