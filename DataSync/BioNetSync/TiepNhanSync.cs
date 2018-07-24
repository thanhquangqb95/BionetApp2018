﻿using BioNetModel;
using BioNetModel.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;


namespace DataSync.BioNetSync
{
    public class TiepNhanSync
    {
        private static BioNetDBContextDataContext db = null;
        private static string linkGetTiepNhan = "/api/tiepnhan/getall?keyword=&page=0&pagesize=20";
        private static string linkPostTiepNhan = "/api/tiepnhan/AddUpFromApp";

        public static PsReponse GetTiepNhan()
        {
            PsReponse res = new PsReponse();
            res.Result = true;
            try
            {
                ProcessDataSync cn = new ProcessDataSync();
                db = cn.db;
                var account = db.PSAccount_Syncs.FirstOrDefault();
                if (account != null)
                {
                    string token = cn.GetToken(account.userName, account.passWord);
                    if (!string.IsNullOrEmpty(token))
                    {
                        var result = cn.GetRespone(cn.CreateLink(linkGetTiepNhan), token);
                        if (result.Result)
                        {
                            string json = result.ValueResult;
                            JavaScriptSerializer jss = new JavaScriptSerializer();
                            ObjectModel.RootObjectAPI psl = jss.Deserialize<ObjectModel.RootObjectAPI>(json);
                            //List<PSPatient> patient = jss.Deserialize<List<PSPatient>>(json);
                            List<PSTiepNhan> lstpsl = new List<PSTiepNhan>();
                            if (psl.TotalCount > 0)
                            {
                                foreach (var item in psl.Items)
                                {
                                    PSTiepNhan term = new PSTiepNhan();
                                    term = cn.CovertDynamicToObjectModel(item, term);
                                    lstpsl.Add(term);
                                }
                                //UpdatePatient(patient);
                                UpdateTiepNhan(lstpsl);
                                res.Result = true;

                            }
                        }
                        else
                        {
                            res.Result = false;
                            res.StringError = result.ErorrResult;
                        }
                    }
                    else
                    {
                        res.Result = false;
                        res.StringError = "Kiểm tra lại kết nối mạng hoặc tài khoản đồng bộ!";
                    }

                }
                else
                {
                    res.Result = false;
                    res.StringError = "Chưa có  tài khoản đồng bộ!";
                }

            }
            catch (Exception ex)
            {
                res.Result = false;
                res.StringError = DateTime.Now.ToString() + "Lỗi khi get dữ liệu Danh Mục Mapping Kỹ Thuật - Dịch Vụ từ server \r\n " + ex.Message;

            }
            return res;
        }
        public static PsReponse UpdateTiepNhan(List<PSTiepNhan> lstpsl)
        {

            PsReponse res = new PsReponse();

            try
            {
                ProcessDataSync cn = new ProcessDataSync();
                db = cn.db;
                var account = db.PSPhieuSangLocs.FirstOrDefault();
                db.Connection.Open();
                db.Transaction = db.Connection.BeginTransaction();
                foreach (var psl in lstpsl)
                {
                    var psldb = db.PSTiepNhans.FirstOrDefault(p => p.MaPhieu == psl.MaPhieu);
                    if (psldb != null)
                    {
                        var term = psldb.RowIDPhieu;
                        cn.ConvertObjectToObject(psl, psldb);
                        psldb.RowIDPhieu = term;

                        db.SubmitChanges();
                    }
                    else
                    {
                        PSTiepNhan newpsl = new PSTiepNhan();
                        newpsl = psl;
                        newpsl.RowIDPhieu = 0;
                        newpsl.isDongBo = true;
                        db.PSTiepNhans.InsertOnSubmit(newpsl);
                        db.SubmitChanges();
                    }
                }

                db.Transaction.Commit();
                db.Connection.Close();
                res.Result = true;
            }
            catch (Exception ex)
            {
                db.Transaction.Rollback();
                db.Connection.Close();
                res.Result = false;
                res.StringError = ex.ToString();
            }
            return res;
        }

