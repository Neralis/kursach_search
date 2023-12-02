using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace test2
{
    public partial class Form1 : Form
    {
        private SqlConnection PC1 = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "pC1DataSet.Computers". При необходимости она может быть перемещена или удалена.
            this.computersTableAdapter.Fill(this.pC1DataSet.Computers);

            PC1 = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\kosty\\source\\repos\\PC1.mdf;Integrated Security=True;Connect Timeout=30");
            PC1.Open();
            SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM Computers", PC1);
            DataSet db = new DataSet();
            dataAdapter.Fill(db);
            dataGridView1.DataSource = db.Tables[0];

            // Заполняем ComboBox названиями столбцов
            foreach (DataColumn column in db.Tables[0].Columns)
            {
                comboBox1.Items.Add(column.ColumnName);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                string columnName = comboBox1.SelectedItem.ToString();
                string filterExpression = $"{columnName} LIKE '%{textBox1.Text}%'";

                // Проверяем, является ли столбец числовым
                if (dataGridView1.Columns[columnName].ValueType == typeof(int) || dataGridView1.Columns[columnName].ValueType == typeof(decimal))
                {
                    // Если столбец числовой, используем метод Convert.ToString() для преобразования числа в строку перед использованием LIKE
                    filterExpression = $"Convert({columnName}, 'System.String') LIKE '%{textBox1.Text}%'";
                }

                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = filterExpression;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // При изменении выбранного столбца вызываем обработчик изменения текста для обновления фильтрации
            textBox1_TextChanged(sender, e);
        }
    }
}
