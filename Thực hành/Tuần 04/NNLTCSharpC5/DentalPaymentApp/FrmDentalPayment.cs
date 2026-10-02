namespace DentalPaymentApp;

public partial class FrmDentalPayment : Form
{
    public FrmDentalPayment()
    {
        InitializeComponent();
    }

    private void GetPay()
    {
        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            MessageBox.Show(
                "Chưa nhập tên khách hàng!",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );

            txtName.Focus();
            return;
        }

        decimal total = 0;

        if (chkClean.Checked)
        {
            total += 100000;
        }

        if (chkWhitening.Checked)
        {
            total += 1200000;
        }

        if (chkXRay.Checked)
        {
            total += 200000;
        }

        total += numFilling.Value * 80000;

        txtTotal.Text = total.ToString("N0");
    }

    private void btnCalc_Click(
        object sender,
        EventArgs e
    )
    {
        GetPay();
    }

    private void btnExit_Click(
        object sender,
        EventArgs e
    )
    {
        Close();
    }
}