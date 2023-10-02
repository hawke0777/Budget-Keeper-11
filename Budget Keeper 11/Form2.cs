using System;
using System.Windows.Forms;
namespace Budget_Keeper_11
{
    public partial class Form2 : Form
    {

        private CheckBox[] checkboxes;

        public Form2()
        {
            InitializeComponent();
            checkboxes = new CheckBox[] { CheckBox2, CheckBox3, CheckBox4, CheckBox5 };
            for (int i = 0; i < checkboxes.Length; i++)
            {
                checkboxes[i].CheckedChanged += CheckBox_CheckedChanged;
            }
        }
        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox clickedCheckbox = (CheckBox)sender;

            foreach (CheckBox checkbox in checkboxes)
            {
                if (checkbox != clickedCheckbox)
                {
                    checkbox.CheckState = CheckState.Unchecked;
                }
            }
        }

        private void monthTosave()
        {
            DateTime currentDate = dateTimePicker1.Value;

            for (int i = 0; i < checkboxes.Length; i++)
            {
                checkboxes[i].Text = currentDate.AddMonths(i + 1).ToString("MMMM yyyy");
            }
        }

        private void Form2_Load_1(object sender, EventArgs e)
        {
            monthTosave();
        }

        private void Button1_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Hide();
        }
    }
}
