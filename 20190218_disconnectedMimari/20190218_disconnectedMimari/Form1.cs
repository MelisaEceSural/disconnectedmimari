using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace _20190218_disconnectedMimari
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Server=.;Database=Northwind2;Integrated Security=true");
        private void Form1_Load(object sender, EventArgs e)
        {
            getUrunler();
        }

        private void getUrunler()
        {
            

            SqlDataAdapter adp = new SqlDataAdapter("select * from Urunler", baglanti);
            // Eğer sorgu sonucunda tek bir tablo dönüyorsa DataTable nesnesini kullanabiliriz.
            DataTable dt = new DataTable();
            //Sorgudan dönen tabloyu datatable nesnesibe aktaralım
            adp.Fill(dt);
            //grid'i datatable ile dolduralım
            grdUrunler.DataSource = dt;

            // benim 2 comboya ilgili verileri yüklemem lazım
            SqlDataAdapter adpKatTed = new SqlDataAdapter("Select * from Kategoriler Select * from Tedarikciler", baglanti);

            DataSet ds = new DataSet();// Dataset içinde birden fazla datatable bulunduran nesnedir. Sorgu sonucunda birden fazla tablo döneceği için bu tabloları datatable ile değil dataset ile karşılıyorum.
            adpKatTed.Fill(ds);

            cmbKategori.DataSource = ds.Tables[0];// Kategoriler
            cmbKategori.DisplayMember = "KategoriAdi";// Kategori Adı gözüksün
            cmbKategori.ValueMember = "KategoriID";// KategoriId ile işlem yapılsın


            cmbTedarikci.DataSource = ds.Tables[1];//Tedarikçiler
            cmbTedarikci.DisplayMember = "SirketAdi";
            cmbTedarikci.ValueMember = "TedarikciID";

            txtAdi.Text = "";
            numAdet.Value = numFiyat.Value = 0;
            cmbKategori.SelectedIndex = cmbTedarikci.SelectedIndex = 0;
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAdi.Text) || numAdet.Value == 0 || numFiyat.Value == 0)
            {
                MessageBox.Show("Lütfen ürün bilgilerini boş geçmeyin.");
                return;
            }

            SqlCommand cmd = new SqlCommand("Insert into Urunler(UrunAdi, BirimFiyati, HedefStokDuzeyi, KategoriID, TedarikciID) values(@Adi, @Fiyat, @Stok, @Kat, @Ted)", baglanti);
            cmd.Parameters.AddWithValue("@Adi", txtAdi.Text);
            cmd.Parameters.AddWithValue("@Fiyat", numFiyat.Value);
            cmd.Parameters.AddWithValue("@Stok", numAdet.Value);
            cmd.Parameters.AddWithValue("@Kat", cmbKategori.SelectedValue);
            cmd.Parameters.AddWithValue("@Ted", cmbTedarikci.SelectedValue);
            baglanti.Open();
            int s = cmd.ExecuteNonQuery();
            baglanti.Close();
            if (s != 0) MessageBox.Show("Ürün eklendi.");
            else MessageBox.Show("Ürün eklenirken hata oluştu.");

            getUrunler();

        }
    }
}
