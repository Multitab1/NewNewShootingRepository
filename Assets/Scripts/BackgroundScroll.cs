using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    // 그림이 밀리는 속도 (인스펙터에서 조절하세요)
    [SerializeField] private float scrollSpeed = 0.5f;

    private Renderer myRenderer;
    private Vector2 savedOffset;

    void Start()
    {
        myRenderer = GetComponent<Renderer>();
        // 혹시 모르니 원래 오프셋 값을 저장해둡니다.
        savedOffset = myRenderer.material.mainTextureOffset;
    }

    void Update()
    {
        // 몬스터가 아래로 내려가니까, 배경은 위로 밀려야 속도감이 납니다.
        // Y값을 계속 증가시켜서 그림을 위로 밀어냅니다.
        float y = Mathf.Repeat(Time.time * scrollSpeed, 1);

        // 새로운 오프셋 값을 만듭니다 (X는 고정, Y만 변화)
        Vector2 offset = new Vector2(savedOffset.x, y);

        // 머티리얼에 적용!
        myRenderer.material.mainTextureOffset = offset;
    }

    // 게임 끄면 오프셋 값을 원래대로 돌려놓는 매너 (안 하면 저장됨)
    void OnDisable()
    {
        myRenderer.material.mainTextureOffset = savedOffset;
    }
}