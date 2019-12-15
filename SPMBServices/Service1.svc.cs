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
        SqlDataReader reader;
        
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
                string tahunAktif = CekTahunAktif();
                current = reader.GetString(0);
                editable = current.Substring(9, 5);
                noPos = Int32.Parse(editable);
                noPos++;
                if (noPos > 9999)
                    noPendaftaran = "SPMB"+ tahunAktif + "-" + noPos;
                else if (noPos > 999)
                    noPendaftaran = "SPMB" + tahunAktif + "-0" + noPos;
                else if (noPos > 99)
                    noPendaftaran = "SPMB" + tahunAktif + "-00" + noPos;
                else if (noPos > 9)
                    noPendaftaran = "SPMB" + tahunAktif + "-000" + noPos;
                else if (noPos < 10)
                    noPendaftaran = "SPMB" + tahunAktif + "-0000" + noPos;
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
                "[nisn] = @nisn," +
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
            cmd.Parameters.AddWithValue("@nisn", pendaftar.Nisn);
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
                DateTime date1 = Convert.ToDateTime(reader["waktupendaftaranmulai"].ToString());
                waktuPendaftaran.Mulai = date1.ToString("dd-MM-yyyy").Substring(0, 10);
                DateTime date2 = Convert.ToDateTime(reader["waktupendaftaranselesai"].ToString());
                waktuPendaftaran.Selesai = date2.ToString("dd-MM-yyyy").Substring(0, 10);
            }

            koneksi.Close();
            return waktuPendaftaran;
        }

        public Pendaftar CekDataPendaftar(string noPendaftaran)
        {
            Pendaftar dataPendaftar = new Pendaftar();

            koneksi.ConnectionString = con;
            //query = "SELECT * FROM Pendaftar " +
            //    "JOIN Jurusan AS J1 ON Pendaftar.jurusan1 = J1.id " +
            //    "JOIN Jurusan AS J2 ON Pendaftar.jurusan2 = J2.id " +
            //    "JOIN Tahun ON Pendaftar.id_tahun_daftar = Tahun.id " +
            //    "JOIN [User] ON Pendaftar.id_verificator = [User].[id] " +
            //    "JOIN StatusPendaftar ON Pendaftar.id_status = StatusPendaftar.id " +
            //    "WHERE no_pendaftaran = @noPendaftaran";

            query = "SELECT * FROM Pendaftar " +
                "JOIN Jurusan AS J1 ON Pendaftar.jurusan1 = J1.id " +
                "JOIN Jurusan AS J2 ON Pendaftar.jurusan2 = J2.id " +
                "JOIN Tahun ON Pendaftar.id_tahun_daftar = Tahun.id " +
                "JOIN [User] ON Pendaftar.id_verificator = [User].[id] " +
                "JOIN StatusPendaftar ON Pendaftar.id_status = StatusPendaftar.id " +
                "WHERE no_pendaftaran = @noPendaftaran";

            cmd = new SqlCommand(query, koneksi);
            cmd.Parameters.AddWithValue("@noPendaftaran", noPendaftaran);

            koneksi.Open();
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                
                DateTime dateTL = reader.IsDBNull(11) ? new DateTime() : Convert.ToDateTime(reader["tanggal_lahir"].ToString());
                DateTime dateWT = reader.IsDBNull(12) ? new DateTime() : Convert.ToDateTime(reader["waktu_test"].ToString());

                dataPendaftar.NoPendaftaran = reader.IsDBNull(0) ? "" : reader["no_pendaftaran"].ToString();
                dataPendaftar.Email = reader.IsDBNull(3) ? "" : reader["email"].ToString();
                dataPendaftar.NoHP = reader.IsDBNull(4) ? "" : reader["nohp"].ToString();
                dataPendaftar.Nisn = reader.IsDBNull(5) ? "" : reader["nisn"].ToString();
                dataPendaftar.Nama = reader.IsDBNull(6) ? "" : reader["nama"].ToString();
                dataPendaftar.AsalSekolah = reader.IsDBNull(7) ? "" : reader["asal_sekolah"].ToString();
                dataPendaftar.JenisKelamin = reader.IsDBNull(8) ? "" : reader["jenis_kelamin"].ToString();
                dataPendaftar.Alamat = reader.IsDBNull(9) ? "" : reader["alamat"].ToString();
                dataPendaftar.TempatLahir = reader.IsDBNull(10) ? "" : reader["tempat_lahir"].ToString();
                dataPendaftar.NamaOrangTua = reader.IsDBNull(15) ? "" : reader["nama_orang_tua"].ToString();
                dataPendaftar.PekerjaanOrangTua = reader.IsDBNull(16) ? "" : reader["pekerjaan_orang_tua"].ToString();
                dataPendaftar.TanggalLahir = dateTL.ToString("dd-MM-yyyy").Substring(0, 10);
                dataPendaftar.WaktuTest = dateWT.ToString("dd-MM-yyyy HH:mm");
                dataPendaftar.Jurusan1 = reader.IsDBNull(13) ? 0 : Convert.ToInt32(reader["jurusan1"].ToString());
                dataPendaftar.Jurusan2 = reader.IsDBNull(14) ? 0 : Convert.ToInt32(reader["jurusan2"].ToString());
                dataPendaftar.NamaJ1 = reader.IsDBNull(21) ? "" : reader[21].ToString();
                dataPendaftar.NamaJ2 = reader.IsDBNull(24) ? "" : reader[24].ToString();
                dataPendaftar.IdVerificator = reader.IsDBNull(17) ? 0 : Convert.ToInt32(reader["id_verificator"].ToString());
                dataPendaftar.IdStatus = reader.IsDBNull(18) ? 0 : Convert.ToInt32(reader["id_status"].ToString());
                dataPendaftar.IdTahunDaftar = reader.IsDBNull(27) ? 0 : Convert.ToInt32(reader[27].ToString());
                dataPendaftar.Verificator = reader.IsDBNull(29) ? "" : reader[29].ToString();
                dataPendaftar.StatusPendaftar = reader.IsDBNull(33) ? "" : reader[33].ToString();
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

        public LoginAdmin LoginAdmin(string username, string password)
        {
            LoginAdmin user = new LoginAdmin();
            string status = cekLoginPendaftar(username, password);

            if (!status.Equals("berhasil"))
            {
                user.Status = status;
                return user;
            }

            koneksi.ConnectionString = con;
            query = "SELECT id, nama, username FROM [User] WHERE username = @username AND password = @password";
            cmd = new SqlCommand(query, koneksi);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);

            koneksi.Open();
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                user.Id = Convert.ToString(reader.GetInt32(0));
                user.Nama = reader.GetString(1);
                user.Username = reader.GetString(2);
                user.Status = status;
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

        public WaktuTest CekWaktuTest()
        {
            WaktuTest waktuTest = new WaktuTest();
            koneksi.ConnectionString = con;
            query = "SELECT waktutest1, waktutest2, waktutest3 FROM Config";
            cmd = new SqlCommand(query, koneksi);
            
            koneksi.Open();
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                DateTime T1 = Convert.ToDateTime(reader["waktutest1"].ToString());
                waktuTest.Test1 = T1.ToString("dd-MM-yyyy").Substring(0, 10);
                DateTime T2 = Convert.ToDateTime(reader["waktutest2"].ToString());
                waktuTest.Test2 = T2.ToString("dd-MM-yyyy").Substring(0, 10);
                DateTime T3 = Convert.ToDateTime(reader["waktutest3"].ToString());
                waktuTest.Test3 = T3.ToString("dd-MM-yyyy").Substring(0, 10);
            }

            koneksi.Close();
            return waktuTest;
        }

        public List<DataJurusan> CekDaftarJurusan()
        {
            List<DataJurusan> dataJurusan = new List<DataJurusan>();
            koneksi.ConnectionString = con;
            query = "SELECT * FROM Jurusan";
            cmd = new SqlCommand(query, koneksi);

            koneksi.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                DataJurusan data = new DataJurusan();
                data.Id = Convert.ToInt32(reader["id"].ToString());
                data.Nama = reader["nama"].ToString();
                data.IdFakultas = Convert.ToInt32(reader["id_fakultas"].ToString());
                dataJurusan.Add(data);
            }

            koneksi.Close();
            return dataJurusan;
        }

        public DataJurusan DetailJurusan(int idJurusan)
        {
            DataJurusan jurusan = new DataJurusan();
            koneksi.ConnectionString = con;
            query = "SELECT * FROM Jurusan WHERE id = @idJurusan";
            cmd = new SqlCommand(query, koneksi);
            cmd.Parameters.AddWithValue("@idJurusan", idJurusan);

            koneksi.Open();
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                jurusan.Id = Convert.ToInt32(reader["id"].ToString());
                jurusan.Nama = reader["nama"].ToString();
                jurusan.IdFakultas = Convert.ToInt32(reader["id_fakultas"].ToString());
            }

            koneksi.Close();
            return jurusan;
        }

        public string setJurusan(string noPendaftaran, int id1, int id2)
        {
            string status = cekSetJurusan(id1, id2);

            if (!status.Equals("berhasil"))
                return status;

            DateTime waktuTest = getWaktuTest();

            int idTA = getIDTA();

            int idStatus = getIDStatus();

            koneksi.ConnectionString = con;
            query = "UPDATE Pendaftar SET [waktu_test] = @waktutest, " +
                "[jurusan1] = @jurusan1, " +
                "[jurusan2] = @jurusan2, " +
                "[id_verificator] = @verif, " +
                "[id_tahun_daftar] = @idta, " +
                "[id_status] = @ids WHERE no_pendaftaran = @no_pendaftaran";
            cmd = new SqlCommand(query, koneksi);
            cmd.Parameters.AddWithValue("@Waktutest", waktuTest);
            cmd.Parameters.AddWithValue("@jurusan1", id1);
            cmd.Parameters.AddWithValue("@jurusan2", id2);
            cmd.Parameters.AddWithValue("@verif", "1");
            cmd.Parameters.AddWithValue("@idta", idTA);
            cmd.Parameters.AddWithValue("@ids", idStatus);
            cmd.Parameters.AddWithValue("@no_pendaftaran", noPendaftaran);

            koneksi.Open();
            try
            {
                cmd.ExecuteNonQuery();
                status = "berhasil";
            } catch(Exception e)
            {
                status = "Error "+e.Message;
            }

            koneksi.Close();

            return status;
        }

        private int getIDStatus()
        {
            SqlConnection koneksiS = new SqlConnection();
            koneksiS.ConnectionString = con;
            string queryS = "SELECT id FROM StatusPendaftar";
            SqlCommand cmdS = new SqlCommand(queryS, koneksiS);
            koneksiS.Open();
            SqlDataReader readS = cmdS.ExecuteReader();
            readS.Read();
            int status = Convert.ToInt32(readS["id"]);
            koneksiS.Close();

            return status;
        }

        private int getIDTA()
        {
            SqlConnection koneksiTA = new SqlConnection();
            koneksiTA.ConnectionString = con;
            string queryTA = "SELECT id FROM Tahun";
            SqlCommand cmdTA = new SqlCommand(queryTA, koneksiTA);
            koneksiTA.Open();
            SqlDataReader readTA = cmdTA.ExecuteReader();
            readTA.Read();
            int idTA = Convert.ToInt32(readTA["id"]);
            koneksiTA.Close();

            return idTA;
        }

        private DateTime getWaktuTest()
        {
            DateTime waktuTest = new DateTime();

            koneksi.ConnectionString = con;
            query = "SELECT COUNT(no_pendaftaran) AS 'rows' FROM Pendaftar";
            cmd = new SqlCommand(query, koneksi);

            SqlConnection koneksiWT = new SqlConnection();
            koneksiWT.ConnectionString = con;
            string queryWT = "SELECT waktutest1, waktutest2, waktutest3 FROM Config";
            SqlCommand cmdWT = new SqlCommand(queryWT, koneksiWT);

            koneksi.Open();
            koneksiWT.Open();
            reader = cmd.ExecuteReader();
            SqlDataReader readWT = cmdWT.ExecuteReader();
            readWT.Read();

            if (reader.Read())
            {
                int rows = Convert.ToInt32(reader["rows"]);

                if (rows <= 120)
                {
                    waktuTest = Convert.ToDateTime(readWT["waktutest1"]);
                }
                else if(rows <= 240)
                {
                    waktuTest = Convert.ToDateTime(readWT["waktutest2"]);
                }
                else
                {
                    waktuTest = Convert.ToDateTime(readWT["waktutest3"]);
                }
            }
            else
            {
                waktuTest = Convert.ToDateTime(readWT["waktutest1"]);
            }

            koneksiWT.Close();
            koneksi.Close();

            return waktuTest;
        }

        private string cekSetJurusan(int id1, int id2)
        {
            if (id1.Equals(0) || id2.Equals(0))
                return "Jurusan harus diisi untuk melanjutkan proses";
            else if (id1.Equals(id2))
                return "Jurusan Tidak boleh sama";
            else
                return "berhasil";
        }

        public string UpdateInformasi(string infoData)
        {
            string status = "";

            DataInformasi dataInformasi = new DataInformasi();
            dataInformasi = JsonConvert.DeserializeObject<DataInformasi>(infoData);
            koneksi.ConnectionString = con;
            query = "UPDATE Config SET " +
                "[waktupendaftaranmulai] = @waktuPendMu, " +
                "[waktupendaftaranselesai] = @waktuPendSe, " +
                "[waktupengumuman] = @waktuPeng," +
                "[waktutest1] = @waktuTest1, " +
                "[waktutest2] = @waktuTest2, " +
                "[waktutest3] = @waktuTest3, " +
                "[tahunaktif] = @tahunAktif";
            cmd = new SqlCommand(query, koneksi);
            cmd.Parameters.AddWithValue("@waktuPendMu", dataInformasi.WaktuPendaftaranMulai);
            cmd.Parameters.AddWithValue("@waktuPendSe", dataInformasi.WaktuPendaftaranSelesai);
            cmd.Parameters.AddWithValue("@waktuPeng", dataInformasi.WaktuPengumuman);
            cmd.Parameters.AddWithValue("@waktuTest1", dataInformasi.WaktuTest1);
            cmd.Parameters.AddWithValue("@waktuTest2", dataInformasi.WaktuTest2);
            cmd.Parameters.AddWithValue("@waktuTest3", dataInformasi.WaktuTest3);
            cmd.Parameters.AddWithValue("@tahunAktif", dataInformasi.TahunAktif);
            
            koneksi.Open();
            try
            {
                cmd.ExecuteNonQuery();
                TambahTahun(dataInformasi.TambahTahun);
                status = "berhasil";

            }
            catch (Exception e)
            {
                status = "Error - " + e.Message + " Please Contact the Administrator.";
            }
            koneksi.Close();

            return status;
        }

        public List<Pendaftar> GetAllPendaftar()
        {
            List<Pendaftar> pendaftars = new List<Pendaftar>();
            
            koneksi.ConnectionString = con;
            query = "SELECT * FROM Pendaftar JOIN StatusPendaftar ON Pendaftar.id_status = StatusPendaftar.id " +
                "WHERE id_tahun_daftar = (SELECT Tahun.id FROM Tahun JOIN Config ON Config.tahunaktif = Tahun.tahun)";

            cmd = new SqlCommand(query, koneksi);

            koneksi.Open();
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Pendaftar dataPendaftar = new Pendaftar();
                    dataPendaftar.NoPendaftaran = reader.IsDBNull(0) ? "" : reader["no_pendaftaran"].ToString();
                    dataPendaftar.Nama = reader.IsDBNull(6) ? "" : reader["nama"].ToString();
                    dataPendaftar.JenisKelamin = reader.IsDBNull(8) ? "" : reader["jenis_kelamin"].ToString();
                    dataPendaftar.Alamat = reader.IsDBNull(9) ? "" : reader["alamat"].ToString();
                    dataPendaftar.StatusPendaftar = reader.IsDBNull(21) ? "" : reader[21].ToString();

                    dataPendaftar.Status = "berhasil";
                    pendaftars.Add(dataPendaftar);
                }
            }
            else
            {
                //dataPendaftar.Status = "gagal";
                throw new WebFaultException<string>("Belum ada Pendaftar", System.Net.HttpStatusCode.NoContent);
            }

            koneksi.Close();

            return pendaftars;
        }

        public string VerifikasiPendaftar(string noPendaftaran)
        {
            // TODO : Add Admin ID to id_verificator
            string status = "";

            SqlConnection koneksiIDS = new SqlConnection();
            koneksiIDS.ConnectionString = con;
            string queryIDS = "SELECT id FROM StatusPendaftar WHERE status = @status";
            SqlCommand cmdIDS = new SqlCommand(queryIDS, koneksiIDS);
            cmdIDS.Parameters.AddWithValue("@status", "Terverifikasi");
            
            koneksiIDS.Open();
            reader = cmdIDS.ExecuteReader();
            reader.Read();
            string ids = reader["id"].ToString();
            koneksiIDS.Close();
            
            koneksi.ConnectionString = con;
            query = "UPDATE Pendaftar SET [id_status] = @idstatus WHERE no_pendaftaran = @no_pendaftaran";
            cmd = new SqlCommand(query, koneksi);
            cmd.Parameters.AddWithValue("@idstatus", ids);
            cmd.Parameters.AddWithValue("@no_pendaftaran", noPendaftaran);

            koneksi.Open();
            try
            {
                cmd.ExecuteNonQuery();
                status = "berhasil";
            }
            catch (Exception e)
            {
                status = "Error " + e.Message;
            }

            koneksi.Close();

            return status;
        }

        public List<Pendaftar> DownloadAllPendaftar()
        {
            List<Pendaftar> pendaftars = new List<Pendaftar>();

            koneksi.ConnectionString = con;
            query = "SELECT * FROM Pendaftar " +
                "JOIN Jurusan AS J1 ON Pendaftar.jurusan1 = J1.id " +
                "JOIN Jurusan AS J2 ON Pendaftar.jurusan2 = J2.id " +
                "JOIN Tahun ON Pendaftar.id_tahun_daftar = Tahun.id " +
                "JOIN [User] ON Pendaftar.id_verificator = [User].[id] " +
                "JOIN StatusPendaftar ON Pendaftar.id_status = StatusPendaftar.id " +
                "WHERE Pendaftar.id_tahun_daftar = (SELECT Tahun.id FROM Tahun JOIN Config ON Config.tahunaktif = Tahun.tahun)";

            cmd = new SqlCommand(query, koneksi);

            koneksi.Open();
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    DateTime dateTL = reader.IsDBNull(11) ? new DateTime() : Convert.ToDateTime(reader["tanggal_lahir"].ToString());
                    DateTime dateWT = reader.IsDBNull(12) ? new DateTime() : Convert.ToDateTime(reader["waktu_test"].ToString());

                    Pendaftar dataPendaftar = new Pendaftar();
                    dataPendaftar.NoPendaftaran = reader.IsDBNull(0) ? "" : reader["no_pendaftaran"].ToString();
                    dataPendaftar.Email = reader.IsDBNull(3) ? "" : reader["email"].ToString();
                    dataPendaftar.NoHP = reader.IsDBNull(4) ? "" : reader["nohp"].ToString();
                    dataPendaftar.Nisn = reader.IsDBNull(5) ? "" : reader["nisn"].ToString();
                    dataPendaftar.Nama = reader.IsDBNull(6) ? "" : reader["nama"].ToString();
                    dataPendaftar.AsalSekolah = reader.IsDBNull(7) ? "" : reader["asal_sekolah"].ToString();
                    dataPendaftar.JenisKelamin = reader.IsDBNull(8) ? "" : reader["jenis_kelamin"].ToString();
                    dataPendaftar.Alamat = reader.IsDBNull(9) ? "" : reader["alamat"].ToString();
                    dataPendaftar.TempatLahir = reader.IsDBNull(10) ? "" : reader["tempat_lahir"].ToString();
                    dataPendaftar.NamaOrangTua = reader.IsDBNull(15) ? "" : reader["nama_orang_tua"].ToString();
                    dataPendaftar.PekerjaanOrangTua = reader.IsDBNull(16) ? "" : reader["pekerjaan_orang_tua"].ToString();
                    dataPendaftar.TanggalLahir = dateTL.ToString("dd-MM-yyyy").Substring(0, 10);
                    dataPendaftar.WaktuTest = dateWT.ToString("dd-MM-yyyy HH:mm");
                    dataPendaftar.Jurusan1 = reader.IsDBNull(13) ? 0 : Convert.ToInt32(reader["jurusan1"].ToString());
                    dataPendaftar.Jurusan2 = reader.IsDBNull(14) ? 0 : Convert.ToInt32(reader["jurusan2"].ToString());
                    dataPendaftar.NamaJ1 = reader.IsDBNull(21) ? "" : reader[21].ToString();
                    dataPendaftar.NamaJ2 = reader.IsDBNull(24) ? "" : reader[24].ToString();
                    dataPendaftar.IdVerificator = reader.IsDBNull(17) ? 0 : Convert.ToInt32(reader["id_verificator"].ToString());
                    dataPendaftar.IdStatus = reader.IsDBNull(18) ? 0 : Convert.ToInt32(reader["id_status"].ToString());
                    dataPendaftar.IdTahunDaftar = reader.IsDBNull(27) ? 0 : Convert.ToInt32(reader[27].ToString());
                    dataPendaftar.Verificator = reader.IsDBNull(29) ? "" : reader[29].ToString();
                    dataPendaftar.StatusPendaftar = reader.IsDBNull(33) ? "" : reader[33].ToString();
                    dataPendaftar.Status = "berhasil";

                    pendaftars.Add(dataPendaftar);
                }
            }
            else
            {
                //dataPendaftar.Status = "gagal";
                throw new WebFaultException<string>("Belum ada Pendaftar", System.Net.HttpStatusCode.NoContent);
            }

            koneksi.Close();

            return pendaftars;
        }

        public string HapusPendaftar(string noPendaftaran)
        {
            string status = "";
            
            koneksi.ConnectionString = con;
            query = "DELETE FROM Pendaftar WHERE no_pendaftaran = @no_pendaftaran";
            cmd = new SqlCommand(query, koneksi);
            cmd.Parameters.AddWithValue("@no_pendaftaran", noPendaftaran);

            koneksi.Open();
            try
            {
                cmd.ExecuteNonQuery();
                status = "berhasil";
            }
            catch (Exception e)
            {
                status = "Error " + e.Message;
            }

            koneksi.Close();

            return status;
        }

        public string TambahPengumuman(string pengumumanData)
        {
            string status = "";
            List<Pengumuman> pengumuman = new List<Pengumuman>();
            
            dynamic dirtyData = JsonConvert.DeserializeObject(pengumumanData);

            foreach(dynamic data in dirtyData)
            {
                foreach(dynamic cleanData in data)
                {
                    Pengumuman whatIWant = JsonConvert.DeserializeObject<Pengumuman>(Convert.ToString(cleanData));
                    pengumuman.Add(whatIWant);
                }
            }

            koneksi.ConnectionString = con;
            query = "INSERT INTO [PengumumanPendaftar]([no_pendaftaran], [nama], [nama_jurusan])" +
                "VALUES(@noPendaftaran, @nama, @jurusan)";
            foreach(Pengumuman data in pengumuman)
            {
                cmd = new SqlCommand(query, koneksi);
                cmd.Parameters.AddWithValue("@noPendaftaran", data.NoPendaftaran);
                cmd.Parameters.AddWithValue("@nama", data.Nama);
                cmd.Parameters.AddWithValue("@jurusan", data.Jurusan);

                koneksi.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                    status = "berhasil";
                    UpdateStatusPendaftar(data.NoPendaftaran);
                }
                catch (Exception e)
                {
                    status = "Error - " + e.Message + " Please Contact the Administrator.";
                }
                koneksi.Close();
            }

            return status;
        }

        private void UpdateStatusPendaftar(string noPendaftaran)
        {
            SqlConnection koneksiIDS = new SqlConnection();
            koneksiIDS.ConnectionString = con;
            string queryIDS = "SELECT id FROM StatusPendaftar WHERE status = @status";
            SqlCommand cmdIDS = new SqlCommand(queryIDS, koneksiIDS);
            cmdIDS.Parameters.AddWithValue("@status", "Diterima");

            koneksiIDS.Open();
            reader = cmdIDS.ExecuteReader();
            reader.Read();
            string ids = reader["id"].ToString();
            koneksiIDS.Close();

            SqlConnection koneksiUSP = new SqlConnection();
            koneksiIDS.ConnectionString = con;
            string queryUSP = "UPDATE Pendaftar SET [id_status] = @idstatus WHERE no_pendaftaran = @no_pendaftaran";
            SqlCommand cmdUSP = new SqlCommand(queryUSP, koneksiUSP);
            cmdUSP.Parameters.AddWithValue("@idstatus", ids);
            cmdUSP.Parameters.AddWithValue("@no_pendaftaran", noPendaftaran);

            koneksiUSP.Open();
            cmdUSP.ExecuteNonQuery();
            koneksiUSP.Close();
        }

        public List<Pendaftar> CekPengumumanPendaftar()
        {
            List<Pendaftar> pendaftars = new List<Pendaftar>();

            koneksi.ConnectionString = con;
            query = "SELECT * FROM PengumumanPendaftar " +
                "JOIN Pendaftar ON PengumumanPendaftar.no_pendaftaran = Pendaftar.no_pendaftaran " +
                "WHERE Pendaftar.id_tahun_daftar = (SELECT Tahun.id FROM Tahun JOIN Config ON Config.tahunaktif = Tahun.tahun)";

            cmd = new SqlCommand(query, koneksi);

            koneksi.Open();
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {

                    Pendaftar dataPendaftar = new Pendaftar();
                    dataPendaftar.NoPendaftaran = reader.IsDBNull(0) ? "" : reader["no_pendaftaran"].ToString();
                    dataPendaftar.Nama = reader.IsDBNull(1) ? "" : reader["nama"].ToString();
                    dataPendaftar.JenisKelamin = reader.IsDBNull(8) ? "" : reader["jenis_kelamin"].ToString();
                    dataPendaftar.AsalSekolah = reader.IsDBNull(7) ? "" : reader["asal_sekolah"].ToString();
                    dataPendaftar.NamaJ1 = reader.IsDBNull(3) ? "" : reader["nama_jurusan"].ToString();
                    dataPendaftar.Status = "berhasil";

                    pendaftars.Add(dataPendaftar);
                }
            }
            else
            {
                //dataPendaftar.Status = "gagal";
                throw new WebFaultException<string>("Belum ada Pendaftar", System.Net.HttpStatusCode.NoContent);
            }

            koneksi.Close();

            return pendaftars;
        }

        public List<Pendaftar> CariPengumumanPendaftar(string search)
        {
            List<Pendaftar> pendaftars = new List<Pendaftar>();

            koneksi.ConnectionString = con;
            query = "SELECT * FROM PengumumanPendaftar " +
                "JOIN Pendaftar ON PengumumanPendaftar.no_pendaftaran = Pendaftar.no_pendaftaran " +
                "WHERE PengumumanPendaftar.nama LIKE @nama " +
                "AND Pendaftar.id_tahun_daftar = (SELECT Tahun.id FROM Tahun JOIN Config ON Config.tahunaktif = Tahun.tahun)";

            cmd = new SqlCommand(query, koneksi);
            cmd.Parameters.AddWithValue("@nama", '%' + search + '%');

            koneksi.Open();
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Pendaftar dataPendaftar = new Pendaftar();
                    dataPendaftar.NoPendaftaran = reader.IsDBNull(0) ? "" : reader["no_pendaftaran"].ToString();
                    dataPendaftar.Nama = reader.IsDBNull(1) ? "" : reader["nama"].ToString();
                    dataPendaftar.JenisKelamin = reader.IsDBNull(8) ? "" : reader["jenis_kelamin"].ToString();
                    dataPendaftar.AsalSekolah = reader.IsDBNull(7) ? "" : reader["asal_sekolah"].ToString();
                    dataPendaftar.NamaJ1 = reader.IsDBNull(3) ? "" : reader["nama_jurusan"].ToString();
                    dataPendaftar.Status = "berhasil";

                    pendaftars.Add(dataPendaftar);
                }
            }
            else
            {
                //dataPendaftar.Status = "gagal";
                throw new WebFaultException<string>("Belum ada Pendaftar", System.Net.HttpStatusCode.NoContent);
            }

            koneksi.Close();

            return pendaftars;
        }

        public List<Pendaftar> CariPendaftar(string search)
        {

            List<Pendaftar> pendaftars = new List<Pendaftar>();

            koneksi.ConnectionString = con;
            query = "SELECT * FROM Pendaftar " +
                "JOIN StatusPendaftar ON Pendaftar.id_status = StatusPendaftar.id " +
                "WHERE Pendaftar.nama LIKE @nam " +
                "AND Pendaftar.id_tahun_daftar = (SELECT Tahun.id FROM Tahun JOIN Config ON Config.tahunaktif = Tahun.tahun)";

            cmd = new SqlCommand(query, koneksi);
            cmd.Parameters.AddWithValue("@nama", '%' + search + '%');

            koneksi.Open();
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Pendaftar dataPendaftar = new Pendaftar();
                    dataPendaftar.NoPendaftaran = reader.IsDBNull(0) ? "" : reader["no_pendaftaran"].ToString();
                    dataPendaftar.Nama = reader.IsDBNull(6) ? "" : reader["nama"].ToString();
                    dataPendaftar.JenisKelamin = reader.IsDBNull(8) ? "" : reader["jenis_kelamin"].ToString();
                    dataPendaftar.Alamat = reader.IsDBNull(9) ? "" : reader["alamat"].ToString();
                    dataPendaftar.StatusPendaftar = reader.IsDBNull(21) ? "" : reader[21].ToString();

                    dataPendaftar.Status = "berhasil";

                    pendaftars.Add(dataPendaftar);
                }
            }
            else
            {
                //dataPendaftar.Status = "gagal";
                throw new WebFaultException<string>("Belum ada Pendaftar", System.Net.HttpStatusCode.NoContent);
            }

            koneksi.Close();

            return pendaftars;
        }

        public string TambahTahun(string tahun)
        {
            string status = "";
            
            SqlConnection ttCon = new SqlConnection(con);
            string ttQuery = "INSERT INTO [Tahun]([tahun])" +
                "VALUES(@tahun)";

            if (cekTahun(tahun)!="benar")
            {
                return cekTahun(tahun);
            }

            SqlCommand ttCmd = new SqlCommand(ttQuery, ttCon);
            ttCmd.Parameters.AddWithValue("@tahun", tahun);

            ttCon.Open();
            try
            {
                ttCmd.ExecuteNonQuery();
                status = "berhasil";
            }
            catch (Exception e)
            {
                throw new WebFaultException<string>("Gagal. Silahkan hubungi Administrator.", System.Net.HttpStatusCode.Unused);
            }
            ttCon.Close();

            return status;
        }

        private string cekTahun(string tahun)
        {
            if(int.TryParse(tahun, out int result))
            {
                return "benar";
            }
            else
            {
                throw new WebFaultException<string>("Masukkan tahun berupa angka", System.Net.HttpStatusCode.Unused);
            }

        }

        public List<TahunPendaftaran> CekTahunPendaftaran()
        {
            List<TahunPendaftaran> tahuns = new List<TahunPendaftaran>();

            koneksi.ConnectionString = con;
            query = "SELECT * FROM Tahun";

            cmd = new SqlCommand(query, koneksi);

            koneksi.Open();
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    TahunPendaftaran tahun = new TahunPendaftaran();
                    tahun.IdTahun = reader[0].ToString();
                    tahun.Tahun = reader[1].ToString();

                    tahuns.Add(tahun);
                }
            }
            else
            {
                throw new WebFaultException<string>("Belum ada Tahun", System.Net.HttpStatusCode.NoContent);
            }

            koneksi.Close();

            return tahuns;
        }

        public string CekTahunAktif()
        {
            string tahun = "";

            SqlConnection tsCon = new SqlConnection (con);
            string taquery = "SELECT tahunaktif FROM Config";

            SqlCommand taCmd = new SqlCommand(taquery, tsCon);

            tsCon.Open();
            SqlDataReader taReader = taCmd.ExecuteReader();
            if (taReader.HasRows)
            {
                taReader.Read();
                tahun = taReader[0].ToString();
            }
            else
            {
                throw new WebFaultException<string>("Belum ada Tahun", System.Net.HttpStatusCode.NoContent);
            }

            tsCon.Close();

            return tahun;
        }
        
        public string TambahJurusan(string jurusan)
        {
            string status = "";

            SqlConnection ttCon = new SqlConnection(con);
            string ttQuery = "INSERT INTO [Jurusan]([jurusan])" +
                "VALUES(@jurusan)";

            if (cekTahun(jurusan) != "benar")
            {
                return cekTahun(jurusan);
            }

            SqlCommand ttCmd = new SqlCommand(ttQuery, ttCon);
            ttCmd.Parameters.AddWithValue("@jurusan", jurusan);

            ttCon.Open();
            try
            {
                ttCmd.ExecuteNonQuery();
                status = "berhasil";
            }
            catch (Exception e)
            {
                throw new WebFaultException<string>("Gagal. Silahkan hubungi Administrator.", System.Net.HttpStatusCode.Unused);
            }
            ttCon.Close();

            return status;
        }

        public List<DetailJurusan> GetAllJurusan()
        {
            List<DetailJurusan> dataJurusan = new List<DetailJurusan>();

            koneksi.ConnectionString = con;
            query = "SELECT * FROM Jurusan JOIN Fakultas ON Jurusan.id_fakultas = Fakultas.id";

            cmd = new SqlCommand(query, koneksi);

            koneksi.Open();
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    DetailJurusan jurusan = new DetailJurusan();
                    jurusan.Id = Convert.ToInt32(reader[0]);
                    jurusan.Nama = reader[1].ToString();
                    jurusan.NamaFakultas = reader[4].ToString();

                    dataJurusan.Add(jurusan);
                }
            }
            else
            {
                throw new WebFaultException<string>("Belum ada Jurusan", System.Net.HttpStatusCode.NoContent);
            }

            koneksi.Close();

            return dataJurusan;
        }
    }
}
