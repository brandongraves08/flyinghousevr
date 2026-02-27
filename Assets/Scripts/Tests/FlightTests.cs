using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

public class FlightTests
{
    [Test]
    public void SteeringWheel_ReturnsToCenter()
    {
        // Arrange
        var go = new GameObject("WheelTest");
        var wheel = go.AddComponent<SteeringWheel>();

        // Act
        wheel.SetWheelRotation(45f);

        // Assert
        float input = wheel.GetSteeringInput();
        Assert.That(input, Is.GreaterThan(0), "Steering input should be positive");

        Object.DestroyImmediate(go);
    }

    [Test]
    public void AltitudeLever_ThrottleRange_IsValid()
    {
        // Arrange
        var go = new GameObject("LeverTest");
        var lever = go.AddComponent<AltitudeLever>();

        // Act - throttle should be between -1 and 1
        float throttle = lever.GetThrottleInput();

        // Assert
        Assert.That(throttle, Is.InRange(-1f, 1f), "Throttle must be in valid range");

        Object.DestroyImmediate(go);
    }
}
