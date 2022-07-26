using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

using PathCreation; 

public class TrackGenerator : Agent
{
    [SerializeField] private Vector3[] _points;
    private PathCreator _pathCreator;
    
    void Start(){
        _pathCreator = gameObject.AddComponent<PathCreator>();
    }

    public override void OnEpisodeBegin(){
        //_points = GenerateVector2();
        //BezierPath bezierPath = new BezierPath (_points, true, PathSpace.xz);
        //GetComponent<pathCreator> ().bezierPath = bezierPath;
        
        _points = GenerateVector3();
        BezierPath bezierPath = new BezierPath (_points, true, PathSpace.xyz);
        GetComponent<PathCreator> ().bezierPath = bezierPath;
        //clear _points
    }

    public override void CollectObservations(VectorSensor sensor){
        //what data does the AI need to solve the problem we are giving?
        
        //player and target position . a total of 6 floats
        //sensor.AddObservation(transform.localPosition); 
        //sensor.AddObservation(_points[i].x); 
        
        sensor.AddObservation(_points.Length); 
        for(int i =0;i<_points.Length;i++){
            sensor.AddObservation(_points[i].x); 
            sensor.AddObservation(_points[i].y); 
        }
    }

    public override void OnActionReceived(ActionBuffers actions){ //floats or ints 
        //Debug.Log(actions.ContinuousActions[0]);

        //float moveX = actions.ContinuousActions[0];
        //float moveZ = actions.ContinuousActions[1];
        //float moveSpeed = 5.0f;
        //transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;


    }

    public override void Heuristic(in ActionBuffers actionsOut){
        // Modify the actions that will be received by the onActionsReceived
        //ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        //continuousActions[0] = Input.GetAxisRaw("Horizontal");
        //continuousActions[1] = Input.GetAxisRaw("Vertical");
    }


    Vector2[] GenerateVector2()
    {
        int randomLength = Random.Range(2, 20);
        Vector2[] G_points = new Vector2[randomLength]; 
        Vector2 prevPos = new Vector2(0, 0);

        for (int i=1;i<randomLength;i++){
            float randomPosX = Random.Range(-10, 10);
            float randomPosY = Random.Range(-10, 10);
            //Vector2 point1 = Gpoint;
            Vector2 point1 = new Vector2(randomPosX, randomPosY) + prevPos;
            prevPos = new Vector2(point1.x, point1.y);
            G_points[i].Set(point1.x, point1.y);
        }
        return G_points;
    } 
    Vector3[] GenerateVector3()
    {
        int randomLength = Random.Range(2, 20);
        Vector3[] G_points = new Vector3[randomLength]; 
        Vector3 prevPos = new Vector3(0, 0, 0);

        for (int i=1;i<randomLength;i++){
            float randomPosX = Random.Range(-10, 10);
            float randomPosY = Random.Range(0,4);
            float randomPosZ = Random.Range(-10, 10);
            //Vector2 point1 = Gpoint;
            Vector3 point1 = new Vector3(randomPosX+prevPos.x, randomPosY, randomPosZ+prevPos.z);
            prevPos = new Vector3(point1.x, point1.y, point1.z);
            G_points[i].Set(point1.x, point1.y, point1.z);
        }
        return G_points;
    } 
} 
