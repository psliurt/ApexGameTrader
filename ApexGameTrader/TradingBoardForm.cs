using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapitalSkLib;
using CapitalSkLib.ModuleObject;
using ApexCloudApi;
using ApexCloudApi.ApiMsg;
using ApexCloudApi.Mgr;
using System.Configuration;
using System.Security.Cryptography;
using System.IO;

namespace ApexGameTrader
{
    public partial class TradingBoardForm : Form
    {
        /// <summary>
        /// 寶碩的api
        /// </summary>
        private ApiFacade _apexApi { get; set; }
        /// <summary>
        /// 群益報價模組
        /// </summary>
        private SkFacade _lib { get; set; }

        private StrategyX _strategy { get; set; }

        private StrategyVolHot _strategyHot { get; set; }

        private TickInfo _currentTick { get; set; }

        private System.Threading.Timer _timer { get; set; }

        private List<StrategyXShot> _shotList { get; set; }

        private VirtualOrder _currentOrder = null;

        private string _apexSymbol { get; set; }

        private string _aesKey = "tbueWVovyLATbn9oKEspOyPfinp08UWNKx/fX7z7TYQ=";
        private string _aesIv = "N9lJI7WnVuLJp4udOLB1iQ==";

        private System.Threading.Timer _timeCheckTimer { get; set; }
        private bool _stopMakeOrder { get; set; }

        public TradingBoardForm()
        {
            InitializeComponent();

            //this._apexSymbol = GetConfigSetting("ApexSymbol");
            //this._stopMakeOrder = false;

            InitialSKLib();
            InitialApexApi();
            //_shotList = new List<StrategyXShot>();
            //_strategy = new StrategyX();
            //_strategyHot = new StrategyVolHot();
            //_timer = new System.Threading.Timer(ThreadTimerCallBack, this, 120000, 1500);
            //_timeCheckTimer = new System.Threading.Timer(CheckTimeCallBack, this, 10000, 30000);
        }

        private void CheckTimeCallBack(object state)
        {
            DateTime closeTime = DateTime.Today.AddHours(13).AddMinutes(40).AddSeconds(0);
            if (DateTime.Now > closeTime)
            {
                this._stopMakeOrder = true;
                if (this._currentOrder != null)
                {
                    ClearOrder(ref this._currentOrder);
                }                
            }            
        }

        private void ClearOrder(ref VirtualOrder currentOrder)
        {
            int longOrShort = currentOrder.Direction;
            if (longOrShort > 0)
            {
                _apexApi.SendFutureOrder(this._apexSymbol, FutureBuyOrSell.Sell, false, FutureTradeCondition.ROD, FuturePriceType.Limit, (_currentTick.Close / 100) - 100, 10);
                PostMsg(string.Format("已經到達收盤時間， 多 單 全數出場"));
            }
            else
            {
                _apexApi.SendFutureOrder(this._apexSymbol, FutureBuyOrSell.Buy, false, FutureTradeCondition.ROD, FuturePriceType.Limit, (_currentTick.Close / 100) + 100, 10);
                PostMsg(string.Format("已經到達收盤時間， 空 單 全數出場"));
            }
            currentOrder = null;
        }

