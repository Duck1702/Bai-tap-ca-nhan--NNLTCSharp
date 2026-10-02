namespace SecurityPanelApp;

partial class FrmSecurityPanel
{
    private System.ComponentModel.IContainer components = null;

    private Label lblSecurityCode;
    private TextBox txtSecurityCode;

    private Button btn1;
    private Button btn2;
    private Button btn3;
    private Button btn4;
    private Button btn5;
    private Button btn6;
    private Button btn7;
    private Button btn8;
    private Button btn9;
    private Button btn0;

    private Button btnClear;
    private Button btnEnter;

    private Label lblAccessLog;
    private ListBox lbxAccessLog;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        lblSecurityCode = new Label();
        txtSecurityCode = new TextBox();

        btn1 = new Button();
        btn2 = new Button();
        btn3 = new Button();
        btn4 = new Button();
        btn5 = new Button();
        btn6 = new Button();
        btn7 = new Button();
        btn8 = new Button();
        btn9 = new Button();
        btn0 = new Button();

        btnClear = new Button();
        btnEnter = new Button();

        lblAccessLog = new Label();
        lbxAccessLog = new ListBox();

        SuspendLayout();

        // lblSecurityCode
        lblSecurityCode.AutoSize = true;
        lblSecurityCode.Location = new Point(35, 30);
        lblSecurityCode.Name = "lblSecurityCode";
        lblSecurityCode.Size = new Size(100, 20);
        lblSecurityCode.TabIndex = 0;
        lblSecurityCode.Text = "Security code";

        // txtSecurityCode
        txtSecurityCode.Location = new Point(150, 27);
        txtSecurityCode.Name = "txtSecurityCode";
        txtSecurityCode.PasswordChar = '*';
        txtSecurityCode.ReadOnly = true;
        txtSecurityCode.Size = new Size(145, 27);
        txtSecurityCode.TabIndex = 1;
        txtSecurityCode.TextAlign = HorizontalAlignment.Center;

        // btn1
        btn1.Location = new Point(65, 85);
        btn1.Name = "btn1";
        btn1.Size = new Size(50, 40);
        btn1.Text = "1";
        btn1.UseVisualStyleBackColor = true;
        btn1.Click += NumberButton_Click;

        // btn2
        btn2.Location = new Point(125, 85);
        btn2.Name = "btn2";
        btn2.Size = new Size(50, 40);
        btn2.Text = "2";
        btn2.UseVisualStyleBackColor = true;
        btn2.Click += NumberButton_Click;

        // btn3
        btn3.Location = new Point(185, 85);
        btn3.Name = "btn3";
        btn3.Size = new Size(50, 40);
        btn3.Text = "3";
        btn3.UseVisualStyleBackColor = true;
        btn3.Click += NumberButton_Click;

        // btnClear
        btnClear.Location = new Point(245, 85);
        btnClear.Name = "btnClear";
        btnClear.Size = new Size(50, 40);
        btnClear.Text = "C";
        btnClear.UseVisualStyleBackColor = true;
        btnClear.Click += btnClear_Click;

        // btn4
        btn4.Location = new Point(65, 135);
        btn4.Name = "btn4";
        btn4.Size = new Size(50, 40);
        btn4.Text = "4";
        btn4.UseVisualStyleBackColor = true;
        btn4.Click += NumberButton_Click;

        // btn5
        btn5.Location = new Point(125, 135);
        btn5.Name = "btn5";
        btn5.Size = new Size(50, 40);
        btn5.Text = "5";
        btn5.UseVisualStyleBackColor = true;
        btn5.Click += NumberButton_Click;

        // btn6
        btn6.Location = new Point(185, 135);
        btn6.Name = "btn6";
        btn6.Size = new Size(50, 40);
        btn6.Text = "6";
        btn6.UseVisualStyleBackColor = true;
        btn6.Click += NumberButton_Click;

        // btnEnter
        btnEnter.Location = new Point(245, 135);
        btnEnter.Name = "btnEnter";
        btnEnter.Size = new Size(50, 90);
        btnEnter.Text = "OK";
        btnEnter.UseVisualStyleBackColor = true;
        btnEnter.Click += btnEnter_Click;

        // btn7
        btn7.Location = new Point(65, 185);
        btn7.Name = "btn7";
        btn7.Size = new Size(50, 40);
        btn7.Text = "7";
        btn7.UseVisualStyleBackColor = true;
        btn7.Click += NumberButton_Click;

        // btn8
        btn8.Location = new Point(125, 185);
        btn8.Name = "btn8";
        btn8.Size = new Size(50, 40);
        btn8.Text = "8";
        btn8.UseVisualStyleBackColor = true;
        btn8.Click += NumberButton_Click;

        // btn9
        btn9.Location = new Point(185, 185);
        btn9.Name = "btn9";
        btn9.Size = new Size(50, 40);
        btn9.Text = "9";
        btn9.UseVisualStyleBackColor = true;
        btn9.Click += NumberButton_Click;

        // btn0
        btn0.Location = new Point(125, 235);
        btn0.Name = "btn0";
        btn0.Size = new Size(50, 40);
        btn0.Text = "0";
        btn0.UseVisualStyleBackColor = true;
        btn0.Click += NumberButton_Click;

        // lblAccessLog
        lblAccessLog.AutoSize = true;
        lblAccessLog.Location = new Point(35, 305);
        lblAccessLog.Name = "lblAccessLog";
        lblAccessLog.Size = new Size(82, 20);
        lblAccessLog.TabIndex = 20;
        lblAccessLog.Text = "Access log:";

        // lbxAccessLog
        lbxAccessLog.FormattingEnabled = true;
        lbxAccessLog.HorizontalScrollbar = true;
        lbxAccessLog.ItemHeight = 20;
        lbxAccessLog.Location = new Point(35, 335);
        lbxAccessLog.Name = "lbxAccessLog";
        lbxAccessLog.Size = new Size(300, 144);
        lbxAccessLog.TabIndex = 21;

        // FrmSecurityPanel
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;

        ClientSize = new Size(380, 520);

        Controls.Add(lblSecurityCode);
        Controls.Add(txtSecurityCode);

        Controls.Add(btn1);
        Controls.Add(btn2);
        Controls.Add(btn3);
        Controls.Add(btn4);
        Controls.Add(btn5);
        Controls.Add(btn6);
        Controls.Add(btn7);
        Controls.Add(btn8);
        Controls.Add(btn9);
        Controls.Add(btn0);

        Controls.Add(btnClear);
        Controls.Add(btnEnter);

        Controls.Add(lblAccessLog);
        Controls.Add(lbxAccessLog);

        Name = "FrmSecurityPanel";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Security Panel";

        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}