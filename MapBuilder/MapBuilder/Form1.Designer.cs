namespace MapBuilder
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mapLoad1 = new System.Windows.Forms.Button();
            this.mapName1 = new System.Windows.Forms.TextBox();
            this.mapLoad2 = new System.Windows.Forms.Button();
            this.mapName2 = new System.Windows.Forms.TextBox();
            this.addToRightButton = new System.Windows.Forms.Button();
            this.addToLeftButton = new System.Windows.Forms.Button();
            this.addToTopButton = new System.Windows.Forms.Button();
            this.addToBottomButton = new System.Windows.Forms.Button();
            this.newMapName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // mapLoad1
            // 
            this.mapLoad1.Location = new System.Drawing.Point(28, 12);
            this.mapLoad1.Name = "mapLoad1";
            this.mapLoad1.Size = new System.Drawing.Size(128, 23);
            this.mapLoad1.TabIndex = 0;
            this.mapLoad1.Text = "Load First Map";
            this.mapLoad1.UseVisualStyleBackColor = true;
            this.mapLoad1.Click += new System.EventHandler(this.MapLoad1_Click);
            // 
            // mapName1
            // 
            this.mapName1.Location = new System.Drawing.Point(162, 13);
            this.mapName1.Name = "mapName1";
            this.mapName1.ReadOnly = true;
            this.mapName1.Size = new System.Drawing.Size(616, 23);
            this.mapName1.TabIndex = 1;
            // 
            // mapLoad2
            // 
            this.mapLoad2.Location = new System.Drawing.Point(28, 41);
            this.mapLoad2.Name = "mapLoad2";
            this.mapLoad2.Size = new System.Drawing.Size(128, 23);
            this.mapLoad2.TabIndex = 2;
            this.mapLoad2.Text = "Load Second Map";
            this.mapLoad2.UseVisualStyleBackColor = true;
            this.mapLoad2.Click += new System.EventHandler(this.MapLoad2_Click);
            // 
            // mapName2
            // 
            this.mapName2.Location = new System.Drawing.Point(162, 42);
            this.mapName2.Name = "mapName2";
            this.mapName2.ReadOnly = true;
            this.mapName2.Size = new System.Drawing.Size(616, 23);
            this.mapName2.TabIndex = 3;
            // 
            // addToRightButton
            // 
            this.addToRightButton.Location = new System.Drawing.Point(28, 71);
            this.addToRightButton.Name = "addToRightButton";
            this.addToRightButton.Size = new System.Drawing.Size(186, 23);
            this.addToRightButton.TabIndex = 4;
            this.addToRightButton.Text = "Attach Second Map to Right";
            this.addToRightButton.UseVisualStyleBackColor = true;
            this.addToRightButton.Click += new System.EventHandler(this.AddToRight);
            // 
            // addToLeftButton
            // 
            this.addToLeftButton.Location = new System.Drawing.Point(220, 71);
            this.addToLeftButton.Name = "addToLeftButton";
            this.addToLeftButton.Size = new System.Drawing.Size(182, 23);
            this.addToLeftButton.TabIndex = 5;
            this.addToLeftButton.Text = "Attach Second Map to Left";
            this.addToLeftButton.UseVisualStyleBackColor = true;
            this.addToLeftButton.Click += new System.EventHandler(this.AddToLeft);
            // 
            // addToTopButton
            // 
            this.addToTopButton.Location = new System.Drawing.Point(408, 71);
            this.addToTopButton.Name = "addToTopButton";
            this.addToTopButton.Size = new System.Drawing.Size(182, 23);
            this.addToTopButton.TabIndex = 6;
            this.addToTopButton.Text = "Attach Second Map to Top";
            this.addToTopButton.UseVisualStyleBackColor = true;
            this.addToTopButton.Click += new System.EventHandler(this.AddToTop);
            // 
            // addToBottomButton
            // 
            this.addToBottomButton.Location = new System.Drawing.Point(596, 71);
            this.addToBottomButton.Name = "addToBottomButton";
            this.addToBottomButton.Size = new System.Drawing.Size(182, 23);
            this.addToBottomButton.TabIndex = 7;
            this.addToBottomButton.Text = "Attach Second Map to Bottom";
            this.addToBottomButton.UseVisualStyleBackColor = true;
            this.addToBottomButton.Click += new System.EventHandler(this.AddToBottom);
            // 
            // newMapName
            // 
            this.newMapName.Location = new System.Drawing.Point(28, 126);
            this.newMapName.Name = "newMapName";
            this.newMapName.Size = new System.Drawing.Size(750, 23);
            this.newMapName.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 108);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(166, 15);
            this.label1.TabIndex = 9;
            this.label1.Text = "Type a name for the new map:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.newMapName);
            this.Controls.Add(this.addToBottomButton);
            this.Controls.Add(this.addToTopButton);
            this.Controls.Add(this.addToLeftButton);
            this.Controls.Add(this.addToRightButton);
            this.Controls.Add(this.mapName2);
            this.Controls.Add(this.mapLoad2);
            this.Controls.Add(this.mapName1);
            this.Controls.Add(this.mapLoad1);
            this.Name = "Form1";
            this.Text = "Map Builder";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button mapLoad1;
        private TextBox mapName1;
        private Button mapLoad2;
        private TextBox mapName2;
        private Button addToRightButton;
        private Button addToLeftButton;
        private Button addToTopButton;
        private Button addToBottomButton;
        private TextBox newMapName;
        private Label label1;
    }
}