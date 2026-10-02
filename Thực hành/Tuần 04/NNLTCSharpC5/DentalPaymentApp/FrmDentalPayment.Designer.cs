namespace DentalPaymentApp;

partial class FrmDentalPayment
{
    private System.ComponentModel.IContainer components = null;

    private Label lblTitle;
    private Label lblName;
    private TextBox txtName;

    private CheckBox chkClean;
    private Label lblCleanCost;

    private CheckBox chkWhitening;
    private Label lblWhiteningCost;

    private CheckBox chkXRay;
    private Label lblXRayCost;

    private Label lblFilling;
    private NumericUpDown numFilling;
    private Label lblFillCost;

    private Label lblTotal;
    private TextBox txtTotal;

    private Button btnExit;
    private Button btnCalc;

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
        lblTitle = new Label();
        lblName = new Label();
        txtName = new TextBox();

        chkClean = new CheckBox();
        lblCleanCost = new Label();

        chkWhitening = new CheckBox();
        lblWhiteningCost = new Label();

        chkXRay = new CheckBox();
        lblXRayCost = new Label();

        lblFilling = new Label();
        numFilling = new NumericUpDown();
        lblFillCost = new Label();

        lblTotal = new Label();
        txtTotal = new TextBox();

        btnExit = new Button();
        btnCalc = new Button();

        ((System.ComponentModel.ISupportInitialize)numFilling).BeginInit();

        SuspendLayout();

        // lblTitle
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font(
            "Arial",
            18F,
            FontStyle.Bold,
            GraphicsUnit.Point
        );
        lblTitle.ForeColor = Color.Blue;
        lblTitle.Location = new Point(85, 25);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(250, 29);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "Dental Payment Form";

        // lblName
        lblName.AutoSize = true;
        lblName.Location = new Point(45, 85);
        lblName.Name = "lblName";
        lblName.Size = new Size(95, 20);
        lblName.TabIndex = 1;
        lblName.Text = "Tên khách hàng";

        // txtName
        txtName.Location = new Point(160, 82);
        txtName.Name = "txtName";
        txtName.Size = new Size(210, 27);
        txtName.TabIndex = 2;

        // chkClean
        chkClean.AutoSize = true;
        chkClean.Location = new Point(45, 135);
        chkClean.Name = "chkClean";
        chkClean.Size = new Size(75, 24);
        chkClean.TabIndex = 3;
        chkClean.Text = "Cạo vôi";
        chkClean.UseVisualStyleBackColor = true;

        // lblCleanCost
        lblCleanCost.AutoSize = true;
        lblCleanCost.Location = new Point(285, 137);
        lblCleanCost.Name = "lblCleanCost";
        lblCleanCost.Size = new Size(68, 20);
        lblCleanCost.TabIndex = 4;
        lblCleanCost.Text = "$100000";

        // chkWhitening
        chkWhitening.AutoSize = true;
        chkWhitening.Location = new Point(45, 175);
        chkWhitening.Name = "chkWhitening";
        chkWhitening.Size = new Size(84, 24);
        chkWhitening.TabIndex = 5;
        chkWhitening.Text = "Tẩy trắng";
        chkWhitening.UseVisualStyleBackColor = true;

        // lblWhiteningCost
        lblWhiteningCost.AutoSize = true;
        lblWhiteningCost.Location = new Point(275, 177);
        lblWhiteningCost.Name = "lblWhiteningCost";
        lblWhiteningCost.Size = new Size(82, 20);
        lblWhiteningCost.TabIndex = 6;
        lblWhiteningCost.Text = "$1200000";

        // chkXRay
        chkXRay.AutoSize = true;
        chkXRay.Location = new Point(45, 215);
        chkXRay.Name = "chkXRay";
        chkXRay.Size = new Size(125, 24);
        chkXRay.TabIndex = 7;
        chkXRay.Text = "Chụp hình răng";
        chkXRay.UseVisualStyleBackColor = true;

        // lblXRayCost
        lblXRayCost.AutoSize = true;
        lblXRayCost.Location = new Point(285, 217);
        lblXRayCost.Name = "lblXRayCost";
        lblXRayCost.Size = new Size(68, 20);
        lblXRayCost.TabIndex = 8;
        lblXRayCost.Text = "$200000";

        // lblFilling
        lblFilling.AutoSize = true;
        lblFilling.Location = new Point(45, 260);
        lblFilling.Name = "lblFilling";
        lblFilling.Size = new Size(72, 20);
        lblFilling.TabIndex = 9;
        lblFilling.Text = "Trám răng";

        // numFilling
        numFilling.Location = new Point(160, 257);
        numFilling.Maximum = new decimal(
            new int[] { 32, 0, 0, 0 }
        );
        numFilling.Name = "numFilling";
        numFilling.Size = new Size(75, 27);
        numFilling.TabIndex = 10;

        // lblFillCost
        lblFillCost.AutoSize = true;
        lblFillCost.Location = new Point(275, 260);
        lblFillCost.Name = "lblFillCost";
        lblFillCost.Size = new Size(86, 20);
        lblFillCost.TabIndex = 11;
        lblFillCost.Text = "$80000/cái";

        // lblTotal
        lblTotal.AutoSize = true;
        lblTotal.Location = new Point(185, 315);
        lblTotal.Name = "lblTotal";
        lblTotal.Size = new Size(42, 20);
        lblTotal.TabIndex = 12;
        lblTotal.Text = "Total";

        // txtTotal
        txtTotal.Enabled = false;
        txtTotal.Location = new Point(245, 312);
        txtTotal.Name = "txtTotal";
        txtTotal.Size = new Size(125, 27);
        txtTotal.TabIndex = 13;
        txtTotal.TextAlign = HorizontalAlignment.Right;

        // btnExit
        btnExit.Location = new Point(60, 370);
        btnExit.Name = "btnExit";
        btnExit.Size = new Size(100, 35);
        btnExit.TabIndex = 14;
        btnExit.Text = "Thoát";
        btnExit.UseVisualStyleBackColor = true;
        btnExit.Click += btnExit_Click;

        // btnCalc
        btnCalc.Location = new Point(270, 370);
        btnCalc.Name = "btnCalc";
        btnCalc.Size = new Size(100, 35);
        btnCalc.TabIndex = 15;
        btnCalc.Text = "Tính tiền";
        btnCalc.UseVisualStyleBackColor = true;
        btnCalc.Click += btnCalc_Click;

        // FrmDentalPayment
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;

        ClientSize = new Size(430, 440);

        Controls.Add(lblTitle);
        Controls.Add(lblName);
        Controls.Add(txtName);

        Controls.Add(chkClean);
        Controls.Add(lblCleanCost);

        Controls.Add(chkWhitening);
        Controls.Add(lblWhiteningCost);

        Controls.Add(chkXRay);
        Controls.Add(lblXRayCost);

        Controls.Add(lblFilling);
        Controls.Add(numFilling);
        Controls.Add(lblFillCost);

        Controls.Add(lblTotal);
        Controls.Add(txtTotal);

        Controls.Add(btnExit);
        Controls.Add(btnCalc);

        Name = "FrmDentalPayment";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Dental Payment Application";

        ((System.ComponentModel.ISupportInitialize)numFilling).EndInit();

        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}