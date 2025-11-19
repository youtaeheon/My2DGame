using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    public float speed = 5f;
    public float interactionRange = 1.5f;

    private Rigidbody2D rb;
    private Animator anim;

    private Vector2 moveDir = Vector2.zero;
    private Vector2 lastMoveDir = Vector2.down;

    private bool up, down, left, right;

    public LayerMask interactableLayer;
    public UIManager uiManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;
    }

    void Update()
    {
        // -------------------------
        // 0) ESC 입력은 항상 처리
        // -------------------------
        if (Input.GetKeyDown(KeyCode.Escape) && uiManager != null)
        {
            uiManager.ToggleEscMenu();
        }

        // ESC 메뉴가 열려있으면 이동·상호작용 막기 (ESC만 예외)
        if (uiManager != null && uiManager.IsEscOpen())
            return;

        // -------------------------
        // 1) 인벤토리는 이동 가능해야 하므로 막지 않음
        // -------------------------

        // 이동 입력
        up = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        down = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        left = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        right = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) moveDir = Vector2.up;
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) moveDir = Vector2.down;
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) moveDir = Vector2.left;
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) moveDir = Vector2.right;

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow) ||
            Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow) ||
            Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow) ||
            Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            if (up) moveDir = Vector2.up;
            else if (down) moveDir = Vector2.down;
            else if (left) moveDir = Vector2.left;
            else if (right) moveDir = Vector2.right;
            else moveDir = Vector2.zero;
        }

        if (moveDir != Vector2.zero)
            lastMoveDir = moveDir;

        anim.SetInteger("hAxisRaw", (int)moveDir.x);
        anim.SetInteger("vAxisRaw", (int)moveDir.y);

        // F 키 상호작용
        if (Input.GetKeyDown(KeyCode.F))
            Interact();

        // 인벤토리 (Tab)
        if (Input.GetKeyDown(KeyCode.Tab) && uiManager != null)
            uiManager.ToggleInventory();
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveDir * speed * Time.fixedDeltaTime);
    }

    void Interact()
    {
        Vector2 rayDir = moveDir != Vector2.zero ? moveDir : lastMoveDir;
        Vector2 rayStart = rb.position + rayDir * 0.1f;

        Debug.DrawRay(rayStart, rayDir * interactionRange, Color.green, 1f);

        RaycastHit2D hit = Physics2D.Raycast(rayStart, rayDir, interactionRange, interactableLayer);

        if (hit.collider != null)
        {
            Debug.Log("Raycast에 맞음: " + hit.collider.name);
        }
        else
        {
            Debug.Log("Raycast에 아무것도 맞지 않음");
        }
    }
}
