public class Foodstuff : GrabbableObject
{
    public float throwForce;

    private Grabber pickedUpHand;

    public override void OnHoverStart()
    {
        // Don't do anything, we don't want hover effects
    }

    public override void OnHoverEnd()
    {
        // Don't do anything, we don't want hover effects
    }

    public override void OnGrabbed(Grabber hand)
    {
        base.OnGrabbed(hand);

        // Store the hand that picked up the food
        pickedUpHand = hand;
    }

    public override void OnDropped()
    {
        base.OnDropped();

        // Throw the food really hard in the direction of the hand
        rigidBody.AddForce(pickedUpHand.transform.forward * throwForce);
    }
}
