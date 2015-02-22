using System;

namespace Breeze.Graphics
{
    public class AlphaAutomation : Automation<byte, Drawable>
    {
        public AlphaAutomation(byte startVal, bool active = false)
            : base(startVal, active)
        {
        }

        protected override void CalculateValue(double k)
        {
            CurrentValue = (byte)(Values[CurrentPos] + (Values[CurrentPos + 1] - Values[CurrentPos]) * k);
        }

        protected override void AutomateClient(Drawable client)
        {
            client.Alpha = CurrentValue;
        }
    }
}