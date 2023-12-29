using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum Side { Left = -2, Middle = 0, Right = 2 };    
public class PlayerController : MonoBehaviour
{
    private Transform _myTransform;
    private Animator myAnimator;
    private CharacterController _myCharacterController;
    private PlayerCollision playerCollision;
    public GameManager gameManager;
    public CharacterController MyCharacterController { get => _myCharacterController; set => _myCharacterController = value; }
    public int IdStumbleLow { get => _IdStumbleLow; set => _IdStumbleLow = value; }
    public int IdDeathLower { get => _IdDeathLower; set => _IdDeathLower = value; }
    public int IdDeathBounce { get => _IdDeathBounce; set => _IdDeathBounce = value; }
    public int IdDeathMovingTrain { get => _IdDeathMovingTrain; set => _IdDeathMovingTrain = value; }
    public bool IsRolling { get => _isRolling; set => _isRolling = value; }
    public int IdDeathUpper { get => _IdDeathUpper; set => _IdDeathUpper = value; }
    public int IdStumbleCornerRight { get => _IdStumbleCornerRight; set => _IdStumbleCornerRight = value; }
    public int IdStumbleCornerLeft { get => _IdStumbleCornerLeft; set => _IdStumbleCornerLeft = value; }
    public int IdStumbleFall { get => _IdStumbleFall; set => _IdStumbleFall = value; }
    public int IdStumbleOffLeft { get => _IdStumbleOffLeft; set => _IdStumbleOffLeft = value; }
    public int IdStumbleOffRight { get => _IdStumbleOffRight; set => _IdStumbleOffRight = value; }
    public int IdStumbleSideLeft { get => _IdStumbleSideLeft; set => _IdStumbleSideLeft = value; }
    public int IdStumbleSideRight { get => _IdStumbleSideRight; set => _IdStumbleSideRight = value; }
    public Transform MyTransform { get => _myTransform; set => _myTransform = value; }
    public Side PreviousXPos { get => _previousXPos; set => _previousXPos = value; }
    public bool IsStumbleTransitionComplete { get => _isStumbleTransitionComplete; set => _isStumbleTransitionComplete = value; }

    private Side position;
    private Side _previousXPos;
    private Vector3 motionVector;
    [Header("Player Controller")]
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float dodgeSpeed;
    private float rollTimer;
    private float newXPosition;
    private float xPosition, yPosition;
    private int IdDodgeLeft = Animator.StringToHash("DodgeLeft");
    private int IdDodgeRight = Animator.StringToHash("DodgeRight");
    private int IdJump = Animator.StringToHash("Jump");
    private int IdFall = Animator.StringToHash("Fall");
    private int IdLanding = Animator.StringToHash("Landing");
    private int IdRoll = Animator.StringToHash("Roll");
    private int _IdStumbleLow = Animator.StringToHash("StumbleLow"); 
    private int _IdStumbleCornerRight = Animator.StringToHash("StumbleCornerRight"); 
    private int _IdStumbleCornerLeft = Animator.StringToHash("StumbleCornerLeft"); 
    private int _IdStumbleFall = Animator.StringToHash("StumbleFall"); 
    private int _IdStumbleOffLeft = Animator.StringToHash("StumbleOffLeft"); 
    private int _IdStumbleOffRight = Animator.StringToHash("StumbleOffRight"); 
    private int _IdStumbleSideLeft = Animator.StringToHash("StumbleSideLeft"); 
    private int _IdStumbleSideRight = Animator.StringToHash("StumbleSideRight"); 
    private int _IdDeathBounce = Animator.StringToHash("DeathBounce"); 
    private int _IdDeathLower = Animator.StringToHash("DeathLower"); 
    private int _IdDeathMovingTrain = Animator.StringToHash("DeathMovingTrain"); 
    private int _IdDeathUpper = Animator.StringToHash("DeathUpper");
    private bool swipeLeft, swipeRight, swipeUp, swipeDown;
    [Header("Player Controller")]
    [SerializeField] private bool _isRolling;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool _isStumbleTransitionComplete = false;

    // Start is called before the first frame update
    void Start()
    {
        position= Side.Middle;
        _myTransform = GetComponent<Transform>();
        myAnimator = GetComponent<Animator>();
        _myCharacterController = GetComponent<CharacterController>();
        playerCollision = GetComponent<PlayerCollision>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        yPosition = -7;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.CanMove)
            return;

        if (gameManager.IsInputDisabled)
            return;

