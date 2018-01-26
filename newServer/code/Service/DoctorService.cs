using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using newServer;
using System.Threading;
namespace newserver
{
    class DoctorService
    {
        private Dao dao;
        public DoctorService()
        {
            dao = new Dao();
        }
        public JObject getRegRecordOFDoc(string id)
        {
            int docID = int.Parse(id);
            ArrayList schedulenumbers = dao.getScheduleNumberOfDoctor(docID);
            JArray result = new JArray();
            for (int i = 0; i < schedulenumbers.Count; i++)
            {
                //在今天以后？
                DateTime begin = dao.getScheduleByID((int)schedulenumbers[i]).BEGIN_TIME;
                DateTime end = dao.getScheduleByID((int)schedulenumbers[i]).END_TIME;
                if ((DateTime.Now.CompareTo(begin) > 0) && (DateTime.Now.CompareTo(end) < 0))
                {
                    var temp = dao.getPatientWithSchedule((int)schedulenumbers[i]);
                    foreach (REGISTRATION_RECORD reg_record in temp)
                    {
                        DateTime ptime = reg_record.TIME;
                        string newtime = ptime.Year.ToString() + "年" + ptime.Month.ToString() + "月" + ptime.Day.ToString()
                            + "日    ";
                        if (ptime.Hour < 10)
                            newtime += ("0" + ptime.Hour.ToString() + ":");
                        else
                            newtime += (ptime.Hour.ToString() + ":");
                        if(ptime.Minute < 10)
                            newtime += ("0" + ptime.Minute.ToString());
                        else
                            newtime += (ptime.Minute.ToString());
                        if (((ptime.CompareTo(begin) > 0) && (ptime.CompareTo(end) < 0)) && reg_record.STATE == "未就诊")
                        {
                            JObject newone = new JObject();
                            newone.Add(new JProperty("id", reg_record.PATIENT.ID));
                            newone.Add(new JProperty("name", reg_record.PATIENT.NAME));
                            newone.Add(new JProperty("time", newtime));
                            result.Add(newone);
                        }
                    }
                }
            }
            JObject Result = new JObject();
            Result.Add(new JProperty("doc_id", id));
            Result.Add(new JProperty("result", result));
            return Result;
        }
        public JObject getMedicalRecordOfDoc(string id)
        {
            int docID = int.Parse(id);
            ArrayList medicalRecords = dao.getMedicalRecordByDoctor(docID);
            JArray res = new JArray();
            for (int i = 0; i < medicalRecords.Count; i++)
            {
                //在今天以后？
                if (((MEDICAL_RECORD)medicalRecords[i]).TREAT_STATE != "诊断完成")
                {
                    JObject newone = new JObject();
                    newone.Add(new JProperty("id", ((MEDICAL_RECORD)medicalRecords[i]).PATIENT.ID));
                    newone.Add(new JProperty("name", ((MEDICAL_RECORD)medicalRecords[i]).PATIENT.NAME));
                    res.Add(newone);
                }
            }
            JObject Result = new JObject();
            Result.Add(new JProperty("doc_id", id));
            Result.Add(new JProperty("result", res));
            return Result;
        }
        public MEDICAL_RECORD getMedicalRecordByDocAndPat(string doc_id, string pat_id)
        {
            int docID = int.Parse(doc_id);
            int patID = int.Parse(pat_id);
            ArrayList medicalRecords = dao.getMedicalRecordByDoctorAndPatient(docID, patID);
            return (MEDICAL_RECORD)medicalRecords[0];
        }
        public JObject getMedicalRecordByPat(string id, string email)
        {
            int patID = dao.getPatientIdByEmail(email);
            ArrayList medicalRecords = dao.getMedicalRecordByDoctorAndPatient(null, patID);
            JArray res = new JArray();
            JObject result = new JObject();
            foreach (MEDICAL_RECORD medicalRecord in medicalRecords)
            {
                DateTime time = medicalRecord.TIME;
                string datetime = time.Year.ToString() + "-" + time.Month.ToString() + "-" + time.Day.ToString()
                    + " " + time.Hour.ToString() + ":" + time.Minute.ToString();
                JObject temp = new JObject();
                temp.Add(new JProperty("id", medicalRecord.ID));
                temp.Add(new JProperty("status", medicalRecord.TREAT_STATE));
                temp.Add(new JProperty("datetime", datetime));
                temp.Add(new JProperty("doctorEmail", medicalRecord.DOCTOR.E_MAIL));
                temp.Add(new JProperty("doctorName", medicalRecord.DOCTOR.NAME));
                res.Add(temp);
            }
            result.Add(new JProperty("id", id));
            result.Add(new JProperty("result", res));
            return result;
        }
        public JObject getMedicalRecordByDeptAndPat(string name, string deptname, string medicalnum)
        {
            int? ID = dao.getPatientIDByName(name);
            int? recID = null;
            if (medicalnum != null)
                recID = int.Parse(medicalnum);
            ArrayList medicalRecords = dao.getMedicalRecordsByID(recID, ID);
            JArray PatientList = new JArray();
            JObject result = new JObject();
            JObject Object = new JObject();
            foreach(MEDICAL_RECORD temp in medicalRecords)
            {
                JObject newone = new JObject();
                if ((temp.TREAT_STATE != "诊断完成") && (temp.PATIENT.NAME == name || name == null) && (temp.DOCTOR.DEPT_NAME == deptname || deptname == null))
                {
                    newone.Add("name", temp.PATIENT.NAME);
                    newone.Add("department", temp.DOCTOR.DEPT_NAME);
                    newone.Add("medicalnum", ((int)temp.ID).ToString());
                    PatientList.Add(newone);
                }
            }
            result.Add(new JProperty("PatientList", PatientList));
            return result;
        }
        public JObject getMedicalDetailByID(string id, string medicalid)
        {
            int medicalID = int.Parse(medicalid);
            MEDICAL_RECORD medicalRecord = dao.getMedicalRecordByID(medicalID);
            ArrayList medicines = dao.getPrescribeByMedicalRecord(medicalID);
            ArrayList exams = dao.getMedicalExams(medicalID, null, null, null, null, null);
            JArray medicine = new JArray();
            JArray exam = new JArray();
            JObject result = new JObject();
            DateTime time = medicalRecord.TIME;
            string datetime = time.Year.ToString() + "-" + time.Month.ToString() + "-" + time.Day.ToString()
                + " " + time.Hour.ToString() + ":" + time.Minute.ToString();
            foreach (PRESCRIBE Medicine in medicines)
            {
                JObject temp = new JObject();
                temp.Add(new JProperty("name", Medicine.DRUG_INVENTORY.DRUG.NAME));
                temp.Add(new JProperty("amount", ((int)Medicine.QUANTITY).ToString()));
                temp.Add(new JProperty("datetime", datetime));
                temp.Add(new JProperty("doctorEmail", medicalRecord.DOCTOR.E_MAIL));
                temp.Add(new JProperty("doctorName", medicalRecord.DOCTOR.NAME));
                medicine.Add(temp);
            }
            foreach (MEDICAL_EXAM Exam in exams)
            {
                DateTime newtime = Exam.TIME.Value;
                string newdatetime = newtime.Year.ToString() + "-" + newtime.Month.ToString() + "-" + newtime.Day.ToString()
                    + " " + newtime.Hour.ToString() + ":" + newtime.Minute.ToString();
                JObject temp = new JObject();
                temp.Add(new JProperty("name", Exam.EXAM_ITEM.NAME));
                temp.Add(new JProperty("status", Exam.STATE));
                temp.Add(new JProperty("datetime", newdatetime));
                if(Exam.DOCTOR != null)
                    temp.Add(new JProperty("doctorName", Exam.DOCTOR.NAME));
                else
                    temp.Add(new JProperty("doctorName", ""));
                exam.Add(temp);
            }
            result.Add(new JProperty("id", id));
            result.Add(new JProperty("datetime", datetime));
            result.Add(new JProperty("doctorEmail", medicalRecord.DOCTOR.E_MAIL));
            result.Add(new JProperty("doctorName", medicalRecord.DOCTOR.NAME));
            result.Add(new JProperty("medicine", medicine));
            result.Add(new JProperty("exam", exam));
            result.Add(new JProperty("disease", medicalRecord.DISEASE));
            result.Add(new JProperty("description", medicalRecord.DESCRIPTION));
            result.Add(new JProperty("diagnosis", medicalRecord.DIAGNOSIS));
            return result;
        }
        public JObject getOppointmentByPat(string id, string email)
        {
            ArrayList oppos = dao.getOppointmentByPat(email);
            JObject result = new JObject();
            JArray res = new JArray();
            foreach (OPPOINTMENT oppo in oppos)
            {
                DateTime time = oppo.TIME;
                string datetime = time.Year.ToString() + "-" + time.Month.ToString() + "-" + time.Day.ToString()
                    + " " + time.Hour.ToString() + ":" + time.Minute.ToString();
                JObject temp = new JObject();
                temp.Add(new JProperty("datetime", datetime));
                temp.Add(new JProperty("doctorEmail", oppo.SCHEDULE.DOCTOR.E_MAIL));
                temp.Add(new JProperty("doctorName", oppo.SCHEDULE.DOCTOR.NAME));
                res.Add(temp);
            }
            result.Add(new JProperty("id", id));
            result.Add(new JProperty("result", res));
            return result;
        }
        public JObject getPatientInfoByID(string doc_id, string pat_id)
        {
            int docID = int.Parse(doc_id);
            int patID = int.Parse(pat_id);
            ArrayList patients = dao.getPatientById(patID);
            ArrayList medicalRecords = dao.getMedicalRecordByDoctorAndPatient(docID,patID);
            ArrayList patmedicalrecords = dao.getMedicalRecordsOfPatient(patID);
            JObject basicInfo = new JObject();
            JObject diseaseDiag = new JObject();
            JArray treatSche = new JArray();
            JObject recordCover = new JObject();
            JArray currentCheck = new JArray();
            JArray history = new JArray();
            for(int i = 0; i < patients.Count; i++)
            {
                DateTime time = ((PATIENT)patients[i]).BIRTHDAY;
                string newtime = time.Year.ToString() + "年" + time.Month.ToString() + "月" + time.Day.ToString()
                    + "日"; 
                basicInfo.Add("name", ((PATIENT)patients[i]).NAME);
                basicInfo.Add("age", ((PATIENT)patients[i]).AGE);
                basicInfo.Add("gen", ((PATIENT)patients[i]).GENDER);
                recordCover.Add("name", ((PATIENT)patients[i]).NAME);
                recordCover.Add("cardID", ((PATIENT)patients[i]).IDENTITY);
                recordCover.Add("gen", ((PATIENT)patients[i]).GENDER);
                recordCover.Add("date", newtime);
                recordCover.Add("age", ((PATIENT)patients[i]).AGE);
                recordCover.Add("tel", ((PATIENT)patients[i]).E_MAIL);
            }
            for (int i = 0; i < medicalRecords.Count; i++)
            {
                DateTime tim = ((MEDICAL_RECORD)medicalRecords[i]).TIME;
                string newtim = tim.Year.ToString() + "年" + tim.Month.ToString() + "月" + tim.Day.ToString()
                        + "日";
                diseaseDiag.Add("date", newtim);
                diseaseDiag.Add("doctor", ((MEDICAL_RECORD)medicalRecords[i]).DOCTOR.NAME);
                diseaseDiag.Add("history", ((MEDICAL_RECORD)medicalRecords[i]).DESCRIPTION);
                diseaseDiag.Add("feature", ((MEDICAL_RECORD)medicalRecords[i]).DISEASE);
                diseaseDiag.Add("suggest", ((MEDICAL_RECORD)medicalRecords[i]).DIAGNOSIS);
                ArrayList medicalTreatement = dao.getMedicalTreatmentByID((int)((MEDICAL_RECORD)medicalRecords[i]).ID);
                for (int j = 0; j < medicalTreatement.Count; j++)
                {
                    //在今天以后？
                    JObject newone = new JObject();
                    newone.Add(new JProperty("stage", ((MEDICAL_TREATEMENT)medicalTreatement[j]).SEQUENCE));
                    newone.Add(new JProperty("method", ((MEDICAL_TREATEMENT)medicalTreatement[j]).TITLE));
                    newone.Add(new JProperty("info", ((MEDICAL_TREATEMENT)medicalTreatement[j]).DESCRIPTION));
                    treatSche.Add(newone);
                }
                ArrayList medicalExam = dao.getMedicalExams((int)((MEDICAL_RECORD)medicalRecords[i]).ID, null, null, null, null, null);
                for (int j = 0; j < medicalExam.Count; j++)
                {
                    //在今天以后？
                    JObject newone = new JObject();
                    DateTime time = ((MEDICAL_EXAM)medicalExam[j]).TIME.Value;
                    string newtime = time.Year.ToString() + "年" + time.Month.ToString() + "月" + time.Day.ToString()
                            + "日    ";
                    if (time.Hour < 10)
                        newtime += ("0" + time.Hour.ToString() + ":");
                    else
                        newtime += (time.Hour.ToString() + ":");
                    if (time.Minute < 10)
                        newtime += ("0" + time.Minute.ToString());
                    else
                        newtime += (time.Minute.ToString());
                    EXAM_ITEM item = ((MEDICAL_EXAM)medicalExam[j]).EXAM_ITEM;
                    string address = ((int)item.BUILDING_ID).ToString() + "号楼" + ((int)item.ROOM_ID).ToString() + "室";
                    newone.Add(new JProperty("id", ((MEDICAL_EXAM)medicalExam[j]).ITEM_ID));
                    newone.Add(new JProperty("name", ((MEDICAL_EXAM)medicalExam[j]).EXAM_ITEM.NAME));
                    newone.Add(new JProperty("expense", ((MEDICAL_EXAM)medicalExam[j]).EXAM_ITEM.EXPENSE));
                    newone.Add(new JProperty("state", ((MEDICAL_EXAM)medicalExam[j]).STATE));
                    newone.Add(new JProperty("location", address));
                    newone.Add(new JProperty("time", newtime));
                    currentCheck.Add(newone);
                }
            }
            for (int i = 0; i < patmedicalrecords.Count; i++)
            {
                DateTime time = ((MEDICAL_RECORD)patmedicalrecords[i]).TIME;
                string newtime = time.Year.ToString() + "年" + time.Month.ToString() + "月" + time.Day.ToString()
                    + "日";
                JObject newone = new JObject();
                newone.Add("date", newtime);
                newone.Add("info", ((MEDICAL_RECORD)patmedicalrecords[i]).DISEASE);
                newone.Add("id", ((MEDICAL_RECORD)patmedicalrecords[i]).ID);
                history.Add(newone);
            }
            JObject Result = new JObject();
            Result.Add(new JProperty("doc_id", doc_id));
            Result.Add(new JProperty("basicInfo", basicInfo));
            Result.Add(new JProperty("diseaseDiag", diseaseDiag));
            Result.Add(new JProperty("treatSche", treatSche));
            Result.Add(new JProperty("recordCover", recordCover));
            Result.Add(new JProperty("currentCheck", currentCheck));
            Result.Add(new JProperty("history", history));
            return Result;
        }
        public JObject getNursePatientInfoByID(string rec_id)
        {
            Dao newdao = new Dao();
            int recID = int.Parse(rec_id);
            MEDICAL_RECORD medicalrecord = newdao.getMedicalRecordByID(recID);
            PATIENT patient = medicalrecord.PATIENT;
            JObject basedata = new JObject();
            JObject medicaldata = new JObject();
            JArray schedule = new JArray();
            JArray operation = new JArray();
            basedata.Add("name", patient.NAME);
            basedata.Add("age", ((int)patient.AGE).ToString());
            medicaldata.Add("department", medicalrecord.DOCTOR.DEPT_NAME);
            medicaldata.Add("doctor", medicalrecord.DOCTOR.NAME);
            ArrayList medicalExams = newdao.getMedicalExams((int)medicalrecord.ID, null, null, null, null, null);
            for (int j = 0; j < medicalExams.Count; j++)
            {
                //在今天以后？
                JObject newone = new JObject();
                newone.Add(new JProperty("event", ((MEDICAL_EXAM)medicalExams[j]).EXAM_ITEM.NAME));
                newone.Add(new JProperty("status", ((MEDICAL_EXAM)medicalExams[j]).STATE));
                schedule.Add(newone);
            }
            ArrayList clients = newdao.getClinical((int)medicalrecord.ID, null, null);
            for (int j = 0; j < clients.Count; j++)
            {
                //在今天以后？
                JObject newone = new JObject();
                newone.Add(new JProperty("event", ((CLINICAL)clients[j]).OPERATION.NAME));
                newone.Add(new JProperty("status", ((CLINICAL)clients[j]).STATE));
                schedule.Add(newone);
                if (((CLINICAL)clients[j]).STATE == "待处理")
                {
                    JObject newtwo = new JObject();
                    newtwo.Add(new JProperty("event", ((CLINICAL)clients[j]).OPERATION.NAME));
                    newtwo.Add(new JProperty("eventid", rec_id + "_" + ((int)((CLINICAL)clients[j]).ITEM_ID).ToString()));
                    operation.Add(newtwo);
                }
            }
            ArrayList infusions = newdao.getInfusion((int)medicalrecord.ID, null, null, null);
            for (int j = 0; j < infusions.Count; j++)
            {
                //在今天以后？
                JObject newone = new JObject();
                newone.Add(new JProperty("event", ((INFUSION)infusions[j]).OPERATION.NAME));
                newone.Add(new JProperty("status", ((INFUSION)infusions[j]).STATE));
                schedule.Add(newone);
                if (((INFUSION)infusions[j]).STATE == "待处理")
                {
                    JObject newtwo = new JObject();
                    newtwo.Add(new JProperty("event", ((INFUSION)infusions[j]).OPERATION.NAME));
                    newtwo.Add(new JProperty("eventid", rec_id + "_" + ((int)((INFUSION)infusions[j]).ITEM_ID).ToString() + "_" + ((int)((INFUSION)infusions[j]).DRUG_ID).ToString()));
                    operation.Add(newtwo);
                }
            }
            JObject Result = new JObject();
            Result.Add(new JProperty("basedata", basedata));
            Result.Add(new JProperty("medicaldata", medicaldata));
            Result.Add(new JProperty("schedule", schedule));
            Result.Add(new JProperty("operation", operation));
            return Result;
        }
        public JObject getNursePatientInfoByID(string rec_id, string eventid)
        {
            Dao newdao = new Dao();
            int recID = int.Parse(rec_id); 
            MEDICAL_RECORD medicalrecord = newdao.getMedicalRecordByID(recID);
            PATIENT patient = medicalrecord.PATIENT;
            JObject basedata = new JObject();
            JObject medicaldata = new JObject();
            JArray schedule = new JArray();
            JArray operation = new JArray();
            basedata.Add("name", patient.NAME);
            basedata.Add("age", ((int)patient.AGE).ToString());
            medicaldata.Add("department", medicalrecord.DOCTOR.DEPT_NAME);
            medicaldata.Add("doctor", medicalrecord.DOCTOR.NAME);
            ArrayList AllmedicalExams = newdao.getMedicalExams(null, null, null, null, null, null);
            ArrayList medicalExams = new ArrayList();
            foreach(MEDICAL_EXAM temp in AllmedicalExams)
            {
                if ((int)temp.RECORD_ID == recID)
                    medicalExams.Add(temp);
            }
            for (int j = 0; j < medicalExams.Count; j++)
            {
                //在今天以后？
                JObject newone = new JObject();
                newone.Add(new JProperty("event", ((MEDICAL_EXAM)medicalExams[j]).EXAM_ITEM.NAME));
                newone.Add(new JProperty("status", ((MEDICAL_EXAM)medicalExams[j]).STATE));
                schedule.Add(newone);
            }
            ArrayList clients = newdao.getClinical(recID, null, null);
            for (int j = 0; j < clients.Count; j++)
            {
                //在今天以后？
                JObject newone = new JObject();
                newone.Add(new JProperty("event", ((CLINICAL)clients[j]).OPERATION.NAME));
                newone.Add(new JProperty("status", ((CLINICAL)clients[j]).STATE));
                schedule.Add(newone);
            }
            ArrayList newclients = newdao.getClinical(recID, null, "待处理");
            for (int j = 0; j < newclients.Count; j++)
            {
                //在今天以后？
                JObject newtwo = new JObject();
                newtwo.Add(new JProperty("event", ((CLINICAL)newclients[j]).OPERATION.NAME));
                newtwo.Add(new JProperty("eventid", rec_id + "_" + ((int)((CLINICAL)newclients[j]).ITEM_ID).ToString()));
                operation.Add(newtwo);
            }
            ArrayList infusions = newdao.getInfusion(recID, null, null, null);
            for (int j = 0; j < infusions.Count; j++)
            {
                //在今天以后？
                JObject newone = new JObject();
                newone.Add(new JProperty("event", ((INFUSION)infusions[j]).OPERATION.NAME));
                newone.Add(new JProperty("status", ((INFUSION)infusions[j]).STATE));
                schedule.Add(newone);
            }
            ArrayList newinfusions = newdao.getInfusion(recID, null, null, "待处理");
            for (int j = 0; j < newinfusions.Count; j++)
            {
                //在今天以后？
                JObject newtwo = new JObject();
                newtwo.Add(new JProperty("event", ((INFUSION)newinfusions[j]).OPERATION.NAME));
                newtwo.Add(new JProperty("eventid", rec_id + "_" + ((int)((INFUSION)newinfusions[j]).ITEM_ID).ToString()+ "_" + ((int)((INFUSION)newinfusions[j]).ITEM_ID)));
                operation.Add(newtwo);
            }
            JObject Result = new JObject();
            Result.Add(new JProperty("basedata", basedata));
            Result.Add(new JProperty("medicaldata", medicaldata));
            Result.Add(new JProperty("schedule", schedule));
            Result.Add(new JProperty("operation", operation));
            return Result;
        }
        public JObject getHistoryPatientInfoByID(string rec_id)
        {
            int recID = int.Parse(rec_id); ;
            MEDICAL_RECORD medicalrecord = dao.getMedicalRecordByID(recID);
            PATIENT patient = medicalrecord.PATIENT;
            JObject diseaseDiag = new JObject();
            JArray treatSche = new JArray();
            DateTime tim = medicalrecord.TIME;
            string newtim = tim.Year.ToString() + "年" + tim.Month.ToString() + "月" + tim.Day.ToString()
                    + "日";
            diseaseDiag.Add("date", newtim);
            diseaseDiag.Add("doctor", medicalrecord.DOCTOR.NAME);
            diseaseDiag.Add("history", medicalrecord.DESCRIPTION);
            diseaseDiag.Add("feature", medicalrecord.DISEASE);
            diseaseDiag.Add("suggest", medicalrecord.DIAGNOSIS);
            ArrayList medicalTreatement = dao.getMedicalTreatmentByID((int)medicalrecord.ID);
            for (int j = 0; j < medicalTreatement.Count; j++)
            {
                //在今天以后？
                JObject newone = new JObject();
                newone.Add(new JProperty("stage", ((MEDICAL_TREATEMENT)medicalTreatement[j]).SEQUENCE));
                newone.Add(new JProperty("method", ((MEDICAL_TREATEMENT)medicalTreatement[j]).TITLE));
                newone.Add(new JProperty("info", ((MEDICAL_TREATEMENT)medicalTreatement[j]).DESCRIPTION));
                treatSche.Add(newone);
            }
            JObject Result = new JObject();
            Result.Add(new JProperty("diseaseDiag", diseaseDiag));
            Result.Add(new JProperty("treatSche", treatSche));
            return Result;
        }
        /*public JObject addMedicalRecordOfDoc(string emaildoc, string emailpat, string id)
        {
            JObject result = new JObject();
            result.Add("id", id);
            if (dao.addMedicalRecordOfDoc(emaildoc, emailpat))
            {
                result.Add("result", "success");
            }
            else
            {
                result.Add("result", "fail");
            }
            return result;
        }*/
        public JObject addPrescription(string recordID, string drugname, int quantity, string eat_way)
        {
            int drugid = dao.getdrugIDByName(drugname);
            int medicalID = int.Parse(recordID);
            int expense = dao.getdrugPriceByID(drugid) * quantity;
            JObject result = new JObject();
            Console.WriteLine(drugid + "  " + medicalID);
            if (dao.addPrescription(medicalID, drugid, quantity, expense, eat_way))
            {
                result.Add(new JProperty("result", "success"));
            }
            else
            {
                result.Add(new JProperty("result", "fail"));
            }
            return result;
        }
        public JObject changeRegistPatient(string doc_id, string pat_id)
        {
            int docID = int.Parse(doc_id);
            int patID = int.Parse(pat_id);
            JObject result = new JObject();
            result.Add(new JProperty("doc_id", doc_id));
            if (dao.addMedicalRecordOfDoc(docID, patID))
            {
                result.Add(new JProperty("addresult", "success"));
            }
            else
            {
                result.Add(new JProperty("addresult", "fail"));
            }
            if (dao.delregRecord(patID))
            {
                result.Add(new JProperty("removeresult", "success"));
            }
            else
            {
                result.Add(new JProperty("removeresult", "fail"));
            }
            return result;
        }
        public bool changeRegistRecord(string reg_id, string state)
        {
            int ID = int.Parse(reg_id);
            REGISTRATION_RECORD record = dao.getRegRecordByID(ID);
            if (dao.changeRegistRecord(ID, (int)record.PATIENT_ID, (int)record.SCHEDULE_ID, (int)record.EXPENSE, state, record.TIME))
            {
                return true; 
            }
            else
            {
                return false;
            }
        }
        public bool changeMediacalExam(string rec_id, string item_id, string state)
        {
            int recID = int.Parse(rec_id);
            int itemID = int.Parse(item_id);
            MEDICAL_EXAM exam = (MEDICAL_EXAM)((dao.getMedicalExams(recID, itemID, null, null, null, null))[0]);
            int? doc_id = null;
            if(exam.DOCTOR_ID != null)
            {
                doc_id = (int)exam.DOCTOR_ID;
            }
            if (dao.changeMedicalExam(recID, itemID, state, exam.RESULT, exam.TIME.Value, doc_id))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool changeInfusion(string rec_id, string item_id, string drug_id, string state)
        {
            int recID = int.Parse(rec_id);
            int itemID = int.Parse(item_id);
            int drugID = int.Parse(drug_id);
            INFUSION infusion = (INFUSION)((dao.getInfusion(recID, itemID, drugID, null))[0]);
            if (dao.changeInfusion(recID, itemID, (int)infusion.DRUG_ID, (int)infusion.EXPENSE, state, (int)infusion.NUMBERS))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool changeClinical(string rec_id, string item_id, string state)
        {
            int recID = int.Parse(rec_id);
            int itemID = int.Parse(item_id);
            CLINICAL clinical = (CLINICAL)((dao.getClinical(recID, itemID, null))[0]);
            if (dao.changeClinical(recID, itemID, (int)clinical.NUMBERS, clinical.ADVICE, (int)clinical.EXPENSE, state))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool changeMedicalRecordState(string rec_id, string t_state, string c_state, string i_state, string d_state)
        {
            int recID = int.Parse(rec_id);
            MEDICAL_RECORD record = dao.getMedicalRecordByID(recID);
            if (dao.changeMedicalRecord(recID, (int)record.DOCTOR_ID, (int)record.PATIENT_ID, t_state, record.TIME, record.DISEASE, record.DESCRIPTION, record.DIAGNOSIS, c_state, i_state, d_state))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public JObject changeMedicalRecord(string doc_id, string pat_id, string feature, string suggest)
        {
            int docID = int.Parse(doc_id);
            int patID = int.Parse(pat_id);
            JObject result = new JObject();
            MEDICAL_RECORD record = (MEDICAL_RECORD)(dao.getMedicalRecordByDoctorAndPatient(docID, patID)[0]);
            result.Add(new JProperty("doc_id", doc_id));
            if (dao.changeMedicalRecord((int)record.ID, docID, patID, record.TREAT_STATE, record.TIME, feature, record.DESCRIPTION, suggest, record.CLIN_STATE, record.INFU_STATE, record.DRUG_STATE))
            {
                result.Add(new JProperty("result", "success"));
            }
            else
            {
                result.Add(new JProperty("result", "fail"));
            }
            return result;
        }
        public JObject changeMedicalRecordState(string doc_id, string pat_id, string state)
        {
            int docID = int.Parse(doc_id);
            int patID = int.Parse(pat_id);
            JObject result = new JObject();
            MEDICAL_RECORD record = (MEDICAL_RECORD)(dao.getMedicalRecordByDoctorAndPatient(docID, patID)[0]);
            if (dao.changeMedicalRecord((int)record.ID, docID, patID, state, record.TIME, record.DISEASE, record.DESCRIPTION, record.DIAGNOSIS, record.CLIN_STATE, record.INFU_STATE, record.DRUG_STATE))
            {
                result.Add(new JProperty("result", "success"));
            }
            else
            {
                result.Add(new JProperty("result", "fail"));
            }
            return result;
        }
        public JObject changeMedicalExamState(string doc_id, string pat_id, string record_state, string exam_state, string chk_id, string type)
        {
            int docID = int.Parse(doc_id);
            int patID = int.Parse(pat_id);
            int chkID = int.Parse(chk_id);
            int itemID = int.Parse(type);
            JObject result = new JObject();
            MEDICAL_RECORD record = (MEDICAL_RECORD)(dao.getMedicalRecordByDoctorAndPatient(docID, patID)[0]);
            MEDICAL_EXAM exam = (MEDICAL_EXAM)(dao.getMedicalExams((int)record.ID, itemID, null, null, null, null)[0]);
            if (dao.changeMedicalRecord((int)record.ID, docID, patID, record_state, record.TIME, record.DISEASE, record.DESCRIPTION, record.DIAGNOSIS, record.CLIN_STATE, record.INFU_STATE, record.DRUG_STATE)
                && dao.changeMedicalExam((int)record.ID, itemID, exam_state, exam.RESULT, exam.TIME.Value, chkID))
            {
                result.Add(new JProperty("result", "success"));
            }
            else
            {
                result.Add(new JProperty("result", "fail"));
            }
            return result;
        }
        public JObject checkMedicalExamState(string doc_id, string pat_id, string report, string chk_id, string type)
        {
            int docID = int.Parse(doc_id);
            int patID = int.Parse(pat_id);
            int chkID = int.Parse(chk_id);
            int itemID = int.Parse(type);
            JObject result = new JObject();
            MEDICAL_RECORD record = (MEDICAL_RECORD)(dao.getMedicalRecordByDoctorAndPatient(docID, patID)[0]);
            MEDICAL_EXAM exam = (MEDICAL_EXAM)(dao.getMedicalExams((int)record.ID, itemID, null, null, null, null)[0]);
            if (dao.changeMedicalExam((int)record.ID, itemID, exam.STATE, report, exam.TIME.Value, chkID))
            {
                result.Add(new JProperty("result", "success"));
            }
            else
            {
                result.Add(new JProperty("result", "fail"));
            }
            return result;
        }
        public JObject getMedicalExamReport(string doc_id, string pat_id, string type)
        {
            int docID = int.Parse(doc_id);
            int patID = int.Parse(pat_id);
            int itemID = int.Parse(type);
            JObject result = new JObject();
            MEDICAL_RECORD record = (MEDICAL_RECORD)(dao.getMedicalRecordByDoctorAndPatient(docID, patID)[0]);
            MEDICAL_EXAM exam = (MEDICAL_EXAM)(dao.getMedicalExams((int)record.ID, itemID, null, null, null, null)[0]);
            result.Add(new JProperty("result", exam.RESULT));
            Thread.Sleep(2000);
            return result;
        }
        public JObject getMedicalExamReportInfo(string doc_id, string pat_id, string type)
        {
            int docID = int.Parse(doc_id);
            int patID = int.Parse(pat_id);
            int itemID = int.Parse(type);
            JObject result = new JObject();
            MEDICAL_RECORD record = (MEDICAL_RECORD)(dao.getMedicalRecordByDoctorAndPatient(docID, patID)[0]);
            MEDICAL_EXAM exam = (MEDICAL_EXAM)(dao.getMedicalExams((int)record.ID, itemID, null, null, null, null)[0]);
            DateTime time = exam.TIME.Value;
            string newtime = time.Year.ToString() + "年" + time.Month.ToString() + "月" + time.Day.ToString()
                + "日    " + time.Hour.ToString() + ":" + time.Minute.ToString();
            result.Add(new JProperty("patientID", ((int)record.PATIENT_ID).ToString()));
            result.Add(new JProperty("name", record.PATIENT.NAME));
            result.Add(new JProperty("sex", record.PATIENT.GENDER));
            result.Add(new JProperty("department", record.DOCTOR.DEPT_NAME));
            result.Add(new JProperty("age", record.PATIENT.AGE));
            result.Add(new JProperty("time", newtime));
            result.Add(new JProperty("checkID", ((int)exam.DOCTOR_ID).ToString()));
            result.Add(new JProperty("mainID", ((int)record.DOCTOR_ID).ToString()));
            result.Add(new JProperty("doctorName", record.DOCTOR.NAME));
            result.Add(new JProperty("checkName", exam.DOCTOR.NAME));
            result.Add(new JProperty("result", exam.RESULT));
            if(itemID == 2)
                result.Add(new JProperty("reference", "a"));
            else
                result.Add(new JProperty("reference", exam.EXAM_ITEM.REFERENCE_VALUE));
            return result;
        }
        public JObject finishMedicalExam(string doc_id, string pat_id, string chk_id, string type)
        {
            int docID = int.Parse(doc_id);
            int patID = int.Parse(pat_id);
            int chkID = int.Parse(chk_id);
            int itemID = int.Parse(type);
            bool flag = true;
            JObject result = new JObject();
            MEDICAL_RECORD record = (MEDICAL_RECORD)dao.getMedicalRecordByDoctorAndPatient(docID, patID)[0];
            MEDICAL_EXAM exam = (MEDICAL_EXAM)dao.getMedicalExams((int)record.ID, itemID, null, null, null, null)[0];
            dao.changeMedicalExam((int)record.ID, itemID, "全部完成", exam.RESULT, exam.TIME.Value, chkID);
            ArrayList list = dao.getMedicalExams((int)record.ID, null, null, null, null, null);
            foreach (MEDICAL_EXAM temp in list)
            {
                if (temp.STATE != "全部完成")
                {
                    flag = false;
                }
            }
            if (flag)
            {
                if (dao.changeMedicalRecord((int)record.ID, docID, patID, "检查完成", record.TIME, record.DISEASE, record.DESCRIPTION, record.DIAGNOSIS, record.CLIN_STATE, record.INFU_STATE, record.DRUG_STATE))
                {
                    result.Add(new JProperty("result", "success"));
                }
                else
                {
                    result.Add(new JProperty("result", "fail"));
                }
            }
            else
            {
                if (dao.changeMedicalRecord((int)record.ID, docID, patID, "待检查", record.TIME, record.DISEASE, record.DESCRIPTION, record.DIAGNOSIS, record.CLIN_STATE, record.INFU_STATE, record.DRUG_STATE))
                {
                    result.Add(new JProperty("result", "success"));
                }
                else
                {
                    result.Add(new JProperty("result", "fail"));
                }
            }
            return result;
        }
        public JObject delPrescription(string email, string drugname, string id)
        {
            int drugid = dao.getdrugIDByName(drugname);
            var temp = dao.getMedicalRecordsOfPatient(dao.getPatientIdByEmail(email));
            int medicalID = (int)((MEDICAL_RECORD)temp[0]).ID;
            JObject result = new JObject();
            result.Add(new JProperty("id", id));
            if (dao.delPrescription(medicalID, drugid))
            {
                result.Add(new JProperty("result", "success"));
            }
            else
            {
                result.Add(new JProperty("result", "fail"));
            }
            return result;
        }
        public JObject getMedicalSchemeOfPatient(string email, string id)
        {
            int PatientID = dao.getPatientIdByEmail(email);
            ArrayList medicalRecords = dao.getMedicalRecordsOfPatient(PatientID);
            JArray res = new JArray();
            foreach (MEDICAL_RECORD record in medicalRecords)
            {
                //if (((MEDICAL_RECORD)medicalRecords[i]).STATE != "abv")
                //{
                ArrayList temp = dao.getMedicalSchemeOfRecord((int)record.ID);
                foreach (MEDICAL_TREATEMENT item in temp)
                {
                    JObject tmp = new JObject();
                    tmp.Add(new JProperty("record_ID", item.RECORD_ID.ToString()));
                    tmp.Add(new JProperty("sequence", item.SEQUENCE.ToString()));
                    tmp.Add(new JProperty("title", item.TITLE));
                    tmp.Add(new JProperty("description", item.DESCRIPTION));
                    res.Add(tmp);
                }
                //}

            }
            JObject result = new JObject();
            result.Add(new JProperty("id", id));
            result.Add(new JProperty("result", res));
            return result;


        }
        public JObject addMedicalTreatment(string doc_id, string pat_id, string sequence, string type, string describe)
        {
            int PatientID = int.Parse(pat_id);
            int DoctorID = int.Parse(doc_id);
            ArrayList medicalRecords = dao.getMedicalRecordByDoctorAndPatient(DoctorID, PatientID);
            MEDICAL_RECORD mrecord = (MEDICAL_RECORD)medicalRecords[0];
            int mrecordID = (int)(mrecord.ID);
            int seq = int.Parse(sequence);
            Console.WriteLine("mrecordID" + mrecordID);
            JObject result = new JObject();
            Console.WriteLine("sequence" + sequence);
            Console.WriteLine("type" + type);
            Console.WriteLine("describe" + describe);
            if (dao.addMedicalSchemeOfRecord(mrecordID, seq, type, describe))
            {
                result.Add(new JProperty("res", "success"));
            }
            else
            {
                result.Add(new JProperty("res", "fail"));
            }
            return result;
        }
        public JArray getDrugInfoByName(string name)
        {
            ArrayList allDrugs = dao.getdrugsByName(name);
            JArray result = new JArray();
            foreach (DRUG drug in allDrugs)
            {
                DRUG_INVENTORY inventory = dao.getdrugInventoryByID(dao.getdrugInventoryIDByName(drug.NAME));
                if (inventory != null)
                {
                    JObject temp = new JObject();
                    temp.Add(new JProperty("name", drug.NAME));
                    temp.Add(new JProperty("short", drug.CODE));
                    temp.Add(new JProperty("price", ((int)inventory.PRICE).ToString()));
                    temp.Add(new JProperty("left", ((int)inventory.SURPLUS).ToString()));
                    temp.Add(new JProperty("spec", drug.STANDARD));
                    result.Add(temp);
                }
            }
            return result;
        }
        public JObject getDoctorInfoByID(string id)
        {
            DOCTOR doctor = dao.getDoctorsByID(int.Parse(id));
            JObject result = new JObject();
            result.Add(new JProperty("name", doctor.NAME));
            result.Add(new JProperty("sex", doctor.GENDER));
            result.Add(new JProperty("department", doctor.DEPT_NAME));
            return result;
        }
        public JObject delMedicalTreatment(string doc_id, string pat_id)
        {
            int PatientID = int.Parse(pat_id);
            int DoctorID = int.Parse(doc_id);
            bool flag = true;
            ArrayList medicalRecords = dao.getMedicalRecordByDoctorAndPatient(DoctorID, PatientID);
            MEDICAL_RECORD mrecord = (MEDICAL_RECORD)medicalRecords[0];
            int mrecordID = (int)(mrecord.ID);
            ArrayList medicalTreatments = dao.getMedicalTreatmentByID(mrecordID);
            foreach(MEDICAL_TREATEMENT medicalTreatment in medicalTreatments)
            {
                if(!dao.delMedicalTreatment((int)medicalTreatment.RECORD_ID, (int)medicalTreatment.SEQUENCE, medicalTreatment.TITLE, medicalTreatment.DESCRIPTION))
                {
                    flag = false;
                }
            }
            JObject result = new JObject();
            if (flag)
            {
                result.Add(new JProperty("res", "success"));
            }
            else
            {
                result.Add(new JProperty("res", "fail"));
            }
            return result;
        }
        public JObject getMedicineBysubString(string subname, string id)
        {
            ArrayList allMedicine = dao.getMedicinebyName(null);
            JArray res = new JArray();
            for (int i = 0; i < allMedicine.Count; i++)
            {
                DRUG_INVENTORY temp = (DRUG_INVENTORY)allMedicine[i];
                string name = dao.getdrugNameByID((int)temp.DRUG_ID);
                if (name.Contains(subname))
                {
                    JObject rightDrug = new JObject();
                    rightDrug.Add(new JProperty("id", temp.ID));
                    rightDrug.Add(new JProperty("name", name));
                    res.Add(rightDrug);
                }

            }
            JObject result = new JObject();
            result.Add(new JProperty("id", id));
            result.Add(new JProperty("result", res));
            return result;

        }
        /*public JObject addMedicalExam(string email, string Examname, string id)
        {
            int ExamID = dao.getExamIDByName(Examname);
            int patientID = dao.getPatientIdByEmail(email);
            ArrayList medicalRecords = dao.getMedicalRecordsOfPatient(patientID);
            int recordID = -1;
            for (int i = 0; i < medicalRecords.Count; i++)
            {
                if (((MEDICAL_RECORD)medicalRecords[i]).STATE != "全部完成")
                {
                    var temp = dao.getMedicalSchemeOfRecord((int)(((MEDICAL_RECORD)medicalRecords[i]).ID));
                    for (int j = 0; j < temp.Count; i++)
                    {

                        recordID = (int)((MEDICAL_RECORD)temp[i]).ID;

                    }
                }

            }
            JObject result = new JObject();
            result.Add(new JProperty("id", id));
            if (dao.addExam(recordID, ExamID))
            {
                result.Add(new JProperty("result", "success"));
            }
            else
            {
                result.Add(new JProperty("result", "fail"));
            }
            return result;

        }*/
        public JObject getAllRoreInfo(string id)
        {
            ArrayList temp = dao.getAllRoreInfo();
            string res = "";
            for (int i = 0; i < temp.Count; i++)
            {
                if (((FOREGROUND_INFORMATION)temp[i]).STATE == "前台给病人")
                {
                    res += "前台→";
                    res += ((FOREGROUND_INFORMATION)temp[i]).PATIENT_ID.ToString();
                    res += ":";
                    res += ((FOREGROUND_INFORMATION)temp[i]).INFORMATION;
                    res += "\n";
                }
                else
                {
                    res += ((FOREGROUND_INFORMATION)temp[i]).PATIENT_ID.ToString();
                    res += "→前台";
                    res += ":";
                    res += ((FOREGROUND_INFORMATION)temp[i]).INFORMATION;
                    res += "\n";
                }
            }
            JObject result = new JObject();
            result.Add(new JProperty("id", id));
            result.Add(new JProperty("result", res));
            return result;
        }
        public JObject addRoreInfo(string id, int patientid, string information)
        {
            string email = dao.getPatientEmailById(patientid);
            JObject result = new JObject();
            result.Add(new JProperty("id", id));
            Console.WriteLine("PatientID" + patientid);
            Console.WriteLine("email" + email);
            Console.WriteLine("information" + information);
            if (dao.addRoreInfo(patientid, email, information))
            {
                result.Add(new JProperty("res", "success"));
            }
            else
            {
                result.Add(new JProperty("res", "fail"));
            }
            return result;
        }
        public JObject addPatientRoreInfo(string id, int patientid, string information)
        {
            string email = dao.getPatientEmailById(patientid);
            JObject result = new JObject();
            result.Add(new JProperty("id", id));
            Console.WriteLine("PatientID" + patientid);
            Console.WriteLine("email" + email);
            Console.WriteLine("information" + information);
            if (dao.addPatientRoreInfo(patientid, email, information))
            {
                result.Add(new JProperty("res", "SUCCESS"));
            }
            else
            {
                result.Add(new JProperty("res", "FAIL"));
            }
            return result;
        }
        public JObject getAllRegRecord(string id)
        {
            ArrayList temp = dao.getAllRegRecord();
            JArray res = new JArray();
            foreach (REGISTRATION_RECORD item in temp)
            {
                string name = dao.getPatientNameByID((int)(item.PATIENT_ID));
                string dept_name = dao.getDepartmentNameBySchedualID((int)(item.SCHEDULE_ID));
                JObject record = new JObject();
                DateTime time = item.TIME;
                string newtime = time.Year.ToString() + "年" + time.Month.ToString() + "月" + time.Day.ToString()
                    + "日    " + time.Hour.ToString() + ":" + time.Minute.ToString();
                record.Add(new JProperty("id", item.ID));
                record.Add(new JProperty("name", name));
                record.Add(new JProperty("expense", item.EXPENSE));
                record.Add(new JProperty("state", item.STATE));
                record.Add(new JProperty("time", newtime));
                record.Add(new JProperty("department", dept_name));
                res.Add(record);
            }
            JObject result = new JObject();
            result.Add(new JProperty("id", id));
            result.Add(new JProperty("result", res));
            return result;
        }
        public JObject addRegRecord(string id, string email, string dept)
        {
            
            int patientid = dao.getPatientIdByEmail(email);
            int scheduleid = dao.getScheduleIDByDept(dept);
            OPERATION operation = dao.getOperation(5);
            JObject result = new JObject();
            result.Add(new JProperty("id", id));
            if (scheduleid == -1)
            {
                Thread.Sleep(2000);
                result.Add(new JProperty("res", "fail"));
                Console.WriteLine("fail");
                return result;
            }
            if (dao.addregRecord(patientid, scheduleid, (int)operation.PRICE))
            {
                result.Add(new JProperty("res", "success"));
            }
            else
            {
                Thread.Sleep(2000);
                result.Add(new JProperty("res", "fail"));
                Console.WriteLine("fail");
            }
            return result;
        }
        public JObject addOppointment(string id, string email, string sche_id, DateTime time)
        {

            int patientid = dao.getPatientIdByEmail(email);
            int scheduleid = int.Parse(sche_id);
            OPERATION operation = dao.getOperation(6);
            JObject result = new JObject();
            result.Add(new JProperty("id", id));
            if (dao.addoppointment(patientid, scheduleid, (int)operation.PRICE, time))
            {
                result.Add(new JProperty("result", "SUCCESS"));
            }
            else
            {
                Thread.Sleep(2000);
                result.Add(new JProperty("result", "FAIL"));
            }
            return result;
        }

        public JObject getPrescribeByDocAndPat(string doc_id, string pat_id)
        {
            MEDICAL_RECORD record = (MEDICAL_RECORD)(dao.getMedicalRecordByDoctorAndPatient(int.Parse(doc_id), int.Parse(pat_id))[0]);
            int medicalID = (int)record.ID;
            ArrayList allMedicine = dao.getPrescribeByMedicalRecord(medicalID);
            JArray res = new JArray();
            JObject result = new JObject();
            foreach (PRESCRIBE prescribe in allMedicine)
            {
                JObject temp = new JObject();
                temp.Add(new JProperty("name", prescribe.DRUG_INVENTORY.DRUG.NAME));
                temp.Add(new JProperty("price", ((int)prescribe.DRUG_INVENTORY.PRICE).ToString()));
                temp.Add(new JProperty("use", prescribe.EAT_WAYS));
                temp.Add(new JProperty("quan", ((int)prescribe.QUANTITY).ToString()));
                temp.Add(new JProperty("total", ((int)prescribe.EXPENSE).ToString()));
                res.Add(temp);
            }
            result.Add(new JProperty("result", res));
            return result;

        }
        public JObject getReport(string doc_id, string pat_id, string item_id)
        {
            MEDICAL_RECORD record = (MEDICAL_RECORD)dao.getMedicalRecordByDoctorAndPatient(int.Parse(doc_id), int.Parse(pat_id))[0];
            int medicalID = (int)record.ID;
            int itemID = int.Parse(item_id);
            ArrayList allMedicine = dao.getMedicalExams(medicalID, itemID, null, null, null, null);
            MEDICAL_EXAM exam = (MEDICAL_EXAM)(allMedicine[0]);
            JObject result = new JObject();
            result.Add(new JProperty("doc_id", doc_id));
            result.Add(new JProperty("result", exam.RESULT));
            return result;

        }
        public JObject addMedicalExam(int recordID, string name)
        {
            EXAM_ITEM item = (EXAM_ITEM)(dao.getExamItem(name)[0]);
            int examID = (int)((item).ID);
            JArray res = new JArray();
            JObject result = new JObject();
            dao.addMedicalExam(recordID, examID);
            result.Add(new JProperty("id", examID.ToString()));
            result.Add(new JProperty("name", item.NAME));
            result.Add(new JProperty("expense", ((int)item.EXPENSE).ToString()));
            result.Add(new JProperty("state", "未缴费"));
            return result;

        }
        public bool addClinical(int recordID, string name, int num, string advice)
        {
            OPERATION item = (OPERATION)(dao.getOperationItem(name)[0]);
            int itemID = (int)((item).ID);
            int expense = (num * ((int)item.PRICE));
            if (dao.addClinical(recordID, itemID, num, advice, expense))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool addInfusion(int recordID, string name, string type, int number)
        {
            OPERATION item = (OPERATION)(dao.getOperationItem(type)[0]);
            int itemID = (int)((item).ID);
            int drugID = dao.getdrugIDByName(name);
            DRUG drug = dao.getdrugByID(drugID);
            int drugInventoryID = dao.getdrugInventoryIDByName(drug.NAME);
            DRUG_INVENTORY drug_I = dao.getdrugInventoryByID(drugInventoryID);
            int expense = ((int)item.PRICE + (int)drug_I.PRICE) * number;
            if (dao.addInfusion(recordID, itemID, drugID, expense, number))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public JObject getMedicalBill(string doc_id, string pat_id)
        {
            int docID = int.Parse(doc_id);
            int patID = int.Parse(pat_id);
            int money = 0;
            MEDICAL_RECORD record = (MEDICAL_RECORD)(dao.getMedicalRecordByDoctorAndPatient(docID, patID)[0]);
            JArray medicine = new JArray();
            JArray injection = new JArray();
            JArray infusion = new JArray();
            JArray debridement = new JArray();
            ArrayList Prescribes = dao.getPrescribeByMedicalRecord((int)record.ID);
            ArrayList Infusions = dao.getInfusion((int)record.ID, null, null, "未缴费");
            ArrayList Clinicals = dao.getClinical((int)record.ID, null, "未缴费");
            foreach (PRESCRIBE prescribe in Prescribes)
            {
                JObject temp = new JObject();
                temp.Add(new JProperty("name", prescribe.DRUG_INVENTORY.DRUG.NAME));
                temp.Add(new JProperty("price", ((int)prescribe.DRUG_INVENTORY.PRICE).ToString()));
                temp.Add(new JProperty("use", prescribe.EAT_WAYS));
                temp.Add(new JProperty("quan", ((int)prescribe.QUANTITY).ToString()));
                temp.Add(new JProperty("total", (((int)prescribe.DRUG_INVENTORY.PRICE)* ((int)prescribe.QUANTITY)).ToString()));
                medicine.Add(temp);
                money += ((int)prescribe.DRUG_INVENTORY.PRICE) * ((int)prescribe.QUANTITY);
            }
            foreach (INFUSION infusions in Infusions)
            {
                if (infusions.OPERATION.NAME == "注射")
                {
                    JObject temp = new JObject();
                    temp.Add(new JProperty("name", infusions.DRUG_INVENTORY.DRUG.NAME));
                    temp.Add(new JProperty("price", ((int)infusions.DRUG_INVENTORY.PRICE).ToString()));
                    temp.Add(new JProperty("quan", ((int)infusions.NUMBERS).ToString()));
                    temp.Add(new JProperty("total", ((int)infusions.EXPENSE).ToString()));
                    injection.Add(temp);
                    money += (int)infusions.EXPENSE;
                }
            }
            foreach (INFUSION infusions in Infusions)
            {
                if (infusions.OPERATION.NAME == "输液")
                {
                    JObject temp = new JObject();
                    temp.Add(new JProperty("name", infusions.DRUG_INVENTORY.DRUG.NAME));
                    temp.Add(new JProperty("price", ((int)infusions.DRUG_INVENTORY.PRICE).ToString()));
                    temp.Add(new JProperty("quan", ((int)infusions.NUMBERS).ToString()));
                    temp.Add(new JProperty("total", ((int)infusions.EXPENSE).ToString()));
                    infusion.Add(temp);
                    money += (int)infusions.EXPENSE;
                }
            }
            foreach (CLINICAL clinical in Clinicals)
            {
                JObject temp = new JObject();
                temp.Add(new JProperty("name", clinical.OPERATION.NAME));
                temp.Add(new JProperty("num", ((int)clinical.NUMBERS).ToString()));
                temp.Add(new JProperty("price", ((int)clinical.OPERATION.PRICE).ToString()));
                temp.Add(new JProperty("total", (((int)clinical.OPERATION.PRICE) * ((int)clinical.NUMBERS)).ToString()));
                debridement.Add(temp);
                money += ((int)clinical.OPERATION.PRICE) * ((int)clinical.NUMBERS);
            }
            JObject result = new JObject();
            result.Add(new JProperty("doc_id", doc_id));
            result.Add(new JProperty("medicine", medicine));
            result.Add(new JProperty("injection", injection));
            result.Add(new JProperty("infusion", infusion));
            result.Add(new JProperty("debridement", debridement));
            result.Add(new JProperty("total", money.ToString()));
            return result;
        }
        public JObject getCheckPatientInfo(string id)
        {
            DOCTOR doctor = dao.getDoctorsByID(int.Parse(id));
            string Dept = doctor.DEPT_NAME;
            JArray res = new JArray();
            if (Dept == "检验科")
            {
                ArrayList temp1 = dao.getMedicalExams(null, 1, "未完成", null, null, null);
                ArrayList temp2 = dao.getMedicalExams(null, 3, "未完成", null, null, null);
                ArrayList temp3 = dao.getMedicalExams(null, 4, "未完成", null, null, null);
                foreach (MEDICAL_EXAM item in temp1)
                {
                    DateTime time = item.TIME.Value;
                    string newtime = time.Year.ToString() + "年" + time.Month.ToString() + "月" + time.Day.ToString()
                        + "日    " + time.Hour.ToString() + ":" + time.Minute.ToString();
                    JObject record = new JObject();
                    if (item.MEDICAL_RECORD.TREAT_STATE == "待检查")
                    {
                        record.Add(new JProperty("patientID", ((int)item.MEDICAL_RECORD.PATIENT_ID).ToString()));
                        record.Add(new JProperty("name", item.MEDICAL_RECORD.PATIENT.NAME));
                        record.Add(new JProperty("sex", item.MEDICAL_RECORD.PATIENT.GENDER));
                        record.Add(new JProperty("doctorID", item.MEDICAL_RECORD.DOCTOR_ID));
                        record.Add(new JProperty("checkItem", item.EXAM_ITEM.NAME));
                        record.Add(new JProperty("department", "检验科"));
                        record.Add(new JProperty("time", newtime));
                        res.Add(record);
                    }
                }
                foreach (MEDICAL_EXAM item in temp2)
                {
                    DateTime time = item.TIME.Value;
                    string newtime = time.Year.ToString() + "年" + time.Month.ToString() + "月" + time.Day.ToString()
                        + "日    " + time.Hour.ToString() + ":" + time.Minute.ToString();
                    JObject record = new JObject();
                    if (item.MEDICAL_RECORD.TREAT_STATE == "待检查")
                    {
                        record.Add(new JProperty("patientID", ((int)item.MEDICAL_RECORD.PATIENT_ID).ToString()));
                        record.Add(new JProperty("name", item.MEDICAL_RECORD.PATIENT.NAME));
                        record.Add(new JProperty("sex", item.MEDICAL_RECORD.PATIENT.GENDER));
                        record.Add(new JProperty("doctorID", item.MEDICAL_RECORD.DOCTOR_ID));
                        record.Add(new JProperty("checkItem", item.EXAM_ITEM.NAME));
                        record.Add(new JProperty("department", "检验科"));
                        record.Add(new JProperty("time", newtime));
                        res.Add(record);
                    }
                }
                foreach (MEDICAL_EXAM item in temp3)
                {
                    JObject record = new JObject();
                    if (item.MEDICAL_RECORD.TREAT_STATE == "待检查")
                    {
                        DateTime time = item.TIME.Value;
                        string newtime = time.Year.ToString() + "年" + time.Month.ToString() + "月" + time.Day.ToString()
                            + "日    " + time.Hour.ToString() + ":" + time.Minute.ToString();
                        record.Add(new JProperty("patientID", ((int)item.MEDICAL_RECORD.PATIENT_ID).ToString()));
                        record.Add(new JProperty("name", item.MEDICAL_RECORD.PATIENT.NAME));
                        record.Add(new JProperty("sex", item.MEDICAL_RECORD.PATIENT.GENDER));
                        record.Add(new JProperty("doctorID", item.MEDICAL_RECORD.DOCTOR_ID));
                        record.Add(new JProperty("checkItem", item.EXAM_ITEM.NAME));
                        record.Add(new JProperty("department", "检验科"));
                        record.Add(new JProperty("time", newtime));
                        res.Add(record);
                    }
                }
            }
            else if(Dept == "放射科")
            {
                ArrayList temp = dao.getMedicalExams(null, 2, "未完成", null, null, null);
                foreach (MEDICAL_EXAM item in temp)
                {
                    JObject record = new JObject();
                    if (item.MEDICAL_RECORD.TREAT_STATE == "待检查")
                    {
                        DateTime time = item.TIME.Value;
                        string newtime = time.Year.ToString() + "年" + time.Month.ToString() + "月" + time.Day.ToString()
                            + "日    " + time.Hour.ToString() + ":" + time.Minute.ToString();
                        record.Add(new JProperty("patientID", ((int)item.MEDICAL_RECORD.PATIENT_ID).ToString()));
                        record.Add(new JProperty("name", item.MEDICAL_RECORD.PATIENT.NAME));
                        record.Add(new JProperty("sex", item.MEDICAL_RECORD.PATIENT.GENDER));
                        record.Add(new JProperty("doctorID", item.MEDICAL_RECORD.DOCTOR_ID));
                        record.Add(new JProperty("checkItem", item.EXAM_ITEM.NAME));
                        record.Add(new JProperty("department", "放射科"));
                        record.Add(new JProperty("time", newtime));
                        res.Add(record);
                    }
                }
            }
            JObject result = new JObject();
            result.Add(new JProperty("result", res));
            return result;
        }
        public JObject getMedicalExamBycheckID(string id)
        {
            int checkID = int.Parse(id);
            ArrayList allExam = dao.getMedicalExams(null, null, null, null, null, checkID);
            JArray res = new JArray();
            foreach (MEDICAL_EXAM exam in allExam)
            {
                if (exam.STATE == "未完成")
                {
                    DateTime time = exam.TIME.Value;
                    string newtime = time.Year.ToString() + "年" + time.Month.ToString() + "月" + time.Day.ToString()
                        + "日    " + time.Hour.ToString() + ":" + time.Minute.ToString();
                    JObject temp = new JObject();
                    temp.Add(new JProperty("patientID", ((int)exam.MEDICAL_RECORD.PATIENT_ID).ToString()));
                    temp.Add(new JProperty("name", exam.MEDICAL_RECORD.PATIENT.NAME));
                    temp.Add(new JProperty("sex", exam.MEDICAL_RECORD.PATIENT.GENDER));
                    temp.Add(new JProperty("type", exam.EXAM_ITEM.NAME));
                    temp.Add(new JProperty("doctorID", exam.MEDICAL_RECORD.DOCTOR_ID));
                    temp.Add(new JProperty("doctorName", exam.MEDICAL_RECORD.DOCTOR.NAME));
                    temp.Add(new JProperty("department", exam.MEDICAL_RECORD.DOCTOR.DEPT_NAME));
                    temp.Add(new JProperty("time", newtime));
                    res.Add(temp);
                }
            }
            JObject result = new JObject();
            result.Add(new JProperty("result", res));
            return result;
        }
        public JObject getSchedualByDoctor(string id, string email)
        {
            ArrayList allSchedules = dao.getScheduleByDoctor(email);
            JArray res = new JArray();
            JObject result = new JObject();
            foreach (SCHEDULE schedule in allSchedules)
            {
                DateTime time = schedule.BEGIN_TIME;
                string startTime = time.Year.ToString() + "-" + time.Month.ToString() + "-" + time.Day.ToString()
                    + " " + time.Hour.ToString() + ":" + time.Minute.ToString();
                time = schedule.END_TIME;
                string endTime = time.Year.ToString() + "-" + time.Month.ToString() + "-" + time.Day.ToString()
                    + " " + time.Hour.ToString() + ":" + time.Minute.ToString();
                JObject temp = new JObject();
                temp.Add(new JProperty("id", schedule.ID));
                temp.Add(new JProperty("startTime", startTime));
                temp.Add(new JProperty("endTime", endTime));
                res.Add(temp);
            }
            result.Add(new JProperty("id", id));
            result.Add(new JProperty("result", res));
            return result;
        }
        public JObject getHistoryByPat(string id, string email)
        {
            int patID = dao.getPatientIdByEmail(email);
            ArrayList allRecords = dao.getMedicalRecordsOfPatient(patID);
            JArray res = new JArray();
            JObject result = new JObject();
            foreach (MEDICAL_RECORD record in allRecords)
            {
                DateTime time = record.TIME;
                string datetime = time.Year.ToString() + "-" + time.Month.ToString() + "-" + time.Day.ToString()
                    + " " + time.Hour.ToString() + ":" + time.Minute.ToString();
                JObject temp = new JObject();
                temp.Add(new JProperty("id", record.ID));
                temp.Add(new JProperty("datetime", datetime));
                temp.Add(new JProperty("info", record.DISEASE));
                res.Add(temp);
            }
            result.Add(new JProperty("id", id));
            result.Add(new JProperty("result", res));
            return result;
        }
        public JObject getinfomation(string id, string email)
        {
            int patID = dao.getPatientIdByEmail(email);
            ArrayList allInfo = dao.getPatientRoreInfo(email);
            JArray res = new JArray();
            JObject result = new JObject();
            foreach (FOREGROUND_INFORMATION record in allInfo)
            {
                JObject temp = new JObject();
                if(record.STATE == "病人给前台")
                    temp.Add(new JProperty("sender", "me"));
                else if(record.STATE == "前台给病人")
                    temp.Add(new JProperty("sender", "front"));
                temp.Add(new JProperty("message", record.INFORMATION));
                res.Add(temp);
            }
            result.Add(new JProperty("id", id));
            result.Add(new JProperty("result", res));
            return result;
        }
        public JObject getPatientInfoByName(string id, string name)
        {
            ArrayList allPatients = dao.getPatientsByName(name);
            JArray res = new JArray();
            JObject result = new JObject();
            foreach (PATIENT Patient in allPatients)
            {
                JObject temp = new JObject();
                temp.Add(new JProperty("code", Patient.IDENTITY));
                temp.Add(new JProperty("id", ((int)Patient.ID).ToString()));
                temp.Add(new JProperty("name", Patient.NAME));
                res.Add(temp);
            }
            result.Add(new JProperty("id", id));
            result.Add(new JProperty("result", res));
            return result;
        }
        public JObject payBillByID(string id, string pat_ID)
        {
            int pat_id = int.Parse(pat_ID);
            PATIENT Patient = (PATIENT)((dao.getPatientById(pat_id))[0]);
            JArray reg_res = new JArray();
            JArray exam_res = new JArray();
            JArray infusion_res = new JArray();
            JArray clinical_res = new JArray();
            JArray drug_res = new JArray();
            JObject result = new JObject();
            ArrayList regists = dao.getRegRecordByPatAndState(pat_id, "未缴费");
            ArrayList records = dao.getMedicalRecordByPat(pat_id);
            foreach(REGISTRATION_RECORD reg in regists)
            {
                DateTime time = reg.TIME;
                string datetime = time.Year.ToString() + "-" + time.Month.ToString() + "-" + time.Day.ToString()
                    + " " + time.Hour.ToString() + ":" + time.Minute.ToString();
                JObject temp = new JObject();
                temp.Add(new JProperty("id", reg.ID));
                temp.Add(new JProperty("name", "挂号费"));
                temp.Add(new JProperty("datetime", datetime));
                temp.Add(new JProperty("amount", ((int)reg.EXPENSE).ToString()));
                reg_res.Add(temp);
            }
            foreach (MEDICAL_RECORD record in records)
            {
                ArrayList exams = new ArrayList();
                ArrayList drugs = new ArrayList();
                ArrayList infusions = new ArrayList();
                ArrayList clinicals = new ArrayList();
                exams = dao.getMedicalExams((int)record.ID, null, "未缴费", null, null, null);
                if (record.DRUG_STATE == "待付药费")
                    drugs = dao.getPrescribeByMedicalRecord((int)record.ID);
                infusions = dao.getInfusion((int)record.ID, null, null, "未缴费");
                clinicals = dao.getClinical((int)record.ID, null, "未缴费");
                foreach(MEDICAL_EXAM exam in exams)
                {
                    DateTime time = exam.TIME.Value;
                    string datetime = time.Year.ToString() + "-" + time.Month.ToString() + "-" + time.Day.ToString()
                        + " " + time.Hour.ToString() + ":" + time.Minute.ToString();
                    JObject temp = new JObject();
                    temp.Add(new JProperty("rec_id", ((int)record.ID).ToString()));
                    temp.Add(new JProperty("item_id", ((int)exam.ITEM_ID).ToString()));
                    temp.Add(new JProperty("name", exam.EXAM_ITEM.NAME));
                    temp.Add(new JProperty("datetime", datetime));
                    temp.Add(new JProperty("amount", ((int)exam.EXAM_ITEM.EXPENSE).ToString()));
                    exam_res.Add(temp);
                }
                foreach (INFUSION infusion in infusions)
                {
                    DateTime time = record.TIME;
                    string datetime = time.Year.ToString() + "-" + time.Month.ToString() + "-" + time.Day.ToString()
                        + " " + time.Hour.ToString() + ":" + time.Minute.ToString();
                    JObject temp = new JObject();
                    temp.Add(new JProperty("rec_id", ((int)record.ID).ToString()));
                    temp.Add(new JProperty("item_id", ((int)infusion.ITEM_ID).ToString()));
                    temp.Add(new JProperty("drug_id", ((int)infusion.DRUG_ID).ToString()));
                    temp.Add(new JProperty("name", infusion.OPERATION.NAME));
                    temp.Add(new JProperty("datetime", datetime));
                    temp.Add(new JProperty("amount", ((int)infusion.OPERATION.PRICE).ToString()));
                    infusion_res.Add(temp);
                }
                foreach (CLINICAL clinical in clinicals)
                {
                    DateTime time = record.TIME;
                    string datetime = time.Year.ToString() + "-" + time.Month.ToString() + "-" + time.Day.ToString()
                        + " " + time.Hour.ToString() + ":" + time.Minute.ToString();
                    JObject temp = new JObject();
                    temp.Add(new JProperty("rec_id", ((int)record.ID).ToString()));
                    temp.Add(new JProperty("item_id", ((int)clinical.ITEM_ID).ToString()));
                    temp.Add(new JProperty("name", clinical.OPERATION.NAME));
                    temp.Add(new JProperty("datetime", datetime));
                    temp.Add(new JProperty("amount", ((int)clinical.OPERATION.PRICE).ToString()));
                    clinical_res.Add(temp);
                }
                foreach (PRESCRIBE drug in drugs)
                {
                    DateTime time = record.TIME;
                    string datetime = time.Year.ToString() + "-" + time.Month.ToString() + "-" + time.Day.ToString()
                        + " " + time.Hour.ToString() + ":" + time.Minute.ToString();
                    JObject temp = new JObject();
                    temp.Add(new JProperty("rec_id", ((int)record.ID).ToString()));
                    temp.Add(new JProperty("item_id", ((int)drug.DRUG_ID).ToString()));
                    temp.Add(new JProperty("name", drug.DRUG_INVENTORY.DRUG.NAME));
                    temp.Add(new JProperty("datetime", datetime));
                    temp.Add(new JProperty("amount", drug.EXPENSE));
                    drug_res.Add(temp);
                }
            }
            result.Add(new JProperty("id", id));
            result.Add(new JProperty("reg_result", reg_res));
            result.Add(new JProperty("exam_result", exam_res));
            result.Add(new JProperty("infusion_result",infusion_res));
            result.Add(new JProperty("clinical_result", clinical_res));
            result.Add(new JProperty("drug_result", drug_res));
            return result;
        }
        public JObject getPatientCount(string id, string patID)
        {
            Dao u = new Dao();
            ArrayList allPatients = u.getPatientById(int.Parse(patID));
            JObject result = new JObject();
            int amount = 0;
            foreach (PATIENT Patient in allPatients)
            {
                amount = u.getPatientCount(Patient);
            }
            result.Add(new JProperty("id", id));
            result.Add(new JProperty("amount", amount.ToString()));
            return result;
        }
        public JObject changePatientCount(string id, string patID, string addamount)
        {
            bool flag = true;
            ArrayList allPatients = dao.getPatientById(int.Parse(patID));
            JObject result = new JObject();
            int amount = 0;
            foreach (PATIENT Patient in allPatients)
            {
                amount = (int)Patient.COUNT;
                amount += int.Parse(addamount);
                if(!dao.changePatientCount((int)Patient.ID, amount))
                {
                    flag = false;
                }
            }
            result.Add(new JProperty("id", id));
            if (flag)
            {
                result.Add(new JProperty("result", "SUCCESS"));
            }
            else
            {
                result.Add(new JProperty("result", "FAIL"));
            }
            return result;
        }
    }
}
