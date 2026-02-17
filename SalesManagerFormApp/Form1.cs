using System.Windows.Forms;

namespace SalesManagerFormApp
{
    public partial class Form1 : Form
    {
        public class Product
        {
            public Image Priority { get; set; }
            public string DueDate { get; set; }
            public string Subject { get; set; }
            public string Description { get; set; }
            public Image Completion { get; set; }
        }
        public class ManagerForm
        {
            public string CreatedOn { get; set; }
            public string Subject { get; set; }
            public string Manager { get; set; }

        }
        protected List<Product> GetProductList()
        {
            Image img1 = Image.FromFile(@"C:\Users\Sudharsni\Downloads\priority.png");
            Image img2 = Image.FromFile(@"C:\Users\Sudharsni\Downloads\completion.jpg");

            List<Product> products = new List<Product>();

            // Assume you have an 'Images' folder in your project directory
            products.Add(new Product { Priority = img1, DueDate = "2/22/2020", Subject = "Direct vs Sales Comparison Report", Description = "To better understand 2020 online sales information", Completion = img2 });
            products.Add(new Product { Priority = img1, DueDate = "4/3/2020", Subject = "Comment on Revenue Projections", Description = "Board requires 2020 project report comment on sales report and my projections", Completion = img2 });
            products.Add(new Product { Priority = img1, DueDate = "9/11/2020", Subject = "New Online Marketing Strtegy", Description = "We need to do something to stop the fall in online sales right away", Completion = img2 });

            // Add more items as needed
            return products;
        }
        protected List<ManagerForm> GetManagerForm()
        {

            List<ManagerForm> managerForms = new List<ManagerForm>();

            // Assume you have an 'Images' folder in your project directory
            managerForms.Add(new ManagerForm { CreatedOn = "2/1/2010", Subject = "Comparison Report", Manager = "Wills Smith" });
            managerForms.Add(new ManagerForm { CreatedOn = "27/11/2011", Subject = "Stack Report", Manager = "Ronald" });
            managerForms.Add(new ManagerForm { CreatedOn = "29/12/2010", Subject = "Product Manager Report", Manager = "Hailey" });
            managerForms.Add(new ManagerForm { CreatedOn = "2/1/2014", Subject = "Comparison Report", Manager = "Mizi" });
            managerForms.Add(new ManagerForm { CreatedOn = "27/11/2012", Subject = "Stack Report", Manager = "Masha" });
            managerForms.Add(new ManagerForm { CreatedOn = "29/12/2013", Subject = "Product Manager Report", Manager = "Jenny" });
            managerForms.Add(new ManagerForm { CreatedOn = "2/1/2010", Subject = "Comparison Report", Manager = "Wills Smith" });
            managerForms.Add(new ManagerForm { CreatedOn = "27/11/2011", Subject = "Stack Report", Manager = "Ronald" });
            managerForms.Add(new ManagerForm { CreatedOn = "29/12/2010", Subject = "Product Manager Report", Manager = "Hailey" });
            managerForms.Add(new ManagerForm { CreatedOn = "2/1/2014", Subject = "Comparison Report", Manager = "Mizi" });
            managerForms.Add(new ManagerForm { CreatedOn = "27/11/2012", Subject = "Stack Report", Manager = "Masha" });
            managerForms.Add(new ManagerForm { CreatedOn = "29/12/2013", Subject = "Product Manager Report", Manager = "Jenny" });
            managerForms.Add(new ManagerForm { CreatedOn = "27/11/2012", Subject = "Stack Report", Manager = "Masha" });
            managerForms.Add(new ManagerForm { CreatedOn = "29/12/2013", Subject = "Product Manager Report", Manager = "Jenny" });
            // Add more items as needed
            return managerForms;
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView2.DataSource = GetProductList();
            dataGridView2.Columns[2].Width = 300;
            dataGridView2.Columns[3].Width = 650;
            dataGridView2.Columns[4].Width = 200;
            dataGridView2.Columns[0].Width = 70;
            if (dataGridView2.Columns["Priority"] is DataGridViewImageColumn imgCol)
            {
                imgCol.ImageLayout = DataGridViewImageCellLayout.Stretch;
            }
            dataGridView1.DataSource = GetManagerForm();
            dataGridView1.Columns[1].Width = 200;
            dataGridView1.Columns[2].Width = 250;
        }

        private void panel9_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            MessageBox.Show("welcome");
        }

        private void label6_Click(object sender, EventArgs e)
        {
            MessageBox.Show("welcome");
        }
    }
}
