using UnityEngine;

public class SerializableWheelCollider
{
    public SerializableWheelCollider()
    {
    }

    public SerializableWheelCollider(WheelCollider wheelCollider)
    {
        Mass = wheelCollider.mass;
        Radius = wheelCollider.radius;
        DampingRate = wheelCollider.wheelDampingRate;
        ForceApplicationPoint = wheelCollider.forceAppPointDistance;
        Center = wheelCollider.center;
        SuspensionSpring = wheelCollider.suspensionSpring;
        ForwardFrictionSpring = wheelCollider.forwardFriction;
        SideFrictionSpring = wheelCollider.sidewaysFriction;
    }

    public float Mass { get; set; }
    public float Radius { get; set; }
    public float DampingRate { get; set; }
    public float ForceApplicationPoint { get; set; }
    public SerializableVector Center { get; set; }
    public JointSpring SuspensionSpring { get; set; }
    public WheelFrictionCurve ForwardFrictionSpring { get; set; }
    public WheelFrictionCurve SideFrictionSpring { get; set; }
}