using UnityEngine;

public class AirHockeySceneBuilder : MonoBehaviour
{
    private PhysicsMaterial bounceMaterial;

    void Start()
    {
        BuildScene();
    }

    void BuildScene()
    {
        CreateBounceMaterial();

        if (FindFirstObjectByType<AirHockeyGameManager>() == null)
        {
            gameObject.AddComponent<AirHockeyGameManager>();
        }

        CreateFloor();
        CreateWalls();
        CreateGoals();
        CreatePuck();
        CreatePaddles();
        CreateCameraAndLight();
    }

    void CreateBounceMaterial()
    {
        bounceMaterial = new PhysicsMaterial("AirHockeyBounce");
        bounceMaterial.dynamicFriction = 0f;
        bounceMaterial.staticFriction = 0f;
        bounceMaterial.bounciness = 1f;
        bounceMaterial.frictionCombine = PhysicsMaterialCombine.Minimum;
        bounceMaterial.bounceCombine = PhysicsMaterialCombine.Maximum;
    }

    void CreateFloor()
    {
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        floor.name = "Rink Floor";
        floor.transform.position = Vector3.zero;
        floor.transform.localScale = new Vector3(12f, 0.2f, 6f);
        floor.GetComponent<Renderer>().material.color = new Color(0.05f, 0.35f, 0.85f);

        Rigidbody rb = floor.AddComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    void CreateWalls()
    {
        CreateWall("Top Wall", new Vector3(0, 0.6f, 3.2f), new Vector3(12.5f, 1f, 0.3f));
        CreateWall("Bottom Wall", new Vector3(0, 0.6f, -3.2f), new Vector3(12.5f, 1f, 0.3f));

        CreateWall("Left Upper Wall", new Vector3(-6.2f, 0.6f, 2.25f), new Vector3(0.3f, 1f, 1.9f));
        CreateWall("Left Lower Wall", new Vector3(-6.2f, 0.6f, -2.25f), new Vector3(0.3f, 1f, 1.9f));

        CreateWall("Right Upper Wall", new Vector3(6.2f, 0.6f, 2.25f), new Vector3(0.3f, 1f, 1.9f));
        CreateWall("Right Lower Wall", new Vector3(6.2f, 0.6f, -2.25f), new Vector3(0.3f, 1f, 1.9f));
    }

    void CreateWall(string name, Vector3 position, Vector3 scale)
    {
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.name = name;
        wall.transform.position = position;
        wall.transform.localScale = scale;
        wall.GetComponent<Renderer>().material.color = Color.gray;

        Rigidbody rb = wall.AddComponent<Rigidbody>();
        rb.isKinematic = true;

        Collider collider = wall.GetComponent<Collider>();
        collider.material = bounceMaterial;
    }

    void CreateGoals()
    {
        CreateGoal("Left Goal", new Vector3(-6.45f, 0.45f, 0), true);
        CreateGoal("Right Goal", new Vector3(6.45f, 0.45f, 0), false);
    }

    void CreateGoal(string name, Vector3 position, bool isLeftGoal)
    {
        GameObject goal = GameObject.CreatePrimitive(PrimitiveType.Cube);
        goal.name = name;
        goal.transform.position = position;
        goal.transform.localScale = new Vector3(0.25f, 0.8f, 1.7f);
        goal.GetComponent<Renderer>().material.color = Color.yellow;

        Collider collider = goal.GetComponent<Collider>();
        collider.isTrigger = true;

        GoalTrigger trigger = goal.AddComponent<GoalTrigger>();
        trigger.isLeftGoal = isLeftGoal;
    }

    void CreatePuck()
    {
        GameObject puck = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        puck.name = "Puck";
        puck.transform.position = new Vector3(0f, 0.35f, 0f);
        puck.transform.localScale = new Vector3(0.55f, 0.12f, 0.55f);
        puck.GetComponent<Renderer>().material.color = Color.black;

        Rigidbody rb = puck.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.mass = 0.6f;
        rb.linearDamping = 0.01f;
        rb.angularDamping = 4f;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.constraints =
            RigidbodyConstraints.FreezePositionY |
            RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationZ;

        Collider collider = puck.GetComponent<Collider>();
        collider.material = bounceMaterial;

        AirHockeyGameManager.Instance.RegisterPuck(puck);
        AirHockeyGameManager.Instance.ServePuck(Random.value > 0.5f ? 1 : -1);
    }

    void CreatePaddles()
    {
        GameObject puck = GameObject.Find("Puck");

        CreatePaddle("Left Defender", new Vector3(-4.8f, 0.45f, 0), Color.blue, PaddleRole.LeftDefender, puck);
        CreatePaddle("Left Striker", new Vector3(-2.2f, 0.45f, 0), Color.cyan, PaddleRole.HumanLeftStriker, puck);
        CreatePaddle("Right Defender", new Vector3(4.8f, 0.45f, 0), Color.red, PaddleRole.RightDefender, puck);
        CreatePaddle("Right Striker", new Vector3(2.2f, 0.45f, 0), new Color(1f, 0.5f, 0f), PaddleRole.RightStriker, puck);
    }

    void CreatePaddle(string name, Vector3 position, Color color, PaddleRole role, GameObject puck)
    {
        GameObject paddle = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        paddle.name = name;
        paddle.transform.position = position;
        paddle.transform.localScale = new Vector3(0.7f, 0.22f, 0.7f);
        paddle.GetComponent<Renderer>().material.color = color;

        Rigidbody rb = paddle.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.mass = 5f;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.constraints =
            RigidbodyConstraints.FreezePositionY |
            RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationY |
            RigidbodyConstraints.FreezeRotationZ;

        Collider collider = paddle.GetComponent<Collider>();
        collider.material = bounceMaterial;

        if (role == PaddleRole.HumanLeftStriker)
        {
            PlayerPaddleController controller = paddle.AddComponent<PlayerPaddleController>();
            controller.minX = -3f;
            controller.maxX = 0f;
            controller.minZ = -2.7f;
            controller.maxZ = 2.7f;
            controller.speed = 35f;
        }
        else
        {
            SimpleAIPaddleController ai = paddle.AddComponent<SimpleAIPaddleController>();
            ai.puck = puck.transform;
            ai.speed = 4.5f;

            if (role == PaddleRole.LeftDefender)
            {
                ai.minX = -5.8f;
                ai.maxX = -3.1f;
                ai.minZ = -2.7f;
                ai.maxZ = 2.7f;
            }
            else if (role == PaddleRole.RightDefender)
            {
                ai.minX = 3.1f;
                ai.maxX = 5.8f;
                ai.minZ = -2.7f;
                ai.maxZ = 2.7f;
            }
            else if (role == PaddleRole.RightStriker)
            {
                ai.minX = 0f;
                ai.maxX = 3f;
                ai.minZ = -2.7f;
                ai.maxZ = 2.7f;
            }
        }
    }

    void CreateCameraAndLight()
    {
        if (Camera.main != null)
        {
            Camera.main.transform.position = new Vector3(0f, 8.5f, -7.5f);
            Camera.main.transform.rotation = Quaternion.Euler(55f, 0f, 0f);
        }

        GameObject light = GameObject.Find("Directional Light");
        if (light != null)
        {
            light.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
        }
    }
}

public enum PaddleRole
{
    HumanLeftStriker,
    LeftDefender,
    RightDefender,
    RightStriker
}
