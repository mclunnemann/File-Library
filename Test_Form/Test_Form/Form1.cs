using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Diagnostics;

namespace Test_Form
{
    public partial class Form1 : Form
    {
        DataGridView grid;
        DataGridView grid2;
        string[] allfiles = new string[] { "" };
        string path = @"C:\Users\Michael.Lunnemann\Desktop";
        string ext;
        int dirCount = 0;
        string fileName = "";
        string filePath = "";
        bool search1Dir = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void loadForm (string path)
        {   
            if(search1Dir == false)
            {
                dirCount = Directory.GetFiles(path, ext, SearchOption.AllDirectories).Length;
                allfiles = Directory.GetFiles(path, ext, SearchOption.AllDirectories);
            }
            else
            {
                dirCount = Directory.GetFiles(path, ext, SearchOption.TopDirectoryOnly).Length;
                allfiles = Directory.GetFiles(path, ext, SearchOption.TopDirectoryOnly);
            }

            Array.Sort(allfiles);
            grid = new DataGridView();
            grid.Rows.Clear();
            grid.Name = "grid";
            grid.AllowUserToResizeColumns = true;
            grid.AllowUserToResizeRows = false;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grid.AllowUserToAddRows = false;
            grid.RowHeadersVisible = false;
            grid.ColumnHeadersVisible = true;
            grid.ColumnHeadersHeight = 25;
            grid.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Calibri", 10F);
            grid.ScrollBars = ScrollBars.Vertical;
            grid.Size = new Size(1620, 1015);
            grid.Location = new Point(300, 100);
            grid.Font = new System.Drawing.Font("Calibri", 10F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.CellDoubleClick += Grid_CellDoubleClick1;
            grid.CellEndEdit += Grid_CellEndEdit;

            DataGridViewTextBoxColumn grid_name = new DataGridViewTextBoxColumn();
            grid.Columns.Add(grid_name);
            grid.Columns[0].Name = "Name";
            grid.Columns[0].Width = 1600 / 4;
            grid.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grid.Columns[0].ReadOnly = false;

            DataGridViewTextBoxColumn grid_type = new DataGridViewTextBoxColumn();
            grid.Columns.Add(grid_type);
            grid.Columns[1].Name = "Type";
            grid.Columns[1].Width = 1600 / 4;
            grid.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grid.Columns[1].ReadOnly = true;

            DataGridViewTextBoxColumn grid_dateMod = new DataGridViewTextBoxColumn();
            grid.Columns.Add(grid_dateMod);
            grid.Columns[2].Name = "Date Modified";
            grid.Columns[2].Width = 1600 / 4;
            grid.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grid.Columns[2].ReadOnly = true;

            DataGridViewTextBoxColumn grid_size = new DataGridViewTextBoxColumn();
            grid.Columns.Add(grid_size);
            grid.Columns[3].Name = "Size";
            grid.Columns[3].Width = 1600 / 4;
            grid.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grid.Columns[3].ReadOnly = true;

            DataGridViewTextBoxColumn grid_directoryPath = new DataGridViewTextBoxColumn();
            grid.Columns.Add(grid_directoryPath);
            grid.Columns[4].Name = "Path";
            grid.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grid.Columns[4].ReadOnly = true;
            grid.Columns[4].Visible = false;

            for (int i = 1; i < grid.Height / 23; i++)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.Height = 23;
                row.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                if (i % 2 == 1)
                {
                    row.DefaultCellStyle.BackColor = Color.Gainsboro;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.GhostWhite;
                }
                row.DefaultCellStyle.SelectionBackColor = Color.CornflowerBlue;
                grid.Rows.Add(row);
            }
            Controls.Add(grid);

            for (int i = 0; i < allfiles.Length; i++)
            {
                grid.Rows[i].HeaderCell.Value = (i + 1).ToString();
                FileInfo info = new FileInfo(allfiles[i]);
                grid.Rows[i].Cells[0].Value = info.Name;
                fileName = info.Name;
                grid.Rows[i].Cells[1].Value = info.Extension;
                grid.Rows[i].Cells[2].Value = info.LastWriteTime;
                grid.Rows[i].Cells[3].Value = info.Length;
                filePath = info.FullName.Substring(0, info.FullName.Length - info.Name.Length);
                grid.Rows[i].Cells[4].Value = filePath;
            }
            //new grid for directory file is in
            
        }

        private void Grid_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            filePath = grid.Rows[e.RowIndex].Cells[4].Value.ToString();
            fileName = grid.Rows[e.RowIndex].Cells[0].Value.ToString();
        }

