/*!
* DisplayMonkey source file
* http://displaymonkey.org
*
* Copyright (c) 2015 Fuel9 LLC and contributors
*
* Released under the MIT license:
* http://opensource.org/licenses/MIT
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
//using System.Data;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
//using System.Drawing;
//using System.Drawing.Imaging;
//using Microsoft.SharePoint.Client;
//using System.Security;

namespace DisplayMonkey
{
	/// <summary>
	/// Summary description for getHtml
	/// </summary>
	public class getHtml : HttpTaskAsyncHandler
	{
		public override async Task ProcessRequestAsync(HttpContext context)
		{
			HttpRequest request = context.Request;
			HttpResponse response = context.Response;

            int frameId = request.IntOrZero("frame");
            int trace = request.IntOrZero("trace");

            try
			{
                Html html = new Html(frameId);

                response.Clear();
                response.Cache.SetCacheability(HttpCacheability.NoCache);
                response.Cache.SetSlidingExpiration(true);
                response.Cache.SetNoStore();
                response.ContentType = "text/html";

                if (html.Content.Contains("<div id=\"omsattning\""))
                {
                    string DisplayId = request.UrlReferrer.Query.Split('=')[1];
                    string LevelName = "";
                    string Datum = "", Butik = "", Koncept_dag = "", Butik_dag = "", Marginalkronor = "", Antal_kunder = "", Fys = "", LevelId = "", Koncept = "";
                    string labelclass = "omslabel";

                    try
                    {



                        using (SqlCommand dispcmd = new SqlCommand()
                        {
                            CommandType = CommandType.Text,
                            CommandText = "select top 1  L.Name,L.LevelId from Display D join Location L on d.LocationId = L.LocationId where DisplayId = @displayId",

                        })
                        {
                            dispcmd.Parameters.AddWithValue("@displayId", DisplayId);
                            dispcmd.ExecuteReaderExt((dispdr) =>
                            {
                                LevelName = dispdr.StringOrBlank("Name");
                                LevelId = Convert.ToString(dispdr.IntOrZero("LevelId"));
                                return false;
                            });
                        }

                        LevelName = LevelName.Split('-')[0].Trim();

                    }
                    catch (Exception Ex)
                    {
                        Console.WriteLine(Ex);
                        throw;
                    }

                    try
                    {
                        using (SqlCommand salecmd = new SqlCommand()
                        {
                            CommandType = CommandType.Text,
                            CommandText = "SELECT TOP 1  * FROM CoopSales WHERE Datum = @datum AND Butik = @butik",

                        })
                        {
                            salecmd.Parameters.AddWithValue("@datum", DateTime.Now.AddDays(-1).ToString("yyyyMMdd"));
                            salecmd.Parameters.AddWithValue("@butik", Convert.ToString(LevelName));
                            salecmd.ExecuteReaderExt((saledr) =>
                            {
                                Datum = Convert.ToString(saledr.StringOrBlank("Datum"));
                                Butik = Convert.ToString(saledr.IntOrZero("Butik"));
                                Koncept_dag = Convert.ToString(saledr.DecimalOrZero("Koncept_dag"));
                                Butik_dag = Convert.ToString(saledr.DecimalOrZero("Butik_dag"));
                                Fys = Convert.ToString(saledr.DecimalOrZero("Fys"));
                                Antal_kunder = Convert.ToString(saledr.DecimalOrZero("Antal_kunder"));
                                Marginalkronor = Convert.ToString(saledr.DecimalOrZero("Marginalkronor"));

                                return false;
                            });
                        }
                    }
                    catch (Exception Ex)
                    {
                        Console.WriteLine(Ex);
                        throw;
                    }

                    if (Butik.Length != 0)
                    {
                        string outstring = "";
                        try
                        {
                            if (LevelId == "3" || LevelId == "12")
                            {
                                labelclass = "omslabel_red";
                            }
                            else
                            {
                                labelclass = "omslabel_green";
                            }


                                outstring = "<div id=\"omsattning2\" style=\"margin-top -10px; margin-left: 10px;\">  " +
                            "<table style=\"border-collapse: collapse; height: 90px; width: 1800px;\" border=\"0\">  " +
                            "<tbody>  " +
                            "<tr style=\"height: 32px;\"> " +
                            "<td style=\"height: 32px;\" colspan=\"4\">" +
                            "<span class=\""+labelclass+"\">BUDGET</span>" +
                            "</td>  " +
                            "<td style=\"height: 32px;\" colspan=\"9\">" +
                            "<span class=\""+labelclass+"\">F&Ouml;REG&Aring;ENDE &Aring;R</span>" +
                            "</td> " +
                            "</tr> " +
                            "<tr style=\"height: 32px;\"> " +
                            "<td style=\"width: 60px; height: 46px;\" rowspan=\"2\"> ";
                            if (Convert.ToDecimal(Koncept_dag) < 100)
                            {
                                outstring += "<em class=\"material-icons\" style=\"font-size: 85px; color: #e74c3c;\">trending_down</em>";
                            }
                            else if (Convert.ToDecimal(Koncept_dag) > 100)
                            {
                                outstring += "<em class=\"material-icons\" style=\"font-size: 85px; color: #28b463;\">trending_up</em>";
                            }
                            else
                            {
                                outstring += "<em class=\"material-icons\" style=\"font-size: 85px; color: #f1c40f;\">trending_flat</em>";
                            }
                            outstring += "</td> " +
                            "<td style=\"width: 450px; height: 23px;\">" +
                            "<span style=\"font-family: CoopNew; font-size: 26pt;\">Kedja</span>" +
                            "</td>  " +
                            "<td style=\"width: 60px; height: 46px;\" rowspan=\"2\">";
                            if (Convert.ToDecimal(Butik_dag) < 100)
                            {
                                outstring += "<em class=\"material-icons\" style=\"font-size: 85px; color: #e74c3c;\">trending_down</em>";
                            }
                            else if (Convert.ToDecimal(Butik_dag) > 100)
                            {
                                outstring += "<em class=\"material-icons\" style=\"font-size: 85px; color: #28b463;\">trending_up</em>";
                            }
                            else
                            {
                                outstring += "<em class=\"material-icons\" style=\"font-size: 85px; color: #f1c40f;\">trending_flat</em>";
                            }
                            outstring += "</td>  " +
                            "<td style=\"width: 450px; height: 23px;\">" +
                            "<span style=\"font-family: CoopNew; font-size: 26pt;\">Butik</span>" +
                            "</td> " +
                            "<td style=\"width: 60px; height: 46px;\" rowspan=\"2\">";

                            if (Convert.ToDecimal(Marginalkronor) < 100)
                            {
                                outstring += "<em class=\"material-icons\" style=\"font-size: 85px; color: #e74c3c;\">trending_down</em>";
                            }
                            else if (Convert.ToDecimal(Marginalkronor) > 100)
                            {
                                outstring += "<em class=\"material-icons\" style=\"font-size: 85px; color: #28b463;\">trending_up</em>";
                            }
                            else
                            {
                                outstring += "<em class=\"material-icons\" style=\"font-size: 85px; color: #f1c40f;\">trending_flat</em>";
                            }
                            outstring += "</td>  " +
                            "<td style=\"width: 450px; height: 23px;\">" +
                            "<span style=\"font-family: CoopNew; font-size: 26pt;\">Marginalkronor</span>" +
                            "</td> " +
                            "<td style=\"width: 60px; height: 46px;\" rowspan=\"2\">";
                            if (Convert.ToDecimal(Antal_kunder) < 100)
                            {
                                outstring += "<em class=\"material-icons\" style=\"font-size: 85px; color: #e74c3c;\">trending_down</em>";
                            }
                            else if (Convert.ToDecimal(Antal_kunder) > 100)
                            {
                                outstring += "<em class=\"material-icons\" style=\"font-size: 85px; color: #28b463;\">trending_up</em>";
                            }
                            else
                            {
                                outstring += "<em class=\"material-icons\" style=\"font-size: 85px; color: #f1c40f;\">trending_flat</em>";
                            }
                            outstring += "</td>  " +
                            "<td style=\"width: 550px; height: 23px;\">" +
                            "<span style=\"font-family: CoopNew; font-size: 26pt;\">Antal kunder</span>" +
                            "</td> " +
                            "<td style=\"width: 60px; height: 46px;\" rowspan=\"2\">";
                            if (Convert.ToDecimal(Fys) > 100)
                            {
                                outstring += "<em class=\"material-icons\" style=\"font-size: 85px; color: #e74c3c;\">sentiment_very_dissatisfied</em>";
                            }
                            else if (Convert.ToDecimal(Fys) < 100)
                            {
                                outstring += "<em class=\"material-icons\" style=\"font-size: 85px; color: #28b463;\">sentiment_very_satisfied</em>";
                            }
                            else
                            {
                                outstring += "<em class=\"material-icons\" style=\"font-size: 85px; color: #f1c40f;\">sentiment_neutral</em>";
                            }
                            outstring +=
                            "</td>  " +
                            "<td style=\"width: 450px; height: 23px;\">" +
                            "<span style=\"font-family: CoopNew; font-size: 26pt;\">Fys</span>" +
                            "</td>  " +
                            "</tr>  " +
                            "<tr style=\"height: 23px;\">  " +
                            "<td>" +
                            "<span style=\"font-family: CoopNew; font-size: 26pt;\">" + Koncept_dag + "</span>" +
                            "</td>  " +
                            "<td style=\"height: 23px;\">" +
                            "<span style=\"font-family: CoopNew; font-size: 26pt;\">" + Butik_dag + "</span>" +
                            "</td>  " +
                            "<td style=\"height: 23px;\">" +
                            "<span style=\"font-family: CoopNew; font-size: 26pt;\">" + Marginalkronor + "</span>" +
                            "</td>  " +
                            "<td style=\"height: 23px;\">" +
                            "<span style=\"font-family: CoopNew; font-size: 26pt;\">" + Antal_kunder + "</span>" +
                            "</td>  " +
                            "<td style=\"height: 23px;\">" +
                            "<span style=\"font-family: CoopNew; font-size: 26pt;\">" + Fys + "</span>" +
                            "</td>" +
                            "</tr>" +
                            "</tbody>" +
                            "</table>" +
                            "</div>";

                            if (Butik.Length != 0)
                            {
                                await response.Output.WriteAsync(outstring);
                            }
                            else
                            {
                                //Content = "";
                            }
                        }
                        catch (Exception Ex)
                        {
                            Console.WriteLine(Ex);
                            throw;
                        }
                    }
                    else
                    {
                        try
                        {
                            if (LevelId == "5")
                                LevelId = "COOP";
                            else if (LevelId == "3")
                                LevelId = "PK";
                            else if (LevelId == "4")
                                LevelId = "GK";
                            using (SqlCommand salecmd = new SqlCommand()
                            {
                                CommandType = CommandType.Text,
                                CommandText = "SELECT TOP 1 cast(datum as varchar) as datum, koncept, "+
                                " case when SUM(fsg) = 0 OR SUM(budget_omsättning) = 0 THEN 100.00 ELSE " +
                                " (SUM(fsg) / SUM(Budget_Omsättning)) * 100 END as idx_budgetbutik,  "+
	                            " case when SUM(marginalkronor) = 0 OR SUM(marginalkronor_jmf) = 0 THEN 100.00 ELSE "+
                                " (SUM(marginalkronor) / SUM(marginalkronor_jmf)) * 100 END as idx_marginalkronor, "+
                                " case when SUM(antalkunder) = 0 OR SUM(Antalkunder_jmf) = 0 THEN 100.00 ELSE " +
                                " (SUM(antalkunder) / SUM(Antalkunder_jmf)) * 100 END as idx_antalkunder, "+
                                " case when SUM(fys) = 0 OR SUM(fys_jmf) = 0 THEN 100.00 ELSE " +
                                " (SUM(fys) / ISNULL(SUM(fys_jmf), SUM(fys))) * 100 END as idx_fys " +
                                " FROM( " +
                                " select distinct " +
                                " cast(datum as varchar) as datum, " +
                                " SUM(marginalkronor) as marginalkronor, " +
                                " SUM(snittköp) as snittköp, " +
                                " SUM(antalkunder) as antalkunder, " +
                                " SUM(fys) as fys, " +
                                " SUM(fsg) as fsg, " +
                                " SUM(marginalkronor_jmf) as marginalkronor_jmf, " +
                                " sum(cast(Budget_Omsättning as decimal)) as budget_omsättning, " +
                                " SUM(SUM_fsg_dag_koncept) AS  SUM_fsg_dag_koncept, " +
                                " SUM(sum_budget_dag_koncept) AS sum_budget_dag_koncept, " +
                                " SUM(Antalkunder_jmf) AS Antalkunder_jmf, " +
                                " SUM(Fys_jmf) AS Fys_jmf, " +
                                " case when koncept in ('01', 'CE', 'CF', 'CK', 'CN') then 'COOP' else koncept end as koncept " +
                                " FROM[BPMSRV2].[KV_BPW_Prod_Catalog].[dbo].[COOP_Direkt_allt_arkiv] cda " +
                                " WHERE cda.datum = @datum " +
                                " group by  koncept,datum " +
                                " ) a1 " +
                                " WHERE datum = @datum AND Koncept = @LevelId" +
                                " group by koncept, datum "
                            })
                            {
                                salecmd.Parameters.AddWithValue("@datum", DateTime.Now.AddDays(-1).ToString("yyyyMMdd"));
                                salecmd.Parameters.AddWithValue("@LevelId", Convert.ToString(LevelId));
                                salecmd.ExecuteReaderExt((saledr) =>
                                {
                                    Datum = Convert.ToString(saledr.StringOrBlank("Datum"));
                                    Koncept = Convert.ToString(saledr.StringOrBlank("Koncept"));
                                    Koncept_dag = String.Format("{0:0.00}", saledr.DecimalOrZero("idx_budgetbutik"));
                                    Butik_dag = String.Format("{0:0.00}", saledr.DecimalOrZero("idx_budgetbutik"));
                                    Fys = String.Format("{0:0.00}", saledr.DecimalOrZero("idx_fys"));
                                    Antal_kunder = String.Format("{0:0.00}", saledr.DecimalOrZero("idx_antalkunder"));
                                    Marginalkronor = String.Format("{0:0.00}", saledr.DecimalOrZero("idx_marginalkronor"));

                                    return false;
                                });
                            }
                        }
                        catch (Exception Ex)
                        {
                            Console.WriteLine(Ex);
                            throw;
                        }
                        String.Format("{0:0.00}", 123.0);


                        if (Koncept.Length != 0)
                        {
                            if (Koncept == "GK")
                                Koncept = "MaxiMat";
                            else if (Koncept == "PK")
                                Koncept = "Pekås";
                            string outstring = "";
                            try
                            {

                                outstring = "<div id=\"omsattning2\" style=\"margin-top -10px; margin-left: 10px;\">  " +
                                "<table style=\"border-collapse: collapse; height: 90px; width: 1800px;\" border=\"0\">  " +
                                "<tbody>  " +
                                "<tr style=\"height: 32px;\"> " +
                                "<td style=\"height: 32px;\" colspan=\"4\">" +
                                "<span class=\"omslabel\">BUDGET</span>" +
                                "</td>  " +
                                "<td style=\"height: 32px;\" colspan=\"9\">" +
                                "<span class=\"omslabel\">F&Ouml;REG&Aring;ENDE &Aring;R</span>" +
                                "</td> " +
                                "</tr> " +
                                "<tr style=\"height: 32px;\"> " +
                                "<td style=\"width: 60px; height: 46px;\" rowspan=\"2\"> ";
                                
                                outstring += "<em class=\"material-icons\" style=\"font-size: 85px; color: #f1c40f;\">local_grocery_store</em>";
                                outstring += "</td> " +
                                "<td style=\"width: 450px; height: 23px;\">" +
                                "<span style=\"font-family: CoopNew; font-size: 26pt;\">Kedja</span>" +
                                "</td>  " +
                                "<td style=\"width: 60px; height: 46px;\" rowspan=\"2\">";
                                if (Convert.ToDecimal(Butik_dag) < 100)
                                {
                                    outstring += "<em class=\"material-icons\" style=\"font-size: 85px; color: #e74c3c;\">trending_down</em>";
                                }
                                else if (Convert.ToDecimal(Butik_dag) > 100)
                                {
                                    outstring += "<em class=\"material-icons\" style=\"font-size: 85px; color: #28b463;\">trending_up</em>";
                                }
                                else
                                {
                                    outstring += "<em class=\"material-icons\" style=\"font-size: 85px; color: #f1c40f;\">trending_flat</em>";
                                }
                                outstring += "</td>  " +
                                "<td style=\"width: 450px; height: 23px;\">" +
                                "<span style=\"font-family: CoopNew; font-size: 26pt;\">Kedja</span>" +
                                "</td> " +
                                "<td style=\"width: 60px; height: 46px;\" rowspan=\"2\">";

                                if (Convert.ToDecimal(Marginalkronor) < 100)
                                {
                                    outstring += "<em class=\"material-icons\" style=\"font-size: 85px; color: #e74c3c;\">trending_down</em>";
                                }
                                else if (Convert.ToDecimal(Marginalkronor) > 100)
                                {
                                    outstring += "<em class=\"material-icons\" style=\"font-size: 85px; color: #28b463;\">trending_up</em>";
                                }
                                else
                                {
                                    outstring += "<em class=\"material-icons\" style=\"font-size: 85px; color: #f1c40f;\">trending_flat</em>";
                                }
                                outstring += "</td>  " +
                                "<td style=\"width: 450px; height: 23px;\">" +
                                "<span style=\"font-family: CoopNew; font-size: 26pt;\">Marginalkronor</span>" +
                                "</td> " +
                                "<td style=\"width: 60px; height: 46px;\" rowspan=\"2\">";
                                if (Convert.ToDecimal(Antal_kunder) < 100)
                                {
                                    outstring += "<em class=\"material-icons\" style=\"font-size: 85px; color: #e74c3c;\">trending_down</em>";
                                }
                                else if (Convert.ToDecimal(Antal_kunder) > 100)
                                {
                                    outstring += "<em class=\"material-icons\" style=\"font-size: 85px; color: #28b463;\">trending_up</em>";
                                }
                                else
                                {
                                    outstring += "<em class=\"material-icons\" style=\"font-size: 85px; color: #f1c40f;\">trending_flat</em>";
                                }
                                outstring += "</td>  " +
                                "<td style=\"width: 550px; height: 23px;\">" +
                                "<span style=\"font-family: CoopNew; font-size: 26pt;\">Antal kunder</span>" +
                                "</td> " +
                                "<td style=\"width: 60px; height: 46px;\" rowspan=\"2\">";
                                if (Convert.ToDecimal(Fys) > 100)
                                {
                                    outstring += "<em class=\"material-icons\" style=\"font-size: 85px; color: #e74c3c;\">sentiment_very_dissatisfied</em>";
                                }
                                else if (Convert.ToDecimal(Fys) < 100)
                                {
                                    outstring += "<em class=\"material-icons\" style=\"font-size: 85px; color: #28b463;\">sentiment_very_satisfied</em>";
                                }
                                else
                                {
                                    outstring += "<em class=\"material-icons\" style=\"font-size: 85px; color: #f1c40f;\">sentiment_neutral</em>";
                                }
                                outstring +=
                                "</td>  " +
                                "<td style=\"width: 450px; height: 23px;\">" +
                                "<span style=\"font-family: CoopNew; font-size: 26pt;\">Fys</span>" +
                                "</td>  " +
                                "</tr>  " +
                                "<tr style=\"height: 23px;\">  " +
                                "<td>" +
                                "<span style=\"font-family: CoopNew; font-size: 26pt;\">" + Koncept + "</span>" +
                                "</td>  " +
                                "<td style=\"height: 23px;\">" +
                                "<span style=\"font-family: CoopNew; font-size: 26pt;\">" + Butik_dag + "</span>" +
                                "</td>  " +
                                "<td style=\"height: 23px;\">" +
                                "<span style=\"font-family: CoopNew; font-size: 26pt;\">" + Marginalkronor + "</span>" +
                                "</td>  " +
                                "<td style=\"height: 23px;\">" +
                                "<span style=\"font-family: CoopNew; font-size: 26pt;\">" + Antal_kunder + "</span>" +
                                "</td>  " +
                                "<td style=\"height: 23px;\">" +
                                "<span style=\"font-family: CoopNew; font-size: 26pt;\">" + Fys + "</span>" +
                                "</td>" +
                                "</tr>" +
                                "</tbody>" +
                                "</table>" +
                                "</div>";

                                if (Koncept.Length != 0)
                                {
                                    await response.Output.WriteAsync(outstring);
                                }
                                else
                                {
                                    //Content = "";
                                }
                            }
                            catch (Exception Ex)
                            {
                                Console.WriteLine(Ex);
                                throw;
                            }
                        }


                        /*
                        await response.Output.WriteAsync(html.Content);
                        await response.Output.FlushAsync();*/
                    }

                }
                else
                {
                    await response.Output.WriteAsync(html.Content);
                    await response.Output.FlushAsync();
                }
                
            }

			catch (Exception ex)
			{
                if (trace == 0)
                    response.Write(ex.Message);
                else
                    response.Write(ex.ToString());
            }
		}
	}
}