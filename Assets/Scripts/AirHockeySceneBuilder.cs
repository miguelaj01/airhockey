using UnityEngine;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Actuators;

public class AirHockeySceneBuilder : MonoBehaviour
{
    void Start()
    {
        BuildScene();
    }

    void BuildScene()
    {
        CreateFloor();
        CreateWalls();
        CreateGoals();
        CreatePuck();
        CreatePaddles();
        CreateCameraAndLight();

        MakePuckObject(GameObject.Find("Puck"));

        MakePaddleObject(GameObject.Find("Left Striker"));
        MakePaddleObject(GameObject.Find("Right Striker"));

        MakeStaticObject(GameObject.Find("Top Wall"));
        MakeStaticObject(GameObject.Find("Bottom Wall"));
        MakeStaticObject(GameObject.Find("Left Upper Wall"));
        MakeStaticObject(GameObject.Find("Left Lower Wall"));
        MakeStaticObject(GameObject.Find("Right Upper Wall"));
        MakeStaticObject(GameObject.Find("Right Lower Wall"));

        MakeStaticObject(GameObject.Find("Left Defender"));
        MakeStaticObject(GameObject.Find("Right Defender"));
    }

    void CreateFloor()
    {
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        floor.name = "Rink Floor";
        floor.transform.position = Vector3.zero;
        floor.transform.localScale = new Vector3(12f, 0.2f, 6f);

        Renderer r = floor.GetComponent<Renderer>();
        r.material.color = new Color(0.1f, 0.4f, 0.8f);
    }

    void CreateWalls()
    {
        CreateWall("Top Wall", new Vector3(0, 0.6f, 3.2f), new Vector3(12.5f, 1f, 0.3f));
        CreateWall("Bottom Wall", new Vector3(0, 0.6f, -3.2f), new Vector3(12.5f, 1f, 0.3f));

        CreateWall("Left Upper Wall", new Vector3(-6.2f, 0.6f, 2.2f), new Vector3(0.3f, 1f, 2f));
        CreateWall("Left Lower Wall", new Vector3(-6.2f, 0.6f, -2.2f), new Vector3(0.3f, 1f, 2f));

        CreateWall("Right Upper Wall", new Vector3(6.2f, 0.6f, 2.2f), new Vector3(0.3f, 1f, 2f));
        CreateWall("Right Lower Wall", new Vector3(6.2f, 0.6f, -2.2f), new Vector3(0.3f, 1f, 2f));
    }

    void CreateWall(string name, Vector3 pos, Vector3 scale)
    {
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.name = name;
        wall.transform.position = pos;
        wall.transform.localScale = scale;
        wall.GetComponent<Renderer>().material.color = Color.gray;

        Rigidbody rb = wall.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        PhysicMaterial wallBounce = new PhysicMaterial("WallBounce");
        wallBounce.bounciness = 1f;
        wallBounce.dynamicFriction = 0f;
        wallBounce.staticFriction = 0f;
        wallBounce.frictionCombine = PhysicMaterialCombine.Minimum;
        wallBounce.bounceCombine = PhysicMaterialCombine.Maximum;

        wall.GetComponent<Collider>().material = wallBounce;
    }

    void CreateGoals()
    {
        CreateGoal("Left Goal", new Vector3(-6.35f, 0.4f, 0));
        CreateGoal("Right Goal", new Vector3(6.35f, 0.4f, 0));
    }

    void CreateGoal(string name, Vector3 pos)
    {
        GameObject goal = GameObject.CreatePrimitive(PrimitiveType.Cube);
        goal.name = name;
        goal.transform.position = pos;
        goal.transform.localScale = new Vector3(0.3f, 0.8f, 1.8f);
        goal.GetComponent<Renderer>().material.color = Color.yellow;

        Collider col = goal.GetComponent<Collider>();
        col.isTrigger = true;

        GoalTrigger trigger = goal.AddComponent<GoalTrigger>();
        trigger.isLeftGoal = name == "Left Goal";
    }

    void CreatePuck()
    {
        GameObject puck = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        puck.name = "Puck";
        puck.tag = "Puck";

        puck.transform.position = new Vector3(0, 0.35f, 0);
        puck.transform.localScale = new Vector3(0.6f, 0.15f, 0.6f);
        puck.GetComponent<Renderer>().material.color = Color.black;

        Rigidbody rb = puck.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.drag = 0.05f;
        rb.angularDrag = 5f;

        rb.constraints =
            RigidbodyConstraints.FreezePositionY |
            RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationZ;

        PhysicMaterial bounceMaterial = new PhysicMaterial("PuckBounce");
        bounceMaterial.bounciness = 1f;
        bounceMaterial.dynamicFriction = 0f;
        bounceMaterial.staticFriction = 0f;
        bounceMaterial.frictionCombine = PhysicMaterialCombine.Minimum;
        bounceMaterial.bounceCombine = PhysicMaterialCombine.Maximum;

        puck.GetComponent<Collider>().material = bounceMaterial;

        AirHockeyGameManager.Instance.RegisterPuck(puck);

        rb.AddForce(new Vector3(5f, 0, 2f), ForceMode.Impulse);
    }

