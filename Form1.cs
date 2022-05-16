using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;

namespace Dnevnik_2._0
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }
        bool profesor = false;
        public string put, baze;
        public SQLiteConnection conn, conn2, conn3;
        Form2 f2;
        public void login_nastavnika(string u,string p)
        {

            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            conn.Open();
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM Nastavnik";

            sqlite_datareader = sqlite_cmd.ExecuteReader();
            //login nastavnika, proverava se tabela "Nastavnik"
            while (sqlite_datareader.Read())
            {
                if (u == sqlite_datareader.GetString(3))
                {
                    if (p == sqlite_datareader.GetString(4))
                    {
                        //ako se podaci poklapaju druga forma se ucitava
                        f2 = new Form2();
                        f2.id_nastavnika = sqlite_datareader.GetInt16(0);
                        f2.username = u;// f2.password = p;
                        f2.Show();
                        this.Hide();
                    }
                    else
                        MessageBox.Show("Pogresna lozinka");
                }
            }
            conn.Close();
        }
        public void login_ucenika(string u,string p)
        {
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            conn.Open();
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM Ucenik";

            sqlite_datareader = sqlite_cmd.ExecuteReader();
            //login ucenika, proverava se tabela "Ucenika"
            while (sqlite_datareader.Read())
            {
                if (u == sqlite_datareader.GetString(3))
                {
                    if (p == sqlite_datareader.GetString(4))
                    {
                        //ako se podaci poklapaju druga forma se ucitava
                        //Form5 f5 = new Form5();
                        //f5.index = sqlite_datareader.GetInt32(0);
                        //f5.ime = sqlite_datareader.GetString(1);
                        //f5.pol = sqlite_datareader.GetBoolean(5);
                        //f5.Show();
                        //this.Hide();
                    }
                    else
                        MessageBox.Show("Pogresna lozinka");
                }
            }
            conn.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //proverava se koj buttons se klikne
            if (profesor)
            {
                login_nastavnika(textBox1.Text,textBox2.Text);
            }
            else
            {
                login_ucenika(textBox1.Text, textBox2.Text);
            }
        }
        public void pocetni()
        {
            //Pocetni ekran
            button1.Show();
            button2.Show();
            button4.Hide();
            this.Size = new Size(250, 140);
            Putanje();
            groupBox1.Visible = false;
            textBox1.Text = "";
            textBox2.Text = "";

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            pocetni();
        }

        public void Putanje()
        {
            put = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Baze za Dnavnik 2.0");
            if (!System.IO.Directory.Exists(put))
                Directory.CreateDirectory(put);
            baze = "Baze.db";
            //sqlite konekcije
            conn = new SQLiteConnection(String.Format($"Data Source={baze};Version=3;"));
            conn2 = new SQLiteConnection(String.Format($"Data Source={baze};Version=3;"));
            conn3 = new SQLiteConnection(String.Format($"Data Source={baze};Version=3;"));

            SQLiteCommand cmd;
            //Provera da li baza postoji
            if (!File.Exists(baze))
            {
                SQLiteConnection.CreateFile(baze);
                conn.Open();
                cmd = new SQLiteCommand("CREATE TABLE Nastavnik (" +
                                                            "ID_nastavnika INTEGER PRIMARY KEY NOT NULL," +
                                                            "Ime Varchar2(10) not null," +
                                                            "Prezime Varchar2(10) not null," +
                                                            "user_name varchar(20) not null," +
                                                            "password varchar(20) not null);",
                                                          conn);

                
                cmd.ExecuteNonQuery();
                conn.Close();
                conn2.Open();
                cmd = new SQLiteCommand("CREATE TABLE Ucenik (" +
                                                            "ID_ucenika INTEGER PRIMARY KEY," +
                                                            "Ucenik     VARCHAR (30) NOT NULL," +
                                                            "ID_nastavnika INTEGER NOT NULL," +
                                                            "password varchar(20) not null," +
                                                            "Pol Boolean not null," +
                                                            "FOREIGN KEY (ID_nastavnika) REFERENCES Nastavnik (ID_nastavnika));",
                                                          conn2);

                cmd.ExecuteNonQuery();
                conn2.Close();
                conn3.Open();
                cmd = new SQLiteCommand("CREATE TABLE Ocena (" +
                                                            "ID_ocene INTEGER PRIMARY KEY NOT NULL," +
                                                            "ID_ucenika      INTEGER NOT NULL," +
                                                            "Ocena INTEGER NOT NULL," +
                                                            "FOREIGN KEY (ID_ucenika) REFERENCES Ucenik (ID_ucenika));",
                                                          conn3);


                cmd.ExecuteNonQuery();
                conn3.Close();
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            pocetni();
        }

        public void Log_in_scr()
        {
            //Login screen
            button4.Show();
            groupBox1.Visible = true;
            this.Size = new Size(300,300);
            button1.Visible = false;
            button2.Visible = false;

        }
        //proverava se koj je buttnon kliknuo korisnik
        //vazi sa dva voida ispod ovog komentara
        private void button1_Click(object sender, EventArgs e)
        {
            profesor = true;
            Log_in_scr();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            profesor = false;
            Log_in_scr();
        }
    }
}
