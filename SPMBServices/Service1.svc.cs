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
        
        public DaftarPendaftar Daftar(string username, string password, string repassword, string email, string nohp, string agreement)
        {
            DaftarPendaftar daftarPendaftar = new DaftarPendaftar();
            string status = "";
            status = cekDaftar(username, password, repassword, email, nohp, agreement);

            if (status != "berhasil")
            {
                daftarPendaftar.Status = status;
                return daftarPendaftar;
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
                daftarPendaftar.NoPendaftaran = noPendaftaran;
                daftarPendaftar.Username = username;
                daftarPendaftar.Status = "berhasil";
                daftarPendaftar.Nama = "-";
            }
            catch (Exception e)
            {
                status = "Error - " + e.Message + " Please Contact the Administrator";
            }
            koneksi.Close();

            return daftarPendaftar;
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

        private string cekDaftar(string username, string password, string repassword, string email, string nohp, string agreement)
        {
            string status = "";
            koneksi.ConnectionString = con;
            query = "SELECT username FROM Pendaftar WHERE username = @username";
            cmd = new SqlCommand(query, koneksi);
            cmd.Parameters.AddWithValue("@username", username);

            koneksi.Open();
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                koneksi.Close();
                status = " |Username telah digunakan| ";
            }
            koneksi.Close();

            if (username.Length < 6)
                status += " |Username minimal 6 huruf| ";
            if (password.Length < 8)
                status += " |Password minimal 8 huruf| ";
            if (!password.Equals(repassword))
                status += " |Password yang anda masukkan tidak sama| ";
            if (string.IsNullOrEmpty(email) || string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                status += " |Email harus di isi| ";
            if (string.IsNullOrEmpty(nohp) || string.IsNullOrWhiteSpace(nohp) || nohp.Any(x => char.IsLetter(x)))
                status += " |No Hp harus diisi angka| ";
            if (!agreement.Equals("setuju"))
                status += " |Anda Harus menyetuji persetujuan yang ada| ";
            if(status=="")
                status = "berhasil";

            return status;
        }

        public string UpdatePendaftar(string jsonData)
        {
            string status = "";
            Pendaftar pendaftar = new Pendaftar();
            pendaftar = JsonConvert.DeserializeObject<Pendaftar>(jsonData);
            koneksi.ConnectionString = con;
            query = "UPDATE Pendaftar SET " +
                "[nama] = @nama," +
                "[email] = @email," +
                "[asal_sekolah] = @asalSekolah," +
                "[jenis_kelamin] = @jenisKelamin," +
                "[alamat] = @alamat," +
                "[tempat_lahir] = @tempatLahir," +
                "[tanggal_lahir] = @tanggalLahir," +
                "[nama_orang_tua] = @namaOrangTua," +
                "[pekerjaan_orang_tua] = @pekerjaanOrangTua " +
                "WHERE no_pendaftaran = @noPendaftaran";
            cmd = new SqlCommand(query, koneksi);
            cmd.Parameters.AddWithValue("@noPendaftaran", pendaftar.NoPendaftaran);
            cmd.Parameters.AddWithValue("@nama", pendaftar.Nama);
            cmd.Parameters.AddWithValue("@email", pendaftar.Email);
            cmd.Parameters.AddWithValue("@asalSekolah", pendaftar.AsalSekolah);
            cmd.Parameters.AddWithValue("@jenisKelamin", pendaftar.JenisKelamin);
            cmd.Parameters.AddWithValue("@alamat", pendaftar.Alamat);
            cmd.Parameters.AddWithValue("@tempatLahir", pendaftar.TempatLahir);
            cmd.Parameters.AddWithValue("@tanggalLahir", pendaftar.TanggalLahir);
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
                user.Nama = reader.IsDBNull(2) ? "-" : reader.GetString(2);
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
            query = "SELECT waktupendaftaranmulai, waktupendaftaranselesai FROM Config";
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

        public Pendaftar CekDataPendaftar(string noPendaftaran)
        {
            Pendaftar dataPendaftar = new Pendaftar();

            koneksi.ConnectionString = con;
            query = "SELECT * FROM Pendaftar WHERE no_pendaftaran = @noPendaftaran";
            cmd = new SqlCommand(query, koneksi);
            cmd.Parameters.AddWithValue("@noPendaftaran", noPendaftaran);

            koneksi.Open();
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                dataPendaftar.NoPendaftaran = reader.IsDBNull(0) ? "-" : reader["no_pendaftaran"].ToString();
                dataPendaftar.Email = reader.IsDBNull(3) ? "-" : reader["email"].ToString();
                dataPendaftar.NoHP = reader.IsDBNull(4) ? "-" : reader["nohp"].ToString();
                dataPendaftar.Nisn = reader.IsDBNull(5) ? "-" : reader["nisn"].ToString();
                dataPendaftar.Nama = reader.IsDBNull(6) ? "-" : reader["nama"].ToString();
                dataPendaftar.AsalSekolah = reader.IsDBNull(7) ? "-" : reader["asal_sekolah"].ToString();
                dataPendaftar.JenisKelamin = reader.IsDBNull(8) ? "-" : reader["jenis_kelamin"].ToString();
                dataPendaftar.Alamat = reader.IsDBNull(9) ? "-" : reader["alamat"].ToString();
                dataPendaftar.TempatLahir = reader.IsDBNull(10) ? "-" : reader["tempat_lahir"].ToString();
                dataPendaftar.NamaOrangTua = reader.IsDBNull(15) ? "-" : reader["nama_orang_tua"].ToString();
                dataPendaftar.PekerjaanOrangTua = reader.IsDBNull(16) ? "-" : reader["pekerjaan_orang_tua"].ToString();
                dataPendaftar.TanggalLahir = reader.IsDBNull(11) ? "-" : reader["tanggal_lahir"].ToString().Substring(0, 10);
                dataPendaftar.WaktuTest = reader.IsDBNull(12) ? "-" : reader["waktu_test"].ToString().Substring(0, 10);
                dataPendaftar.Jurusan1 = reader.IsDBNull(13) ? 0 : Convert.ToInt32(reader["jurusan1"].ToString());
                dataPendaftar.Jurusan2 = reader.IsDBNull(14) ? 0 : Convert.ToInt32(reader["jurusan2"].ToString());
                dataPendaftar.IdVerificator = reader.IsDBNull(17) ? 0 : Convert.ToInt32(reader["id_verificator"].ToString());
                dataPendaftar.IdStatus = reader.IsDBNull(18) ? 0 : Convert.ToInt32(reader["id_status"].ToString());
                dataPendaftar.IdTahunDaftar = reader.IsDBNull(19) ? 0 : Convert.ToInt32(reader["id_tahun_daftar"].ToString());
                dataPendaftar.Status = "berhasil";
            }
            else
            {
                dataPendaftar.Status = "gagal";
            }

            koneksi.Close();

            return dataPendaftar;
        }

        public WaktuPengumuman CekWaktuPengumuman()
        {
            WaktuPengumuman waktuPengumuman = new WaktuPengumuman();
            koneksi.ConnectionString = con;
            query = "SELECT waktupengumuman FROM Config";
            cmd = new SqlCommand(query, koneksi);

            koneksi.Open();
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                DateTime date = Convert.ToDateTime(reader["waktupengumuman"].ToString());
                waktuPengumuman.Tanggal  = date.ToString("dd-MM-yyyy").Substring(0, 10);
            }

            koneksi.Close();
            return waktuPengumuman;
        }
    }
}
