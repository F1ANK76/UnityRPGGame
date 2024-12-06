using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject
{
    public string itemName; // �������� �̸�

    [TextArea]
    public string itemDesc; // �������� ����

    public ItemType itemType; // �������� ����
    public Sprite itemImage; // �������� �̹���, ��������Ʈ�� ĵ���� ���̵� ��� �� ����
    public GameObject itemPrefab; // �������� ������

    public string weaponType; // ���� ����

    public enum ItemType
    {
        Equipment,
        Used,
        Ingrediant,
        ETC
    }

    
}
