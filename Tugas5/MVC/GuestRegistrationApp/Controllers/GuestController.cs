using GuestRegistrationApp.Models;
using Dapper;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GuestRegistrationApp.Controllers
{
    public class GuestController
    {
        public List<Guest> GetAllGuests()
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                return conn.Query<Guest>("SELECT * FROM Guests ORDER BY RegistrationDate DESC").ToList();
            }
        }

        public void AddGuest(Guest guest)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Execute("INSERT INTO Guests (Name, Address, Phone, InvitationCode) VALUES (@Name, @Address, @Phone, @InvitationCode)", guest);
            }
        }

        public void UpdateGuest(Guest guest)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Execute("UPDATE Guests SET Name = @Name, Address = @Address, Phone = @Phone WHERE Id = @Id", guest);
            }
        }

        public void DeleteGuest(int id)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Execute("DELETE FROM Guests WHERE Id = @Id", new { Id = id });
            }
        }

        public string GenerateInvitationCode()
        {
            return "INV-" + DateTime.Now.ToString("yyyyMMdd") + "-" + Guid.NewGuid().ToString().Substring(0, 4).ToUpper();
        }

        public Bitmap GenerateBarcode(string text)
        {
            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new QRCode(qrCodeData);
            return qrCode.GetGraphic(20);
        }
        public Bitmap GenerateInvitationTemplate(Guest guest)
{
    
    string bgPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "blt.jpg");

   
    Bitmap bitmap = new Bitmap(bgPath); 
    using Graphics g = Graphics.FromImage(bitmap);

    var titleFont = new Font("Segoe UI", 24, FontStyle.Bold);
    var bodyFont = new Font("Segoe UI", 18);
    var brush = Brushes.White;

    g.DrawString("UNDANGAN PENGAMBILAN BLT UNP", titleFont, brush, new PointF(180, 20));
    g.DrawString($"Nama: {guest.Name}", bodyFont, brush, new PointF(50, 100));
    g.DrawString($"Alamat: {guest.Address}", bodyFont, brush, new PointF(50, 140));
    g.DrawString($"Kode Undangan: {guest.InvitationCode}", bodyFont, brush, new PointF(50, 180));
    g.DrawString("Mohon tunjukkan QR ini saat hadir:", bodyFont, brush, new PointF(50, 220));

    // Generate QR
    Bitmap qrImage = GenerateBarcode(guest.InvitationCode!);
    var qrResized = new Bitmap(qrImage, new Size(150, 150));
    g.DrawImage(qrResized, new Point(bitmap.Width - 200, 100));

    return bitmap;
}

    }
}
