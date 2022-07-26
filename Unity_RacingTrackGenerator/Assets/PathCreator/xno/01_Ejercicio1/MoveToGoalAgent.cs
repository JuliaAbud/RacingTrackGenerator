using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

//Add this and Behaviour paramenters (automatic)
public class MoveToGoalAgent : Agent
{
    [SerializeField]    private Transform targetTransform;    
    public MeshRenderer floorMeshRenderer;
    public Material winMaterial;
    public Material loseMaterial;

    public override void OnEpisodeBegin(){
        transform.localPosition = new Vector3(Random.Range(-3f,0.6f),0,Random.Range(-2f,2f));
        targetTransform.localPosition = new Vector3(Random.Range(1.9f,3f),0,Random.Range(-2f,2f));
    }
    public override void CollectObservations(VectorSensor sensor){
        //what data does the AI need to solve the problem we are giving?
        
        //player and target position . a total of 6 floats
        sensor.AddObservation(transform.localPosition); 
        sensor.AddObservation(targetTransform.localPosition);
    }
    public override void OnActionReceived(ActionBuffers actions){
        //Debug.Log(actions.ContinuousActions[0]);

        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];
        float moveSpeed = 5.0f;
        transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
    }
    public override void Heuristic(in ActionBuffers actionsOut){
        // Modify the actions that will be received by the onActionsReceived
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter(Collider other){
        //AddReward()
        if(other.TryGetComponent<Goal>(out Goal goal)){
            SetReward(1f);
            floorMeshRenderer.material = winMaterial;
            EndEpisode();
        }
        if(other.TryGetComponent<Wall>(out Wall wall)){
            SetReward(-1f);
            floorMeshRenderer.material = loseMaterial;
            EndEpisode();
        }
    }
}
//venv\Scripts\activate

//mlagents-learn
//mlagents-learn --force
//mlagents-learn --run-id Test2
//mlagents-learn --force --run-id MoveToGoal

//training yaml file with parameters
//mlagents-learn config/moveToGoal.yaml --run-id=TestParameters
//mlagents-learn config/moveToGoal.yaml --initialize-from=MoveToGoal --run-id=MoveToGoal2


//mlagents-learn config/moveToGoal_config.yaml --initialize-from=MoveToGoal --run-id=MoveToGoal2
//mlagents-learn --initialize-from=MoveToGoal --run-id=MoveToGoal2

//tensorboard --logdir results

/*
ML AGENTS RELEASE 18

cd go to directory
python -m venv venv   (second venv is the name of the folder)
venv\Scripts\activate

install python packages
----------------------
python -m pip install --upgrade pip
pip3 install torch~=1.7.1 -f https://download.pytorch.org/whl/torch_stable.html
python -m pip install mlagents==0.27.0

....
pip install mlagents --use-feature-2020-resolver

mlagents-learn --help
......

cuda?? 
cudnn64_7.dll 

https://github.com/Unity-Technologies/ml-agents/blob/release_18_docs/docs/Installation.md

https://github.com/Unity-Technologies/ml-agents/blob/release_18_docs/docs/Getting-Started.md


MLAgents package... and update
*/