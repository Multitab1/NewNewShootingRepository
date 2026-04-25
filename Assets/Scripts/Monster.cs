using UnityEngine;

public class Monster : MonoBehaviour
{
    private int myLaneIndex;
    private Color myColor;
    private float speed;
    public bool isBlack = false;

    [Header("--- 프리팹 연결 (인스펙터에서 꼭 다시 넣으세요) ---")]
    public GameObject prefabsExplosion;

    private bool isDead = false;

    public void SetMonster(int lane, float s)
    {
        myLaneIndex = lane;
        speed = s;
        isBlack = false;
        isDead = false;

        if (GetComponent<Collider>() != null) GetComponent<Collider>().enabled = true;

        myColor = GameManager.instance.roundColors[lane];

        // [난이도 수정] 0.3f -> 0.4f (스파이가 나올 확률 40%로 상향)
        if (Random.value < 0.5f)
        {
            Color wrong;
            do
            {
                wrong = MonsterManager.instance.allColors[Random.Range(0, MonsterManager.instance.allColors.Length)];
            } while (wrong == myColor || wrong == Color.black);
            myColor = wrong;
        }

        GetComponent<Renderer>().material.color = myColor;
    }

    public void SetAsBlack(float s)
    {
        isBlack = true;
        isDead = false;
        speed = s;
        myColor = Color.black;
        GetComponent<Renderer>().material.color = Color.black;

        // 검은색은 물리적으로 존재하지 않게 처리
        if (GetComponent<Collider>() != null) GetComponent<Collider>().enabled = false;
    }

    void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;

        // 화면 하단 탈출 판정 (반응이 없으면 -4.5f를 더 높여보세요)
        if (transform.position.y < -4.5f)
        {
            if (isBlack == false && isDead == false)
            {
                // 스파이(다른 색)인데 그냥 보냈을 때만 게임 오버
                GameManager.instance.CheckCensor(myLaneIndex, myColor, false);
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isBlack || isDead) return;

        if (other.name.ToLower().Contains("bullet"))
        {
            isDead = true;

            GameManager.instance.CheckCensor(myLaneIndex, myColor, true);

            ScoreManager sm = FindObjectOfType<ScoreManager>();
            if (sm != null) sm.AddScore();

            // 폭발 이펙트 실행
            if (prefabsExplosion != null)
            {
                GameObject effect = Instantiate(prefabsExplosion, transform.position, Quaternion.identity);
                Destroy(effect, 2f);
            }

            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Player") || other.name.ToLower().Contains("player"))
        {
            isDead = true;
            GameManager.instance.GameOver("CRASHED!");
        }
    }
}