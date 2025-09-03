using UnityEngine;

// �擾���Ă���H�ނ�\������N���X
public class InventryUI : MonoBehaviour
{
    [SerializeField]
    Transform slotsParent;
    Slot[] slots;

    [SerializeField]
    int playerIndex;

    [SerializeField]
    GameObject[] images;

    void Awake()
    {
        //Start����InventryUI�ŎQ�Ƃ���Ƃ��ɃG���[��f������
        //�e�v�f(InventryParent)�ɂ��Ă��邷�ׂĂ�Slot���܂񂾎q�v�f�����ׂĎ擾����
        slots = slotsParent.GetComponentsInChildren<Slot>();
    }

    private void Update()
    {
        UpdateItemUI();
        UpdateSelectUI();
    }

    //Inventry�ɃA�C�e��������Ε\��������Δ�\��
    public void UpdateItemUI()
    {
        var ingredients = GameManager.Instance.Ingredients[playerIndex];
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < ingredients.Count)
            {
                slots[i].AddItem(ingredients[i].ItemData);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }

    // �t�H�[�J�X��\��
    public void UpdateSelectUI()
    {
        for (int i = 0; i < images.Length; i++)
        {
            if (i == GameManager.Instance.Players[playerIndex].SelectNum)
            {
                images[i].SetActive(true);
            }
            else
            {
                images[i].SetActive(false);
            }
        }
    }
}
