using UnityEngine;

public class Player_Joystick_Controller : MonoBehaviour
{

    private GameManager _gameManager;

    [SerializeField]
    private Joystick _joystick;

    [SerializeField]
    private CharacterController _characterController;

    [SerializeField]
    private float _turnSpeed;

    private float InputX;
    private float InputZ;

    #region Gravity
    private float _vSpeed = 0;
    private Vector3 _movement;
    #endregion

    void Start()
    {
        _gameManager = GameManager.instance;
        _characterController = gameObject.GetComponent<CharacterController>();
    }

    void Update()
    {
        InputX = _joystick.Horizontal;
        InputZ = _joystick.Vertical;

    }

    private void FixedUpdate()
    {
        if (GameManager.isGameEnded || !GameManager.isGameStarted)
        {
            return;
        }

        JoystickController();

        #region Gravity Jobs

        GravityComp();

        #endregion

    }

    private void GravityComp()
    {
        _vSpeed -= _gameManager.Gravity * Time.deltaTime;
        _movement.y = _vSpeed;
        _characterController.Move(_movement * Time.deltaTime);
    }

    private void JoystickController()
    {
        _movement = new Vector3(InputX, 0, InputZ);
        _characterController.Move(_movement * _gameManager.PlayerSpeed * Time.deltaTime);

        if (_movement != Vector3.zero)
        {
            Quaternion _newDirect = Quaternion.LookRotation(_movement);
            this.transform.rotation = _newDirect;
        }

    }
}