    void CreatePaddles()
    {
        CreatePaddle("Left Defender", new Vector3(-4.8f, 0.45f, 0), Color.blue);
        CreatePaddle("Left Striker", new Vector3(-2.2f, 0.45f, 0), Color.cyan);
        CreatePaddle("Right Defender", new Vector3(4.8f, 0.45f, 0), Color.red);
        CreatePaddle("Right Striker", new Vector3(2.2f, 0.45f, 0), new Color(1f, 0.5f, 0f));
    }

    void CreatePaddle(string name, Vector3 pos, Color color)
    {
        GameObject paddle = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        paddle.name = name;
        paddle.transform.position = pos;
        paddle.transform.localScale = new Vector3(0.7f, 0.25f, 0.7f);
        paddle.GetComponent<Renderer>().material.color = color;

        Rigidbody rb = paddle.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        PhysicMaterial paddleBounce = new PhysicMaterial("PaddleBounce");
        paddleBounce.bounciness = 1f;
        paddleBounce.dynamicFriction = 0f;
        paddleBounce.staticFriction = 0f;
        paddleBounce.frictionCombine = PhysicMaterialCombine.Minimum;
        paddleBounce.bounceCombine = PhysicMaterialCombine.Maximum;

        paddle.GetComponent<Collider>().material = paddleBounce;
        if (name == "Left Striker")
        {
            PlayerPaddleController controller = paddle.AddComponent<PlayerPaddleController>();
            controller.minX = -3f;
            controller.maxX = 0f;
            controller.minZ = -2.7f;
            controller.maxZ = 2.7f;
        }
        if (name == "Right Striker")
        {
            AirHockeyAgent agent = paddle.AddComponent<AirHockeyAgent>();

            GameObject puck = GameObject.Find("Puck");
            agent.puck = puck.transform;
            agent.puckRb = puck.GetComponent<Rigidbody>();

            agent.minX = 3f;
            agent.maxX = 5.2f;
            agent.minZ = -2.2f;
            agent.maxZ = 2.2f;

            var behaviors = paddle.GetComponents<Unity.MLAgents.Policies.BehaviorParameters>();

            for (int i = 1; i < behaviors.Length; i++)
            {
                Destroy(behaviors[i]);
            }

            var behavior = behaviors[0];

            behavior.BehaviorName = "StrikerAgent";
            behavior.BehaviorType = Unity.MLAgents.Policies.BehaviorType.Default;
            behavior.TeamId = 1;

            behavior.BrainParameters.VectorObservationSize = 9;
            behavior.BrainParameters.ActionSpec = Unity.MLAgents.Actuators.ActionSpec.MakeContinuous(2); ;

            var decisionRequester = paddle.AddComponent<Unity.MLAgents.DecisionRequester>();
            decisionRequester.DecisionPeriod = 1;
            decisionRequester.TakeActionsBetweenDecisions = true;
        }
    }

    void CreateCameraAndLight()
    {
        Camera.main.transform.position = new Vector3(0, 9, -7);
        Camera.main.transform.rotation = Quaternion.Euler(55, 0, 0);

        GameObject lightObj = GameObject.Find("Directional Light");
        if (lightObj != null)
        {
            lightObj.transform.rotation = Quaternion.Euler(50, -30, 0);
        }
    }
    void MakeStaticObject(GameObject obj)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb == null) rb = obj.AddComponent<Rigidbody>();

        rb.useGravity = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;

        Collider col = obj.GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = false;
        }
    }

    void MakePaddleObject(GameObject obj)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb == null) rb = obj.AddComponent<Rigidbody>();

        rb.useGravity = false;
        if (obj.GetComponent<AirHockeyAgent>() != null)
        {
            rb.isKinematic = false;
        }
        else
        {
            rb.isKinematic = true;
        }
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        rb.constraints =
            RigidbodyConstraints.FreezePositionY |
            RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationY |
            RigidbodyConstraints.FreezeRotationZ;

        Collider col = obj.GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = false;
        }
    }

    void MakePuckObject(GameObject obj)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb == null) rb = obj.AddComponent<Rigidbody>();

        rb.useGravity = false;
        rb.isKinematic = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.drag = 0f;
        rb.angularDrag = 0f;

        rb.constraints =
            RigidbodyConstraints.FreezePositionY |
            RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationZ;

        Collider col = obj.GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = false;
        }
    }

}
