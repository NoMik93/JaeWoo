using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace jaewoo
{
    public partial class Form2 : Form
    {
        Image image;
        public Form2()
        {
            InitializeComponent();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
        }

        public Image Picture
        {
            get
            {
                return image;
            }
            set
            {
                image = value;
                pictureBox1.Image = image;
            }
        }

        private void Form2_KeyDown(object sender, KeyEventArgs e)
        {
            this.Close();
        }

        private void Form2_MouseClick(object sender, MouseEventArgs e)
        {
            this.Close();
        }

        private void PictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            this.Close();
        }
    }
}
