using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Parkour System/NewParkourActions")]
public class ParkourActions : ScriptableObject
{
    [SerializeField] string animName;
    [SerializeField] string obstacleTag;
    [SerializeField] float minHeight;
    [SerializeField] float maxHeight;
    [SerializeField] bool rotateToObstacle = true;
    [SerializeField] bool targetMatching = true;
    [SerializeField] AvatarTarget matchBodyPart;
    [SerializeField] float matchStartTime;
    [SerializeField] float matchTargetTime;
    [SerializeField] int priority;
    [SerializeField] Vector3 matchPosWeight = new Vector3(0,1,0);
    public Quaternion targetRotation{get; set;}
    public Vector3 MatchPos{get; set;}
    public bool CheckIfPossible(ObstacleHitData hitData, Transform player)
    {
        if(obstacleTag != "" && !hitData.forwardHit.collider.CompareTag(obstacleTag))
        {
            return false;
        }
        float height = hitData.clearanceHit.point.y - player.position.y;
        if(height < minHeight || height > maxHeight)
        {
            return false;
        }
        Vector3 approachDir = player.forward;
        Vector3 obstacleNormal = hitData.forwardHit.normal;

        float angle = Vector3.Angle(approachDir, -obstacleNormal);

        if (angle > 35f)
            return false;
        if(rotateToObstacle)
        {
            targetRotation = Quaternion.LookRotation(hitData.forwardHit.normal * -1);
        }
        if(targetMatching)
        {
            MatchPos = hitData.clearanceHit.point;
        }
        return true;
    }
    public string AnimName => animName;
    public bool RotateToObstacle => rotateToObstacle;

    public bool TargetMatching => targetMatching;
    public AvatarTarget MatchBodyPart => matchBodyPart;
    public float MatchStartTime => matchStartTime;
    public float MatchTargetTime => matchTargetTime;
    public Vector3 MatchPosWeight => matchPosWeight;
    public string ObstacleTag => obstacleTag;
    public int Priority => priority;
}
