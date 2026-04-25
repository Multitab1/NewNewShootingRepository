using UnityEngine;
using TMPro; // TextMeshPro를 쓰기 위해 필수

public class ScoreManager : MonoBehaviour
{
    // [연결] 유니티 인스펙터에서 텍스트 오브젝트를 드래그해서 넣어주세요
    public TextMeshProUGUI nowScoreUI;
    public TextMeshProUGUI bestScoreUI;

    public int nowScore;
    public int bestScore;

    private void Start()
    {
        // 1. 저장된 최고 점수 불러오기
        bestScore = PlayerPrefs.GetInt("BestScore", 0);

        // 2. UI 초기 텍스트 설정
        if (bestScoreUI != null)
            bestScoreUI.text = "Best Score : " + bestScore;

        nowScore = 0;
        UpdateScoreUI();
    }

    // [중요!] 외부(Monster.cs)에서 "점수 올려!"라고 부를 수 있는 통로입니다.
    // public이 붙어야 다른 스크립트에서 이 함수를 찾을 수 있어요.
    public void AddScore()
    {
        nowScore++; // 점수 1 증가
        UpdateScoreUI(); // 화면 글자 업데이트

        // 최고 점수보다 높으면 갱신
        if (nowScore > bestScore)
        {
            bestScore = nowScore;
            if (bestScoreUI != null)
                bestScoreUI.text = "Best Score : " + bestScore;

            // 기기에 점수 저장
            PlayerPrefs.SetInt("BestScore", bestScore);
        }
    }

    // UI 글자만 전담해서 바꿔주는 함수
    public void UpdateScoreUI()
    {
        if (nowScoreUI != null)
        {
            nowScoreUI.text = "Now Score : " + nowScore;
        }
    }
}