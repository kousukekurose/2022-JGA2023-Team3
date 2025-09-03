using UnityEngine;

// 食材生成を管理するクラス
public class GenerateFood : MonoBehaviour
{
    // 生成する食材
    [SerializeField]
    GameObject food;

    // 生成する位置
    [SerializeField]
    Transform[] generatePositions;

    // 食材生成可能時間
    [SerializeField]
    float generateTime = 5.0f;

    // 食材生成カウント
    float[] generateCounts = { 99.0f, 99.0f };

    // 食材の有無を判定
    bool[] isGenerates = { true, true };

    // Update is called once per frame
    void Update()
    {
        // 生成位置分繰り返し
        for (int i = 0; i < generatePositions.Length; i++)
        {
            // 食材が生成可能か確認
            isGenerates[i] = CheckGenerate(generatePositions[i]);
            // 生成可能であれば実行
            if (isGenerates[i])
            {
                // 食材が存在しないときにカウントを加算
                generateCounts[i] += Time.deltaTime;
                // 生成可能時間を過ぎている
                if (generateCounts[i] > generateTime)
                {
                    // 食材生成
                    Generate(food, generatePositions[i]);
                    generateCounts[i] = 0;
                }
            }
        }
    }

    // 食材生成
    public void Generate(GameObject food, Transform generatePos)
    {
        // プレハブから食材を生成
        Instantiate(food, generatePos.position, Quaternion.identity).transform.parent = generatePos;
    }

    // 食材の存在確認
    public bool CheckGenerate(Transform generatePos)
    {
        int childCount = generatePos.childCount;
        // 食材がなければ真、あれば偽を返す
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
