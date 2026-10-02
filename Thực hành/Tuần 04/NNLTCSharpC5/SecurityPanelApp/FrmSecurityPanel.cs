namespace SecurityPanelApp;

public partial class FrmSecurityPanel : Form
{
    public FrmSecurityPanel()
    {
        InitializeComponent();
    }

    private void NumberButton_Click(
        object sender,
        EventArgs e
    )
    {
        Button btn = (Button)sender;

        txtSecurityCode.Text += btn.Text;
    }

    private void btnClear_Click(
        object sender,
        EventArgs e
    )
    {
        txtSecurityCode.Clear();
    }

    private void btnEnter_Click(
        object sender,
        EventArgs e
    )
    {
        CheckAccess();
    }

    private void CheckAccess()
    {
        string code = txtSecurityCode.Text;

        string time =
            DateTime.Now.ToString(
                "dd/MM/yyyy HH:mm:ss"
            );

        string message;

        switch (code)
        {
            case "1645":
            case "1689":
                message =
                    $"{time} - Technicians";
                break;

            case "8345":
                message =
                    $"{time} - Custodians";
                break;

            case "9998":
            case "1006":
            case "1007":
            case "1008":
                message =
                    $"{time} - Scientist";
                break;

            default:
                if (code.Length == 1)
                {
                    message =
                        $"{time} - Restricted Access";
                }
                else
                {
                    message =
                        $"{time} - Access denied";
                }
                break;
        }

        lbxAccessLog.Items.Add(message);

        txtSecurityCode.Clear();
    }
}