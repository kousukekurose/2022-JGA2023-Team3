using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using TMPro;

// ���ʉ�ʂ��Ǘ�����N���X
public class ResultController : MonoBehaviour
{
    // ���������_��
    [SerializeField]
    int resultPoint = 500;

    // �i���_��
    [SerializeField]
    int[] qualityScore;

    // 1��v���邲�ƂɖႦ��_��
    [SerializeField]
    int point = 80;

    // 1�s��v���邲�ƂɌ���_��
    [SerializeField]
    int misPoint = 50;

    // �񓚐���_��
    [SerializeField]
    int correctAnswerPoint = 300;

    // �񓚕s����_��
    [SerializeField]
    int uncorrectAnswerPoint = 150;

    // ��v��
    int matchCount = 0;

    // ���v�X�R�A
    int[] totalScore = new int[] { 0, 0 };

    // ���E���h�X�R�A
    int roundScore;

    // �\���X�R�A
    int showScore;

    // �����̉摜
    [SerializeField]
    Sprite[] dishImage;

    // �����̉摜��\������UI
    [SerializeField]
    Image[] dishImageUI;

    // �H�ނ̉摜
    [SerializeField]
    Sprite[] ingredientsImatge;

    // �H�ނ̉摜��\������UI
    [SerializeField]
    ChildArray[] ingredientsImatgesUI;
    [Serializable]
    public class ChildArray
    {
        public Image[] ingredientsImatges;
    }

    // �񓚌��ʃC���[�W
    [SerializeField]
    Image[] resultImage;

    // �񓚌��ʃe�L�X�g
    [SerializeField]
    TextMeshProUGUI[] resultText;
    [SerializeField]
    TextMeshProUGUI[] answerText;

    // �񓚌��ʃX�R�A
    [SerializeField]
    TextMeshProUGUI[] answerScore;

    // ��������
    [SerializeField]
    TextMeshProUGUI[] correctText;

    // �����Ă���H�ސ�
    [SerializeField]
    TextMeshProUGUI[] IngredientsNo;

    // �H�ރX�R�A
    [SerializeField]
    TextMeshProUGUI[] IngredientsScore;

    // �Ԉ���Ă���H�ސ�
    [SerializeField]
    TextMeshProUGUI[] misMatchNo;

    // �Ԉ���Ă���H�ރX�R�A
    [SerializeField]
    TextMeshProUGUI[] misMatchScore;

    // �i���X�R�A
    [SerializeField]
    TextMeshProUGUI[] tmpQualityScore;

    // ���v�X�R�A
    [SerializeField]
    TextMeshProUGUI[] totalScoreText;

    // �v���C���[�̏����ʒu�w��
    [SerializeField]
    Vector3[] firstPos;

    // SceneNavigation���w��
    [SerializeField]
    SceneNavigation sceneNavigation;

    // SwitchAudioController���w��
    [SerializeField]
    SwitchAudioController switchAudioController;

    // ���O�ɎQ�Ƃ���R���|�[�l���g
    AudioSource audioSource;

    // BGM���w��
    [SerializeField]
    AudioClip audioClipBGM = null;

    // �W���O�����w��
    [SerializeField]
    AudioClip audioClipJingle = null;

