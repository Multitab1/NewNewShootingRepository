using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Image iconImage;
    public TMP_Text countText;

    private InventoryUI inventoryUI;
    private List<InventoryItem> itemList;
    private int index;
    private RectTransform iconRect;          // 3. 드래그할 아이콘의 RectTransform 저장
    private Vector2 iconStartPosition;       // 4. 드래그 시작 전 아이콘의 원래 위치 저장

    public void SetSlot(InventoryUI inventoryUI, List<InventoryItem> itemList, int index)
    {
        this.inventoryUI = inventoryUI;
        this.itemList = itemList;
        this.index = index;
        // 현재 슬롯의 아이콘과 개수 텍스트 갱신
        RefreshView();
    }

    private void Awake()
    {
        if (iconImage != null)               // 5. 아이콘 이미지가 연결되어 있는지 확인
        {
            // 6. 아이콘 이미지의 RectTransform 컴포넌트 가져오기
            iconRect = iconImage.GetComponent<RectTransform>();
        }

        if (countText != null)               // 7. CountText는 드래그/드롭 판정을 막지 않게 함
        {
            // 8. 개수 텍스트가 마우스 클릭, 드래그, 드롭 이벤트를 막지 않도록 설정
            countText.raycastTarget = false;
        }
    }

    public void RefreshView()
    {
        if (itemList == null || index < 0 || index >= itemList.Count)
        {
            ClearView();
            return;
        }

        InventoryItem item = itemList[index];

        if (item == null || item.data == null)
        {
            ClearView();
            return;
        }

        iconImage.enabled = true;
        iconImage.sprite = item.data.icon;
        iconImage.color = Color.white;

        // 아이콘 이미지가 마우스 클릭, 드래그 같은 UI 이벤트를 받을 수 있게 설정
        iconImage.raycastTarget = true;

        if (item.count > 1)
        {
            countText.text = item.count.ToString();
        }
        else
        {
            countText.text = "";
        }
    }

    private void ClearView()
    {
        if (iconImage != null)
        {
            iconImage.enabled = false;
            iconImage.sprite = null;

            // 아이콘 이미지가 UI 이벤트를 받을 수 있게 설정
            iconImage.raycastTarget = true;
        }

        if (countText != null)
        {
            countText.text = "";
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        /*
         * 아이템이 없으면 return, 아이콘 raycast 끄기
         */
        // 1. 아이템 리스트가 없거나 인덱스가 유효하지 않으면 드래그 중단
        if (itemList == null || index < 0 || index >= itemList.Count) return;

        // 2. 현재 슬롯에 아이템이 없거나 아이템 데이터가 없으면 드래그 중단
        if (itemList[index] == null || itemList[index].data == null) return;

        if (iconRect == null) return;   // 3. 아이콘 RectTransform이 없으면 드래그 중단

        Debug.Log("드래그 시작: " + itemList[index].data.itemName); // 4. 드래그 시작 로그 출력

        // 5. 드래그 시작 전 아이콘의 원래 위치 저장
        iconStartPosition = iconRect.anchoredPosition;

        // 6. 드래그 중 아이콘이 EquipSlot의 드롭 판정을 막지 않게 함
        if (iconImage != null)
        {
            // 7. 드래그 중에는 아이콘 이미지가 UI 이벤트를 받지 않도록 설정
            iconImage.raycastTarget = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        /*
         * 마우스 이동량만큼 아이콘 이동
         */
        // 1. 아이템 리스트가 없거나 인덱스가 유효하지 않으면 드래그 이동 중단
        if (itemList == null || index < 0 || index >= itemList.Count) return;

        // 2. 현재 슬롯에 아이템이 없거나 아이템 데이터가 없으면 드래그 이동 중단
        if (itemList[index] == null || itemList[index].data == null) return;

        // 3. 아이콘 RectTransform이 없으면 드래그 이동 중단
        if (iconRect == null) return;

        // 4. 마우스 이동량만큼 아이콘 위치 이동
        iconRect.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        /*
         * 아이콘을 원위치하고 raycast 다시 켜기
         */
        Debug.Log("드래그 종료"); // 1. 드래그 종료 로그 출력

        if (iconRect != null)     // 2. 아이콘 RectTransform이 있으면
        {
            // 3. 아이콘을 드래그 시작 전 위치로 되돌림
            iconRect.anchoredPosition = iconStartPosition;
        }

        if (iconImage != null)    // 4. 아이콘 이미지가 있으면
        {
            // 5. 아이콘 이미지가 다시 UI 이벤트를 받을 수 있게 설정
            iconImage.raycastTarget = true;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        /*
         * 드래그 시작 슬롯을 찾아 PlayerInventory.MoveItem() 호출
         */

        // 1. 드래그를 시작한 오브젝트에서 InventorySlotUI 컴포넌트 가져오기
        InventorySlotUI fromSlot = eventData.pointerDrag.GetComponent<InventorySlotUI>();

        if (fromSlot == null) // 2. 드래그 시작 슬롯을 찾지 못한 경우
        {
            Debug.LogWarning("드래그 시작 슬롯을 찾지 못했습니다."); // 3. 경고 로그 출력
            return; // 4. 더 이상 실행하지 않고 함수 종료
        }

        if (fromSlot == this) return; // 5. 자기 자신에게 드롭한 경우 이동하지 않음

        if (PlayerInventory.Instance == null) // 6. PlayerInventory 인스턴스가 없는 경우
        {
            Debug.LogWarning("PlayerInventory.Instance가 없습니다."); // 7. 경고 로그 출력
            return; // 8. 더 이상 실행하지 않고 함수 종료
        }

        Debug.Log("드롭 성공"); // 9. 드롭 성공 로그 출력

        // 10. 드래그 시작 슬롯의 아이템과 현재 슬롯의 아이템을 이동 또는 교환
        PlayerInventory.Instance.MoveItem(
            fromSlot.itemList,
            fromSlot.index,
            this.itemList,
            this.index
        );

        // 11. 인벤토리 UI가 연결되어 있으면, 아이템 이동 후 인벤토리 UI 새로고침
        if (inventoryUI != null) inventoryUI.Refresh();
    }
}
