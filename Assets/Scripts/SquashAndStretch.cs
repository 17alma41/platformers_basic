using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SquashAndStretch : MonoBehaviour
{
    [Header("Squash and Stretch Core")]
    [SerializeField, Tooltip("Defaults to current GO if not set.")] private Transform transformToAffect;
    [SerializeField] private SquashStretchAxis axisToAffect = SquashStretchAxis.Y;
    [SerializeField, Range(0, 1f)] private float animationDuration = 0.25f;
    [SerializeField] bool canBeOverwritten;
    [SerializeField] bool playOnStart;
    [SerializeField] bool playsEveryTime = true;
    [SerializeField, Range(0, 100f)] private float chanceToPlay = 100f;



    [Flags]
    public enum SquashStretchAxis
    {
        None = 0,
        X = 1,
        Y = 2,
        Z = 4
    }

    [Header("Animation Settings")]
    [SerializeField] float initialScale = 1f;
    [SerializeField] float maximumScale = 1.3f;
    [SerializeField] bool resetToInitialScaleAfterAnimation = true;
    [SerializeField] bool reverseAnimationCurveAfterPlaying;
    bool _isReserved;

    [SerializeField]
    AnimationCurve squashAndStretchCurve;
    

    [Header("Looping Settings")]
    [SerializeField] bool looping;
    [SerializeField] float loopingDelay = 0.5f;


    private Coroutine _squashAndStretchCoroutine;
    private WaitForSeconds _loopingDelayWaitForSeconds;
    private Vector3 _initialScaleVector;


    bool affectX => (axisToAffect & SquashStretchAxis.X) != 0;
    bool affectY => (axisToAffect & SquashStretchAxis.Y) != 0;
    bool affectZ => (axisToAffect & SquashStretchAxis.Z) != 0;


    void Awake()
    {
        if (transformToAffect == null)
            transformToAffect = transform;

        _initialScaleVector = transformToAffect.localScale;
        _loopingDelayWaitForSeconds = new WaitForSeconds(loopingDelay);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (playOnStart)
            CheckForAndStartCoroutine();
    }

    /*
   [PropertySpace(25), Button]
   [ContextMenu(itemName:"Play Squash and Stretch")]
    */

    //hay que poner el playSquashAndStretch en el player
    public void PlaySquashAndStretch()
    {
        if (looping && !canBeOverwritten)
            return;

        CheckForAndStartCoroutine();
    }

    void CheckForAndStartCoroutine()
    {
        if (axisToAffect == SquashStretchAxis.None)
        {
            Debug.Log(message: "Axis to affect is set to None.", gameObject);
            return;
        }

        if (_squashAndStretchCoroutine != null)
        {
            StopCoroutine(_squashAndStretchCoroutine);
            if (playsEveryTime && resetToInitialScaleAfterAnimation)
                transform.localScale = _initialScaleVector;
        }

        _squashAndStretchCoroutine = StartCoroutine(routine:SquashAndStrechEffect());
    }

    private IEnumerator SquashAndStrechEffect()
    {
        do
        {
            if (!playsEveryTime)
            {
                float random = UnityEngine.Random.Range(0, 100f);
                if (random > chanceToPlay)
                {
                    yield return null;
                    continue;
                }
            }


            if (reverseAnimationCurveAfterPlaying)
                _isReserved = !_isReserved;

            float elapsedTime = 0;
            Vector3 originalScale = _initialScaleVector;
            Vector3 modifiedScale = originalScale;

            while (elapsedTime < animationDuration)
            {
                elapsedTime += Time.deltaTime;

                float curvePosition;

                if (_isReserved)
                    curvePosition = 1 - (elapsedTime / animationDuration);
                else
                    curvePosition = elapsedTime / animationDuration;

                float curveValue = squashAndStretchCurve.Evaluate(curvePosition);
                float remappedValue = initialScale + (curveValue * (maximumScale - initialScale));

                float minimumThreshold = 0.0001f;
                if (Mathf.Abs(remappedValue) < minimumThreshold)
                    remappedValue = minimumThreshold;


                if (affectX)
                    modifiedScale.x = originalScale.x * remappedValue;
                else
                    modifiedScale.x = originalScale.x / remappedValue;

                if (affectY)
                    modifiedScale.y = originalScale.y * remappedValue;
                else
                    modifiedScale.y = originalScale.y / remappedValue;

                if (affectZ)
                    modifiedScale.z = originalScale.z * remappedValue;
                else
                    modifiedScale.z = originalScale.z / remappedValue;

                transformToAffect.localScale = modifiedScale;

                yield return new WaitForEndOfFrame();
            }

            if (resetToInitialScaleAfterAnimation)
                transformToAffect.localScale = originalScale;

            if (looping)
            {
                yield return _loopingDelayWaitForSeconds;
            }

        } while (looping);
    }

    public void SetLooping(bool shouldLoop)
    {
        looping = shouldLoop;
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}



