using UnityEngine;

[CreateAssetMenu(fileName = "Order", menuName = "Scriptable Objects/Order")]
public class Order : ScriptableObject
{
    public Material itemSprite;
    public string itemName;
    public int quantity;
}
