using UnityEngine;

public class UIManager : MonoBehaviour
{
    private bool escMenuOpen = false;
    private bool inventoryOpen = false;

    public GameObject escBackground;

    public bool IsEscOpen()
    {
        return escMenuOpen;
    }

    public bool IsInventoryOpen()
    {
        return inventoryOpen;
    }

    // ---------------------------
    // ESC 메뉴
    // ---------------------------
    public void ToggleEscMenu()
    {
        escMenuOpen = !escMenuOpen;

        // ESC만 시간 멈춤
        Time.timeScale = escMenuOpen ? 0f : 1f;

        if (escBackground != null)
            escBackground.SetActive(escMenuOpen);

        Debug.Log("ESC 메뉴 " + (escMenuOpen ? "열림" : "닫힘"));
    }

    // ---------------------------
    // 인벤토리
    // ---------------------------
    public void ToggleInventory()
    {
        inventoryOpen = !inventoryOpen;

        // ❌ 시간 멈추지 말기! 플레이어는 자유롭게 움직여야 함
        // Time.timeScale = inventoryOpen ? 0f : 1f;  // ← 이거 삭제

        Debug.Log("인벤토리 " + (inventoryOpen ? "열림" : "닫힘"));
    }
}
