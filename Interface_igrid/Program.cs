﻿//using BLL.Services;
//using CsvHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections;

namespace Interface_igrid
{
    public class Program
    {
        public static string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        public static string InterfacePathInbound = ConfigurationManager.AppSettings["InterfacePathInbound"];
        public static string InterfacePathOutbound = ConfigurationManager.AppSettings["InterfacePathOutbound"];
        public static string runFlageOutbound = ConfigurationManager.AppSettings["runFlageOutbound"];
        public static string runFlageInbound = ConfigurationManager.AppSettings["runFlageInbound"];

        static void Main(string[] args)
        {
            #region Outbound
            if (bool.Parse(runFlageOutbound) == true) // flage for true run or false not run
            {
                try
                {
                    //DataSet dsspGetMasterData = GetData("spGetMasterData", "@Active", "X");
                    //SQ01_ListMAT(dsspGetMasterData.Tables[0]); 
                    //CT04(dsspGetMasterData.Tables[0]); //Insert,Remove 

                    //DataSet dsspQuery = GetData("spQuery", "@Material", "X");
                    //MM01_CreateMAT_ExtensionPlant(dsspQuery.Tables[0]);
                    //BAPI_UpdateMATCharacteristics(dsspQuery.Tables[0]);

                    DataSet dsspGetImpactmat = GetData("spGetImpactmat", "@Active", "X");
                    CLMM_ChangeMatClass(dsspGetImpactmat.Tables[0]);
                    //MM02_ImpactMatDesc(dsspGetImpactmat.Tables[0]);

                    Console.WriteLine("Outbound Completed");
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine($"Message :{e.Message} ");
                    Console.WriteLine("Not success");
                }
            }

            #endregion

            #region Inbound        
            if (bool.Parse(runFlageInbound) == true) // flage for true run or false not run
            {
                try
                {
                    string imported = "";
                    string bodyMsg = "";
                    FileInfo fileI = null;
                    string fileN = "";
                    var filesToImport = Directory.GetFiles(InterfacePathInbound, "*_Result.csv");
                    if (filesToImport != null)
                    {
                        foreach (string file in filesToImport)
                        {
                            fileI = new FileInfo(file);
                            fileN = fileI.Name;
                            switch (fileN.Substring(0, 6))
                            {
                                case "SQ01_L":
                                    imported = Import_SQ01(file);
                                    break;
                                case "CT04_I":
                                    //imported = Import_CT04_I(fileI.FullName, out bodyMsg);
                                    break;
                                case "CT04_R":
                                    //imported = Import_CT04_R(fileI.FullName, out bodyMsg);
                                    break;
                                case "MM01_C":
                                    //imported = Import_MM01_C(fileI.FullName, out bodyMsg);
                                    break;
                                case "BAPI_U":
                                    //imported = Import_BAPI_U(fileI.FullName, out bodyMsg);
                                    break;
                                case "CLMM_C":
                                    //imported = Import_CLMM_C(fileI.FullName, out bodyMsg);
                                    break;
                                case "MM02":
                                    //imported = Import_CMM02(fileI.FullName, out bodyMsg);
                                    break;
                            }
                            Console.WriteLine(imported);
                        }
                    }
                    Console.WriteLine("Inbound Completed");
                }
                catch (Exception e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine($"Message :{e.Message} ");
                    //ErrorLogger.LOGGER.Error(ex.Message, ex);
                    //ITF_Data.SendNotification("SAP Import Service - Error Executing Interface", ex.Message + "<br />" + ex.StackTrace, "");
                }
            }
            #endregion
        }

