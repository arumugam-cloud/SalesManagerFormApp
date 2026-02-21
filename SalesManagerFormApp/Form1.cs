using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;

using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
namespace SalesManagerFormApp
{
    public partial class Form1 : Form
    {
        SqlConnection connection = new SqlConnection();
        string connectionString = ConfigurationManager.ConnectionStrings["EmpDBCS"].ConnectionString;

        public Form1()
        {
            InitializeComponent();
        }

        //Normal method
        //private void DepartmentName()
        //{
        //    string query = "SELECT DepartmentID, DepartmentName FROM Department";

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
        //        DataTable dataTable = new DataTable();

        //        try
        //        {
        //            connection.Open();
        //            adapter.Fill(dataTable);
        //            DataRow dr = dataTable.NewRow();
        //            dr["DepartmentID"] = DBNull.Value; // Use NULL for the ID or a specific indicator like -1 or 0
        //            dr["DepartmentName"] = "-- Select --";
        //            dataTable.Rows.InsertAt(dr, 0);
        //            cbxDepartment.DataSource = dataTable;
        //            cbxDepartment.DisplayMember = "DepartmentName";
        //            cbxDepartment.ValueMember = "DepartmentID";

        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show("Error loading departments: " + ex.Message);
        //        }
        //    }
        //}

        //Async and await method

