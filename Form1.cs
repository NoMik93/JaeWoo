using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Diagnostics;

namespace jaewoo
{
    public partial class Form1 : Form
    {
        string select_path;
        string image_folder_path;
        int page_num = 0, max_num = 0;
        int selected_image_num = 0;
        string key = "devU01TX0FVVEgyMDIwMDExNjIzNTc0MDEwOTQwMDk=";

        bool p1 = false, p2 = false, p3 = false, p4 = false, p5 = false, p6 = false, p7 = false, p8 = false, p9 = false;
        ArrayList image_files = new ArrayList();

        public Form1()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void Button_open_folder_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.ShowDialog();
            if(folderBrowserDialog.SelectedPath == null || folderBrowserDialog.SelectedPath == "")
            {
                return;
            }
            select_path = folderBrowserDialog.SelectedPath;
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(select_path);
            string[] folder_name = select_path.Split('\\');
            listBox_folder.Items.Clear();
            foreach (var item in di.GetDirectories())
            {
                listBox_folder.Items.Add(item.Name);
            }
            label_folder.Text = folder_name[folder_name.Length - 1];
            label_folder_number.Text = listBox_folder.Items.Count + "개";
        }

        private void Button_change_folder_name_Click(object sender, EventArgs e)
        {
            if (textBox_si.Text == "" || textBox_s.Text == "" || textBox_d.Text == "" || textBox_type.Text == "" || textBox_n.Text == "" || textBox_r.Text == "")
            {
                MessageBox.Show("비어있는 칸이 있습니다.");
                return;
            }

            if (listBox_folder.SelectedItem == null)
            {
                MessageBox.Show("폴더를 선택해주세요.");
                return;
            }
            String new_name = "00000_" + textBox_si.Text +"_" + textBox_s.Text + "_" + textBox_d.Text + "_" + textBox_type.Text + "_000_" + textBox_d.Text + " " + textBox_n.Text + " " + textBox_type.Text + "(" + textBox_r.Text + ")";
            String target_path = select_path + "\\" + new_name;
            try
            {
                System.IO.Directory.Move(image_folder_path, target_path);
            }catch
            {
                MessageBox.Show("이름을 변경하는데 실패하였습니다.");
                return;
            }

            image_folder_path = target_path;

            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(select_path);
            listBox_folder.Items.Clear();
            foreach (var item in di.GetDirectories())
            {
                listBox_folder.Items.Add(item.Name);
            }
            listBox_folder.ClearSelected();
            int index = 0;
            foreach(var item in listBox_folder.Items)
            {
                if (item.ToString().Equals(new_name))
                    break;
                index++;
            }
            if (listBox_folder.Items.Count != index)
            {
                listBox_folder.SetSelected(index, true);
            }
            listBox_folder.Focus();

            if (checkBox_si.Checked == false)
            {
                textBox_si.Text = "";
            }
            if (checkBox_s.Checked == false)
            {
                textBox_s.Text = "";
            }
            if (checkBox_d.Checked == false)
            {
                textBox_d.Text = "";
            }
            if (checkBox_type.Checked == false)
            {
                textBox_type.Text = "";
            }
        }

        private void Button_left_image_Click(object sender, EventArgs e)
        {
            ImagePageDown();
        }

        private void Button_right_image_Click(object sender, EventArgs e)
        {
            ImagePageUp();
        }
        private void PictureReset()
        {
            pictureBox1.Image = null;
            pictureBox2.Image = null;
            pictureBox3.Image = null;
            pictureBox4.Image = null;
            pictureBox5.Image = null;
            pictureBox6.Image = null;
            pictureBox7.Image = null;
            pictureBox8.Image = null;
            pictureBox9.Image = null;
        }

