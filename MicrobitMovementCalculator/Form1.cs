using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO.Ports;
using System.Security.Principal;

namespace MicrobitMovementCalculator
{
    public partial class Form1 : Form
    {
        bool isConnected = false;
        int validRecv = 0, invalidRecv = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void swapBtns()
        {
            button1.Enabled = !isConnected;
            button2.Enabled = isConnected;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!SerialHandler.Instance.IsOpened)
                isConnected = new ConnectionForm().ShowDialog() == DialogResult.OK;

            swapBtns();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (SerialHandler.Instance.IsOpened)
            {
                SerialHandler.Instance.Close();
                isConnected = SerialHandler.Instance.IsOpened;
                if (!isConnected)
                    swapBtns();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (isConnected)
            {
                lblConnected.Text = "IGEN!";
                List<string> buffer = SerialHandler.Instance.ReceivedMessages;
                SerialHandler.Instance.ClearBuffer();

                foreach (string item in buffer)
                {
                    if (IsValidJson(item))
                    {
                        //listView1.Items.Add(new ListViewItem(item));
                        SerialPacket sp = JsonConvert.DeserializeObject<SerialPacket>(item);
                        listView1.Items.Add(sp.ToString());
                        validRecv++;
                    }
                    else
                        invalidRecv++;
                    //listView1.Items.Add(new ListViewItem(item));
                }

                listView1.Items[listView1.Items.Count - 1].EnsureVisible();

                lblValid.Text = validRecv.ToString();
                lblInvalid.Text = invalidRecv.ToString();
            }
            else
            {
                listView1.Items.Clear();
                lblConnected.Text = "Nem.";
                lblValid.Text = "-";
                lblInvalid.Text = "-";
                invalidRecv = validRecv = 0;
            }
        }

        private static bool IsValidJson(string strInput)
        {
            if (string.IsNullOrWhiteSpace(strInput)) { return false; }
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    //Exception in parsing json
                    Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex) //some other exception
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}