namespace Exercise3
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.mainChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.functionTextBox = new System.Windows.Forms.TextBox();
            this.applyButton = new System.Windows.Forms.Button();
            this.xMinTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.xMaxTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.yMinTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.yMaxTextBox = new System.Windows.Forms.TextBox();
            this.changeTypeButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.stepTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.mainChart)).BeginInit();
            this.SuspendLayout();
            // 
            // mainChart
            // 
            chartArea4.Name = "ChartArea1";
            this.mainChart.ChartAreas.Add(chartArea4);
            legend4.Name = "Legend1";
            this.mainChart.Legends.Add(legend4);
            this.mainChart.Location = new System.Drawing.Point(12, 12);
            this.mainChart.Name = "mainChart";
            series4.ChartArea = "ChartArea1";
            series4.Legend = "Legend1";
            series4.Name = "Series1";
            this.mainChart.Series.Add(series4);
            this.mainChart.Size = new System.Drawing.Size(776, 426);
            this.mainChart.TabIndex = 0;
            this.mainChart.Text = "chart1";
            // 
            // functionTextBox
            // 
            this.functionTextBox.Location = new System.Drawing.Point(795, 13);
            this.functionTextBox.Multiline = true;
            this.functionTextBox.Name = "functionTextBox";
            this.functionTextBox.Size = new System.Drawing.Size(288, 152);
            this.functionTextBox.TabIndex = 1;
            this.functionTextBox.Text = "x";
            // 
            // applyButton
            // 
            this.applyButton.Location = new System.Drawing.Point(1008, 348);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(75, 23);
            this.applyButton.TabIndex = 2;
            this.applyButton.Text = "Apply";
            this.applyButton.UseVisualStyleBackColor = true;
            this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
            // 
            // xMinTextBox
            // 
            this.xMinTextBox.Location = new System.Drawing.Point(831, 180);
            this.xMinTextBox.Name = "xMinTextBox";
            this.xMinTextBox.Size = new System.Drawing.Size(100, 20);
            this.xMinTextBox.TabIndex = 3;
            this.xMinTextBox.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(794, 183);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "x min";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(794, 209);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "x max";
            // 
            // xMaxTextBox
            // 
            this.xMaxTextBox.Location = new System.Drawing.Point(831, 206);
            this.xMaxTextBox.Name = "xMaxTextBox";
            this.xMaxTextBox.Size = new System.Drawing.Size(100, 20);
            this.xMaxTextBox.TabIndex = 8;
            this.xMaxTextBox.Text = "20";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(946, 183);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "y min";
            // 
            // yMinTextBox
            // 
            this.yMinTextBox.Location = new System.Drawing.Point(983, 180);
            this.yMinTextBox.Name = "yMinTextBox";
            this.yMinTextBox.Size = new System.Drawing.Size(100, 20);
            this.yMinTextBox.TabIndex = 10;
            this.yMinTextBox.Text = "-10";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(946, 209);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "y max";
            // 
            // yMaxTextBox
            // 
            this.yMaxTextBox.Location = new System.Drawing.Point(983, 206);
            this.yMaxTextBox.Name = "yMaxTextBox";
            this.yMaxTextBox.Size = new System.Drawing.Size(100, 20);
            this.yMaxTextBox.TabIndex = 12;
            this.yMaxTextBox.Text = "10";
            // 
            // changeTypeButton
            // 
            this.changeTypeButton.Location = new System.Drawing.Point(1008, 378);
            this.changeTypeButton.Name = "changeTypeButton";
            this.changeTypeButton.Size = new System.Drawing.Size(75, 23);
            this.changeTypeButton.TabIndex = 14;
            this.changeTypeButton.Text = "Change type";
            this.changeTypeButton.UseVisualStyleBackColor = true;
            this.changeTypeButton.Click += new System.EventHandler(this.changeTypeButton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(796, 269);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(27, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "step";
            // 
            // stepTextBox
            // 
            this.stepTextBox.Location = new System.Drawing.Point(831, 266);
            this.stepTextBox.Name = "stepTextBox";
            this.stepTextBox.Size = new System.Drawing.Size(100, 20);
            this.stepTextBox.TabIndex = 16;
            this.stepTextBox.Text = "0,1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1095, 450);
            this.Controls.Add(this.stepTextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.changeTypeButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.yMaxTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.yMinTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.xMaxTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.xMinTextBox);
            this.Controls.Add(this.applyButton);
            this.Controls.Add(this.functionTextBox);
            this.Controls.Add(this.mainChart);
            this.Name = "Form1";
            this.Text = "Exercise 3";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.mainChart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart mainChart;
        private System.Windows.Forms.TextBox functionTextBox;
        private System.Windows.Forms.Button applyButton;
        private System.Windows.Forms.TextBox xMinTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox xMaxTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox yMinTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox yMaxTextBox;
        private System.Windows.Forms.Button changeTypeButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox stepTextBox;
    }
}

