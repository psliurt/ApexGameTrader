namespace ApexGameTrader
{
    partial class TradingBoardForm
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this._msgListBox = new System.Windows.Forms.ListBox();
            this.buyFutureBtn = new System.Windows.Forms.Button();
            this._sellFutureBtn = new System.Windows.Forms.Button();
            this._futureQtyTxt = new System.Windows.Forms.TextBox();
            this._apexFutureCodeTxt = new System.Windows.Forms.TextBox();
            this._futureCodeLbl = new System.Windows.Forms.Label();
            this._futureList = new System.Windows.Forms.ListBox();
            this._futureQty1 = new System.Windows.Forms.Button();
            this._futureQty2 = new System.Windows.Forms.Button();
            this._futureQty3 = new System.Windows.Forms.Button();
            this._futureQty4 = new System.Windows.Forms.Button();
            this._futureQty5 = new System.Windows.Forms.Button();
            this._optionCodeTxt = new System.Windows.Forms.TextBox();
            this._optionPriceTxt = new System.Windows.Forms.TextBox();
            this._optionCodeLbl = new System.Windows.Forms.Label();
            this._optionPriceLbl = new System.Windows.Forms.Label();
            this._optionQtyTxt = new System.Windows.Forms.TextBox();
            this._optionQtyLbl = new System.Windows.Forms.Label();
            this._buyOptionBtn = new System.Windows.Forms.Button();
            this._sellOptionBtn = new System.Windows.Forms.Button();
            this._buyEndOptionBtn = new System.Windows.Forms.Button();
            this._sellEndOptionBtn = new System.Windows.Forms.Button();
            this._futureDayTradeChk = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // _msgListBox
            // 
            this._msgListBox.FormattingEnabled = true;
            this._msgListBox.ItemHeight = 12;
            this._msgListBox.Location = new System.Drawing.Point(14, 189);
            this._msgListBox.Name = "_msgListBox";
            this._msgListBox.ScrollAlwaysVisible = true;
            this._msgListBox.Size = new System.Drawing.Size(936, 52);
            this._msgListBox.TabIndex = 0;
            // 
            // buyFutureBtn
            // 
            this.buyFutureBtn.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.buyFutureBtn.ForeColor = System.Drawing.Color.Red;
            this.buyFutureBtn.Location = new System.Drawing.Point(720, 9);
            this.buyFutureBtn.Name = "buyFutureBtn";
            this.buyFutureBtn.Size = new System.Drawing.Size(112, 55);
            this.buyFutureBtn.TabIndex = 1;
            this.buyFutureBtn.Text = "作多";
            this.buyFutureBtn.UseVisualStyleBackColor = true;
            this.buyFutureBtn.Click += new System.EventHandler(this.buyFutureBtn_Click);
            // 
            // _sellFutureBtn
            // 
            this._sellFutureBtn.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this._sellFutureBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this._sellFutureBtn.Location = new System.Drawing.Point(838, 9);
            this._sellFutureBtn.Name = "_sellFutureBtn";
            this._sellFutureBtn.Size = new System.Drawing.Size(112, 55);
            this._sellFutureBtn.TabIndex = 2;
            this._sellFutureBtn.Text = "作空";
            this._sellFutureBtn.UseVisualStyleBackColor = true;
            this._sellFutureBtn.Click += new System.EventHandler(this._sellFutureBtn_Click);
            // 
            // _futureQtyTxt
            // 
            this._futureQtyTxt.Font = new System.Drawing.Font("新細明體", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this._futureQtyTxt.Location = new System.Drawing.Point(659, 18);
            this._futureQtyTxt.Name = "_futureQtyTxt";
            this._futureQtyTxt.Size = new System.Drawing.Size(55, 40);
            this._futureQtyTxt.TabIndex = 3;
            this._futureQtyTxt.Text = "10";
            // 
            // _apexFutureCodeTxt
            // 
            this._apexFutureCodeTxt.Location = new System.Drawing.Point(140, 39);
            this._apexFutureCodeTxt.Name = "_apexFutureCodeTxt";
            this._apexFutureCodeTxt.Size = new System.Drawing.Size(98, 22);
            this._apexFutureCodeTxt.TabIndex = 4;
            // 
            // _futureCodeLbl
            // 
            this._futureCodeLbl.Location = new System.Drawing.Point(138, 9);
            this._futureCodeLbl.Name = "_futureCodeLbl";
            this._futureCodeLbl.Size = new System.Drawing.Size(100, 20);
            this._futureCodeLbl.TabIndex = 5;
            this._futureCodeLbl.Text = "期貨代碼";
            this._futureCodeLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _futureList
            // 
            this._futureList.FormattingEnabled = true;
            this._futureList.ItemHeight = 12;
            this._futureList.Items.AddRange(new object[] {
            "TXFK8.tw-11",
            "MXFK8.tw-小11"});
            this._futureList.Location = new System.Drawing.Point(12, 9);
            this._futureList.Name = "_futureList";
            this._futureList.Size = new System.Drawing.Size(120, 52);
            this._futureList.TabIndex = 6;
            this._futureList.SelectedIndexChanged += new System.EventHandler(this._futureList_SelectedIndexChanged);
            // 
            // _futureQty1
            // 
            this._futureQty1.Font = new System.Drawing.Font("新細明體", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this._futureQty1.Location = new System.Drawing.Point(352, 10);
            this._futureQty1.Name = "_futureQty1";
            this._futureQty1.Size = new System.Drawing.Size(55, 50);
            this._futureQty1.TabIndex = 7;
            this._futureQty1.Text = "1";
            this._futureQty1.UseVisualStyleBackColor = true;
            this._futureQty1.Click += new System.EventHandler(this._futureQty1_Click);
            // 
            // _futureQty2
            // 
            this._futureQty2.Font = new System.Drawing.Font("新細明體", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this._futureQty2.Location = new System.Drawing.Point(413, 10);
            this._futureQty2.Name = "_futureQty2";
            this._futureQty2.Size = new System.Drawing.Size(55, 50);
            this._futureQty2.TabIndex = 8;
            this._futureQty2.Text = "2";
            this._futureQty2.UseVisualStyleBackColor = true;
            this._futureQty2.Click += new System.EventHandler(this._futureQty2_Click);
            // 
            // _futureQty3
            // 
            this._futureQty3.Font = new System.Drawing.Font("新細明體", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this._futureQty3.Location = new System.Drawing.Point(474, 9);
            this._futureQty3.Name = "_futureQty3";
            this._futureQty3.Size = new System.Drawing.Size(55, 51);
            this._futureQty3.TabIndex = 9;
            this._futureQty3.Text = "3";
            this._futureQty3.UseVisualStyleBackColor = true;
            this._futureQty3.Click += new System.EventHandler(this._futureQty3_Click);
            // 
            // _futureQty4
            // 
            this._futureQty4.Font = new System.Drawing.Font("新細明體", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this._futureQty4.Location = new System.Drawing.Point(535, 9);
            this._futureQty4.Name = "_futureQty4";
            this._futureQty4.Size = new System.Drawing.Size(55, 51);
            this._futureQty4.TabIndex = 10;
            this._futureQty4.Text = "4";
            this._futureQty4.UseVisualStyleBackColor = true;
            this._futureQty4.Click += new System.EventHandler(this._futureQty4_Click);
            // 
            // _futureQty5
            // 
            this._futureQty5.Font = new System.Drawing.Font("新細明體", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this._futureQty5.Location = new System.Drawing.Point(596, 10);
            this._futureQty5.Name = "_futureQty5";
            this._futureQty5.Size = new System.Drawing.Size(57, 51);
            this._futureQty5.TabIndex = 11;
            this._futureQty5.Text = "5";
            this._futureQty5.UseVisualStyleBackColor = true;
            this._futureQty5.Click += new System.EventHandler(this._futureQty5_Click);
            // 
            // _optionCodeTxt
            // 
            this._optionCodeTxt.Location = new System.Drawing.Point(13, 129);
            this._optionCodeTxt.Name = "_optionCodeTxt";
            this._optionCodeTxt.Size = new System.Drawing.Size(119, 22);
            this._optionCodeTxt.TabIndex = 12;
            // 
            // _optionPriceTxt
            // 
            this._optionPriceTxt.Location = new System.Drawing.Point(140, 129);
            this._optionPriceTxt.Name = "_optionPriceTxt";
            this._optionPriceTxt.Size = new System.Drawing.Size(100, 22);
            this._optionPriceTxt.TabIndex = 13;
            // 
            // _optionCodeLbl
            // 
            this._optionCodeLbl.Location = new System.Drawing.Point(12, 106);
            this._optionCodeLbl.Name = "_optionCodeLbl";
            this._optionCodeLbl.Size = new System.Drawing.Size(100, 20);
            this._optionCodeLbl.TabIndex = 14;
            this._optionCodeLbl.Text = "OP代碼";
            this._optionCodeLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _optionPriceLbl
            // 
            this._optionPriceLbl.Location = new System.Drawing.Point(138, 106);
            this._optionPriceLbl.Name = "_optionPriceLbl";
            this._optionPriceLbl.Size = new System.Drawing.Size(100, 20);
            this._optionPriceLbl.TabIndex = 15;
            this._optionPriceLbl.Text = "OP價格";
            this._optionPriceLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _optionQtyTxt
            // 
            this._optionQtyTxt.Location = new System.Drawing.Point(247, 129);
            this._optionQtyTxt.Name = "_optionQtyTxt";
            this._optionQtyTxt.Size = new System.Drawing.Size(100, 22);
            this._optionQtyTxt.TabIndex = 16;
            // 
            // _optionQtyLbl
            // 
            this._optionQtyLbl.Location = new System.Drawing.Point(247, 106);
            this._optionQtyLbl.Name = "_optionQtyLbl";
            this._optionQtyLbl.Size = new System.Drawing.Size(100, 20);
            this._optionQtyLbl.TabIndex = 17;
            this._optionQtyLbl.Text = "OP口數";
            this._optionQtyLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _buyOptionBtn
            // 
            this._buyOptionBtn.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this._buyOptionBtn.ForeColor = System.Drawing.Color.Red;
            this._buyOptionBtn.Location = new System.Drawing.Point(366, 92);
            this._buyOptionBtn.Name = "_buyOptionBtn";
            this._buyOptionBtn.Size = new System.Drawing.Size(125, 34);
            this._buyOptionBtn.TabIndex = 18;
            this._buyOptionBtn.Text = "買新";
            this._buyOptionBtn.UseVisualStyleBackColor = true;
            this._buyOptionBtn.Click += new System.EventHandler(this._buyOptionBtn_Click);
            // 
            // _sellOptionBtn
            // 
            this._sellOptionBtn.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this._sellOptionBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this._sellOptionBtn.Location = new System.Drawing.Point(366, 132);
            this._sellOptionBtn.Name = "_sellOptionBtn";
            this._sellOptionBtn.Size = new System.Drawing.Size(125, 34);
            this._sellOptionBtn.TabIndex = 19;
            this._sellOptionBtn.Text = "賣新";
            this._sellOptionBtn.UseVisualStyleBackColor = true;
            this._sellOptionBtn.Click += new System.EventHandler(this._sellOptionBtn_Click);
            // 
            // _buyEndOptionBtn
            // 
            this._buyEndOptionBtn.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this._buyEndOptionBtn.ForeColor = System.Drawing.Color.Red;
            this._buyEndOptionBtn.Location = new System.Drawing.Point(499, 132);
            this._buyEndOptionBtn.Name = "_buyEndOptionBtn";
            this._buyEndOptionBtn.Size = new System.Drawing.Size(125, 34);
            this._buyEndOptionBtn.TabIndex = 20;
            this._buyEndOptionBtn.Text = "買平";
            this._buyEndOptionBtn.UseVisualStyleBackColor = true;
            this._buyEndOptionBtn.Click += new System.EventHandler(this._buyEndOptionBtn_Click);
            // 
            // _sellEndOptionBtn
            // 
            this._sellEndOptionBtn.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this._sellEndOptionBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this._sellEndOptionBtn.Location = new System.Drawing.Point(499, 92);
            this._sellEndOptionBtn.Name = "_sellEndOptionBtn";
            this._sellEndOptionBtn.Size = new System.Drawing.Size(125, 34);
            this._sellEndOptionBtn.TabIndex = 21;
            this._sellEndOptionBtn.Text = "賣平";
            this._sellEndOptionBtn.UseVisualStyleBackColor = true;
            this._sellEndOptionBtn.Click += new System.EventHandler(this._sellEndOptionBtn_Click);
            // 
            // _futureDayTradeChk
            // 
            this._futureDayTradeChk.Checked = true;
            this._futureDayTradeChk.CheckState = System.Windows.Forms.CheckState.Checked;
            this._futureDayTradeChk.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this._futureDayTradeChk.Location = new System.Drawing.Point(266, 17);
            this._futureDayTradeChk.Name = "_futureDayTradeChk";
            this._futureDayTradeChk.Size = new System.Drawing.Size(80, 40);
            this._futureDayTradeChk.TabIndex = 22;
            this._futureDayTradeChk.Text = "當沖";
            this._futureDayTradeChk.UseVisualStyleBackColor = true;
            // 
            // TradingBoardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(962, 250);
            this.Controls.Add(this._futureDayTradeChk);
            this.Controls.Add(this._sellEndOptionBtn);
            this.Controls.Add(this._buyEndOptionBtn);
            this.Controls.Add(this._sellOptionBtn);
            this.Controls.Add(this._buyOptionBtn);
            this.Controls.Add(this._optionQtyLbl);
            this.Controls.Add(this._optionQtyTxt);
            this.Controls.Add(this._optionPriceLbl);
            this.Controls.Add(this._optionCodeLbl);
            this.Controls.Add(this._optionPriceTxt);
            this.Controls.Add(this._optionCodeTxt);
            this.Controls.Add(this._futureQty5);
            this.Controls.Add(this._futureQty4);
            this.Controls.Add(this._futureQty3);
            this.Controls.Add(this._futureQty2);
            this.Controls.Add(this._futureQty1);
            this.Controls.Add(this._futureList);
            this.Controls.Add(this._futureCodeLbl);
            this.Controls.Add(this._apexFutureCodeTxt);
            this.Controls.Add(this._futureQtyTxt);
            this.Controls.Add(this._sellFutureBtn);
            this.Controls.Add(this.buyFutureBtn);
            this.Controls.Add(this._msgListBox);
            this.Name = "TradingBoardForm";
            this.Text = "TradingBoardForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TradingBoardForm_FormClosing);
            this.Load += new System.EventHandler(this.TradingBoardForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox _msgListBox;
        private System.Windows.Forms.Button buyFutureBtn;
        private System.Windows.Forms.Button _sellFutureBtn;
        private System.Windows.Forms.TextBox _futureQtyTxt;
        private System.Windows.Forms.TextBox _apexFutureCodeTxt;
        private System.Windows.Forms.Label _futureCodeLbl;
        private System.Windows.Forms.ListBox _futureList;
        private System.Windows.Forms.Button _futureQty1;
        private System.Windows.Forms.Button _futureQty2;
        private System.Windows.Forms.Button _futureQty3;
        private System.Windows.Forms.Button _futureQty4;
        private System.Windows.Forms.Button _futureQty5;
        private System.Windows.Forms.TextBox _optionCodeTxt;
        private System.Windows.Forms.TextBox _optionPriceTxt;
        private System.Windows.Forms.Label _optionCodeLbl;
        private System.Windows.Forms.Label _optionPriceLbl;
        private System.Windows.Forms.TextBox _optionQtyTxt;
        private System.Windows.Forms.Label _optionQtyLbl;
        private System.Windows.Forms.Button _buyOptionBtn;
        private System.Windows.Forms.Button _sellOptionBtn;
        private System.Windows.Forms.Button _buyEndOptionBtn;
        private System.Windows.Forms.Button _sellEndOptionBtn;
        private System.Windows.Forms.CheckBox _futureDayTradeChk;
    }
}

