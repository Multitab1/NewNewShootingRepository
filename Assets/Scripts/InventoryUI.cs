using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public InventorySlotUI[] bagSlots;
    public InventorySlotUI[] equipSlots;

    private void Start()
    {
        inventoryPanel.SetActive(false);
    }

    public void Toggle()
    {
        // 패널 열기/닫기
        // 1. 인벤토리 패널이 연결되어 있는지 확인
        if (inventoryPanel == null)
        {
            // 2. 패널이 연결되지 않았으면 경고 로그 출력
            Debug.LogWarning("Inventory Panel이 연결되지 않았습니다.");

            // 3. 더 이상 실행하지 않고 함수 종료
            return;
        }

        // 4. 현재 패널 상태
        bool nextOpen = !inventoryPanel.activeSelf;

        // 5. 현재 패널 상태의 반대로 변경
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);

        // 열릴 때 Refresh() 호출
        if (nextOpen)
        {
            Refresh();
        }
    }

    public void Refresh()
    {
        // bagSlots와 equipSlots에 PlayerInventory의 리스트 연결
        PlayerInventory inventory = PlayerInventory.Instance; // 1. PlayerInventory 싱글톤 인스턴스 가져오기

        if (inventory == null) // 2. PlayerInventory가 존재하는지 확인
        {
            Debug.LogWarning("PlayerInventory.Instance가 없습니다."); // 3. 인벤토리 인스턴스가 없으면 경고 로그 출력
            return; // 4. 더 이상 실행하지 않고 함수 종료
        }

        for (int i = 0; i < bagSlots.Length; i++) // 5. 가방 슬롯 UI에 PlayerInventory의 bagItems 리스트 연결
        {
            bagSlots[i].SetSlot(this, inventory.bagItems, i); // 6. 현재 가방 슬롯에 UI 매니저, 아이템 리스트, 슬롯 인덱스 설정
        }

        for (int i = 0; i < equipSlots.Length; i++) // 7. 장비 슬롯 UI에 PlayerInventory의 equipItems 리스트 연결
        {
            equipSlots[i].SetSlot(this, inventory.equipItems, i); // 8. 현재 장비 슬롯에 UI 매니저, 아이템 리스트, 슬롯 인덱스 설정
        }
    }
}
