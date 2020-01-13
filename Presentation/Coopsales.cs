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
using System.Runtime.Serialization;

namespace DisplayMonkey
{
    public class CoopSales : Frame
	{
        public string Subject { get; private set; }
        public string Body { get; private set; }
        public string DateCreated2 { get; private set; }

        public CoopSales(Frame frame)
            : base(frame)
        {
            _init();
        }

        private void _init()
        {
            Location L = new Location(DisplayId);
            string butiksnr = L.Name.Split('-').First().Trim();
            using (SqlCommand cmd = new SqlCommand()
            {
                CommandType = CommandType.Text,
                CommandText = "SELECT TOP(1) c.* " 
                + " FROM display d " 
                + " JOIN location l ON d.LocationId = l.LocationId "
                + " JOIN coopsales c ON (SELECT TOP(1) Value FROM STRING_SPLIT(l.Name, '-')) = c.Butik "
                + " WHERE c.Butik = " + butiksnr
            })
            {
                cmd.Parameters.AddWithValue("@frameId", FrameId);
                cmd.ExecuteReaderExt((dr) =>
                {
                    Subject = dr.StringOrBlank("Subject");
                    Body = dr.StringOrBlank("Body");
                    DateCreated2 = dr.DateTimeOrBlank("BeginsOn").ToString("yyyy-MM-dd"); //MM 2019-11-21
                    return false;
                });
            }
        }
	}
}
