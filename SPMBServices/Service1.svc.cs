using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SPMBServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        string con = "Data Source=DESKTOP-3FECEAK;Initial Catalog=SPMB;User ID=sa;Password=ArKa1234";
        string query = "";
        SqlConnection koneksi = new SqlConnection();
        SqlCommand cmd;
        SqlDataAdapter sda;
        SqlDataReader reader;
        DataTable dataTable;
        
        public string Daftar(string username, string password, string email, string nohp)
        {
            string status = "";
            status = cekDaftar(username, password, email, nohp);

            if (status != "berhasil")
            {
                return status;
            }

            string noPendaftaran = getNoPendaftaran();
            koneksi.ConnectionString = con;
            query = "INSERT INTO Pendaftar([no_pendaftaran], [username], [password], [email], [nohp]) VALUES(@noPendaftaran, @username, @password, @email, @nohp)";
            cmd = new SqlCommand(query, koneksi);
            cmd.Parameters.AddWithValue("@noPendaftaran", noPendaftaran);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@nohp", nohp);

            koneksi.Open();
            try
            {
                cmd.ExecuteNonQuery();
            } catch (Exception e)
            {
                status = "Error - " + e.Message + " Please Contact the Administrator";
            }
            koneksi.Close();

            return status;
        }

        string getNoPendaftaran()
        {
            string noPendaftaran, current, editable;
            noPendaftaran = "";
            int noPos = 0;
            koneksi.ConnectionString = con;
            query = "SELECT no_pendaftaran FROM Pendaftar ORDER BY no_pendaftaran DESC";
            cmd = new SqlCommand(query, koneksi);

            koneksi.Open();
            reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                current = reader.GetString(0);
                editable = current.Substring(9, 5);
                noPos = Int32.Parse(editable);
                noPos++;
                if (noPos > 9999)
                    noPendaftaran = "SPMB2019-" + noPos;
                else if (noPos > 999)
                    noPendaftaran = "SPMB2019-0" + noPos;
                else if (noPos > 99)
                    noPendaftaran = "SPMB2019-00" + noPos;
                else if (noPos > 9)
                    noPendaftaran = "SPMB2019-000" + noPos;
                else if (noPos < 10)
                    noPendaftaran = "SPMB2019-0000" + noPos;
            }
            else
            {
                noPendaftaran = "SPMB2019-00001";
            }
            koneksi.Close();
            return noPendaftaran;
        }

        private string cekDaftar(string username, string password, string email, string nohp)
        {
            koneksi.ConnectionString = con;
            query = "SELECT username FROM Pendaftar WHERE username = @username";
            cmd = new SqlCommand(query, koneksi);
            cmd.Parameters.AddWithValue("@username", username);

            koneksi.Open();
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                koneksi.Close();
                return "Username telah digunakan";
            }
            koneksi.Close();

            string status = "";
            if (username.Length < 6)
                status = "Username minimal 6 huruf";
            else if (password.Length < 8)
                status = "Password minimal 8 huruf";
            else if (string.IsNullOrEmpty(email) || string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                status = "Email harus di isi";
            else if (string.IsNullOrEmpty(nohp) || string.IsNullOrWhiteSpace(nohp) || nohp.Any(x => char.IsLetter(x)))
                status = "No Hp harus diisi";
            else
                status = "berhasil";
            return status;
        }

        public string UpdatePendaftar(string jsonData)
        {
            string status = "";
            Pendaftar pendaftar = new Pendaftar();
            pendaftar = JsonConvert.DeserializeObject<Pendaftar>(jsonData);
            pendaftar.WaktuTest = Convert.ToDateTime(pendaftar.WaktuTest.ToString("dd/MM/yyyy"));
            pendaftar.TanggalLahir = Convert.ToDateTime(pendaftar.TanggalLahir.ToString("dd/MM/yyyy"));
            koneksi.ConnectionString = con;
            query = "UPDATE Pendaftar SET " +
                "[nisn] = @nisn," +
                "[nama] = @nama," +
                "[asal_sekolah] = @asalSekolah," +
                "[jenis_kelamin] = @jenisKelamin," +
                "[alamat] = @alamat," +
                "[tempat_lahir] = @tempatLahir," +
                "[tanggal_lahir] = @tanggalLahir," +
                "[jurusan1] = @jurusan1," +
                "[jurusan2] = @jurusan2," +
                "[nama_orang_tua] = @namaOrangTua," +
                "[pekerjaan_orang_tua] = @pekerjaanOrangTua " +
                "WHERE no_pendaftaran = @noPendaftaran";
            cmd = new SqlCommand(query, koneksi);
            cmd.Parameters.AddWithValue("@noPendaftaran", pendaftar.NoPendaftaran);
            cmd.Parameters.AddWithValue("@nisn", pendaftar.Nisn);
            cmd.Parameters.AddWithValue("@nama", pendaftar.Nama);
            cmd.Parameters.AddWithValue("@asalSekolah", pendaftar.AsalSekolah);
            cmd.Parameters.AddWithValue("@jenisKelamin", pendaftar.JenisKelamin);
            cmd.Parameters.AddWithValue("@alamat", pendaftar.Alamat);
            cmd.Parameters.AddWithValue("@tempatLahir", pendaftar.TempatLahir);
            cmd.Parameters.AddWithValue("@tanggalLahir", pendaftar.TanggalLahir);
            cmd.Parameters.AddWithValue("@jurusan1", pendaftar.Jurusan1);
            cmd.Parameters.AddWithValue("@jurusan2", pendaftar.Jurusan2);
            cmd.Parameters.AddWithValue("@namaOrangTua", pendaftar.NamaOrangTua);
            cmd.Parameters.AddWithValue("@pekerjaanOrangTua", pendaftar.PekerjaanOrangTua);


            koneksi.Open();
            try
            {
                cmd.ExecuteNonQuery();
                status = "berhasil";
            } catch(Exception e)
            {
                status = "Error - " + e.Message + " Please Contact the Administrator.";
            }
            koneksi.Close();

            return status;
        }

        public LoginPendaftar LoginPendaftar(string username, string password)
        {
            LoginPendaftar user = new LoginPendaftar();
            string status = cekLoginPendaftar(username, password);

            if (!status.Equals("berhasil"))
            {
                user.Status = status;
                return user;
            }

            koneksi.ConnectionString = con;
            query = "SELECT no_pendaftaran, username, nama FROM Pendaftar WHERE username = @username AND password = @password";
            cmd = new SqlCommand(query, koneksi);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);

            koneksi.Open();
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                user.Status = status;
                user.NoPendaftaran = reader.GetString(0);
                user.Username = reader.GetString(1);
                user.Nama = reader.GetString(2);
                koneksi.Close();
                return user;
            }
            else
            {
                koneksi.Close();
                user.Status = "Username atau Password Salah.";
                return user;
            }
        }

        private string cekLoginPendaftar(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrWhiteSpace(username))
            {
                return "Username tidak boleh kosong";
            }
            else if(string.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(password))
            {
                return "Password tidak boleh kosong";
            }

            return "berhasil";
            
        }

        public WaktuPendaftaran CekWaktuPendaftaran()
        {
            WaktuPendaftaran waktuPendaftaran = new WaktuPendaftaran();
            koneksi.ConnectionString = con;
            query = "SELECT waktupendaftaranmulai, waktupendaftaranselesai FROM config";
            cmd = new SqlCommand(query, koneksi);

            koneksi.Open();
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                waktuPendaftaran.Mulai = reader["waktupendaftaranmulai"].ToString().Substring(0, 10);
                waktuPendaftaran.Selesai = reader["waktupendaftaranselesai"].ToString().Substring(0, 10);
            }

            koneksi.Close();
            return waktuPendaftaran;
        }
    }
}
