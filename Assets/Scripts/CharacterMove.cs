using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    // MonsterManager와 똑같은 레인 좌표를 고정합니다.
    private float[] laneX = { -2.5f, -1.5f, -0.5f, 0.5f, 1.5f, 2.5f };

    // 시작 위치 (0.5 좌표인 3번 레인에서 시작)
    private int currentLaneIndex = 3;

    void Start()
    {
        // 시작할 때 캐릭터 위치를 초기 레인에 딱 맞춥니다.
        UpdatePosition();
    }

    void Update()
    {
        // GetKey가 아니라 GetKeyDown을 써서 '한 번 누를 때 한 칸만' 움직이게 합니다.
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            MoveLane(-1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            MoveLane(1);
        }
    }

    void MoveLane(int direction)
    {
        // 레인 범위를 벗어나지 않게 체크 (0번 ~ 5번 사이)
        int nextIndex = currentLaneIndex + direction;

        if (nextIndex >= 0 && nextIndex < laneX.Length)
        {
            currentLaneIndex = nextIndex;
            UpdatePosition();
        }
    }

    void UpdatePosition()
    {
        // Y와 Z 좌표는 유지하고 X 좌표만 해당 레인으로 순간이동 시킵니다.
        transform.position = new Vector3(laneX[currentLaneIndex], transform.position.y, transform.position.z);
    }
}