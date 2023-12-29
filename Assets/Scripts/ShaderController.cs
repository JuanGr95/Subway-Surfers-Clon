using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ShaderController : MonoBehaviour
{
    [SerializeField, Range(-1, 1)] private float curveX;
    [SerializeField, Range(-1, 1)] private float curveY;
    [SerializeField] private Material[] materials;
    [SerializeField] private float transitionDuration = 2f;
    [SerializeField] private float fixedDuration = 3f;
    public GameManager gameManager;

    private float timer;
    private bool isTransitioning;
    private bool isCurvePositiveX;
    private bool isCurvePositiveY;
    private float currentCurveX;
    private float currentCurveY;

    private void Awake()
    {
        
    }

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        currentCurveX = 0f;
        currentCurveY = 0f;
        foreach (var m in materials)
        {
            m.SetFloat(Shader.PropertyToID("_Curve_X"), currentCurveX);
            m.SetFloat(Shader.PropertyToID("_Curve_Y"), currentCurveY);
        }

        if (gameManager.CanMove)
        {
            currentCurveX = Random.Range(-1f, 1f);
            currentCurveY = Random.Range(-1f, 1f);
        }
    }
    void Update()
    {
        if (gameManager.CanMove)
        {
            if (!isTransitioning)
            {
                timer += Time.deltaTime;
                if (timer >= fixedDuration)
                {
                    isTransitioning = true;
                    timer = 0f;
                    float targetCurveX = Random.Range(-1f, 1f);
                    float targetCurveY = Random.Range(-1f, 1f);
                    isCurvePositiveX = targetCurveX >= currentCurveX;
                    isCurvePositiveY = targetCurveY >= currentCurveY;
                    StartCoroutine(TransitionCurve(targetCurveX, targetCurveY));
                }
            }
        }
        foreach (var m in materials)
        {
            m.SetFloat(Shader.PropertyToID("_Curve_X"), currentCurveX);
            m.SetFloat(Shader.PropertyToID("_Curve_Y"), currentCurveY);
        }
    }

    private System.Collections.IEnumerator TransitionCurve(float targetCurveX, float targetCurveY)
    {
        float elapsedTime = 0f;
        float startCurveX = currentCurveX;
        float startCurveY = currentCurveY;
        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionDuration;
            currentCurveX = Mathf.Lerp(startCurveX, targetCurveX, t);
            currentCurveY = Mathf.Lerp(startCurveY, targetCurveY, t);

            yield return null;
        }
        currentCurveX = targetCurveX;
        currentCurveY = targetCurveY;
        isTransitioning = false;
    }
}

