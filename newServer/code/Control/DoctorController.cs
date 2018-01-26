using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;
using System.Threading;
using System.Security.Cryptography;
using System.Data;
using System.Collections;
using newServer;

namespace newserver
{
    class DoctorController
    {
        private Socket socket;
        private Dao dao;
        private DoctorService doctorService;
        public DoctorController(Socket sockets)
        {
            socket = sockets;
            dao = new Dao();
            doctorService = new DoctorService();

        }
        public void startListening()
        {
            socket.On("web_get_regis_patient_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string id = (string)jobe.GetValue("id");
                    socket.Emit("web_get_regis_patient_reply", (doctorService.getRegRecordOFDoc(id)));
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_get_regis_patient_apply: \n");
                    Console.WriteLine(e);
                }

            });

            socket.On("web_change_regis_patient_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string doc_ID = (string)jobe.GetValue("doc_id");
                    string pat_ID = (string)jobe.GetValue("pat_id");
                    socket.Emit("web_change_regis_patient_reply", (doctorService.changeRegistPatient(doc_ID,pat_ID)));
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_change_regis_patient_apply: \n");
                    Console.WriteLine(e);
                }

            });

            socket.On("web_get_medical_patient_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string id = (string)jobe.GetValue("id");
                    socket.Emit("web_get_medical_patient_reply", (doctorService.getMedicalRecordOfDoc(id)));
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_get_medical_patient_apply: \n");
                    Console.WriteLine(e);
                }

            });

            /*socket.On("web_add_medical_patient_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string id = (string)jobe.GetValue("id");
                    string emaildoc = (string)jobe.GetValue("emailDoc");
                    string emailpat = (string)jobe.GetValue("emailPatient");
                    socket.Emit("web_add_medical_patient_reply", (doctorService.addMedicalRecordOfDoc(emaildoc, emailpat, id)));
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_add_medical_patient_apply: \n");
                    Console.WriteLine(e);
                }
            });*/

            socket.On("web_get_patient_info_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string doc_id = (string)jobe.GetValue("doc_id");
                    string pat_id = (string)jobe.GetValue("pat_id");
                    socket.Emit("web_get_patient_info_reply", (doctorService.getPatientInfoByID(doc_id, pat_id)));
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_get_patient_info_apply: \n");
                    Console.WriteLine(e);
                }

            });

            socket.On("web_get_history_patient_info_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string rec_id = (string)jobe.GetValue("id");
                    socket.Emit("web_get_history_patient_info_reply", (doctorService.getHistoryPatientInfoByID(rec_id)));
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_get_history_patient_info_apply: \n");
                    Console.WriteLine(e);
                }

            });

            socket.On("web_change_disease_diagnosis_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string doc_id = (string)jobe.GetValue("doc_id");
                    string pat_id = (string)jobe.GetValue("pat_id");
                    string feature = (string)jobe.GetValue("feature");
                    string suggest = (string)jobe.GetValue("suggest");
                    socket.Emit("web_change_disease_diagnosis_reply", (doctorService.changeMedicalRecord(doc_id, pat_id, feature, suggest)));
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_change_disease_diagnosis_apply: \n");
                    Console.WriteLine(e);
                }

            });

            socket.On("web_get_treatment_apply", (data) =>
            {
                try{
                    JObject jobe = (JObject)data;
                    string id = (string)jobe.GetValue("id");
                    string email = (string)jobe.GetValue("email");
                    socket.Emit("web_get_treatment_reply", doctorService.getMedicalSchemeOfPatient(email, id));
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_get_treatment_apply: \n");
                    Console.WriteLine(e);
                }
            });
            socket.On("web_get_drug_info_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string name = (string)jobe.GetValue("name");
                    var result = doctorService.getDrugInfoByName(name);
                    socket.Emit("web_get_drug_info_reply", result);
                }
                catch (Exception e)
                {
                    Console.WriteLine("webget_drug_info_apply: \n");
                    Console.WriteLine(e);
                }
            });
            socket.On("web_change_treatment_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    JObject result = new JObject();
                    bool flag = true;
                    string doc_ID = (string)jobe.GetValue("doc_id");
                    string pat_ID = (string)jobe.GetValue("pat_id");
                    JArray treats = (JArray)jobe.GetValue("treat");
                    if ((string)doctorService.delMedicalTreatment(doc_ID, pat_ID).GetValue("res") == "fail")
                    {
                        flag = false;
                    }
                    foreach (JObject treat in treats)
                    {
                        string stage = (string)treat.GetValue("stage");
                        string method = (string)treat.GetValue("method");
                        string info = (string)treat.GetValue("info");
                        if ((string)doctorService.addMedicalTreatment(doc_ID, pat_ID, stage, method, info).GetValue("res") == "fail")
                        {
                            flag = false;
                        }
                    }
                    result.Add(new JProperty("doc_id", doc_ID));
                    if (flag)
                    {
                        result.Add(new JProperty("res", "success"));
                    }
                    else
                    {
                        result.Add(new JProperty("res", "fail"));
                    }
                    socket.Emit("web_add_treatment_reply", result);
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_add_treatment_apply: \n");
                    Console.WriteLine(e);
                }

            });
            socket.On("web_search_substring_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string medicineName = (string)jobe.GetValue("medicine");
                    string id = (string)jobe.GetValue("id");
                    socket.Emit("web_search_substring_reply", doctorService.getMedicineBysubString(medicineName, id));
                    //socket.Emit("show_patient", doctorService.getMedicalSchemeOfPatient(patientID));
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_search_substring_apply: \n");
                    Console.WriteLine(e);
                }

            });
            /*socket.On("web_add_patient_exam_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string id = (string)jobe.GetValue("id");
                    string email = (string)jobe.GetValue("email");
                    string Examname = (string)jobe.GetValue("name");
                    socket.Emit("add_patient_exam_reply", doctorService.addMedicalExam(email, Examname, id));
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_add_patient_exam_apply: \n");
                    Console.WriteLine(e);
                }

            });
            socket.On("add_perscribe_apply", (data) =>
			{
				JObject jobe = (JObject)data;
				string id = (string)jobe.GetValue("id");
				string examID = (string)jobe.GetValue("exam_id");
				int ExamID = int.Parse(examID);
				string email = (string)jobe.GetValue("email");
				socket.Emit("add_Patient_Exam_reply", doctorService.addMedicalExam(email, ExamID, id));

			});*/
            /*socket.On("web_add_drug_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string id = (string)jobe.GetValue("id");
                    string email = (string)jobe.GetValue("email");
                    string drugname = (string)jobe.GetValue("drugname");
                    string quantity = (string)jobe.GetValue("quantity");
                    string eat_way = (string)jobe.GetValue("eat_way");
                    socket.Emit("web_add_drug_reply", doctorService.addPrescription(email, drugname, id, int.Parse(quantity), eat_way));
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_add_drug_apply: \n");
                    Console.WriteLine(e);
                }
            });*/
            socket.On("web_del_drug_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string id = (string)jobe.GetValue("id");
                    string email = (string)jobe.GetValue("email");
                    string drugname = (string)jobe.GetValue("drugname");
                    socket.Emit("web_del_drug_reply", doctorService.delPrescription(email, drugname, id));
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_del_drug_apply: \n");
                    Console.WriteLine(e);
                }

            });
            //门诊信息
            socket.On("web_get_chat_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string id = (string)jobe.GetValue("id");
                    socket.Emit("web_get_chat_reply", doctorService.getAllRoreInfo(id));
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_get_chat_apply: \n");
                    Console.WriteLine(e);
                }

            });
            socket.On("web_get_reglist_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string id = (string)jobe.GetValue("id");
                    socket.Emit("web_get_reglist_reply", (doctorService.getAllRegRecord(id)));
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_get_reglist_apply: \n");
                    Console.WriteLine(e);
                }
            });
            socket.On("web_add_chat_apply", (data) =>
            {
                try
                {
                    Console.WriteLine("apply");
                    JObject jobe = (JObject)data;
                    string id = (string)jobe.GetValue("id");
                    string content = (string)jobe.GetValue("content");
                    string patient_id = ""; string information = "";
                    int i = 1;
                    for (; content[i] != ' '; i++)
                    {
                        patient_id += content[i];
                    }
                    for (++i; i < content.Length; i++)
                    {
                        information += content[i];
                    }
                    socket.Emit("web_add_chat_reply", doctorService.addRoreInfo(id, int.Parse(patient_id), information));
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_add_chat_apply: \n");
                    Console.WriteLine(e);
                }

            });
            socket.On("web_add_reg_apply", (data) =>
            {
                try
                {
                    Console.WriteLine("apply");
                    JObject jobe = (JObject)data;
                    string id = (string)jobe.GetValue("id");
                    string email = (string)jobe.GetValue("email");
                    string dept = (string)jobe.GetValue("dept");
                    socket.Emit("web_add_reg_reply", doctorService.addRegRecord(id, email, dept));
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_add_reg_apply: \n");
                    Console.WriteLine(e);
                }
            });
            socket.On("web_add_prescribe_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    JObject result = new JObject();
                    bool flag = true;
                    string docId = (string)jobe.GetValue("doc_id");
                    string patId = (string)jobe.GetValue("pat_id");
                    JArray chosen = (JArray)jobe.GetValue("chosen");
                    MEDICAL_RECORD record = doctorService.getMedicalRecordByDocAndPat(docId, patId);
                    if (((record.TREAT_STATE == "开始诊断") || (record.TREAT_STATE == "检查完成")) && (record.DRUG_STATE == "未选择"))
                    {
                        foreach (JObject temp in chosen)
                        {
                            string name = (string)temp.GetValue("name");
                            string quan = (string)temp.GetValue("quan");
                            string times = (string)temp.GetValue("times");
                            string days = (string)temp.GetValue("days");
                            string eatway = "每日" + times + "次，用药" + days + "天";
                            if (((string)(doctorService.addPrescription(((int)record.ID).ToString(), name, int.Parse(quan), eatway)).GetValue("result")) == "fail")
                            {
                                flag = false;
                            }
                        }
                    }
                    else
                        flag = false;
                    if(!doctorService.changeMedicalRecordState(((int)record.ID).ToString(), record.TREAT_STATE, record.CLIN_STATE, record.INFU_STATE, "待付药费"))
                    {
                        flag = false;
                    }
                    result.Add(new JProperty("doc_id", docId));
                    if (flag)
                    {
                        result.Add(new JProperty("result", "success"));
                    }
                    else
                    {
                        result.Add(new JProperty("result", "fail"));
                    }
                    socket.Emit("web_add_prescribe_reply",result );
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_add_prescribe_apply: \n");
                    Console.WriteLine(e);
                }
            });
            socket.On("web_get_prescribe_apply", (data) =>
            {
                try
                {
                    Console.WriteLine("apply");
                    JObject jobe = (JObject)data;
                    string docID = (string)jobe.GetValue("doc_id");
                    string patID = (string)jobe.GetValue("pat_id");
                    socket.Emit("web_get_prescribe_reply", doctorService.getPrescribeByDocAndPat(docID, patID));
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_get_prescribe_apply: \n");
                    Console.WriteLine(e);
                }
            });
            socket.On("web_add_exam_item_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string doc_ID = (string)jobe.GetValue("doc_id");
                    string pat_ID = (string)jobe.GetValue("pat_id");
                    MEDICAL_RECORD record = doctorService.getMedicalRecordByDocAndPat(doc_ID, pat_ID);
                    JObject result = new JObject();
                    if (record.TREAT_STATE == "开始诊断")
                    {
                        int medicalID = (int)record.ID;
                        JArray res = new JArray();
                        JArray array = (JArray)jobe.GetValue("array");
                        foreach (JObject temp in array)
                        {
                            string name = (string)temp.GetValue("name");
                            res.Add(doctorService.addMedicalExam(medicalID, name));
                        }
                        doctorService.changeMedicalRecordState(doc_ID, pat_ID, "待检查");
                        result.Add(new JProperty("doc_id", doc_ID));
                        result.Add(new JProperty("result", res));
                        socket.Emit("web_add_exam_item_reply", result);
                    }
                    else
                    {
                        result.Add(new JProperty("result", "fail"));
                        socket.Emit("web_add_exam_item_reply", result);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_add_exam_item_apply: \n");
                    Console.WriteLine(e);
                }
            });
            socket.On("web_add_debridement_apply", (data) =>
            {
                try
                {
                    bool flag = true;
                    JObject jobe = (JObject)data;
                    string doc_ID = (string)jobe.GetValue("doc_id");
                    string pat_ID = (string)jobe.GetValue("pat_id");
                    string num = (string)jobe.GetValue("num");
                    string advice = (string)jobe.GetValue("advice");
                    string type = (string)jobe.GetValue("type");
                    MEDICAL_RECORD record = doctorService.getMedicalRecordByDocAndPat(doc_ID, pat_ID);
                    JObject result = new JObject();
                    if (((record.TREAT_STATE == "开始诊断") || (record.TREAT_STATE == "检查完成")) && (record.CLIN_STATE == "未选择"))
                    {
                        if (doctorService.addClinical((int)record.ID, type, int.Parse(num), advice))
                        {
                            flag = true;
                        }
                        else
                        {
                            flag = false;
                        }
                    }
                    else
                    {
                        flag = false;
                    }
                    if (!doctorService.changeMedicalRecordState(((int)record.ID).ToString(), record.TREAT_STATE, "已选择", record.INFU_STATE, record.DRUG_STATE))
                    {
                        flag = false;
                    }
                    result.Add(new JProperty("doc_id", doc_ID));
                    if (flag)
                    {
                        result.Add(new JProperty("result", "success"));
                    }
                    else
                    {
                        result.Add(new JProperty("result", "fail"));
                    }
                    socket.Emit("web_add_debridement_reply", result);
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_add_debridement_apply: \n");
                    Console.WriteLine(e);
                }
            });
            socket.On("web_add_infusion_injection_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    JObject result = new JObject();
                    bool flag = true;
                    string docId = (string)jobe.GetValue("doc_id");
                    string patId = (string)jobe.GetValue("pat_id");
                    string type = (string)jobe.GetValue("type");
                    JArray chosen = (JArray)jobe.GetValue("chosen");
                    MEDICAL_RECORD record = doctorService.getMedicalRecordByDocAndPat(docId, patId);
                    if (((record.TREAT_STATE == "开始诊断") || (record.TREAT_STATE == "检查完成")) && (record.INFU_STATE == "未选择" || record.INFU_STATE =="已选输液") && type == "注射")
                    {
                        foreach (JObject temp in chosen)
                        {
                            string name = (string)temp.GetValue("name");
                            int quan = int.Parse((string)temp.GetValue("quan"));
                            if (!(doctorService.addInfusion((int)record.ID, name, type, quan)))
                            {
                                flag = false;
                            }
                        }
                    }
                    else if (((record.TREAT_STATE == "开始诊断") || (record.TREAT_STATE == "检查完成")) && (record.INFU_STATE == "未选择" || record.INFU_STATE == "已选注射") && type == "输液")
                    {
                        foreach (JObject temp in chosen)
                        {
                            string name = (string)temp.GetValue("name");
                            int quan = int.Parse((string)temp.GetValue("quan"));
                            if (!(doctorService.addInfusion((int)record.ID, name, type, quan)))
                            {
                                flag = false;
                            }
                        }
                    }
                    else
                        flag = false;
                    if (type == "输液" && record.INFU_STATE == "未选择")
                    {
                        doctorService.changeMedicalRecordState(((int)record.ID).ToString(), record.TREAT_STATE, record.CLIN_STATE, "已选输液", record.DRUG_STATE);
                    }
                    else if (type == "注射" && record.INFU_STATE == "未选择")
                    {
                        doctorService.changeMedicalRecordState(((int)record.ID).ToString(), record.TREAT_STATE, record.CLIN_STATE, "已选注射", record.DRUG_STATE);
                    }
                    else if (type == "输液" && record.INFU_STATE == "已选注射")
                    {
                        doctorService.changeMedicalRecordState(((int)record.ID).ToString(), record.TREAT_STATE, record.CLIN_STATE, "已选择", record.DRUG_STATE);
                    }
                    else if (type == "注射" && record.INFU_STATE == "已选输液")
                    {
                        doctorService.changeMedicalRecordState(((int)record.ID).ToString(), record.TREAT_STATE, record.CLIN_STATE, "已选择", record.DRUG_STATE);
                    }
                    else
                    {
                        flag = false;
                    }
                    result.Add(new JProperty("doc_id", docId));
                    if (flag)
                    {
                        result.Add(new JProperty("result", "success"));
                    }
                    else
                    {
                        result.Add(new JProperty("result", "fail"));
                    }
                    socket.Emit("web_add_infusion_injection_reply", result);
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_add_infusion_injection_apply: \n");
                    Console.WriteLine(e);
                }
            });
            socket.On("web_get_presentation_apply", (data) =>
            {
                try
                {
                    Console.WriteLine("apply");
                    JObject jobe = (JObject)data;
                    string docID = (string)jobe.GetValue("doc_id");
                    string patID = (string)jobe.GetValue("pat_id");
                    string itemID = (string)jobe.GetValue("item_id");
                    socket.Emit("web_get_presentation_reply", doctorService.getReport(docID, patID, itemID));
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_get_presentation_apply: \n");
                    Console.WriteLine(e);
                }
            });
            socket.On("web_patient_finish_apply", (data) =>
            {
                try
                {
                    Console.WriteLine("apply");
                    JObject jobe = (JObject)data;
                    string docID = (string)jobe.GetValue("doc_id");
                    string patID = (string)jobe.GetValue("pat_id");
                    JObject result = new JObject();
                    MEDICAL_RECORD record = (MEDICAL_RECORD)(dao.getMedicalRecordByDoctorAndPatient(int.Parse(docID), int.Parse(patID))[0]);
                    result.Add(new JProperty("doc_id", docID));
                    if (record.TREAT_STATE == "开始诊断" || record.TREAT_STATE == "检查完成")
                    {
                        if (doctorService.changeMedicalRecordState(((int)record.ID).ToString(), "诊断完成", record.CLIN_STATE, record.INFU_STATE, record.DRUG_STATE))
                        {
                            result.Add(new JProperty("result", "success"));
                        }
                        else
                        {
                            result.Add(new JProperty("result", "false"));
                        }
                    }
                    else
                        result.Add(new JProperty("result", "false"));
                    socket.Emit("web_patient_finish_reply", result);
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_patient_finish_apply: \n");
                    Console.WriteLine(e);
                }
            });
            socket.On("web_get_medical_bill_apply", (data) =>
            {
                try
                {
                    Console.WriteLine("apply");
                    JObject jobe = (JObject)data;
                    string docID = (string)jobe.GetValue("doc_id");
                    string patID = (string)jobe.GetValue("pat_id");
                    MEDICAL_RECORD record = (MEDICAL_RECORD)(dao.getMedicalRecordByDoctorAndPatient(int.Parse(docID), int.Parse(patID))[0]);
                    socket.Emit("web_get_medical_bill_reply", doctorService.getMedicalBill(docID, patID));
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_get_medical_bill_apply: \n");
                    Console.WriteLine(e);
                }
            });
            //检查医生接口
            socket.On("web_totalpatient_apply", (data) =>
            {
                try

                {
                    JObject jobe = (JObject)data;
                    string id = (string)jobe.GetValue("ID");
                    socket.Emit("web_totalpatient_reply", (doctorService.getCheckPatientInfo(id)));
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_totalpatient_apply: \n");
                    Console.WriteLine(e);
                }
            });
            socket.On("web_selectpatient_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string checkID = (string)jobe.GetValue("checkID");
                    string type = (string)jobe.GetValue("type");
                    string patID = (string)jobe.GetValue("patientID");
                    string doctorID = (string)jobe.GetValue("doctorID");
                    socket.Emit("web_selectpatient_reply", (doctorService.changeMedicalExamState(doctorID, patID, "检查中", "未完成", checkID, type)));
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_selectpatient_apply: \n");
                    Console.WriteLine(e);
                }
            });
            socket.On("web_mypatient_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string id = (string)jobe.GetValue("ID");
                    socket.Emit("web_mypatient_reply", (doctorService.getMedicalExamBycheckID(id)));
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_mypatient_apply: \n");
                    Console.WriteLine(e);
                }
            });
            socket.On("web_sendreport_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string checkID = (string)jobe.GetValue("checkID");
                    string type = (string)jobe.GetValue("type");
                    string patID = (string)jobe.GetValue("patientID");
                    string result = (string)jobe.GetValue("result");
                    string doctorID = (string)jobe.GetValue("doctorID");
                    socket.Emit("web_sendreport_reply", (doctorService.checkMedicalExamState(doctorID, patID, result, checkID, type)));
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_sendreport_apply: \n");
                    Console.WriteLine(e);
                }
            });
            socket.On("web_checkresult_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string type = (string)jobe.GetValue("type");
                    string patID = (string)jobe.GetValue("patientID");
                    string doctorID = (string)jobe.GetValue("doctorID");
                    socket.Emit("web_checkresult_reply", (doctorService.getMedicalExamReport(doctorID, patID, type)));
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_checkresult_apply: \n");
                    Console.WriteLine(e);
                }
            });
            socket.On("web_checkreport_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string type = (string)jobe.GetValue("type");
                    string patID = (string)jobe.GetValue("patientID");
                    string doctorID = (string)jobe.GetValue("doctorID");
                    socket.Emit("web_checkreport_reply", (doctorService.getMedicalExamReportInfo(doctorID, patID, type)));
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_checkreport_apply: \n");
                    Console.WriteLine(e);
                }
            });
            socket.On("web_finishcheck_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string checkID = (string)jobe.GetValue("checkID");
                    string type = (string)jobe.GetValue("type");
                    string patID = (string)jobe.GetValue("patientID");
                    string doctorID = (string)jobe.GetValue("doctorID");
                    socket.Emit("web_finishcheck_reply", (doctorService.finishMedicalExam(doctorID, patID, checkID, type)));
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_finishcheck_apply: \n");
                    Console.WriteLine(e);
                }
            });
            socket.On("web_accountinfo_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string checkID = (string)jobe.GetValue("ID");
                    socket.Emit("web_accountinfo_reply", (doctorService.getDoctorInfoByID(checkID)));
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_accountinfo_apply: \n");
                    Console.WriteLine(e);
                }
            });
            //护士接口
            socket.On("web_nurse_search_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string name = ((string)jobe.GetValue("name")) == "" ? null : ((string)jobe.GetValue("name"));
                    string department = (string)jobe.GetValue("department");
                    string medicalnum = ((string)jobe.GetValue("medicalnum")) == "" ? null : ((string)jobe.GetValue("medicalnum"));
                    socket.Emit("web_nurse_search_reply", (doctorService.getMedicalRecordByDeptAndPat(name, department, medicalnum)));
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_nurse_search_apply: \n");
                    Console.WriteLine(e);
                }
            });
            socket.On("web_patient_data_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string medicalnum = ((string)jobe.GetValue("medicalnum")) == "" ? null : ((string)jobe.GetValue("medicalnum"));
                    socket.Emit("web_patient_data_reply", (doctorService.getNursePatientInfoByID(medicalnum)));
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_patient_data_apply: \n");
                    Console.WriteLine(e);
                }
            });
            socket.On("web_patient_data_change_apply", (data) =>
            {
                try
                {
                    Dao newdao = new Dao();
                    JObject jobe = (JObject)data;
                    string eventid = (string)jobe.GetValue("eventid");
                    string medicalnum = "";
                    string itemid = "";
                    string drugid = "";
                    bool flag1 = true;
                    bool flag2 = true;
                    for(int i = 0; i < eventid.Length; i++)
                    {
                        if (eventid[i] == '_')
                        {
                            if (flag1 == true)
                                flag1 = false;
                            else
                                flag2 = false;
                        }
                        else if ((flag1 == true) && (flag2 == true))
                            medicalnum += eventid[i];
                        else if ((flag1 == false) && (flag2 == true))
                            itemid += eventid[i];
                        else
                            drugid += eventid[i];
                    }
                    if(itemid == "3" || itemid == "4")
                    {
                        CLINICAL clinical = (CLINICAL)(newdao.getClinical(int.Parse(medicalnum), int.Parse(itemid), null)[0]);
                        newdao.changeClinical(int.Parse(medicalnum), int.Parse(itemid), (int)clinical.NUMBERS, clinical.ADVICE, (int)clinical.EXPENSE, "全部完成");
                    }
                    else if (itemid == "1" || itemid == "2")
                    {
                        INFUSION infusion = (INFUSION)(newdao.getInfusion(int.Parse(medicalnum), int.Parse(itemid), int.Parse(drugid), null)[0]);
                        newdao.changeInfusion(int.Parse(medicalnum), int.Parse(itemid), int.Parse(drugid), (int)infusion.EXPENSE, "全部完成", (int)infusion.NUMBERS);
                    }
                    socket.Emit("web_patient_data_change_reply", (doctorService.getNursePatientInfoByID(medicalnum, eventid)));
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_patient_data_change_apply: \n");
                    Console.WriteLine(e);
                }
            });
            //病人
            socket.On("web_make_reservation_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string id = (string)jobe.GetValue("id");
                    string patient = (string)jobe.GetValue("patient");
                    string schedule = (string)jobe.GetValue("schedule");
                    DateTime datetime = (DateTime)jobe.GetValue("datetime");
                    socket.Emit("web_make_reservation_reply", doctorService.addOppointment(id, patient, schedule, datetime));
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_make_reservation_apply: \n");
                    Console.WriteLine(e);
                }

            }); 
            socket.On("web_get_schedule_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string id = (string)jobe.GetValue("id");
                    string doctor = (string)jobe.GetValue("doctor");
                    socket.Emit("web_get_schedule_reply", doctorService.getSchedualByDoctor(id, doctor));
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_get_schedule_apply: \n");
                    Console.WriteLine(e);
                }

            }); 
            socket.On("web_get_reservation_list_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string id = (string)jobe.GetValue("id");
                    string patient = (string)jobe.GetValue("patient");
                    socket.Emit("web_get_reservation_list_reply", doctorService.getOppointmentByPat(id, patient));
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_get_reservation_list_apply: \n");
                    Console.WriteLine(e);
                }

            });
            socket.On("web_get_medical_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string id = (string)jobe.GetValue("id");
                    string patient = (string)jobe.GetValue("patient");
                    socket.Emit("web_get_medical_reply", doctorService.getMedicalRecordByPat(id, patient));
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_get_medical_apply: \n");
                    Console.WriteLine(e);
                }

            });
            socket.On("web_get_medical_detail_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string id = (string)jobe.GetValue("id");
                    string medicalId = (string)jobe.GetValue("medicalId");
                    socket.Emit("web_get_medical_detail_reply", doctorService.getMedicalDetailByID(id, medicalId));
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_get_medical_detail_apply: \n");
                    Console.WriteLine(e);
                }
            
            });
            socket.On("web_get_record_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string id = (string)jobe.GetValue("id");
                    string patient = (string)jobe.GetValue("patient");
                    socket.Emit("web_get_record_reply", doctorService.getHistoryByPat(id, patient));
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_get_record_apply: \n");
                    Console.WriteLine(e);
                }

            });
            socket.On("web_patient_get_chat_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string id = (string)jobe.GetValue("id");
                    string patient = (string)jobe.GetValue("patient");
                    socket.Emit("web_patient_get_chat_reply", doctorService.getinfomation(id, patient));
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_patient_get_record_apply: \n");
                    Console.WriteLine(e);
                }

            });
            socket.On("web_patient_send_chat_apply", (data) =>
            {
                try
                {
                    Console.WriteLine("apply");
                    JObject jobe = (JObject)data;
                    string id = (string)jobe.GetValue("id");
                    string content = (string)jobe.GetValue("message");
                    string patient_id = ""; string information = "";
                    int i = 0;
                    for (; content[i] != ' '; i++)
                    {
                        patient_id += content[i];
                    }
                    for (++i; i < content.Length; i++)
                    {
                        information += content[i];
                    }
                    socket.Emit("web_patient_send_chat_reply", doctorService.addPatientRoreInfo(id, int.Parse(patient_id), information));
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_patient_send_chat_apply: \n");
                    Console.WriteLine(e);
                }

            });
            //收费台
            socket.On("web_search_patient_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string id = (string)jobe.GetValue("id");
                    string patient = (string)jobe.GetValue("patient");
                    socket.Emit("web_search_patient_reply", doctorService.getPatientInfoByName(id, patient));
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_search_patient_apply: \n");
                    Console.WriteLine(e);
                }

            });
            socket.On("web_get_bills_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string id = (string)jobe.GetValue("id");
                    string patient = (string)jobe.GetValue("patient");
                    socket.Emit("web_get_bills_reply", doctorService.payBillByID(id, patient));
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_get_bills_apply: \n");
                    Console.WriteLine(e);
                }

            });
            socket.On("web_pay_bills_apply", (data) =>
            {
                Dao vwx = new Dao();
                try
                {
                    bool flag = true;
                    JObject jobe = (JObject)data;
                    JObject result = new JObject();
                    string id = (string)jobe.GetValue("id");
                    string pat_id = (string)jobe.GetValue("patient");
                    JArray reg_id = (JArray)jobe.GetValue("reg_id");
                    JArray exam_id = (JArray)jobe.GetValue("exam_id");
                    JArray infusion_id = (JArray)jobe.GetValue("infusion_id");
                    JArray clinical_id = (JArray)jobe.GetValue("clinical_id");
                    JArray drug_id = (JArray)jobe.GetValue("drug_id");
                    int money = 0;
                    int patID = int.Parse(pat_id);
                    PATIENT patient = (PATIENT)(vwx.getPatientById(patID)[0]);
                    foreach(JObject temp in reg_id)
                    {
                        string regID = (string)temp.GetValue("id");
                        REGISTRATION_RECORD record = vwx.getRegRecordByID(int.Parse(regID));
                        money += (int)record.EXPENSE;
                    }
                    foreach (JObject temp in exam_id)
                    {
                        string recID = (string)temp.GetValue("rec_id");
                        string itemID = (string)temp.GetValue("item_id");
                        if ((vwx.getMedicalExams(int.Parse(recID), int.Parse(itemID), "未缴费", null, null, null)).Count > 0)
                        {
                            MEDICAL_EXAM exam = (MEDICAL_EXAM)(vwx.getMedicalExams(int.Parse(recID), int.Parse(itemID), "未缴费", null, null, null)[0]);
                            money += (int)exam.EXAM_ITEM.EXPENSE;
                        }
                    }
                    foreach (JObject temp in infusion_id)
                    {
                        string recID = (string)temp.GetValue("rec_id");
                        string itemID = (string)temp.GetValue("item_id");
                        string drugID = (string)temp.GetValue("drug_id");
                        if ((vwx.getInfusion(int.Parse(recID), int.Parse(itemID), int.Parse(drugID), "未缴费")).Count > 0)
                        {
                            INFUSION infusion = (INFUSION)(vwx.getInfusion(int.Parse(recID), int.Parse(itemID), int.Parse(drugID), "未缴费")[0]);
                            money += (int)infusion.OPERATION.PRICE;
                        }
                    }
                    for(int i = 0; i < drug_id.Count; i++)
                    {
                        JObject temp = (JObject)drug_id[i];
                        string recID = (string)temp.GetValue("rec_id");
                        PRESCRIBE drug = (PRESCRIBE)(vwx.getPrescribeByMedicalRecord(int.Parse(recID))[i]);
                        money += (int)drug.DRUG_INVENTORY.PRICE;
                    }
                    if ((int)patient.COUNT >= money)
                    {
                        foreach (JObject temp in reg_id)
                        {
                            string regID = (string)temp.GetValue("id");
                            if (!doctorService.changeRegistRecord(regID, "未就诊"))
                            {
                                flag = false;
                            }
                        }
                        foreach (JObject temp in exam_id)
                        {
                            string recID = (string)temp.GetValue("rec_id");
                            string itemID = (string)temp.GetValue("item_id");
                            if (!doctorService.changeMediacalExam(recID, itemID, "未完成"))
                            {
                                flag = false;
                            }
                        }
                        foreach (JObject temp in infusion_id)
                        {
                            string recID = (string)temp.GetValue("rec_id");
                            string itemID = (string)temp.GetValue("item_id");
                            string drugID = (string)temp.GetValue("drug_id");
                            if (!doctorService.changeInfusion(recID, itemID, drugID, "待处理"))
                            {
                                flag = false;
                            }
                        }
                        foreach (JObject temp in clinical_id)
                        {
                            string recID = (string)temp.GetValue("rec_id");
                            string itemID = (string)temp.GetValue("item_id");
                            if (!doctorService.changeClinical(recID, itemID, "待处理"))
                            {
                                flag = false;
                            }
                        }
                        foreach (JObject temp in drug_id)
                        {
                            string recID = (string)temp.GetValue("rec_id");
                            MEDICAL_RECORD record = vwx.getMedicalRecordByID(int.Parse(recID));
                            if (!doctorService.changeMedicalRecordState(recID, record.TREAT_STATE, record.CLIN_STATE, record.INFU_STATE, "取药中"))
                            {
                                flag = false;
                            }
                        }
                        vwx.newchangePatientCount(patID, ((int)patient.COUNT - money));
                    }
                    else
                    {
                        flag = false;
                    }
                    result.Add(new JProperty("id", id));
                    if (flag)
                    {
                        result.Add(new JProperty("result", "SUCCESS"));
                        Console.WriteLine("success");
                    }
                    else
                    {
                        result.Add(new JProperty("result", "FAIL"));
                        Console.WriteLine("fail");
                    }
                    socket.Emit("web_pay_bills_reply", result);
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_pay_bills_apply: \n");
                    Console.WriteLine(e);
                }

            });
            socket.On("web_get_balance_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string id = (string)jobe.GetValue("id");
                    string patient = (string)jobe.GetValue("patient");
                    socket.Emit("web_get_balance_reply", (new DoctorService()).getPatientCount(id, patient));
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_get_balance_apply: \n");
                    Console.WriteLine(e);
                }

            });
            socket.On("web_top_up_balance_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string id = (string)jobe.GetValue("id");
                    string patient = (string)jobe.GetValue("patient");
                    string amount = (string)jobe.GetValue("amount");
                    socket.Emit("web_top_up_balance_reply", doctorService.changePatientCount(id, patient, amount));
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_top_up_balance_apply: \n");
                    Console.WriteLine(e);
                }

            });
        }
    }
}
