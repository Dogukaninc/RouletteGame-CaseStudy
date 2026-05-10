using _RouletteGame.Scripts.Interfaces;

namespace _RouletteGame.Scripts.Events
{
    public struct OnMoneyChanged : IEvent
    {
        public int NewMoneyAmount { get; set; }
        public OnMoneyChanged(int newMoneyAmount)
        {
            NewMoneyAmount = newMoneyAmount;
        }
    }
}