using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ���V�s�\�zUI���Ǘ�����N���X
public class PredictRecipe : MonoBehaviour
{
    // �\�z���V�s
    [SerializeField]
    ChildArray[] predictRecipeImages;
    [Serializable]
    public class ChildArray
    {
        public Image[] recipeImages;
    }

    // ���V�s
    [SerializeField]
    Sprite[] recipe;

    List<int> predictRecipe;

    // Update is called once per frame
    void Update()
    {
        // �v���C���[�����[�v
        for (int i = 0; i < GameManager.Instance.Players.Count; i++)
        {
            // �\�z���V�s�擾
            predictRecipe = GameManager.Instance.GetPredictRecipe(i);
            var recipeImages = predictRecipeImages[i].recipeImages;
            // �\�z���V�s����łȂ��ꍇ
            if (predictRecipe != null)
            {
                // �\�z���V�s�\���g���[�v
                for (int j = 0; j < recipeImages.Length; j++)
                {
                    // �\�z���V�s������Ε\��
                    if (j < predictRecipe.Count)
                    {
                        // �v���C���[[i]�̗\�z���V�sUI���X�V
                        recipeImages[j].sprite = recipe[predictRecipe[j]];
                        // �\���A�j���[�V�������Đ�
                        recipeImages[j].GetComponent<PredictRecipeController>().ShowAnimation();
                    }
                    // ������Δ�\��
                    else
                    {
                        // ��\���A�j���[�V�������Đ�
                        recipeImages[j].GetComponent<PredictRecipeController>().HideAnimation();
                    }
                }
            }
            // �\�z���V�s����̏ꍇ
            else
            {
                // �\�z���V�s�����[�v
                for (int j = 0; j < recipeImages.Length; j++)
                {
                    // ��\���A�j���[�V�������Đ�
                    recipeImages[j].GetComponent<PredictRecipeController>().HideAnimation();
                }
            }
        }
    }
}
