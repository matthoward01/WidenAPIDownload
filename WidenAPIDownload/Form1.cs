using Newtonsoft.Json.Linq;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace WidenAPIDownload
{
    public partial class Form1 : Form
    {
        WebClient webClient;
        Stopwatch sw = new Stopwatch();
        string downloadPath = @"C:\Temp\ShawAPI\Download\";
        string tempPath = @"C:\Temp\";

        public Form1()
        {
            InitializeComponent();
            Directory.CreateDirectory(downloadPath);
            Directory.CreateDirectory(tempPath);
        }
        private void DownloadFile(string urlAddress, string filename)
        {
            using (webClient = new WebClient())
            {
                string httpType = "https://";
                
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);

                // The variable that will be holding the url address (making sure it starts with http://)
                Uri URL = urlAddress.StartsWith(httpType, StringComparison.OrdinalIgnoreCase) ? new Uri(urlAddress) : new Uri(httpType + urlAddress);

                // Start the stopwatch which we will be using to calculate the download speed
                sw.Start();

                try
                {
                    // Start downloading the file
                    webClient.DownloadFileAsync(URL, tempPath + filename + "." + ".zip");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            // Calculate download speed and output it to labelSpeed.
            //labelSpeed.Text = string.Format("{0} kb/s", (e.BytesReceived / 1024d / sw.Elapsed.TotalSeconds).ToString("0.00"));

            // Update the progressbar percentage only when the value is not the same.
            progressBar1.Value = e.ProgressPercentage;

            // Show the percentage on our label.
            //labelPerc.Text = e.ProgressPercentage.ToString() + "%";

            // Update the label with how much data have been downloaded so far and the total size of the file we are currently downloading
            /*labelDownloaded.Text = string.Format("{0} MB's / {1} MB's",
                (e.BytesReceived / 1024d / 1024d).ToString("0.00"),
                (e.TotalBytesToReceive / 1024d / 1024d).ToString("0.00"));*/
        }
        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            // Reset the stopwatch.
            sw.Reset();

            if (e.Cancelled == true)
            {
                MessageBox.Show("Download has been canceled.");
            }
            else
            {
                //Process(downloadName);
                MessageBox.Show("Download completed!");
            }
        }
                
        private void downloadButton_Click(object sender, EventArgs e)
        {
            string filename = filenameTextBox.Text;
            string userName = "matt.howard@brownind.com";
            string userPassword = "Brown2020";
            string url = "https://shawfloors.widencollective.com/api/rest/asset/search/" + filename + ".zip?options=downloadUrl&metadata=roomsceneCX52colorcorrect";
            const string widenUrl = "https://shawfloors.widencollective.com/login.loginform";
            string getJSON = "";



            // validate your login! 


            //string getJSON = client.DownloadString(url);

            JObject o1 = JObject.Parse(File.ReadAllText(getJSON));

            string downloadUrl = (string)o1["assets"][0]["downloadUrl"];
            string cc = (string)o1["assets"][0]["metadata"][0]["value"];
            string results = (string)o1["numResults"];
            if (cc == null)
            {
                cc = "No";
            }

            urlTextBox.Text += downloadUrl + "\r\n";
            urlTextBox.Text += cc + "\r\n";
            urlTextBox.Text += results + "\r\n";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string jsonString = @"C:\Users\mhoward\Desktop\Brown Automation Testing\0736V Endura 512C Plus J96 00059994 48.zip.json";

            JObject o1 = JObject.Parse(File.ReadAllText(jsonString));

            string downloadUrl = (string)o1["assets"][0]["downloadUrl"];
            string cc = (string)o1["assets"][0]["metadata"][0]["value"];
            string results = (string)o1["numResults"];
            if (cc == null)
            {
                cc = "No";
            }
            urlTextBox.Text += "Download Url: " + downloadUrl + "\r\n\r\n";
            urlTextBox.Text += "Is it CC: " + cc + "\r\n\r\n";
            urlTextBox.Text += "How Many Results: " + results + "\r\n\r\n";
        }
    }    
}