        public static PsReponse PostTiepNhan()
        {
            PsReponse res = new PsReponse();
            try
            {
                ProcessDataSync cn = new ProcessDataSync();
                db = cn.db;
                var account = db.PSAccount_Syncs.FirstOrDefault();
                if (account != null)
                {
                    string token = cn.GetToken(account.userName, account.passWord);
                    if (!string.IsNullOrEmpty(token))
                    {
                        var datas = db.PSTiepNhans.Where(p => p.isDongBo != true && p.isXoa!=true).OrderBy(x => x.RowIDTiepNhan).ToList();
                        if(datas!=null)
                        {
                            List<string> jsonstr = new List<string>();
                            string Nhom = (string)null;
                            while (datas.Count() > 1000)
                            {
                                var temp = datas.Take(1000);
                                Nhom = JsonConvert.SerializeObject(temp);
                                //Nhom = new JavaScriptSerializer().Serialize(temp);
                                jsonstr.Add(Nhom);
                                datas.RemoveRange(0, 1000);
                            }
                            if (datas.Count() <= 1000 && datas.Count() > 0)
                            {
                                Nhom = JsonConvert.SerializeObject(datas);
                               // Nhom = new JavaScriptSerializer().Serialize(datas);
                                jsonstr.Add(Nhom);
                            }
                            if (jsonstr.Count() > 0)
                            {
                                #region Đồng bộ phiếu
                                foreach (var jsons in jsonstr)
                                {
                                    var result = cn.PostRespone(cn.CreateLink(linkPostTiepNhan), token, jsons);
                                    if (result.Result)
                                    {
                                        JavaScriptSerializer js = new JavaScriptSerializer();
                                        List<PSTiepNhan> datares = js.Deserialize<List<PSTiepNhan>>(jsons);
                                        var data = db.PSTiepNhans.Where(s => (from d in datares select d.MaPhieu).Contains(s.MaPhieu));
                                        data.ToList().ForEach(c => c.isDongBo = true);
                                        db.SubmitChanges();
                                        string json = result.ErorrResult;
                                        JavaScriptSerializer jss = new JavaScriptSerializer();
                                        List<String> psl = jss.Deserialize<List<String>>(json);
                                        if (psl != null)
                                        {
                                            if (psl.Count > 0)
                                            {
                                                res.StringError = "Danh sách phiếu tiếp nhận lỗi: \r\n ";
                                                foreach (var lst in psl)
                                                {
                                                    PSResposeSync sn = cn.CutString(lst.TrimEnd());
                                                    if (sn != null)
                                                    {
                                                        var ds = db.PSTiepNhans.FirstOrDefault(p => p.MaTiepNhan == sn.Code);
                                                        if (ds != null)
                                                        {
                                                            ds.isDongBo = false;
                                                            res.StringError = res.StringError + sn.Code + ": " + sn.Error + ".\r\n";
                                                        }
                                                    }
                                                }
                                            }
                                            db.SubmitChanges();
                                            res.Result = false;
                                        }
                                    }
                                    else
                                    {
                                        res.Result = false;
                                        res.StringError = "Đồng bộ phiếu tiếp nhận lỗi - Kiểm tra kết nội mạng !\r\n";
                                    }
                                }
                                #endregion

                            }
                            if (String.IsNullOrEmpty(res.StringError))
                            {
                                res.Result = true;
                            }
                            else
                            {
                                res.Result = false;
                            }
                        }
                      

                    }
                }
                else
                {
                    res.Result = false;
                    res.StringError = "Đồng bộ phiếu tiếp nhận lỗi - Kiểm tra kết nội mạng hoặc tài khoản đồng bộ!\r\n";
                }
            }
            catch (Exception ex)
            {
                res.Result = false;
                res.StringError += DateTime.Now.ToString() + "Lỗi khi đồng bộ dữ liệu phiếu tiếp nhận \r\n " + ex.Message;

            }
            return res;
        }


    }
}