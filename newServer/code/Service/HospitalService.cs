using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using newServer;
namespace newserver
{
    class HospitalService
    {
        private Dao dao;
        public HospitalService()
        {
            dao = new Dao();
        }
        public PATIENT Login(string email, string password, string random)
        {
            ArrayList result = dao.getOnlyPatient(email);
            if (result == null || result.Count <= 0)
            {
                return null;
            }
            else
            {
                if (code(((PATIENT)result[0]).PASSWORD + random).Equals(password.ToLower()))
                {

                    return (PATIENT)result[0];
                }
                else
                {
                    return null;
                }
            }

        }
        public bool changeDoctorPW(string email, string password)
        {
            int docID = dao.getDoctorIdByEmail(email);
            bool result = dao.changeDoctorPW(docID, password);
            return result;
        }
        public bool changeNursePW(string email, string password)
        {
            int nurID = dao.getNurseIdByEmail(email);
            bool result = dao.changeNursePW(nurID, password);
            return result;
        }
        public DOCTOR DoctorLogin(string email, string password, string random)
        {
            int ID = dao.getDoctorIdByEmail(email);
            DOCTOR result = dao.getDoctorsByID(ID);
            if (result != null)
            {
                if (code((result).PASSWORD + random).Equals(password.ToLower()))
                {

                    return result;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

        }
        public NURSE NurseLogin(string email, string password, string random)
        {
            int ID = dao.getNurseIdByEmail(email);
            NURSE result = dao.getNurseByID(ID);
            if (result != null)
            {
                if (code((result).PASSWORD + random).Equals(password.ToLower()))
                {

                    return result;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

        }
        public void updLastLogin(PATIENT patient)
        {
            patient.LASTLOGIN_TIME = DateTime.Now;
            dao.updPatientLastLoginTime(patient);
        }
        public void updDoctorLastLogin(DOCTOR doctor)
        {
            doctor.LASTLOGIN_TIME = DateTime.Now;
            dao.updDoctorLastLoginTime(doctor);
        }
        public void updNurserLastLogin(NURSE nurse)
        {
            nurse.LASTLOGIN_TIME = DateTime.Now;
            dao.updNurseLastLoginTime(nurse);
        }
        public bool Register(string name, string password, string email, string year, string month, string day, string identity, string gender)
        {
            bool result = dao.addPatient(name, password, email, year, month, day, identity, gender);
            return result;
        }
        public bool DoctorRegister(string name, string password, string email, string deptname, string position, string gender, string age)
        {
            bool result = dao.addDoctor(name, password, email, deptname, position, gender, int.Parse(age));
            return result;
        }
        public bool NurserRegister(string name, string password, string email, string deptname, string gender, string age)
        {
            bool result = dao.addNurse(name, password, email, deptname, gender, int.Parse(age));
            return result;
        }
        public bool RemoveDoctor(string email)
        {
            int docID = dao.getDoctorIdByEmail(email);
            bool result = dao.delDoctor(docID);
            return result;
        }
        public bool RemoveNurse(string email)
        {
            int nurID = dao.getNurseIdByEmail(email);
            bool result = dao.delNurse(nurID);
            return result;
        }
        public JObject getAllPatient(string id)
        {
            ArrayList temp = dao.getOnlyPatient(null);
            JArray res = new JArray();
            for (int i = 0; i < temp.Count; i++)
            {
                JObject tmp = new JObject();
                tmp.Add(new JProperty("name", ((PATIENT)temp[i]).NAME));
                tmp.Add(new JProperty("email", ((PATIENT)temp[i]).E_MAIL));
                res.Add(tmp);
            }
            JObject result = new JObject();
            result.Add(new JProperty("id", id));
            result.Add(new JProperty("result", res));
            return result;
        }
        public JObject getAllSpecial(string id)
        {
            ArrayList temp = dao.getAllDoctor();
            ArrayList temp2 = dao.getAllNurses();
            JArray res = new JArray();
            for (int i = 0; i < temp.Count; i++)
            {
                JObject tmp = new JObject();
                tmp.Add(new JProperty("name", ((DOCTOR)temp[i]).NAME));
                tmp.Add(new JProperty("email", ((DOCTOR)temp[i]).E_MAIL));
                tmp.Add(new JProperty("position", ((DOCTOR)temp[i]).POSITION));
                tmp.Add(new JProperty("department", ((DOCTOR)temp[i]).DEPT_NAME));
                tmp.Add(new JProperty("gender", ((DOCTOR)temp[i]).GENDER));
                tmp.Add(new JProperty("age", ((int)((DOCTOR)temp[i]).AGE).ToString()));
                res.Add(tmp);
            }
            for (int i = 0; i < temp2.Count; i++)
            {
                JObject tmp = new JObject();
                tmp.Add(new JProperty("name", ((NURSE)temp2[i]).NAME));
                tmp.Add(new JProperty("email", ((NURSE)temp2[i]).E_MAIL));
                tmp.Add(new JProperty("position", "护士"));
                tmp.Add(new JProperty("department", ((NURSE)temp2[i]).DEPT_NAME));
                tmp.Add(new JProperty("gender", ((NURSE)temp2[i]).GENDER));
                tmp.Add(new JProperty("age", ((int)((NURSE)temp2[i]).AGE).ToString()));
                res.Add(tmp);
            }
            JObject result = new JObject();
            result.Add(new JProperty("id", id));
            result.Add(new JProperty("result", res));
            return result;
        }
        public ArrayList findProfessor()
        {
            ArrayList final = new ArrayList();
            ArrayList result = dao.getDoctorsByPosition();
            foreach(DOCTOR doctor in result)
            {
                if(dao.getScheduleByDoctor(doctor.E_MAIL).Count != 0)
                {
                    final.Add(doctor);
                }
            }
            return final;
        }
        /*public JObject changePatientToDoc(string email, string position, string department, string id)
        {
            JObject result = new JObject();
            result.Add(new JProperty("id", id));
            if (dao.delPatientAndDoc(email, position, department))
            {
                result.Add(new JProperty("res", "success"));
            }
            else
            {
                result.Add(new JProperty("res", "fail"));
            }
            return result;
        }*/
        public string code(string beforeCoding)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            var hash_user = md5.ComputeHash(Encoding.Default.GetBytes(beforeCoding));
            var result = BitConverter.ToString(hash_user).Replace("-", string.Empty).ToLower();
            Console.WriteLine(result);
            return result;
        }
        public bool bookCertainProfessor(string patientemail, string expertemail)
        {
            int docid = dao.getDoctorIdByEmail(expertemail);
            ArrayList IDofSchedule = dao.getScheduleNumberOfDoctor(docid);
            int patientID = dao.getPatientIdByEmail(patientemail);
            Console.WriteLine("IDofSchedule  " + IDofSchedule.Count);
            for (int i = 0; i < IDofSchedule.Count; i++)
            {
                //排序
                int num = (int)IDofSchedule[i];
                Console.WriteLine("scheduleid  " + num);
                var temp = dao.getPatientWithSchedule(num);
                Console.WriteLine("temp.Count  " + temp.Count);
                if (temp.Count < 5)
                {

                    if (dao.addregRecord(patientID, num, 50))
                    {
                        Console.WriteLine("a");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("b");
                        return true;
                    }
                }

            }
            Console.WriteLine("c");
            return false;

        }
        public bool commonbook(string email)
        {
            ArrayList IDofSchedule = dao.getScheduleNumberOfDoctor(null);
            for (int i = 0; i < IDofSchedule.Count; i++)
            {
                //排序
                var temp = dao.getPatientWithSchedule((int)IDofSchedule[i]);
                if (temp.Count < 5)
                {


                    dao.addregRecord(dao.getPatientIdByEmail(email), (int)IDofSchedule[i], 8);
                    return true;
                }
            }
            return false;
        }
        public ArrayList getMedicalRecordOfPatient(int patientID)
        {
            return dao.getMedicalRecordsOfPatient(patientID);
        }
        public JObject getDrugByPatient(string email, string returnid)
        {
            int id = 0;
            ArrayList result = new ArrayList();
            JArray res = new JArray();
            foreach (PATIENT item in dao.getOnlyPatient(email))
            {
                id = (int)item.ID

;
            }
            ArrayList recordID = new ArrayList();
            foreach (MEDICAL_RECORD item in dao.getMedicalRecordsOfPatient(id))
            {
                recordID.Add((int)item.ID);

            }
            ArrayList drugID = new ArrayList();
            foreach (int item in recordID)
            {
                foreach (int i in dao.getPrescribeIDByMedicalRecord(item))
                {
                    drugID.Add(i);
                }
            }
            ArrayList drugs = dao.getAllDrugs();

            foreach (int item in drugID)
            {
                foreach (DRUG_INVENTORY drug in drugs)
                {
                    if (drug.ID == item)
                    {
                        string name = dao.getdrugNameByID((int)drug.DRUG_ID);
                        JObject temp = new JObject();
                        temp.Add(new JProperty("name", name));
                        temp.Add(new JProperty("price", drug.PRICE));
                        temp.Add(new JProperty("count", "1"));
                        res.Add(temp);
                    }
                }
            }
            JObject finalRes = new JObject();
            finalRes.Add("id", returnid);
            finalRes.Add("result", res);

            return finalRes;
        }
        public bool getMedicineByCount(string medicinename, int count)
        {
            ArrayList medicines = dao.getMedicinebyName(medicinename);
            bool result = true;
            for (int i = 0; i < medicines.Count; i++)
            {
                DRUG_INVENTORY line = (DRUG_INVENTORY)medicines[i];
                string name = dao.getdrugNameByID((int)line.DRUG_ID);
                if (!dao.changeMedicinebyCount((int)line.ID, (int)line.SURPLUS - count, (int)line.PRICE, name))
                {
                    result = false;
                }
            }
            return result;

        }

        public JObject getAllDepartments(string id)
        {

            ArrayList departments = dao.getAllDepartments();
            JArray res = new JArray();
            for (int i = 0; i < departments.Count; i++)
            {
                JObject temp = new JObject();
                temp.Add(new JProperty("name", ((DEPARTMENT)departments[i]).NAME));
                res.Add(temp);

            }
            JObject result = new JObject();
            result.Add(new JProperty("id", id));
            result.Add(new JProperty("result", res));
            return result;


        }
        public JObject getPatientInfo(string email, string id)
        {
            PATIENT tmp = (PATIENT)dao.getOnlyPatient(email)[0];
            PATIENT patient = tmp;
            DateTime birthday = patient.BIRTHDAY;
            string newtime = birthday.Year.ToString() + "年" + birthday.Month.ToString() + "月" + birthday.Day.ToString()
                    + "日";
            JObject result = new JObject();
            result.Add(new JProperty("id", id));
            result.Add(new JProperty("name", patient.NAME));
            result.Add(new JProperty("explanation", patient.EXPLANATION));
            result.Add(new JProperty("patid", patient.ID));
            result.Add(new JProperty("age", ((int)patient.AGE).ToString()));
            result.Add(new JProperty("email", patient.E_MAIL));
            result.Add(new JProperty("birthday", newtime));
            return result;

        }
        public JObject getFee(string email, string id)
        {
            int patientId = dao.getPatientIdByEmail(email);
            ArrayList reg_records = dao.getMedicalRecordsOfPatient(patientId);
            JObject result = new JObject();
            result.Add(new JProperty("id", id));
            JArray ja = new JArray();
            for (int i = 0; i < reg_records.Count; i++)
            {
                //if (((MEDICAL_RECORD)reg_records[i]).STATE != "全部完成")
                //{
                JObject tmp = new JObject(); int money = 0;
                ArrayList reg_prescribe = dao.getPrescribeIDByMedicalRecord(Decimal.ToInt32(((MEDICAL_RECORD)reg_records[i]).ID));
                for (int j = 0; j < reg_prescribe.Count; j++)
                {
                    money += Decimal.ToInt32(((PRESCRIBE)reg_prescribe[j]).EXPENSE);
                }
                tmp.Add(new JProperty("money", (money.ToString())));
                tmp.Add(new JProperty("date", ((MEDICAL_RECORD)reg_records[i]).TIME.ToLongDateString()));
                tmp.Add(new JProperty("state", ((MEDICAL_RECORD)reg_records[i]).TREAT_STATE));
                ja.Add(tmp);
                //}
            }
            result.Add(new JProperty("result", ja));
            return result;
        }
        //py
        public JArray getExam(string email)
        {
            JArray ja = new JArray();
            foreach (ArrayList item in dao.getExam(email))
            {
                JObject jo = new JObject();
                for (int i = 0; i < 4; ++i)
                {
                    switch (i)
                    {
                        case 0:
                            jo.Add(new JProperty("location", (string)item[i]));
                            break;
                        case 1:
                            jo.Add(new JProperty("time", (string)item[i]));
                            break;
                        case 2:
                            jo.Add(new JProperty("doctor", (string)item[i]));
                            break;
                        case 3:
                            jo.Add(new JProperty("name", (string)item[i]));
                            break;
                    }
                }
                ja.Add(jo);
            }
            return ja;
        }


    }
}
