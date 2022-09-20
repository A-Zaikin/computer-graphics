namespace Exercise1
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.showAllButton = new System.Windows.Forms.Button();
            this.hideAllButton = new System.Windows.Forms.Button();
            this.circleRadiusTextBox = new System.Windows.Forms.TextBox();
            this.drawCircleButton = new System.Windows.Forms.Button();
            this.hideLastCircleButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.circleCenterXTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.circleCenterYTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.circleWidthTextBox = new System.Windows.Forms.TextBox();
            this.drawPolygonButton = new System.Windows.Forms.Button();
            this.hideLastPolygonButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.polygonRadiusTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.polygonCenterXTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.polygonCenterYTextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.polygonWidthTextBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.polygonEdgesTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox.Location = new System.Drawing.Point(13, 13);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(649, 789);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // showAllButton
            // 
            this.showAllButton.Location = new System.Drawing.Point(678, 13);
            this.showAllButton.Name = "showAllButton";
            this.showAllButton.Size = new System.Drawing.Size(75, 23);
            this.showAllButton.TabIndex = 1;
            this.showAllButton.Text = "draw all";
            this.showAllButton.UseVisualStyleBackColor = true;
            this.showAllButton.Click += new System.EventHandler(this.showAllButton_Click);
            // 
            // hideAllButton
            // 
            this.hideAllButton.Location = new System.Drawing.Point(678, 42);
            this.hideAllButton.Name = "hideAllButton";
            this.hideAllButton.Size = new System.Drawing.Size(75, 23);
            this.hideAllButton.TabIndex = 2;
            this.hideAllButton.Text = "hide all";
            this.hideAllButton.UseVisualStyleBackColor = true;
            this.hideAllButton.Click += new System.EventHandler(this.hideAllButton_Click);
            // 
            // circleRadiusTextBox
            // 
            this.circleRadiusTextBox.Location = new System.Drawing.Point(726, 143);
            this.circleRadiusTextBox.Name = "circleRadiusTextBox";
            this.circleRadiusTextBox.Size = new System.Drawing.Size(100, 20);
            this.circleRadiusTextBox.TabIndex = 3;
            this.circleRadiusTextBox.Text = "75";
            // 
            // drawCircleButton
            // 
            this.drawCircleButton.Location = new System.Drawing.Point(678, 111);
            this.drawCircleButton.Name = "drawCircleButton";
            this.drawCircleButton.Size = new System.Drawing.Size(75, 23);
            this.drawCircleButton.TabIndex = 5;
            this.drawCircleButton.Text = "draw circle";
            this.drawCircleButton.UseVisualStyleBackColor = true;
            this.drawCircleButton.Click += new System.EventHandler(this.drawCircleButton_Click);
            // 
            // hideLastCircleButton
            // 
            this.hideLastCircleButton.Location = new System.Drawing.Point(759, 111);
            this.hideLastCircleButton.Name = "hideLastCircleButton";
            this.hideLastCircleButton.Size = new System.Drawing.Size(95, 23);
            this.hideLastCircleButton.TabIndex = 6;
            this.hideLastCircleButton.Text = "hide last circle";
            this.hideLastCircleButton.UseVisualStyleBackColor = true;
            this.hideLastCircleButton.Click += new System.EventHandler(this.hideLastCircleButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(675, 146);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "radius";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(675, 172);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "center x";
            // 
            // circleCenterXTextBox
            // 
            this.circleCenterXTextBox.Location = new System.Drawing.Point(726, 169);
            this.circleCenterXTextBox.Name = "circleCenterXTextBox";
            this.circleCenterXTextBox.Size = new System.Drawing.Size(100, 20);
            this.circleCenterXTextBox.TabIndex = 10;
            this.circleCenterXTextBox.Text = "200";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(675, 198);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "center y";
            // 
            // circleCenterYTextBox
            // 
            this.circleCenterYTextBox.Location = new System.Drawing.Point(726, 195);
            this.circleCenterYTextBox.Name = "circleCenterYTextBox";
            this.circleCenterYTextBox.Size = new System.Drawing.Size(100, 20);
            this.circleCenterYTextBox.TabIndex = 12;
            this.circleCenterYTextBox.Text = "100";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(675, 224);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "width";
            // 
            // circleWidthTextBox
            // 
            this.circleWidthTextBox.Location = new System.Drawing.Point(726, 221);
            this.circleWidthTextBox.Name = "circleWidthTextBox";
            this.circleWidthTextBox.Size = new System.Drawing.Size(100, 20);
            this.circleWidthTextBox.TabIndex = 14;
            this.circleWidthTextBox.Text = "5";
            // 
            // drawPolygonButton
            // 
            this.drawPolygonButton.Location = new System.Drawing.Point(678, 316);
            this.drawPolygonButton.Name = "drawPolygonButton";
            this.drawPolygonButton.Size = new System.Drawing.Size(128, 23);
            this.drawPolygonButton.TabIndex = 16;
            this.drawPolygonButton.Text = "draw regular polygon";
            this.drawPolygonButton.UseVisualStyleBackColor = true;
            this.drawPolygonButton.Click += new System.EventHandler(this.drawPolygonButton_Click);
            // 
            // hideLastPolygonButton
            // 
            this.hideLastPolygonButton.Location = new System.Drawing.Point(812, 316);
            this.hideLastPolygonButton.Name = "hideLastPolygonButton";
            this.hideLastPolygonButton.Size = new System.Drawing.Size(109, 23);
            this.hideLastPolygonButton.TabIndex = 17;
            this.hideLastPolygonButton.Text = "hide last polygon";
            this.hideLastPolygonButton.UseVisualStyleBackColor = true;
            this.hideLastPolygonButton.Click += new System.EventHandler(this.hideLastPolygonButton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(675, 348);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "radius";
            // 
            // polygonRadiusTextBox
            // 
            this.polygonRadiusTextBox.Location = new System.Drawing.Point(726, 345);
            this.polygonRadiusTextBox.Name = "polygonRadiusTextBox";
            this.polygonRadiusTextBox.Size = new System.Drawing.Size(100, 20);
            this.polygonRadiusTextBox.TabIndex = 18;
            this.polygonRadiusTextBox.Text = "75";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(675, 374);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "center x";
            // 
            // polygonCenterXTextBox
            // 
            this.polygonCenterXTextBox.Location = new System.Drawing.Point(726, 371);
            this.polygonCenterXTextBox.Name = "polygonCenterXTextBox";
            this.polygonCenterXTextBox.Size = new System.Drawing.Size(100, 20);
            this.polygonCenterXTextBox.TabIndex = 20;
            this.polygonCenterXTextBox.Text = "300";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(675, 400);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(45, 13);
            this.label7.TabIndex = 23;
            this.label7.Text = "center y";
            // 
            // polygonCenterYTextBox
            // 
            this.polygonCenterYTextBox.Location = new System.Drawing.Point(726, 397);
            this.polygonCenterYTextBox.Name = "polygonCenterYTextBox";
            this.polygonCenterYTextBox.Size = new System.Drawing.Size(100, 20);
            this.polygonCenterYTextBox.TabIndex = 22;
            this.polygonCenterYTextBox.Text = "300";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(675, 426);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(32, 13);
            this.label8.TabIndex = 25;
            this.label8.Text = "width";
            // 
            // polygonWidthTextBox
            // 
            this.polygonWidthTextBox.Location = new System.Drawing.Point(726, 423);
            this.polygonWidthTextBox.Name = "polygonWidthTextBox";
            this.polygonWidthTextBox.Size = new System.Drawing.Size(100, 20);
            this.polygonWidthTextBox.TabIndex = 24;
            this.polygonWidthTextBox.Text = "15";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(675, 452);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(36, 13);
            this.label9.TabIndex = 27;
            this.label9.Text = "edges";
            // 
            // polygonEdgesTextBox
            // 
            this.polygonEdgesTextBox.Location = new System.Drawing.Point(726, 449);
            this.polygonEdgesTextBox.Name = "polygonEdgesTextBox";
            this.polygonEdgesTextBox.Size = new System.Drawing.Size(100, 20);
            this.polygonEdgesTextBox.TabIndex = 26;
            this.polygonEdgesTextBox.Text = "7";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 814);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.polygonEdgesTextBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.polygonWidthTextBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.polygonCenterYTextBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.polygonCenterXTextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.polygonRadiusTextBox);
            this.Controls.Add(this.hideLastPolygonButton);
            this.Controls.Add(this.drawPolygonButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.circleWidthTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.circleCenterYTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.circleCenterXTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.hideLastCircleButton);
            this.Controls.Add(this.drawCircleButton);
            this.Controls.Add(this.circleRadiusTextBox);
            this.Controls.Add(this.hideAllButton);
            this.Controls.Add(this.showAllButton);
            this.Controls.Add(this.pictureBox);
            this.Name = "MainForm";
            this.Text = "Exercise 1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button showAllButton;
        private System.Windows.Forms.Button hideAllButton;
        private System.Windows.Forms.TextBox circleRadiusTextBox;
        private System.Windows.Forms.Button drawCircleButton;
        private System.Windows.Forms.Button hideLastCircleButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox circleCenterXTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox circleCenterYTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox circleWidthTextBox;
        private System.Windows.Forms.Button drawPolygonButton;
        private System.Windows.Forms.Button hideLastPolygonButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox polygonRadiusTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox polygonCenterXTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox polygonCenterYTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox polygonWidthTextBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox polygonEdgesTextBox;
    }
}

