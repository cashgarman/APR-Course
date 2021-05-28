public class Foodstuff : ThrowableObject
{
    public FoodFightGame game;

    public override void OnHoverStart()
    {
        // Don't do anything, we don't want hover effects
    }

    public override void OnHoverEnd()
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
