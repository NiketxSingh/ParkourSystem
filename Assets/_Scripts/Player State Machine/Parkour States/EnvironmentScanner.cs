using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentScanner : MonoBehaviour
{
    [SerializeField] Vector3 forwardRayOffset = new Vector3(0, 2.5f, 0);
    [SerializeField] float forwardRayLength = 1.0f;
    [SerializeField] float clearanceCheckHeight = 2.0f;
    [SerializeField] LayerMask obstacleLayer;
    
    public ObstacleHitData ObstacleCheck(float additionalForwardOffset = 0f)
    {
        var hitData =  new ObstacleHitData();
        float UpdatedForwardRayLength = forwardRayLength + additionalForwardOffset;
        // Debug.Log("Checking for obstacle with forward ray length: " + UpdatedForwardRayLength);
        Vector3 origin = transform.position + forwardRayOffset;
        hitData.forwardHitFound = Physics.Raycast(origin, transform.forward, out hitData.forwardHit,
         UpdatedForwardRayLength, obstacleLayer);
        Debug.DrawRay(origin, transform.forward * UpdatedForwardRayLength, hitData.forwardHitFound ? Color.red : Color.green);
        
        if(hitData.forwardHitFound)
        {
            var heightOrigin = hitData.forwardHit.point + Vector3.up * clearanceCheckHeight;
            hitData.clearanceHitFound = Physics.Raycast(heightOrigin, Vector3.down, 
            out hitData.clearanceHit, clearanceCheckHeight, obstacleLayer);

            Debug.DrawRay(heightOrigin, Vector3.down * clearanceCheckHeight,
                hitData.clearanceHitFound ? Color.red : Color.green);
        }

        return hitData;


    }
    public bool GetVaultHandTargets(out Vector3 leftHand, out Vector3 rightHand)
    {
        leftHand = Vector3.zero;
        rightHand = Vector3.zero;

        Vector3 origin = transform.position + Vector3.up * 1.2f;
        Vector3 dir = transform.forward;

        if (!Physics.Raycast(origin, dir, out RaycastHit hit, 1.2f, obstacleLayer))
            return false;

        Vector3 ledgePoint = hit.point;

        Vector3 rightOffset = transform.right * 0.25f;
        Vector3 leftOffset  = -transform.right * 0.25f;

        rightHand = ledgePoint + rightOffset + Vector3.up * 0.05f;
        leftHand  = ledgePoint + leftOffset  + Vector3.up * 0.05f;

        return true;
    }
}

public struct ObstacleHitData
{
    public bool forwardHitFound;
    public bool clearanceHitFound;
    public RaycastHit forwardHit;
    public RaycastHit clearanceHit;
}