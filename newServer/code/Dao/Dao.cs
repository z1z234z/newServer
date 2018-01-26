using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Data;
using System.Security.Cryptography;
using newServer;
namespace newserver
{
    class Dao
    {
        private Entities10 hospitalEntities;
        private BaseDao basedao;
        public Dao()
        {
            basedao = new BaseDao();
            hospitalEntities = new Entities10();
        }
        public ArrayList getOnlyPatient(string email)
        {
            var result = basedao.getPatient(null, null, null, null, null, null, email, null, null, null);
            ArrayList arraylist = new ArrayList();
            if (result.LongCount() == 0)
            {
                return null;
            }
            else if (result.LongCount() == 1)
            {
                foreach (PATIENT patient in result)
                {
                    arraylist.Add(patient);

                }
                return arraylist;

            }
            else if (email == null)
            {
                foreach (PATIENT patient in result)
                {
                    arraylist.Add(patient);

                }
                return arraylist;
            }
            return null;
        }
        public bool updPatientLastLoginTime(PATIENT patient)
        {
            return basedao.setPatient((int)patient.ID, patient.PASSWORD, patient.EXPLANATION, patient.NAME, patient.LASTLOGIN_TIME, patient.CREATION_TIME, patient.E_MAIL, patient.BIRTHDAY, patient.IDENTITY, patient.GENDER, (int)patient.COUNT);
        }
        public bool updDoctorLastLoginTime(DOCTOR doctor)
        {
            return basedao.setDoctor((int)doctor.ID, doctor.PASSWORD, doctor.DEPT_NAME, doctor.NAME, doctor.POSITION, doctor.LASTLOGIN_TIME, doctor.CREATION_TIME, doctor.E_MAIL, doctor.GENDER, (int)doctor.AGE);
        }
        public bool updNurseLastLoginTime(NURSE nurse)
        {
            return basedao.setNurse((int)nurse.ID, nurse.PASSWORD, nurse.DEPT_NAME, nurse.NAME, nurse.LASTLOGIN_TIME, nurse.CREATION_TIME, nurse.E_MAIL, nurse.GENDER, (int)nurse.AGE);
        }
        public bool addPatient(string name, string password, string email, string year, string month, string day, string identity, string gender)
        {
            DateTime now = DateTime.Now;
            DateTime birthday = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));
            bool result = basedao.addPatient(Math.Abs((int)now.ToFileTime()), password, "", name, now, now, email, birthday, identity, gender, 0);
            return result;

        }
        public bool addDoctor(string name, string password, string email, string deptname, string position, string gender, int age)
        {
            DateTime now = DateTime.Now;
            bool result = basedao.addDoctor(Math.Abs((int)now.ToFileTime()), password, deptname, name, position, now, now, email, gender, age);
            return result;

        }
        public bool addNurse(string name, string password, string email, string deptname, string gender, int age)
        {
            DateTime now = DateTime.Now;
            bool result = basedao.addNurse(Math.Abs((int)now.ToFileTime()), password, deptname, name, now, now, email, gender, age);
            return result;

        }
        public ArrayList getDoctorsByPosition()
        {
            var result = basedao.getDoctor(null, null, null, null, "主任", null, null, null);
            ArrayList arraylist = new ArrayList();
            foreach (DOCTOR doctor in result)
            {
                arraylist.Add(doctor);
            }
            var result1 = basedao.getDoctor(null, null, null, null, "副主任", null, null, null);
            foreach (DOCTOR doctor in result1)
            {
                arraylist.Add(doctor);
            }
            return arraylist;
        }
        public DOCTOR getDoctorsByID(int id)
        {
            var temp = basedao.getDoctor(id, null, null, null, null, null, null, null);
            DOCTOR result = null;
            try
            {
                result = temp.First();
            }
            catch (System.InvalidOperationException e)
            {
                Console.WriteLine(e);
                return null;
            }
            catch (System.ArgumentNullException e)
            {
                Console.WriteLine(e);
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
            return result;
        }
        public NURSE getNurseByID(int id)
        {
            var temp = basedao.getNurse(id, null, null, null, null, null, null);
            NURSE result = null;
            try
            {
                result = temp.First();
            }
            catch (System.InvalidOperationException e)
            {
                Console.WriteLine(e);
                return null;
            }
            catch (System.ArgumentNullException e)
            {
                Console.WriteLine(e);
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
            return result;
        }
        public ArrayList getAllDoctor()
        {
            var doctors = basedao.getDoctor(null, null, null, null, null, null, null, null);
            ArrayList result = new ArrayList();
            foreach (DOCTOR doctor in doctors)
            {
                result.Add(doctor);
            }
            return result;
        }
        public ArrayList getAllNurses()
        {
            var nurses = basedao.getNurse(null, null, null, null, null, null, null);
            ArrayList result = new ArrayList();
            foreach (NURSE nurse in nurses)
            {
                result.Add(nurse);
            }
            return result;
        }
        public ArrayList getScheduleNumberOfDoctor(int? docID)
        {
            var schedules = basedao.getSchedule(null, docID, null, null, null, null);
            schedules = schedules.AsQueryable().OrderBy(schedule => schedule.BEGIN_TIME);
            ArrayList result = new ArrayList();
            foreach (SCHEDULE schedule in schedules)
            {
                result.Add((int)schedule.ID);
            }
            return result;
        }
        public ArrayList getScheduleByDoctor(string email)
        {
            int docID = getDoctorIdByEmail(email);
            var schedules = basedao.getSchedule(null, docID, null, null, null, null);
            schedules = schedules.AsQueryable().OrderBy(schedule => schedule.BEGIN_TIME);
            ArrayList result = new ArrayList();
            foreach (SCHEDULE schedule in schedules)
            {
                result.Add(schedule);
            }
            return result;
        }
        public ArrayList getOppointmentByPat(string email)
        {
            int patID = getPatientIdByEmail(email);
            var oppointments = basedao.getOppointment(null, patID, null, null, null, null);
            oppointments = oppointments.AsQueryable().OrderBy(oppointment => oppointment.TIME);
            ArrayList result = new ArrayList();
            foreach (OPPOINTMENT oppointment in oppointments)
            {
                result.Add(oppointment);
            }
            return result;
        }
        public SCHEDULE getScheduleByID(int ID)
        {
            var schedules = basedao.getSchedule(ID, null, null, null, null, null);
            SCHEDULE result = null;
            try
            {
                result = schedules.First();
            }
            catch (System.InvalidOperationException e)
            {
                Console.WriteLine(e);
                return null;
            }
            catch (System.ArgumentNullException e)
            {
                Console.WriteLine(e);
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
            return result;
        }
        public ArrayList getPatientWithSchedule(int scheduleID)
        {
            var reg_records = basedao.getReg_record(null, null, scheduleID, null, "未就诊", null);
            ArrayList result = new ArrayList();
            foreach (REGISTRATION_RECORD reg_record in reg_records)
            {
                result.Add(reg_record);
            }
            return result;


        }
        public ArrayList getAllInstruments()
        {
            try
            {
                var instruments = basedao.getInstruments(null);
                ArrayList result = new ArrayList();
                foreach (var instrument in instruments)
                {
                    result.Add(instrument);
                }
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        public bool addInstrument(string name, int num)
        {
            return basedao.addInstrument(name, num);
        }
        public bool updInstrument(string oldname, string newname, int num)
        {
            return basedao.setInstrument(oldname, newname, num);
        }
        public bool delInstrument(string newname, int num)
        {
            return basedao.removeInstrument(newname, num);
        }
        /*public void addregRecord(int reg_ID,int patientID,int scheduleID,int fee)
		{
			basedao.addReg_record(reg_ID, patientID, scheduleID, fee, "未繳", DateTime.Now);
		}*/
        public bool delregRecord(int pat_id)
        {
            return basedao.removeReg_record(null, pat_id, null, null, null, null);
        }
        public ArrayList getAllBloods()
        {
            var bloods = basedao.getBloodRank(null, null);
            ArrayList result = new ArrayList();
            foreach (BLOOD_BANK blood in bloods)
            {
                result.Add(blood);
            }
            return result;
        }
        public bool addBlood(string name, int num)
        {
            return basedao.addBloodRank(name, num);
        }
        public bool updBlood(string oldname, string newname, int num)
        {
            return basedao.setBloodRank(newname, oldname, num);
        }
        public bool delBlood(string name, int num)
        {
            return basedao.removeBloodRank(name, num);
        }
        public bool delDoctor(int id)
        {
            return basedao.removeDoctor(id, null, null, null, null, null, null, null);
        }
        public bool delNurse(int id)
        {
            return basedao.removeNurse(id, null, null, null, null, null, null);
        }
        public ArrayList getAllDrugs()
        {
            var drugs = basedao.getDruginventory(null, null, null, null);
            ArrayList result = new ArrayList();
            foreach (DRUG_INVENTORY drug in drugs)
            {
                result.Add(drug);
            }
            return result;
        }
        public ArrayList getPatientDrugs()
        {
            var drugs = basedao.getDruginventory(null, null, null, null);
            ArrayList result = new ArrayList();
            foreach (DRUG_INVENTORY drug in drugs)
            {
                result.Add(drug);
            }
            return result;
        }
        public bool addDrugInventory(int id, string name, int? price, int? quantity)
        {
            int? drugID = null;
            if (name != null)
                drugID = getdrugIDByName(name);
            return basedao.addDruginventory(id, drugID, price, quantity);
        }
        public bool updDrugInventory(int oldId, int id, int drug_ID, int? price, int? quantity)
        {
            return basedao.setDruginventory(oldId, id, drug_ID, price, quantity);
        }
        public bool delDrugInventory(int? id, string name, int? price, int? quantity)
        {
            int? drugID = null;
            if (name != null)
                drugID = getdrugIDByName(name);
            return basedao.removeDruginventory(id, drugID, price, quantity);
        }
        public bool addDrug(int id, string name, string standard, int price, string manufactor, int prime)
        {
            return basedao.addDrug(id, name, PinyinHelper.GetChineseSpell(name), standard, price, manufactor, prime);
        }
        public bool updDrug(int oldId, int id, string name, string standard, int price, string manufactor, int prime)
        {
            return basedao.setDrug(oldId, id, name, PinyinHelper.GetChineseSpell(name), standard, price, manufactor, prime);
        }
        public bool delDrug(int id, string name, string standard, int? price, string manufactor, int? prime)
        {
            return basedao.removeDrug(id, name, PinyinHelper.GetChineseSpell(name), standard, price, manufactor, prime);
        }
        public bool addregRecord(int patientID, int scheduleID, int fee)
        {
            return basedao.addReg_record((int)DateTime.Now.ToFileTime(), patientID, scheduleID, fee, "未缴费", DateTime.Now);
        }
        public bool addoppointment(int patientID, int scheduleID, int fee, DateTime time)
        {
            return basedao.addOppointment((int)DateTime.Now.ToFileTime(), patientID, scheduleID, fee, "未就诊", time);
        }
        public ArrayList getMedicalRecordsOfPatient(int patientId)
        {
            var medicalRecords = basedao.getMedicalrecord(null, null, patientId, null, null, null, null, null, null, null, null);
            medicalRecords = medicalRecords.AsQueryable().OrderBy(medicalrecord => medicalrecord.TIME);
            ArrayList result = new ArrayList();
            foreach (MEDICAL_RECORD medicalrecord in medicalRecords)
            {
                result.Add(medicalrecord);
            }
            return result;
        }
        public ArrayList getMedicalSchemeOfRecord(int medicalRecordID)
        {
            var medicalRecords = basedao.getMedicalTreatement(medicalRecordID, null, null, null);
            ArrayList result = new ArrayList();
            foreach (MEDICAL_TREATEMENT medicalScheme in medicalRecords)
            {
                result.Add(medicalScheme);
            }
            return result;
        }
        public ArrayList getMedicalExams(int? medicalRecordID, int? itemID, string State, string Result, DateTime? Time, int? Doc_id)
        {
            var medicalExams = basedao.getMedicalExam(medicalRecordID, itemID, State, Result, Time, Doc_id);
            ArrayList result = new ArrayList();
            foreach (MEDICAL_EXAM medicalExam in medicalExams)
            {
                result.Add(medicalExam);
            }
            return result;
        }
        public bool addMedicalSchemeOfRecord(int medicalRecordID, int sequence, string type, string describe)
        {
            return basedao.addMedicalTreatement(medicalRecordID, sequence, type, describe);

        }
        /*public ArrayList getMedicalRecordsOfPatient(int patientId)
		{
			var medicalRecords = basedao.getMedicalrecord(null, null, patientId, null, null, null);
			ArrayList result = new ArrayList();
			foreach (MEDICAL_RECORD medicalrecord in medicalRecords)
			{
				result.Add(medicalrecord);
			}
			return result;
		}*/

        public ArrayList getPrescribeIDByMedicalRecord(int medicalRecordId)
        {
            ArrayList result = new ArrayList();
            foreach (var item in basedao.getPrescribe(medicalRecordId, null, null, null, null))
            {
                result.Add((int)item.DRUG_ID);
            }
            return result;

        }
        public ArrayList getPrescribeByMedicalRecord(int medicalRecordId)
        {
            ArrayList result = new ArrayList();
            foreach (var item in basedao.getPrescribe(medicalRecordId, null, null, null, null))
            {
                result.Add(item);
            }
            return result;

        }
        public ArrayList getMedicinebyName(string medicinename)
        {
            int drugID = getdrugIDByName(medicinename);
            var temp = basedao.getDruginventory(null, drugID, null, null);
            ArrayList result = new ArrayList();
            foreach (DRUG_INVENTORY drug in temp)
            {
                result.Add(drug);
            }
            return result;

        }
        public bool changePatientCount(int patID, int count)
        {
            PATIENT pat = (PATIENT)(getPatientById(patID)[0]);
            return basedao.setPatient(patID, pat.PASSWORD, pat.EXPLANATION, pat.NAME, pat.LASTLOGIN_TIME, pat.CREATION_TIME, pat.E_MAIL, pat.BIRTHDAY, pat.IDENTITY, pat.GENDER, count);
        }
        public bool newchangePatientCount(int? ID, int count)
        {
            if (ID != null)
            {
                var query = from Entity in hospitalEntities.PATIENT
                            where Entity.ID == ID
                            select Entity;
                foreach (var item in query)
                {
                        item.COUNT = count;
                }
                hospitalEntities.SaveChanges();
                return true;
            }
            else
                return false;
        }
        public bool changeDoctorPW(int docID, string password)
        {
            DOCTOR doc = getDoctorsByID(docID);
            return basedao.setDoctor(docID, password, doc.DEPT_NAME, doc.NAME, doc.POSITION, doc.LASTLOGIN_TIME, doc.CREATION_TIME, doc.E_MAIL, doc.GENDER, (int)doc.AGE);
        }
        public bool changeNursePW(int nurtID, string password)
        {
            NURSE nur = getNurseByID(nurtID);
            return basedao.setNurse(nurtID, password, nur.DEPT_NAME, nur.NAME, nur.LASTLOGIN_TIME, nur.CREATION_TIME, nur.E_MAIL, nur.GENDER, (int)nur.AGE);
        }
        public bool changeMedicinebyCount(int medicineID, int count, int price, string name)
        {
            int drugID = getdrugIDByName(name);
            return basedao.setDruginventory(medicineID, medicineID, drugID, price, count);
        }
        public bool changeMedicalRecord(int id, int doc_id, int pat_id, string state, DateTime time, string feature, string description, string suggest, string Clin_state, string infu_state, string drug_state)
        {
            return basedao.setMedicalrecord(id, doc_id, pat_id, state, time, feature, description, suggest, Clin_state, infu_state, drug_state);
        }
        public bool changeRegistRecord(int id, int pat_id, int schedule_id, int expense, string state, DateTime time)
        {
            return basedao.setReg_record(id, pat_id, schedule_id, expense, state, time);
        }
        public bool changeDurgInventory(int id_old, int id_new, int drug_id, int price, int quantiry)
        {
            return basedao.setDruginventory(id_old, id_new, drug_id, price, quantiry);
        }
        public bool changeMedicalExam(int rec_id, int item_id, string state, string result, DateTime time, int? doc_id)
        {
            return basedao.setMedicalExam(rec_id, rec_id, item_id, item_id, state, result, time, doc_id);
        }
        public bool changeInfusion(int rec_id, int item_id, int drug_id, int expense, string state, int number)
        {
            return basedao.setInfusion(rec_id, item_id, drug_id, expense, state, number);
        }
        public bool changeClinical(int rec_id, int item_id, int numbers, string advise, int expense, string state)
        {
            return basedao.setClinical(rec_id, item_id, numbers, advise, expense, state);
        }
        /*
        public bool addExam(int recordID, int ExamID)
        {
            return basedao.addMedicalExam(recordID, ExamID, "未缴费", null, DateTime.Now, null);
        }*/
        //gpy
        /*public bool delPatientAndDoc(string email, string position, string department)
        {
            var pat = getOnlyPatient(email);
            PATIENT temp;
            if (pat != null)
            {
                temp = (PATIENT)(getOnlyPatient(email)[0]);
            }
            else
            {
                return false;
            }
            if (!basedao.removePatient(getPatientIdByEmail(email), null, null, null, null, null, email, null, null, null))
            {
            	return false;
            }
            if (!basedao.addDoctor(getIndex(), temp.PASSWORD, department, temp.NAME, position, DateTime.Now, DateTime.Now, email))
            {
                return false;
            }
            return true;
        }*/
        public ArrayList getExam(string email)
        {
            ArrayList result = new ArrayList();
            int id = getPatientIdByEmail(email);
            ArrayList recordIds = getMeidicalRecordIdByPatientId(id);
            foreach (int index in recordIds)
            {
                ArrayList tmp = new ArrayList();
                var item = basedao.getMedicalExam(index, null, null, null, null, null);
                int itemId = -1;
                foreach (var itemid in item)
                {
                    itemId = (int)itemid.ITEM_ID;
                }
                int building = -1;
                int room = -1;
                string examName = "";
                foreach (EXAM_ITEM exam in basedao.getExamitem(itemId, null, null, null, null))
                {
                    examName = exam.NAME

;
                    building = (int)exam.BUILDING_ID;
                    room = (int)exam.ROOM_ID;
                }
                DateTime time = new DateTime();
                int docId = -1;
                foreach (MEDICAL_RECORD medical in basedao.getMedicalrecord(index, null, null, null, null, null, null, null, null, null, null))
                {
                    time = medical.TIME;
                    docId = (int)medical.DOCTOR_ID;
                }
                string docName = "";
                foreach (DOCTOR doc in basedao.getDoctor(docId, null, null, null, null, null, null, null))
                {
                    docName = doc.NAME

;
                }
                string location = building.ToString() + "-" + room.ToString();
                string Stime = time.ToLongDateString() + " " + time.ToLongTimeString();
                tmp.Add(location);
                tmp.Add(Stime);
                tmp.Add(docName);
                tmp.Add(examName);
                result.Add(tmp);
            }
            return result;
        }
        public ArrayList getExamItem(string name)
        {
            ArrayList result = new ArrayList();
            foreach (EXAM_ITEM item in basedao.getExamitem(null, name, null, null, null))
            {
                result.Add(item);
            }
            return result;
        }
        public ArrayList getOperationItem(string name)
        {
            ArrayList result = new ArrayList();
            foreach (OPERATION item in basedao.getOperation(null, name, null, null))
            {
                result.Add(item);
            }
            return result;
        }
        public int getPatientIdByEmail(string email)
        {
            int id = 0;
            foreach (PATIENT patient in basedao.getPatient(null, null, null, null, null, null, email, null, null, null))
            {
                id = (int)patient.ID

;
            }
            return id;
        }
        public string getPatientEmailById(int id)
        {
            string email = null;
            foreach (PATIENT patient in basedao.getPatient(id, null, null, null, null, null, null, null, null, null))
            {
                email = patient.E_MAIL;
            }
            return email;
        }
        public int getDoctorIdByEmail(string email)
        {
            int id = 0;
            foreach (DOCTOR doctor in basedao.getDoctor(null, null, null, null, null, null, null, email))
            {
                id = (int)doctor.ID;

            }
            return id;
        }

        public int getNurseIdByEmail(string email)
        {
            int id = 0;
            foreach (NURSE nurse in basedao.getNurse(null, null, null, null, null, null, email))
            {
                id = (int)nurse.ID;

            }
            return id;
        }
        public ArrayList getPatientById(int id)
        {
            ArrayList result = new ArrayList();
            foreach (PATIENT item in basedao.getPatient(id,null,null,null,null,null,null,null,null, null))
            {
                result.Add(item);
            }
            return result;
        }
        public int getPatientCount(PATIENT patient)
        {
            int count = 0;
            foreach (PATIENT item in basedao.getPatient((int)patient.ID, patient.PASSWORD, patient.EXPLANATION, patient.NAME, null, null, null, null, null, null))
            {
                count = (int)item.COUNT;
            }
            return count;
        }
        public ArrayList getPatientsByName(string name)
        {
            ArrayList result = new ArrayList();
            foreach (PATIENT item in basedao.getPatient(null, null, null, null, null, null, null, null, null, null))
            {
                if (item.NAME.Contains(name))
                {
                    result.Add(item);
                }
            }
            return result;
        }
        public ArrayList getMeidicalRecordIdByPatientId(int id)
        {
            ArrayList result = new ArrayList();
            foreach (MEDICAL_RECORD item in getMedicalRecordsOfPatient(id))
            {
                result.Add((int)item.ID);
            }
            return result;
        }
        public ArrayList getAllDepartments()
        {
            var temp = basedao.getDepartment(null, null, null);
            ArrayList result = new ArrayList();
            foreach (DEPARTMENT department in temp)
            {
                result.Add(department);
            }
            return result;
        }
        public ArrayList getMedicalRecordByDoctor(int? docID)
        {
            var medicalRecords = basedao.getMedicalrecord(null, docID, null, null, null, null, null, null, null, null, null);
            ArrayList result = new ArrayList();
            foreach (MEDICAL_RECORD medicalRecord in medicalRecords)
            {
                result.Add(medicalRecord);
            }
            return result;
        }
        public ArrayList getMedicalRecordByPat(int? patID)
        {
            var medicalRecords = basedao.getMedicalrecord(null, null, patID, null, null, null, null, null, null, null, null);
            ArrayList result = new ArrayList();
            foreach (MEDICAL_RECORD medicalRecord in medicalRecords)
            {
                result.Add(medicalRecord);
            }
            return result;
        }
        public ArrayList getMedicalRecordsByID(int? recID, int? PatID)
        {
            var medicalRecords = basedao.getMedicalrecord(recID, null, PatID, null, null, null, null, null, null, null, null);
            ArrayList result = new ArrayList();
            foreach (MEDICAL_RECORD medicalRecord in medicalRecords)
            {
                result.Add(medicalRecord);
            }
            return result;
        }
        public MEDICAL_RECORD getMedicalRecordByID(int recID)
        {
            var medicalRecords = basedao.getMedicalrecord(recID, null, null, null, null, null, null, null, null, null, null);
            MEDICAL_RECORD result = null;
            try
            {
                result = medicalRecords.First();
            }
            catch (System.InvalidOperationException e)
            {
                Console.WriteLine(e);
                return null;
            }
            catch (System.ArgumentNullException e)
            {
                Console.WriteLine(e);
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
            return result;
        }
        public ArrayList getMedicalTreatmentByID(int recID)
        {
            var medicaltreatments = basedao.getMedicalTreatement(recID, null, null, null);
            medicaltreatments = medicaltreatments.AsQueryable().OrderBy(medicaltreatment => medicaltreatment.SEQUENCE);
            ArrayList result = new ArrayList();
            foreach (MEDICAL_TREATEMENT medicaltreatment in medicaltreatments)
            {
                result.Add(medicaltreatment);
            }
            return result;
        }
        public ArrayList getMedicalRecordByDoctorAndPatient(int? docID, int? patID)
        {
            var medicalRecords = basedao.getMedicalrecord(null, docID, patID, null, null, null, null, null, null, null, null);
            ArrayList result = new ArrayList();
            DateTime time = new DateTime(1900,1,1);
            foreach (MEDICAL_RECORD medicalRecord in medicalRecords)
            {
                if (time.CompareTo(medicalRecord.TIME) < 0)
                {
                    if(time.Year == 1900)
                    {
                        result.Add(medicalRecord);
                        time = medicalRecord.TIME;
                    }
                    else
                    {
                        result.RemoveAt(0);
                        result.Add(medicalRecord);
                        time = medicalRecord.TIME;
                    }
                }
                
            }
            return result;
        }
        public bool delMedicalTreatment(int record_ID, int suquence, string Title, string Description)
        {
            return basedao.removeMedicalTreatement(record_ID, suquence, Title, Description);
        }
        public bool addMedicalRecordOfDoc(int doc_id, int pat_id)
        {
            return basedao.addMedicalrecord(Math.Abs((int)DateTime.Now.ToFileTime()), doc_id, pat_id, "开始诊断", DateTime.Now, null, null, null, "未选择", "未选择", "未选择");
        }
        public bool addMedicalExam(int record_id, int item_id)
        {
            return basedao.addMedicalExam(record_id, item_id, "未缴费", null, DateTime.Now, null);
        }
        public bool addClinical(int record_id, int item_id, int num, string advice, int expense)
        {
            return basedao.addClinical(record_id, item_id, num, advice, expense, "未缴费");
        }
        public bool addInfusion(int record_id, int item_id, int drug_id, int expense, int number)
        {
            return basedao.addInfusion(record_id, item_id, drug_id, expense, "未缴费", number);
        }
        public int getdrugInventoryIDByName(string name)
        {
            int drugID = getdrugIDByName(name);
            var temp = basedao.getDruginventory(null, drugID, null, null);
            foreach (DRUG_INVENTORY drug in temp)
            {
                return (int)drug.ID;
            }
            return -1;

        }
        public DRUG_INVENTORY getdrugInventoryByID(int id)
        {
            var temp = basedao.getDruginventory(id, null, null, null);
            DRUG_INVENTORY result = null;
            try
            {
                result = temp.First();
            }
            catch (System.InvalidOperationException e)
            {
                Console.WriteLine(e);
                return null;
            }
            catch (System.ArgumentNullException e)
            {
                Console.WriteLine(e);
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
            return result;

        }
        public int getdrugIDByName(string name)
        {
            var temp = basedao.getDrug(null, name, null, null, null, null, null);
            foreach (DRUG drug in temp)
            {
                return (int)drug.ID;
            }
            return -1;

        }
        public ArrayList getdrugsByName(string name)
        {
            var drugs = basedao.getDrug(null, null, null, null, null, null, null);
            ArrayList result = new ArrayList();
            foreach (DRUG drug in drugs)
            {
                if(drug.NAME.Contains(name))
                    result.Add(drug);
            }
            return result;
        }
        public string getdrugNameByID(int id)
        {
            var temp = basedao.getDrug(id, null, null, null, null, null, null);
            foreach (DRUG drug in temp)
            {
                return drug.NAME;
            }
            return null;

        }
        public DRUG getdrugByID(int id)
        {
            var temp = basedao.getDrug(id, null, null, null, null, null, null);
            DRUG result = null;
            try
            {
                result = temp.First();
            }
            catch (System.InvalidOperationException e)
            {
                Console.WriteLine(e);
                return null;
            }
            catch (System.ArgumentNullException e)
            {
                Console.WriteLine(e);
                return null;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
            return result;

        }
        public int getdrugPriceByID(int id)
        {
            var temp = basedao.getDruginventory(id, null, null, null);
            foreach (DRUG_INVENTORY drug in temp)
            {
                return (int)drug.PRICE;
            }
            return -1;

        }
        public int getExamIDByName(string name)
        {
            var temp = basedao.getExamitem(null, name, null, null, null);
            foreach (EXAM_ITEM exam in temp)
            {
                return (int)exam.ID;
            }
            return -1;

        }
        public bool addPrescription(int medicalID, int drugID, int quantity, int expense, string eat_way)
        {
            return basedao.addPrescribe(medicalID, drugID, quantity, expense, eat_way);
        }
        public bool delPrescription(int medicalID, int drugID)
        {
            return basedao.removePrescribe(medicalID, drugID, null, null, null);
        }
        private static object lockOj = new object();
        private static int index = 1;
        private int getIndex()
        {
            lock (lockOj)
            {
                return index++;
            }
        }

        public ArrayList getAllRoreInfo()
        {
            var temp = basedao.getFore_Information(null, null, null, null, null);
            ArrayList result = new ArrayList();
            foreach (FOREGROUND_INFORMATION fore_info in temp)
            {
                result.Add(fore_info);
            }
            return result;
        }
        public ArrayList getPatientRoreInfo(string email)
        {
            var temp = basedao.getFore_Information(null, null, email, null, null);
            ArrayList result = new ArrayList();
            foreach (FOREGROUND_INFORMATION fore_info in temp)
            {
                result.Add(fore_info);
            }
            return result;
        }
        public ArrayList getRoreInfoByPatientID(int patID)
        {
            var temp = basedao.getFore_Information(null, patID, null, null, null);
            ArrayList result = new ArrayList();
            foreach (FOREGROUND_INFORMATION fore_info in temp)
            {
                result.Add(fore_info);
            }
            return result;
        }
        public bool addRoreInfo(int patID, string email, string information)
        {
            return basedao.addFore_Information((int)(DateTime.Now.ToFileTime()), patID, email, information, "前台给病人");
        }
        public bool addPatientRoreInfo(int patID, string email, string information)
        {
            return basedao.addFore_Information((int)(DateTime.Now.ToFileTime()), patID, email, information, "病人给前台");
        }
        public ArrayList getAllRegRecord()
        {
            var temp = basedao.getReg_record(null, null, null, null, null, null);
            temp = temp.AsQueryable().OrderBy(record => record.ID);
            ArrayList result = new ArrayList();
            foreach (REGISTRATION_RECORD record in temp)
            {
                result.Add(record);
            }
            return result;
        }
        public ArrayList getRegRecordByPatAndState(int? patID, string state)
        {
            var temp = basedao.getReg_record(null, patID, null, null, state, null);
            temp = temp.AsQueryable().OrderBy(record => record.ID);
            ArrayList result = new ArrayList();
            foreach (REGISTRATION_RECORD record in temp)
            {
                result.Add(record);
            }
            return result;
        }
        public REGISTRATION_RECORD getRegRecordByID(int? ID)
        {
            var temp = basedao.getReg_record(ID, null, null, null, null, null);
            foreach (REGISTRATION_RECORD record in temp)
            {
                return record;
            }
            return null;
        }
        public ArrayList getOppointmentPatAndState(int? patID, string state)
        {
            var temp = basedao.getOppointment(null, patID, null, null, state, null);
            temp = temp.AsQueryable().OrderBy(record => record.ID);
            ArrayList result = new ArrayList();
            foreach (OPPOINTMENT record in temp)
            {
                result.Add(record);
            }
            return result;
        }
        public string getPatientNameByID(int id)
        {
            var temp = basedao.getPatient(id, null, null, null, null, null, null, null, null, null);
            foreach (PATIENT patient in temp)
            {
                return (string)patient.NAME;
            }
            return null;
        }
        public int? getPatientIDByName(string name)
        {
            if (name != null)
            {
                var temp = basedao.getPatient(null, null, null, name, null, null, null, null, null, null);
                foreach (PATIENT patient in temp)
                {
                    return (int)patient.ID;
                }
                return null;
            }
            else
                return null;
        }
        public string getDepartmentNameBySchedualID(int id)
        {
            var temp_schedule = basedao.getSchedule(id, null, null, null, null, null);
            foreach (SCHEDULE schedule in temp_schedule)
            {
                var temp_doctor = basedao.getDoctor((int)schedule.DOCTOR_ID, null, null, null, null, null, null, null);
                foreach (DOCTOR doctor in temp_doctor)
                {
                    return doctor.DEPT_NAME;
                }
            }
            return null;
        }
        public int getScheduleIDByDept(string dept)
        {
            int id = -1;
            foreach (DOCTOR doctor in basedao.getDoctor(null, null, dept, null, null, null, null, null))
            {
                foreach (SCHEDULE schedule in basedao.getSchedule(null, (int)doctor.ID, null, null, null, null))
                {
                    if((DateTime.Now.CompareTo(schedule.BEGIN_TIME) > 0) && (DateTime.Now.CompareTo(schedule.END_TIME) < 0))
                    {
                        id = (int)schedule.ID;
                        return id;
                    }
                }
            }
            return id;
        }

        public ArrayList getInventory(int? Number, string People, string remark, DateTime? Date)
        {
            var temp = basedao.getInventory(Number, People, remark, Date);
            ArrayList result = new ArrayList();
            foreach (INVENTORY exam in temp)
            {
                result.Add(exam);
            }
            return result;

        }
        public bool addInventory(int? Number, string People, string remark, DateTime? Date)
        {
            return basedao.addInventory(Number, People, remark, Date);
        }
        public bool delInventory(int? Number, string People, string remark, DateTime? Date)
        {
            return basedao.removeInventory(Number, People, remark, Date);
        }

        public ArrayList getInventoryExample(int? ID, int? Number, int? quantity_old, int? quantity_new)
        {
            var temp = basedao.getInventoryExample(ID, Number, quantity_old, quantity_new);
            ArrayList result = new ArrayList();
            foreach (INVENTORY_EXAMPLE exam in temp)
            {
                result.Add(exam);
            }
            return result;

        }
        public bool addInventoryExample(int? ID, int? Number, int? quantity_old, int? quantity_new)
        {
            return basedao.addInventoryExample(ID, Number, quantity_old, quantity_new);
        }
        public bool delInventoryExample(int? ID, int? Number, int? quantity_old, int? quantity_new)
        {
            return basedao.removeInventoryExample(ID, Number, quantity_old, quantity_new);
        }
        public ArrayList getInfusion(int? recID, int? itemID, int? drugID, string state)
        {
            var infusions = basedao.getInfusion(recID, itemID, drugID, null, state, null);
            ArrayList result = new ArrayList();
            foreach (INFUSION infusion in infusions)
            {
                result.Add(infusion);
            }
            return result;
        }
        public ArrayList getClinical(int? recID, int? itemID, string state)
        {
            var clinicals = basedao.getClinical(recID, itemID, null, null, null, state);
            ArrayList result = new ArrayList();
            foreach (CLINICAL clinical in clinicals)
            {
                result.Add(clinical);
            }
            return result;
        }
        public OPERATION getOperation(int ID)
        {
            ArrayList result = new ArrayList();
            foreach (OPERATION item in basedao.getOperation(ID, null, null, null))
            {
                return item;
            }
            return null;

        }
    }
}