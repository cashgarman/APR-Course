public class Foodstuff : ThrowableObject
{
    public FoodFightGame game;

    public override void OnTouchStart()
    {
        // Don't do anything, we don't want hover effects
    }

    public override void OnTouchEnd()
    {
        // Don't do anything, we don't want hover effects
    }

    public override void OnDropped()
    {
        base.OnDropped();

        // Let the game know the food was thrown
        game.OnFoodThrown();
    }
}
