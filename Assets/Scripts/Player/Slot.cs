using UnityEngine;
using UnityEngine.UI;

// スロットを管理するクラス
public class Slot : MonoBehaviour
{
    public Image icon;
    ItemData itemData;

    // アニメーションの実行確認用
    bool isFirst = true;

    // 事前に参照するコンポーネント」
    Animator animator;

    // AnimatorのパラメーターID
    static readonly int pickUpId = Animator.StringToHash("PickUp");

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    //アイテムが追加されたときにその画像を表示
    public void AddItem(ItemData newitemData)
    {
        itemData = newitemData;
        //取得したアイテムのiconを追加する
        icon.sprite = newitemData.icon;
        if (isFirst)
        {
            animator.SetTrigger(pickUpId);
            isFirst = false;
        }
    }
    //アイテムを削除するときにその画像を削除
    public void ClearSlot()
    {
        itemData = null;
        icon.sprite = null;
        if (!isFirst)
        {
            isFirst = true;
        }
    }
}
