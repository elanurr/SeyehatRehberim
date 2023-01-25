using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SeyehatRehberim
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GirisKontrol();
        }

        protected void Haber1_Click(object sender, CommandEventArgs e)
        {
            Response.Redirect("KahvaltiMekanlari.aspx");
        }

        protected void Haber2_Click(object sender, CommandEventArgs e)
        {
            Response.Redirect("Oteller.aspx");
        }

        protected void Haber3_Click(object sender, CommandEventArgs e)
        {
            Response.Redirect("Restorantlar.aspx");
        }

        protected void UyeOl_Click(object sender, CommandEventArgs e)
        {
            if (string.IsNullOrEmpty(KullaniciAdi.Text) ||
                string.IsNullOrEmpty(Sifre.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Bilgileri eksiksiz giriniz.'); window.location='Default.aspx';", true);
            }
            else
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = "Server=localhost\\SQLEXPRESS;Database=SeyehatRehberim;Trusted_Connection=true";
                    conn.Open();

                    SqlCommand selectCommand = new SqlCommand("SELECT * FROM Kullanicilar WHERE KullaniciAdi = @KullaniciAdi", conn);
                    selectCommand.Parameters.Add(new SqlParameter("KullaniciAdi", KullaniciAdi.Text));

                    string kullaniciAdi = "";
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            kullaniciAdi = reader["KullaniciAdi"].ToString();
                        }
                    }

                    if (string.IsNullOrEmpty(kullaniciAdi))
                    {
                        SqlCommand insertCommand = new SqlCommand("INSERT INTO Kullanicilar (KullaniciAdi, Sifre) VALUES (@KullaniciAdi, @Sifre)", conn);
                        insertCommand.Parameters.Add(new SqlParameter("KullaniciAdi", KullaniciAdi.Text));
                        insertCommand.Parameters.Add(new SqlParameter("Sifre", Sifre.Text));
                        insertCommand.ExecuteNonQuery();

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Üye başarıyla oluşturuldu.'); window.location='Default.aspx';", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Üye zaten kayıtlı.'); window.location='Default.aspx';", true);
                    }
                }
            }
        }

        protected void GirisYap_Click(object sender, CommandEventArgs e)
        {
            if (string.IsNullOrEmpty(KullaniciAdi.Text) ||
                string.IsNullOrEmpty(Sifre.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Bilgileri eksiksiz giriniz.'); window.location='Default.aspx';", true);
            }
            else
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = "Server=localhost\\SQLEXPRESS;Database=SeyehatRehberim;Trusted_Connection=true";
                    conn.Open();
                    SqlCommand selectCommand = new SqlCommand("SELECT * FROM Kullanicilar WHERE KullaniciAdi = @KullaniciAdi and Sifre = @Sifre", conn);
                    selectCommand.Parameters.Add(new SqlParameter("KullaniciAdi", KullaniciAdi.Text));
                    selectCommand.Parameters.Add(new SqlParameter("Sifre", Sifre.Text));

                    string kullaniciAdi = "";
                    string sifre = "";
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            kullaniciAdi = reader["KullaniciAdi"].ToString();
                            sifre = reader["Sifre"].ToString();
                        }
                    }

                    if (string.IsNullOrEmpty(kullaniciAdi) ||
                        string.IsNullOrEmpty(sifre))
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Kullanıcı adı veya şifre yanlış.'); window.location='Default.aspx';", true);
                    }
                    else
                    {
                        Session["KullaniciAdi"] = kullaniciAdi;
                        Session["Sifre"] = sifre;

                        GirisKontrol();

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Başarıyla giriş yapıldı.'); window.location='Default.aspx';", true);
                    }
                }
            }
        }

        protected void CikisYap_Click(object sender, CommandEventArgs e)
        {
            if (Session["KullaniciAdi"] != null)
            {
                Session["KullaniciAdi"] = null;
                Session["Sifre"] = null;

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Başarıyla çıkış yapıldı.'); window.location='Default.aspx';", true);
            }
        }

        private void GirisKontrol()
        {
            if (Session["KullaniciAdi"] != null)
            {
                KullaniciAdi.Text = Session["KullaniciAdi"].ToString();
                KullaniciAdi.Enabled = false;
                SifreLabel.Visible = false;
                Sifre.Visible = false;
                UyeOlBtn.Visible = false;
                GirisYapBtn.Visible = false;
                CikisYapBtn.Visible = true;
            }
        }
    }
}