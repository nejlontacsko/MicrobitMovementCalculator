using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MicrobitMovementCalculator
{
    public partial class ConnectionForm : Form
    {
        public ConnectionForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();

            comboBox1.Items.Clear();
            foreach (var item in ports)
                comboBox1.Items.Add(item);

            comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
            button2.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SerialHandler.Instance.Init(comboBox1.SelectedItem.ToString());
            SerialHandler.Instance.Start();
            DialogResult = SerialHandler.Instance.IsOpened ? DialogResult.OK : DialogResult.Abort;
        }
    }
}