        private void Grid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string new_fd_name = "";
            string rowNum_str = "";
            int rowNum = 0;
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("Do you want to change the file name?", "Change File Name", buttons);
            if (e.ColumnIndex == 0)
            {
                grid.CellBeginEdit += Grid_CellBeginEdit;
                new_fd_name = grid[e.ColumnIndex, e.RowIndex].Value.ToString();
                rowNum_str = grid.Rows[e.RowIndex].HeaderCell.Value.ToString();
                rowNum = Convert.ToInt32(rowNum_str);
                rowNum = rowNum - 1;
                
                if(result == System.Windows.Forms.DialogResult.Yes)
                {
                    File.Copy(filePath + fileName, (string)grid.Rows[e.RowIndex].Cells[4].Value + new_fd_name);
                    File.Delete(filePath + fileName);
                }
            }
            else
            {
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    new_fd_name = grid[e.ColumnIndex, e.RowIndex].Value.ToString();
                    Directory.CreateDirectory(filePath + new_fd_name);
                    File.Copy(filePath + fileName, (string)grid.Rows[e.RowIndex].Cells[4].Value + new_fd_name);
                }
            }
        }

        private void Grid_CellDoubleClick1(object sender, DataGridViewCellEventArgs e)
        {           
            string rowNum_str = grid.Rows[e.RowIndex].HeaderCell.Value.ToString();
            int rowNum = Convert.ToInt32(rowNum_str);
            string file = (allfiles[rowNum-1]);
            MessageBox.Show(file);
        }

        private void Grid2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string rowNum_str = (e.RowIndex).ToString();
            int rowNum = Convert.ToInt32(rowNum_str);

            if (rowNum > 0 )
            {
                dirCount = Directory.GetFiles(path, ext, SearchOption.AllDirectories).Length;
                allfiles = Directory.GetFiles(path, ext, SearchOption.AllDirectories);
                Controls.Remove(grid);
                path = @"C:\Users\Michael.Lunnemann\Desktop";
                search1Dir = false;
                loadForm(path);
            }
            else
            {
                path = grid2.Rows[e.RowIndex].Cells[1].Value.ToString();
                Controls.Remove(grid);
                search1Dir = true;
                loadForm(path);
            }
        }

        private void txt_extension_TextChanged(object sender, EventArgs e)
        {

        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            //Array.Clear(allfiles,0,allfiles.Length);
            allfiles = new string[] { "" };
            Controls.Remove(grid);
            Controls.Remove(grid2);
            ext = "";
            ext = "*." + txt_extension.Text.ToLower();
            if(txt_extension.Text == "")
            {
                MessageBox.Show("Enter a valid file extension to search.");
            }
            else
            {
                loadForm(path);
                this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                grid2 = new DataGridView();
                grid2.Name = "grid2";
                grid2.AllowUserToResizeColumns = true;
                grid2.AllowUserToResizeRows = false;
                grid2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                grid2.AllowUserToAddRows = false;
                grid2.RowHeadersVisible = false;
                grid2.ColumnHeadersVisible = true;
                grid2.ColumnHeadersHeight = 25;
                grid2.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Calibri", 10F);
                grid2.ScrollBars = ScrollBars.Vertical;
                //grid2.ScrollBars = ScrollBars.Both;
                grid2.Size = new Size(299, 1015);
                grid2.Location = new Point(1, 100);
                grid2.Font = new System.Drawing.Font("Calibri", 10F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
                grid2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                grid2.CellDoubleClick += Grid2_CellDoubleClick;

                DataGridViewTextBoxColumn grid2_name = new DataGridViewTextBoxColumn();
                grid2.Columns.Add(grid2_name);
                grid2.Columns[0].Name = "Directory Name";
                grid2.Columns[0].Width = 300;
                grid2.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                grid2.Columns[0].ReadOnly = true;
                grid2.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;

                DataGridViewTextBoxColumn grid2_directoryPath = new DataGridViewTextBoxColumn();
                grid2.Columns.Add(grid2_directoryPath);
                grid2.Columns[1].Name = "Path";
                grid2.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                grid2.Columns[1].ReadOnly = true;
                grid2.Columns[1].Visible = false;

                for (int i = 1; i < grid2.Height / 23; i++)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    row.Height = 23;
                    row.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    if (i % 2 == 1)
                    {
                        row.DefaultCellStyle.BackColor = Color.Gainsboro;
                    }
                    else
                    {
                        row.DefaultCellStyle.BackColor = Color.GhostWhite;
                    }
                    row.DefaultCellStyle.SelectionBackColor = Color.CornflowerBlue;
                    grid2.Rows.Add(row);
                }
                Controls.Add(grid2);
                int c = 1;
                int c2 = c;
                while (c <= allfiles.Length)
                {
                    grid2.Rows[c].HeaderCell.Value = (c).ToString();
                    FileInfo info = new FileInfo(allfiles[c - 1]);
                    string directory = info.DirectoryName;
                    int startPosition = directory.LastIndexOf(@"\") + 1;
                    int endPosition = directory.Length;
                    directory = directory.Substring(startPosition, endPosition - startPosition);
                    grid2.Rows[0].Cells[0].Value = "All Files";
                    grid2.Rows[0].Cells[0].ReadOnly = true;
                    string prevDir = "";
                    prevDir = grid2.Rows[c2 - 1].Cells[0].Value.ToString();
                    if (prevDir == directory)
                    {
                        //c2--;
                        c++;
                    }
                    else
                    {
                        grid2.Rows[c2].Cells[0].Value = directory;
                        filePath = info.FullName.Substring(0, info.FullName.Length - info.Name.Length);
                        grid2.Rows[c2].Cells[1].Value = filePath;
                        c2++;
                        c++;
                    }
                }
            }
            txt_extension.Clear();
        }
    }
}
