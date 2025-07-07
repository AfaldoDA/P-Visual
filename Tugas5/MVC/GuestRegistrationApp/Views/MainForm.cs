using GuestRegistrationApp.Controllers;
using GuestRegistrationApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace GuestRegistrationApp.Views
{
    public partial class MainForm : Form
    {
        private TextBox? txtName, txtAddress, txtPhone;
        private Label? lblInvitationCode;
        private DataGridView? dataGridView;
        private Button? btnSave, btnDelete, btnEdit, btnGenerateBarcode;
        private PictureBox? picBarcode;
        private int? selectedGuestId = null;
        private GuestController controller = new GuestController();

        public MainForm()
        {
            this.Text = "Buku Undangan Digital";
            this.Size = new Size(800, 644);
            InitControls();
            LoadGuests();
            GenerateInvitationCode();
        }

        private void InitControls()
        {
            this.BackColor = Color.FromArgb(138, 173, 244);

            var lblName = new Label { Text = "Nama:", Left = 20, Top = 20, Width = 100, Font = new Font("Segoe UI", 10) };
            txtName = new TextBox { Left = 150, Top = 20, Width = 300, Font = new Font("Segoe UI", 10), BackColor = Color.FromArgb(202, 211, 245) };

            var lblAddress = new Label { Text = "Alamat:", Left = 20, Top = 60, Width = 100 };
            txtAddress = new TextBox { Left = 150, Top = 60, Width = 300, Font = new Font("Segoe UI", 10), BackColor = Color.FromArgb(202, 211, 245) };

            var lblPhone = new Label { Text = "Telepon:", Left = 20, Top = 100, Width = 100 };
            txtPhone = new TextBox { Left = 150, Top = 100, Width = 300, Font = new Font("Segoe UI", 10), BackColor = Color.FromArgb(202, 211, 245) };

            var lblCode = new Label { Text = "Kode Undangan:", Left = 20, Top = 140, Width = 120 };
            lblInvitationCode = new Label { Left = 150, Top = 140, Width = 200, Font = new Font("Segoe UI", 10), BackColor = Color.FromArgb(202, 211, 245) };

            btnSave = new Button { Text = "Simpan", Left = 150, Top = 180, Width = 100 };
            btnDelete = new Button { Text = "Hapus", Left = 270, Top = 180, Width = 100 };
            btnEdit = new Button { Text = "Edit", Left = 390, Top = 180, Width = 100 };
            btnGenerateBarcode = new Button { Text = "Generate Barcode", Left = 510, Top = 180, Width = 120 };
            var btnPreview = new Button { Text = "Preview Undangan", Left = 20, Top = 660, Width = 150 };
            var btnDownload = new Button { Text = "Download Undangan", Left = 200, Top = 660, Width = 150 };

            btnSave.Click += btnSave_Click;
            btnDelete.Click += btnDelete_Click;
            btnEdit.Click += btnEdit_Click;
            btnGenerateBarcode.Click += btnGenerateBarcode_Click;
            btnPreview.Click += btnPreview_Click;
            btnDownload.Click += btnDownload_Click;


            picBarcode = new PictureBox { Left = 150, Top = 440, Width = 200, Height = 200, BorderStyle = BorderStyle.FixedSingle, SizeMode = PictureBoxSizeMode.Zoom };

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
            dataGridView.SelectionChanged += DataGridView_SelectionChanged;

            Controls.AddRange(new Control[] {
                lblName, txtName, lblAddress, txtAddress, lblPhone, txtPhone,
                lblCode, lblInvitationCode, btnSave, btnDelete, btnEdit,
                btnGenerateBarcode, btnPreview, btnDownload, picBarcode, dataGridView
            });
        }

        private void GenerateInvitationCode()
        {
            lblInvitationCode!.Text = controller.GenerateInvitationCode();
        }

        private void btnSave_Click(object? sender, EventArgs e)
        {
            var guest = new Guest
            {
                Name = txtName!.Text,
                Address = txtAddress!.Text,
                Phone = txtPhone!.Text,
                InvitationCode = lblInvitationCode!.Text
            };
            controller.AddGuest(guest);
            MessageBox.Show("Data tamu berhasil disimpan!");
            LoadGuests();
            ClearForm();
            GenerateInvitationCode();
        }

        private void btnEdit_Click(object? sender, EventArgs e)
        {
            if (selectedGuestId == null) return;

            var guest = new Guest
            {
                Id = selectedGuestId.Value,
                Name = txtName!.Text,
                Address = txtAddress!.Text,
                Phone = txtPhone!.Text
            };
            controller.UpdateGuest(guest);
            MessageBox.Show("Data tamu berhasil diperbarui!");
            LoadGuests();
            ClearForm();
            GenerateInvitationCode();
        }

        private void btnDelete_Click(object? sender, EventArgs e)
        {
            if (dataGridView!.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView.SelectedRows[0];
                var guest = selectedRow.DataBoundItem as Guest;
                if (guest != null)
                {
                    controller.DeleteGuest(guest.Id);
                    MessageBox.Show("Data tamu berhasil dihapus!");
                    LoadGuests();
                    ClearForm();
                }
            }
        }

        private void btnGenerateBarcode_Click(object? sender, EventArgs e)
        {
            if (dataGridView!.SelectedRows.Count > 0)
            {
                var guest = dataGridView.SelectedRows[0].DataBoundItem as Guest;
                if (guest != null)
                {
                    picBarcode!.Image = controller.GenerateBarcode(guest.InvitationCode!);
                }
            }
        }
        
        private void btnPreview_Click(object? sender, EventArgs e)
        {
            if (dataGridView!.SelectedRows.Count > 0)
            {
                var guest = dataGridView.SelectedRows[0].DataBoundItem as Guest;
                if (guest != null)
                {
                    var image = controller.GenerateInvitationTemplate(guest);
                    var previewForm = new Form
                    {
                        Text = "Preview Undangan",
                        Size = new Size(1200, 900),
                        StartPosition = FormStartPosition.CenterParent
                    };
                    var picture = new PictureBox
                    {
                        Image = image,
                        Dock = DockStyle.Fill,
                        SizeMode = PictureBoxSizeMode.Zoom
                    };
                    previewForm.Controls.Add(picture);
                    previewForm.ShowDialog();
                }
            }
        }

        private void btnDownload_Click(object? sender, EventArgs e)
{
    if (dataGridView!.SelectedRows.Count > 0)
    {
        var guest = dataGridView.SelectedRows[0].DataBoundItem as Guest;
        if (guest != null)
        {
            using SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "PNG Image|*.png",
                FileName = $"Undangan_{guest.Name}.png"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                var image = controller.GenerateInvitationTemplate(guest);
                image.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Png);
                MessageBox.Show("Undangan berhasil disimpan!");
            }
        }
    }
}


        private void LoadGuests()
        {
            var list = controller.GetAllGuests();
            dataGridView!.DataSource = list;
        }

        private void ClearForm()
        {
            txtName!.Text = "";
            txtAddress!.Text = "";
            txtPhone!.Text = "";
            selectedGuestId = null;
            dataGridView!.ClearSelection();
        }

        private void DataGridView_SelectionChanged(object? sender, EventArgs e)
        {
            if (dataGridView!.SelectedRows.Count > 0)
            {
                var guest = dataGridView.SelectedRows[0].DataBoundItem as Guest;
                if (guest != null)
                {
                    txtName!.Text = guest.Name;
                    txtAddress!.Text = guest.Address;
                    txtPhone!.Text = guest.Phone;
                    lblInvitationCode!.Text = guest.InvitationCode;
                    selectedGuestId = guest.Id;
                }
            }
        }
    }
}
