using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;

    [SerializeField]
    private float spawnHeight;

    [SerializeField]
    private Vector2 vec2Move;

    [SerializeField]
    private Vector3 vec3Move;

    private CharacterController characterController;

    private WorldGenerator worldGenerator;

    private GameObject mainCam;

    private float camAngle;

    private Vector2 inputVec;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        spawnHeight = transform.position.y;
        worldGenerator = GameObject.FindGameObjectWithTag("GameController").GetComponent<WorldGenerator>();
        mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        GetCameraAngle();
    }

    private void Update()
    {
        MoveChar();
    }

    //Changes y input to z movement
    private Vector3 ConstructMoveVec(Vector2 moveInput)
    {
        return new Vector3(vec2Move.x, 0, vec2Move.y);
    }

    private void GetCameraAngle()
    {
        Vector3 realAngle = (worldGenerator.origin - mainCam.transform.position).normalized;
        camAngle = Vector2.SignedAngle(new Vector2(realAngle.x, realAngle.z), new Vector2(0, 1));
    }

    private void OnMove(InputValue inputVal)
    {
        inputVec = inputVal.Get<Vector2>();
        vec2Move = Quaternion.AngleAxis(camAngle, -Vector3.forward) * inputVec.normalized * moveSpeed;
    }

    private void MoveChar()
    {
        vec3Move = ConstructMoveVec(vec2Move) * Time.deltaTime;

        characterController.Move(vec3Move);
    }
}