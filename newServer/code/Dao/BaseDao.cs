using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;
using System.Threading;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using newServer;
namespace newserver
{
    class BaseDao
    {

        private Entities10 hospitalEntities;
        private EntitiesHelper entitiesHelper;
        public BaseDao()
        {
            entitiesHelper = new EntitiesHelper();
            hospitalEntities = entitiesHelper.getHospitalEntity();

        }
        /// <summary>
        /// BLOOD_BANK
        /// </summary>
        /// <param name="BloodType"></param>
        /// <param name="surplus"></param>
        /// <returns></returns>
        public IQueryable<BLOOD_BANK> getBloodRank(string BloodType, int? surplus)
        {
            try
            {
                var query = from Entity in hospitalEntities.BLOOD_BANK
                            where (BloodType == null || Entity.BLOOD_TYPE == BloodType) && (surplus == null || Entity.SURPLUS >= surplus)
                            select Entity;
                return query;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public bool removeBloodRank(string BloodType, int? surplus)
        {
            try
            {
                var query = from Entity in hospitalEntities.BLOOD_BANK
                            where (BloodType == null || Entity.BLOOD_TYPE == BloodType) && (surplus == null || Entity.SURPLUS >= surplus)
                            select Entity;
                foreach (var item in query)
                {
                    hospitalEntities.BLOOD_BANK.Remove(item);
                }
                hospitalEntities.SaveChanges();
                return true;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool setBloodRank(string BloodType_new, string BloodType_old, int? surplus_new)
        {
            try
            {
                if ((BloodType_new != null) && (BloodType_old != null))
                {
                    var query = from Entity in hospitalEntities.BLOOD_BANK
                                where Entity.BLOOD_TYPE == BloodType_old
                                select Entity;
                    foreach (var item in query)
                    {
                        if ((BloodType_new != null) && (surplus_new != null))
                        {
                            item.BLOOD_TYPE = BloodType_new;
                            item.SURPLUS = surplus_new.Value;
                        }
                        else
                            return false;
                    }
                    hospitalEntities.SaveChanges();
                    return true;
                }
                else
                    return false;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool addBloodRank(string BloodType, int? surplus)
        {
            try
            {
                BLOOD_BANK tmp = new BLOOD_BANK();
                if ((BloodType != null) && (surplus != null))
                {
                    tmp.BLOOD_TYPE = BloodType;
                    tmp.SURPLUS = surplus.Value;
                }
                else
                    return false;
                var query = from Entity in hospitalEntities.BLOOD_BANK
                            where Entity.BLOOD_TYPE == BloodType
                            select Entity;
                int i = 0;
                foreach (BLOOD_BANK pre in query)
                {
                    i++;
                }
                if (i == 0)
                {
                    hospitalEntities.BLOOD_BANK.Add(tmp);
                    hospitalEntities.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        /// <summary>
        /// DOCTOR
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Password"></param>
        /// <param name="Department_name"></param>
        /// <param name="Name"></param>
        /// <param name="Position"></param>
        /// <returns></returns>
        public IQueryable<DOCTOR> getDoctor(int? ID, string Password, string Department_name, string Name, string Position, DateTime? Lastlogin_time, DateTime? Creation_time, string Email)
        {
            try
            {
                var query = from Entity in hospitalEntities.DOCTOR
                            where (ID == null || Entity.ID == ID) && (Password == null || Entity.PASSWORD == Password) && (Department_name == null || Entity.DEPT_NAME == Department_name) &&
                            (Name == null || Entity.NAME == Name) && (Position == null || Entity.POSITION == Position) && (Lastlogin_time == null || Entity.LASTLOGIN_TIME == Lastlogin_time) &&
                            (Creation_time == null || Entity.CREATION_TIME == Creation_time) && (Email == null || Entity.E_MAIL == Email)
                            select Entity;
                return query;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        public bool removeDoctor(int? ID, string Password, string Department_name, string Name, string Position, DateTime? Lastlogin_time, DateTime? Creation_time, string Email)
        {
            try
            {
                var query = from Entity in hospitalEntities.DOCTOR
                            where (ID == null || Entity.ID == ID) && (Password == null || Entity.PASSWORD == Password) && (Department_name == null || Entity.DEPT_NAME == Department_name) &&
                        (Name == null || Entity.NAME == Name) && (Position == null || Entity.POSITION == Position) && (Lastlogin_time == null || Entity.LASTLOGIN_TIME == Lastlogin_time) &&
                        (Creation_time == null || Entity.CREATION_TIME == Creation_time) && (Email == null || Entity.E_MAIL == Email)
                            select Entity;
                foreach (var item in query)
                {
                    hospitalEntities.DOCTOR.Remove(item);
                }
                hospitalEntities.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        public bool setDoctor(int? ID, string Password, string Department_name, string Name, string Position, DateTime? Lastlogin_time, DateTime? Creation_time, string Email, string gender, int? age)
        {
            try
            {
                if (ID != null)
                {
                    var query = from Entity in hospitalEntities.DOCTOR
                                where Entity.ID == ID
                                select Entity;
                    foreach (var item in query)
                    {
                        if ((ID != null) && (Name != null) && (Password != null) && (Position != null) && (Department_name != null) && (Creation_time != null) && (Email != null) && (Lastlogin_time != null) && (gender != null) && (age != null))
                        {
                            item.ID = ID.Value;
                            item.NAME = Name;
                            item.PASSWORD = Password;
                            item.POSITION = Position;
                            item.DEPT_NAME = Department_name;
                            item.LASTLOGIN_TIME = Lastlogin_time.Value;
                            item.CREATION_TIME = Creation_time.Value;
                            item.E_MAIL = Email;
                            item.GENDER = gender;
                            item.AGE = age.Value;
                        }
                        else
                            return false;
                    }
                    hospitalEntities.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        public bool addDoctor(int? ID, string Password, string Department_name, string Name, string Position, DateTime? Lastlogin_time, DateTime? Creation_time, string Email, string gender, int? age)
        {
            try
            {
                DOCTOR tmp = new DOCTOR();
                if ((ID != null) && (Name != null) && (Password != null) && (Position != null) && (Department_name != null) && (Creation_time != null) && (Email != null) && (gender != null) && (age != null))
                {
                    tmp.ID = ID.Value;
                    tmp.NAME = Name;
                    tmp.PASSWORD = Password;
                    tmp.POSITION = Position;
                    tmp.DEPT_NAME = Department_name;
                    tmp.LASTLOGIN_TIME = Lastlogin_time.Value;
                    tmp.CREATION_TIME = Creation_time.Value;
                    tmp.E_MAIL = Email;
                    tmp.GENDER = gender;
                    tmp.AGE = age.Value;
                }
                else
                    return false;
                hospitalEntities.DOCTOR.Add(tmp);
                hospitalEntities.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        /// <summary>
        /// PAATIENT
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Password"></param>
        /// <param name="Explantion"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        public IQueryable<PATIENT> getPatient(int? ID, string Password, string Explantion, string Name, DateTime? Lastlogin_time, DateTime? Creation_time, string Email, DateTime? Birth_day, string identity, string gender)
        {
            try
            {
                var query = from Entity in hospitalEntities.PATIENT
                            where (ID == null || Entity.ID == ID) && (Password == null || Entity.PASSWORD == Password) && (Name == null || Entity.NAME == Name) && (gender == null || Entity.GENDER == gender) &&
                            (Explantion == null || Entity.EXPLANATION == Explantion) && (Lastlogin_time == null || Entity.LASTLOGIN_TIME == Lastlogin_time) && (Creation_time == null || Entity.CREATION_TIME == Creation_time)
                            && (Email == null || Entity.E_MAIL == Email) && (Birth_day == null || Entity.BIRTHDAY == Birth_day) && (identity == null || Entity.IDENTITY == identity)
                            select Entity;
                return query;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        public bool removePatient(int? ID, string Password, string Explantion, string Name, DateTime? Lastlogin_time, DateTime? Creation_time, string Email, DateTime? Birth_day, string identity, string gender)
        {
            try
            {
                var query = from Entity in hospitalEntities.PATIENT
                            where (ID == null || Entity.ID == ID) && (Password == null || Entity.PASSWORD == Password) && (Name == null || Entity.NAME == Name) && (gender == null || Entity.GENDER == gender) &&
                            (Explantion == null || Entity.EXPLANATION == Explantion) && (Lastlogin_time == null || Entity.LASTLOGIN_TIME == Lastlogin_time) && (Creation_time == null || Entity.CREATION_TIME == Creation_time)
                            && (Email == null || Entity.E_MAIL == Email) && (Birth_day == null || Entity.BIRTHDAY == Birth_day) && (identity == null || Entity.IDENTITY == identity)
                            select Entity;
                foreach (var item in query)
                {
                    hospitalEntities.PATIENT.Remove(item);
                }
                hospitalEntities.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        public bool setPatient(int? ID, string Password, string Explantion, string Name, DateTime? Lastlogin_time, DateTime? Creation_time, string Email, DateTime? Birth_day, string identity, string gender, int? Count)
        {
            try
            {
                if (ID != null)
                {
                    var query = from Entity in hospitalEntities.PATIENT
                                where Entity.ID == ID
                                select Entity;
                    foreach (var item in query)
                    {
                        if ((ID != null) && (Name != null) && (Password != null) && (Creation_time != null) && (Email != null) && (Lastlogin_time != null) && (Birth_day != null) && (identity != null) && (gender != null) && (Count != null))
                        {
                            item.ID = ID.Value;
                            item.NAME = Name;
                            item.PASSWORD = Password;
                            item.EXPLANATION = Explantion;
                            item.LASTLOGIN_TIME = Lastlogin_time.Value;
                            item.CREATION_TIME = Creation_time.Value;
                            item.E_MAIL = Email;
                            item.BIRTHDAY = Birth_day.Value;
                            item.IDENTITY = identity;
                            item.GENDER = gender;
                            item.AGE = DateTime.Now.Year - Birth_day.Value.Year;
                            item.COUNT = Count.Value;
                        }
                        else
                            return false;
                    }
                    hospitalEntities.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        public bool addPatient(int? ID, string Password, string Explantion, string Name, DateTime? Lastlogin_time, DateTime? Creation_time, string Email, DateTime? Birth_day, string identity, string gender, int? Count)
        {
            try
            {
                PATIENT tmp = new PATIENT();
                if ((ID != null) && (Name != null) && (Password != null) && (Creation_time != null) && (Email != null) && (Birth_day != null) && (identity != null) && (Count != null))
                {
                    tmp.ID = ID.Value;
                    tmp.NAME = Name;
                    tmp.PASSWORD = Password;
                    tmp.EXPLANATION = Explantion;
                    tmp.LASTLOGIN_TIME = Lastlogin_time.Value;
                    tmp.CREATION_TIME = Creation_time.Value;
                    tmp.E_MAIL = Email;
                    tmp.BIRTHDAY = Birth_day.Value;
                    tmp.IDENTITY = identity;
                    tmp.GENDER = gender;
                    tmp.AGE = DateTime.Now.Year - Birth_day.Value.Year;
                    tmp.COUNT = Count.Value;
                }
                else
                    return false;
                hospitalEntities.PATIENT.Add(tmp);
                hospitalEntities.SaveChanges();
                return true;
            }
            catch (Exception e)
            {

                Console.WriteLine(e);
                return false;
            }
        }
        /// <summary>
        /// DEPARTMENT
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="BuildId"></param>
        /// <param name="RoomId"></param>
        /// <returns></returns>
        public IQueryable<DEPARTMENT> getDepartment(string Name, int? BuildId, int? RoomId)
        {
            try
            {
                var query = from Entity in hospitalEntities.DEPARTMENT
                            where (Name == null || Entity.NAME == Name) && (BuildId == null || Entity.BUILDING_ID == BuildId) && (RoomId == null || Entity.ROOM_ID == RoomId)
                            select Entity;
                return query;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        public bool removeDepartment(string Name, int? BuildId, int? RoomId)
        {
            try
            {
                var query = from Entity in hospitalEntities.DEPARTMENT
                            where (Name == null || Entity.NAME == Name) && (BuildId == null || Entity.BUILDING_ID == BuildId) && (RoomId == null || Entity.ROOM_ID == RoomId)
                            select Entity;
                foreach (var item in query)
                {
                    hospitalEntities.DEPARTMENT.Remove(item);
                }
                hospitalEntities.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        public bool setDepartment(string Name, int? BuildId, int? RoomId)
        {
            try
            {
                if (Name != null)
                {
                    var query = from Entity in hospitalEntities.DEPARTMENT
                                where Entity.NAME == Name
                                select Entity;
                    foreach (var item in query)
                    {
                        if ((Name != null) && (BuildId != null) && (RoomId != null))
                        {
                            item.NAME = Name;
                            item.BUILDING_ID = BuildId.Value;
                            item.ROOM_ID = RoomId.Value;
                        }
                        else
                            return false;
                    }
                    hospitalEntities.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        public bool addDepartment(string Name, int? BuildId, int? RoomId)
        {
            try
            {
                if (Name == null)
                    return false;
                DEPARTMENT tmp = new DEPARTMENT();
                if ((Name != null) && (BuildId != null) && (RoomId != null))
                {
                    tmp.NAME = Name;
                    tmp.BUILDING_ID = BuildId.Value;
                    tmp.ROOM_ID = RoomId.Value;
                }
                else
                    return false;
                var query = from Entity in hospitalEntities.DEPARTMENT
                            where Entity.NAME == Name
                            select Entity;
                int i = 0;
                foreach (DEPARTMENT pre in query)
                {
                    i++;
                }
                if (i == 0)
                {
                    hospitalEntities.DEPARTMENT.Add(tmp);
                    hospitalEntities.SaveChanges();
                    return true;
                }
                else return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        /// <summary>
        /// REGISTRATION_RECORD
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="PatientID"></param>
        /// <param name="ScheduleID"></param>
        /// <param name="Expenses"></param>
        /// <param name="State"></param>
        /// <param name="Time"></param>
        /// <returns></returns>
        public IQueryable<REGISTRATION_RECORD> getReg_record(int? ID, int? PatientID, int? ScheduleID, int? Expenses, string State, DateTime? Time)
        {
            try
            {
                var query = from Entity in hospitalEntities.REGISTRATION_RECORD
                            where (ID == null || Entity.ID == ID) && (PatientID == null || Entity.PATIENT_ID == PatientID) && (ScheduleID == null || Entity.SCHEDULE_ID == ScheduleID) &&
                            (Expenses == null || Entity.EXPENSE == Expenses) && (State == null || Entity.STATE == State) && (Time == null || Entity.TIME == Time)
                            select Entity;
                return query;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        public bool removeReg_record(int? ID, int? PatientID, int? ScheduleID, int? Expenses, string State, DateTime? Time)
        {
            try
            {
                var query = from Entity in hospitalEntities.REGISTRATION_RECORD
                            where (PatientID == null || Entity.PATIENT_ID == PatientID) && (ScheduleID == null || Entity.SCHEDULE_ID == ScheduleID) &&
                            (Expenses == null || Entity.EXPENSE == Expenses) && (State == null || Entity.STATE == State) && (Time == null || Entity.TIME == Time)
                            select Entity;
                foreach (var item in query)
                {
                    hospitalEntities.REGISTRATION_RECORD.Remove(item);
                }
                hospitalEntities.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        public bool setReg_record(int? ID, int? PatientID, int? ScheduleID, int? Expenses, string State, DateTime? Time)
        {
            try
            {
                if (ID != null)
                {
                    var query = from Entity in hospitalEntities.REGISTRATION_RECORD
                                where Entity.ID == ID
                                select Entity;
                    foreach (var item in query)
                    {
                        if ((ID != null) && (PatientID != null) && (ScheduleID != null) && (Expenses != null) && (State != null) && (Time != null))
                        {
                            item.ID = ID.Value;
                            item.PATIENT_ID = PatientID.Value;
                            item.SCHEDULE_ID = ScheduleID.Value;
                            item.EXPENSE = Expenses.Value;
                            item.STATE = State;
                            item.TIME = Time.Value;
                        }
                        else
                            return false;
                    }
                    hospitalEntities.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        public bool addReg_record(int? ID, int? PatientID, int? ScheduleID, int? Expenses, string State, DateTime? Time)
        {
            try
            {
                REGISTRATION_RECORD tmp = new REGISTRATION_RECORD();
                if ((ID != null) && (PatientID != null) && (ScheduleID != null) && (Expenses != null) && (State != null) && (Time != null))
                {
                    tmp.ID = ID.Value;
                    tmp.PATIENT_ID = PatientID.Value;
                    tmp.SCHEDULE_ID = ScheduleID.Value;
                    tmp.EXPENSE = Expenses.Value;
                    tmp.STATE = State;
                    tmp.TIME = Time.Value;
                }
                else
                    return false;
                hospitalEntities.REGISTRATION_RECORD.Add(tmp);
                hospitalEntities.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        /// <summary>
        /// OPPOINTMENT
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="PatientID"></param>
        /// <param name="ScheduleID"></param>
        /// <param name="Expenses"></param>
        /// <param name="State"></param>
        /// <param name="Time"></param>
        /// <returns></returns>
        public IQueryable<OPPOINTMENT> getOppointment(int? ID, int? PatientID, int? ScheduleID, int? Expenses, string State, DateTime? Time)
        {
            try
            {
                var query = from Entity in hospitalEntities.OPPOINTMENT
                            where (ID == null || Entity.ID == ID) && (PatientID == null || Entity.PATIENT_ID == PatientID) && (ScheduleID == null || Entity.SCHEDULE_ID == ScheduleID) &&
                            (Expenses == null || Entity.EXPENSE == Expenses) && (State == null || Entity.STATE == State) && (Time == null || Entity.TIME == Time)
                            select Entity;
                return query;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        public bool removeOppointment(int? ID, int? PatientID, int? ScheduleID, int? Expenses, string State, DateTime? Time)
        {
            try
            {
                var query = from Entity in hospitalEntities.OPPOINTMENT
                            where (PatientID == null || Entity.PATIENT_ID == PatientID) && (ScheduleID == null || Entity.SCHEDULE_ID == ScheduleID) &&
                            (Expenses == null || Entity.EXPENSE == Expenses) && (State == null || Entity.STATE == State) && (Time == null || Entity.TIME == Time)
                            select Entity;
                foreach (var item in query)
                {
                    hospitalEntities.OPPOINTMENT.Remove(item);
                }
                hospitalEntities.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        public bool setOppointment(int? ID, int? PatientID, int? ScheduleID, int? Expenses, string State, DateTime? Time)
        {
            try
            {
                if (ID != null)
                {
                    var query = from Entity in hospitalEntities.OPPOINTMENT
                                where Entity.ID == ID
                                select Entity;
                    foreach (var item in query)
                    {
                        if ((ID != null) && (PatientID != null) && (ScheduleID != null) && (Expenses != null) && (State != null) && (Time != null))
                        {
                            item.ID = ID.Value;
                            item.PATIENT_ID = PatientID.Value;
                            item.SCHEDULE_ID = ScheduleID.Value;
                            item.EXPENSE = Expenses.Value;
                            item.STATE = State;
                            item.TIME = Time.Value;
                        }
                        else
                            return false;
                    }
                    hospitalEntities.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        public bool addOppointment(int? ID, int? PatientID, int? ScheduleID, int? Expenses, string State, DateTime? Time)
        {
            try
            {
                OPPOINTMENT tmp = new OPPOINTMENT();
                if ((ID != null) && (PatientID != null) && (ScheduleID != null) && (Expenses != null) && (State != null) && (Time != null))
                {
                    tmp.ID = ID.Value;
                    tmp.PATIENT_ID = PatientID.Value;
                    tmp.SCHEDULE_ID = ScheduleID.Value;
                    tmp.EXPENSE = Expenses.Value;
                    tmp.STATE = State;
                    tmp.TIME = Time.Value;
                }
                else
                    return false;
                hospitalEntities.OPPOINTMENT.Add(tmp);
                hospitalEntities.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        /// <summary>
        /// SCHEDULE
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Doctor_ID"></param>
        /// <param name="BeginTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="BuildingID"></param>
        /// <param name="RoomID"></param>
        /// <returns></returns>
        public IQueryable<SCHEDULE> getSchedule(int? ID, int? Doctor_ID, DateTime? BeginTime, DateTime? EndTime, int? BuildingID, int? RoomID)
        {
            try
            {
                var query = from Entity in hospitalEntities.SCHEDULE
                            where (ID == null || Entity.ID == ID) && (Doctor_ID == null || Entity.DOCTOR_ID == Doctor_ID) && (BeginTime == null || Entity.BEGIN_TIME == BeginTime) &&
                            (EndTime == null || Entity.END_TIME == EndTime) && (BuildingID == null || Entity.BUILDING_ID == BuildingID) && (RoomID == null || Entity.ROOM_ID == RoomID)
                            select Entity;
                return query;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public bool removeSchedule(int? ID, int? Doctor_ID, DateTime? BeginTime, DateTime? EndTime, int? BuildingID, int? RoomID)
        {
            try
            {
                var query = from Entity in hospitalEntities.SCHEDULE
                            where (ID == null || Entity.ID == ID) && (Doctor_ID == null || Entity.DOCTOR_ID == Doctor_ID) && (BeginTime == null || Entity.BEGIN_TIME == BeginTime) &&
                            (EndTime == null || Entity.END_TIME == EndTime) && (BuildingID == null || Entity.BUILDING_ID == BuildingID) && (RoomID == null || Entity.ROOM_ID == RoomID)
                            select Entity;
                foreach (var item in query)
                {
                    hospitalEntities.SCHEDULE.Remove(item);
                }
                hospitalEntities.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool setSchedule(int? ID, int? Doctor_ID, DateTime? BeginTime, DateTime? EndTime, int? BuildingID, int? RoomID)
        {
            try
            {
                if (ID != null)
                {
                    var query = from Entity in hospitalEntities.SCHEDULE
                                where Entity.ID == ID
                                select Entity;
                    foreach (var item in query)
                    {
                        if ((ID != null) && (Doctor_ID != null) && (BeginTime != null) && (EndTime != null) && (BuildingID != null) && (RoomID != null) && (EndTime != null))
                        {
                            item.ID = ID.Value;
                            item.DOCTOR_ID = Doctor_ID.Value;
                            item.BEGIN_TIME = BeginTime.Value;
                            item.END_TIME = EndTime.Value;
                            item.BUILDING_ID = BuildingID.Value;
                            item.ROOM_ID = RoomID.Value;
                        }
                        else
                            return false;
                    }
                    hospitalEntities.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool addSchedule(int? ID, int? Doctor_ID, DateTime? BeginTime, DateTime? EndTime, int? BuildingID, int? RoomID)
        {
            try
            {
                if (ID == null)
                    return false;
                SCHEDULE tmp = new SCHEDULE();
                if ((ID != null) && (Doctor_ID != null) && (BeginTime != null) && (EndTime != null) && (BuildingID != null) && (RoomID != null) && (EndTime != null))
                {
                    tmp.ID = ID.Value;
                    tmp.DOCTOR_ID = Doctor_ID.Value;
                    tmp.BEGIN_TIME = BeginTime.Value;
                    tmp.END_TIME = EndTime.Value;
                    tmp.BUILDING_ID = BuildingID.Value;
                    tmp.ROOM_ID = RoomID.Value;
                }
                else
                    return false;
                hospitalEntities.SCHEDULE.Add(tmp);
                hospitalEntities.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        /// <summary>
        /// EXAM_ITEM
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Name"></param>
        /// <param name="Expenses"></param>
        /// <param name="BuildingID"></param>
        /// <param name="RoomID"></param>
        /// <returns></returns>
        public IQueryable<EXAM_ITEM> getExamitem(int? ID, string Name, int? Expenses, int? BuildingID, int? RoomID)
        {
            try
            {
                var query = from Entity in hospitalEntities.EXAM_ITEM
                            where (ID == null || Entity.ID == ID) && (Name == null || Entity.NAME == Name) && (Expenses == null || Entity.EXPENSE == Expenses) &&
                            (BuildingID == null || Entity.BUILDING_ID == BuildingID) && (RoomID == null || Entity.ROOM_ID == RoomID)
                            select Entity;
                return query;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public bool removeExamitem(int? ID, string Name, int? Expenses, int? BuildingID, int? RoomID)
        {
            try
            {
                var query = from Entity in hospitalEntities.EXAM_ITEM
                            where (ID == null || Entity.ID == ID) && (Name == null || Entity.NAME == Name) && (Expenses == null || Entity.EXPENSE == Expenses) &&
                            (BuildingID == null || Entity.BUILDING_ID == BuildingID) && (RoomID == null || Entity.ROOM_ID == RoomID)
                            select Entity;
                foreach (var item in query)
                {
                    hospitalEntities.EXAM_ITEM.Remove(item);
                }
                hospitalEntities.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        /*public bool setExamitem(int? ID, string Name, int? Expenses, int? BuildingID, int? RoomID)
        {
            try
            {
                if (ID != null)
                {
                    var query = from Entity in hospitalEntities.EXAM_ITEM
                                where Entity.ID == ID
                                select Entity;
                    foreach (var item in query)
                    {
                        if ((ID != null) && (Name != null) && (Expenses != null) && (BuildingID != null) && (RoomID != null))
                        {
                            item.ID = ID.Value;
                            item.NAME = Name;
                            item.EXPENSE = Expenses.Value;
                            item.BUILDING_ID = BuildingID.Value;
                            item.ROOM_ID = RoomID.Value;
                        }
                        else
                            return false;
                    }
                    hospitalEntities.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool addExamitem(int? ID, string Name, int? Expenses, int? BuildingID, int? RoomID)
        {
            try
            {
                if (ID == null)
                    return false;
                EXAM_ITEM tmp = new EXAM_ITEM();
                if ((ID != null) && (Name != null) && (Expenses != null) && (BuildingID != null) && (RoomID != null))
                {
                    tmp.ID = ID.Value;
                    tmp.NAME = Name;
                    tmp.EXPENSE = Expenses.Value;
                    tmp.BUILDING_ID = BuildingID.Value;
                    tmp.ROOM_ID = RoomID.Value;
                }
                else
                    return false;
                hospitalEntities.EXAM_ITEM.Add(tmp);
                hospitalEntities.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }*/
        /// <summary>
        /// MEDICAL_RECORD
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Doctor_ID"></param>
        /// <param name="Patient_ID"></param>
        /// <param name="State"></param>
        /// <param name="ExamExpenses"></param>
        /// <param name="MediExpenses"></param>
        /// <returns></returns>
        public IQueryable<MEDICAL_RECORD> getMedicalrecord(int? ID, int? Doctor_ID, int? Patient_ID, string Treat_state, DateTime? Time, string Disease, string Description, string Diagnosis, string Clin_state, string Infu_state, string drug_state)
        {
            try
            {
                var query = from Entity in hospitalEntities.MEDICAL_RECORD
                            where (ID == null || Entity.ID == ID) && (Doctor_ID == null || Entity.DOCTOR_ID == Doctor_ID) && (Patient_ID == null || Entity.PATIENT_ID == Patient_ID) && 
                            (Diagnosis == null || Entity.DIAGNOSIS == Diagnosis) && (Treat_state == null || Entity.TREAT_STATE == Treat_state) && (Time == null || Entity.TIME == Time) && 
                            (Disease == null || Entity.DISEASE == Disease) && (Description == null || Entity.DESCRIPTION == Description) && (Clin_state == null || Entity.CLIN_STATE == Clin_state) && 
                            (Infu_state == null || Entity.INFU_STATE == Infu_state) && (drug_state == null || Entity.DRUG_STATE == drug_state)
                            select Entity;
                return query;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public bool removeMedicalrecord(int? ID, int? Doctor_ID, int? Patient_ID, string Treat_state, DateTime? Time, string Disease, string Description, string Diagnosis, string Clin_state, string Infu_state, string drug_state)
        {
            try
            {
                var query = from Entity in hospitalEntities.MEDICAL_RECORD
                            where (ID == null || Entity.ID == ID) && (Doctor_ID == null || Entity.DOCTOR_ID == Doctor_ID) && (Patient_ID == null || Entity.PATIENT_ID == Patient_ID) &&
                            (Diagnosis == null || Entity.DIAGNOSIS == Diagnosis) && (Treat_state == null || Entity.TREAT_STATE == Treat_state) && (Time == null || Entity.TIME == Time) &&
                            (Disease == null || Entity.DISEASE == Disease) && (Description == null || Entity.DESCRIPTION == Description) && (Clin_state == null || Entity.CLIN_STATE == Clin_state) &&
                            (Infu_state == null || Entity.INFU_STATE == Infu_state) && (drug_state == null || Entity.DRUG_STATE == drug_state)
                            select Entity;
                foreach (var item in query)
                {
                    hospitalEntities.MEDICAL_RECORD.Remove(item);
                }
                hospitalEntities.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool setMedicalrecord(int? ID, int? Doctor_ID, int? Patient_ID, string Treat_state, DateTime? Time, string Disease, string Description, string Diagnosis, string Clin_state, string Infu_state, string drug_state)
        {
            try
            {
                if (ID != null)
                {
                    var query = from Entity in hospitalEntities.MEDICAL_RECORD
                                where Entity.ID == ID
                                select Entity;

                    foreach (var item in query)
                    {
                        if ((ID != null) && (Doctor_ID != null) && (Patient_ID != null) && (Treat_state != null) && (Time != null) && (Clin_state != null) && (Infu_state != null) && (drug_state != null))
                        {
                            item.ID = ID.Value;
                            item.DOCTOR_ID = Doctor_ID.Value;
                            item.PATIENT_ID = Patient_ID.Value;
                            item.TREAT_STATE = Treat_state;
                            item.DISEASE = Disease;
                            item.DESCRIPTION = Description;
                            item.TIME = Time.Value;
                            item.DIAGNOSIS = Diagnosis;
                            item.CLIN_STATE = Clin_state;
                            item.INFU_STATE = Infu_state;
                            item.DRUG_STATE = drug_state;
                        }
                        else
                            return false;
                    }
                    hospitalEntities.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool addMedicalrecord(int? ID, int? Doctor_ID, int? Patient_ID, string Treat_state, DateTime? Time, string Disease, string Description, string Diagnosis, string Clin_state, string Infu_state, string drug_state)
        {
            try
            {
                MEDICAL_RECORD tmp = new MEDICAL_RECORD();
                if ((ID != null) && (Doctor_ID != null) && (Patient_ID != null) && (Treat_state != null) && (Time != null) && (Clin_state != null) && (Infu_state != null) && (drug_state != null)) 
                {
                    tmp.ID = ID.Value;
                    tmp.DOCTOR_ID = Doctor_ID.Value;
                    tmp.PATIENT_ID = Patient_ID.Value;
                    tmp.TREAT_STATE = Treat_state;
                    tmp.DISEASE = Disease;
                    tmp.DESCRIPTION = Description;
                    tmp.TIME = Time.Value;
                    tmp.DIAGNOSIS = Diagnosis;
                    tmp.CLIN_STATE = Clin_state;
                    tmp.INFU_STATE = Infu_state;
                    tmp.DRUG_STATE = drug_state;
                }
                else
                    return false;
                hospitalEntities.MEDICAL_RECORD.Add(tmp);
                hospitalEntities.SaveChanges();
                return true;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("as");
                return false;
            }
        }
        /// <summary>
        /// DRUG_INVENTORY
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Name"></param>
        /// <param name="Price"></param>
        /// <param name="Quantity"></param>
        /// <returns></returns>
        public IQueryable<DRUG_INVENTORY> getDruginventory(int? ID, int? Drug_ID, int? Price, int? Quantity)
        {
            try
            {
                var query = from Entity in hospitalEntities.DRUG_INVENTORY
                            where (ID == null || Entity.ID == ID) && (Drug_ID == null || Entity.DRUG_ID == Drug_ID) &&
                            (Price == null || Entity.PRICE == Price) && (Quantity == null || Entity.SURPLUS == Quantity)
                            select Entity;
                return query;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public bool removeDruginventory(int? ID, int? Drug_ID, int? Price, int? Quantity)
        {
            try
            {
                var query = from Entity in hospitalEntities.DRUG_INVENTORY
                            where (ID == null || Entity.ID == ID) && (Drug_ID == null || Entity.DRUG_ID == Drug_ID) &&
                            (Price == null || Entity.PRICE == Price) && (Quantity == null || Entity.SURPLUS == Quantity)
                            select Entity;
                foreach (var item in query)
                {
                    hospitalEntities.DRUG_INVENTORY.Remove(item);
                }
                hospitalEntities.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool setDruginventory(int? ID_old, int? ID_new, int? Drug_ID, int? Price, int? Quantity)
        {
            try
            {
                if (ID_old != null)
                {
                    var query = from Entity in hospitalEntities.DRUG_INVENTORY
                                where Entity.ID == ID_old
                                select Entity;
                    foreach (var item in query)
                    {
                        if ((ID_new != null) && (Drug_ID != null) && (Price != null) && (Quantity != null))
                        {
                            if(ID_new != ID_old)
                                item.ID = ID_new.Value;
                            item.DRUG_ID = Drug_ID.Value;
                            item.PRICE = Price.Value;
                            item.SURPLUS = Quantity.Value;
                        }
                        else
                            return false;
                    }
                    hospitalEntities.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool addDruginventory(int? ID, int? Drug_ID, int? Price, int? Quantity)
        {
            try
            {
                DRUG_INVENTORY tmp = new DRUG_INVENTORY();
                if ((ID != null) && (Drug_ID != null) && (Price != null) && (Quantity != null))
                {
                    tmp.ID = ID.Value;
                    tmp.DRUG_ID = Drug_ID.Value;
                    tmp.PRICE = Price.Value;
                    tmp.SURPLUS = Quantity.Value;
                }
                else
                    return false;
                hospitalEntities.DRUG_INVENTORY.Add(tmp);
                hospitalEntities.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        /// <summary>
        /// MEDICAL_INSTRUMENT
        /// </summary>
        /// <param name="instrumentName"></param>
        /// <returns></returns>
        public IQueryable<MEDICAL_INSTRUMENT> getInstruments(string instrumentName)
        {
            try
            {
                if (instrumentName == null)
                {
                    var queryOfAll = from Entity in hospitalEntities.MEDICAL_INSTRUMENT
                                     select Entity;
                    return queryOfAll;

                }
                else
                {
                    var queryOfone = from Entity in hospitalEntities.MEDICAL_INSTRUMENT
                                     where Entity.NAME == instrumentName
                                     select Entity;
                    return queryOfone;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        public bool addInstrument(string instrumentName, int? num)
        {
            try
            {
                MEDICAL_INSTRUMENT instrument = new MEDICAL_INSTRUMENT();
                instrument.NAME = instrumentName;
                instrument.AVAILABLE = num.Value;
                hospitalEntities.MEDICAL_INSTRUMENT.Add(instrument);
                hospitalEntities.SaveChanges();
                return true;
                ;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        public bool setInstrument(string oldname, string newname, int? num)
        {
            if (oldname != null)
            {
                var query = from Entity in hospitalEntities.MEDICAL_INSTRUMENT
                            where Entity.NAME == oldname
                            select Entity;
                if (newname == null || num < 0)
                    return false;
                foreach (var item in query)
                {

                    item.NAME = newname;
                    item.AVAILABLE = num.Value;
                    hospitalEntities.SaveChanges();
                }
                return true;
            }
            else
            {
                return false;
            }

        }
        public bool removeInstrument(string name, int? num)
        {
            try
            {
                if (name == null || num == null)
                {
                    return false;
                }
                var query = from Entity in hospitalEntities.MEDICAL_INSTRUMENT
                            where Entity.NAME == name
                            select Entity;


                foreach (var item in query)
                {
                    hospitalEntities.MEDICAL_INSTRUMENT.Remove(item);
                }
                hospitalEntities.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        /// <summary>
        /// MEDICAL_EXAM
        /// </summary>
        /// <param name="RecordID"></param>
        /// <param name="ItemID"></param>
        /// <param name="State"></param>
        /// <param name="Result"></param>
        /// <returns></returns>
        public IQueryable<MEDICAL_EXAM> getMedicalExam(int? RecordID, int? ItemID, string State, string Result, DateTime? Time, int? DocID)
        {
            try
            {
                var query = from Entity in hospitalEntities.MEDICAL_EXAM
                            where (RecordID == null || Entity.RECORD_ID == RecordID) && (ItemID == null || Entity.ITEM_ID == ItemID) && (DocID == null || Entity.DOCTOR_ID == DocID)
                            && (State == null || Entity.STATE == State) && (Result == null || Entity.RESULT == Result) && (Time == null || Entity.TIME == Time)
                            select Entity;
                return query;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public bool removeMedicalExam(int? RecordID, int? ItemID, string State, string Result, DateTime? Time, int? DocID)
        {
            try
            {
                var query = from Entity in hospitalEntities.MEDICAL_EXAM
                            where (RecordID == null || Entity.RECORD_ID == RecordID) && (ItemID == null || Entity.ITEM_ID == ItemID) && (DocID == null || Entity.DOCTOR_ID == DocID)
                            && (State == null || Entity.STATE == State) && (Result == null || Entity.RESULT == Result) && (Time == null || Entity.TIME == Time)
                            select Entity;
                foreach (var item in query)
                {
                    hospitalEntities.MEDICAL_EXAM.Remove(item);
                }
                hospitalEntities.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool setMedicalExam(int? RecordID_old, int? RecordID_new, int? ItemID_old, int? ItemID_new, string State, string Result, DateTime? Time, int? DocID)
        {
            try
            {
                if ((RecordID_old != null) && (ItemID_old != null))
                {
                    var query = from Entity in hospitalEntities.MEDICAL_EXAM
                                where (Entity.ITEM_ID == ItemID_old) && (Entity.RECORD_ID == RecordID_old)
                                select Entity;
                    foreach (var item in query)
                    {
                        if ((RecordID_new != null) && (ItemID_new != null) && (State != null))
                        {
                            item.RECORD_ID = RecordID_new.Value;
                            item.ITEM_ID = ItemID_new.Value;
                            item.RESULT = Result;
                            item.STATE = State;
                            item.TIME = Time.Value;
                            if(DocID != null)
                                item.DOCTOR_ID = DocID.Value;
                        }
                        else
                            return false;
                    }
                    hospitalEntities.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool addMedicalExam(int? RecordID, int? ItemID, string State, string Result, DateTime? Time, int? DocID)
        {
            try
            {
                MEDICAL_EXAM tmp = new MEDICAL_EXAM();
                if ((RecordID != null) && (ItemID != null) && (State != null))
                {
                    tmp.RECORD_ID = RecordID.Value;
                    tmp.ITEM_ID = ItemID.Value;
                    tmp.RESULT = Result;
                    tmp.STATE = State;
                    tmp.TIME = Time.Value;
                    if(DocID != null)
                        tmp.DOCTOR_ID = DocID.Value;
                }
                else
                    return false;
                var query = from Entity in hospitalEntities.MEDICAL_EXAM
                            where (Entity.RECORD_ID == RecordID) && (Entity.ITEM_ID == ItemID)
                            select Entity;
                int i = 0;
                foreach (MEDICAL_EXAM pre in query)
                {
                    i++;
                }
                if (i == 0)
                {
                    hospitalEntities.MEDICAL_EXAM.Add(tmp);
                    hospitalEntities.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        /// <summary>
        /// PRESCRIBE
        /// </summary>
        /// <param name="RecordID"></param>
        /// <param name="DrugID"></param>
        /// <returns></returns>
        public IQueryable<PRESCRIBE> getPrescribe(int? RecordID, int? DrugID, int? Quantity, int? Expense, string Eat_ways)
        {
            try
            {
                var query = from Entity in hospitalEntities.PRESCRIBE
                            where (RecordID == null || Entity.RECORD_ID == RecordID) && (DrugID == null || Entity.DRUG_ID == DrugID) && (Quantity == null || Entity.QUANTITY == Quantity)
                             && (Expense == null || Entity.EXPENSE == Expense) && (Eat_ways == null || Entity.EAT_WAYS == Eat_ways)
                            select Entity;
                return query;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public bool removePrescribe(int? RecordID, int? DrugID, int? Quantity, int? Expense, string Eat_ways)
        {
            try
            {
                var query = from Entity in hospitalEntities.PRESCRIBE
                            where (RecordID == null || Entity.RECORD_ID == RecordID) && (DrugID == null || Entity.DRUG_ID == DrugID) && (Quantity == null || Entity.QUANTITY == Quantity)
                             && (Expense == null || Entity.EXPENSE == Expense) && (Eat_ways == null || Entity.EAT_WAYS == Eat_ways)
                            select Entity;
                foreach (var item in query)
                {
                    hospitalEntities.PRESCRIBE.Remove(item);
                }
                hospitalEntities.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool addPrescribe(int? RecordID, int? DrugID, int? Quantity, int? Expense, string Eat_ways)
        {
            try
            {
                PRESCRIBE tmp = new PRESCRIBE();
                if ((RecordID != null) && (DrugID != null))
                {
                    tmp.RECORD_ID = RecordID.Value;
                    tmp.DRUG_ID = DrugID.Value;
                    tmp.QUANTITY = Quantity.Value;
                    tmp.EXPENSE = Expense.Value;
                    tmp.EAT_WAYS = Eat_ways;
                }
                else
                {
                    Console.WriteLine("zheli");
                    return false;
                }

                var query = from Entity in hospitalEntities.PRESCRIBE
                            where (Entity.RECORD_ID == RecordID) && (Entity.DRUG_ID == DrugID)
                            select Entity;
                int i = 0;
                foreach (PRESCRIBE pre in query)
                {
                    i++;
                }
                if (i == 0)
                {
                    hospitalEntities.PRESCRIBE.Add(tmp);
                    hospitalEntities.SaveChanges();
                    return true;
                }
                else
                {
                    Console.WriteLine("a");
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("b");
                Console.WriteLine(e);
                return false;
            }
        }

        /// <summary>
        /// MEDICAL_TREATEMENT
        /// </summary>
        /// <param name="record_ID"></param>
        /// <param name="suquence"></param>
        /// <param name="Title"></param>
        /// <param name="Description"></param>
        /// <returns></returns>
		public IQueryable<MEDICAL_TREATEMENT> getMedicalTreatement(int? record_ID, int? suquence, string Title, string Description)
        {
            try
            {
                var query = from Entity in hospitalEntities.MEDICAL_TREATEMENT
                            where (record_ID == null || Entity.RECORD_ID == record_ID) && (suquence == null || Entity.SEQUENCE == suquence) && (Title == null || Entity.TITLE == Title) && (Description == null || Entity.DESCRIPTION == Description)
                            select Entity;
                return query;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public bool removeMedicalTreatement(int? record_ID, int? suquence, string Title, string Description)
        {
            try
            {
                var query = from Entity in hospitalEntities.MEDICAL_TREATEMENT
                            where (record_ID == null || Entity.RECORD_ID == record_ID) && (suquence == null || Entity.SEQUENCE == suquence) && (Title == null || Entity.TITLE == Title) && (Description == null || Entity.DESCRIPTION == Description)
                            select Entity;
                foreach (var item in query)
                {
                    hospitalEntities.MEDICAL_TREATEMENT.Remove(item);
                }
                hospitalEntities.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        public bool setMedicalTreatement(int? record_ID, int? suquence, string Title, string Description)
        {
            try
            {
                if ((record_ID != null) && (suquence != null))
                {
                    var query = from Entity in hospitalEntities.MEDICAL_TREATEMENT
                                where (Entity.RECORD_ID == record_ID) && (Entity.TITLE == Title) && (Entity.DESCRIPTION == Description)
                                select Entity;
                    foreach (var item in query)
                    {
                        if ((record_ID != null) && (suquence != null) && (Title != null) && (Description != null))
                        {
                            item.RECORD_ID = record_ID.Value;
                            item.SEQUENCE = suquence.Value;
                            item.TITLE = Title;
                            item.DESCRIPTION = Description;
                        }
                        else
                            return false;
                    }
                    hospitalEntities.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool addMedicalTreatement(int? record_ID, int? sequence, string Title, string Description)
        {
            try
            {
                MEDICAL_TREATEMENT tmp = new MEDICAL_TREATEMENT();
                if ((record_ID != null) && (record_ID != null) && (Title != null))
                {
                    tmp.RECORD_ID = record_ID.Value;
                    tmp.SEQUENCE = sequence.Value;
                    tmp.TITLE = Title;
                    tmp.DESCRIPTION = Description;

                }
                else
                {
                    Console.WriteLine("a");
                    return false;
                }
                var query = from Entity in hospitalEntities.MEDICAL_TREATEMENT
                            where (Entity.RECORD_ID == record_ID) && (Entity.SEQUENCE == sequence)
                            select Entity;
                int i = 0;
                foreach (MEDICAL_TREATEMENT pre in query)
                {
                    i++;
                }
                if (i == 0)
                {
                    hospitalEntities.MEDICAL_TREATEMENT.Add(tmp);
                    hospitalEntities.SaveChanges();
                    return true;
                }
                else
                {
                    Console.WriteLine("b");
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("c");
                Console.WriteLine(e);
                return false;
            }


        }

        /// <summary>
        /// FOREGROUND_INFORMATION
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Patient_ID"></param>
        /// <param name="email"></param>
        /// <param name="Information"></param>
        /// <returns></returns>
        public IQueryable<FOREGROUND_INFORMATION> getFore_Information(int? ID, int? Patient_ID, string email, string Information, string state)
        {
            try
            {
                var query = from Entity in hospitalEntities.FOREGROUND_INFORMATION
                            where (ID == null || Entity.ID == ID) && (Patient_ID == null || Entity.PATIENT_ID == Patient_ID) && (email == null || Entity.E_MAIL == email) &&
                            (Information == null || Entity.INFORMATION == Information) && (state == null || Entity.STATE == state)
                            select Entity;
                if (query == null)
                    Console.WriteLine("null result");
                return query;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public bool removeFore_Information(int? ID, int? Patient_ID, string email, string Information, string state)
        {
            try
            {
                var query = from Entity in hospitalEntities.FOREGROUND_INFORMATION
                            where (ID == null || Entity.ID == ID) && (Patient_ID == null || Entity.PATIENT_ID == Patient_ID) && (email == null || Entity.E_MAIL == email) &&
                            (Information == null || Entity.INFORMATION == Information) && (state == null || Entity.STATE == state)
                            select Entity;
                foreach (var item in query)
                {
                    hospitalEntities.FOREGROUND_INFORMATION.Remove(item);
                }
                hospitalEntities.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool setFore_Information(int? ID, int? Patient_ID, string email, string Information, string state)
        {
            try
            {
                if (ID != null)
                {
                    var query = from Entity in hospitalEntities.FOREGROUND_INFORMATION
                                where Entity.ID == ID
                                select Entity;
                    foreach (var item in query)
                    {
                        if ((ID != null) && (Patient_ID != null) && (email != null) && (Information != null) && (state != null))
                        {
                            item.ID = ID.Value;
                            item.PATIENT_ID = Patient_ID.Value;
                            item.E_MAIL = email;
                            item.INFORMATION = Information;
                            item.STATE = state;
                        }
                        else
                            return false;
                    }
                    hospitalEntities.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool addFore_Information(int? ID, int? Patient_ID, string email, string Information, string state)
        {
            try
            {
                if (ID == null)
                    return false;
                FOREGROUND_INFORMATION tmp = new FOREGROUND_INFORMATION();
                if ((ID != null) && (Patient_ID != null) && (email != null) && (Information != null) && (state != null))
                {
                    tmp.ID = ID.Value;
                    tmp.PATIENT_ID = Patient_ID.Value;
                    tmp.E_MAIL = email;
                    tmp.INFORMATION = Information;
                    tmp.STATE = state;
                }
                else
                {
                    Console.WriteLine("a");
                    return false;
                }
                hospitalEntities.FOREGROUND_INFORMATION.Add(tmp);
                hospitalEntities.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        /// <summary>
        /// DRUG
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Name"></param>
        /// <param name="Code"></param>
        /// <param name="Standard"></param>
        /// <param name="Price"></param>
        /// <param name="Manufactor"></param>
        /// <param name="Prime"></param>
        /// <returns></returns>
        public IQueryable<DRUG> getDrug(int? ID, string Name, string Code, string Standard, int? Price, string Manufactor, int? Prime)
        {
            try
            {
                var query = from Entity in hospitalEntities.DRUG
                            where (ID == null || Entity.ID == ID) && (Name == null || Entity.NAME == Name) && (Standard == null || Entity.STANDARD == Standard) && (Code == null || Entity.CODE == Code) &&
                            (Price == null || Entity.PAURCH_PRICE == Price) && (Manufactor == null || Entity.MANUFACTOR == Manufactor) && (Prime == null || Entity.PRIME == Prime)
                            select Entity;
                if (query == null)
                    Console.WriteLine("null result");
                return query;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public bool removeDrug(int? ID, string Name, string Code, string Standard, int? Price, string Manufactor, int? Prime)
        {
            try
            {
                var query = from Entity in hospitalEntities.DRUG
                            where (ID == null || Entity.ID == ID) && (Name == null || Entity.NAME == Name) && (Standard == null || Entity.STANDARD == Standard) && (Code == null || Entity.CODE == Code) &&
                            (Price == null || Entity.PAURCH_PRICE == Price) && (Manufactor == null || Entity.MANUFACTOR == Manufactor) && (Prime == null || Entity.PRIME == Prime)
                            select Entity;
                foreach (var item in query)
                {
                    hospitalEntities.DRUG.Remove(item);
                }
                hospitalEntities.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool setDrug(int? ID_old, int? ID_new, string Name, string Code, string Standard, int? Price, string Manufactor, int? Prime)
        {
            try
            {
                if (ID_old != null)
                {
                    var query = from Entity in hospitalEntities.DRUG
                                where Entity.ID == ID_old
                                select Entity;
                    foreach (var item in query)
                    {
                        if ((ID_new != null) && (Name != null) && (Standard != null) && (Price != null) 
                            && (Manufactor != null) && (Prime != null) && (Code != null))
                        {
                            item.ID = ID_new.Value;
                            item.NAME = Name;
                            item.STANDARD = Standard;
                            item.PAURCH_PRICE = Price.Value;
                            item.MANUFACTOR = Manufactor;
                            item.CODE = Code;
                            item.PRIME = Prime.Value;
                        }
                        else
                            return false;
                    }
                    hospitalEntities.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool addDrug(int? ID, string Name, string Code, string Standard, int? Price, string Manufactor, int? Prime)
        {
            try
            {
                if (ID == null)
                    return false;
                DRUG tmp = new DRUG();
                if ((ID != null) && (Name != null) && (Standard != null) && (Price != null)
                            && (Manufactor != null) && (Prime != null) && (Code != null))
                {
                    tmp.ID = ID.Value;
                    tmp.NAME = Name;
                    tmp.STANDARD = Standard;
                    tmp.PAURCH_PRICE = Price.Value;
                    tmp.MANUFACTOR = Manufactor;
                    tmp.CODE = Code;
                    tmp.PRIME = Prime.Value;
                }
                else
                {
                    Console.WriteLine("a");
                    return false;
                }
                hospitalEntities.DRUG.Add(tmp);
                hospitalEntities.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        /// <summary>
        /// INVENTORY
        /// </summary>
        /// <param name="Number"></param>
        /// <param name="People"></param>
        /// <param name="Date"></param>
        /// <returns></returns>
        public IQueryable<INVENTORY> getInventory(int? Number, string People, string remark, DateTime? Date)
        {
            try
            {
                var query = from Entity in hospitalEntities.INVENTORY
                            where (Number == null || Entity.LIST_NUMBRE == Number) && (People == null || Entity.INVENTORY_PEOPLE == People) 
                            && (Date == null || Entity.INVENTORY_DATE == Date) && (remark == null || Entity.REMARK == remark)
                            select Entity;
                return query;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public bool removeInventory(int? Number, string People, string remark, DateTime? Date)
        {
            try
            {
                var query = from Entity in hospitalEntities.INVENTORY
                            where (Number == null || Entity.LIST_NUMBRE == Number) && (People == null || Entity.INVENTORY_PEOPLE == People)
                            && (Date == null || Entity.INVENTORY_DATE == Date) && (remark == null || Entity.REMARK == remark)
                            select Entity;
                foreach (var item in query)
                {
                    hospitalEntities.INVENTORY.Remove(item);
                }
                hospitalEntities.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool setInventory(int? Number, string People, string remark, DateTime? Date)
        {
            try
            {
                if (Number != null)
                {
                    var query = from Entity in hospitalEntities.INVENTORY
                                where Entity.LIST_NUMBRE == Number
                                select Entity;
                    foreach (var item in query)
                    {
                        if ((Number != null) && (People != null) && (Date != null))
                        {
                            item.LIST_NUMBRE = Number.Value;
                            item.INVENTORY_PEOPLE = People;
                            item.INVENTORY_DATE = Date.Value;
                            item.REMARK = remark;
                        }
                        else
                            return false;
                    }
                    hospitalEntities.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool addInventory(int? Number, string People, string remark, DateTime? Date)
        {
            try
            {
                INVENTORY tmp = new INVENTORY();
                if ((Number != null) && (People != null) && (Date != null))
                {
                    tmp.LIST_NUMBRE = Number.Value;
                    tmp.INVENTORY_PEOPLE = People;
                    tmp.INVENTORY_DATE = Date.Value;
                    tmp.REMARK = remark;
                }
                else
                    return false;
                hospitalEntities.INVENTORY.Add(tmp);
                hospitalEntities.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        /// <summary>
        /// INVENTORY_EXAMPLE
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Number"></param>
        /// <param name="quantity_old"></param>
        /// <param name="quantity_new"></param>
        /// <returns></returns>

        public IQueryable<INVENTORY_EXAMPLE> getInventoryExample(int? ID, int? Number, int? quantity_old, int? quantity_new)
        {
            try
            {
                var query = from Entity in hospitalEntities.INVENTORY_EXAMPLE
                            where (ID == null || Entity.ID == ID) && (Number == null || Entity.LIST_NUMBER == Number)
                            && (quantity_old == null || Entity.QUANTITY_OLD == quantity_old) && (quantity_new == null || Entity.QUANTITY_NEW == quantity_new)
                            select Entity;
                return query;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public bool removeInventoryExample(int? ID, int? Number, int? quantity_old, int? quantity_new)
        {
            try
            {
                var query = from Entity in hospitalEntities.INVENTORY_EXAMPLE
                            where (ID == null || Entity.ID == ID) && (Number == null || Entity.LIST_NUMBER == Number)
                            && (quantity_old == null || Entity.QUANTITY_OLD == quantity_old) && (quantity_new == null || Entity.QUANTITY_NEW == quantity_new)
                            select Entity;
                foreach (var item in query)
                {
                    hospitalEntities.INVENTORY_EXAMPLE.Remove(item);
                }
                hospitalEntities.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool setInventoryExample(int? ID, int? Number, int? quantity_old, int? quantity_new)
        {
            try
            {
                if ((ID != null) &&　(Number != null))
                {
                    var query = from Entity in hospitalEntities.INVENTORY_EXAMPLE
                                where (Entity.LIST_NUMBER == Number) && (Entity.ID == ID)
                                select Entity;
                    foreach (var item in query)
                    {
                        if ((Number != null) && (ID != null) && (quantity_old != null) && (quantity_new != null))
                        {
                            item.LIST_NUMBER = Number.Value;
                            item.ID = ID.Value;
                            item.QUANTITY_OLD = quantity_old.Value;
                            item.QUANTITY_NEW = quantity_new.Value;
                        }
                        else
                            return false;
                    }
                    hospitalEntities.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool addInventoryExample(int? ID, int? Number, int? quantity_old, int? quantity_new)
        {
            try
            {
                INVENTORY_EXAMPLE tmp = new INVENTORY_EXAMPLE();
                if ((Number != null) && (ID != null) && (quantity_old != null) && (quantity_new != null))
                {
                    tmp.LIST_NUMBER = Number.Value;
                    tmp.ID = ID.Value;
                    tmp.QUANTITY_OLD = quantity_old.Value;
                    tmp.QUANTITY_NEW = quantity_new.Value;
                }
                else
                    return false;
                hospitalEntities.INVENTORY_EXAMPLE.Add(tmp);
                hospitalEntities.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }

        /// <summary>
        /// CLINICAL
        /// </summary>
        /// <param name="record_ID"></param>
        /// <param name="item_ID"></param>
        /// <param name="numbers"></param>
        /// <param name="advice"></param>
        /// <param name="expense"></param>
        /// <param name="state"></param>
        /// <returns></returns>

        public IQueryable<CLINICAL> getClinical(int? record_ID, int? item_ID, int? numbers, string advice, int? expense, string state)
        {
            try
            {
                var query = from Entity in hospitalEntities.CLINICAL
                            where (record_ID == null || Entity.RECORD_ID == record_ID) && (item_ID == null || Entity.ITEM_ID == item_ID) && (numbers == null || Entity.NUMBERS == numbers)
                            && (advice == null || Entity.ADVICE == advice) && (expense == null || Entity.EXPENSE == expense) && (state == null || Entity.STATE == state)
                            select Entity;
                return query;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public bool removeClinicalt(int? record_ID, int? item_ID, int? numbers, string advice, int? expense, string state)
        {
            try
            {
                var query = from Entity in hospitalEntities.CLINICAL
                            where (record_ID == null || Entity.RECORD_ID == record_ID) && (item_ID == null || Entity.ITEM_ID == item_ID) && (numbers == null || Entity.NUMBERS == numbers)
                            && (advice == null || Entity.ADVICE == advice) && (expense == null || Entity.EXPENSE == expense) && (state == null || Entity.STATE == state)
                            select Entity;
                foreach (var item in query)
                {
                    hospitalEntities.CLINICAL.Remove(item);
                }
                hospitalEntities.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool setClinical(int? record_ID, int? item_ID, int? numbers, string advice, int? expense, string state)
        {
            try
            {
                if ((record_ID != null) && (item_ID != null))
                {
                    var query = from Entity in hospitalEntities.CLINICAL
                                where (Entity.RECORD_ID == record_ID) && (Entity.ITEM_ID == item_ID)
                                select Entity;
                    foreach (var item in query)
                    {
                        if ((record_ID != null) && (item_ID != null) && (numbers != null) && (expense != null) && (state != null))
                        {
                            item.RECORD_ID = record_ID.Value;
                            item.ITEM_ID = item_ID.Value;
                            item.NUMBERS = numbers.Value;
                            item.ADVICE = advice;
                            item.EXPENSE = expense.Value;
                            item.STATE = state;
                        }
                        else
                            return false;
                    }
                    hospitalEntities.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool addClinical(int? record_ID, int? item_ID, int? numbers, string advice, int? expense, string state)
        {
            try
            {
                CLINICAL tmp = new CLINICAL();
                if ((record_ID != null) && (item_ID != null) && (numbers != null) && (expense != null) && (state != null))
                {
                    tmp.RECORD_ID = record_ID.Value;
                    tmp.ITEM_ID = item_ID.Value;
                    tmp.NUMBERS = numbers.Value;
                    tmp.ADVICE = advice;
                    tmp.EXPENSE = expense.Value;
                    tmp.STATE = state;
                }
                else
                {
                    Console.WriteLine("a");
                    return false;
                }
                var query = from Entity in hospitalEntities.CLINICAL
                            where (Entity.RECORD_ID == record_ID) && (Entity.ITEM_ID == item_ID)
                            select Entity;
                int i = 0;
                foreach (CLINICAL pre in query)
                {
                    i++;
                }
                if (i == 0)
                {
                    hospitalEntities.CLINICAL.Add(tmp);
                    hospitalEntities.SaveChanges();
                    return true;
                }
                else
                {
                    Console.WriteLine("b");
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("c");
                Console.WriteLine(e);
                return false;
            }


        }

        /// <summary>
        /// INFUSION
        /// </summary>
        /// <param name="record_ID"></param>
        /// <param name="item_ID"></param>
        /// <param name="drug_ID"></param>
        /// <param name="expense"></param>
        /// <param name="state"></param>
        /// <returns></returns>

        public IQueryable<INFUSION> getInfusion(int? record_ID, int? item_ID, int? drug_ID, int? expense, string state, int? number)
        {
            try
            {
                var query = from Entity in hospitalEntities.INFUSION
                            where (record_ID == null || Entity.RECORD_ID == record_ID) && (item_ID == null || Entity.ITEM_ID == item_ID) && (drug_ID == null || Entity.DRUG_ID == drug_ID)
                            && (expense == null || Entity.EXPENSE == expense) && (state == null || Entity.STATE == state) && (number == null || Entity.NUMBERS == number)
                            select Entity;
                return query;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public bool removeInfusion(int? record_ID, int? item_ID, int? drug_ID, int? expense, string state, int? number)
        {
            try
            {
                var query = from Entity in hospitalEntities.INFUSION
                            where (record_ID == null || Entity.RECORD_ID == record_ID) && (item_ID == null || Entity.ITEM_ID == item_ID) && (drug_ID == null || Entity.DRUG_ID == drug_ID)
                            && (expense == null || Entity.EXPENSE == expense) && (state == null || Entity.STATE == state) && (number == null || Entity.NUMBERS == number)
                            select Entity;
                foreach (var item in query)
                {
                    hospitalEntities.INFUSION.Remove(item);
                }
                hospitalEntities.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool setInfusion(int? record_ID, int? item_ID, int? drug_ID, int? expense, string state, int? number)
        {
            try
            {
                if ((record_ID != null) && (item_ID != null) && (drug_ID != null))
                {
                    var query = from Entity in hospitalEntities.INFUSION
                                where (Entity.RECORD_ID == record_ID) && (Entity.ITEM_ID == item_ID) && (Entity.DRUG_ID == drug_ID)
                                select Entity;
                    foreach (var item in query)
                    {
                        if ((record_ID != null) && (item_ID != null) && (drug_ID != null) && (expense != null) && (state != null) && (number != null))
                        {
                            item.RECORD_ID = record_ID.Value;
                            item.ITEM_ID = item_ID.Value;
                            item.DRUG_ID = drug_ID.Value;
                            item.EXPENSE = expense.Value;
                            item.STATE = state;
                            item.NUMBERS = number.Value;
                        }
                        else
                            return false;
                    }
                    hospitalEntities.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool addInfusion(int? record_ID, int? item_ID, int? drug_ID, int? expense, string state, int? number)
        {
            try
            {
                INFUSION tmp = new INFUSION();
                if ((record_ID != null) && (item_ID != null) && (drug_ID != null) && (expense != null) && (state != null) && (number != null))
                {
                    tmp.RECORD_ID = record_ID.Value;
                    tmp.ITEM_ID = item_ID.Value;
                    tmp.DRUG_ID = drug_ID.Value;
                    tmp.EXPENSE = expense.Value;
                    tmp.STATE = state;
                    tmp.NUMBERS = number.Value;
                }
                else
                {
                    Console.WriteLine("a");
                    return false;
                }
                var query = from Entity in hospitalEntities.INFUSION
                            where (Entity.RECORD_ID == record_ID) && (Entity.ITEM_ID == item_ID) && (Entity.DRUG_ID == drug_ID)
                            select Entity;
                int i = 0;
                foreach (INFUSION pre in query)
                {
                    i++;
                }
                if (i == 0)
                {
                    hospitalEntities.INFUSION.Add(tmp);
                    hospitalEntities.SaveChanges();
                    return true;
                }
                else
                {
                    Console.WriteLine("b");
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("c");
                Console.WriteLine(e);
                return false;
            }


        }

        /// <summary>
        /// OPERATION
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="name"></param>
        /// <param name="Price"></param>
        /// <param name="code"></param>
        /// <returns></returns>

        public IQueryable<OPERATION> getOperation(int? ID, string name, int? Price, string code)
        {
            try
            {
                var query = from Entity in hospitalEntities.OPERATION
                            where (ID == null || Entity.ID == ID) && (name == null || Entity.NAME == name) &&
                            (Price == null || Entity.PRICE == Price) && (code == null || Entity.CODE == code)
                            select Entity;
                return query;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public bool removeOperation(int? ID, string name, int? Price, string code)
        {
            try
            {
                var query = from Entity in hospitalEntities.OPERATION
                            where (ID == null || Entity.ID == ID) && (name == null || Entity.NAME == name) &&
                            (Price == null || Entity.PRICE == Price) && (code == null || Entity.CODE == code)
                            select Entity;
                foreach (var item in query)
                {
                    hospitalEntities.OPERATION.Remove(item);
                }
                hospitalEntities.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool setOperation(int? ID, string name, int? Price, string code)
        {
            try
            {
                if (ID != null)
                {
                    var query = from Entity in hospitalEntities.OPERATION
                                where Entity.ID == ID
                                select Entity;
                    foreach (var item in query)
                    {
                        if ((ID != null) && (name != null) && (Price != null) && (code != null))
                        {
                            item.ID = ID.Value;
                            item.NAME = name;
                            item.PRICE = Price.Value;
                            item.CODE = code;
                        }
                        else
                            return false;
                    }
                    hospitalEntities.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool addOperation(int? ID, string name, int? Price, string code)
        {
            try
            {
                OPERATION tmp = new OPERATION();
                if ((ID != null) && (name != null) && (Price != null) && (code != null))
                {
                    tmp.ID = ID.Value;
                    tmp.NAME = name;
                    tmp.PRICE = Price.Value;
                    tmp.CODE = code;
                }
                else
                    return false;
                hospitalEntities.OPERATION.Add(tmp);
                hospitalEntities.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        /// <summary>
        /// NURSE
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Password"></param>
        /// <param name="Department_name"></param>
        /// <param name="Name"></param>
        /// <param name="Lastlogin_time"></param>
        /// <param name="Creation_time"></param>
        /// <param name="Email"></param>
        /// <returns></returns>

        public IQueryable<NURSE> getNurse(int? ID, string Password, string Department_name, string Name, DateTime? Lastlogin_time, DateTime? Creation_time, string Email)
        {
            try
            {
                var query = from Entity in hospitalEntities.NURSE
                            where (ID == null || Entity.ID == ID) && (Password == null || Entity.PASSWORD == Password) && (Department_name == null || Entity.DEPT_NAME == Department_name) &&
                            (Name == null || Entity.NAME == Name) && (Lastlogin_time == null || Entity.LASTLOGIN_TIME == Lastlogin_time) &&
                            (Creation_time == null || Entity.CREATION_TIME == Creation_time) && (Email == null || Entity.E_MAIL == Email)
                            select Entity;
                return query;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        public bool removeNurse(int? ID, string Password, string Department_name, string Name, DateTime? Lastlogin_time, DateTime? Creation_time, string Email)
        {
            try
            {
                var query = from Entity in hospitalEntities.NURSE
                            where (ID == null || Entity.ID == ID) && (Password == null || Entity.PASSWORD == Password) && (Department_name == null || Entity.DEPT_NAME == Department_name) &&
                        (Name == null || Entity.NAME == Name) && (Lastlogin_time == null || Entity.LASTLOGIN_TIME == Lastlogin_time) &&
                        (Creation_time == null || Entity.CREATION_TIME == Creation_time) && (Email == null || Entity.E_MAIL == Email)
                            select Entity;
                foreach (var item in query)
                {
                    hospitalEntities.NURSE.Remove(item);
                }
                hospitalEntities.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        public bool setNurse(int? ID, string Password, string Department_name, string Name, DateTime? Lastlogin_time, DateTime? Creation_time, string Email, string gender, int? age)
        {
            try
            {
                if (ID != null)
                {
                    var query = from Entity in hospitalEntities.NURSE
                                where Entity.ID == ID
                                select Entity;
                    foreach (var item in query)
                    {
                        if ((ID != null) && (Name != null) && (Password != null) && (Department_name != null) && (Creation_time != null) && (Email != null) && (Lastlogin_time != null) && (gender != null) && (age != null))
                        {
                            item.ID = ID.Value;
                            item.NAME = Name;
                            item.PASSWORD = Password;
                            item.DEPT_NAME = Department_name;
                            item.LASTLOGIN_TIME = Lastlogin_time.Value;
                            item.CREATION_TIME = Creation_time.Value;
                            item.E_MAIL = Email;
                            item.GENDER = gender;
                            item.AGE = age.Value;
                        }
                        else
                            return false;
                    }
                    hospitalEntities.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        public bool addNurse(int? ID, string Password, string Department_name, string Name, DateTime? Lastlogin_time, DateTime? Creation_time, string Email, string gender, int? age)
        {
            try
            {
                NURSE tmp = new NURSE();
                if ((ID != null) && (Name != null) && (Password != null) && (Department_name != null) && (Creation_time != null) && (Email != null) && (gender != null) && (age != null))
                {
                    tmp.ID = ID.Value;
                    tmp.NAME = Name;
                    tmp.PASSWORD = Password;
                    tmp.DEPT_NAME = Department_name;
                    tmp.LASTLOGIN_TIME = Lastlogin_time.Value;
                    tmp.CREATION_TIME = Creation_time.Value;
                    tmp.E_MAIL = Email;
                    tmp.GENDER = gender;
                    tmp.AGE = age.Value;
                }
                else
                    return false;
                hospitalEntities.NURSE.Add(tmp);
                hospitalEntities.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}
