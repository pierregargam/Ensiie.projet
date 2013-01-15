using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ensiie.projet3.Models
{
    using System.Data;
    using System.Data.SqlClient;

    public class access
    {
        //This is your database connection:
        static string connectionString = "Data Source=.\\SQLEXPRESS;"
                + "AttachDbFilename=\"C:\\Users\\pierre\\documents\\VISUAL STUDIO 2010\\PROJECTS\\ENSIIE.PROJET31\\ENSIIE.PROJET3\\APP_DATA\\TEST.MDF\";"
                + "User Instance=True;"
                + "Integrated Security=SSPI;";

        static SqlConnection cn = new SqlConnection(connectionString);

        public void delete_tables_ref_theme(int theme_to_del_id, string chaine)
        {

            SqlParameter myParam_theme_to_del_id = new SqlParameter("@myParam_theme_to_del_id", SqlDbType.Int, 4);
            myParam_theme_to_del_id.Value = theme_to_del_id;

            try
            {

                System.Diagnostics.Debug.WriteLine("ATTENTION id: " + theme_to_del_id);

                cn.Open();

                SqlCommand myCommand1 = new SqlCommand(chaine, cn);
                myCommand1.Parameters.Add(myParam_theme_to_del_id);
                myCommand1.ExecuteReader();

            }
            catch (SqlException e)
            {
                string msg = "";
                for (int i = 0; i < e.Errors.Count; i++)
                {
                    msg += "Error #" + i + " Message: " + e.Errors[i].Message + "\n";
                }
                System.Diagnostics.Debug.WriteLine(msg);
            }
            finally
            {
                if (cn.State != ConnectionState.Closed)
                {
                    cn.Close();
                }
            }
        }

        public void delete_tables_ref_theme3(int theme_to_del_id)
        {
            String stat = "delete from Stat where theme_id = @myParam_theme_to_del_id";
            String message = "delete from Message where theme_id = @myParam_theme_to_del_id";
            String abonnement = "delete from Abonnement where theme_id = @myParam_theme_to_del_id";
            String cmt = "delete from Commentaire where theme_id = @myParam_theme_to_del_id";
            String theme = "delete from Theme where id = @myParam_theme_to_del_id";

            delete_tables_ref_theme(theme_to_del_id, stat);
            delete_tables_ref_theme(theme_to_del_id, cmt);
            delete_tables_ref_theme(theme_to_del_id, message);
            delete_tables_ref_theme(theme_to_del_id, abonnement);
            delete_tables_ref_theme(theme_to_del_id, theme);
            
        }

        public bool insert_ab(int id_theme, int id_agent)
        {
            SqlParameter myParam_theme = new SqlParameter("@myParam_theme", SqlDbType.Int, 4);
            myParam_theme.Value = id_theme;

            SqlParameter myParam_agent = new SqlParameter("@myParam_agent", SqlDbType.Int, 4);
            myParam_agent.Value = id_agent;

            bool ret = false;

            String requete = "insert into Abonnement (theme_id,collaborateur_id) "
            + "values(@myParam_theme,@myParam_agent)";

            try
            {

                cn.Open();

                SqlCommand myCommand1 = new SqlCommand(requete, cn);
                myCommand1.Parameters.Add(myParam_theme);
                myCommand1.Parameters.Add(myParam_agent);
                myCommand1.ExecuteReader();
                ret = true;

            }
            catch (SqlException e)
            {
                string msg = "";
                for (int i = 0; i < e.Errors.Count; i++)
                {
                    msg += "Error #" + i + " Message: " + e.Errors[i].Message + "\n";
                }
                System.Diagnostics.Debug.WriteLine(msg);
            }
            finally
            {
                if (cn.State != ConnectionState.Closed)
                {
                    cn.Close();
                }
            }
            return ret;
        }

        public bool delete_ab(int id_theme, int id_agent)
        {
            SqlParameter myParam_theme = new SqlParameter("@myParam_theme", SqlDbType.Int, 4);
            myParam_theme.Value = id_theme;

            SqlParameter myParam_agent = new SqlParameter("@myParam_agent", SqlDbType.Int, 4);
            myParam_agent.Value = id_agent;

            bool ret = false;

            String requete = "delete from Abonnement where "
            + "theme_id = @myParam_theme and collaborateur_id = @myParam_agent";

            try
            {

                cn.Open();

                SqlCommand myCommand1 = new SqlCommand(requete, cn);
                myCommand1.Parameters.Add(myParam_theme);
                myCommand1.Parameters.Add(myParam_agent);
                myCommand1.ExecuteReader();
                ret = true;

            }
            catch (SqlException e)
            {
                string msg = "";
                for (int i = 0; i < e.Errors.Count; i++)
                {
                    msg += "Error #" + i + " Message: " + e.Errors[i].Message + "\n";
                }
                System.Diagnostics.Debug.WriteLine(msg);
            }
            finally
            {
                if (cn.State != ConnectionState.Closed)
                {
                    cn.Close();
                }
            }
            return ret;
        }

        public bool verif_ab(int id_theme, int id_agent)
        {
            SqlParameter myParam_theme = new SqlParameter("@myParam_theme", SqlDbType.Int, 4);
            myParam_theme.Value = id_theme;

            SqlParameter myParam_agent = new SqlParameter("@myParam_agent", SqlDbType.Int, 4);
            myParam_agent.Value = id_agent;

            bool ret = false;

            String requete = "select COUNT(id) AS Expr1 from Abonnement where "
            + "theme_id = @myParam_theme and collaborateur_id = @myParam_agent";

            try
            {

                cn.Open();

                SqlCommand myCommand1 = new SqlCommand(requete, cn);
                myCommand1.Parameters.Add(myParam_theme);
                myCommand1.Parameters.Add(myParam_agent);
                SqlDataReader reader = myCommand1.ExecuteReader();

                while (reader.Read())
                {
                    System.Diagnostics.Debug.WriteLine("__________________________________" + String.Format("{0}", reader[0]) + "__________");
                    if (((int)reader[0]) > 0)
                    {
                        ret = true;
                    }
                }
                
            }
            catch (SqlException e)
            {
                string msg = "";
                for (int i = 0; i < e.Errors.Count; i++)
                {
                    msg += "Error #" + i + " Message: " + e.Errors[i].Message + "\n";
                }
                System.Diagnostics.Debug.WriteLine(msg);
            }
            finally
            {
                if (cn.State != ConnectionState.Closed)
                {
                    cn.Close();
                }
            }
            return ret;
        }

        public int stat_theme(int id_theme)
        {
            SqlParameter myParam_theme = new SqlParameter("@myParam_theme", SqlDbType.Int, 4);
            myParam_theme.Value = id_theme;

            String requete = "select count(id) AS Expr1 from Message where "
            + "theme_id = @myParam_theme";

            int ret = 0;

            try
            {

                cn.Open();

                SqlCommand myCommand1 = new SqlCommand(requete, cn);
                myCommand1.Parameters.Add(myParam_theme);
                SqlDataReader reader = myCommand1.ExecuteReader();

                while (reader.Read())
                {
                    if (((int)reader[0]) > 0)
                    {
                        ret = (int)reader[0];
                    }
                }

            }
            catch (SqlException e)
            {
                string msg = "";
                for (int i = 0; i < e.Errors.Count; i++)
                {
                    msg += "Error #" + i + " Message: " + e.Errors[i].Message + "\n";
                }
                System.Diagnostics.Debug.WriteLine(msg);
            }
            finally
            {
                if (cn.State != ConnectionState.Closed)
                {
                    cn.Close();
                }
            }
            return ret;
        }

        public void maj_stat(int id_theme)
        {
            SqlParameter myParam_theme = new SqlParameter("@myParam_theme", SqlDbType.Int, 4);
            myParam_theme.Value = id_theme;

            SqlParameter myone = new SqlParameter("@myone", SqlDbType.Int, 4);
            myone.Value = 1;

            String requete = "UPDATE Stat SET nombre_vues = ((SELECT SUM(nombre_vues) AS Expr1 FROM Stat AS Stat_1 WHERE (theme_id = @myParam_theme)) + @myone) WHERE (theme_id = @myParam_theme)";

            try
            {

                cn.Open();

                SqlCommand myCommand1 = new SqlCommand(requete, cn);
                myCommand1.Parameters.Add(myParam_theme);
                myCommand1.Parameters.Add(myone);
                SqlDataReader reader = myCommand1.ExecuteReader();

            }
            catch (SqlException e)
            {
                string msg = "";
                for (int i = 0; i < e.Errors.Count; i++)
                {
                    msg += "Error #" + i + " Message: " + e.Errors[i].Message + "\n";
                }
                System.Diagnostics.Debug.WriteLine(msg);
            }
            finally
            {
                if (cn.State != ConnectionState.Closed)
                {
                    cn.Close();
                }
            }
        }

        public bool verif_agent_has_news(int id_news, int id_agent)
        {
            SqlParameter myParam_news = new SqlParameter("@myParam_news", SqlDbType.Int, 4);
            myParam_news.Value = id_news;

            SqlParameter myParam_agent = new SqlParameter("@myParam_agent", SqlDbType.Int, 4);
            myParam_agent.Value = id_agent;

            bool ret = false;

            String requete = "select COUNT(id) AS Expr1 from News where "
            + "id = @myParam_news and collaborateur_id = @myParam_agent";

            try
            {

                cn.Open();

                SqlCommand myCommand1 = new SqlCommand(requete, cn);
                myCommand1.Parameters.Add(myParam_news);
                myCommand1.Parameters.Add(myParam_agent);
                SqlDataReader reader = myCommand1.ExecuteReader();

                while (reader.Read())
                {
                    if (((int)reader[0]) > 0)
                    {
                        ret = true;
                    }
                }

            }
            catch (SqlException e)
            {
                string msg = "";
                for (int i = 0; i < e.Errors.Count; i++)
                {
                    msg += "Error #" + i + " Message: " + e.Errors[i].Message + "\n";
                }
                System.Diagnostics.Debug.WriteLine(msg);
            }
            finally
            {
                if (cn.State != ConnectionState.Closed)
                {
                    cn.Close();
                }
            }
            return ret;
        }

        public bool verif_agent_has_avis(int id_avis, int id_agent)
        {
            SqlParameter myParam_avis = new SqlParameter("@myParam_avis", SqlDbType.Int, 4);
            myParam_avis.Value = id_avis;

            SqlParameter myParam_agent = new SqlParameter("@myParam_agent", SqlDbType.Int, 4);
            myParam_agent.Value = id_agent;

            bool ret = false;

            String requete = "select COUNT(id) AS Expr1 from Avis_news where "
            + "id = @myParam_avis and collaborateur_id = @myParam_agent";

            try
            {

                cn.Open();

                SqlCommand myCommand1 = new SqlCommand(requete, cn);
                myCommand1.Parameters.Add(myParam_avis);
                myCommand1.Parameters.Add(myParam_agent);
                SqlDataReader reader = myCommand1.ExecuteReader();

                while (reader.Read())
                {
                    if (((int)reader[0]) > 0)
                    {
                        ret = true;
                    }
                }

            }
            catch (SqlException e)
            {
                string msg = "";
                for (int i = 0; i < e.Errors.Count; i++)
                {
                    msg += "Error #" + i + " Message: " + e.Errors[i].Message + "\n";
                }
                System.Diagnostics.Debug.WriteLine(msg);
            }
            finally
            {
                if (cn.State != ConnectionState.Closed)
                {
                    cn.Close();
                }
            }
            return ret;
        }

        public bool verif_agent_has_message(int id_message, int id_agent)
        {
            SqlParameter myParam_message = new SqlParameter("@myParam_message", SqlDbType.Int, 4);
            myParam_message.Value = id_message;

            SqlParameter myParam_agent = new SqlParameter("@myParam_agent", SqlDbType.Int, 4);
            myParam_agent.Value = id_agent;

            bool ret = false;

            String requete = "select COUNT(id) AS Expr1 from Message where "
            + "id = @myParam_message and collaborateur_id = @myParam_agent";

            try
            {

                cn.Open();

                SqlCommand myCommand1 = new SqlCommand(requete, cn);
                myCommand1.Parameters.Add(myParam_message);
                myCommand1.Parameters.Add(myParam_agent);
                SqlDataReader reader = myCommand1.ExecuteReader();

                while (reader.Read())
                {
                    if (((int)reader[0]) > 0)
                    {
                        ret = true;
                    }
                }

            }
            catch (SqlException e)
            {
                string msg = "";
                for (int i = 0; i < e.Errors.Count; i++)
                {
                    msg += "Error #" + i + " Message: " + e.Errors[i].Message + "\n";
                }
                System.Diagnostics.Debug.WriteLine(msg);
            }
            finally
            {
                if (cn.State != ConnectionState.Closed)
                {
                    cn.Close();
                }
            }
            return ret;
        }

        public bool verif_agent_has_commentaire(int id_commentaire, int id_agent)
        {
            SqlParameter myParam_commentaire = new SqlParameter("@myParam_commentaire", SqlDbType.Int, 4);
            myParam_commentaire.Value = id_commentaire;

            SqlParameter myParam_agent = new SqlParameter("@myParam_agent", SqlDbType.Int, 4);
            myParam_agent.Value = id_agent;

            bool ret = false;

            String requete = "select COUNT(id) AS Expr1 from Commentaire where "
            + "id = @myParam_commentaire and collaborateur_id = @myParam_agent";

            try
            {

                cn.Open();

                SqlCommand myCommand1 = new SqlCommand(requete, cn);
                myCommand1.Parameters.Add(myParam_commentaire);
                myCommand1.Parameters.Add(myParam_agent);
                SqlDataReader reader = myCommand1.ExecuteReader();

                while (reader.Read())
                {
                    if (((int)reader[0]) > 0)
                    {
                        ret = true;
                    }
                }

            }
            catch (SqlException e)
            {
                string msg = "";
                for (int i = 0; i < e.Errors.Count; i++)
                {
                    msg += "Error #" + i + " Message: " + e.Errors[i].Message + "\n";
                }
                System.Diagnostics.Debug.WriteLine(msg);
            }
            finally
            {
                if (cn.State != ConnectionState.Closed)
                {
                    cn.Close();
                }
            }
            return ret;
        }
    }
}