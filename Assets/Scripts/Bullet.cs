using UnityEngine;

public class Bullet : MonoBehaviour
{
    float spd = 15f; // 속도를 살짝 높였습니다.

    void Start()
    {
        // 총알이 화면 밖으로 나가도 안 사라지면 렉이 걸리므로 3초 뒤 자동 삭제
        Destroy(gameObject, 3f);
    }

    void Update()
    {
        transform.Translate(Vector2.up * spd * Time.deltaTime);
    }
}