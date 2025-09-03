using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 範囲内にあるアイテムを検索するクラス
public class ItemSearch : MonoBehaviour
{
    [SerializeField, Header("アイテムを取得する原点(これを元に近いアイテム、遠いアイテムが決まる)")] private GameObject originPoint;

    public List<GameObject> ItemList { get; private set; } = new List<GameObject>();

    [Header("アイテムリストを常に更新")]
    public bool isAlwaysUpdate = true;
    // アイテムリストが更新されたか
    private bool isItemListUpdate;

    private void Start()
    {
        // アイテム削除関数を実行開始
        StartCoroutine(LateFixedUpdate());
    }

    private void Update()
    {
        // リストのごみを削除
        ItemList.RemoveAll(item => item == null);
    }

    // オブジェクトと接触した時呼ばれる
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            isItemListUpdate = true;
            ItemList.Add(other.gameObject);
        }
    }


    // オブジェクトが離れた時呼ばれる
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            isItemListUpdate = true;
            ItemList.Remove(other.gameObject);
        }
    }


    // FixedUpdateの実行タイミングで一番最後に実行される関数
    private IEnumerator LateFixedUpdate()
    {
        var waitForFixed = new WaitForFixedUpdate();
        while (true)
        {
            // 常に更新フラグが立っているか、アイテムリストが更新された時だけ要素の入れ替えを実行
            if (isAlwaysUpdate || isItemListUpdate)
            {
                PickUpNearItemFirst();
                isItemListUpdate = false;
            }
            yield return waitForFixed;
        }
    }


    // 一番近場のアイテムを配列の先頭に持ってくる
    private void PickUpNearItemFirst()
    {
        if (ItemList.Count <= 1) return;

        var originPos = originPoint.transform.position;
        if (ItemList[0] != null)
        {
            // 初期最小値を設定
            var minDirection = Vector3.Distance(ItemList[0].transform.position, originPos);
            // 二つ目のアイテムから取得ポイントとの距離を計算
            for (int itemNum = 1; itemNum < ItemList.Count; itemNum++)
            {
                var direction = Vector3.Distance(ItemList[itemNum].transform.position, originPos);
                // より近いオブジェクトを0番目の要素に代入
                if (minDirection > direction)
                {
                    minDirection = direction;
                    var temp = ItemList[0];
                    ItemList[0] = ItemList[itemNum];
                    ItemList[itemNum] = temp;
                }
            }
        }
    }


    // 一番近いアイテムを返す
    public GameObject GetNearItem()
    {
        if (ItemList.Count <= 0) return null;

        return ItemList[0];
    }

#if UNITY_EDITOR // UnityEditorのみ
    /// 拾う対象のアイテムにギズモを表示
    private void SetPickUpTargetItemMarker()
    {
        var item = GetNearItem();
        if (item == null) return;
        Gizmos.DrawSphere(item.transform.position, 0.1f);
    }
    private void OnDrawGizmos()
    {
        SetPickUpTargetItemMarker();
    }
#endif
}