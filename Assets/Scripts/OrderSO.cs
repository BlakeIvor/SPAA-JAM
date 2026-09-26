using UnityEngine;

[CreateAssetMenu(fileName = "Order", menuName = "Scriptable Objects/Order")]
public class Order : ScriptableObject
{
    public Sprite itemSprite;
    public string itemName;
    public int quantity;
}
