﻿using System;
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
        DataJurusan DetailJurusan(int idJurusan);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "setJurusan?noPendaftaran={noPendaftaran}&id1={id1}&id2={id2}"
        )]
        string setJurusan(string noPendaftaran, int id1, int id2);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "updateInformasi?jsonData={infoData}"
        )]
        string UpdateInformasi(string infoData);

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "getAllPendaftar?mulaiDari={mulaiDari}"
        )]
        List<Pendaftar> GetAllPendaftar(int mulaiDari);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "verifikasiPendaftar?noPendaftaran={noPendaftaran}"
        )]
        string VerifikasiPendaftar(string noPendaftaran);

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "downloadAllPendaftar"
        )]
        List<Pendaftar> DownloadAllPendaftar();

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "hapusPendaftar?noPendaftaran={noPendaftaran}"
        )]
        string HapusPendaftar(string noPendaftaran);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "tambahPengumuman?pengumumanData={pengumumanData}"
        )]
        string TambahPengumuman(string pengumumanData);

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "pengumumanPendaftar?mulaiDari={mulaiDari}"
        )]
        List<Pendaftar> pengumumanPendaftar(int mulaiDari);
        
        [OperationContract]
        [WebInvoke(
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "cariPengumumanPendaftar?search={search}"
        )]
        List<Pendaftar> CariPengumumanPendaftar(string search);

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "cariPendaftar?search={search}"
        )]
        List<Pendaftar> CariPendaftar(string search);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "tambahTahun?tahun={tahun}"
        )]
        string TambahTahun(string tahun);

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "cekTahunPendaftaran"
        )]
        List<TahunPendaftaran> CekTahunPendaftaran();

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "cekTahunAktif"
        )]
        string CekTahunAktif();

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "tambahJurusan?namaJurusan={namaJurusan}&idFakultas={idFakultas}"
        )]
        string TambahJurusan(string namaJurusan, string idFakultas);
        
        [OperationContract]
        [WebInvoke(
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "getAllJurusan"
        )]
        List<DetailJurusan> GetAllJurusan();

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "getAllFakultas"
        )]
        List<DetailFakultas> GetAllFakultas();

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "hapusJurusan?idJurusan={idJurusan}"
        )]
        string HapusJurusan(string idJurusan);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "tambahFakultas?namaFakultas={namaFakultas}"
        )]
        string TambahFakultas(string namaFakultas);
        
        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "hapusFakultas?idFakultas={idFakultas}"
        )]
        string HapusFakultas(string idFakultas);

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "cekPengumumanPendaftar?noPendaftaran={noPendaftaran}"
        )]
        string PengumumanPendaftar(string noPendaftaran);

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "paginationPendaftar?whichOne={whichOne}&pages={pages}"
        )]
        List<string> Pagination(string whichOne, int pages);
    }

    [DataContract]
    public class Pendaftar
    {
        string noPendaftaran, nama, email, nohp, nisn, asalSekolah, jenisKelamin, alamat, tempatLahir, namaOrangTua, pekerjaanOrangTua, status;
        int idVerificator, idStatus, idTahunDaftar, jurusan1, jurusan2;
        string tanggalLahir, waktuTest, namaJ1, namaJ2, verificator, statusPendaftar;
        
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
        [DataMember]
        public string NamaJ1 { get => namaJ1; set => namaJ1 = value; }
        [DataMember]
        public string NamaJ2 { get => namaJ2; set => namaJ2 = value; }
        [DataMember]
        public string Verificator { get => verificator; set => verificator = value; }
        [DataMember]
        public string StatusPendaftar { get => statusPendaftar; set => statusPendaftar = value; }
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

    [DataContract]
    public class DataInformasi
    {
        DateTime waktuTest1, waktuTest2, waktuTest3, waktuPengumuman, waktuPendaftaranMulai, waktuPendaftaranSelesai;
        string tahunAktif, tambahTahun;

        [DataMember]
        public DateTime WaktuTest1 { get => waktuTest1; set => waktuTest1 = value; }
        [DataMember]
        public DateTime WaktuTest2 { get => waktuTest2; set => waktuTest2 = value; }
        [DataMember]
        public DateTime WaktuTest3 { get => waktuTest3; set => waktuTest3 = value; }
        [DataMember]
        public DateTime WaktuPengumuman { get => waktuPengumuman; set => waktuPengumuman = value; }
        [DataMember]
        public DateTime WaktuPendaftaranMulai { get => waktuPendaftaranMulai; set => waktuPendaftaranMulai = value; }
        [DataMember]
        public DateTime WaktuPendaftaranSelesai { get => waktuPendaftaranSelesai; set => waktuPendaftaranSelesai = value; }
        [DataMember]
        public string TahunAktif { get => tahunAktif; set => tahunAktif = value; }
        [DataMember]
        public string TambahTahun { get => tambahTahun; set => tambahTahun = value; }
    }

    [DataContract]
    public class Pengumuman
    {
        [DataMember]
        public string NoPendaftaran { get; set; }
        [DataMember]
        public string Nama { get; set; }
        [DataMember]
        public string Jurusan { get; set; }
    }

    [DataContract]
    public class TahunPendaftaran
    {
        [DataMember]
        public string IdTahun { get; set; }
        [DataMember]
        public string Tahun { get; set; }
    }

    [DataContract]
    public class DetailJurusan
    {
        [DataMember]
        public string Nama { get; set; }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string NamaFakultas { get; set; }
    }

    public class DetailFakultas
    {
        [DataMember]
        public string Nama { get; set; }
        [DataMember]
        public int Id { get; set; }
    }
}

