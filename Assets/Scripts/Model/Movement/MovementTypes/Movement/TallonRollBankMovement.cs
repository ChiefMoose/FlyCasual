using Movement;

public class TallonRollBankMovement : TallonRollMovement
{
    private readonly float[] TURN_POINTS_FOR_BANK = new float[] { 4.6f, 7.4f, 10.4f };

    public TallonRollBankMovement(int speed, ManeuverDirection direction, ManeuverBearing bearing, ManeuverColor color) : base(speed, direction, bearing, color)
    {

    }

    protected override float SetTurningAroundDistance()
    {
        return GetMovement1() * TURN_POINTS_FOR_BANK[Speed - 1];
    }

    protected override float SetProgressTarget()
    {
        return 45f;
    }

    protected override float SetAnimationSpeed()
    {
        return 360f / Speed;
    }
}