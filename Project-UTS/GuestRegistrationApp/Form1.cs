using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Dapper;
using ZXing;
using ZXing.Common;
using System.Drawing;


namespace GuestRegistrationApp
{
    public partial class Form1 : Form
    {
        private TextBox? txtName, txtAddress, txtPhone;
        private Label? lblInvitationCode;
        private DataGridView? dataGridView;
        private Button? btnSave;
        private Button? btnDelete;
        // private PictureBox? picBarcode; 
        public Form1()
        {
            this.Text = "Buku Undangan Digital";
            this.Size = new System.Drawing.Size(800, 644);

            InitControls();
            DatabaseHelper.InitializeDatabase();
            LoadGuests();
            GenerateInvitationCode();
        }

        private void InitControls()
        {   this.BackColor = Color.FromArgb(138, 173, 244);

            var lblName = new Label { Text = "Nama:", Left = 20, Top = 20, Width = 100, Font = new Font("Segoe UI", 10) };
            txtName = new TextBox { Left = 150, Top = 20, Width = 300, Font = new Font("Segoe UI", 10),
        BackColor = Color.FromArgb(202, 211, 245) };

            var lblAddress = new Label { Text = "Alamat:", Left = 20, Top = 60, Width = 100 };
            txtAddress = new TextBox { Left = 150, Top = 60, Width = 300, Font = new Font("Segoe UI", 10),
        BackColor = Color.FromArgb(202, 211, 245) };

            var lblPhone = new Label { Text = "Telepon:", Left = 20, Top = 100, Width = 100 };
            txtPhone = new TextBox { Left = 150, Top = 100, Width = 300, Font = new Font("Segoe UI", 10),
        BackColor = Color.FromArgb(202, 211, 245) };

            var lblCode = new Label { Text = "Kode Undangan:", Left = 20, Top = 140, Width = 120 };
            lblInvitationCode = new Label { Left = 150, Top = 140, Width = 200, Font = new Font("Segoe UI", 10),
        BackColor = Color.FromArgb(202, 211, 245) };

            btnSave = new Button { Text = "Simpan", Left = 150, Top = 180, Width = 100,   };
            btnDelete = new Button { Text = "Hapus", Left = 270, Top = 180, Width = 100 };
        // picBarcode = new PictureBox{Left = 150, Top = 440, Width = 200,Height = 80, BorderStyle = BorderStyle.FixedSingle};
        //                             Controls.Add(picBarcode);


#pragma warning disable CS8622
            btnSave.Click += btnSave_Click;
            btnDelete.Click += btnDelete_Click;
#pragma warning restore CS8622

            dataGridView = new DataGridView
            {
                Left = 20,
                Top = 230,
                Width = 644,
                Height = 200,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                BackgroundColor = Color.FromArgb(202, 211, 245),
                GridColor = Color.FromArgb(128, 135, 162),
                BorderStyle = BorderStyle.FixedSingle,
                EnableHeadersVisualStyles = false
            };

            Controls.Add(lblName);
            Controls.Add(txtName);
            Controls.Add(lblAddress);
            Controls.Add(txtAddress);
            Controls.Add(lblPhone);
            Controls.Add(txtPhone);
            Controls.Add(lblCode);
            Controls.Add(lblInvitationCode);
            Controls.Add(btnSave);
            Controls.Add(btnDelete);
            Controls.Add(dataGridView);
        }

        private void GenerateInvitationCode()
        {
            string code = "INV-" + DateTime.Now.ToString("yyyyMMdd") + "-" + Guid.NewGuid().ToString().Substring(0, 4).ToUpper();
            lblInvitationCode!.Text = code;
        }

        
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var guest = new Guest
                {
                    Name = txtName!.Text,
                    Address = txtAddress!.Text,
                    Phone = txtPhone!.Text,
                    InvitationCode = lblInvitationCode!.Text
                };

                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Execute(
                        "INSERT INTO Guests (Name, Address, Phone, InvitationCode) VALUES (@Name, @Address, @Phone, @InvitationCode)",
                        guest
                    );
                }

                MessageBox.Show("Data tamu berhasil disimpan!");
                ClearForm();
                LoadGuests();
                GenerateInvitationCode();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView!.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView.SelectedRows[0];
                var guest = selectedRow.DataBoundItem as Guest;
            if (guest == null)
{
    MessageBox.Show("Data tamu tidak valid.");
    return;
}


                var confirm = MessageBox.Show($"Yakin ingin menghapus data tamu '{guest.Name}'?", "Konfirmasi", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    using (var conn = DatabaseHelper.GetConnection())
                    {
                        conn.Execute("DELETE FROM Guests WHERE Id = @Id", new { guest.Id });
                    }

                    MessageBox.Show("Data tamu berhasil dihapus!");
                    LoadGuests();
                }
            }
            else
            {
                MessageBox.Show("Pilih salah satu baris untuk dihapus.");
            }
        }

        private void LoadGuests()
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                var list = conn.Query<Guest>("SELECT * FROM Guests ORDER BY RegistrationDate DESC").ToList();
                dataGridView!.DataSource = list;
            }
        }

        private void ClearForm()
        {
            txtName!.Text = "";
            txtAddress!.Text = "";
            txtPhone!.Text = "";
        }
    }
}
