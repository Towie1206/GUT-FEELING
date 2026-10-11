using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerInput input { get; private set; }
    public StateMachine stateMachine { get; private set; }
    public PlayerFreeState freeState { get; private set; }
    public PlayerInspectState inspectState { get; private set; }

    public PlayerMovement movement { get; private set; }
    public PlayerLook look { get; private set; }
    public PlayerInspect inspect { get; private set; }

    public Vector2 moveInput { get; private set; }
    public Vector2 mousePosition { get; private set; }

    public PlayerInteraction interaction { get; private set; }
    public PlayerHand hand { get; private set; }


    private void Awake()
    {
        input = new PlayerInput();
        stateMachine = new StateMachine();
        freeState = new PlayerFreeState(this, stateMachine);
        inspectState = new PlayerInspectState(this, stateMachine);

        stateMachine.Initialize(freeState);
        movement = GetComponent<PlayerMovement>();
        look = GetComponent<PlayerLook>();
        interaction = GetComponent<PlayerInteraction>();
        hand = GetComponent<PlayerHand>();
        inspect = GetComponent<PlayerInspect>();
    }
    private void OnEnable()
    {
        input.Enable();

        input.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        input.Player.Look.performed += ctx => mousePosition = ctx.ReadValue<Vector2>();
        input.Player.Look.canceled += ctx => mousePosition = Vector2.zero;
    }
    private void OnDisable()
    {
        input.Disable();
    }

    private void Update()
    {
        stateMachine.currentState.Update();
    }

}
