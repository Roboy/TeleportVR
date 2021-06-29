using UnityEngine;

public class BioIKLatencyManager : MonoBehaviour
{
    [System.Serializable]
    public class VelAcc
    {
        public float velocity, acceleration;
        public VelAcc(float velocity, float acceleration)
        {
            this.velocity = velocity;
            this.acceleration = acceleration;
        }
    }

    public BioIK.BioIK arms, leftHand, rightHand;
    public VelAcc armControlLimits = new VelAcc(velocity: 10, acceleration: 20);
    public VelAcc handControlLimits = new VelAcc(velocity: 10, acceleration: 20);

    private VelAcc armDefault, handDefault;
    private StateManager.States currentState;

    // Start is called before the first frame update
    void Start()
    {
        armDefault = new VelAcc(arms.MaximumVelocity, arms.MaximumAcceleration);
        handDefault = new VelAcc(Mathf.Max(leftHand.MaximumVelocity, rightHand.MaximumVelocity),
                                 Mathf.Max(leftHand.MaximumAcceleration, rightHand.MaximumAcceleration));
        currentState = StateManager.States.Training;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == StateManager.Instance.currentState)
        {
            return;
        }
        currentState = StateManager.Instance.currentState;
        switch (currentState)
        {
            case StateManager.States.HUD:
                arms.MaximumVelocity = armControlLimits.velocity;
                arms.MaximumAcceleration = armControlLimits.acceleration;
                leftHand.MaximumVelocity = handControlLimits.velocity;
                leftHand.MaximumAcceleration = handControlLimits.acceleration;
                rightHand.MaximumVelocity = handControlLimits.velocity;
                rightHand.MaximumAcceleration = handControlLimits.acceleration;
                break;
            default:
                arms.MaximumVelocity = armDefault.velocity;
                arms.MaximumAcceleration = armDefault.acceleration;
                leftHand.MaximumVelocity = handDefault.velocity;
                leftHand.MaximumAcceleration = handDefault.acceleration;
                rightHand.MaximumVelocity = handDefault.velocity;
                rightHand.MaximumAcceleration = handDefault.acceleration;
                break;
        }
    }
}
