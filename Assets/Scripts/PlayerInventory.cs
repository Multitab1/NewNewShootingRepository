using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }

    public int bagSlotCount = 12;
    public int equipSlotCount = 3;

    public List<InventoryItem> bagItems = new List<InventoryItem>();
    public List<InventoryItem> equipItems = new List<InventoryItem>();

    private void Awake()
    {
        Instance = this;

        // bagItems와 equipItems를 슬롯 수만큼 null로 채우기
        bagItems.Clear();      // 1. 기존 가방 아이템 리스트 초기화
        equipItems.Clear();    // 2. 기존 장비 아이템 리스트 초기화

        FillEmptySlots(bagItems, bagSlotCount);       // 3. 가방 슬롯 개수만큼 빈 슬롯 생성
        FillEmptySlots(equipItems, equipSlotCount);   // 4. 장비 슬롯 개수만큼 빈 슬롯 생성
    }

    private void FillEmptySlots(List<InventoryItem> list, int slotCount)
    {
        // 리스트의 개수가 슬롯 개수보다 적으면 빈 슬롯 추가
        while (list.Count < slotCount)
        {
            list.Add(null); // 빈 슬롯을 의미하는 null 추가
        }
    }

    public bool AddItem(ItemData itemData, int count = 1)
    {
        // 같은 아이템이 있으면 개수 누적
        if (itemData == null) return false; // 1. 추가할 ItemData가 없습니다.
        if (count <= 0) return false;       // 2. 추가할 아이템 개수가 0 이하입니다.

        if (itemData.canStack)              // 3. 이미 있는 스택 아이템에 추가
        {
            for (int i = 0; i < bagItems.Count; i++) // 4. 가방 슬롯을 처음부터 끝까지 검사
            {
                InventoryItem item = bagItems[i];    // 5. 현재 슬롯에 들어있는 아이템 가져오기

                // 6. 슬롯에 아이템이 있고, 같은 아이템이며, 최대 스택 개수보다 적게 쌓여있는지 확인
                if (item != null && item.data == itemData && item.count < itemData.maxStack)
                {
                    // 7. 현재 스택에 추가할 수 있는 개수 계산
                    int addCount = Mathf.Min(count, itemData.maxStack - item.count);
                    item.count += addCount;   // 8. 기존 스택 아이템 개수 증가
                    count -= addCount;        // 9. 추가한 개수만큼 남은 획득 개수 감소

                    // 10. 모든 아이템을 스택에 추가했다면 성공 처리
                    if (count <= 0)
                    {
                        // 11. 스택 추가 성공 로그 출력
                        Debug.Log(itemData.itemName + " 스택 추가 성공");
                        return true; // 12. 아이템 추가 성공 반환
                    }
                }
            }
        }

        // 빈 칸을 찾아 새 아이템 넣기
        for (int i = 0; i < bagItems.Count; i++) // 1. 빈 슬롯에 새로 추가
        {
            // 2. 현재 슬롯이 비어있거나 아이템 데이터가 없는 슬롯인지 확인
            if (bagItems[i] == null || bagItems[i].data == null)
            {
                // 3. 스택 가능한 아이템이면 최대 스택 수까지 추가하고, 아니면 1개만 추가
                int addCount = itemData.canStack ? Mathf.Min(count, itemData.maxStack) : 1;

                // 4. 빈 슬롯에 새 인벤토리 아이템 생성 후 추가
                bagItems[i] = new InventoryItem(itemData, addCount);
                count -= addCount; // 5. 추가한 개수만큼 남은 획득 개수 감소

                // 6. 새 슬롯 추가 성공 로그 출력
                Debug.Log(itemData.itemName + " 새 슬롯에 추가 성공");

                if (count <= 0) // 7. 모든 아이템을 추가했다면 성공 처리
                {
                    return true; // 8. 아이템 추가 성공 반환
                }
            }
        }

        return false;
    }

    public void MoveItem(List<InventoryItem> fromList, int fromIndex, List<InventoryItem> toList, int toIndex)
    {
        /*
         * from 슬롯과 to 슬롯의 아이템을 서로 바꾸기
         */
        // 2. 이동할 슬롯과 도착 슬롯의 인덱스가 유효한지 확인
        if (!IsValidIndex(fromList, fromIndex) || !IsValidIndex(toList, toIndex)) return;

        if (fromList[fromIndex] == null) return; // 3. 이동할 슬롯에 아이템이 없으면 이동하지 않음

        InventoryItem temp = toList[toIndex];    // 4. 도착 슬롯에 있던 아이템을 임시로 저장
        toList[toIndex] = fromList[fromIndex];   // 5. 이동할 슬롯의 아이템을 도착 슬롯으로 이동
        fromList[fromIndex] = temp;              // 6. 임시로 저장한 아이템을 이동한 슬롯에 넣기
    }

    // 1. 리스트가 존재하고, 인덱스가 0 이상이며, 리스트 범위 안에 있는지 확인
    private bool IsValidIndex(List<InventoryItem> list, int index)
    {
        return list != null && index >= 0 && index < list.Count;
    }
}