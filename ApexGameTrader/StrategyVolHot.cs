using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CapitalSkLib.ModuleObject;

namespace ApexGameTrader
{
    public class StrategyVolHot
    {
        private List<TickInfo> _30SecTicks = new List<TickInfo>();

        public int ThirtyTradeCount { get; set; }
        public decimal ThirtyTickGap { get; set; }
        public int ThirtyTicksVolume { get; set; }

        public int Ten5TradeCount { get; set; }
        public decimal Ten5TickGap { get; set; }
        public int Ten5TicksVolume { get; set; }

        public int ThreeSecTradeCount { get; set; }
        public decimal ThreeSecTickGap { get; set; }
        public int ThreeSecTicksVolume { get; set; }

        public int FiveSecTradeCount { get; set; }
        public decimal FiveSecTickGap { get; set; }
        public int FiveSecTicksVolume { get; set; }

        public int TenSecTradeCount { get; set; }
        public decimal TenSecTickGap { get; set; }
        public int TenSecTicksVolume { get; set; }



        public StrategyVolHot()
        {
            this.Ten5TradeCount = 0;
            this.Ten5TicksVolume = 0;
        }

        public void ProcessVolumeHot(TickInfo tick)
        {
            _30SecTicks.Add(tick);
            CheckHot();
        }

        private void CheckHot()
        {
            TickInfo lastestTick = _30SecTicks.LastOrDefault();
            DateTime sec30Before = lastestTick.TickTime.AddSeconds(-30);
            DateTime sec15Before = lastestTick.TickTime.AddSeconds(-15);
            DateTime sec10Before = lastestTick.TickTime.AddSeconds(-10);
            DateTime sec5Before = lastestTick.TickTime.AddSeconds(-5);
            DateTime sec3Before = lastestTick.TickTime.AddSeconds(-3);

            bool keepRemove = true;
            do
            {
                TickInfo firstTick = _30SecTicks.FirstOrDefault();
                if (firstTick.TickTime < sec30Before)
                {
                    _30SecTicks.RemoveAt(0);
                }
                else
                {
                    keepRemove = false;
                }

            } while (keepRemove);

            this.ThirtyTradeCount = this._30SecTicks.Count();
            this.ThirtyTicksVolume = this._30SecTicks.Select(x => x.Qty).Sum();
            this.ThirtyTickGap = (this._30SecTicks.Select(x => x.Close).Max() - this._30SecTicks.Select(x => x.Close).Min()) / 100;

            var sec15TickList = this._30SecTicks.Where(x => x.TickTime > sec15Before).ToList();
            var sec10TickList = this._30SecTicks.Where(x => x.TickTime > sec10Before).ToList();
            var sec5TickList = this._30SecTicks.Where(x => x.TickTime > sec5Before).ToList();
            var sec3TickList = this._30SecTicks.Where(x => x.TickTime > sec3Before).ToList();

            this.Ten5TradeCount = sec15TickList.Count();
            this.Ten5TicksVolume = sec15TickList.Select(x => x.Qty).Sum();
            this.Ten5TickGap = (sec15TickList.Select(x => x.Close).Max() - sec15TickList.Select(x => x.Close).Min()) / 100;

            

            this.TenSecTradeCount = sec10TickList.Count();
            this.TenSecTicksVolume = sec10TickList.Select(x => x.Qty).Sum();
            this.TenSecTickGap = (sec10TickList.Select(x => x.Close).Max() - sec10TickList.Select(x => x.Close).Min()) / 100;

            this.FiveSecTradeCount = sec5TickList.Count();
            this.FiveSecTicksVolume = sec5TickList.Select(x => x.Qty).Sum();
            this.FiveSecTickGap = (sec5TickList.Select(x => x.Close).Max() - sec5TickList.Select(x => x.Close).Min()) / 100;

            this.ThreeSecTradeCount = sec3TickList.Count();
            this.ThreeSecTicksVolume = sec3TickList.Select(x => x.Qty).Sum();
            this.ThreeSecTickGap = (sec3TickList.Select(x => x.Close).Max() - sec3TickList.Select(x => x.Close).Min()) / 100;
        }


        public StrategyVolHotShot GetSnapshot()
        {
            return new StrategyVolHotShot
            {
                TradeCountIn30Sec = this.ThirtyTradeCount,
                TickGapIn30Sec = this.ThirtyTickGap,
                TotalVolIn30Sec = this.ThirtyTicksVolume,
                TradeCountIn15Sec = this.Ten5TradeCount,
                TickGapIn15Sec = this.Ten5TickGap,
                TotalVolIn15Sec = this.Ten5TicksVolume,
                TradeCountIn10Sec = this.TenSecTradeCount,
                TickGapIn10Sec = this.TenSecTickGap,
                TotalVolIn10Sec = this.TenSecTicksVolume,
                TradeCountIn5Sec = this.FiveSecTradeCount,
                TickGapIn5Sec = this.FiveSecTickGap,
                TotalVolIn5Sec = this.FiveSecTicksVolume,
                TradeCountIn3Sec = this.ThreeSecTradeCount,
                TickGapIn3Sec = this.ThreeSecTickGap,
                TotalVolIn3Sec = this.ThreeSecTicksVolume

            };
        }
    }

    public class StrategyVolHotShot
    {
        public int TradeCountIn30Sec { get; set; }
        public decimal TickGapIn30Sec { get; set; }
        public int TotalVolIn30Sec { get; set; }

        public int TradeCountIn15Sec { get; set; }
        public decimal TickGapIn15Sec { get; set; }
        public int TotalVolIn15Sec { get; set; }

        public int TradeCountIn10Sec { get; set; }
        public decimal TickGapIn10Sec { get; set; }
        public int TotalVolIn10Sec { get; set; }

        public int TradeCountIn5Sec { get; set; }
        public decimal TickGapIn5Sec { get; set; }
        public int TotalVolIn5Sec { get; set; }

        public int TradeCountIn3Sec { get; set; }
        public decimal TickGapIn3Sec { get; set; }
        public int TotalVolIn3Sec { get; set; }
    }
}