        private async Task DepartmentNameAsync()
        {
            string query = "SELECT DepartmentID, DepartmentName FROM Department";

            using (connection = new SqlConnection(connectionString))
            {
                DataTable dataTable = new DataTable();

                try
                {

                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            dataTable.Load(reader);
                        }
                    }
                    cbxDepartment.DataSource = dataTable;
                    cbxDepartment.DisplayMember = "DepartmentName";
                    cbxDepartment.ValueMember = "DepartmentID";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading departments: " + ex.Message);
                }
            }
        }
        //private void Form1_Load(object sender, EventArgs e)
        //{
        //    DepartmentNameAsync();
        //    LoadDepartmentData();
        //    LoadEmployeeData();
        //    dataGridView1.Columns[0].Width = 300;
        //    dataGridView1.Columns[1].Width = 300;
        //    cbxState.SelectedIndex = 0;
        //    cbxPrefix.SelectedIndex = 0;
        //    cbxState.SelectedIndex = 0;
        //    cbxTitle.SelectedIndex = 0;
        //    cbxStatus.SelectedIndex = 0;
        //    cbxDepartment.SelectedIndex = 0;
        //    //dataGridView2.DataSource = GetProductList();
        //    //dataGridView2.Columns[2].Width = 300;
        //    //dataGridView2.Columns[3].Width = 650;
        //    //dataGridView2.Columns[4].Width = 200;
        //    //dataGridView2.Columns[0].Width = 70;
        //    //if (dataGridView2.Columns["Priority"] is DataGridViewImageColumn imgCol)
        //    //{
        //    //    imgCol.ImageLayout = DataGridViewImageCellLayout.Stretch;
        //    //}
        //    //dataGridView1.DataSource = GetManagerForm();
        //    //dataGridView1.Columns[1].Width = 200;
        //    //dataGridView1.Columns[2].Width = 250;
        //}

        //asnyc and await method 
        private async void Form1_Load(object sender, EventArgs e) // Added async
        {
            try
            {
                dataGridView2.AutoGenerateColumns = false;
                dataGridView2.ForeColor = Color.Black;
                await DepartmentNameAsync();
                await LoadDepartmentDataAsync();
                await LoadEmployeeDataAsync();
                dataGridView2.CellContentClick += new DataGridViewCellEventHandler(dataGridView2_CellContentClick);

                if (dataGridView1.Columns.Count >= 2)
                {
                    dataGridView1.Columns[0].Width = 300;
                    dataGridView1.Columns[1].Width = 300;
                }

                cbxState.SelectedIndex = 0;
                cbxPrefix.SelectedIndex = 0;
                cbxTitle.SelectedIndex = 0;
                cbxStatus.SelectedIndex = 0;

                if (cbxDepartment.Items.Count > 0)
                {
                    cbxDepartment.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during initialization: " + ex.Message);
            }
        }
        private void panel9_Paint(object sender, PaintEventArgs e)
        {

        }

        private async void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        { }
        private async void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex < 0) return;

            string colName = dataGridView2.Columns[e.ColumnIndex].Name;
            var row = dataGridView2.Rows[e.RowIndex];
            int recordId = Convert.ToInt32(row.Cells["EmployeeID"].Value); // Assume "Id" is your Primary Key column

            try
            {
                if (colName == "Edit")
                {
                    string FirstName = row.Cells["FirstName"].Value.ToString();
                    await UpdateDatabaseAsync(recordId, FirstName);
                    MessageBox.Show("Record updated in database.");
                }
                else if (colName == "Delete")
                {
                    var confirm = MessageBox.Show("Delete from database?", "Confirm", MessageBoxButtons.YesNo);
                    if (confirm == DialogResult.Yes)
                    {
                        await DeleteFromDatabaseAsync(recordId);
                        dataGridView2.Rows.RemoveAt(e.RowIndex); 
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database Error: {ex.Message}");
            }
        }

        private async Task UpdateDatabaseAsync(int EmployeeID, string FirstName)
        {
            using (connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE Employee SET FirstName = @FirstName WHERE EmployeeID = @EmployeeID";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@FirstName", FirstName);
                cmd.Parameters.AddWithValue("@EmployeeID", EmployeeID);

                await connection.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                LoadDepartmentDataAsync();
            }
        }

        private async Task DeleteFromDatabaseAsync(int EmployeeID)
        {
            using (connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Employee WHERE EmployeeID = @EmployeeID";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@EmployeeID", EmployeeID);

                await connection.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                LoadDepartmentDataAsync();
            }
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

        public async Task<int> GetDepartmentIDAsync(string DepartmentName)
        {
            int departmentid = 0;
            string queryString = "SELECT DepartmentID FROM Department WHERE DepartmentName = @DepartmentName";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@DepartmentName", DepartmentName);

                    try
                    {

                        var result = await command.ExecuteScalarAsync();

                        if (result != null)
                        {
                            departmentid = Convert.ToInt32(result.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error retrieving department name: " + ex.Message);
                        throw;
                    }
                }
            }

            return departmentid;
        }

        //Normal method
        //public int GetDepartmentID(string DepartmentName)
        //{
        //    int departmentid = 0;
        //    string queryString = "SELECT DepartmentID FROM Department WHERE DepartmentName = @DepartmentName";
        //    using (connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();
        //        using (SqlCommand command = new SqlCommand(queryString, connection))
        //        {
        //            command.Parameters.AddWithValue("@DepartmentName", DepartmentName);

        //            try
        //            {
        //                var result = command.ExecuteScalar();

        //                if (result != null)
        //                {
        //                    departmentid = Convert.ToInt32(result.ToString());

        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine("Error retrieving department name: " + ex.Message);
        //                // You might want to log the exception or throw it further up
        //            }
        //        }
        //    }

        //    return departmentid;
        //}


        //private void LoadDepartmentData()
        //{

        //    using (connection = new SqlConnection(connectionString))
        //    {
        //        string query = "SELECT * FROM Department";
        //        using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
        //        {
        //            DataTable dataTable = new DataTable();
        //            try
        //            {
        //                connection.Open();
        //                adapter.Fill(dataTable);
        //                dataGridView1.DataSource = dataTable;
        //            }
        //            catch (SqlException ex)
        //            {
        //                MessageBox.Show("An error occurred: " + ex.Message);
        //            }
        //        }
        //    }
        //}

        //Async and await method
        private async Task LoadDepartmentDataAsync()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Department";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    DataTable dataTable = new DataTable();
                    try
                    {

                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            dataTable.Load(reader);
                        }

                        dataGridView1.DataSource = dataTable;
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message);
                    }

                }
            }
        }

        //Normal method
        //private void LoadEmployeeData()
        //{

        //    using (connection = new SqlConnection(connectionString))
        //    {
        //        string query = "SELECT * FROM Employee";
        //        using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
        //        {
        //            DataTable dataTable = new DataTable();
        //            try
        //            {
        //                connection.Open();
        //                adapter.Fill(dataTable);
        //                dataGridView2.DataSource = dataTable;
        //            }
        //            catch (SqlException ex)
        //            {
        //                MessageBox.Show("An error occurred: " + ex.Message);
        //            }
        //        }
        //    }
        //}

        //Async and Await method
        private async Task LoadEmployeeDataAsync()
        {
            using (connection = new SqlConnection(connectionString))
            {
                string query = "SELECT EmployeeID,FirstName, LastName, DepartmentID, MobilePhone, Status FROM Employee";

                DataTable dataTable = new DataTable();

                try
                {
                    using (connection = new SqlConnection(connectionString))
                    {
                        await connection.OpenAsync();

                        using (SqlCommand command = new SqlCommand(query, connection))
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            await Task.Run(() => adapter.Fill(dataTable));
                        }
                    }
                    DisplayInDataGridView(dataTable);
                    dataGridView2.Columns["EmployeeID"].ReadOnly = true;
                    dataGridView2.Columns["DepartmentID"].ReadOnly = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading data: " + ex.Message);
                }
            }
        }
        private void DisplayInDataGridView(DataTable dataTable)
        {
            dataGridView2.AutoGenerateColumns = false;
            dataGridView2.Columns.Clear();
            
            DataGridViewTextBoxColumn EmployeeID = new DataGridViewTextBoxColumn();
            EmployeeID.Name = "EmployeeID"; 
            EmployeeID.HeaderText = "EmployeeID"; 
            EmployeeID.DataPropertyName = "EmployeeID"; 
            dataGridView2.Columns.Add(EmployeeID);
            
            DataGridViewTextBoxColumn firstNameColumn = new DataGridViewTextBoxColumn();
            firstNameColumn.Name = "FirstName"; 
            firstNameColumn.HeaderText = "First Name"; 
            firstNameColumn.DataPropertyName = "FirstName"; 
            dataGridView2.Columns.Add(firstNameColumn);
            
            DataGridViewTextBoxColumn lastNameColumn = new DataGridViewTextBoxColumn();
            lastNameColumn.Name = "LastName";
            lastNameColumn.HeaderText = "Last Name";
            lastNameColumn.DataPropertyName = "LastName";
            dataGridView2.Columns.Add(lastNameColumn);
            
            DataGridViewTextBoxColumn idColumn = new DataGridViewTextBoxColumn();
            idColumn.Name = "DepartmentID";
            idColumn.HeaderText = "DepartmentID";
            idColumn.DataPropertyName = "DepartmentID";
            idColumn.Visible = true; 
            dataGridView2.Columns.Add(idColumn);

            DataGridViewTextBoxColumn mobilephone = new DataGridViewTextBoxColumn();
            mobilephone.Name = "MobilePhone";
            mobilephone.HeaderText = "MobilePhone";
            mobilephone.DataPropertyName = "MobilePhone";
            mobilephone.Visible = true; 
            dataGridView2.Columns.Add(mobilephone);

            DataGridViewTextBoxColumn status = new DataGridViewTextBoxColumn();
            status.Name = "Status";
            status.HeaderText = "Status";
            status.DataPropertyName = "Status";
            status.Visible = true; 
            dataGridView2.Columns.Add(status);

            DataGridViewButtonColumn editButtonColumn = new DataGridViewButtonColumn();
            editButtonColumn.Name = "Edit";
            editButtonColumn.HeaderText = "Edit";
            editButtonColumn.Text = "Edit";
            editButtonColumn.UseColumnTextForButtonValue = true;
            editButtonColumn.FlatStyle = FlatStyle.Standard;
            dataGridView2.Columns.Add(editButtonColumn);

            DataGridViewButtonColumn deleteButtonColumn = new DataGridViewButtonColumn();
            deleteButtonColumn.Name = "Delete";
            deleteButtonColumn.HeaderText = "Delete";
            deleteButtonColumn.Text = "Delete";
            deleteButtonColumn.UseColumnTextForButtonValue = true;
            deleteButtonColumn.FlatStyle = FlatStyle.Standard;
            dataGridView2.Columns.Add(deleteButtonColumn);
            
            dataGridView2.DataSource = dataTable;
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    string DepartmentName = cbxDepartment.Text;
        //    int DeptID =GetDepartmentIDAsync(DepartmentName);
        //    string FirstName = txtFirstName.Text;
        //    string LastName = txtLastName.Text;
        //    string FullName = txtFullName.Text;
        //    string BirthDate = dTPBirthDate.Value.ToString("dd/MM/yyyy");
        //    string Email = txtEmail.Text;
        //    string Title = cbxTitle.SelectedItem.ToString();
        //    if (Title == "-Select-")
        //    {
        //        Title = "";
        //    }
        //    string Prefix = cbxPrefix.SelectedItem.ToString();
        //    if (Prefix == "-Select-")
        //    {
        //        Prefix = "";
        //    }
        //    string Address = txtAddress.Text;
        //    string City = txtCity.Text;
        //    string State = cbxState.SelectedItem.ToString();
        //    if (State == "-Select-")
        //    {
        //        State = "";
        //    }
        //    string HomePhone = txtHomePhone.Text;
        //    string MobilePhone = txtMobilePhone.Text;
        //    string Skype = txtskype.Text;
        //    string Status = cbxStatus.SelectedItem.ToString();
        //    if (Status == "-Select-")
        //    {
        //        Status = "";
        //    }
        //    string Hiredate = dTPHireDate.Value.ToString("dd/MM/yyyy");

        //    using (connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();

        //        string sqlQuery = "INSERT INTO Employee (FirstName,LastName,FullName,BirthDate,Title, Prefix,Address,City,State,HomePhone,MobilePhone,Email,Skype,DepartmentID,Status,HireDate) VALUES (@FirstName,@LastName,@FullName,@BirthDate,@Title,@Prefix,@Address,@City,@State,@HomePhone,@MobilePhone,@Email,@Skype,@DepartmentID,@Status,@HireDate)";
        //        using (SqlCommand cmd = new SqlCommand(sqlQuery, connection))
        //        {
        //            // Use parameters to prevent SQL injection
        //            cmd.Parameters.AddWithValue("@FirstName", FirstName);
        //            cmd.Parameters.AddWithValue("@Lastname", LastName);
        //            cmd.Parameters.AddWithValue("@FullName", FullName);
        //            cmd.Parameters.AddWithValue("@BirthDate", BirthDate);
        //            cmd.Parameters.AddWithValue("@Title", Title);
        //            cmd.Parameters.AddWithValue("@Prefix", Prefix);
        //            cmd.Parameters.AddWithValue("@Address", Address);
        //            cmd.Parameters.AddWithValue("@City", City);
        //            cmd.Parameters.AddWithValue("@State", State);
        //            cmd.Parameters.AddWithValue("@HomePhone", HomePhone);
        //            cmd.Parameters.AddWithValue("@MobilePhone", MobilePhone);
        //            cmd.Parameters.AddWithValue("@Email", Email);
        //            cmd.Parameters.AddWithValue("@Skype", Skype);
        //            cmd.Parameters.AddWithValue("@DepartmentID", DeptID);
        //            cmd.Parameters.AddWithValue("@Status", Status);
        //            cmd.Parameters.AddWithValue("@HireDate", Hiredate);
        //            int rowsInserted = cmd.ExecuteNonQuery();
        //            MessageBox.Show("Rows Inserted Successfully");
        //            LoadDepartmentData();
        //            LoadEmployeeData();
        //        }

        //    }
        //}

        //Async and Await method

        private async void button1_Click(object sender, EventArgs e) // Added async
        {
            string DepartmentName = cbxDepartment.Text;

            int DeptID = await GetDepartmentIDAsync(DepartmentName);

            string FirstName = txtFirstName.Text;
            string LastName = txtLastName.Text;
            string FullName = txtFullName.Text;
            string BirthDate = dTPBirthDate.Value.ToString("dd/MM/yyyy");
            string Email = txtEmail.Text;

            string Title = cbxTitle.SelectedItem?.ToString() ?? "";
            if (Title == "-Select-") Title = "";

            string Prefix = cbxPrefix.SelectedItem?.ToString() ?? "";
            if (Prefix == "-Select-") Prefix = "";

            string Address = txtAddress.Text;
            string City = txtCity.Text;

            string State = cbxState.SelectedItem?.ToString() ?? "";
            if (State == "-Select-") State = "";

            string HomePhone = txtHomePhone.Text;
            string MobilePhone = txtMobilePhone.Text;
            string Skype = txtskype.Text;

            string Status = cbxStatus.SelectedItem?.ToString() ?? "";
            if (Status == "-Select-") Status = "";

            string Hiredate = dTPHireDate.Value.ToString("dd/MM/yyyy");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                await connection.OpenAsync();

                string sqlQuery = "INSERT INTO Employee (FirstName,LastName,FullName,BirthDate,Title, Prefix,Address,City,State,HomePhone,MobilePhone,Email,Skype,DepartmentID,Status,HireDate) VALUES (@FirstName,@LastName,@FullName,@BirthDate,@Title,@Prefix,@Address,@City,@State,@HomePhone,@MobilePhone,@Email,@Skype,@DepartmentID,@Status,@HireDate)";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@FirstName", FirstName);
                    cmd.Parameters.AddWithValue("@Lastname", LastName);
                    cmd.Parameters.AddWithValue("@FullName", FullName);
                    cmd.Parameters.AddWithValue("@BirthDate", BirthDate);
                    cmd.Parameters.AddWithValue("@Title", Title);
                    cmd.Parameters.AddWithValue("@Prefix", Prefix);
                    cmd.Parameters.AddWithValue("@Address", Address);
                    cmd.Parameters.AddWithValue("@City", City);
                    cmd.Parameters.AddWithValue("@State", State);
                    cmd.Parameters.AddWithValue("@HomePhone", HomePhone);
                    cmd.Parameters.AddWithValue("@MobilePhone", MobilePhone);
                    cmd.Parameters.AddWithValue("@Email", Email);
                    cmd.Parameters.AddWithValue("@Skype", Skype);
                    cmd.Parameters.AddWithValue("@DepartmentID", DeptID);
                    cmd.Parameters.AddWithValue("@Status", Status);
                    cmd.Parameters.AddWithValue("@HireDate", Hiredate);

                    int rowsInserted = await cmd.ExecuteNonQueryAsync();

                    MessageBox.Show("Rows Inserted Successfully");
                    await LoadDepartmentDataAsync();
                    await LoadEmployeeDataAsync();
                }
            }

        }
        private void DELETE_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0 && !dataGridView1.SelectedRows[0].IsNewRow)
            {
                while (dataGridView1.SelectedRows.Count > 0)
                {
                    dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);
                }

                MessageBox.Show("Selected row(s) deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Please select a row to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}