        GetSwipe();
        SetPlayerPosition();
        MovePlayer();
        Jump();
        Roll();
        isGrounded = _myCharacterController.isGrounded;
        SetStumblePosition();
    }
    private void SetStumblePosition()
    {
        if ((myAnimator.GetCurrentAnimatorStateInfo(0).IsName("StumbleSideRight") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("StumbleSideLeft")) && _isStumbleTransitionComplete)
        {
            UpdatePlayerXPosition(_previousXPos);
            _isStumbleTransitionComplete = false;
        }
    }
    private void Roll()
    {
        rollTimer -= Time.deltaTime;

        if (rollTimer <= 0)
        {
            _isRolling= false;
            rollTimer = 0;
            _myCharacterController.center = new Vector3(0, 0.45f, 0);
            _myCharacterController.height = 0.9f;
        }
        if (swipeDown && !isJumping)
        {
            _isRolling = true;
            rollTimer = 1f;
            SetPlayerAnimator(IdRoll, true);
            _myCharacterController.center = new Vector3(0, 0.2f, 0);
            _myCharacterController.height = 0.4f;
        }
    }
    private void GetSwipe()
    {
        if (isGrounded)
        {
            swipeLeft = Input.GetKeyDown(KeyCode.LeftArrow);
            swipeRight = Input.GetKeyDown(KeyCode.RightArrow);
            swipeDown = Input.GetKeyDown(KeyCode.DownArrow);
            swipeUp = Input.GetKeyDown(KeyCode.UpArrow);
        }
    }
    private void SetPlayerPosition()
    {

        if (swipeLeft && !_isRolling)
        {
            if (position == Side.Middle)
            {
                _previousXPos = Side.Middle;
                UpdatePlayerXPosition(Side.Left);
                SetPlayerAnimator(IdDodgeLeft,false);
            }
            else if (position == Side.Right)
            {
                _previousXPos = Side.Right;
                UpdatePlayerXPosition(Side.Middle);
                SetPlayerAnimator(IdDodgeLeft, false);
            }
        }
        else if (swipeRight && !_isRolling)
        {
            if (position == Side.Middle)
            {
                _previousXPos = Side.Middle;
                UpdatePlayerXPosition(Side.Right);
                SetPlayerAnimator(IdDodgeRight, false);
            }
            else if (position == Side.Left)
            {
                _previousXPos = Side.Left;
                UpdatePlayerXPosition(Side.Middle);
                SetPlayerAnimator(IdDodgeRight, false);
            }
        }
    }
    private void UpdatePlayerXPosition(Side plPosition)
    {
        newXPosition = (int)plPosition;
        position = plPosition;
    }
    public void SetPlayerAnimatorWithLayer(int id)
    {
        myAnimator.SetLayerWeight(1,1);
        myAnimator.Play(id);
        ResetCollision();
    }
    public void SetPlayerAnimator(int id,bool isCrossFade, float fadeTime = 0.1f)
    {
        myAnimator.SetLayerWeight(0, 1);
        if (isCrossFade)
        myAnimator.Play(id);
        else
        {
            myAnimator.Play(id);
        }
        ResetCollision();
    }
    private void ResetCollision()
    {
        Debug.Log(playerCollision.CollisionX.ToString() + " " + playerCollision.CollisionY.ToString() + " " + playerCollision.CollisionZ.ToString());
        playerCollision.CollisionX = CollisionX.None;
        playerCollision.CollisionY = CollisionY.None;
        playerCollision.CollisionZ = CollisionZ.None;
    }
    private void MovePlayer()
    {
        motionVector = new Vector3(xPosition - _myTransform.position.x, yPosition * Time.deltaTime, forwardSpeed * Time.deltaTime);
        xPosition = Mathf.Lerp(xPosition, newXPosition, Time.deltaTime * dodgeSpeed);

        _myCharacterController.Move(motionVector);
    }
    private void Jump()
    {
        if (_myCharacterController.isGrounded)
        {
            isJumping = false;

            if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Fall"))
            {
                SetPlayerAnimator(IdLanding, false);
            }
            if (swipeUp && !_isRolling)
            {
                isJumping = true;
                yPosition = jumpPower;
                SetPlayerAnimator(IdJump, true, 1f);
            }
        }
        else
        {
            yPosition -= jumpPower * 2 * Time.deltaTime;
            if (_myCharacterController.velocity.y <= 0)
            {
                SetPlayerAnimator(IdFall, false);
            }
        }
    }

}
