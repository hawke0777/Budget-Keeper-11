////Designed and coded with the use of chatgpt with some modifications
//Programer Andre Collins
//http://amcollinsresume.experserv.com/

using System;
using System.Windows.Forms;
using System.Data;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.VisualBasic;
using System.IO;
using Budget_Keeper_11;

namespace budget_keeper_11
{
    public partial class Form1 : Form
    {
        private DataTable table1 = new DataTable("Table1");

        public Form1()
        {
            InitializeComponent();
            InitializeDataGrid();
            Start();
        }

        private void InitializeDataGrid()
        {
            table1.Columns.AddRange(new[]
            {
                new DataColumn("Bill Name", typeof(string)),
                new DataColumn("Bill Amount", typeof(string)),
                new DataColumn("Amount Paid", typeof(string)),
                new DataColumn("Card Paid", typeof(string)),
                new DataColumn("Paid On", typeof(string)),
                new DataColumn("Amount Owed", typeof(string))
            });

            dataGridView1.DataSource = table1;
        }
        private void Start()
        {

            ClearDataGridView();
            comboBox1.SelectedIndex = 0;
            groupBox1.Text = "For Month and Year Of:";
            groupBox3.Text = "Bills";
            label3.Text = DateTime.Now.AddMonths(1).ToString("MMMM yyyy");
            clearLabelsWhenNothingIsSelected();
        }

       
        private void clearLabelsWhenNothingIsSelected()
        {
            textBox1.Clear();
            textBox2.Clear();
            checkBox1.Checked = false;
            label5.Text = string.Empty;
            label7.Text = string.Empty;
            label9.Text = string.Empty;
            label11.Text = string.Empty;
            label13.Text = string.Empty;
            label15.Text = string.Empty;
            label17.Text = string.Empty;
            label19.Text = string.Empty;
            label21.Text = string.Empty;
            label22.Text = string.Empty;
            label24.Text = string.Empty;
            label26.Text = string.Empty;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Start();
            ClearDataGridView();


        }

        private void ClearDataGridView()
        {
            table1.Rows.Clear();
            table1.Columns.Clear();
            dataGridView1.DataSource = null;
            dataGridView1.Refresh();
        }
        private void AddBillInformation()
        {
            string recurring = checkBox1.Checked ? "yes" : "no";
            table1.Rows.Add(textBox1.Text, textBox2.Text, "0", recurring, "----N/A----", textBox2.Text);
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    AddBillInformation();
                    break;
                case 1:
                    EnterPaymentIntoDataGridView();
                    break;
                case 2:
                    DeleteFromDataGridView();
                    break;
                case 3:
                    EditBillAmount();
                    break;
            }

            ClearInputFields();
            CalculateStats();
        }

        private void EnterPaymentIntoDataGridView()
        {
            try
            {
                int amountDue = Convert.ToInt32(dataGridView1.CurrentRow.Cells[1].Value);
                int apaid = Convert.ToInt32(textBox2.Text);
                int aowed = amountDue - apaid;
                string pday = DateTime.Now.ToString("MM/dd");
                dataGridView1.CurrentRow.Cells[2].Value = textBox2.Text;
                dataGridView1.CurrentRow.Cells[4].Value = pday;
                dataGridView1.CurrentRow.Cells[5].Value = aowed.ToString();
            }
            catch { /* Handle the exception appropriately or log it */ }
        }

