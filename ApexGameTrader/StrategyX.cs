using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CapitalSkLib.ModuleObject;

namespace ApexGameTrader
{
    class StrategyX
    {
        public decimal DayHigh { get; set; }
        public decimal DayLow { get; set; }

        public DateTime DayHighTime { get; set; }
        public DateTime DayLowTime { get; set; }

        public decimal ThreeMinHigh { get; set; }
        public decimal ThreeMinLow { get; set; }

        public DateTime ThreeMinHighTime { get; set; }
        public DateTime ThreeMinLowTime { get; set; }

        private List<TickInfo> _threeMinTicks = new List<TickInfo>();

        public StrategyX()
        {
            ThreeMinLow = decimal.MaxValue;
            ThreeMinHigh = decimal.MinValue;
        }

        public void ProcessHighLow(TickInfo tick)
        {
            _threeMinTicks.Add(tick);
            Check3MinHighLow();
            CheckDayHigh(tick);
            CheckDayLow(tick);
        }

        public StrategyXShot GetSnapShot()
        {
            return new StrategyXShot
            {
                DayHigh = this.DayHigh,
                DayHighTime = this.DayHighTime,
                DayLow = this.DayLow,
                DayLowTime = this.DayLowTime,
                ThreeMinHigh = this.ThreeMinHigh,
                ThreeMinHighTime = this.ThreeMinHighTime,
                ThreeMinLow = this.ThreeMinLow,
                ThreeMinLowTime = this.ThreeMinLowTime
            };
        }

        private void CheckDayHigh(TickInfo tick)
        {
            decimal price = tick.Close / 100;
            if (DayHigh == 0)
            {
                DayHigh = price;
                DayHighTime = tick.TickTime;
            }
            if (price > DayHigh)
            {
                DayHigh = price;
                DayHighTime = tick.TickTime;
            }
            return;
        }

        private void CheckDayLow(TickInfo tick)
        {
            decimal price = tick.Close / 100;
            if (DayLow == 0)
            {
                DayLow = price;
                DayLowTime = tick.TickTime;
            }
            if (price < DayLow)
            {
                DayLow = price;
                DayLowTime = tick.TickTime;
            }
            return;
        }

        private void Check3MinHighLow()
        {
            TickInfo lastestTick = _threeMinTicks.LastOrDefault();
            decimal lastestPrice = lastestTick.Close / 100;
            TickInfo firstTick = _threeMinTicks.FirstOrDefault();

            DateTime threeMinBefore = lastestTick.TickTime.AddMinutes(-3);

            if (firstTick.TickTime <= threeMinBefore)
            {
                int stopIndex = 0;
                for (int idx = 0; idx < _threeMinTicks.Count; idx++)
                {
                    TickInfo tmpItem = _threeMinTicks.ElementAt(idx);
                    if (tmpItem.TickTime > threeMinBefore)
                    {
                        stopIndex = idx;
                        break;
                    }
                }
                _threeMinTicks.RemoveRange(0, stopIndex);
            }

            decimal currentMax = _threeMinTicks.Max(y => y.Close);
            TickInfo maxTick = _threeMinTicks.Where(x => x.Close == currentMax).FirstOrDefault();
            if ((currentMax / 100) > ThreeMinHigh)
            {
                ThreeMinHigh = currentMax / 100;
                ThreeMinHighTime = maxTick.TickTime;
            }
            else
            {
                if (ThreeMinHighTime < maxTick.TickTime)
                {
                    ThreeMinHigh = currentMax / 100;
                    ThreeMinHighTime = maxTick.TickTime;
                }
            }
            decimal currentMin = _threeMinTicks.Min(y => y.Close);
            TickInfo minTick = _threeMinTicks.Where(x => x.Close == currentMin).FirstOrDefault();
            if ((currentMin / 100) < ThreeMinLow)
            {
                ThreeMinLow = currentMin / 100;
                ThreeMinLowTime = minTick.TickTime;
            }
            else
            {
                if (ThreeMinLowTime < minTick.TickTime)
                {
                    ThreeMinLow = currentMin / 100;
                    ThreeMinLowTime = minTick.TickTime;
                }
            }

        }

        private DateTime GetDateTime(string timeStr)
        {
            DateTime dt = DateTime.Today;
            dt = dt.AddHours(Convert.ToInt32(timeStr.Substring(0, 2)));
            dt = dt.AddMinutes(Convert.ToInt32(timeStr.Substring(2, 2)));
            dt = dt.AddSeconds(Convert.ToInt32(timeStr.Substring(4, 2)));
            return dt;
        }
    }
    public class StrategyXShot
    {
        public decimal DayHigh { get; set; }
        public decimal DayLow { get; set; }

        public DateTime DayHighTime { get; set; }
        public DateTime DayLowTime { get; set; }

        public decimal ThreeMinHigh { get; set; }
        public decimal ThreeMinLow { get; set; }

        public DateTime ThreeMinHighTime { get; set; }
        public DateTime ThreeMinLowTime { get; set; }
    }
}
