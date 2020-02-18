using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace JsonMaker
{
    public partial class JsonMForm : Form
    {
        const char separator = ',';
        List<string> elements = new List<string>();
        List<int> pos = new List<int>();
        int numRows = 0;
        DataTable finalTable = new DataTable();
        
     
        public JsonMForm()
        {
            InitializeComponent();
        }

        private void JsonMForm_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = Image.FromFile(@"C:\Users\caravena\source\repos\JsonMaker\tenor.gif");
            pictureBox2.Image = Image.FromFile(@"C:\Users\caravena\source\repos\JsonMaker\TemptingShyFireant-size_restricted.gif");
            pictureBox3.Image = Image.FromFile(@"C:\Users\caravena\source\repos\JsonMaker\tenor2.gif");
            pictureBox3.Visible = false;
           // pictureBox2.Size.
        }

        private void btnReadFile_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Refresh();
            string path = txtFilePath.Text;
            const char separator = ',';
            chkElementsBox.Items.Clear();
            using (var reader = new StreamReader(path))
            {
                string firstLine = reader.ReadLine();

                //lblPath.Text = firstLine;

                foreach (var element in firstLine.Split(separator))
                {
                    elements.Add(element);
                }

                //listBox1.DataSource = elements;
                foreach (var item in elements)
                {
                    chkElementsBox.Items.Add(item);
                }

            }
            dataGridView1.DataSource = ReadDataTable(txtFilePath.Text);
        }

        public string DataTableToJSONWithJSONNet(DataTable table)
        {
            string JSONString = string.Empty;
            JSONString =  JsonConvert.SerializeObject(table);
            return JSONString;
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {

            //agrego la posicion de cada item seleccionado en una lista
            foreach (string itemChecked in chkElementsBox.CheckedItems)
            {
                pos.Add(chkElementsBox.Items.IndexOf(itemChecked));
            }
            
            finalTable = WriteDataTable(txtFilePath.Text, pos);
            dataGridView1.DataSource = finalTable;


        }

        public DataTable ReadDataTable(string filePath)
        {
            using (StreamReader sr = new StreamReader(filePath))
            {

                DataTable dataTable = new DataTable();

                string[] headers = sr.ReadLine().Split(separator);
                foreach (string header in headers)
                {
                    dataTable.Columns.Add(header);
                }
                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(separator);
                    DataRow dr = dataTable.NewRow();
                    numRows=numRows+1;
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    dataTable.Rows.Add(dr);
                }
                return dataTable;

            }

        }



        public DataTable WriteDataTable(string filePath, List<int> colums)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Refresh();
            int t = 0;
            DataTable dataTable = new DataTable();
            using (StreamReader sr = new StreamReader(filePath))
            {

                foreach(string item in chkElementsBox.CheckedItems)
                {
                    dataTable.Columns.Add(item);
                }
                
                while(!sr.EndOfStream)
                {
                    DataRow dr = dataTable.NewRow();
                    string[] rows = sr.ReadLine().Split(separator);
                    if (t > 0)
                    {
       
                        for (int i = 0; i < colums.Count; i++)
                        {
                            dr[i] = rows[colums[i]];


                        }
                        dataTable.Rows.Add(dr);
                    }
                    t++;
                }
                
            }

            return dataTable;


        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtFilePath.Text = null;
            pictureBox3.Visible = false;
             elements = new List<string>();
             pos = new List<int>();

            numRows = 0;
            dataGridView1.DataSource = null;
            dataGridView1.Refresh();
            chkElementsBox.Items.Clear();

        }

        private void btnToJSON_Click(object sender, EventArgs e)
        {
            string jsonString = DataTableToJSONWithJSONNet(finalTable);
            string pathExit = @"C:\Users\caravena\source\repos\JsonMaker\Json.json";
            File.WriteAllText(pathExit, jsonString);
            pictureBox3.Visible = true;


        }
    }
}








