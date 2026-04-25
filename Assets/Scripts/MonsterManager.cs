using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager instance;
    [SerializeField] private GameObject monsterPrefab;

    [Header("--- Balance ---")]
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float spawnInterval = 4.0f;
    [SerializeField][Range(0, 1)] private float blackChance = 0.4f;

    public Color[] allColors = {
        Color.red, Color.yellow, Color.green, Color.blue, Color.white,
        new Color(1f, 0.5f, 0f), new Color(0.5f, 0f, 0.5f)
    };

    private float[] laneX = { -2.5f, -1.5f, -0.5f, 0.5f, 1.5f, 2.5f };
    private float timer;

    private void Awake()
    {
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }
    }

    // [수정된 부분] 시작하자마자 색깔부터 정하고 소환합니다.
    void Start()
    {
        PickNewRound(); // 1. 색깔 먼저 정하기 (정답지 작성)
        SpawnWave();    // 2. 그 다음 소환하기 (시험지 배부)
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            PickNewRound(); // 다음 라운드 색깔 미리 정하기
            SpawnWave();
            timer = 0;
        }
    }

    public void PickNewRound()
    {
        Color[] round = new Color[6];
        for (int i = 0; i < 6; i++)
        {
            Color picked;
            // 검은색은 정답으로 쓰지 않도록 방어
            do { picked = allColors[Random.Range(0, allColors.Length)]; }
            while (picked == Color.black);

            round[i] = picked;
        }
        // 이 함수가 실행되어야 상단 UI(하얀색 박스들)가 실제 색깔로 변합니다.
        GameManager.instance.SetRound(round);
    }

    void SpawnWave()
    {
        for (int i = 0; i < 6; i++)
        {
            Vector3 spawnPos = new Vector3(laneX[i], transform.position.y, 0);
            GameObject go = Instantiate(monsterPrefab, spawnPos, Quaternion.identity);

            Monster script = go.GetComponent<Monster>();

            // 이제 PickNewRound가 먼저 실행됐으므로, 
            // script.SetMonster가 실행될 때 올바른 정답 색깔을 가져올 수 있습니다.
            if (Random.value < blackChance) script.SetAsBlack(moveSpeed);
            else script.SetMonster(i, moveSpeed);
        }
    }
}