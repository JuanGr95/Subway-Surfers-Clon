using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    [SerializeField] PlayerCollision playerCollision;
    [SerializeField] PlayerController playerController;
    private Transform playerTransform;
    private CapsuleCollider playerCollider;
    public GameManager gameManager;
    private Vector3 playerInitialPosition;
    private bool obstacleActivated = false;
    Collider[] initialObstacles;
    [SerializeField]
    private LayerMask obstacleLayer;

    void Awake()
    {
        playerCollision = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCollision>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerCollider = GetComponent<CapsuleCollider>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerInitialPosition = playerTransform.position;
    }

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        initialObstacles = Physics.OverlapSphere(playerTransform.position + 0.5f * Vector3.up, 0.2f, obstacleLayer);
        foreach (Collider obstacle in initialObstacles)
        {
            obstacle.enabled = false;
        }
    }

    private void Update()
    {
        if (playerController.IsRolling)
        {
            playerCollider.height = 0.1f;
            playerCollider.radius = 0.1f;
        }
        else
        {
            playerCollider.height = 0.92f;
            playerCollider.radius = 0.35f;
        }
        if (!obstacleActivated && playerTransform.position.z > playerInitialPosition.z + 100)
        {
            obstacleActivated = true;
            foreach (Collider obstacle in initialObstacles)
            {
                obstacle.enabled = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) return;

        if (gameManager.CanMove)
            playerCollision.OnCharacterCollision(collision.collider);
    }
}