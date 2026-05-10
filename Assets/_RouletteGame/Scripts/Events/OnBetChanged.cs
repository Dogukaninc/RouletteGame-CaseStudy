using _RouletteGame.Scripts.Interfaces;

namespace _RouletteGame.Scripts.Events
{
    public struct OnBetChanged : IEvent
    {
        public int NewBetAmount { get; set; }
        public OnBetChanged(int newBetAmount)
        {
            NewBetAmount = newBetAmount;
        }
        
    }
}