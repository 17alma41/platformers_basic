using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Squash : MonoBehaviour
{
    [Header("Squash and Stretch Core")]
    [SerializeField, Tooltip("Defaults to current GO if not set.")] private Transform transformToAffect;
    [SerializeField] private SquashStretchAxis axisToAffect = SquashStretchAxis.Y;
    [SerializeField, Range(0, 1f)] private float animationDuration = 0.25f;
    [SerializeField] bool canBeOverwritten;
    [SerializeField] bool playOnStart;




    public enum SquashStretchAxis
    {
        None = 0,
        X = 1,
        Y = 2,
        Z = 4
    }

    [Header("Animation Setiings")]
    [SerializeField] float initialScale = 1f;
    [SerializeField] float maximumScale = 1.3f;
    [SerializeField] bool resetToInitialScaleAfterAnimation = true;

    /*
    [SerializeField]
    AnimationCurve squashAndStretchCurve = new AnimationCurve(
            params keys:new Keyframe(time:0f, value:0f),
            new Keyframe(time:0.25f, value:1f),
            new Keyframe(time:1f, value:0f)
        );
    */

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


    void CheckForAndStartCoroutine()
    {
        if (axisToAffect == SquashStretchAxis.None)
        {
            Debug.Log(message: "Axis to affect is set to None.", gameObject);
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}



