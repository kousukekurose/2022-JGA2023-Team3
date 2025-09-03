using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �͈͓��ɂ���A�C�e������������N���X
public class ItemSearch : MonoBehaviour
{
    [SerializeField, Header("�A�C�e�����擾���錴�_(��������ɋ߂��A�C�e���A�����A�C�e�������܂�)")] private GameObject originPoint;

    public List<GameObject> ItemList { get; private set; } = new List<GameObject>();

    [Header("�A�C�e�����X�g����ɍX�V")]
    public bool isAlwaysUpdate = true;
    // �A�C�e�����X�g���X�V���ꂽ��
    private bool isItemListUpdate;

    private void Start()
    {
        // �A�C�e���폜�֐������s�J�n
        StartCoroutine(LateFixedUpdate());
    }

    private void Update()
    {
        // ���X�g�̂��݂��폜
        ItemList.RemoveAll(item => item == null);
    }

    // �I�u�W�F�N�g�ƐڐG�������Ă΂��
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            isItemListUpdate = true;
            ItemList.Add(other.gameObject);
        }
    }


    // �I�u�W�F�N�g�����ꂽ���Ă΂��
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            isItemListUpdate = true;
            ItemList.Remove(other.gameObject);
        }
    }


    // FixedUpdate�̎��s�^�C�~���O�ň�ԍŌ�Ɏ��s�����֐�
    private IEnumerator LateFixedUpdate()
    {
        var waitForFixed = new WaitForFixedUpdate();
        while (true)
        {
            // ��ɍX�V�t���O�������Ă��邩�A�A�C�e�����X�g���X�V���ꂽ�������v�f�̓���ւ������s
            if (isAlwaysUpdate || isItemListUpdate)
            {
                PickUpNearItemFirst();
                isItemListUpdate = false;
            }
            yield return waitForFixed;
        }
    }


    // ��ԋߏ�̃A�C�e����z��̐擪�Ɏ����Ă���
    private void PickUpNearItemFirst()
    {
        if (ItemList.Count <= 1) return;

        var originPos = originPoint.transform.position;
        if (ItemList[0] != null)
        {
            // �����ŏ��l��ݒ�
            var minDirection = Vector3.Distance(ItemList[0].transform.position, originPos);
            // ��ڂ̃A�C�e������擾�|�C���g�Ƃ̋������v�Z
            for (int itemNum = 1; itemNum < ItemList.Count; itemNum++)
            {
                var direction = Vector3.Distance(ItemList[itemNum].transform.position, originPos);
                // ���߂��I�u�W�F�N�g��0�Ԗڂ̗v�f�ɑ��
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


    // ��ԋ߂��A�C�e����Ԃ�
    public GameObject GetNearItem()
    {
        if (ItemList.Count <= 0) return null;

        return ItemList[0];
    }

#if UNITY_EDITOR // UnityEditor�̂�
    /// �E���Ώۂ̃A�C�e���ɃM�Y����\��
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