using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

using PathCreation; 

public class TrackGeneratorAgent : Agent
{
    public int Runs = 0;
    public int GoodRuns = 0;
    public int BadRuns = 0;

    public IntersectionDetection intersectDet;
    private PathCreator pathCreator;
    public GameObject BezierObject;

    public Vector3[] points;
    int randomLength;
    int currentLength;
    Vector3 startPos;
    Vector3 prevPos;
    int intersections;

    void Start(){
        pathCreator = BezierObject.GetComponent<PathCreator>();
        startPos = BezierObject.transform.position;
    }

    public override void OnEpisodeBegin(){
        Runs+=1;
        //clear points
        intersections = 0;
        currentLength = 0;
        randomLength = Random.Range(3, 20);
        points = new Vector3[randomLength]; 

        prevPos = new Vector3(0, 0, 0);
        Vector3 point1=prevPos;
        points[currentLength].Set(point1.x, point1.y, point1.z);
    }

    public override void CollectObservations(VectorSensor sensor){
        //what data does the AI need to solve the problem we are giving?
        sensor.AddObservation(prevPos);  //3 floats 
        sensor.AddObservation(currentLength); 
        sensor.AddObservation(randomLength); 
        sensor.AddObservation(intersections); 
    }

    public override void OnActionReceived(ActionBuffers actions){ 

        //Generate a new point
        if(currentLength<randomLength-1){
            currentLength += 1;
            float randomPosX = actions.ContinuousActions[0]*30f-15f;
            float randomPosZ = actions.ContinuousActions[1]*30f-15f;
            float randomPosY = 0; //(int)(actions.ContinuousActions[2]*2)*2

            Vector3 point1 = new Vector3(randomPosX+prevPos.x, randomPosY, randomPosZ+prevPos.z);
            prevPos = new Vector3(point1.x, point1.y, point1.z);
            points[currentLength].Set(point1.x, point1.y, point1.z);

            //Draw the path
            BezierPath bezierPath = new BezierPath (points, true, PathSpace.xyz);
            BezierObject.GetComponent<PathCreator> ().bezierPath = bezierPath;

            //It promoves the idea of having 10 control points or more is desirable
            if(currentLength<10)
                AddReward(0.1f);

            intersections = intersectDet.PathIntersections(points);
        }

        if(currentLength==randomLength-1){
            //check  intersections
            intersections = intersectDet.PathIntersections(points);
            if(intersections==0){
                GoodRuns+=1;
                AddReward(1f);   
                EndEpisode();
            }
            else{
                BadRuns+=1;
                SetReward(-1f);
                EndEpisode();
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

    void Update(){
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SetReward(1f);   
            EndEpisode();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            SetReward(-1f);
            EndEpisode();
        }
    }


}

//venv\Scripts\activate

//mlagents-learn --run-id=TrackGenerator_manual00
//mlagents-learn --initialize-from=TrackGenerator_manual00 --run-id=TrackGenerator_manual01

//tensorboard --logdir results

//mlagents-learn --force --run-id=TrackGenerator_manual00
//mlagents-learn --force --run-id=TrackGenerator_auto00