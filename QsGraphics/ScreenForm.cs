namespace QsGraphics;

public partial class ScreenForm : Form
{
    public ScreenForm()
    {
        InitializeComponent();
    }


    private Graphics _FormGraphics;

    private void ScreenForm_Load(object sender, EventArgs e)
    {
        _FormGraphics = CreateGraphics();
    }

    public Graphics FormGraphics => _FormGraphics;
}