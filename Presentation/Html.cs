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
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Script.Serialization;

namespace DisplayMonkey
{
    public class Html : Frame
	{
        [ScriptIgnore]
        public string Content { get; private set; }

        public string DateCreated2 { get; private set; }

        public Html(int frameId)
            : base(frameId)
        {
            _init();
        }
        
        public Html(Frame frame)
            : base(frame)
		{
            _init();
        }

        private void _init()
        {
            string LevelName = "";
            string Datum = "", Butik = "", Koncept_dag = "", Butik_dag = "", Marginalkronor = "", Snittkop = "", Antal_kunder = "", Fys = "";

            using (SqlCommand cmd = new SqlCommand()
            {
                CommandType = CommandType.Text,
                //CommandText = "SELECT TOP 1 * FROM Html WHERE FrameId=@frameId",
                CommandText = "SELECT TOP 1 * From Html JOIN Frame ON Html.FrameId = Frame.FrameId WHERE Html.FrameId=@frameId", //MM 2019-11-21
            })
            {
                cmd.Parameters.AddWithValue("@frameId", this.FrameId);
                cmd.ExecuteReaderExt((dr) =>
                {

                    Content = dr.StringOrBlank("Content");
                    DateCreated2 = dr.DateTimeOrBlank("BeginsOn").ToString("yyyy-MM-dd"); //MM 2019-11-21
                    return false;
                });
            }

            if (Content.Contains("<div id=\"omsattning\"") && this.DisplayId != 0)
            {
                try
                {
                    using (SqlCommand dispcmd = new SqlCommand()
                    {
                        CommandType = CommandType.Text,
                        CommandText = "select top 1  L.Name from Display D join Location L on d.LocationId = L.LocationId where DisplayId = @displayId",

                    })
                    {
                        dispcmd.Parameters.AddWithValue("@displayId", this.DisplayId);
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
                        if (Convert.ToDecimal(Fys) < 100)
                        {
                            outstring += "<em class=\"material-icons\" style=\"font-size: 85px; color: #e74c3c;\">trending_down</em>";
                        }
                        else if (Convert.ToDecimal(Fys) > 100)
                        {
                            outstring += "<em class=\"material-icons\" style=\"font-size: 85px; color: #28b463;\">trending_up</em>";
                        }
                        else
                        {
                            outstring += "<em class=\"material-icons\" style=\"font-size: 85px; color: #f1c40f;\">trending_flat</em>";
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
                            Content = outstring;
                        }
                        else
                        {
                            Content = Content;
                        }
                    }
                    catch (Exception Ex)
                    {
                        Console.WriteLine(Ex);
                        throw;
                    }
                }
            }
        }
    }
}