        private void DeleteFromDataGridView()
        {
            if (MessageBox.Show("Do you wish to delete " + dataGridView1.CurrentRow.Cells[0].Value.ToString() + "?", "Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
            }
        }

        private void EditBillAmount()
        {
            string pday = "----N/A----";
            if (!string.IsNullOrEmpty(textBox2.Text))
            {
                dataGridView1.CurrentRow.Cells[1].Value = textBox2.Text;
                dataGridView1.CurrentRow.Cells[2].Value = "0";
                dataGridView1.CurrentRow.Cells[4].Value = pday;
                dataGridView1.CurrentRow.Cells[5].Value = textBox2.Text;
            }
            else
            {
                MessageBox.Show("Please enter a new balance.");
            }
        }

        private void ClearInputFields()
        {
            textBox1.Clear();
            textBox2.Clear();
            checkBox1.Checked = false;
        }

        private void CalculateStats()
        {
            // Your stats calculation logic here
            int sum = 0;
            int rsum = 0;
            int nrsum = 0;
            int paidwithaccount = 0;
            int paidoncredit = 0;
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                sum += Conversions.ToInteger(dataGridView1.Rows[i].Cells[5].Value);
                label7.Text = "$" + sum.ToString();
                if (dataGridView1.Rows[i].Cells[3].Value.ToString() == "yes")
                {
                    rsum += Conversions.ToInteger(dataGridView1.Rows[i].Cells[5].Value);
                    label9.Text = "$" + rsum.ToString();
                    paidoncredit += Conversions.ToInteger(dataGridView1.Rows[i].Cells[2].Value);
                    label13.Text = "$" + paidoncredit.ToString();
                }
                if (dataGridView1.Rows[i].Cells[3].Value.ToString() == "no")
                {
                    nrsum += Conversions.ToInteger(dataGridView1.Rows[i].Cells[5].Value);
                    label11.Text = "$" + nrsum.ToString();
                    paidwithaccount += Conversions.ToInteger(dataGridView1.Rows[i].Cells[2].Value);
                    label15.Text = "$" + paidwithaccount.ToString();
                    label17.Text = "$" + (nrsum / 3).ToString();
                    label19.Text = "$" + (nrsum / 6).ToString();
                    label21.Text = "$" + (nrsum / 12).ToString();
                    label22.Text = "$" + (nrsum / 24).ToString();
                    label24.Text = "$" + (nrsum / 36).ToString();
                    label26.Text = "$" + (nrsum / 48).ToString();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClearInputFields();
        }

        private void ModifyDataGridViewRows(Func<DataGridViewRow, bool> condition, Action<DataGridViewRow> action)
        {
            try
            {
                for (int i = dataGridView1.Rows.Count - 1; i >= 0; i--)
                {
                    DataGridViewRow row = dataGridView1.Rows[i];
                    if (condition(row))
                    {
                        action(row);
                    }
                }
            }
            catch
            {
                // Handle exceptions as needed.
            }
        }

        private void CombineMethods()
        {
            if (Interaction.MsgBox("Do you want to change your bill amount to what you owe on your bills?", MsgBoxStyle.YesNo, "Transfer Balances") == MsgBoxResult.Yes)
            {
                ModifyDataGridViewRows(
                    row => Conversions.ToInteger(row.Cells[5].Value) > 0,
                    row =>
                    {
                        row.Cells[1].Value = row.Cells[5].Value;
                        row.Cells[2].Value = "0";
                    }
                );
            }

            ModifyDataGridViewRows(
                row => Conversions.ToString(row.Cells[5].Value) == "0" && Conversions.ToString(row.Cells[3].Value) == "no",
                row => dataGridView1.Rows.Remove(row)
            );

            ModifyDataGridViewRows(
                row => Conversions.ToString(row.Cells[3].Value) == "yes",
                row =>
                {
                    string pday = "----N/A----";
                    row.Cells[2].Value = "0";
                    row.Cells[5].Value = row.Cells[1].Value;
                    row.Cells[4].Value = pday;
                }
            );
        }

        private void doAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CombineMethods();
        }
        private string[] lines;
        private string[] data;
        private void openBudget()
        {

            DialogResult opendialog = openDialog.ShowDialog();
            if (opendialog == DialogResult.OK)
            {
                try
                {
                    string ofd = openDialog.FileName.ToString().Remove(0, 11);
                    label3.Text = ofd.Substring(0, ofd.Length - 4);
                    lines = File.ReadAllLines(openDialog.FileName);
                    for (int i = 0; i <= lines.Length - 1; i++)
                    {
                        data = lines[i].ToString().Split(',');
                        var row = new string[data.Length];
                        for (int j = 0; j <= data.Length - 1; j++)
                            row[j] = data[j].Trim();
                        table1.Rows.Add(row);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Budget did not open due to:\r\n " + ex, "ERROR!!!!!!");
                }
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clearLabelsWhenNothingIsSelected();
            ClearDataGridView();
            InitializeDataGrid();
            openBudget();
            CalculateStats();
        }
        private void savebudget()
        {
            using (Form2 sf = new Form2())
            {
                sf.ShowDialog();

                if (sf.DialogResult != DialogResult.OK)
                {
                    return; // Exit if the dialog result is not OK
                }

                string chosenmonth = sf.CheckBox2.Checked ? sf.CheckBox2.Text :
                                     sf.CheckBox3.Checked ? sf.CheckBox3.Text :
                                     sf.CheckBox4.Checked ? sf.CheckBox4.Text :
                                     sf.CheckBox5.Text;

                string filename = $@"D:\budgets\{chosenmonth}.bkb";
                StreamWriter writer = new StreamWriter(filename);
                {
                    for (int i = 0; i <= dataGridView1.Rows.Count - 2; i += +1)
                    {
                        for (int j = 0; j <= dataGridView1.Columns.Count - 1; j += +1)
                        {
                            if (j == dataGridView1.Columns.Count - 1)
                            {
                                writer.Write(dataGridView1.Rows[i].Cells[j].Value.ToString());

                            }
                            else
                                writer.Write(dataGridView1.Rows[i].Cells[j].Value.ToString() + ",");
                        }
                        writer.WriteLine(" ");
                    }

                    writer.Close();
                    MessageBox.Show("Budget Saved !!!!!", "Saved!!!!!!");
                    MessageBox.Show("You can find this file at this location " + filename.ToString(), "Here I am!!!!");
                }
            }
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            savebudget();

        }

        private void addABill()
        {
            groupBox2.Text = "Add Bill";
            groupBox4.Text = "Stats";
            groupBox5.Text = String.Empty;
            label1.Text = "Bill Name";
            label2.Visible = true;
            label2.Text = "Bill Amount";
            label4.Visible = false;
            label5.Visible = false;
            checkBox1.Visible = true;
            checkBox1.Text = "Card?";
            button1.Text = "Add";
            button2.Visible = true;
            button2.Text = "Clear";
            textBox1.Enabled = true;
            textBox1.Select();
            textBox2.Visible = true;
            textBox2.Enabled = true;
        }
        private void payABill()
        {
            groupBox2.Text = "Pay Bill";
            groupBox4.Text = "Stats";
            groupBox5.Text = String.Empty;
            label1.Text = "Bill Name";
            label2.Visible = true;
            label2.Text = "Payment Amount";
            label4.Visible = true;
            label4.Text = "Amount Owed :$";
            label5.Visible = true;
            checkBox1.Visible = false;
            button1.Text = "Pay";
            button2.Visible = true;
            button2.Text = "Clear";
            textBox1.Enabled = false;
            textBox2.Visible = true;
            textBox2.Select();
        }
        private void removeABill()
        {
            groupBox2.Text = "Delete Bill";
            groupBox4.Text = "Stats";
            groupBox5.Text = String.Empty;
            label1.Text = "Bill Name";
            label2.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            checkBox1.Visible = false;
            button1.Text = "Delete";
            button2.Visible = false;
            textBox1.Enabled = false;
            textBox2.Visible = false;
            textBox2.Enabled = false;
        }
        private void editABill()
        {
            groupBox2.Text = "Edit Bill";
            groupBox4.Text = "Stats";
            groupBox5.Text = String.Empty;
            label1.Text = "Bill Name";
            label2.Visible = true;
            label2.Text = "New Amount";
            label4.Visible = false;
            label5.Visible = false;
            checkBox1.Visible = false;
            button1.Text = "Edit";
            button2.Visible = false;
            textBox1.Enabled = false;
            textBox2.Visible = true;
            textBox2.Enabled = true;
        }
        private void dutySelect()
        {
            groupBox5.Visible = true;
            if (comboBox1.SelectedIndex == 0)
            {
                ClearInputFields();
                addABill();
            }
            if (comboBox1.SelectedIndex == 1)
            {
                ClearInputFields();
                payABill();

            }
            if (comboBox1.SelectedIndex == 2)
            {
                ClearInputFields();
                removeABill();
            }
            if (comboBox1.SelectedIndex == 3)
            {
                ClearInputFields();
                editABill();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dutySelect();
        }
        private void entersDatagridView1cellsInTextBoxes()
        {
            string bname;
            string bamount;
            try
            {
                bname = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                bamount = dataGridView1.CurrentRow.Cells[5].Value.ToString();
                textBox1.Text = bname;
                label5.Text = bamount;
            }
            catch
            {
                //do nothing!
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            entersDatagridView1cellsInTextBoxes();
        }

        private void closeApplication()
        {
            if (Interaction.MsgBox("Do you wish to exit?", MsgBoxStyle.YesNo) == MsgBoxResult.Yes)
            {
                Application.Exit();
            }
              
        }

        private void closeBudgetToolStripMenuItem_Click(object sender, EventArgs e)
        {
      
            if (dataGridView1.Rows.Count > 1 && Interaction.MsgBox("Do you wish to save?", MsgBoxStyle.YesNo) == MsgBoxResult.Yes  )
            {
                savebudget();
            }
            closeApplication();
        }
    }
}
