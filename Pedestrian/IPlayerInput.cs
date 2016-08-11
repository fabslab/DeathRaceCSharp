namespace Pedestrian
{
    public interface IPlayerInput
    {
        // -1 to 1 indicating how far left or right player is turning (0 is straight, no turn)
        float GetTurnAngleNormalized();

        // -1 to 1 indicating how hard the player is accelerating (negative indicates reverse)
        float GetThrottleValue();
    }
}
