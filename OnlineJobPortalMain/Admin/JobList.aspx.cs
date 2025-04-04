using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineJobPortalMain.Admin
{
    public partial class JobList : System.Web.UI.Page
    {
        string str = ConfigurationManager.ConnectionStrings["cs"].ConnectionString;
        private SqlConnection con;
        private SqlCommand cmd;
        DataTable dt;

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (Session["admin"] == null)
            {
                Response.Redirect("../User/Login.aspx");
            }

            if (!IsPostBack)
            {
                ShowJob();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ShowJob();
            }
        }

        private void ShowJob()
        {
            string query = string.Empty;
            SqlConnection con = new SqlConnection(str);

            query = @"SELECT ROW_NUMBER() OVER (ORDER BY (SELECT 1)) AS [Sr.No],JobId,Title,NoOfPost,Qualification,Experience,
            LastDateToApply,CompanyName,Country,State,CreateDate FROM Jobs";

            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            GridView1.DataSource = dt;
            GridView1.DataBind();

        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            ShowJob();
        }

        protected void GridView1_RowDeleting1(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                GridViewRow row = GridView1.Rows[e.RowIndex];
                int jobId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                con = new SqlConnection(str);
                cmd = new SqlCommand("DELETE FROM Jobs WHERE JobId = @id", con);
                cmd.Parameters.AddWithValue("@id", jobId);
                con.Open();

                int r = cmd.ExecuteNonQuery();
                if (r > 0)
                {
                    lblMsg.Text = "Job Deleted Successfully";
                    lblMsg.CssClass = "text-success";
                }
                else
                {
                    lblMsg.Text = "Failed to Delete Job";
                    lblMsg.CssClass = "text-danger";
                }
                con.Close();
                GridView1.EditIndex = -1;
                ShowJob();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }

        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if(e.CommandName == "EditJob")
            {
                Response.Redirect("EditJob.aspx?JobId=" + e.CommandArgument.ToString() );
            }
        }
    }
}