    // Start is called before the first frame update
    void Start()
    {
        // �R���|�[�l���g���擾
        audioSource = GetComponent<AudioSource>();
        // �T�E���h�Đ�
        audioSource.clip = audioClipJingle;
        audioSource.Play();

        // State�X�V
        GameManager.Instance.CurrentSceneState = GameManager.SceneState.Result;
        // ActionMap�X�V
        GameManager.Instance.SetActionMap("Extensively");

        for (int i = 0; i < GameManager.Instance.Players.Count; i++)
        {
            roundScore = 0;
            // �����ʒu�w��
            GameManager.Instance.Players[i].transform.position = firstPos[i];

            // �I������������\��
            dishImageUI[i].sprite = dishImage[GameManager.Instance.SelectCookingIds[i]];

            totalScore[i] = PlayerPrefs.GetInt($"Score{i}", 0);
            // �񓚌��ʔ���
            // ���ʃe�L�X�g�\��
            if (GameManager.Instance.IsAnswerResult(i))
            {
                showScore = correctAnswerPoint;
                resultText[i].text = "������";
                answerText[i].text = "������";
                resultImage[i].color = Color.red;
            }
            else
            {
                showScore = uncorrectAnswerPoint;
                resultText[i].text = "�ԈႢ";
                answerText[i].text = "�ԈႢ";
                resultImage[i].color = Color.blue;
            }
            // �񓚌��ʃX�R�A�\��
            ShowScore(answerScore[i], showScore);
            roundScore += showScore;

            // �I�𗿗����V�s�擾
            var ingredientIds = GameManager.Instance.GetIngredientIds(i);

            // �擾�H�ރ\�[�g
            List<int> ingredientList = GameManager.Instance.GetIngredientsList(i);


            // �擾�����H�ނ�\��
            for (int j = 0; j < ingredientList.Count; j++)
            {
                ingredientsImatgesUI[i].ingredientsImatges[j].sprite = ingredientsImatge[ingredientList[j]];
            }

            // ���V�s�ʂ�̐H�ނ������Ă��邩
            if (IsMatch(ingredientList, ingredientIds))
            {
                // ���V�s�ʂ�
                showScore = resultPoint;
                IngredientsNo[i].text = " ";
            }
            else
            {
                // ��v�����H�ނ̐���
                showScore = matchCount * point;
                ShowScore(IngredientsNo[i], matchCount);
            }
            // �H�ރX�R�A�\��
            ShowScore(IngredientsScore[i], showScore);
            roundScore += showScore;

            // �s��v���擾
            var mismatchCount = IsMisMatch(ingredientList, ingredientIds);
            ShowScore(misMatchNo[i], mismatchCount);
            showScore = mismatchCount * misPoint;
            // �s��v�X�R�A�\��
            ShowScore(misMatchScore[i], showScore);
            roundScore += showScore;

            // �i���X�R�A�擾
            List<int> qualityList = GameManager.Instance.GetQualityList(i);
            showScore = QualityResult(qualityList);
            // �i���X�R�A�\��
            ShowScore(tmpQualityScore[i], showScore);
            roundScore += showScore;

            // ���v�X�R�A�\��
            ShowScore(totalScoreText[i], roundScore);
            totalScore[i] += roundScore;
            PlayerPrefs.SetInt($"Score{i}", totalScore[i]);

            // �J�ڐ�w��
            if (GameManager.Instance.RoundCount >= 2)
            {
                sceneNavigation.SceneName = "FinalResult";
            }

            // ���E���h���U���g�ۑ�
            GameManager.Instance.SetResultScore(i, roundScore);
        }
        GameManager.Instance.RoundCount++;
    }

    private void Update()
    {
        // �T�E���h�X�V
        switchAudioController.SwitchAudio(audioClipJingle, audioClipBGM, audioSource);
    }

    // ���V�s�Ǝ擾�H�ޔ���
    private bool IsMatch(List<int> cooking, IEnumerable<int> ingredientIds)
    {
        bool isMatch = true;
        matchCount = 0;
        // ���V�s�̑f�ނ������Ă��邩
        foreach (var ingredientId in ingredientIds)
        {
            int count = 0;
            // �K�v�ȐH�ނ̐���H�ނ��ƂɃJ�E���g
            count = cooking.Where(cook => cook == ingredientId).Count();
            matchCount += count;
            if (count == 0)
            {
                isMatch = false;
            }
        }
        if (isMatch)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // �s�v�ȐH�ނ𔻒� 
    private int IsMisMatch(List<int> cooking, IEnumerable<int> ingredientIds)
    {
        int mismatchCount = 0;
        for (int i = 0; i < cooking.Count; i++)
        {
            // �s�K�v�ȐH�ނ�������J�E���g
            if (!(ingredientIds.Any(ingredientId => ingredientId == cooking[i])))
            {
                mismatchCount++;
            }
        }
        return mismatchCount;
    }

    // �i���X�R�A
    private int QualityResult(List<int> qualityList)
    {
        int score = 0;
        foreach (var quality in qualityList)
        {
            score += qualityScore[quality];
        }
        return score;
    }

    // �X�R�A�\��
    private void ShowScore(TextMeshProUGUI scoreText, int score)
    {
        scoreText.text = string.Format("{0}", score);
    }
}
