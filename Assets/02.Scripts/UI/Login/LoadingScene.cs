using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    // 목표: 다음 씬을 '비동기 방식'으로 로드
    //      또한 로딩 진행률을 시각적으로 표현
    //                          ㄴ %프로그래스 바와 %별 텍스트
    
    // 속성:
    //  - 다음 씬 번호(인덱스)
    public int NextSceneIndex = 2;

    // - 프로그래서 슬라이더바
    public Slider ProgressSlider;

    // - 프로그레스 텍스트
    public TextMeshProUGUI ProgressText;

    private void Start()
    {
        // SceneManager.LoadScene(NextSceneIndex);
        StartCoroutine(LoadNextScene_Coroutine());
    }

    private IEnumerator LoadNextScene_Coroutine()
    {
        // 지정된 씬을 비동기로 로드
        AsyncOperation ao = SceneManager.LoadSceneAsync(NextSceneIndex);
        ao.allowSceneActivation = false;    // 비동기로 로드되는 씬의 모습이 화면에 보이지 않게 한다.

        // 로딩이 되는 동안 계속해서 반복문
        while(ao.isDone == false)
        {
            // 비동기로 실행할 코드
            ProgressSlider.value = ao.progress; // 0-1 값을 가짐
            ProgressText.text = $"{ao.progress * 100f}%";

            // 서버와 통신해서 유저 데이터, 기획 데이터를 받아옴\
            // 게임 중간마다 받아오는 것보다 로딩동안 데이터를 모두 받아오는 것이 추천됨

            if(ao.progress <= 0.1f)
            {
                ProgressText.text = $"{ao.progress * 100f}%... [시스템] 전술 위성 링크 요청 중... 암호화 채널 확보 대기.";
            }

            if(ao.progress >= 0.2f)
            {
                ProgressText.text = $"{ao.progress * 100f}%... [INFO] 작전지 도착: 구역 C-17 / 산업시설 잔해지대.\n[INFO] 현장 조건: 고온, 화재 잔존, 시야 불량.";
            }

            if(ao.progress <= 0.4f)
            {
                ProgressText.text = $"{ao.progress * 100f}%... [SIGINT] 적대 생명체 다수 확인됨. 패턴 불규칙.\n[ROE] 교전규칙: 최대 억제, 생존 우선.";
            }

            if(ao.progress <= 0.6f)
            {
                ProgressText.text = $"{ao.progress * 100f}%... [LOADOUT] 유닛 ID: UNITY-01 / 장비 상태: FULL OPS.\n[SYSCHK] 무장, 통신, HUD 이상 없음. 작전 가능.";
            }

            if(ao.progress <= 0.8f)
            {
                ProgressText.text = $"{ao.progress * 100f}%... [WARNING] 외부 통신 두절. 전술 네트워크 단절됨.\n[MODE] 단독 전투 모드로 전환. 임시 지휘권 부여됨";
            }

            if(ao.progress >= 0.9f)
            {
                ao.allowSceneActivation = true;
            }

            // yield return new WaitForSeconds(1); // 1초 대기
            yield return null;  // 1프레임 대기
        }
    }
}
