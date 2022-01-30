using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField]
    private bool m_followXaxis = true;
    [SerializeField]
    private bool m_followYaxis = false;
    [SerializeField]
    private bool m_followZaxis = false;

    [SerializeField]
    private Transform m_targetTransform = null;

    private Transform m_cachedTransform = null;

    // Start is called before the first frame update
    void Start()
    {
        m_cachedTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosisiton = m_cachedTransform.position;
        Vector3 targetPosition = m_targetTransform.position;

        if (m_followXaxis)
        {
            newPosisiton.x = targetPosition.x;
        }
        if (m_followYaxis)
        {
            newPosisiton.y = targetPosition.y;
        }
        if (m_followZaxis)
        {
            newPosisiton.z = targetPosition.z;
        }

        m_cachedTransform.position = newPosisiton;
    }
}