        private string Encrypt(string plain)
        {
            string encryptString = null;
            using (SymmetricAlgorithm algorithm = AesCryptoServiceProvider.Create())
            {
                byte[] key = Convert.FromBase64String(_aesKey);
                byte[] iv = Convert.FromBase64String(_aesIv);
                algorithm.Key = key;
                algorithm.IV = iv;

                ICryptoTransform encryptor = algorithm.CreateEncryptor(key, iv);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(plain);
                        }
                        byte[] encryptBytes = ms.ToArray();
                        encryptString = Convert.ToBase64String(encryptBytes);
                    }
                }
            }
            return encryptString;
        }

        private string Decrypt(string encryptString)
        {
            byte[] encryptBytes = Convert.FromBase64String(encryptString);
            string plain = null;
            using (SymmetricAlgorithm algorithm = AesCryptoServiceProvider.Create())
            {
                byte[] key = Convert.FromBase64String(_aesKey);
                byte[] iv = Convert.FromBase64String(_aesIv);

                algorithm.Key = key;
                algorithm.IV = iv;

                ICryptoTransform decryptor = algorithm.CreateDecryptor(key, iv);
                using (MemoryStream ms = new MemoryStream(encryptBytes))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            plain = sr.ReadToEnd();
                        }                        
                    }
                }
            }
            return plain;
        }

        public void ThreadTimerCallBack(object state)
        {
            StrategyXShot shot = _strategy.GetSnapShot();
            _shotList.Insert(0, shot);
            if (this._stopMakeOrder)
            { return; }
            StrategyAnalize();
        }

        private void StrategyAnalize()
        {
            if (this._stopMakeOrder)
            { return; }

            if (_shotList.Count < 3) { return; }

            var shot1 = _shotList.FirstOrDefault();
            var shot2 = _shotList.ElementAt(1);
            var shot3 = _shotList.ElementAt(2);

            var highDiv2_1 = shot2.ThreeMinHigh - shot1.ThreeMinHigh;
            var highDiv3_2 = shot3.ThreeMinHigh - shot2.ThreeMinHigh;

            var lowDiv2_1 = shot2.ThreeMinLow - shot1.ThreeMinLow;
            var lowDiv3_2 = shot3.ThreeMinLow - shot2.ThreeMinLow;

            if (this._stopMakeOrder)
            { return; }

            if (_currentOrder == null || DoCheckAndEndOrder(ref _currentOrder, _currentTick) == true) //目前沒單 或 單子有出掉，才可以再進單
            {

                var volShot = _strategyHot.GetSnapshot();

                if (shot3.ThreeMinHigh < shot2.ThreeMinHigh && shot2.ThreeMinHigh < shot1.ThreeMinHigh && highDiv2_1 < 0 && highDiv3_2 < 0)
                {
                    //可能是反轉點
                    if (volShot.TradeCountIn15Sec >= 100 && volShot.TradeCountIn30Sec >= 200 &&
                        (volShot.TotalVolIn15Sec / volShot.TradeCountIn15Sec) >= 3 &&
                        (volShot.TradeCountIn15Sec * 2) < volShot.TradeCountIn30Sec)
                    {
                        AddOrder(-1, true, volShot);
                    }
                    else //正常的進單方向
                    {
                        //只有在15秒內最高跟最低的差距有大於10秒內的差距才進單，因為若是10秒跟15秒內的差距一樣，很可能是沒有任何進展的意思
                        //if (volShot.TickGapIn15Sec > volShot.TickGapIn10Sec)
                        //{
                            AddOrder(1, false, volShot);
                        //}                        
                    }                    
                }

                if (shot3.ThreeMinLow > shot2.ThreeMinLow && shot2.ThreeMinLow > shot1.ThreeMinLow && lowDiv2_1 > 0 && lowDiv3_2 > 0)
                {
                    //可能是反轉點
                    if (volShot.TradeCountIn15Sec >= 100 && volShot.TradeCountIn30Sec >= 200  &&
                        (volShot.TotalVolIn15Sec / volShot.TradeCountIn15Sec) >= 3 &&
                        (volShot.TradeCountIn15Sec * 2) < volShot.TradeCountIn30Sec)
                    {
                        AddOrder(1, true, volShot);
                    }
                    else //正常的進單方向
                    {
                        //if (volShot.TickGapIn15Sec > volShot.TickGapIn10Sec)
                        //{
                        AddOrder(-1, false, volShot);
                        //}
                    }
                }
            }

        }

        private bool DoCheckAndEndOrder(ref VirtualOrder order, TickInfo currentTick)
        {
            bool doEndOrder = false;

            if (currentTick == null)
            {
                return false;
            }

            if (order != null)
            {
                if (order.Direction > 0)//long
                {
                    if (currentTick.Close / 100 - order.Price <= -5) //停損
                    {
                        EndOrder(order.Direction, -1);
                        order = null;
                        doEndOrder = true;
                        return doEndOrder;
                    }

                    if (currentTick.Close / 100 - order.Price >= 10) //停利
                    {
                        EndOrder(order.Direction, 1);
                        order = null;
                        doEndOrder = true;
                        return doEndOrder;
                    }
                }

                if (order.Direction < 0)//short
                {
                    if (order.Price - currentTick.Close / 100 <= -5)//停損
                    {
                        EndOrder(order.Direction, -1);
                        order = null;
                        doEndOrder = true;
                        return doEndOrder;
                    }

                    if (order.Price - currentTick.Close / 100 >= 10) //停利
                    {
                        EndOrder(order.Direction, 1);
                        order = null;
                        doEndOrder = true;
                        return doEndOrder;
                    }
                }
            }
            else
            {
                doEndOrder = false;
            }
            return doEndOrder;
        }

        private void EndOrder(int longOrShort, int winOrLoss)
        {

            if (longOrShort > 0)
            {
                if (winOrLoss > 0)
                {
                    _apexApi.SendFutureOrder(this._apexSymbol, FutureBuyOrSell.Sell, false, FutureTradeCondition.ROD, FuturePriceType.Limit, (_currentTick.Close / 100) - 100, 10);

                    PostMsg(string.Format("一筆 多 單 停利出場 {0}, 價位 {1}", DateTime.Now.ToString("HH:mm:ss"), (_currentTick.Close / 100) - 1));
                }
                else
                {
                    _apexApi.SendFutureOrder(this._apexSymbol, FutureBuyOrSell.Sell, false, FutureTradeCondition.ROD, FuturePriceType.Limit, (_currentTick.Close / 100) - 100, 10);
                    PostMsg(string.Format("一筆 多 單 停損出場 {0}, 價位 {1}", DateTime.Now.ToString("HH:mm:ss"), (_currentTick.Close / 100) - 1));
                }
            }
            else
            {
                if (winOrLoss > 0)
                {
                    _apexApi.SendFutureOrder(this._apexSymbol, FutureBuyOrSell.Buy, false, FutureTradeCondition.ROD, FuturePriceType.Limit, (_currentTick.Close / 100) + 100, 10);
                    PostMsg(string.Format("一筆 空 單 停利出場 {0}, 價位 {1}", DateTime.Now.ToString("HH:mm:ss"), (_currentTick.Close / 100) + 1));
                }
                else
                {
                    _apexApi.SendFutureOrder(this._apexSymbol, FutureBuyOrSell.Buy, false, FutureTradeCondition.ROD, FuturePriceType.Limit, (_currentTick.Close / 100) + 100, 10);
                    PostMsg(string.Format("一筆 空 單 停損出場 {0}, 價位 {1}", DateTime.Now.ToString("HH:mm:ss"), (_currentTick.Close / 100) + 1));
                }
            }
        }

        private void AddOrder(int longOrShort,bool isReverse, StrategyVolHotShot volShot)
        {
            if (longOrShort > 0)
            {
                if (_currentTick != null)
                {                   
                    string orderId = _apexApi.SendFutureOrder(this._apexSymbol,
                        FutureBuyOrSell.Buy, false,
                        FutureTradeCondition.ROD,
                        FuturePriceType.Limit, (_currentTick.Close / 100) + 100, 10);

                    this._currentOrder = new VirtualOrder
                    {
                        OrderId = orderId,
                        Direction = 1,
                        Price = _currentTick.Close / 100
                    };

                    if (isReverse)
                    {
                        PostMsg(string.Format("作 一筆 [ 反轉 ] 多 單 {0}, 價位 {1}     15:[ {2}, {3}, {4}] | 10:[ {5}, {6}, {7}] | 5:[ {8}, {9}, {10}] | 3:[ {11}, {12}, {13}] 30:[ {14}, {15}, {16}]",
                        DateTime.Now.ToString("HH:mm:ss"), (_currentTick.Close / 100) + 1,
                        volShot.TradeCountIn15Sec, volShot.TotalVolIn15Sec, volShot.TickGapIn15Sec,
                        volShot.TradeCountIn10Sec, volShot.TotalVolIn10Sec, volShot.TickGapIn10Sec,
                        volShot.TradeCountIn5Sec, volShot.TotalVolIn5Sec, volShot.TickGapIn5Sec,
                        volShot.TradeCountIn3Sec, volShot.TotalVolIn3Sec, volShot.TickGapIn3Sec,
                        volShot.TradeCountIn30Sec, volShot.TotalVolIn30Sec, volShot.TickGapIn30Sec));
                    }
                    else
                    {
                        PostMsg(string.Format("作 一筆 多 單 {0}, 價位 {1}     15:[ {2}, {3}, {4}] | 10:[ {5}, {6}, {7}] | 5:[ {8}, {9}, {10}] | 3:[ {11}, {12}, {13}] 30:[ {14}, {15}, {16}]",
                            DateTime.Now.ToString("HH:mm:ss"), (_currentTick.Close / 100) + 1,
                            volShot.TradeCountIn15Sec, volShot.TotalVolIn15Sec, volShot.TickGapIn15Sec,
                            volShot.TradeCountIn10Sec, volShot.TotalVolIn10Sec, volShot.TickGapIn10Sec,
                            volShot.TradeCountIn5Sec, volShot.TotalVolIn5Sec, volShot.TickGapIn5Sec,
                            volShot.TradeCountIn3Sec, volShot.TotalVolIn3Sec, volShot.TickGapIn3Sec,
                            volShot.TradeCountIn30Sec, volShot.TotalVolIn30Sec, volShot.TickGapIn30Sec));
                    }                   
                    
                }
            }
            else
            {

                if (_currentTick != null)
                {
                    
                    string orderId = _apexApi.SendFutureOrder(this._apexSymbol,
                        FutureBuyOrSell.Sell, false,
                        FutureTradeCondition.ROD,
                        FuturePriceType.Limit, (_currentTick.Close / 100) - 100, 10);
                    this._currentOrder = new VirtualOrder
                    {
                        OrderId = orderId,
                        Direction = -1,
                        Price = _currentTick.Close / 100
                    };

                    if (isReverse)
                    {
                        PostMsg(string.Format("作 一筆 [ 反轉 ] 空 單 {0}, 價位 {1}          15:[ {2}, {3}, {4}] | 10:[ {5}, {6}, {7}] | 5:[ {8}, {9}, {10}] | 3:[ {11}, {12}, {13}] 30:[ {14}, {15}, {16}]",
                        DateTime.Now.ToString("HH:mm:ss"), (_currentTick.Close / 100) - 1,
                        volShot.TradeCountIn15Sec, volShot.TotalVolIn15Sec, volShot.TickGapIn15Sec,
                        volShot.TradeCountIn10Sec, volShot.TotalVolIn10Sec, volShot.TickGapIn10Sec,
                        volShot.TradeCountIn5Sec, volShot.TotalVolIn5Sec, volShot.TickGapIn5Sec,
                        volShot.TradeCountIn3Sec, volShot.TotalVolIn3Sec, volShot.TickGapIn3Sec,
                        volShot.TradeCountIn30Sec, volShot.TotalVolIn30Sec, volShot.TickGapIn30Sec));
                    }
                    else
                    {
                        PostMsg(string.Format("作 一筆 空 單 {0}, 價位 {1}          15:[ {2}, {3}, {4}] | 10:[ {5}, {6}, {7}] | 5:[ {8}, {9}, {10}] | 3:[ {11}, {12}, {13}] 30:[ {14}, {15}, {16}]",
                            DateTime.Now.ToString("HH:mm:ss"), (_currentTick.Close / 100) - 1,
                            volShot.TradeCountIn15Sec, volShot.TotalVolIn15Sec, volShot.TickGapIn15Sec,
                            volShot.TradeCountIn10Sec, volShot.TotalVolIn10Sec, volShot.TickGapIn10Sec,
                            volShot.TradeCountIn5Sec, volShot.TotalVolIn5Sec, volShot.TickGapIn5Sec,
                            volShot.TradeCountIn3Sec, volShot.TotalVolIn3Sec, volShot.TickGapIn3Sec,
                            volShot.TradeCountIn30Sec, volShot.TotalVolIn30Sec, volShot.TickGapIn30Sec));
                    }
                    
                }
            }
        }



        private void InitialSKLib()
        {
            this._lib = SkFacade.Instance(SkLibType.Quote | SkLibType.OsQuote);
            this._lib.Initialize(GetConfigAccountPwd("SkAccount"),
                GetConfigAccountPwd("SkPwd"));
            this._lib.OnQuoteServerReady += _lib_OnQuoteServerReady;
            this._lib.OnTwTick += _lib_OnTwTick;
            this._lib.OnTwQuote += _lib_OnTwQuote;
            this._lib.OnTwBest5 += _lib_OnTwBest5;
            this._lib.OnTwMarketInfo += _lib_OnTwMarketInfo;
            PostMsg("群益API登入、載入完成...");
        }

        void _lib_OnQuoteServerReady(SkQuoteReadyType readyType)
        {
            if (readyType == SkQuoteReadyType.TwQuoteReady)
            {
                string listenQuoteSymbol = GetConfigSetting("SkSymbol");
                this._lib.SubscribeTWTick(listenQuoteSymbol);
                PostMsg("群益API報價模組連線完成!");
            }
        }        

        private void InitialApexApi()
        {
            this._apexApi = ApiFacade.GetInstance();
            this._apexApi.Login(
                GetConfigAccountPwd("ApexAccount"),
                GetConfigAccountPwd("ApexPwd"));
            PostMsg("寶碩下單API登入完成...");
        }
        /// <summary>
        /// 這個Function要用加解秘來作
        /// </summary>
        /// <param name="cfgKey"></param>
        /// <returns></returns>
        private string GetConfigAccountPwd(string cfgKey)
        {
            return Decrypt(ConfigurationManager.AppSettings.Get(cfgKey));
        }

        private string GetConfigSetting(string cfgKey)
        {
            return ConfigurationManager.AppSettings.Get(cfgKey);
        }

        private void TradingBoardForm_Load(object sender, EventArgs e)
        {
            
        }       

        void _lib_OnTwMarketInfo(TwMarketInfoType infoType, TwMarketInfo infoObj)
        {
            
        }

        void _lib_OnTwBest5(Best5Info best5)
        {
            
        }

        void _lib_OnTwQuote(StockInfo stockQuote)
        {
            
        }

        void _lib_OnTwTick(TickInfo tick)
        {
            _currentTick = tick;
            //_strategy.ProcessHighLow(tick);
            //_strategyHot.ProcessVolumeHot(tick);
            
        }

        private void TradingBoardForm_FormClosing(object sender, FormClosingEventArgs e)
        {            
            this._lib.ExitSKLib();
        }

        delegate void MessageHandler(string msg);
        private void PostMsg(string msg)
        {
            if (_msgListBox.InvokeRequired)
            {
                MessageHandler msgHandler = new MessageHandler(PostMsg);
                _msgListBox.Invoke(msgHandler, msg);
            }
            else
            {
                if (_msgListBox.Items.Count > 256)
                {
                    _msgListBox.Items.RemoveAt(_msgListBox.Items.Count - 1);
                }
                _msgListBox.Items.Insert(0, msg);
            }
        }

        private void _futureList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedFuture = _futureList.SelectedItem.ToString();
            _apexFutureCodeTxt.Text = selectedFuture;
        }

        private void buyFutureBtn_Click(object sender, EventArgs e)
        {
            int qty = Convert.ToInt32(_futureQtyTxt.Text);
            string futureCode = _apexFutureCodeTxt.Text.Trim().Split('-')[0];
            if (qty > 100)
            {
                int loopTime = qty / 100;
                int remaind = qty % 100;
                for (int i = 0; i < loopTime; i++)
                {
                    _apexApi.SendFutureOrder(futureCode, FutureBuyOrSell.Buy, _futureDayTradeChk.Checked, FutureTradeCondition.ROD, FuturePriceType.Limit, (_currentTick.Close / 100) + 100, 100);    
                }
                if (remaind > 0)
                {
                    _apexApi.SendFutureOrder(futureCode, FutureBuyOrSell.Buy, _futureDayTradeChk.Checked, FutureTradeCondition.ROD, FuturePriceType.Limit, (_currentTick.Close / 100) + 100, remaind);    
                }
            }
            else
            {
                _apexApi.SendFutureOrder(futureCode, FutureBuyOrSell.Buy, _futureDayTradeChk.Checked, FutureTradeCondition.ROD, FuturePriceType.Limit, (_currentTick.Close / 100) + 100, qty);
            }
            MessageBox.Show("買單下單完畢");
        }

        private void _sellFutureBtn_Click(object sender, EventArgs e)
        {
            int qty = Convert.ToInt32(_futureQtyTxt.Text);
            string futureCode = _apexFutureCodeTxt.Text.Trim().Split('-')[0];
            if (qty > 100)
            {
                int loopTime = qty / 100;
                int remaind = qty % 100;
                for (int i = 0; i < loopTime; i++)
                {
                    _apexApi.SendFutureOrder(futureCode, FutureBuyOrSell.Sell, _futureDayTradeChk.Checked, FutureTradeCondition.ROD, FuturePriceType.Limit, (_currentTick.Close / 100) - 100, 100);
                }
                if (remaind > 0)
                {
                    _apexApi.SendFutureOrder(futureCode, FutureBuyOrSell.Sell, _futureDayTradeChk.Checked, FutureTradeCondition.ROD, FuturePriceType.Limit, (_currentTick.Close / 100) - 100, remaind);
                }
            }
            else
            {
                _apexApi.SendFutureOrder(futureCode, FutureBuyOrSell.Sell, _futureDayTradeChk.Checked, FutureTradeCondition.ROD, FuturePriceType.Limit, (_currentTick.Close / 100) - 100, qty);
            }
            MessageBox.Show("賣單下單完畢");
        }

        private void _futureQty1_Click(object sender, EventArgs e)
        {
            _futureQtyTxt.Text = "1";
        }

        private void _futureQty2_Click(object sender, EventArgs e)
        {
            _futureQtyTxt.Text = "2";
        }

        private void _futureQty3_Click(object sender, EventArgs e)
        {
            _futureQtyTxt.Text = "3";
        }

        private void _futureQty4_Click(object sender, EventArgs e)
        {
            _futureQtyTxt.Text = "4";
        }

        private void _futureQty5_Click(object sender, EventArgs e)
        {
            _futureQtyTxt.Text = "5";
        }

        private void _buyOptionBtn_Click(object sender, EventArgs e)
        {
            int qty = Convert.ToInt32(_optionQtyTxt.Text);

            if (qty > 100)
            {
                int loopTime = qty / 100;
                int remaind = qty % 100;
                for (int i = 0; i < loopTime; i++)
                {
                    _apexApi.SendOptionOrder(_optionCodeTxt.Text.Trim(), FutureBuyOrSell.Buy, FutureCloseType.Open, FutureTradeCondition.ROD, FuturePriceType.Limit, Convert.ToDecimal(_optionPriceTxt.Text.Trim()), 100);
                }
                if (remaind > 0)
                {
                    _apexApi.SendOptionOrder(_optionCodeTxt.Text.Trim(), FutureBuyOrSell.Buy, FutureCloseType.Open, FutureTradeCondition.ROD, FuturePriceType.Limit, Convert.ToDecimal(_optionPriceTxt.Text.Trim()), remaind);
                }
            }
            else
            {
                _apexApi.SendOptionOrder(_optionCodeTxt.Text.Trim(), FutureBuyOrSell.Buy, FutureCloseType.Open, FutureTradeCondition.ROD, FuturePriceType.Limit, Convert.ToDecimal(_optionPriceTxt.Text.Trim()), qty);
            }
            MessageBox.Show("買入新倉下單完畢");
        }

        private void _sellOptionBtn_Click(object sender, EventArgs e)
        {
            int qty = Convert.ToInt32(_optionQtyTxt.Text);

            if (qty > 100)
            {
                int loopTime = qty / 100;
                int remaind = qty % 100;
                for (int i = 0; i < loopTime; i++)
                {
                    _apexApi.SendOptionOrder(_optionCodeTxt.Text.Trim(), FutureBuyOrSell.Sell, FutureCloseType.Open, FutureTradeCondition.ROD, FuturePriceType.Limit, Convert.ToDecimal(_optionPriceTxt.Text.Trim()), 100);
                }
                if (remaind > 0)
                {
                    _apexApi.SendOptionOrder(_optionCodeTxt.Text.Trim(), FutureBuyOrSell.Sell, FutureCloseType.Open, FutureTradeCondition.ROD, FuturePriceType.Limit, Convert.ToDecimal(_optionPriceTxt.Text.Trim()), remaind);
                }
            }
            else
            {
                _apexApi.SendOptionOrder(_optionCodeTxt.Text.Trim(), FutureBuyOrSell.Sell, FutureCloseType.Open, FutureTradeCondition.ROD, FuturePriceType.Limit, Convert.ToDecimal(_optionPriceTxt.Text.Trim()), qty);
            }
            MessageBox.Show("賣出新倉下單完畢");
        }

        private void _sellEndOptionBtn_Click(object sender, EventArgs e)
        {
            int qty = Convert.ToInt32(_optionQtyTxt.Text);

            if (qty > 100)
            {
                int loopTime = qty / 100;
                int remaind = qty % 100;
                for (int i = 0; i < loopTime; i++)
                {
                    _apexApi.SendOptionOrder(_optionCodeTxt.Text.Trim(), FutureBuyOrSell.Sell, FutureCloseType.Close, FutureTradeCondition.ROD, FuturePriceType.Limit, Convert.ToDecimal(_optionPriceTxt.Text.Trim()), 100);
                }
                if (remaind > 0)
                {
                    _apexApi.SendOptionOrder(_optionCodeTxt.Text.Trim(), FutureBuyOrSell.Sell, FutureCloseType.Close, FutureTradeCondition.ROD, FuturePriceType.Limit, Convert.ToDecimal(_optionPriceTxt.Text.Trim()), remaind);
                }
            }
            else
            {
                _apexApi.SendOptionOrder(_optionCodeTxt.Text.Trim(), FutureBuyOrSell.Sell, FutureCloseType.Close, FutureTradeCondition.ROD, FuturePriceType.Limit, Convert.ToDecimal(_optionPriceTxt.Text.Trim()), qty);
            }
            MessageBox.Show("賣出平倉下單完畢");
        }

        private void _buyEndOptionBtn_Click(object sender, EventArgs e)
        {
            int qty = Convert.ToInt32(_optionQtyTxt.Text);

            if (qty > 100)
            {
                int loopTime = qty / 100;
                int remaind = qty % 100;
                for (int i = 0; i < loopTime; i++)
                {
                    _apexApi.SendOptionOrder(_optionCodeTxt.Text.Trim(), FutureBuyOrSell.Buy, FutureCloseType.Close, FutureTradeCondition.ROD, FuturePriceType.Limit, Convert.ToDecimal(_optionPriceTxt.Text.Trim()), 100);
                }
                if (remaind > 0)
                {
                    _apexApi.SendOptionOrder(_optionCodeTxt.Text.Trim(), FutureBuyOrSell.Buy, FutureCloseType.Close, FutureTradeCondition.ROD, FuturePriceType.Limit, Convert.ToDecimal(_optionPriceTxt.Text.Trim()), remaind);
                }
            }
            else
            {
                _apexApi.SendOptionOrder(_optionCodeTxt.Text.Trim(), FutureBuyOrSell.Buy, FutureCloseType.Close, FutureTradeCondition.ROD, FuturePriceType.Limit, Convert.ToDecimal(_optionPriceTxt.Text.Trim()), qty);
            }
            MessageBox.Show("買入平倉下單完畢");
        }
    }

    public class VirtualOrder
    {
        public string OrderId { get; set; }
        public int Direction { get; set; }
        public decimal Price { get; set; }
    }
}