        #region IMPORT INTERFACES
        public static string Import_SQ01(string file)
        {
            try
            {
                using (var sr = new StreamReader(file))
                {
                    string[] headers = sr.ReadLine().Split(',');
                    foreach (string header in headers)
                    {
                        Console.WriteLine(header);


                        if (header.Length > 0)
                        {
                            if (header == "Condition records saved")
                            {
                                string Changed_Id = "";
                                string Changed_Action = "";
                                string Material = "";
                                string Description = "";
                                string DMSNo = "";
                                string New_Material = "";
                                string New_Description = "";
                                string Status = "";
                                string Reason = "";
                                string NewMat_JobId = "";
                                string Char_Name = "";
                                string Char_OldValue = "";
                                string Char_NewValue = "";

                                //send email
                                //SendEmailUpdateMaster("U" + Material);

                                //update impactedmat
                                using (SqlConnection con = new SqlConnection(ConnectionString))
                                {
                                    SqlCommand cmd = new SqlCommand();
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.CommandText = "spUpdateImpactedmat";
                                    cmd.Parameters.AddWithValue("@Changed_Id", Changed_Id);
                                    cmd.Parameters.AddWithValue("@Changed_Action", Changed_Action);
                                    cmd.Parameters.AddWithValue("@Material", Material);
                                    cmd.Parameters.AddWithValue("@Description", Description);
                                    cmd.Parameters.AddWithValue("@DMSNo", DMSNo);
                                    cmd.Parameters.AddWithValue("@New_Material", New_Material);
                                    cmd.Parameters.AddWithValue("@New_Description", New_Description);
                                    cmd.Parameters.AddWithValue("@Status", Status);
                                    cmd.Parameters.AddWithValue("@Reason", Reason);
                                    cmd.Parameters.AddWithValue("@NewMat_JobId", NewMat_JobId);
                                    cmd.Parameters.AddWithValue("@Char_Name", Char_Name);
                                    cmd.Parameters.AddWithValue("@Char_OldValue", Char_OldValue);
                                    cmd.Parameters.AddWithValue("@Char_NewValue", Char_NewValue);
                                    cmd.Connection = con;
                                    con.Open();
                                    DataTable dtResult = new DataTable();
                                    SqlDataAdapter oAdapter = new SqlDataAdapter(cmd);
                                    oAdapter.Fill(dtResult);
                                    con.Close();
                                }
                            }
                        }
                    }

                    while (!sr.EndOfStream)
                    {
                        string[] rows = sr.ReadLine().Split(',');
                        foreach (string row in rows)
                        {
                            Console.WriteLine(row);
                        }

                       
                        foreach (string item in rows)
                        {

                            string a = item[0].ToString();
                        }
                        foreach (dynamic record in rows.ToList())
                        {
                            var data = record["IfColumn"];
                        }
                    }                   
                }

                File.Move(file, InterfacePathInbound + @"Processed\" + Path.GetFileName(file));
                return "Success";
            }
            catch (IOException e)
            {
                return e.Message;
            }
        }
        #endregion

        #region EXPORT INTERFACES
        public static void SQ01_ListMAT(DataTable Results)
        {         
            DataTable dtListMat = new DataTable();
            dtListMat.Columns.AddRange(new DataColumn[] 
            { 
                new DataColumn (@"Characteristic Name txtSP$00005-LOW.text"),
                new DataColumn(@"txtSP$00003 - LOW.text"),
            });
            foreach (DataRow row in Results.Rows)
            {                    
                dtListMat.Rows.Add(
                    string.Format("{0}", row["Changed_Charname"].ToString()),
                    string.Format("{0}", row["Old_Description"].ToString()));
            }
            if (dtListMat.Rows.Count > 0)
            {
                string file = InterfacePathOutbound + "SQ01_ListMat" + "_" + DateTime.Now.ToString("yyyyMMddhhmm") + ".csv";
                ToCSV(dtListMat, file);
            }                 
        }
        public static void CT04(DataTable Results)
        {
            //Cancel CT04Update
            DataTable dtCT04Insert = new DataTable();
            //DataTable dtCT04Update = new DataTable();
            DataTable dtCT04Remove = new DataTable();
            dtCT04Insert.Columns.AddRange(new DataColumn[] { new DataColumn (@"IfColumn"),
                new DataColumn(@"Characteristic Name RCTAV-ATNAM"),
                new DataColumn(@"Characteristic Value CAWN-ATWRT(01)"),
                new DataColumn(@"Characteristic value description CAWNT-ATWTB(01)"),
            });
            //dtCT04Update.Columns.AddRange(new DataColumn[] { new DataColumn (@"IfColumn"),
            //    new DataColumn(@"Characteristic Name RCTAV-ATNAM"),
            //    new DataColumn(@"Characteristic Value CAWN-ATWRT(01)"),
            //    new DataColumn(@"Text for a table entry CLHP-CR_STATUS_TEXT"),
            //    new DataColumn(@"Characteristic value description CAWNT-ATWTB(01)"),
            //});
            dtCT04Remove.Columns.AddRange(new DataColumn[] { new DataColumn (@"IfColumn"),
                new DataColumn(@"Characteristic Name RCTAV-ATNAM"),
                new DataColumn(@"Characteristic Value CAWN-ATWRT(01)"),
                new DataColumn(@"Text for a table entry CLHP-CR_STATUS_TEXT"),
            });
            foreach (DataRow row in Results.Rows)
            {
                switch (row["Changed_Action"].ToString())
                {
                    case "Insert":
                        dtCT04Insert.Rows.Add(string.Format("{0}", row["Changed_Action"].ToString().Replace("Insert", "I")),
                            string.Format("{0}", row["Changed_Charname"].ToString()),
                            string.Format("{0}", row["id"].ToString()),
                            string.Format("{0}", row["Description"].ToString()));
                        break;
                    //case "Update":
                    //    dtCT04Update.Rows.Add(string.Format("{0}", row["Changed_Action"].ToString()),
                    //       string.Format("{0}", row["Changed_Charname"].ToString()),
                    //       string.Format("{0}", row["id"].ToString()),
                    //       string.Format("{0}", row["Old_Description"].ToString()),
                    //       string.Format("{0}", row["Description"].ToString()));
                    //    break;
                    case "Remove":
                        dtCT04Remove.Rows.Add(string.Format("{0}", row["Changed_Action"].ToString().Replace("Remove", "D")),
                           string.Format("{0}", row["Changed_Charname"].ToString()),
                           string.Format("{0}", row["Description"].ToString()));
                        break;
                    default:
                        break;
                }
            }
            if (dtCT04Insert.Rows.Count > 0)
            {
                string file = InterfacePathOutbound + "CT04_Insert_" + DateTime.Now.ToString("yyyyMMddhhmm") + ".csv";
                ToCSV(dtCT04Insert, file);
            }
            //if (dtCT04Update.Rows.Count > 0)
            //{
            //    string file = @"D:\SAPInterfaces\Outbound\CT04_Update_" + DateTime.Now.ToString("yyyyMMddhhmm") + ".csv";
            //    ToCSV(dtCT04Update, file);
            //}
            if (dtCT04Remove.Rows.Count > 0)
            {
                string file = InterfacePathOutbound + "CT04_Remove_" + DateTime.Now.ToString("yyyyMMddhhmm") + ".csv";
                ToCSV(dtCT04Remove, file);
            }
        }
        public static void MM01_CreateMAT_ExtensionPlant(DataTable Results)
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[] 
            { 
                new DataColumn (@"IfColumn"),
                new DataColumn(@"Material Number RMMG1-MATNR"),
                new DataColumn(@"Material Description (Short Text) MAKT-MAKTX"),
                new DataColumn(@"Reference material RMMG1_REF-MATNR"),
                new DataColumn(@"IfColumn and Plant RMMG1-WERKS and Reference plant RMMG1_REF-WERKS"),
                new DataColumn(@"IfColumn and Sales Organization RMMG1-VKORG and Reference sales organization RMMG1_REF-VKORG"),
                new DataColumn(@"Distribution Channel RMMG1-VTWEG and Reference distribution channel RMMG1_REF-VTWEG"),
            });
            foreach (DataRow row in Results.Rows)
            {
                string[] split = string.Format("{0}", row["Plant"].ToString()).Split(new Char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in split)
                {
                    if (s.Trim() != "")
                    {
                        string[] splitSOOrg = "DM;EX".Split(new Char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string o in splitSOOrg)
                        {
                            dt.Rows.Add(string.Format("{0}", row["DocumentNo"].ToString()),
                            string.Format("{0}", row["Material"].ToString()),
                            string.Format("{0}", row["Description"].ToString()),
                            string.Format("{0}", row["Ref"].ToString().Trim()),
                            string.Format("{0}", s.ToString().Trim()),
                            string.Format("{0}", row["Plant"]).Substring(0, 3),
                            string.Format("{0}", o)
                            );
                        }
                    }
                }
            }
            if (dt.Rows.Count > 0)
            {
                string file = InterfacePathOutbound + "MM01_CreateMAT_ExtensionPlant_" + DateTime.Now.ToString("yyyyMMddhhmm") + ".csv";
                ToCSV(dt, file);
            }
        }
        public static void BAPI_UpdateMATCharacteristics(DataTable Results)
        {
            DataTable dtClass = new DataTable();
            dtClass.Columns.AddRange(new DataColumn[] { new DataColumn(@"Material Number RMMG1-MATNR"),
                new DataColumn(@"CLASSNUM"),
                new DataColumn(@"Loop Id Column"),
                new DataColumn(@"Characteristic Name ALLOCVALUESCHARNEW-CHARACT"),
                new DataColumn(@"Characteristic Value ALLOCVALUESCHARNEW-VALUE_CHAR")
            });
            int i = 1;
            foreach (DataRow row in Results.Rows)
            {               
                dtClass.Rows.Add(string.Format("{0}", row["Material"].ToString()),
                string.Format("{0}", row["ClassType"].ToString()),
                string.Format("{0}", "H"),
                string.Format("{0}", ""),
                string.Format("{0}", "")
                );
                DataTable dtCharacteristic = builditems(@"select * from MasCharacteristic where MaterialType  like '%" + row["Material"].ToString().Substring(1, 1) + "%' order by Id");
                foreach (DataRow dr in dtCharacteristic.Rows)
                {
                    string value = string.Format("{0}", dr["shortname"]);
                    if (dr["Single_Value"].ToString() == "X")
                    {
                        dtClass.Rows.Add(
                        string.Format("{0}", ""),
                        string.Format("{0}", ""),
                        string.Format("{0}", "D"),
                        string.Format("{0}", dr["Title"]),
                        string.Format("{0}", row[value])
                        );
                    }                   
                    else
                    {
                        string[] splitPlant = string.Format("{0}", row[value].ToString()).Split(new Char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string pl in splitPlant)
                        {
                            dtClass.Rows.Add(
                                string.Format("{0}", ""),
                                string.Format("{0}", ""),
                            string.Format("{0}", "D"),
                            string.Format("{0}", dr["Title"]),
                            string.Format("{0}", pl)
                            );
                        }
                    }
                }
                if (dtClass.Rows.Count > 0)
                {
                    string file = InterfacePathOutbound + "BAPI_UpdateMATCharacteristics_" + DateTime.Now.ToString("yyyyMMddhhmm") + "_" + i  + ".csv";
                    ToCSV(dtClass, file);
                }
                dtClass.Clear();
                i++;
            }
        }
        public static void CLMM_ChangeMatClass(DataTable Results)
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[]
            {                       
                new DataColumn(@"Table cell -TextField txtG_TARGET_TAB - OBJECT"),
                new DataColumn (@"Char.Name ctxtG_CHAR_TAB - ATNAM"),
                new DataColumn(@"Loop Id Column"),
                new DataColumn(@"Characteristic Name ALLOCVALUESCHARNEW-CHARACT"),
                new DataColumn(@"New Value ctxtG_CHAR_TAB - NEWATWRT"),
             });
            int i = 1;
            foreach (DataRow row in Results.Rows)
            {
                dt.Rows.Add(string.Format("{0}", row["Material"].ToString()),
                string.Format("{0}", row["ClassType"].ToString()),
                string.Format("{0}", "H"),
                string.Format("{0}", ""),
                string.Format("{0}", "")
                );
                DataTable dtCharacteristic = builditems(@"select * from MasCharacteristic where MaterialType  like '%" + row["Material"].ToString().Substring(1, 1) + "%' order by Id");
                foreach (DataRow dr in dtCharacteristic.Rows)
                {
                    string value = string.Format("{0}", dr["shortname"]);
                    //string value = (dr["Title"].ToString() == row["Char_Name"].ToString()) ? string.Format("{0}", row["Char_NewValue"]) : string.Format("{0}", dr["shortname"]);

                    if (dr["Single_Value"].ToString() == "X")
                    {
                        //multi                        
                        string[] splitPlant = string.Format("{0}", row[value].ToString()).Split(new Char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string pl in splitPlant)
                        {
                            dt.Rows.Add(
                                string.Format("{0}", ""),
                                string.Format("{0}", ""),
                            string.Format("{0}", "D"),
                            string.Format("{0}", dr["Title"]),
                            string.Format("{0}", pl)
                            );
                        }
                    }
                    else
                    {
                        //single
                        dt.Rows.Add(
                        string.Format("{0}", ""),
                        string.Format("{0}", ""),
                        string.Format("{0}", "D"),
                        string.Format("{0}", dr["Title"]),
                        string.Format("{0}", row[value])
                        );
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    string file = InterfacePathOutbound + "CLMM_ChangeMatClass" + DateTime.Now.ToString("yyyyMMddhhmm") + "_" + i + ".csv";                  
                    ToCSV(dt, file);
                }
                dt.Clear();
                i++;
            }
        }
        public static void MM02_ImpactMatDesc(DataTable Results)
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[] { new DataColumn (@"Material Number RMMG1 - MATNR"),
            new DataColumn(@"Material description MAKT - MAKTX"),});
            foreach (DataRow row in Results.Rows)
            {
                dt.Rows.Add(
                string.Format("{0}", row["Material"].ToString()),
                string.Format("{0}", row["Description"].ToString()));
            }
            if (dt.Rows.Count > 0)
            {
                string file = InterfacePathOutbound + "MM02_ImpactMatDesc" + "_" + DateTime.Now.ToString("yyyyMMddhhmm") + ".csv";
                ToCSV(dt, file);
            }
        }
        #endregion

        #region Util METHODS
        public static DataSet GetData(string sp, string field, string sName)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = sp; 
                cmd.Parameters.AddWithValue(field, sName);
                cmd.Connection = con;
                con.Open();
                DataSet oDataset = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(oDataset);
                con.Close();
                return oDataset;
            }
        }
        public static DataTable builditems(string data)
        {
            using (SqlConnection oConn = new SqlConnection(ConnectionString))
            {
                oConn.Open();
                string strQuery = data;
                DataTable dt = new DataTable();
                SqlDataAdapter oAdapter = new SqlDataAdapter(strQuery, oConn);
                oAdapter.Fill(dt);
                oConn.Close();
                oConn.Dispose();
                return dt;
            }
        }
        public static void ToCSV(DataTable dtDataTable, string strFilePath)
        {
            StreamWriter sw = new StreamWriter(strFilePath, false, new UTF8Encoding(true));
            //headers
            for (int i = 0; i < dtDataTable.Columns.Count; i++)
            {
                sw.Write(dtDataTable.Columns[i]);
                if (i < dtDataTable.Columns.Count - 1)
                {
                    sw.Write(",");
                }
            }
            sw.Write(sw.NewLine);
            foreach (DataRow dr in dtDataTable.Rows)
            {
                for (int i = 0; i < dtDataTable.Columns.Count; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        string value = dr[i].ToString();
                        if (value.Contains(','))
                        {
                            value = String.Format("\"{0}\"", value);
                            sw.Write(value);
                        }
                        else
                        {
                            sw.Write(dr[i].ToString());
                        }
                    }
                    if (i < dtDataTable.Columns.Count - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();
        }
        #endregion

        #region Temp METHODS
        public static void CT04_Inbound(string data)
        {
            var dir = @"D:\SAPInterfaces\Inbound";
            var filePaths = Directory.GetFiles(dir, "CT04*_Result*.csv");
            //foreach (string s in filePaths)
            //{
            //    using (var reader = new StreamReader(s))
            //    {
            //        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            //        {
            //            while (csv.Read())
            //            {//This will advance the reader to the next record.

            //                //You can use an indexer to get by position or name. 
            //                //This will return the field as a string

            //                // By position
            //                var field = csv[0];
            //                //var AppID = csv["AppID"];
            //                // By header name
            //                //csv.Read();
            //                csv.ReadHeader();
            //                string[] headerRow = csv.Context.Reader.HeaderRecord;
            //                string[] filteredValues = Array.FindAll(headerRow, x => x.Contains("Result"));
            //                var Condition = "";// csv["Condition TypeRV13A-KSCHL"];
            //                if (filteredValues.Length > 0)
            //                    if (csv["Result"] == "Condition records saved")
            //                    {
            //                        string Changed_Id = "";
            //                        string Changed_Action = "";
            //                        string Material = "";
            //                        string Description = "";
            //                        string DMSNo = "";
            //                        string New_Material = "";
            //                        string New_Description = "";
            //                        string Status = "";
            //                        string Reason = "";
            //                        string NewMat_JobId = "";
            //                        string Char_Name = "";
            //                        string Char_OldValue = "";
            //                        string Char_NewValue = "";
            //                        //SendEmailUpdateMaster("U" + Material);
            //                        using (SqlConnection con = new SqlConnection(strConn))
            //                        {
            //                            SqlCommand cmd = new SqlCommand();
            //                            cmd.CommandType = CommandType.StoredProcedure;
            //                            cmd.CommandText = "spUpdateImpactedmat";

            //                            cmd.Parameters.AddWithValue("@Changed_Id", Changed_Id);
            //                            cmd.Parameters.AddWithValue("@Changed_Action", Changed_Action);
            //                            cmd.Parameters.AddWithValue("@Material", Material);
            //                            cmd.Parameters.AddWithValue("@Description", Description);
            //                            cmd.Parameters.AddWithValue("@DMSNo", DMSNo);
            //                            cmd.Parameters.AddWithValue("@New_Material", New_Material);
            //                            cmd.Parameters.AddWithValue("@New_Description", New_Description);
            //                            cmd.Parameters.AddWithValue("@Status", Status);
            //                            cmd.Parameters.AddWithValue("@Reason", Reason);
            //                            cmd.Parameters.AddWithValue("@NewMat_JobId", NewMat_JobId);
            //                            cmd.Parameters.AddWithValue("@Char_Name", Char_Name);
            //                            cmd.Parameters.AddWithValue("@Char_OldValue", Char_OldValue);
            //                            cmd.Parameters.AddWithValue("@Char_NewValue", Char_NewValue);


            //                            cmd.Connection = con;
            //                            con.Open();
            //                            DataTable dtResult = new DataTable();
            //                            SqlDataAdapter oAdapter = new SqlDataAdapter(cmd);
            //                            oAdapter.Fill(dtResult);
            //                            con.Close();
            //                        }
            //                        //    using (SqlConnection con = new SqlConnection(strConn))
            //                        //    {
            //                        //        SqlCommand cmd = new SqlCommand();
            //                        //        cmd.CommandType = CommandType.StoredProcedure;
            //                        //        cmd.CommandText = "spUpdateQuotationfromJob";
            //                        //        cmd.Parameters.AddWithValue("@MinPrice", string.Format("{0}", 0));
            //                        //        cmd.Parameters.AddWithValue("@subID", string.Format("{0}", Condition.ToString() == "zpm1" ? "0" : AppID.ToString()));
            //                        //        cmd.Parameters.AddWithValue("@ID", AppID.ToString());
            //                        //        cmd.Connection = con;
            //                        //        con.Open();
            //                        //        cmd.ExecuteNonQuery();
            //                        //        con.Close();
            //                        //    }
            //                        //}
            //                    }
            //                //var records = csv.GetRecords<dynamic>();
            //                //foreach (var item in records)
            //                //{
            //                //    var da = item.Result;
            //                //    var condition = item[@"Condition Type RV13A-KSCHL"];  
            //                //}
            //                //foreach (dynamic record in records.ToList())
            //                //{
            //                //    var data = record["IfColumn"];
            //                //}
            //            }
            //        }
            //    }
            //    try
            //    {
            //        //File.Move(s, @"D:\SAPInterfaces\Inbound\Processed\" + Path.GetFileName(s));
            //        //HttpContext.Current.Server.MapPath("~/ExcelFiles/Processed/" + Path.GetFileName(s)));
            //        // Copy the file and overwrite if it exists
            //        File.Copy(s, @"D:\SAPInterfaces\Inbound\Processed\" + Path.GetFileName(s), true);

            //        // Delete the source file
            //        File.Delete(s);
            //    }
            //    catch (IOException iox)
            //    {
            //        Console.WriteLine(iox.Message);
            //    }
            //}
            // Move MM01 & BAPI_UpdateMATCharacteristics
            filePaths = Directory.GetFiles(dir, "MM01*_Result*.csv");
            foreach (string s in filePaths)
            {
                try
                {
                    using (var reader = new StreamReader(s))
                    {
                        //using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                        //{
                        //    while (csv.Read())
                        //    {//This will advance the reader to the next record.

                        //        //You can use an indexer to get by position or name. 
                        //        //This will return the field as a string

                        //        // By position
                        //        var field = csv[0];
                        //        //var AppID = csv["AppID"];
                        //        // By header name
                        //        //csv.Read();
                        //        csv.ReadHeader();
                        //        string[] headerRow = csv.Context.Reader.HeaderRecord;
                        //        string[] filteredValues = Array.FindAll(headerRow, x => x.Contains("Result"));
                        //        var Condition = "";// csv["Condition TypeRV13A-KSCHL"];
                        //        if (filteredValues.Length > 0)
                        //            if (csv["Result"] == "Condition records saved")
                        //            {
                        //            }
                        //    }
                        //}
                    }
                    //File.Move(s, @"D:\SAPInterfaces\Inbound\Processed\" + Path.GetFileName(s));
                    //HttpContext.Current.Server.MapPath("~/ExcelFiles/Processed/" + Path.GetFileName(s)));
                    // Copy the file and overwrite if it exists
                    File.Copy(s, @"D:\SAPInterfaces\Inbound\Processed\" + Path.GetFileName(s), true);

                    // Delete the source file
                    File.Delete(s);
                }
                catch (IOException iox)
                {
                    Console.WriteLine(iox.Message);
                }
            }
            // Move BAPI_UpdateMATCharacteristics
            filePaths = Directory.GetFiles(dir, "BAPI*_Result*.csv");
            foreach (string s in filePaths)
            {
                try
                {
                    //File.Move(s, @"D:\SAPInterfaces\Inbound\Processed\" + Path.GetFileName(s));
                    //HttpContext.Current.Server.MapPath("~/ExcelFiles/Processed/" + Path.GetFileName(s)));
                    // Copy the file and overwrite if it exists
                    File.Copy(s, @"D:\SAPInterfaces\Inbound\Processed\" + Path.GetFileName(s), true);

                    // Delete the source file
                    File.Delete(s);
                }
                catch (IOException iox)
                {
                    Console.WriteLine(iox.Message);
                }
            }
        }
        public static void Inbound(string data)
        {
            var dir = @"D:\SAPInterfaces\Inbound";
            var filePaths = Directory.GetFiles(dir, "CT04*_Result*.csv");
            //foreach (string s in filePaths)
            //{
            //    using (var reader = new StreamReader(s))
            //    {
            //        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            //        {
            //            while (csv.Read())
            //            {//This will advance the reader to the next record.

            //                //You can use an indexer to get by position or name. 
            //                //This will return the field as a string

            //                // By position
            //                var field = csv[0];
            //                //var AppID = csv["AppID"];
            //                // By header name
            //                //csv.Read();
            //                csv.ReadHeader();
            //                string[] headerRow = csv.Context.Reader.HeaderRecord;
            //                string[] filteredValues = Array.FindAll(headerRow, x => x.Contains("Result"));
            //                var Condition = "";// csv["Condition TypeRV13A-KSCHL"];
            //                if (filteredValues.Length > 0)
            //                    if (csv["Result"] == "Condition records saved")
            //                    {
            //                        string Changed_Id = "";
            //                        string Changed_Action = "";
            //                        string Material = "";
            //                        string Description = "";
            //                        string DMSNo = "";
            //                        string New_Material = "";
            //                        string New_Description = "";
            //                        string Status = "";
            //                        string Reason = "";
            //                        string NewMat_JobId = "";
            //                        string Char_Name = "";
            //                        string Char_OldValue = "";
            //                        string Char_NewValue = "";
            //                        //SendEmailUpdateMaster("U" + Material);
            //                        using (SqlConnection con = new SqlConnection(strConn))
            //                        {
            //                            SqlCommand cmd = new SqlCommand();
            //                            cmd.CommandType = CommandType.StoredProcedure;
            //                            cmd.CommandText = "spUpdateImpactedmat";

            //                            cmd.Parameters.AddWithValue("@Changed_Id", Changed_Id);
            //                            cmd.Parameters.AddWithValue("@Changed_Action", Changed_Action);
            //                            cmd.Parameters.AddWithValue("@Material", Material);
            //                            cmd.Parameters.AddWithValue("@Description", Description);
            //                            cmd.Parameters.AddWithValue("@DMSNo", DMSNo);
            //                            cmd.Parameters.AddWithValue("@New_Material", New_Material);
            //                            cmd.Parameters.AddWithValue("@New_Description", New_Description);
            //                            cmd.Parameters.AddWithValue("@Status", Status);
            //                            cmd.Parameters.AddWithValue("@Reason", Reason);
            //                            cmd.Parameters.AddWithValue("@NewMat_JobId", NewMat_JobId);
            //                            cmd.Parameters.AddWithValue("@Char_Name", Char_Name);
            //                            cmd.Parameters.AddWithValue("@Char_OldValue", Char_OldValue);
            //                            cmd.Parameters.AddWithValue("@Char_NewValue", Char_NewValue);


            //                            cmd.Connection = con;
            //                            con.Open();
            //                            DataTable dtResult = new DataTable();
            //                            SqlDataAdapter oAdapter = new SqlDataAdapter(cmd);
            //                            oAdapter.Fill(dtResult);
            //                            con.Close();
            //                        }
            //                        //    using (SqlConnection con = new SqlConnection(strConn))
            //                        //    {
            //                        //        SqlCommand cmd = new SqlCommand();
            //                        //        cmd.CommandType = CommandType.StoredProcedure;
            //                        //        cmd.CommandText = "spUpdateQuotationfromJob";
            //                        //        cmd.Parameters.AddWithValue("@MinPrice", string.Format("{0}", 0));
            //                        //        cmd.Parameters.AddWithValue("@subID", string.Format("{0}", Condition.ToString() == "zpm1" ? "0" : AppID.ToString()));
            //                        //        cmd.Parameters.AddWithValue("@ID", AppID.ToString());
            //                        //        cmd.Connection = con;
            //                        //        con.Open();
            //                        //        cmd.ExecuteNonQuery();
            //                        //        con.Close();
            //                        //    }
            //                        //}
            //                    }
            //                //var records = csv.GetRecords<dynamic>();
            //                //foreach (var item in records)
            //                //{
            //                //    var da = item.Result;
            //                //    var condition = item[@"Condition Type RV13A-KSCHL"];  
            //                //}
            //                //foreach (dynamic record in records.ToList())
            //                //{
            //                //    var data = record["IfColumn"];
            //                //}
            //            }
            //        }
            //    }
            //    try
            //    {
            //        //File.Move(s, @"D:\SAPInterfaces\Inbound\Processed\" + Path.GetFileName(s));
            //        //HttpContext.Current.Server.MapPath("~/ExcelFiles/Processed/" + Path.GetFileName(s)));
            //        // Copy the file and overwrite if it exists
            //        File.Copy(s, @"D:\SAPInterfaces\Inbound\Processed\" + Path.GetFileName(s), true);

            //        // Delete the source file
            //        File.Delete(s);
            //    }
            //    catch (IOException iox)
            //    {
            //        Console.WriteLine(iox.Message);
            //    }
            //}
            // Move MM01 & BAPI_UpdateMATCharacteristics
            filePaths = Directory.GetFiles(dir, "MM01*_Result*.csv");
            //foreach (string s in filePaths)
            //{
            //    try
            //    {
            //        using (var reader = new StreamReader(s))
            //        {
            //            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            //            {
            //                while (csv.Read())
            //                {//This will advance the reader to the next record.

            //                    //You can use an indexer to get by position or name. 
            //                    //This will return the field as a string

            //                    // By position
            //                    var field = csv[0];
            //                    //var AppID = csv["AppID"];
            //                    // By header name
            //                    //csv.Read();
            //                    csv.ReadHeader();
            //                    string[] headerRow = csv.Context.Reader.HeaderRecord;
            //                    string[] filteredValues = Array.FindAll(headerRow, x => x.Contains("Result"));
            //                    var Condition = "";// csv["Condition TypeRV13A-KSCHL"];
            //                    if (filteredValues.Length > 0)
            //                        if (csv["Result"] == "Condition records saved")
            //                        {
            //                        }
            //                }
            //            }
            //        }
            //        //File.Move(s, @"D:\SAPInterfaces\Inbound\Processed\" + Path.GetFileName(s));
            //        //HttpContext.Current.Server.MapPath("~/ExcelFiles/Processed/" + Path.GetFileName(s)));
            //        // Copy the file and overwrite if it exists
            //        File.Copy(s, @"D:\SAPInterfaces\Inbound\Processed\" + Path.GetFileName(s), true);

            //        // Delete the source file
            //        File.Delete(s);
            //    }
            //    catch (IOException iox)
            //    {
            //        Console.WriteLine(iox.Message);
            //    }
            //}
            // Move BAPI_UpdateMATCharacteristics
            filePaths = Directory.GetFiles(dir, "BAPI*_Result*.csv");
            foreach (string s in filePaths)
            {
                try
                {
                    //File.Move(s, @"D:\SAPInterfaces\Inbound\Processed\" + Path.GetFileName(s));
                    //HttpContext.Current.Server.MapPath("~/ExcelFiles/Processed/" + Path.GetFileName(s)));
                    // Copy the file and overwrite if it exists
                    File.Copy(s, @"D:\SAPInterfaces\Inbound\Processed\" + Path.GetFileName(s), true);

                    // Delete the source file
                    File.Delete(s);
                }
                catch (IOException iox)
                {
                    Console.WriteLine(iox.Message);
                }
            }
        }
        public static void CLMM_ChangeMatClassold(DataTable Results)
        {

            //DataTable dt = new DataTable();
            //dt.Columns.AddRange(new DataColumn[] 
            //{ 
            //    new DataColumn (@"Char.Name ctxtG_CHAR_TAB - ATNAM"),
            //    new DataColumn(@"Old Value ctxtG_CHAR_TAB - OLDATWRT"),
            //    new DataColumn(@"New Value ctxtG_CHAR_TAB - NEWATWRT"),
            //    new DataColumn(@"Table cell -TextField txtG_TARGET_TAB - OBJECT"),
            // });
            //foreach (DataRow row in Results.Rows)
            //{
            //    dt.Rows.Add(
            //        string.Format("{0}", row["Char_Name"].ToString()),
            //        string.Format("{0}", row["Char_OldValue"].ToString()),
            //        string.Format("{0}", row["Char_NewValue"].ToString()),
            //        string.Format("{0}", row["Material"].ToString())
            //        //string.Format("{0}", row["Description"].ToString())
            //        );
            //}
            //if (dt.Rows.Count > 0)
            //{
            //    string file = InterfacePathOutbound + "CLMM_ChangeMatClass" + "_" + DateTime.Now.ToString("yyyyMMddhhmm") + ".csv";
            //    ToCSV(dt, file);
            //}
        }

        #endregion

        //public static void GetmasterUpdateToCSV(DataTable Results)
        //{
        //    DataTable dt = new DataTable();
        //    dt.Columns.AddRange(new DataColumn[] { new DataColumn (@"IfColumn"),
        //        new DataColumn(@"Characteristic Name RCTAV-ATNAM"),
        //        new DataColumn(@"Characteristic Value CAWN-ATWRT(01)"),
        //        new DataColumn(@"Text for a table entry CLHP-CR_STATUS_TEXT"),
        //        new DataColumn(@"Characteristic value description CAWNT-ATWTB(01)"),
        //    });
        //    DataTable listMat = new DataTable();
        //    listMat.Columns.AddRange(new DataColumn[] { new DataColumn (@"Characteristic Name txtSP$00005-LOW.text"),
        //    new DataColumn(@"txtSP$00003 - LOW.text"),
        //    });
        //    foreach (DataRow row in Results.Rows)
        //    {
        //        dt.Rows.Add(string.Format("{0}", row["Changed_Action"].ToString()),
        //        string.Format("{0}", row["Changed_Charname"].ToString()),
        //        string.Format("{0}", row["id"].ToString()),
        //        string.Format("{0}", row["Old_Description"].ToString()),
        //        string.Format("{0}", row["Description"].ToString()));

        //        listMat.Rows.Add(string.Format("{0}", row["Changed_Charname"].ToString()),
        //        string.Format("{0}", row["Old_Description"].ToString()));
        //    }
        //    if (listMat.Rows.Count > 0)
        //    {
        //        string file = @"D:\SAPInterfaces\Outbound\SQ01_ListMat" + "_" + DateTime.Now.ToString("yyyyMMddhhmm") + ".csv";
        //        CNService.ToCSV(listMat, file);
        //    }

        //    DataTable dtCT04Insert = new DataTable();
        //    DataTable dtCT04Update = new DataTable();
        //    DataTable dtCT04Remove = new DataTable();
        //    dtCT04Insert.Columns.AddRange(new DataColumn[] { new DataColumn (@"IfColumn"),
        //        new DataColumn(@"Characteristic Name RCTAV-ATNAM"),
        //        new DataColumn(@"Characteristic Value CAWN-ATWRT(01)"),
        //        new DataColumn(@"Characteristic value description CAWNT-ATWTB(01)"),
        //    });
        //    dtCT04Update.Columns.AddRange(new DataColumn[] { new DataColumn (@"IfColumn"),
        //        new DataColumn(@"Characteristic Name RCTAV-ATNAM"),
        //        new DataColumn(@"Characteristic Value CAWN-ATWRT(01)"),
        //        new DataColumn(@"Text for a table entry CLHP-CR_STATUS_TEXT"),
        //        new DataColumn(@"Characteristic value description CAWNT-ATWTB(01)"),
        //    });
        //    dtCT04Remove.Columns.AddRange(new DataColumn[] { new DataColumn (@"IfColumn"),
        //        new DataColumn(@"Characteristic Name RCTAV-ATNAM"),
        //        new DataColumn(@"Characteristic Value CAWN-ATWRT(01)"),
        //        new DataColumn(@"Text for a table entry CLHP-CR_STATUS_TEXT"),                
        //    });
        //    foreach (DataRow row in Results.Rows)
        //    {
        //        switch (row["Changed_Action"].ToString())
        //        {
        //            case "Insert":
        //                dtCT04Insert.Rows.Add(string.Format("{0}", row["Changed_Action"].ToString().Replace("Insert","I")),
        //                    string.Format("{0}", row["Changed_Charname"].ToString()),
        //                    string.Format("{0}", row["id"].ToString()),                           
        //                    string.Format("{0}", row["Description"].ToString()));
        //                break;
        //            case "Update":
        //                dtCT04Update.Rows.Add(string.Format("{0}", row["Changed_Action"].ToString()),
        //                   string.Format("{0}", row["Changed_Charname"].ToString()),
        //                   string.Format("{0}", row["id"].ToString()),
        //                   string.Format("{0}", row["Old_Description"].ToString()),
        //                   string.Format("{0}", row["Description"].ToString()));
        //                break;
        //            case "Remove":
        //                dtCT04Remove.Rows.Add(string.Format("{0}", row["Changed_Action"].ToString().Replace("Remove", "D")),
        //                   string.Format("{0}", row["Changed_Charname"].ToString()),                           
        //                   string.Format("{0}", row["Description"].ToString()));
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    if (dtCT04Insert.Rows.Count > 0)
        //    {              
        //        string file = @"D:\SAPInterfaces\Outbound\CT04_Insert_" + DateTime.Now.ToString("yyyyMMddhhmm") + ".csv";
        //        CNService.ToCSV(dtCT04Insert, file);
        //    }
        //    if (dtCT04Update.Rows.Count > 0)
        //    {
        //        string file = @"D:\SAPInterfaces\Outbound\CT04_Update_" + DateTime.Now.ToString("yyyyMMddhhmm") + ".csv";
        //        CNService.ToCSV(dtCT04Update, file);
        //    }
        //    if (dtCT04Remove.Rows.Count > 0)
        //    {
        //        string file = @"D:\SAPInterfaces\Outbound\CT04_Remove_" + DateTime.Now.ToString("yyyyMMddhhmm") + ".csv";
        //        CNService.ToCSV(dtCT04Remove, file);
        //    }


        //    //string[] ColumnsToBeDeleted = { "Insert", "Update", "Remove" };
        //    //if (dt.Rows.Count > 0)
        //    //    foreach (string ColName in ColumnsToBeDeleted)
        //    //    {
        //    //        foreach(DataRow row in dt.Rows)
        //    //        {
        //    //            row[0].ToString();
        //    //        }
        //    //         //DataRow["IfColumn"][1].ToString().Replace(ColName, "I");
        //    //        //dt.Rows.Add(dt.Rows[][0].ToString().Replace("Insert", "I")
        //    //            //);

        //    //        //dt.Rows.Add(row["DocumentNo"].ToString(),
        //    //        //    row["DocumentNo"].ToString()
        //    //        //    );

        //    //        //var dtclone = new DataTable();
        //    //        //if (dt.Select("IfColumn='" + ColName + "'").ToList().Count > 0)
        //    //        //{
        //    //        //    //if (ColName == "Update")
        //    //        //    //{
        //    //        //    //    dtclone = dt.Select("IfColumn='" + ColName + "'").CopyToDataTable();
        //    //        //    //}
        //    //        //    //else if (ColName == "Insert")
        //    //        //    //{
        //    //        //    //    dtclone = dt.Select("IfColumn='" + ColName + "'").CopyToDataTable();
        //    //        //    //    //dtclone = dt.Select("IfColumn='Insert'").CopyToDataTable();
        //    //        //    //    //dtclone.Columns[0]..ItemArray[0].ToString().Replace("a","I");
        //    //        //    //    //dtclone.Rows[0].ItemArray[0].ToString().Replace('Insert', 'I');
        //    //        //    //    //dtclone = dt.Rows[0][0].ToString().CopyToDataTable();
        //    //        //    //    dtclone.Rows[0][0].ToString().Replace(ColName, "I");
        //    //        //    //    dtclone.Columns.Remove(@"Text for a table entry CLHP-CR_STATUS_TEXT");
        //    //        //    //}
        //    //        //    //else if (ColName == "Remove")
        //    //        //    //{
        //    //        //    //    dtclone = dt.Select("IfColumn='" + ColName + "'").CopyToDataTable();
        //    //        //    //    dtclone.Columns.Remove(@"Characteristic Value CAWN-ATWRT(01)");
        //    //        //    //    dtclone.Columns.Remove(@"Characteristic value description CAWNT-ATWTB(01)");
        //    //        //    //}
        //    //        //    //string file = HttpContext.Current.Server.MapPath("~/ExcelFiles/CT04_" + ColName + "_" + DateTime.Now.ToString("yyyyMMddhhmm") + ".csv");
        //    //        //    string file = @"D:\SAPInterfaces\Outbound\CT04_" + ColName + "_" + DateTime.Now.ToString("yyyyMMddhhmm") + ".csv";

        //    //        //    CNService.ToCSV(dtclone, file);
        //    //        //}
        //    //    }


        //}
        //public static void GetImpactmat(string sName)
        //{
        //    //Char.Name ctxtG_CHAR_TAB - ATNAM[0, 0].text
        //    //Old Value ctxtG_CHAR_TAB - OLDATWRT[1, 0].text
        //    //New Value ctxtG_CHAR_TAB - NEWATWRT[2, 0].text
        //    //Table cell -TextField txtG_TARGET_TAB - OBJECT[0, 0].text
        //    using (SqlConnection con = new SqlConnection(CNService.strConn))
        //    {
        //        SqlCommand cmd = new SqlCommand();
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.CommandText = "spGetImpactmat";
        //        cmd.Parameters.AddWithValue("@Active", sName);
        //        cmd.Connection = con;
        //        con.Open();
        //        DataSet oDataset = new DataSet();
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(oDataset);
        //        con.Close();
        //        DataTable dt = new DataTable();
        //        dt.Columns.AddRange(new DataColumn[] { new DataColumn (@"Char.Name ctxtG_CHAR_TAB - ATNAM"),
        //        new DataColumn(@"Old Value ctxtG_CHAR_TAB - OLDATWRT"),
        //        new DataColumn(@"New Value ctxtG_CHAR_TAB - NEWATWRT"),
        //        new DataColumn(@"Table cell -TextField txtG_TARGET_TAB - OBJECT"),
        //        });

        //        DataTable dtImpactMatDesc = new DataTable(); 
        //        dtImpactMatDesc.Columns.AddRange(new DataColumn[] { new DataColumn (@"Material Number RMMG1 - MATNR"),
        //        new DataColumn(@"Material description MAKT - MAKTX"),});
        //       // foreach (DataRow row in oDataset.Tables[0].Rows)
        //       // {
        //       //     dt.Rows.Add(string.Format("{0}", row["Char_Name"].ToString()),
        //       //string.Format("{0}", row["Char_OldValue"].ToString()),
        //       //string.Format("{0}", row["Char_NewValue"].ToString()),
        //       //string.Format("{0}", row["Material"].ToString()),
        //       //string.Format("{0}", row["Description"].ToString()));


        //       //     dtImpactMatDesc.Rows.Add(
        //       //string.Format("{0}", row["Material"].ToString()),
        //       //string.Format("{0}", row["Description"].ToString()));
        //       // }
        //        //if (dt.Rows.Count > 0)
        //        //{
        //        //    string file = @"D:\SAPInterfaces\Outbound\CLMM_ChangeMatClass" + "_" + DateTime.Now.ToString("yyyyMMddhhmm") + ".csv";
        //        //    CNService.ToCSV(dt, file);
        //        //}


        //        //if (dtImpactMatDesc.Rows.Count > 0)
        //        //{
        //        //    string file = @"D:\SAPInterfaces\Outbound\MM02_ImpactMatDesc" + "_" + DateTime.Now.ToString("yyyyMMddhhmm") + ".csv";
        //        //    CNService.ToCSV(dtImpactMatDesc, file);
        //        //}
        //    }
        //}
        //public static void GetQuery(string sName)
        //{
        //    using (SqlConnection con = new SqlConnection(CNService.strConn))
        //    {
        //        SqlCommand cmd = new SqlCommand();
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.CommandText = "spQuery";
        //        cmd.Parameters.AddWithValue("@Material", sName);
        //        cmd.Connection = con;
        //        con.Open();
        //        DataSet oDataset = new DataSet();
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(oDataset);
        //        con.Close();
        //        GetQueryToSQL(oDataset.Tables[0]);
        //    }
        //}
        //public static void GetQueryToSQL(DataTable Results)
        //{

        //    DataTable dt = new DataTable();
        //    dt.Columns.AddRange(new DataColumn[] { new DataColumn (@"IfColumn"),
        //        new DataColumn(@"Material Number RMMG1-MATNR"),
        //    new DataColumn(@"Material Description (Short Text) MAKT-MAKTX"),
        //    new DataColumn(@"Reference material RMMG1_REF-MATNR"),
        //    new DataColumn(@"IfColumn and Plant RMMG1-WERKS and Reference plant RMMG1_REF-WERKS"),
        //    new DataColumn(@"IfColumn and Sales Organization RMMG1-VKORG and Reference sales organization RMMG1_REF-VKORG"),
        //    new DataColumn(@"Distribution Channel RMMG1-VTWEG and Reference distribution channel RMMG1_REF-VTWEG"),
        //    });
        //    foreach (DataRow row in Results.Rows)
        //    {
        //        string[] split = string.Format("{0}", row["Plant"].ToString()).Split(new Char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

        //        foreach (string s in split)
        //        {

        //            if (s.Trim() != "")
        //            {
        //                string[] splitSOOrg = "DM;EX".Split(new Char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

        //                foreach (string o in splitSOOrg)
        //                {
        //                    dt.Rows.Add(string.Format("{0}", row["DocumentNo"].ToString()),
        //        string.Format("{0}", row["Material"].ToString()),
        //        string.Format("{0}", row["Description"].ToString()),
        //        string.Format("{0}", row["Ref"].ToString()),
        //        string.Format("{0}", s),
        //        string.Format("{0}", row["Plant"]).Substring(0, 3),
        //        //string.Format("{0}", row["Plant"].ToString())

        //        string.Format("{0}", o)
        //        );
        //                }
        //            }
        //        }
        //    }
        //    if (dt.Rows.Count > 0)
        //    {
        //        string file = @"D:\SAPInterfaces\Outbound\MM01_CreateMAT_ExtensionPlant_" + DateTime.Now.ToString("yyyyMMddhhmm") + ".csv";
        //        //string file = HttpContext.Current.Server.MapPath("~/ExcelFiles/MM01_CreateMAT_ExtensionPlant_" + DateTime.Now.ToString("yyyyMMddhhmm") + ".csv");
        //        CNService.ToCSV(dt, file);
        //    }
        //    //sale org
        //    //DataTable dtSaleOrg = new DataTable();
        //    //dtSaleOrg.Columns.AddRange(new DataColumn[] { new DataColumn(@"Material Number RMMG1-MATNR"),
        //    //    new DataColumn(@"Reference material RMMG1_REF-MATNR"),
        //    //new DataColumn(@"IfColumn and Sales Organization RMMG1-VKORG and Reference sales organization RMMG1_REF-VKORG"),
        //    //new DataColumn(@"Distribution Channel RMMG1-VTWEG and Reference distribution channel RMMG1_REF-VTWEG"),
        //    //});
        //    //DataTable dtSOOrg = new DataTable();
        //    //dtSOOrg.Columns.AddRange(new DataColumn[] { new DataColumn(@"SaleOrg") });
        //    //foreach (DataRow row in Results.Rows)
        //    //{
        //    //    string[] str = string.Format("{0}", row["Plant"].ToString()).Split(new Char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

        //    //    foreach (string s in str)
        //    //    {

        //    //        if (s.Trim() != "" && dtSOOrg.Select("SaleOrg='" + string.Format("{0}", s).Substring(0, 3) + "'").ToList().Count == 0)
        //    //        {
        //    //            dtSOOrg.Rows.Add(string.Format("{0}", s).Substring(0, 3));
        //    //        }
        //    //    }
        //    //    foreach (DataRow soorg in dtSOOrg.Rows)
        //    //    {
        //    //        string[] split = "DM;EX".Split(new Char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

        //    //        foreach (string s in split)
        //    //        {
        //    //            dtSaleOrg.Rows.Add(string.Format("{0}", row["Material"].ToString()),
        //    //        string.Format("{0}", row["Ref"].ToString()),
        //    //        string.Format("{0}", soorg["SaleOrg"].ToString()),
        //    //        //string.Format("{0}", row["Plant"].ToString())
        //    //        string.Format("{0}", s)
        //    //        );
        //    //        }
        //    //    }
        //    //    if (dtSaleOrg.Rows.Count > 0)
        //    //    {
        //    //        string file = @"D:\SAPInterfaces\Outbound\MM01_ExtendSaleOrg_" + DateTime.Now.ToString("yyyyMMddhhmm") + ".csv";
        //    //        //string file = HttpContext.Current.Server.MapPath("~/ExcelFiles/MM01_ExtendSaleOrg_" + DateTime.Now.ToString("yyyyMMddhhmm") + ".csv");
        //    //        CNService.ToCSV(dtSaleOrg, file);
        //    //    }
        //    //}
        //    //Characteristics
        //    DataTable dtClass = new DataTable();
        //    dtClass.Columns.AddRange(new DataColumn[] { new DataColumn(@"Material Number RMMG1-MATNR"),
        //        new DataColumn(@"Loop Id Column"),
        //        new DataColumn(@"Characteristic Name ALLOCVALUESCHARNEW-CHARACT"),
        //        new DataColumn(@"Characteristic Value ALLOCVALUESCHARNEW-VALUE_CHAR") });
        //    foreach (DataRow row in Results.Rows)
        //    {
        //        dtClass.Rows.Add(string.Format("{0}", row["Material"].ToString()),
        //        string.Format("{0}", "H"),
        //        string.Format("{0}", ""),
        //        string.Format("{0}", "")
        //        );
        //        DataTable dtCharacteristic = CNService.builditems(@"select * from MasCharacteristic where MaterialType  like '%" +
        //            row["Material"].ToString().Substring(1, 1) + "%' order by Id");
        //        foreach (DataRow dr in dtCharacteristic.Rows)
        //        {
        //            string value = string.Format("{0}", dr["shortname"]);
        //            dtClass.Rows.Add(string.Format("{0}", ""),
        //            string.Format("{0}", "D"),
        //            string.Format("{0}", dr["Title"]),
        //            string.Format("{0}", row[value])
        //            );
        //        }
        //        if (dtClass.Rows.Count > 0)
        //        {
        //            string file = @"D:\SAPInterfaces\Outbound\BAPI_UpdateMATCharacteristics_" + DateTime.Now.ToString("yyyyMMddhhmm") + ".csv";
        //            //string file = HttpContext.Current.Server.MapPath("~/ExcelFiles/BAPI_UpdateMATCharacteristics_" + DateTime.Now.ToString("yyyyMMddhhmm") + ".csv");
        //            CNService.ToCSV(dtClass, file);
        //        }
        //    }
        //}
        //public static void GetUpdateTOSQL(string data)
        //{
        //    var dir = @"D:\SAPInterfaces\Inbound";
        //    //HttpContext.Current.Server.MapPath("~/ExcelFiles");
        //    var filePaths = Directory.GetFiles(dir, "CT04*_Result*.csv");
        //    foreach (string s in filePaths)
        //    {
        //        using (var reader = new StreamReader(s))
        //        {
        //            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        //            {
        //                while (csv.Read())
        //                {//This will advance the reader to the next record.

        //                    //You can use an indexer to get by position or name. 
        //                    //This will return the field as a string

        //                    // By position
        //                    var field = csv[0];
        //                    //var AppID = csv["AppID"];
        //                    // By header name
        //                    //csv.Read();
        //                    csv.ReadHeader();
        //                    string[] headerRow = csv.Context.Reader.HeaderRecord;
        //                    string[] filteredValues = Array.FindAll(headerRow, x => x.Contains("Result"));
        //                    var Condition = "";// csv["Condition TypeRV13A-KSCHL"];
        //                    if (filteredValues.Length > 0)
        //                        if (csv["Result"] == "Condition records saved")
        //                        {
        //                            string Changed_Id = "";
        //                            string Changed_Action = "";
        //                            string Material = "";
        //                            string Description = "";
        //                            string DMSNo = "";
        //                            string New_Material = "";
        //                            string New_Description = "";
        //                            string Status = "";
        //                            string Reason = "";
        //                            string NewMat_JobId = "";
        //                            string Char_Name = "";
        //                            string Char_OldValue = "";
        //                            string Char_NewValue = "";
        //                            SendEmailUpdateMaster("U" + Material);
        //                            using (SqlConnection con = new SqlConnection(CNService.strConn))
        //                            {
        //                                SqlCommand cmd = new SqlCommand();
        //                                cmd.CommandType = CommandType.StoredProcedure;
        //                                cmd.CommandText = "spUpdateImpactedmat";

        //                                cmd.Parameters.AddWithValue("@Changed_Id", Changed_Id);
        //                                cmd.Parameters.AddWithValue("@Changed_Action", Changed_Action);
        //                                cmd.Parameters.AddWithValue("@Material", Material);
        //                                cmd.Parameters.AddWithValue("@Description", Description);
        //                                cmd.Parameters.AddWithValue("@DMSNo", DMSNo);
        //                                cmd.Parameters.AddWithValue("@New_Material", New_Material);
        //                                cmd.Parameters.AddWithValue("@New_Description", New_Description);
        //                                cmd.Parameters.AddWithValue("@Status", Status);
        //                                cmd.Parameters.AddWithValue("@Reason", Reason);
        //                                cmd.Parameters.AddWithValue("@NewMat_JobId", NewMat_JobId);
        //                                cmd.Parameters.AddWithValue("@Char_Name", Char_Name);
        //                                cmd.Parameters.AddWithValue("@Char_OldValue", Char_OldValue);
        //                                cmd.Parameters.AddWithValue("@Char_NewValue", Char_NewValue);


        //                                cmd.Connection = con;
        //                                con.Open();
        //                                DataTable dtResult = new DataTable();
        //                                SqlDataAdapter oAdapter = new SqlDataAdapter(cmd);
        //                                oAdapter.Fill(dtResult);
        //                                con.Close();
        //                            }
        //                            //    using (SqlConnection con = new SqlConnection(strConn))
        //                            //    {
        //                            //        SqlCommand cmd = new SqlCommand();
        //                            //        cmd.CommandType = CommandType.StoredProcedure;
        //                            //        cmd.CommandText = "spUpdateQuotationfromJob";
        //                            //        cmd.Parameters.AddWithValue("@MinPrice", string.Format("{0}", 0));
        //                            //        cmd.Parameters.AddWithValue("@subID", string.Format("{0}", Condition.ToString() == "zpm1" ? "0" : AppID.ToString()));
        //                            //        cmd.Parameters.AddWithValue("@ID", AppID.ToString());
        //                            //        cmd.Connection = con;
        //                            //        con.Open();
        //                            //        cmd.ExecuteNonQuery();
        //                            //        con.Close();
        //                            //    }
        //                            //}
        //                        }
        //                    //var records = csv.GetRecords<dynamic>();
        //                    //foreach (var item in records)
        //                    //{
        //                    //    var da = item.Result;
        //                    //    var condition = item[@"Condition Type RV13A-KSCHL"];  
        //                    //}
        //                    //foreach (dynamic record in records.ToList())
        //                    //{
        //                    //    var data = record["IfColumn"];
        //                    //}
        //                }
        //            }
        //        }
        //        try
        //        {
        //            //File.Move(s, @"D:\SAPInterfaces\Inbound\Processed\" + Path.GetFileName(s));
        //            //HttpContext.Current.Server.MapPath("~/ExcelFiles/Processed/" + Path.GetFileName(s)));
        //            // Copy the file and overwrite if it exists
        //            File.Copy(s, @"D:\SAPInterfaces\Inbound\Processed\" + Path.GetFileName(s), true);

        //            // Delete the source file
        //            File.Delete(s);
        //        }
        //        catch (IOException iox)
        //        {
        //            Console.WriteLine(iox.Message);
        //        }
        //    }
        //    // Move MM01 & BAPI_UpdateMATCharacteristics
        //    filePaths = Directory.GetFiles(dir, "MM01*_Result*.csv");
        //    foreach (string s in filePaths)
        //    {
        //        try
        //        {
        //            using (var reader = new StreamReader(s))
        //            {
        //                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        //                {
        //                    while (csv.Read())
        //                    {//This will advance the reader to the next record.

        //                        //You can use an indexer to get by position or name. 
        //                        //This will return the field as a string

        //                        // By position
        //                        var field = csv[0];
        //                        //var AppID = csv["AppID"];
        //                        // By header name
        //                        //csv.Read();
        //                        csv.ReadHeader();
        //                        string[] headerRow = csv.Context.Reader.HeaderRecord;
        //                        string[] filteredValues = Array.FindAll(headerRow, x => x.Contains("Result"));
        //                        var Condition = "";// csv["Condition TypeRV13A-KSCHL"];
        //                        if (filteredValues.Length > 0)
        //                            if (csv["Result"] == "Condition records saved")
        //                            {
        //                            }
        //                    }
        //                }
        //            }
        //            //File.Move(s, @"D:\SAPInterfaces\Inbound\Processed\" + Path.GetFileName(s));
        //            //HttpContext.Current.Server.MapPath("~/ExcelFiles/Processed/" + Path.GetFileName(s)));
        //            // Copy the file and overwrite if it exists
        //            File.Copy(s, @"D:\SAPInterfaces\Inbound\Processed\" + Path.GetFileName(s), true);

        //            // Delete the source file
        //            File.Delete(s);
        //        }
        //        catch (IOException iox)
        //        {
        //            Console.WriteLine(iox.Message);
        //        }
        //    }
        //    // Move BAPI_UpdateMATCharacteristics
        //    filePaths = Directory.GetFiles(dir, "BAPI*_Result*.csv");
        //    foreach (string s in filePaths)
        //    {
        //        try
        //        {
        //            //File.Move(s, @"D:\SAPInterfaces\Inbound\Processed\" + Path.GetFileName(s));
        //            //HttpContext.Current.Server.MapPath("~/ExcelFiles/Processed/" + Path.GetFileName(s)));
        //            // Copy the file and overwrite if it exists
        //            File.Copy(s, @"D:\SAPInterfaces\Inbound\Processed\" + Path.GetFileName(s), true);

        //            // Delete the source file
        //            File.Delete(s);
        //        }
        //        catch (IOException iox)
        //        {
        //            Console.WriteLine(iox.Message);
        //        }
        //    }
        //}
        //public static void localProcessKill(string processName)
        //{
        //    foreach (Process p in Process.GetProcessesByName(processName))
        //    {
        //        p.Kill();
        //    }
        //}
        //public static void testsendmaster(string SubChanged_Id)
        //{
        //    string strSQL = " select Id,Changed_Charname,Description,Changed_Action,Old_Description from TransMaster where Changed_id ='" + SubChanged_Id + "'";
        //    DataTable dt = CNService.builditems(strSQL);
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        string _Id = dr["Id"].ToString();
        //        string _Description = dr["Description"].ToString();
        //        string _Changed_Action = dr["Changed_Action"].ToString();
        //        string _old_id = dr["Old_Description"].ToString();
        //        string[] value = { dr["Changed_Charname"].ToString(), _Description, _Id, _Changed_Action, _old_id };
        //        CNService.master_artwork(value);
        //    }
        //}
        //public static void SendEmailUpdateMaster(string _name)
        //{
        //    //string datapath = "~/FileTest/" + _name;
        //    string _email = "";
        //    string _Id = "";
        //    string _Description = "";
        //    string _Body = "";
        //    string _Attached = "";
        //    string SubChanged_Id = CNService.ReadItems(@"select cast(substring('"
        //    + _name.ToString() + "',2,len('" + _name.ToString() + "')-1) as nvarchar(max)) value");
        //    testsendmaster(SubChanged_Id);
        //    DataTable dt = new DataTable();
        //    using (SqlConnection con = new SqlConnection(CNService.strConn))
        //    {
        //        SqlCommand cmd = new SqlCommand();
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.CommandText = "spsendemail_upm";
        //        cmd.Parameters.AddWithValue("@Changed_Id", _name.ToString());
        //        cmd.Connection = con;
        //        con.Open();
        //        SqlDataAdapter oAdapter = new SqlDataAdapter(cmd);
        //        oAdapter.Fill(dt);
        //        con.Close();
        //    }
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        _email = dr["Email"].ToString();
        //        _Id = dr["Id"].ToString();
        //        _Description = dr["Description"].ToString();
        //        _Body = dr["Body"].ToString();
        //        _Attached = dr["attached"].ToString();
        //    }
        //    //        MailMessage msg = new MailMessage();
        //    //        string[] words = _email.Split(';');
        //    //        foreach (string word in words)
        //    //        {
        //    //            msg.To.Add(new MailAddress(word));
        //    //            //Console.WriteLine(word);
        //    //        }
        //    //        //msg.To.Add(new MailAddress(_email));
        //    //        msg.From = new MailAddress("wshuttleadm@thaiunion.com");
        //    //        msg.Subject = "Maintained characteristic master data in SAP" + "[" + _Body.Substring(0, 6) + "]";
        //    //        //msg.Body = "Id  " + _Id.ToString() + "Description  " + _Description.ToString() + " Changed";
        //    //        //msg.Body = "Maintained characteristic master completed";
        //    //        msg.Body = _Body;
        //    //        //msg.Attachments.Add(new System.Net.Mail.Attachment(_Attached));
        //    //        msg.IsBodyHtml = true;
        //    //
        //    //        SmtpClient client = new SmtpClient();
        //    //        client.UseDefaultCredentials = false;
        //    //        client.Credentials = new System.Net.NetworkCredential("wshuttleadm@thaiunion.com", "WSP@ss2018");
        //    //        client.Port = 587; // You can use Port 25 if 587 is blocked (mine is!)
        //    //        client.Host = "smtp.office365.com";
        //    //        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        //    //        client.EnableSsl = true;
        //    //        try
        //    //        {
        //    //            client.Send(msg);
        //    //            Context.Response.Write("Message Sent Succesfully");
        //    //        }
        //    //        catch (Exception ex)
        //    //        {
        //    //            Context.Response.Write(ex.ToString());
        //    //        }
        //    CNService.sendemail(_email, "", _Body,
        //        "PRD Characteristic master is maintained in SAP " + "[" + _Body.Substring(0, 6) + "]",
        //        _Attached);
        //}

    }
}

