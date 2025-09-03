using UnityEngine;

// 取得している食材を表示するクラス
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
        //StartだとInventryUIで参照するときにエラーを吐くため
        //親要素(InventryParent)についているすべてのSlotを含んだ子要素をすべて取得する
        slots = slotsParent.GetComponentsInChildren<Slot>();
    }

    private void Update()
    {
        UpdateItemUI();
        UpdateSelectUI();
    }

    //Inventryにアイテムがあれば表示無ければ非表示
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

    // フォーカスを表示
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
