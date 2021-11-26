using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Security.Cryptography;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Data.OleDb;

namespace Main_Menu
{
    /// <summary>
    /// Interaction logic for Log_in_page.xaml
    /// </summary>
    public partial class Log_in_page : Page
    {
        Frame parentFrame;
        OleDbConnection cn;
        string connstring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\\projectDB.accdb";
        public Log_in_page(Frame parentFrame)
        {
            InitializeComponent();
            this.parentFrame = parentFrame;
            cn = new OleDbConnection(connstring);
        }

        private void LoginSubmit_Click(object sender, RoutedEventArgs e)
        {
            string getUserCountQuery = "SELECT Username, [Password] FROM Users;"; //optimization: use WHERE to select a specific user
            string user = login_username_entry.Text;
            string pass = login_pass_entry.Text;

 

            SHA256 hasher = SHA256.Create();
            byte[] hashPass = Encoding.ASCII.GetBytes(pass);
            byte[] hashUser = Encoding.ASCII.GetBytes(user);
            byte[] encryptedPass = hasher.ComputeHash(hashPass);
            byte[] encryptedUser = hasher.ComputeHash(hashUser);
            SoapHexBinary hexPass = new SoapHexBinary(encryptedPass);
            SoapHexBinary hexUser = new SoapHexBinary(encryptedUser); //Convert to hex

            OleDbCommand cmd = new OleDbCommand(getUserCountQuery, cn);
            cn.Open();
            OleDbDataReader read = cmd.ExecuteReader();
            string data = "";
            while (read.Read())
            {
                data += read[0].ToString() + " " + read[1].ToString() + " "; //read[0] = username, read[1] = password
            }
            read.Close();
            cn.Close();

            int i = 0;
            int found = 0;

            while (data.Substring(i).IndexOf(' ') != -1) //while there is whitespace past i... (there is still data)
            {
                int index = data.IndexOf(' ', i); //first occurance of whitespace starting at i
                int nextIndex = data.IndexOf(' ', index + 1); //next whitespace starting at index
                string dataUser = data.Substring(i, index - i); //username string
                string dataPass = data.Substring(index + 1, nextIndex - (index + 1)); //password string
                i = nextIndex + 1; //start of the next username

                if (dataUser == hexUser.ToString() && dataPass == hexPass.ToString())
                {
                    found++;
                    break;
                }

      
            }

            if (found == 1)
            {
                //get userID from db
                string getUserID = $"SELECT UserID, Balance FROM Users WHERE (Username) = '{hexUser}'";
                OleDbCommand cmd2 = new OleDbCommand(getUserID, cn);
                cn.Open();
                OleDbDataReader read2 = cmd2.ExecuteReader();
                string uidData = "";
                while (read2.Read())
                {
                    uidData += read2[0].ToString() + " " + read2[1].ToString(); //read2[0] = user id, read2[1] = balance
                }
                read2.Close();
                cn.Close();
                int index = uidData.IndexOf(" ");
                int uid = Convert.ToInt32(uidData.Substring(0, index));
                int balance = Convert.ToInt32(uidData.Substring(index + 1));
                MessageBox.Show("Logged in!");
                parentFrame.Content = new MainMenu_Page(parentFrame, uid, user, balance);
            }
            else
            {
                login_error_label.Content = "Incorrect username or password.";
            }
        }

