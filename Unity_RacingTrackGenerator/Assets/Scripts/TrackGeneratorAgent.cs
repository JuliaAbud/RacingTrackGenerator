using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using PathCreation;
using UnityEngine.Events;

public class TrackGeneratorAgent : Agent
{
    [SerializeField] private GameObject BezierObject;
    private PathCreator pathCreator;

    [Tooltip("If using an already trained model, should be False")]
    [SerializeField] private bool _automaticCheck = true;
    [SerializeField] private IntersectionDetection _intersectDetection;
    [SerializeField] private UnityEvent StartRun;
    [SerializeField] private UnityEvent ApprovedPath;
    [SerializeField] private UnityEvent RejectedPath;
    [SerializeField] private UnityEvent<Vector3[]> PointsChange;

    private Vector3[] _points;
    private int _randomLength;
    private int _currentLength;
    private Vector3 startPos;
    private Vector3 _prevPos;
    private int intersections;

    //========================================================================================

    void Start(){
        pathCreator = BezierObject.GetComponent<PathCreator>();
        startPos = BezierObject.transform.position;
    }

    public void PathApproved()
    {
        ApprovedPath?.Invoke();
        SetReward(1);
        EndEpisode();
    }
    public void PathRejected()
    {
        RejectedPath?.Invoke();
        SetReward(-1);
        EndEpisode();
    }

    public override void OnEpisodeBegin(){
        StartRun?.Invoke();
        //clear points
        intersections = 0;
        _currentLength = 0;
        _randomLength = Random.Range(3, 20);
        _points = new Vector3[_randomLength]; 
        _prevPos = new Vector3(0, 0, 0);
        Vector3 point1 = _prevPos;
        _points[_currentLength].Set(point1.x, point1.y, point1.z);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //what data does the AI need to solve the problem we are giving?
        sensor.AddObservation(_prevPos);  //3 floats 
        sensor.AddObservation(_currentLength); 
        sensor.AddObservation(_randomLength); 
        sensor.AddObservation(intersections); 
        //Total of: 6 floats to keep on Observation.
        //In "BehaviourParameters" select a space size vector observation of 6.
    }

    public override void OnActionReceived(ActionBuffers actions)
    { 
        //Generate a new point
        if(_currentLength<_randomLength-1){
            _currentLength += 1;
            float randomPosX = actions.ContinuousActions[0]*30f-15f;
            float randomPosZ = actions.ContinuousActions[1]*30f-15f;
            float randomPosY = 0;

            Vector3 point1 = new Vector3(randomPosX+_prevPos.x, randomPosY, randomPosZ+_prevPos.z);
            _prevPos = new Vector3(point1.x, point1.y, point1.z);
            _points[_currentLength].Set(point1.x, point1.y, point1.z);

            //Draw the path
            BezierPath bezierPath = new BezierPath (_points, true, PathSpace.xyz);
            BezierObject.GetComponent<PathCreator> ().bezierPath = bezierPath;

            //It promoves the idea of having 10 control points or more is desirable
            if(_currentLength < 10)
                AddReward(0.1f);

            intersections = _intersectDetection.PathIntersections(_points);
            PointsChange?.Invoke(_points);
        }

        //check intersections if out Path achieved its final size
        if (_currentLength == _randomLength-1 && _automaticCheck){
            intersections = _intersectDetection.PathIntersections(_points);
            if(intersections == 0){
                PathApproved();
            }
            else{
                PathRejected();
            }
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut){
        // Modify the actions that will be received by the onActionsReceived
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Random.Range(0f, 1f);
        continuousActions[1] = Random.Range(0f, 1f);
        continuousActions[2] = Random.Range(0f, 1f);
    }
}