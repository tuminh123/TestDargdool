using System;
using System.Collections;
using UnityEngine;

[DefaultExecutionOrder(-1111)]
public class MobileInputManager : MonoBehaviour
{
    public static MobileInputManager Instance;

    // Events
    public event Action<Vector2> OnTap;
    public event Action<Vector2> OnPress;
    public event Action<Vector2, Vector2> OnDrag;
    public event Action<float> OnPinch;
    public event Action<float> OnRotate;

    public event Action<string> OnObjectSelected;
    public event Action OnObjectDeselected;

    private enum InputState { Idle, PotentialTap, Pressed, Dragging, Pinch, Rotate }
    private InputState currentState = InputState.Idle;

    [SerializeField] private float tapThreshold = 0.3f; // seconds -> tùy chỉnh
    [SerializeField] private float dragThreshold = 5f; // pixels

    private Camera cam;
    private Vector2 startPos;
    private float startTime;

    private Coroutine pressCoroutine;
    private bool pressTriggered = false;
    private bool isDragging = false;

    private float prevDistance;
    private float prevAngle;

    private string currentObjectID;

    private void Awake()
    {
        Instance = this;
        cam = Camera.main;
    }

    private void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouseInput();
#else
        HandleTouchInput();
#endif

        // Reset to idling when nothing pressed
        if (Input.touchCount == 0 && !Input.GetMouseButton(0))
        {
            // keep state Idle
            // (state is set appropriately on EndTouch)
            
        }
    }

    // ---------------------------
    // Touch handling
    // ---------------------------
    private void HandleTouchInput()
    {
        if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);
            switch (t.phase)
            {
                case TouchPhase.Began:
                    BeginInput(t.position);
                    break;
                case TouchPhase.Moved:
                    ContinueMove(t.position);
                    break;
                case TouchPhase.Stationary:
                    // do nothing here, press is handled by coroutine
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    EndInput(t.position);
                    break;
            }
        }
        else if (Input.touchCount == 2)
        {
            // cancel any pending single-touch press/tap when multi-touch begins
            CancelPendingPress();
            HandleMultiTouch(Input.GetTouch(0), Input.GetTouch(1));
        }
    }

    private void HandleMultiTouch(Touch t0, Touch t1)
    {
        if (currentState != InputState.Pinch && currentState != InputState.Rotate)
            currentState = InputState.Pinch;

        float curDistance = Vector2.Distance(t0.position, t1.position);
        Vector2 diff = t1.position - t0.position;
        float curAngle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

        if (t0.phase == TouchPhase.Began || t1.phase == TouchPhase.Began)
        {
            prevDistance = curDistance;
            prevAngle = curAngle;
            return;
        }

        HandlePinch(curDistance);
        HandleRotate(curAngle);

        prevDistance = curDistance;
        prevAngle = curAngle;
    }

    // ---------------------------
    // Mouse handling (editor)
    // ---------------------------
    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            BeginInput(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            ContinueMove(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            EndInput(Input.mousePosition);
        }

        // scroll for pinch/rotate in editor
        HandleMouseScroll();
    }

    private void HandleMouseScroll()
    {
        float scroll = Input.mouseScrollDelta.y;
        if (Mathf.Abs(scroll) <= 0.001f) return;

        if (currentState == InputState.Idle)
        {
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                currentState = InputState.Pinch;
                OnPinch?.Invoke(scroll * 10f);
            }
            else
            {
                currentState = InputState.Rotate;
                OnRotate?.Invoke(-scroll * 10f);
            }
        }
    }

    // ---------------------------
    // Core single-touch lifecycle
    // ---------------------------
    private void BeginInput(Vector2 screenPos)
    {
        startPos = screenPos;
        startTime = Time.unscaledTime;
        currentState = InputState.PotentialTap;
        pressTriggered = false;
        isDragging = false;

        // select object immediately on touch down
        GameObject tappedObj = GetObjectAtPosition(screenPos);
        string id = tappedObj?.GetComponent<ObjectController>()?.GetObjectID;
        if (!string.IsNullOrEmpty(id))
        {
            currentObjectID = id;
            OnObjectSelected?.Invoke(currentObjectID);
        }
        else if (currentObjectID != null)
        {
            currentObjectID = null;
            OnObjectDeselected?.Invoke();
        }

        // start timer to determine long-press (Press)
        pressCoroutine = StartCoroutine(PressTimer(screenPos));
    }

    private void ContinueMove(Vector2 screenPos)
    {
        if (currentState == InputState.PotentialTap || currentState == InputState.Pressed)
        {
            float dist = Vector2.Distance(screenPos, startPos);
            if (dist > dragThreshold)
            {
                // start dragging: cancel pending press/tap
                isDragging = true;
                currentState = InputState.Dragging;
                CancelPendingPress();
                OnDrag?.Invoke(startPos, screenPos);
            }
            else
            {
                // if already dragging, send continuous drag
                if (currentState == InputState.Dragging)
                {
                    OnDrag?.Invoke(startPos, screenPos);
                }
            }
        }
        else if (currentState == InputState.Dragging)
        {
            OnDrag?.Invoke(startPos, screenPos);
        }
    }

    private void EndInput(Vector2 screenPos)
    {
        // if we were dragging, finish drag (no tap/press)
        if (currentState == InputState.Dragging)
        {
            CancelPendingPress();
            currentState = InputState.Idle;
            isDragging = false;
            return;
        }

        // if press already triggered -> do nothing (we consider press handled)
        if (pressTriggered)
        {
            // leave only Press executed
            CancelPendingPress();
            currentState = InputState.Idle;
            return;
        }

        // if press not triggered yet:
        // - if release before threshold => it's a Tap
        // - else, pressCoroutine might still be running but hasn't set pressTriggered yet, but we should avoid race:
        //    ensure pressCoroutine is cancelled and treat as Tap only if held less than threshold
        float held = Time.unscaledTime - startTime;
        CancelPendingPress();

        if (!isDragging && held <= tapThreshold)
        {
            // it's a valid Tap
            OnTap?.Invoke(screenPos);
        }

        currentState = InputState.Idle;
        isDragging = false;
    }

    // ---------------------------
    // Press timer coroutine
    // ---------------------------
    private IEnumerator PressTimer(Vector2 initialPos)
    {
        // wait for tapThreshold; if not cancelled and not dragged -> it's a press
        yield return new WaitForSeconds(tapThreshold);

        // only trigger press if still in potential tap (not dragging and not multi-touch)
        if (currentState == InputState.PotentialTap && !isDragging && Input.touchCount <= 1)
        {
            pressTriggered = true;
            currentState = InputState.Pressed;
            OnPress?.Invoke(initialPos);
        }

        pressCoroutine = null;
    }

    private void CancelPendingPress()
    {
        if (pressCoroutine != null)
        {
            StopCoroutine(pressCoroutine);
            pressCoroutine = null;
        }
        pressTriggered = false;
    }

    // ---------------------------
    // Pinch / Rotate helpers
    // ---------------------------
    private void HandlePinch(float curDistance)
    {
        float deltaDist = curDistance - prevDistance;
        OnPinch?.Invoke(deltaDist * 0.01f);
    }

    private void HandleRotate(float curAngle)
    {
        float deltaAngle = Mathf.DeltaAngle(prevAngle, curAngle);
        if (Mathf.Abs(deltaAngle) > 1.0f)
        {
            currentState = InputState.Rotate;
            OnRotate?.Invoke(deltaAngle);
        }
    }

    // ---------------------------
    // Raycast helper
    // ---------------------------
    private GameObject GetObjectAtPosition(Vector2 screenPos)
    {
        if (cam == null)
        {
            Debug.LogWarning("⚠️ Không tìm thấy MainCamera!");
            return null;
        }

        Vector3 worldPoint = cam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, cam.nearClipPlane + 1f));
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
        return hit.collider ? hit.collider.gameObject : null;
    }
}
