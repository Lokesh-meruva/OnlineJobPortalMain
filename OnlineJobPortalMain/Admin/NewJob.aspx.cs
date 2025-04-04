using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace OnlineJobPortalMain.Admin
{
    public partial class NewJob : System.Web.UI.Page
    {
        // Use your connection string name (here assumed "cs")
        string connectionString = ConfigurationManager.ConnectionStrings["cs"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["admin"] == null)
            {
                Response.Redirect("../User/Login.aspx");
            }
            if (!IsPostBack)
            {
                FillData();
            }
        }

        private void FillData()
        {
            if (Request.QueryString["id"] != null)
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM Jobs WHERE JobId = @JobId";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@JobId", Request.QueryString["id"].ToString());
                        con.Open();
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                txtJobTitle.Text = sdr["Title"].ToString();
                                txtNoOfPost.Text = sdr["NoOfPost"].ToString();
                                txtDescription.Text = sdr["Description"].ToString();
                                txtQualification.Text = sdr["Qualification"].ToString();
                                txtExperience.Text = sdr["Experience"].ToString();
                                // Note: Adjust the column name if necessary. Here we assume the DB column is named "Specialisation"
                                txtSpecialization.Text = sdr["Specialisation"].ToString();
                                txtLastDate.Text = Convert.ToDateTime(sdr["LastDateToApply"]).ToString("yyyy-MM-dd");
                                txtSalary.Text = sdr["Salary"].ToString();
                                ddlJobType.SelectedValue = sdr["JobType"].ToString();
                                txtCompany.Text = sdr["CompanyName"].ToString();
                                txtWebsite.Text = sdr["Website"].ToString();
                                txtEmail.Text = sdr["Email"].ToString();
                                txtAddress.Text = sdr["Address"].ToString();
                                ddlCountry.SelectedValue = sdr["Country"].ToString();
                                txtState.Text = sdr["State"].ToString();
                                btnAdd.Text = "Update";
                            }
                        }
                        else
                        {
                            lblMsg.Text = "Job Not Found..!";
                            lblMsg.CssClass = "alert alert-danger";
                        }
                        sdr.Close();
                        con.Close();
                    }
                }
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string imagePath = string.Empty;
                bool isValidToExecute = true;

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    if (Request.QueryString["id"] != null)
                    {
                        // Update existing job
                        string updateQuery = "UPDATE Jobs SET Title = @Title, NoOfPost = @NoOfPost, Description = @Description, " +
                                             "Qualification = @Qualification, Experience = @Experience, Specialization = @Specialization, " +
                                             "LastDateToApply = @LastDateToApply, Salary = @Salary, JobType = @JobType, CompanyName = @CompanyName, ";

                        // If a new file is uploaded, update CompanyImage
                        if (fuCompanyLogo.HasFile)
                        {
                            if (Utils.IsValidExtension(fuCompanyLogo.FileName))
                            {
                                Guid obj = Guid.NewGuid();
                                imagePath = "Images/" + obj.ToString() + fuCompanyLogo.FileName;
                                fuCompanyLogo.PostedFile.SaveAs(Server.MapPath("~/Images/") + obj.ToString() + fuCompanyLogo.FileName);
                                updateQuery += "CompanyImage = @CompanyImage, ";
                            }
                            else
                            {
                                lblMsg.Text = "Please select .jpg, .jpeg, .png file for Logo";
                                lblMsg.CssClass = "alert alert-danger";
                                isValidToExecute = false;
                            }
                        }

                        updateQuery += "Website = @Website, Email = @Email, Address = @Address, Country = @Country, State = @State " +
                                       "WHERE JobId = @JobId";

                        if (isValidToExecute)
                        {
                            using (SqlCommand cmd = new SqlCommand(updateQuery, con))
                            {
                                cmd.Parameters.AddWithValue("@Title", txtJobTitle.Text.Trim());
                                cmd.Parameters.AddWithValue("@NoOfPost", txtNoOfPost.Text.Trim());
                                cmd.Parameters.AddWithValue("@Description", txtDescription.Text.Trim());
                                cmd.Parameters.AddWithValue("@Qualification", txtQualification.Text.Trim());
                                cmd.Parameters.AddWithValue("@Experience", txtExperience.Text.Trim());
                                cmd.Parameters.AddWithValue("@Specialization", txtSpecialization.Text.Trim());
                                cmd.Parameters.AddWithValue("@LastDateToApply", txtLastDate.Text.Trim());
                                cmd.Parameters.AddWithValue("@Salary", txtSalary.Text.Trim());
                                cmd.Parameters.AddWithValue("@JobType", ddlJobType.SelectedValue);
                                cmd.Parameters.AddWithValue("@CompanyName", txtCompany.Text.Trim());
                                cmd.Parameters.AddWithValue("@Website", txtWebsite.Text.Trim());
                                cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                                cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                                cmd.Parameters.AddWithValue("@Country", ddlCountry.SelectedValue);
                                cmd.Parameters.AddWithValue("@State", txtState.Text.Trim());
                                cmd.Parameters.AddWithValue("@JobId", Request.QueryString["id"].ToString());
                                if (fuCompanyLogo.HasFile && Utils.IsValidExtension(fuCompanyLogo.FileName))
                                {
                                    cmd.Parameters.AddWithValue("@CompanyImage", imagePath);
                                }
                                con.Open();
                                int res = cmd.ExecuteNonQuery();
                                con.Close();
                                if (res > 0)
                                {
                                    lblMsg.Text = "Job updated successfully!";
                                    lblMsg.CssClass = "alert alert-success";
                                }
                                else
                                {
                                    lblMsg.Text = "Failed to update the job.";
                                    lblMsg.CssClass = "alert alert-danger";
                                }
                            }
                        }
                    }
                    else
                    {
                        // Insert new job
                        string insertQuery = "INSERT INTO Jobs (Title, NoOfPost, Description, Qualification, Experience, Specialisation, LastDateToApply, Salary, JobType, CompanyName, CompanyImage, Website, Email, Address, Country, State, CreateDate) " +
                                             "VALUES (@Title, @NoOfPost, @Description, @Qualification, @Experience, @Specialisation, @LastDateToApply, @Salary, @JobType, @CompanyName, @CompanyImage, @Website, @Email, @Address, @Country, @State, @CreateDate)";
                        using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                        {
                            DateTime time = DateTime.Now;
                            cmd.Parameters.AddWithValue("@Title", txtJobTitle.Text.Trim());
                            cmd.Parameters.AddWithValue("@NoOfPost", txtNoOfPost.Text.Trim());
                            cmd.Parameters.AddWithValue("@Description", txtDescription.Text.Trim());
                            cmd.Parameters.AddWithValue("@Qualification", txtQualification.Text.Trim());
                            cmd.Parameters.AddWithValue("@Experience", txtExperience.Text.Trim());
                            // For insert, note the column is spelled "Specialisation" as in your DB
                            cmd.Parameters.AddWithValue("@Specialisation", txtSpecialization.Text.Trim());
                            cmd.Parameters.AddWithValue("@LastDateToApply", txtLastDate.Text.Trim());
                            cmd.Parameters.AddWithValue("@Salary", txtSalary.Text.Trim());
                            cmd.Parameters.AddWithValue("@JobType", ddlJobType.SelectedValue);
                            cmd.Parameters.AddWithValue("@CompanyName", txtCompany.Text.Trim());
                            cmd.Parameters.AddWithValue("@Website", txtWebsite.Text.Trim());
                            cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                            cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                            cmd.Parameters.AddWithValue("@Country", ddlCountry.SelectedValue);
                            cmd.Parameters.AddWithValue("@State", txtState.Text.Trim());
                            cmd.Parameters.AddWithValue("@CreateDate", time.ToString("yyyy-MM-dd HH:mm:ss"));

                            if (fuCompanyLogo.HasFile)
                            {
                                if (Utils.IsValidExtension(fuCompanyLogo.FileName))
                                {
                                    Guid obj = Guid.NewGuid();
                                    imagePath = "Images/" + obj.ToString() + fuCompanyLogo.FileName;
                                    fuCompanyLogo.PostedFile.SaveAs(Server.MapPath("~/Images/") + obj.ToString() + fuCompanyLogo.FileName);
                                }
                                else
                                {
                                    lblMsg.Text = "Please select .jpg, .jpeg, .png file for Logo";
                                    lblMsg.CssClass = "alert alert-danger";
                                    isValidToExecute = false;
                                }
                            }
                            cmd.Parameters.AddWithValue("@CompanyImage", imagePath);

                            if (isValidToExecute)
                            {
                                con.Open();
                                int res = cmd.ExecuteNonQuery();
                                con.Close();
                                if (res > 0)
                                {
                                    lblMsg.Text = "Job saved successfully!";
                                    lblMsg.CssClass = "alert alert-success";
                                    ClearFields();
                                }
                                else
                                {
                                    lblMsg.Text = "Cannot save the record, please try after sometime!";
                                    lblMsg.CssClass = "alert alert-danger";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // For debugging purposes; consider logging the error in production
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        //private bool IsValidExtension(string fileName)
        //{
        //    string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
        //    foreach (string ext in allowedExtensions)
        //    {
        //        if (fileName.ToLower().EndsWith(ext))
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        private void ClearFields()
        {
            txtJobTitle.Text = "";
            txtNoOfPost.Text = "";
            txtDescription.Text = "";
            txtQualification.Text = "";
            txtExperience.Text = "";
            txtSpecialization.Text = "";
            txtLastDate.Text = "";
            txtSalary.Text = "";
            txtCompany.Text = "";
            txtWebsite.Text = "";
            txtEmail.Text = "";
            txtAddress.Text = "";
            ddlJobType.ClearSelection();
            ddlCountry.ClearSelection();
            txtState.Text = "";
        }
    }
}
