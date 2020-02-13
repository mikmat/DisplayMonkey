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
                    string Datum = "", Butik = "", Koncept_dag = "", Butik_dag = "", Marginalkronor = "", Snittkop = "", Antal_kunder = "", Fys = "";


                    try
                    {



                        using (SqlCommand dispcmd = new SqlCommand()
                        {
                            CommandType = CommandType.Text,
                            CommandText = "select top 1  L.Name from Display D join Location L on d.LocationId = L.LocationId where DisplayId = @displayId",

                        })
                        {
                            dispcmd.Parameters.AddWithValue("@displayId", DisplayId);
                            dispcmd.ExecuteReaderExt((dispdr) =>
                            {
                                LevelName = dispdr.StringOrBlank("Name");
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

                            outstring = "<div id=\"omsattning2\" style=\"margin-top -10px; margin-left: 10px;\">  " +
                            "<table style=\"border-collapse: collapse; height: 90px; width: 1800px;\" border=\"0\">  " +
                            "<tbody>  " +
                            "<tr style=\"height: 32px;\"> " +
                            "<td style=\"height: 32px;\" colspan=\"4\">" +
                            "<span style=\"font-family: CoopNew; font-size: 26pt; color: #3a8d49;\">BUDGET</span>" +
                            "</td>  " +
                            "<td style=\"height: 32px;\" colspan=\"9\">" +
                            "<span style=\"font-family: CoopNew; font-size: 26pt; color: #3a8d49;\">F&Ouml;REG&Aring;ENDE &Aring;R</span>" +
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
                        await response.Output.WriteAsync(html.Content);
                        await response.Output.FlushAsync();
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