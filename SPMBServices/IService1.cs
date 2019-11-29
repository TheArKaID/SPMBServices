using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SPMBServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        // TODO: Add your service operations here
        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "daftar?username={username}&password={password}&repassword={repassword}&email={email}&nohp={nohp}&agreement={agreement}"
        )]
        DaftarPendaftar Daftar(string username, string password, string repassword, string email, string nohp, string agreement);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "updatePendaftar?jsonData={jsonData}"
        )]
        string UpdatePendaftar(string jsonData);

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "cekDataPendaftar?noPendaftaran={noPendaftaran}"
        )]
        Pendaftar CekDataPendaftar(string noPendaftaran);

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "loginPendaftar?username={username}&password={password}"
        )]
        LoginPendaftar LoginPendaftar(string username, string password);

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "waktuPendaftaran"
        )]
        WaktuPendaftaran CekWaktuPendaftaran();

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "waktuPengumuman"
        )]
        WaktuPengumuman CekWaktuPengumuman();

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "loginAdmin?username={username}&password={password}"
        )]
        LoginAdmin LoginAdmin(string username, string password);

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "waktuTest"
        )]
        WaktuTest CekWaktuTest();

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "cekDaftarJurusan"
        )]
        List<DataJurusan> CekDaftarJurusan();

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "dataJurusan?id={idJurusan}"
        )]
        DataJurusan CekJurusan(int idJurusan);
    }

    [DataContract]
    public class Pendaftar
    {
        string noPendaftaran, nama, email, nohp, nisn, asalSekolah, jenisKelamin, alamat, tempatLahir, namaOrangTua, pekerjaanOrangTua, status;
        int idVerificator, idStatus, idTahunDaftar, jurusan1, jurusan2;
        string tanggalLahir, waktuTest;
        
        [DataMember]
        public string NoPendaftaran { get => noPendaftaran; set => noPendaftaran = value; }
        [DataMember]
        public string Nama { get => nama; set => nama = value; }
        [DataMember]
        public string NoHP { get => nohp; set => nohp = value; }
        [DataMember]
        public string Nisn { get => nisn; set => nisn = value; }
        [DataMember]
        public string Email { get => email; set => email = value; }
        [DataMember]
        public string AsalSekolah { get => asalSekolah; set => asalSekolah = value; }
        [DataMember]
        public string JenisKelamin { get => jenisKelamin; set => jenisKelamin = value; }
        [DataMember]
        public string Alamat { get => alamat; set => alamat = value; }
        [DataMember]
        public string TempatLahir { get => tempatLahir; set => tempatLahir = value; }
        [DataMember]
        public string NamaOrangTua { get => namaOrangTua; set => namaOrangTua = value; }
        [DataMember]
        public string PekerjaanOrangTua { get => pekerjaanOrangTua; set => pekerjaanOrangTua = value; }
        [DataMember]
        public int IdVerificator { get => idVerificator; set => idVerificator = value; }
        [DataMember]
        public int IdStatus { get => idStatus; set => idStatus = value; }
        [DataMember]
        public int IdTahunDaftar { get => idTahunDaftar; set => idTahunDaftar = value; }
        [DataMember]
        public int Jurusan1 { get => jurusan1; set => jurusan1 = value; }
        [DataMember]
        public int Jurusan2 { get => jurusan2; set => jurusan2 = value; }
        [DataMember]
        public string Status { get => status; set => status = value; }
        [DataMember]
        public string TanggalLahir { get => tanggalLahir; set => tanggalLahir = value; }
        [DataMember]
        public string WaktuTest { get => waktuTest; set => waktuTest = value; }
    }

    [DataContract]
    public class LoginPendaftar
    {
        private string nama;
        private string username;
        private string noPendaftaran;
        private string status;

        [DataMember]
        public string Username { get => username; set => username = value; }
        [DataMember]
        public string NoPendaftaran { get => noPendaftaran; set => noPendaftaran = value; }
        [DataMember]
        public string Nama { get => nama; set => nama = value; }
        [DataMember]
        public string Status { get => status; set => status = value; }
    }

    [DataContract]
    public class WaktuPendaftaran
    {
        private string selesai;
        private string mulai;

        [DataMember]
        public string Mulai { get => mulai; set => mulai = value; }
        [DataMember]
        public string Selesai { get => selesai; set => selesai = value; }
    }

    [DataContract]
    public class DaftarPendaftar
    {
        private string username, nama, noPendaftaran, status;

        [DataMember]
        public string Username { get => username; set => username = value; }
        [DataMember]
        public string Nama { get => nama; set => nama = value; }
        [DataMember]
        public string NoPendaftaran { get => noPendaftaran; set => noPendaftaran = value; }
        [DataMember]
        public string Status { get => status; set => status = value; }
    }

    [DataContract]
    public class WaktuPengumuman
    {
        private string tanggal;

        [DataMember]
        public string Tanggal { get => tanggal; set => tanggal = value; }
    }

    [DataContract]
    public class LoginAdmin
    {
        private string nama;
        private string status;
        private string username, id;

        [DataMember]
        public string Nama { get => nama; set => nama = value; }
        [DataMember]
        public string Status { get => status; set => status = value; }
        [DataMember]
        public string Username { get => username; set => username = value; }
        [DataMember]
        public string Id { get => id; set => id = value; }
    }

    [DataContract]
    public class WaktuTest
    {
        string test1, test2, test3;

        [DataMember]
        public string Test1 { get => test1; set => test1 = value; }
        [DataMember]
        public string Test2 { get => test2; set => test2 = value; }
        [DataMember]
        public string Test3 { get => test3; set => test3 = value; }
    }

    [DataContract]
    public class DataJurusan
    {
        string nama;
        int id, idFakultas;

        [DataMember]
        public string Nama { get => nama; set => nama = value; }
        [DataMember]
        public int Id { get => id; set => id = value; }
        [DataMember]
        public int IdFakultas { get => idFakultas; set => idFakultas = value; }

    }
}

