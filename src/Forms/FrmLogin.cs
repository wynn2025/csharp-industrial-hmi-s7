using GaugeDemo300.Models;

namespace GaugeDemo300.Forms;

/// <summary>
/// 操作员登录窗体：输入工号/姓名，工程师需密码验证
/// </summary>
public class FrmLogin : Form
{
    private readonly SystemConfig _cfg;
    private TextBox _txtId;
    private TextBox _txtName;
    private ComboBox _cmbRole;
    private TextBox _txtPwd;

    public UserInfo LoggedUser { get; private set; } = new();

    public FrmLogin(SystemConfig cfg)
    {
        _cfg = cfg;
        Text = "用户登录";
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(340, 250);
        BackColor = Color.FromArgb(27, 30, 38);
        ForeColor = Color.White;
        Font = new Font("Microsoft YaHei UI", 10F);
        BuildUi();
    }

    private void BuildUi()
    {
        int y = 20;
        AddLabel("工号：", 30, y); _txtId = AddText(110, y, 200);
        y += 42;
        AddLabel("姓名：", 30, y); _txtName = AddText(110, y, 200);
        y += 42;
        AddLabel("角色：", 30, y);
        _cmbRole = new ComboBox
        {
            Location = new Point(110, y), Size = new Size(200, 28),
            DropDownStyle = ComboBoxStyle.DropDownList,
            BackColor = Color.FromArgb(45, 50, 66), ForeColor = Color.White,
            Font = Font
        };
        _cmbRole.Items.AddRange(new object[] { "操作员", "工程师" });
        _cmbRole.SelectedIndex = 0;
        Controls.Add(_cmbRole);
        y += 42;
        AddLabel("密码：", 30, y); _txtPwd = AddText(110, y, 200);
        _txtPwd.PasswordChar = '*';
        _txtPwd.Visible = false;
        _cmbRole.SelectedIndexChanged += (_, _) => _txtPwd.Visible = _cmbRole.SelectedIndex == 1;
        y += 56;

        var btnOk = new Button
        {
            Text = "登 录", Location = new Point(110, y), Size = new Size(90, 34),
            BackColor = Color.FromArgb(56, 130, 246), ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat, Font = Font
        };
        var btnCancel = new Button
        {
            Text = "取 消", Location = new Point(210, y), Size = new Size(90, 34),
            BackColor = Color.FromArgb(60, 66, 88), ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat, Font = Font
        };
        btnOk.Click += (_, _) => TryLogin();
        btnCancel.Click += (_, _) => { DialogResult = DialogResult.Cancel; Close(); };
        Controls.Add(btnOk);
        Controls.Add(btnCancel);
        _txtId.KeyDown += (_, e) => { if (e.KeyCode == Keys.Enter) TryLogin(); };
        _txtName.KeyDown += (_, e) => { if (e.KeyCode == Keys.Enter) TryLogin(); };
        _txtPwd.KeyDown += (_, e) => { if (e.KeyCode == Keys.Enter) TryLogin(); };
    }

    private Label AddLabel(string text, int x, int y)
    {
        var lb = new Label { Text = text, Location = new Point(x, y), AutoSize = true, ForeColor = Color.FromArgb(160, 172, 196), Font = Font };
        Controls.Add(lb);
        return lb;
    }

    private TextBox AddText(int x, int y, int w)
    {
        var tb = new TextBox
        {
            Location = new Point(x, y), Size = new Size(w, 28),
            BackColor = Color.FromArgb(45, 50, 66), ForeColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle, Font = Font
        };
        Controls.Add(tb);
        return tb;
    }

    private void TryLogin()
    {
        string id = _txtId.Text.Trim();
        string name = _txtName.Text.Trim();
        if (id.Length == 0 || name.Length == 0)
        {
            MessageBox.Show("请输入工号和姓名", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (_cmbRole.SelectedIndex == 1 && _txtPwd.Text != _cfg.EngineerPassword)
        {
            MessageBox.Show("工程师密码错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        LoggedUser = new UserInfo
        {
            Id = id,
            Name = name,
            Role = _cmbRole.SelectedIndex == 1 ? "engineer" : "operator"
        };
        DialogResult = DialogResult.OK;
        Close();
    }
}
