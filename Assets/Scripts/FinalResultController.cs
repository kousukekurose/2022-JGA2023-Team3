using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

// �ŏI���ʉ�ʂ��Ǘ�����N���X
public class FinalResultController : MonoBehaviour
{
    // ���v�X�R�A
    int[] totalScore = { 0, 0 };

    // ���s�C���[�W
    [SerializeField]
    Image[] resultImage;

    // ���s�e�L�X�g
    [SerializeField]
    TextMeshProUGUI[] resultText;

    // ���E���h�X�R�A��\������UI
    [SerializeField]
    ChildArrayScore[] roundScoreText;
    [Serializable]
    public class ChildArrayScore
    {
        public TextMeshProUGUI[] roundScore;
    }

    // �ŏI�X�R�A
    [SerializeField]
    TextMeshProUGUI[] totalScoreText;

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
    ChildArrayIngredients[] ingredientsImatgesUI;
    [Serializable]
    public class ChildArrayIngredients
    {
        public Image[] ingredientsImatges;
    }

    // �\�����郉�E���h
    int[] RoundNum { set; get; } = { 0, 0 };

    // �v���C���[�̏����ʒu�w��
    [SerializeField]
    Vector3[] firstPos;

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

    // �X�R�A��\������܂ł̑ҋ@����
    [SerializeField]
    float time = 3.0f;

    // ���Ԃ��J�E���g
    float count = 0;

    // �����G�t�F�N�g
    [SerializeField]
    GameObject effect = null;

    // �G�t�F�N�g�����L��
    bool isEffect = true;

    // �v���C���[���̃G�t�F�N�g�����ʒu
    [SerializeField]
    Transform[] effectPos;

    //�G�t�F�N�g�����ʒu
    Transform generatePos;

    // Start is called before the first frame update
    void Start()
    {
        // �R���|�[�l���g���擾
        audioSource = GetComponent<AudioSource>();
        // �W���O���Đ�
        audioSource.clip = audioClipJingle;
        audioSource.Play();

        // State�X�V
        GameManager.Instance.CurrentSceneState = GameManager.SceneState.FinalResult;
        // ActionMap�X�V
        GameManager.Instance.SetActionMap("Extensively");

        for (int i = 0; i < GameManager.Instance.Players.Count; i++)
        {
            // �����ʒu�w��
            GameManager.Instance.Players[i].transform.position = firstPos[i];
        }
    }

    public void Update()
    {
        count += Time.deltaTime;
        // �T�E���h��ύX
        switchAudioController.SwitchAudio(audioClipJingle, audioClipBGM, audioSource);
        if (count > time)
        {
            // ���s�\��
            totalScore[0] = PlayerPrefs.GetInt("Score0");
            totalScore[1] = PlayerPrefs.GetInt("Score1");

            // ���s�e�L�X�g�\��
            if (totalScore[0] > totalScore[1])
            {
                resultText[0].text = "����";
                resultImage[0].color = Color.red;
                resultText[1].text = "����";
                resultImage[1].color = Color.blue;
                generatePos = effectPos[0];
            }
            else if (totalScore[1] > totalScore[0])
            {
                resultText[0].text = "����";
                resultImage[0].color = Color.blue;
                resultText[1].text = "����";
                resultImage[1].color = Color.red;
                generatePos = effectPos[1];
            }
            else
            {
                resultText[0].text = "��������";
                resultText[1].text = "��������";
                resultImage[0].color = Color.yellow;
                resultImage[1].color = Color.yellow;
            }

            // �����G�t�F�N�g����
            if (isEffect)
            {
                if (generatePos != null)
                {
                    Instantiate(effect, generatePos.position, Quaternion.identity);
                }
                isEffect = false;
            }

            for (int i = 0; i < GameManager.Instance.Players.Count; i++)
            {
                // �X�R�A�\��
                // ���E���h�X�R�A�\��
                for (int j = 0; j < GameManager.Instance.roundResults[i].RoundScore.Length; j++)
                {
                    ShowScore(roundScoreText[i].roundScore[j], GameManager.Instance.roundResults[i].RoundScore[j]);
                }
                // ���v�X�R�A�\��
                totalScore[i] = PlayerPrefs.GetInt($"Score{i}", 0);
                ShowScore(totalScoreText[i], totalScore[i]);
            }
            for (int i = 0; i < GameManager.Instance.Players.Count; i++)
            {
                // UI�\��
                ShowResultUI(i, GameManager.Instance.roundResults[i].SelectCookingId[RoundNum[i]], GameManager.Instance.roundResults[i].SelectIngredients[RoundNum[i]]);
            }
        }
    }

    // �X�R�A�\��
    private void ShowScore(TextMeshProUGUI scoreText, int score)
    {
        scoreText.text = string.Format("{0}", score);
    }

    // UI�\��
    private void ShowResultUI(int playerIndex, int cookingId, List<int> ingredients)
    {
        // ���V�s�\��
        dishImageUI[playerIndex].sprite = dishImage[cookingId];
        // �H�ޕ\��
        for (int i = 0; i < ingredientsImatgesUI[playerIndex].ingredientsImatges.Length; i++)
        {
            // �H�ނ������Ă��Ȃ��ꍇ�͕\�����Ȃ�
            if (ingredients.Count > i)
            {
                ingredientsImatgesUI[playerIndex].ingredientsImatges[i].sprite = ingredientsImatge[ingredients[i]];
            }
            else
            {
                ingredientsImatgesUI[playerIndex].ingredientsImatges[i].sprite = null;
            }
        }
    }

    // RoundNum�����Z
    public void AddRoundNum(int playerIndex)
    {
        if (RoundNum[playerIndex] < 2)
        {
            RoundNum[playerIndex]++;
        }
    }

    // RoundNum�����Z
    public void SubRoundNum(int playerIndex)
    {
        if (RoundNum[playerIndex] > 0)
        {
            RoundNum[playerIndex]--;
        }
    }
}