        private void Button_image_name_Click(object sender, EventArgs e)
        {
            if(selected_image_num < 0)
            {
                MessageBox.Show("이미지를 선택해주세요.");
                return;
            }

            String old_path = image_folder_path + "\\" + image_files[(page_num * 9) + selected_image_num - 1].ToString();
            String file_name = listBox_folder.SelectedItem.ToString() + "_" + textBox_image_name.Text + ".jpg";
            String new_path = image_folder_path + "\\" + file_name;

            if (System.IO.File.Exists(new_path))
            {
                MessageBox.Show("이미 존재하는 이름입니다.");
                return;
            }
            System.IO.File.Move(old_path, new_path);
            image_files[(page_num * 9) + selected_image_num - 1] = file_name;
            label_image_name.Text = image_files[(page_num * 9) + selected_image_num - 1].ToString();
        }

        private void ListBox_folder_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox_folder.SelectedItem != null)
            {
                image_folder_path = select_path + "\\" + listBox_folder.SelectedItem.ToString();
            }else
            {
                return;
            }
            label_image_name.Text = "";

            if (checkBox_si.Checked == false)
            {
                textBox_si.Text = "";
            }
            if (checkBox_s.Checked == false)
            {
                textBox_s.Text = "";
            }
            if (checkBox_d.Checked == false)
            {
                textBox_d.Text = "";
            }
            if (checkBox_type.Checked == false)
            {
                textBox_type.Text = "";
            }
            textBox_n.Text = "";
            textBox_r.Text = "";
            label_special.Text = "";
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(image_folder_path);
            if (di.GetDirectories().Count() != 0)
            {
                label_special.Text = di.GetDirectories()[0].Name;
            }
            page_num = 0;
            max_num = 0;
            image_files.Clear();
            foreach (System.IO.FileInfo file in di.GetFiles())
            {
                if(file.Extension.ToLower().Equals(".jpg"))
                {
                    image_files.Add(file.Name);
                    max_num++;
                }
            }
            if(image_files.Count == 0)
            {
                MessageBox.Show("폴더 내에 JPG 파일이 없습니다.");
                return;
            }
            PictureReset();
            for (int i = 0; i < max_num; i++)
            {
                String image_path = image_folder_path + "\\" + image_files[i].ToString();
                switch (i)
                {
                    case 0:
                        pictureBox1.Image = LoadBitmap(image_path);
                        break;
                    case 1:
                        pictureBox2.Image = LoadBitmap(image_path);
                        break;
                    case 2:
                        pictureBox3.Image = LoadBitmap(image_path);
                        break;
                    case 3:
                        pictureBox4.Image = LoadBitmap(image_path);
                        break;
                    case 4:
                        pictureBox5.Image = LoadBitmap(image_path);
                        break;
                    case 5:
                        pictureBox6.Image = LoadBitmap(image_path);
                        break;
                    case 6:
                        pictureBox7.Image = LoadBitmap(image_path);
                        break;
                    case 7:
                        pictureBox8.Image = LoadBitmap(image_path);
                        break;
                    case 8:
                        pictureBox9.Image = LoadBitmap(image_path);
                        break;
                    default:
                        break;
                }
            }
            UnselectImage(selected_image_num);
            selected_image_num = -1;
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if (textBox_si.Text == "" || textBox_s.Text == "" || textBox_d.Text == "" || textBox_type.Text == "" || textBox_n.Text == "" || textBox_r.Text == "")
                {
                    MessageBox.Show("비어있는 칸이 있습니다.");
                    return;
                }
                if (listBox_folder.SelectedItem == null)
                {
                    MessageBox.Show("폴더를 선택해주세요.");
                    return;
                }
                String new_name = "00000_서울_" + textBox_s.Text + "_" + textBox_d.Text + "_한옥_000_" + textBox_d.Text + " " + textBox_n.Text + " 한옥(" + textBox_r.Text + ")";
                String target_path = select_path + "\\" + new_name;
                try
                {
                    System.IO.Directory.Move(image_folder_path, target_path);
                }catch
                {
                    MessageBox.Show("이름을 변경하는데 실패하였습니다.");
                    return;
                }

                image_folder_path = target_path;

                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(select_path);
                listBox_folder.Items.Clear();
                foreach (var item in di.GetDirectories())
                {
                    listBox_folder.Items.Add(item.Name);
                }
                listBox_folder.ClearSelected();
                int index = 0;
                foreach (var item in listBox_folder.Items)
                {
                    if (item.ToString().Equals(new_name))
                        break;
                    index++;
                }
                if (listBox_folder.Items.Count != index)
                {
                    listBox_folder.SetSelected(index, true);
                }
                if (checkBox_si.Checked == false)
                {
                    textBox_si.Text = "";
                }
                if (checkBox_s.Checked == false)
                {
                    textBox_s.Text = "";
                }
                if (checkBox_d.Checked == false)
                {
                    textBox_d.Text = "";
                }
                if (checkBox_type.Checked == false)
                {
                    textBox_type.Text = "";
                }
                listBox_folder.Focus();
            }
        }

        private void TextBox_image_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                String old_path = image_folder_path + "\\" + image_files[(page_num * 9) + selected_image_num - 1].ToString();
                String file_name = listBox_folder.SelectedItem.ToString() + "_" + textBox_image_name.Text + ".jpg";
                String new_path = image_folder_path + "\\" + file_name;

                if (System.IO.File.Exists(new_path))
                {
                    MessageBox.Show("이미 존재하는 이름입니다.");
                    return;
                }
                System.IO.File.Move(old_path, new_path);
                image_files[(page_num * 9) + selected_image_num - 1] = file_name;
                label_image_name.Text = image_files[(page_num * 9) + selected_image_num - 1].ToString();
            } else if((e.KeyCode == Keys.Left && e.Control) || (e.KeyCode == Keys.Left && e.Modifiers == Keys.Shift) || (e.KeyCode == Keys.Left && e.Alt))
            {
                if(selected_image_num < 0)
                {
                    MessageBox.Show("이미지를 선택해주세요.");
                    return;
                }
                if (selected_image_num > 1)
                {
                    UnselectImage(selected_image_num);
                    selected_image_num--;
                    SelectImage(selected_image_num);
                    label_image_name.Text = image_files[(page_num * 9) + selected_image_num - 1].ToString();
                }
                else
                {
                    ImagePageDown();
                }
            }
            else if (e.KeyCode == Keys.Right)
            {
                if (selected_image_num < 0)
                {
                    MessageBox.Show("이미지를 선택해주세요.");
                    return;
                }
                int max_page_num = max_num / 9;
                if (max_num % 9 > 0)
                {
                    max_page_num++;
                }

                int max_page_image_num = 9;
                if (page_num == max_page_num - 1)
                {
                    max_page_image_num = max_num % 9;
                    if(max_page_image_num == 0)
                    {
                        max_page_image_num = 9;
                    }
                }

                if (selected_image_num < max_page_image_num)
                {
                    UnselectImage(selected_image_num);
                    selected_image_num++;
                    SelectImage(selected_image_num);
                    label_image_name.Text = image_files[(page_num * 9) + selected_image_num - 1].ToString();
                }
                else
                {
                    ImagePageUp();
                }
            }
            else if(e.KeyCode == Keys.Down)
            {
                if (selected_image_num < 0)
                {
                    MessageBox.Show("이미지를 선택해주세요.");
                    return;
                }
                if (selected_image_num > 0 && selected_image_num < 7)
                {
                    UnselectImage(selected_image_num);
                    selected_image_num += 3;
                    SelectImage(selected_image_num);
                    label_image_name.Text = image_files[(page_num * 9) + selected_image_num - 1].ToString();
                }
            }
            else if(e.KeyCode == Keys.Up)
            {
                if (selected_image_num < 0)
                {
                    MessageBox.Show("이미지를 선택해주세요.");
                    return;
                }
                if (selected_image_num > 3 && selected_image_num < 10)
                {
                    UnselectImage(selected_image_num);
                    selected_image_num -= 3;
                    SelectImage(selected_image_num);
                    label_image_name.Text = image_files[(page_num * 9) + selected_image_num - 1].ToString();
                }
            }
        }

        private void ListBox_folder_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int selectedIndex = -1;
            Point p = e.Location;
            selectedIndex = listBox_folder.IndexFromPoint(p);

            if(selectedIndex != -1)
            {
                String target_folder = select_path + "\\" + listBox_folder.SelectedItem.ToString();
                Process.Start(target_folder);
            }
            else
            {
                return;
            }
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            String new_name = "00000_" + textBox_si.Text + "_" + textBox_s.Text + "_" + textBox_d.Text + "_" + textBox_type.Text + "_000_" + textBox_d.Text + " " + textBox_n.Text + " " + textBox_type.Text + "(" + textBox_r.Text + ")";
            label_folder_name.Text = new_name;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            int height = (groupBox_picture.Height - 20) / 3;
            int width = (groupBox_picture.Width - 20) / 3;

            pictureBox1.Location = new Point(5, 10);
            pictureBox1.Height = height;
            pictureBox1.Width = width;
            pictureBox2.Location = new Point(10 + width, 10);
            pictureBox2.Height = height;
            pictureBox2.Width = width;
            pictureBox3.Location = new Point(15 + (width * 2), 10);
            pictureBox3.Height = height;
            pictureBox3.Width = width;
            pictureBox4.Location = new Point(5, 15 + height);
            pictureBox4.Height = height;
            pictureBox4.Width = width;
            pictureBox5.Location = new Point(10 + width, 15 + height);
            pictureBox5.Height = height;
            pictureBox5.Width = width;
            pictureBox6.Location = new Point(15 + (width * 2), 15 + height);
            pictureBox6.Height = height;
            pictureBox6.Width = width;
            pictureBox7.Location = new Point(5, 20 + (height * 2));
            pictureBox7.Height = height;
            pictureBox7.Width = width;
            pictureBox8.Location = new Point(10 + width, 20 + (height * 2));
            pictureBox8.Height = height;
            pictureBox8.Width = width;
            pictureBox9.Location = new Point(15 + (width * 2), 20 + (height * 2));
            pictureBox9.Height = height;
            pictureBox9.Width = width;

            pictureBox1_back.Location = new Point(5, 10);
            pictureBox1_back.Height = height;
            pictureBox1_back.Width = width;
            pictureBox2_back.Location = new Point(10 + width, 10);
            pictureBox2_back.Height = height;
            pictureBox2_back.Width = width;
            pictureBox3_back.Location = new Point(15 + (width * 2), 10);
            pictureBox3_back.Height = height;
            pictureBox3_back.Width = width;
            pictureBox4_back.Location = new Point(5, 15 + height);
            pictureBox4_back.Height = height;
            pictureBox4_back.Width = width;
            pictureBox5_back.Location = new Point(10 + width, 15 + height);
            pictureBox5_back.Height = height;
            pictureBox5_back.Width = width;
            pictureBox6_back.Location = new Point(15 + (width * 2), 15 + height);
            pictureBox6_back.Height = height;
            pictureBox6_back.Width = width;
            pictureBox7_back.Location = new Point(5, 20 + (height * 2));
            pictureBox7_back.Height = height;
            pictureBox7_back.Width = width;
            pictureBox8_back.Location = new Point(10 + width, 20 + (height * 2));
            pictureBox8_back.Height = height;
            pictureBox8_back.Width = width;
            pictureBox9_back.Location = new Point(15 + (width * 2), 20 + (height * 2));
            pictureBox9_back.Height = height;
            pictureBox9_back.Width = width;

            p1 = p2 = p3 = p4 = p5 = p6 = p7 = p8 = p9 = false;
            pictureBox1_back.BackColor = Color.FromKnownColor(KnownColor.Control);
            pictureBox2_back.BackColor = Color.FromKnownColor(KnownColor.Control);
            pictureBox3_back.BackColor = Color.FromKnownColor(KnownColor.Control);
            pictureBox4_back.BackColor = Color.FromKnownColor(KnownColor.Control);
            pictureBox5_back.BackColor = Color.FromKnownColor(KnownColor.Control);
            pictureBox6_back.BackColor = Color.FromKnownColor(KnownColor.Control);
            pictureBox7_back.BackColor = Color.FromKnownColor(KnownColor.Control);
            pictureBox8_back.BackColor = Color.FromKnownColor(KnownColor.Control);
            pictureBox9_back.BackColor = Color.FromKnownColor(KnownColor.Control);
            selected_image_num = -1;
        }

        private void SelectImage(int i)
        {
            int height = (groupBox_picture.Height - 20) / 3;
            int width = (groupBox_picture.Width - 20) / 3;
            switch (i)
            {
                case 1:
                    p1 = true;
                    pictureBox1.Location = new Point(pictureBox1.Location.X + 10, pictureBox1.Location.Y + 10);
                    pictureBox1.Height = height - 20;
                    pictureBox1.Width = width - 20;
                    pictureBox1_back.BackColor = Color.Yellow;
                    selected_image_num = 1;
                    break;
                case 2:
                    p2 = true;
                    pictureBox2.Location = new Point(pictureBox2.Location.X + 10, pictureBox2.Location.Y + 10);
                    pictureBox2.Height = height - 20;
                    pictureBox2.Width = width - 20;
                    pictureBox2_back.BackColor = Color.Yellow;
                    selected_image_num = 2;
                    break;
                case 3:
                    p3 = true;
                    pictureBox3.Location = new Point(pictureBox3.Location.X + 10, pictureBox3.Location.Y + 10);
                    pictureBox3.Height = height - 20;
                    pictureBox3.Width = width - 20;
                    pictureBox3_back.BackColor = Color.Yellow;
                    selected_image_num = 3;
                    break;
                case 4:
                    p4 = true;
                    pictureBox4.Location = new Point(pictureBox4.Location.X + 10, pictureBox4.Location.Y + 10);
                    pictureBox4.Height = height - 20;
                    pictureBox4.Width = width - 20;
                    pictureBox4_back.BackColor = Color.Yellow;
                    selected_image_num = 4;
                    break;
                case 5:
                    p5 = true;
                    pictureBox5.Location = new Point(pictureBox5.Location.X + 10, pictureBox5.Location.Y + 10);
                    pictureBox5.Height = height - 20;
                    pictureBox5.Width = width - 20;
                    pictureBox5_back.BackColor = Color.Yellow;
                    selected_image_num = 5;
                    break;
                case 6:
                    p6 = true;
                    pictureBox6.Location = new Point(pictureBox6.Location.X + 10, pictureBox6.Location.Y + 10);
                    pictureBox6.Height = height - 20;
                    pictureBox6.Width = width - 20;
                    pictureBox6_back.BackColor = Color.Yellow;
                    selected_image_num = 6;
                    break;
                case 7:
                    p7 = true;
                    pictureBox7.Location = new Point(pictureBox7.Location.X + 10, pictureBox7.Location.Y + 10);
                    pictureBox7.Height = height - 20;
                    pictureBox7.Width = width - 20;
                    pictureBox7_back.BackColor = Color.Yellow;
                    selected_image_num = 7;
                    break;
                case 8:
                    p8 = true;
                    pictureBox8.Location = new Point(pictureBox8.Location.X + 10, pictureBox8.Location.Y + 10);
                    pictureBox8.Height = height - 20;
                    pictureBox8.Width = width - 20;
                    pictureBox8_back.BackColor = Color.Yellow;
                    selected_image_num = 8;
                    break;
                case 9:
                    p9 = true;
                    pictureBox9.Location = new Point(pictureBox9.Location.X + 10, pictureBox9.Location.Y + 10);
                    pictureBox9.Height = height - 20;
                    pictureBox9.Width = width - 20;
                    pictureBox9_back.BackColor = Color.Yellow;
                    selected_image_num = 9;
                    break;
                default:
                    break;
            }
        }

        private void UnselectImage(int i)
        {
            int height = (groupBox_picture.Height - 20) / 3;
            int width = (groupBox_picture.Width - 20) / 3;
            switch (i)
            {
                case 1:
                    p1 = false;
                    pictureBox1.Location = new Point(pictureBox1.Location.X - 10, pictureBox1.Location.Y - 10);
                    pictureBox1.Height = height;
                    pictureBox1.Width = width;
                    pictureBox1_back.BackColor = Color.FromKnownColor(KnownColor.Control);
                    break;
                case 2:
                    p2 = false;
                    pictureBox2.Location = new Point(pictureBox2.Location.X - 10, pictureBox2.Location.Y - 10);
                    pictureBox2.Height = height;
                    pictureBox2.Width = width;
                    pictureBox2_back.BackColor = Color.FromKnownColor(KnownColor.Control);
                    break;
                case 3:
                    p3 = false;
                    pictureBox3.Location = new Point(pictureBox3.Location.X - 10, pictureBox3.Location.Y - 10);
                    pictureBox3.Height = height;
                    pictureBox3.Width = width;
                    pictureBox3_back.BackColor = Color.FromKnownColor(KnownColor.Control);
                    break;
                case 4:
                    p4 = false;
                    pictureBox4.Location = new Point(pictureBox4.Location.X - 10, pictureBox4.Location.Y - 10);
                    pictureBox4.Height = height;
                    pictureBox4.Width = width;
                    pictureBox4_back.BackColor = Color.FromKnownColor(KnownColor.Control);
                    break;
                case 5:
                    p5 = false;
                    pictureBox5.Location = new Point(pictureBox5.Location.X - 10, pictureBox5.Location.Y - 10);
                    pictureBox5.Height = height;
                    pictureBox5.Width = width;
                    pictureBox5_back.BackColor = Color.FromKnownColor(KnownColor.Control);
                    break;
                case 6:
                    p6 = false;
                    pictureBox6.Location = new Point(pictureBox6.Location.X - 10, pictureBox6.Location.Y - 10);
                    pictureBox6.Height = height;
                    pictureBox6.Width = width;
                    pictureBox6_back.BackColor = Color.FromKnownColor(KnownColor.Control);
                    break;
                case 7:
                    p7 = false;
                    pictureBox7.Location = new Point(pictureBox7.Location.X - 10, pictureBox7.Location.Y - 10);
                    pictureBox7.Height = height;
                    pictureBox7.Width = width;
                    pictureBox7_back.BackColor = Color.FromKnownColor(KnownColor.Control);
                    break;
                case 8:
                    p8 = false;
                    pictureBox8.Location = new Point(pictureBox8.Location.X - 10, pictureBox8.Location.Y - 10);
                    pictureBox8.Height = height;
                    pictureBox8.Width = width;
                    pictureBox8_back.BackColor = Color.FromKnownColor(KnownColor.Control);
                    break;
                case 9:
                    p9 = false;
                    pictureBox9.Location = new Point(pictureBox9.Location.X - 10, pictureBox9.Location.Y - 10);
                    pictureBox9.Height = height;
                    pictureBox9.Width = width;
                    pictureBox9_back.BackColor = Color.FromKnownColor(KnownColor.Control);
                    break;
                default:
                    break;
            }
        }

        private void PictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if(pictureBox1.Image == null)
            {
                return;
            }
            if(!p1)
            {
                if (selected_image_num > 0)
                {
                    UnselectImage(selected_image_num);
                }
                label_image_name.Text = image_files[(page_num * 9)].ToString();
                SelectImage(1);
            } else
            {
                UnselectImage(1);
                selected_image_num = -1;
            }
        }

        private void PictureBox2_MouseClick(object sender, MouseEventArgs e)
        {
            if (pictureBox2.Image == null)
            {
                return;
            }
            if (!p2)
            {
                if (selected_image_num > 0)
                {
                    UnselectImage(selected_image_num);
                }
                label_image_name.Text = image_files[(page_num * 9) + 1].ToString();
                SelectImage(2);
            }
            else
            {
                UnselectImage(2);
                selected_image_num = -1;
            }
        }

        private void PictureBox3_MouseClick(object sender, MouseEventArgs e)
        {
            if (pictureBox3.Image == null)
            {
                return;
            }
            if (!p3)
            {
                if (selected_image_num > 0)
                {
                    UnselectImage(selected_image_num);
                }
                label_image_name.Text = image_files[(page_num * 9) + 2].ToString();
                SelectImage(3);
            }
            else
            {
                UnselectImage(3);
                selected_image_num = -1;
            }
        }

        private void PictureBox4_MouseClick(object sender, MouseEventArgs e)
        {
            if (pictureBox4.Image == null)
            {
                return;
            }
            if (!p4)
            {
                if (selected_image_num > 0)
                {
                    UnselectImage(selected_image_num);
                }
                label_image_name.Text = image_files[(page_num * 9) + 3].ToString();
                SelectImage(4);
            }
            else
            {
                UnselectImage(4);
                selected_image_num = -1;
            }
        }

        private void PictureBox5_MouseClick(object sender, MouseEventArgs e)
        {
            if (pictureBox5.Image == null)
            {
                return;
            }
            if (!p5)
            {
                if (selected_image_num > 0)
                {
                    UnselectImage(selected_image_num);
                }
                label_image_name.Text = image_files[(page_num * 9) + 4].ToString();
                SelectImage(5);
            }
            else
            {
                UnselectImage(5);
                selected_image_num = -1;
            }
        }

        private void PictureBox6_MouseClick(object sender, MouseEventArgs e)
        {
            if (pictureBox6.Image == null)
            {
                return;
            }
            if (!p6)
            {
                if (selected_image_num > 0)
                {
                    UnselectImage(selected_image_num);
                }
                label_image_name.Text = image_files[(page_num * 9) + 5].ToString();
                SelectImage(6);
            }
            else
            {
                UnselectImage(6);
                selected_image_num = -1;
            }
        }

        private void PictureBox7_MouseClick(object sender, MouseEventArgs e)
        {
            if (pictureBox7.Image == null)
            {
                return;
            }
            if (!p7)
            {
                if (selected_image_num > 0)
                {
                    UnselectImage(selected_image_num);
                }
                label_image_name.Text = image_files[(page_num * 9) + 6].ToString();
                SelectImage(7);
            }
            else
            {
                UnselectImage(7);
                selected_image_num = -1;
            }
        }

        private void PictureBox8_MouseClick(object sender, MouseEventArgs e)
        {
            if (pictureBox8.Image == null)
            {
                return;
            }
            if (!p8)
            {
                if (selected_image_num > 0)
                {
                    UnselectImage(selected_image_num);
                }
                label_image_name.Text = image_files[(page_num * 9) + 7].ToString();
                SelectImage(8);
            }
            else
            {
                UnselectImage(8);
                selected_image_num = -1;
            }
        }

        private void PictureBox9_MouseClick(object sender, MouseEventArgs e)
        {
            if (pictureBox9.Image == null)
            {
                return;
            }
            if (!p9)
            {
                if (selected_image_num > 0)
                {
                    UnselectImage(selected_image_num);
                }
                label_image_name.Text = image_files[(page_num * 9) + 8].ToString();
                SelectImage(9);
            }
            else
            {
                UnselectImage(9);
                selected_image_num = -1;
            }
        }

        private void PictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Picture = pictureBox1.Image;
            form2.Show();
        }

        private void PictureBox2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Picture = pictureBox2.Image;
            form2.Show();
        }

        private void PictureBox3_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Picture = pictureBox3.Image;
            form2.Show();
        }

        private void PictureBox4_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Picture = pictureBox4.Image;
            form2.Show();
        }

        private void PictureBox5_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Picture = pictureBox5.Image;
            form2.Show();
        }

        private void PictureBox6_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Picture = pictureBox6.Image;
            form2.Show();
        }

        private void PictureBox7_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Picture = pictureBox7.Image;
            form2.Show();
        }

        private void PictureBox8_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Picture = pictureBox8.Image;
            form2.Show();
        }

        private void PictureBox9_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Picture = pictureBox9.Image;
            form2.Show();
        }

        private Bitmap LoadBitmap(String path)
        {
            if(File.Exists(path))
            {
                FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                BinaryReader reader = new BinaryReader(stream);
                var memoryStream = new MemoryStream(reader.ReadBytes((int)stream.Length));
                stream.Close();
                return new Bitmap(memoryStream);
            }
            else
            {
                MessageBox.Show("이미지를 읽어오는데 실패하였습니다.");
                return null;
            }
        }

        private void ImagePageDown()
        {
            if (page_num > 0)
            {
                page_num--;
                for (int i = 0; i < 9; i++)
                {
                    String image_path = image_folder_path + "\\" + image_files[(page_num * 9) + i].ToString();
                    label_image_name.Text = image_files[(page_num * 9) + i].ToString();
                    switch (i)
                    {
                        case 0:
                            pictureBox1.Image = LoadBitmap(image_path);
                            break;
                        case 1:
                            pictureBox2.Image = LoadBitmap(image_path);
                            break;
                        case 2:
                            pictureBox3.Image = LoadBitmap(image_path);
                            break;
                        case 3:
                            pictureBox4.Image = LoadBitmap(image_path);
                            break;
                        case 4:
                            pictureBox5.Image = LoadBitmap(image_path);
                            break;
                        case 5:
                            pictureBox6.Image = LoadBitmap(image_path);
                            break;
                        case 6:
                            pictureBox7.Image = LoadBitmap(image_path);
                            break;
                        case 7:
                            pictureBox8.Image = LoadBitmap(image_path);
                            break;
                        case 8:
                            pictureBox9.Image = LoadBitmap(image_path);
                            break;
                        default:
                            break;
                    }
                }
                UnselectImage(selected_image_num);
                selected_image_num = 9;
                SelectImage(selected_image_num);
            }
            else
            {
                MessageBox.Show("첫번째 이미지입니다.");
            }
        }
        private void ImagePageUp()
        {
            int max_page_num = max_num / 9;
            if (max_num % 9 > 0)
            {
                max_page_num++;
            }
            if (page_num < max_page_num - 1)
            {
                page_num++;
                int picture_num = 9;
                if (page_num == max_page_num - 1)
                {
                    picture_num = max_num % 9;
                    if (picture_num == 0)
                    {
                        picture_num = 9;
                    }
                    PictureReset();
                }
                for (int i = 0; i < picture_num; i++)
                {
                    String image_path = image_folder_path + "\\" + image_files[(page_num * 9) + i].ToString();
                    label_image_name.Text = image_files[(page_num * 9) + i].ToString();
                    switch (i)
                    {
                        case 0:
                            pictureBox1.Image = LoadBitmap(image_path);
                            break;
                        case 1:
                            pictureBox2.Image = LoadBitmap(image_path);
                            break;
                        case 2:
                            pictureBox3.Image = LoadBitmap(image_path);
                            break;
                        case 3:
                            pictureBox4.Image = LoadBitmap(image_path);
                            break;
                        case 4:
                            pictureBox5.Image = LoadBitmap(image_path);
                            break;
                        case 5:
                            pictureBox6.Image = LoadBitmap(image_path);
                            break;
                        case 6:
                            pictureBox7.Image = LoadBitmap(image_path);
                            break;
                        case 7:
                            pictureBox8.Image = LoadBitmap(image_path);
                            break;
                        case 8:
                            pictureBox9.Image = LoadBitmap(image_path);
                            break;
                        default:
                            break;
                    }
                }
                label_image_name.Text = "";
                UnselectImage(selected_image_num);
                selected_image_num = 1;
                SelectImage(selected_image_num);
            }
            else
            {
                MessageBox.Show("마지막 페이지입니다.");
            }
        }
    }
}
