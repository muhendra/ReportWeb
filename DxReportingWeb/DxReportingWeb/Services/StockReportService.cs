using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

public class StockReportService
{
    private readonly string _connectionString;

    public StockReportService(IConfiguration configuration)
    {
        // Ambil connection string dari konfigurasi aplikasi
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<StockReport[]> GetStockReportAsync(string startDate, string endDate, int categoryid, string userid)
    {
        var reports = new List<StockReport>();

        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("SP_STOCK_REPORT_DC_ONLINE", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@DATE_FROM", SqlDbType.VarChar) { Value = startDate });
                    command.Parameters.Add(new SqlParameter("@DATE_TO", SqlDbType.VarChar) { Value = endDate });
                    command.Parameters.Add(new SqlParameter("@GROUP_CODE", SqlDbType.Int) { Value = categoryid });
                    command.Parameters.Add(new SqlParameter("@STORE_CODE", SqlDbType.VarChar, 50) { Value = userid });

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            try
                            {
                                int bom = reader.IsDBNull(reader.GetOrdinal("BOM")) ? 0 : reader.GetInt32(reader.GetOrdinal("BOM"));
                                int recRet = reader.IsDBNull(reader.GetOrdinal("REC_RET")) ? 0 : reader.GetInt32(reader.GetOrdinal("REC_RET"));
                                int retTrf = reader.IsDBNull(reader.GetOrdinal("RET_TRF")) ? 0 : reader.GetInt32(reader.GetOrdinal("RET_TRF"));
                                int sls = reader.IsDBNull(reader.GetOrdinal("SLS")) ? 0 : reader.GetInt32(reader.GetOrdinal("SLS"));

                                int denominator = bom + recRet + retTrf;
                                int stPersen = (denominator > 0) ? (sls * 100) / denominator : 0;

                                reports.Add(new StockReport
                                {
                                    ARTICLE = reader.IsDBNull(reader.GetOrdinal("ARTICLE")) ? "" : reader.GetString(reader.GetOrdinal("ARTICLE")),
                                    CODE1 = reader.IsDBNull(reader.GetOrdinal("CODE1")) ? "" : reader.GetString(reader.GetOrdinal("CODE1")),
                                    BOM = bom,
                                    REC_RET = recRet,
                                    RET_TRF = retTrf,
                                    S_120 = reader.IsDBNull(reader.GetOrdinal("S_120")) ? 0 : reader.GetInt32(reader.GetOrdinal("S_120")),
                                    S_127 = reader.IsDBNull(reader.GetOrdinal("S_127")) ? 0 : reader.GetInt32(reader.GetOrdinal("S_127")),
                                    S_131 = reader.IsDBNull(reader.GetOrdinal("S_131")) ? 0 : reader.GetInt32(reader.GetOrdinal("S_131")),
                                    S_132 = reader.IsDBNull(reader.GetOrdinal("S_132")) ? 0 : reader.GetInt32(reader.GetOrdinal("S_132")),
                                    S_G01 = reader.IsDBNull(reader.GetOrdinal("S_G01")) ? 0 : reader.GetInt32(reader.GetOrdinal("S_G01")),
                                    SLS = sls,
                                    EOM = reader.IsDBNull(reader.GetOrdinal("EOM")) ? 0 : reader.GetInt32(reader.GetOrdinal("EOM")),
                                    STPERSEN = stPersen,
                                    EOM_DC = reader.IsDBNull(reader.GetOrdinal("EOM_DC")) ? 0 : reader.GetInt32(reader.GetOrdinal("EOM_DC")),
                                    CODE = reader.IsDBNull(reader.GetOrdinal("CODE")) ? "" : reader.GetString(reader.GetOrdinal("CODE")),
                                    RET_SLS = reader.IsDBNull(reader.GetOrdinal("RET_SLS")) ? 0 : reader.GetInt32(reader.GetOrdinal("RET_SLS")),
                                    ADJ = reader.IsDBNull(reader.GetOrdinal("ADJ")) ? 0 : reader.GetInt32(reader.GetOrdinal("ADJ")),
                                    ADJ_OPN = reader.IsDBNull(reader.GetOrdinal("ADJ_OPN")) ? 0 : reader.GetInt32(reader.GetOrdinal("ADJ_OPN")),
                                    SLS_RP = reader.IsDBNull(reader.GetOrdinal("SLS_RP")) ? 0.0 : reader.GetDouble(reader.GetOrdinal("SLS_RP")),
                                    EOM_RP = reader.IsDBNull(reader.GetOrdinal("EOM_RP")) ? 0.0 : reader.GetDouble(reader.GetOrdinal("EOM_RP")),
                                    ORD_NO = reader.IsDBNull(reader.GetOrdinal("ORD_NO")) ? 0 : reader.GetInt32(reader.GetOrdinal("ORD_NO")),
                                    STATUS_HARGA = reader.IsDBNull(reader.GetOrdinal("STATUS_HARGA")) ? "" : reader.GetString(reader.GetOrdinal("STATUS_HARGA")),
                                    QTY_ORD = reader.IsDBNull(reader.GetOrdinal("QTY_ORD")) ? 0 : reader.GetInt32(reader.GetOrdinal("QTY_ORD"))
                                    //SSR = reader.IsDBNull(reader.GetOrdinal("SSR")) ? 0 : reader.GetInt32(reader.GetOrdinal("SSR")),
                                });
                            }
                            catch (Exception fieldEx)
                            {
                                Console.WriteLine($"Error reading field: {fieldEx.Message}");
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database error: {ex.Message}");
        }

        return reports.ToArray();
    }

}

public class StockReport
{
    public int Nomor { get; set; }
    public int ORD_NO { get; set; }
    public string? STATUS_HARGA { get; set; }
    public int QTY_ORD { get; set; }
    public string? CODE1 { get; set; }
    public DateTime FirstDate { get; set; }     // FIRSTDATE
    public DateTime AsOfDate { get; set; }      // ASOFDATE
    public string? ARTICLE { get; set; }        // ARTICLE
    public string? EVENT_ID { get; set; }       // EVENT_ID
    public string? DESCRIPTION { get; set; }    // DESCRIPTION
    public double PRICE { get; set; }           // PRICE
    public string? GROUP_6 { get; set; }        // GROUP_6
    public string? CODE { get; set; }           // CODE
    public string? STORE_CODE { get; set; }     // STORE_CODE
    public string? STORE_NAME { get; set; }     // STORE_NAME
    public int BOM { get; set; }                // BOM
    public int SLS { get; set; }                // SLS
    public int RET_SLS { get; set; }            // RET_SLS
    public int ORD { get; set; }                // ORD
    public int TRF { get; set; }                // TRF
    public int RET_TRF { get; set; }            // RET_TRF
    public int REC_RET { get; set; }            // REC_RET
    public int ADJ { get; set; }                // ADJ
    public int ADJ_OPN { get; set; }            // ADJ_OPN
    public int REC_TRF { get; set; }            // REC_TRF
    public int ADJ_SLS { get; set; }            // ADJ_SLS
    public int EOM { get; set; }                // EOM
    public int EOM_DC { get; set; } 
    public int STPERSEN { get; set; }
    public double EOM_RP { get; set; }          // EOM_RP
    public double SLS_RP { get; set; }          // SLS_RP
    public int S_120 { get; set; }              // S_120
    public int S_127 { get; set; }              // S_127
    public int S_131 { get; set; }              // S_131
    public int S_132 { get; set; }              // S_132
    public int S_G01 { get; set; }              // S_G01
    public int SSR {  get; set; }   
}
