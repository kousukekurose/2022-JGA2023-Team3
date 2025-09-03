using UnityEngine;

// �H�ސ������Ǘ�����N���X
public class GenerateFood : MonoBehaviour
{
    // ��������H��
    [SerializeField]
    GameObject food;

    // ��������ʒu
    [SerializeField]
    Transform[] generatePositions;

    // �H�ސ����\����
    [SerializeField]
    float generateTime = 5.0f;

    // �H�ސ����J�E���g
    float[] generateCounts = { 99.0f, 99.0f };

    // �H�ނ̗L���𔻒�
    bool[] isGenerates = { true, true };

    // Update is called once per frame
    void Update()
    {
        // �����ʒu���J��Ԃ�
        for (int i = 0; i < generatePositions.Length; i++)
        {
            // �H�ނ������\���m�F
            isGenerates[i] = CheckGenerate(generatePositions[i]);
            // �����\�ł���Ύ��s
            if (isGenerates[i])
            {
                // �H�ނ����݂��Ȃ��Ƃ��ɃJ�E���g�����Z
                generateCounts[i] += Time.deltaTime;
                // �����\���Ԃ��߂��Ă���
                if (generateCounts[i] > generateTime)
                {
                    // �H�ސ���
                    Generate(food, generatePositions[i]);
                    generateCounts[i] = 0;
                }
            }
        }
    }

    // �H�ސ���
    public void Generate(GameObject food, Transform generatePos)
    {
        // �v���n�u����H�ނ𐶐�
        Instantiate(food, generatePos.position, Quaternion.identity).transform.parent = generatePos;
    }

    // �H�ނ̑��݊m�F
    public bool CheckGenerate(Transform generatePos)
    {
        int childCount = generatePos.childCount;
        // �H�ނ��Ȃ���ΐ^�A����΋U��Ԃ�
        if (childCount == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