        private void SignUpSubmit_Click(object sender, RoutedEventArgs e)
        {
            string numbers = "1234567890";
            string symbols = "!@#$%^&*(){}[]|/;:.,`-+=-_<>~?";
            string upperCaseLetters = "QWERTYUIOPASDFGHJKLZXCVBNM";
            bool upperFound = false;
            bool numberFound = false;
            bool symbolFound = false;
            string user = sign_up_username_entry.Text;

            //Sanity check on the username
            string getUserQuery = "SELECT Username FROM Users;";
            OleDbCommand cmd = new OleDbCommand(getUserQuery, cn);
            cn.Open();
            OleDbDataReader read = cmd.ExecuteReader();
            string data = "";
            while (read.Read())
            {
                data += read[0].ToString() + " "; //read[0] = usernames
            }
            cn.Close();

            //encrypt username
            SHA256 userHasher = SHA256.Create();
            byte[] byteUser = Encoding.ASCII.GetBytes(user);
            byte[] hash = userHasher.ComputeHash(byteUser);
            SoapHexBinary encrypted = new SoapHexBinary(hash);

            //search data for the same username
            int j = 0;
            int found = 0;

            while (data.Substring(j).IndexOf(' ') != -1) //while there is whitespace past j... (there is still data)
            {
                int index = data.IndexOf(' ', j); //first occurance of whitespace starting at j
                string dataUser = data.Substring(j, index - j); //username string
                j = index + 1; //start of the next username

                if (dataUser == encrypted.ToString())
                {
                    found++;
                    break;
                }
            }

            if (found == 1)
            {
                sign_up_error_label.Content = "Username already taken!";
            }

            else
            {
                //Sanity check on the password
                string pass = sign_up_pass_entry.Text;
                    if (pass != confirm_pass_entry.Text)
                    {
                        sign_up_error_label.Content = "Passwords do not match.";
                    }
                    else if (pass.Length < 10)
                    {
                        sign_up_error_label.Content = "Passwords must have at least 10 characters.";
                    }
                    else
                    {
                        for (int i = 0; i < pass.Length; i++)
                        {
                            if (numbers.Contains(pass[i]) && !numberFound) numberFound = true; //character is a number and no numbers have been found?
                            else if (symbols.Contains(pass[i]) && !symbolFound) symbolFound = true; //character is a symbol and no symbols have been found?
                            else if (upperCaseLetters.Contains(pass[i]) && !upperFound) upperFound = true; //character is uppercase and no uppercase letters have been found?
                        }

                        if (upperFound && numberFound && symbolFound) //if the password is valid
                        {
                            //Encrypt username & password
                            SHA256 hasher = SHA256.Create();
                            byte[] hashPass = Encoding.ASCII.GetBytes(pass);
                            byte[] hashUser = Encoding.ASCII.GetBytes(user);
                            byte[] encryptedPass = hasher.ComputeHash(hashPass);
                            byte[] encryptedUser = hasher.ComputeHash(hashUser);
                            SoapHexBinary hexPass = new SoapHexBinary(encryptedPass);
                            SoapHexBinary hexUser = new SoapHexBinary(encryptedUser); //Convert to hex

                            string getUserCountQuery = "SELECT Count(UserID) FROM Users;";

                            cmd = new OleDbCommand(getUserCountQuery, cn);
                            cn.Open();
                            read = cmd.ExecuteReader();
                            data = "";
                            while (read.Read())
                            {
                                data += read[0].ToString(); //read[0] = number of users in db
                            }
                            cn.Close();
                            int newID = Convert.ToInt32(data) + 1;

                            //Send to db

                            OleDbCommand cmd2 = new OleDbCommand();
                            cmd2.CommandType = System.Data.CommandType.Text;
                            cmd2.CommandText = "INSERT INTO Users ([UserID],[Balance],[Username],[Password]) VALUES (?,?,?,?)";
                            cmd2.Parameters.AddWithValue("@id", newID.ToString());
                            cmd2.Parameters.AddWithValue("@balance", 5000.ToString());
                            cmd2.Parameters.AddWithValue("@username", hexUser.ToString());
                            cmd2.Parameters.AddWithValue("@password", hexPass.ToString());
                            cmd2.Connection = cn;
                            cn.Open();
                            cmd2.ExecuteNonQuery();
                            cn.Close();
                            MessageBox.Show("User added!");


                        //Load Main Menu Page
                        parentFrame.Content = new MainMenu_Page(parentFrame, newID, user, 5000);
                        }
                        else
                        {
                            sign_up_error_label.Content = "Number, symbol, or uppercase letter is missing in password.";
                        }
                    }
            }
            
            
            
        }
    }
}
