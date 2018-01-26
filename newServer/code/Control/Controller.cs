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

    class Controller
    {

        private string operation;
        private Socket socket;
        private HospitalService hospitalService;
        public Controller(Socket sockets)
        {
            socket = sockets;
            hospitalService = new HospitalService();
        }
        public void startListening()
        {
            //socket.Emit("java_database", new object[] { "record" });

            Console.WriteLine("login listening");
            socket.On("login_apply", (data) =>
           {
                try
                {
                    /*Console.WriteLine("received login");
                    socket.Emit("login_reply", new Object[] { "success"});*/
                    JObject jobe = (JObject)data;
                    string id = (string)jobe.GetValue("id");
                    string email = (string)jobe.GetValue("email");
                    string password = (string)jobe.GetValue("password");
                    string random = (string)jobe.GetValue("random");
                    string type = (string)jobe.GetValue("type");
                    PATIENT user = hospitalService.Login(email, password, random);
                    JObject result = new JObject();
                    if (user == null)
                    {
                        result.Add(new JProperty("id", id));
                        result.Add(new JProperty("userId", ""));
                        result.Add(new JProperty("result", "failed"));
                        socket.Emit("login_reply", result);
                    }
                    else
                    {
                        result.Add(new JProperty("id", id));
                        result.Add(new JProperty("userId", ((int)user.ID).ToString()));
                        result.Add(new JProperty("result", "success"));
                        result.Add(new JProperty("lastLogintime", user.LASTLOGIN_TIME.ToString()));
                        result.Add(new JProperty("createTime", user.CREATION_TIME.ToString()));
                        result.Add(new JProperty("type", type));
                        socket.Emit("login_reply", result);
                        hospitalService.updLastLogin(user);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("login_apply: \n");
                    Console.WriteLine(e);
                }

            });
            socket.On("web_login_doctor_apply", (data) =>
            {
                try
                {
                    /*Console.WriteLine("received login");
                    socket.Emit("login_reply", new Object[] { "success"});*/
                    JObject jobe = (JObject)data;
                    string id = (string)jobe.GetValue("id");
                    string email = (string)jobe.GetValue("email");
                    string password = (string)jobe.GetValue("password");
                    string random = (string)jobe.GetValue("random");
                    string type = (string)jobe.GetValue("type");
                    DOCTOR user = hospitalService.DoctorLogin(email, password, random);
                    JObject result = new JObject();
                    if (user == null)
                    {
                        result.Add(new JProperty("id", id));
                        result.Add(new JProperty("result", "failed"));
                        socket.Emit("web_login_doctor_reply", result);
                    }
                    else
                    {
                        result.Add(new JProperty("id", id));
                        result.Add(new JProperty("userId", ((int)user.ID).ToString()));
                        result.Add(new JProperty("result", "success"));
                        result.Add(new JProperty("lastLogintime", user.LASTLOGIN_TIME.ToString()));
                        result.Add(new JProperty("createTime", user.CREATION_TIME.ToString()));
                        result.Add(new JProperty("type", type));
                        socket.Emit("web_login_doctor_reply", result);
                        hospitalService.updDoctorLastLogin(user);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_login_doctor_apply: \n");
                    Console.WriteLine(e);
                }

            });
            socket.On(" web_login_nurse_apply", (data) =>
            {
                try
                {
                    /*Console.WriteLine("received login");
                    socket.Emit("login_reply", new Object[] { "success"});*/
                    JObject jobe = (JObject)data;
                    string id = (string)jobe.GetValue("id");
                    string email = (string)jobe.GetValue("email");
                    string password = (string)jobe.GetValue("password");
                    string random = (string)jobe.GetValue("random");
                    string type = (string)jobe.GetValue("type");
                    NURSE user = hospitalService.NurseLogin(email, password, random);
                    JObject result = new JObject();
                    if (user == null)
                    {
                        result.Add(new JProperty("id", id));
                        result.Add(new JProperty("result", "failed"));
                        socket.Emit("web_login_nurse_reply", result);
                    }
                    else
                    {
                        result.Add(new JProperty("id", id));
                        result.Add(new JProperty("userId", ((int)user.ID).ToString()));
                        result.Add(new JProperty("result", "success"));
                        result.Add(new JProperty("lastLogintime", user.LASTLOGIN_TIME.ToString()));
                        result.Add(new JProperty("createTime", user.CREATION_TIME.ToString()));
                        result.Add(new JProperty("type", type));
                        socket.Emit("web_login_nurse_reply", result);
                        hospitalService.updNurserLastLogin(user);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_login_nurse_apply: \n");
                    Console.WriteLine(e);
                }

            });
            //data 已改变
            socket.On("signup_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string id = (string)jobe.GetValue("id");
                    string name = (string)jobe.GetValue("name");
                    string password = (string)jobe.GetValue("password");
                    string email = (string)jobe.GetValue("email");
                    string year = (string)jobe.GetValue("year");
                    string month = (string)jobe.GetValue("month");
                    string day = (string)jobe.GetValue("day");
                    string identity = (string)jobe.GetValue("identity");
                    string gender = (string)jobe.GetValue("gender");
                    var result = hospitalService.Register(name, password, email, year, month, day, identity, gender);
                    // todo email may has been used
                    JObject jobj = new JObject();
                    jobj.Add(new JProperty("id", id));
                    jobj.Add(new JProperty("result", result ? "success" : "failed"));
                    socket.Emit("signup_reply", jobj);
                }
                catch (Exception e)
                {
                    Console.WriteLine("signup_apply: \n");
                    Console.WriteLine(e);
                }
            });
            socket.On("web_get_department_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string id = (string)jobe.GetValue("id");
                    socket.Emit("web_get_department_reply", hospitalService.getAllDepartments(id));
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_get_department_apply: \n");
                    Console.WriteLine(e);
                }

            });
            socket.On("web_get_expert_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string id = (string)jobe.GetValue("id");
                    ArrayList professors = hospitalService.findProfessor();
                    JArray res = new JArray();
                    for (int i = 0; i < professors.Count; i++)
                    {
                        JObject temp = new JObject();
                        temp.Add(new JProperty("name", ((DOCTOR)professors[i]).NAME));
                        temp.Add(new JProperty("position", ((DOCTOR)professors[i]).POSITION));
                        temp.Add(new JProperty("email", ((DOCTOR)professors[i]).E_MAIL));
                        temp.Add(new JProperty("department", ((DOCTOR)professors[i]).DEPT_NAME));
                        res.Add(temp);
                    }
                    JObject result = new JObject();
                    result.Add(new JProperty("id", id));
                    result.Add(new JProperty("result", res));
                    socket.Emit("web_get_expert_reply", result);
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_get_expert_apply: \n");
                    Console.WriteLine(e);
                }

            });
            /*socket.On("web_add_expert_appoint_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string id = (string)jobe.GetValue("id");
                    string email = (string)jobe.GetValue("email");
                    string expertemail = (string)jobe.GetValue("expert");
                    bool res = hospitalService.bookCertainProfessor(email, expertemail);
                    JObject result = new JObject();
                    result.Add(new JProperty("id", id));

                    if (res)
                    {
                        result.Add(new JProperty("res", "success"));
                        socket.Emit("web_add_expert_appoint_reply", result);
                    }
                    else
                    {
                        result.Add(new JProperty("res", "fail"));
                        socket.Emit("web_add_expert_appoint_reply", result);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_add_expert_appoint_apply: \n");
                    Console.WriteLine(e);
                }

            });*/
            socket.On("web_add_booking_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string id = (string)jobe.GetValue("id");
                    string email = (string)jobe.GetValue("email");
                    bool res = hospitalService.commonbook(email);
                    JObject result = new JObject();
                    result.Add(new JProperty("id", id));
                    if (res)
                    {
                        result.Add(new JProperty("res", "success"));
                        socket.Emit("web_add_booking_reply", result);
                    }
                    else
                    {
                        result.Add(new JProperty("book_result", "fail"));
                        socket.Emit("web_add_booking_reply", result);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_add_booking_apply: \n");
                    Console.WriteLine(e);
                }

            });
            socket.On("show_treatementRecord_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string id = (string)jobe.GetValue("id");
                    int patientID = (int)jobe.GetValue("pID");
                    var records = hospitalService.getMedicalRecordOfPatient(patientID);
                    JObject result = new JObject();
                    result.Add(new JProperty("id", id));
                    JArray res = new JArray();
                    for (int i = 0; i < records.Count; i++)
                    {
                        JObject temp = new JObject();
                        temp.Add(new JProperty("TIME", ((MEDICAL_RECORD)records[i]).TIME));
                        temp.Add(new JProperty("DISEASE", ((MEDICAL_RECORD)records[i]).DISEASE));
                        temp.Add(new JProperty("DESCRIPTION", ((MEDICAL_RECORD)records[i]).DESCRIPTION));
                        temp.Add(new JProperty("DIAGNOSIS", ((MEDICAL_RECORD)records[i]).DIAGNOSIS));
                        temp.Add(new JProperty("STATE", ((MEDICAL_RECORD)records[i]).TREAT_STATE));
                        res.Add(temp);
                    }
                    result.Add(new JProperty("medical_record", res));
                    socket.Emit("show_treatementRecord_reply", result);
                }
                catch (Exception e)
                {
                    Console.WriteLine("show_treatementRecord_apply: \n");
                    Console.WriteLine(e);
                }

            });

            socket.On("web_pick_medicine_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string id = (string)jobe.GetValue("id");
                    string email = (string)jobe.GetValue("patient");
                    string medicine = (string)jobe.GetValue("medicine");
                    int number = (int)jobe.GetValue("count");
                    JObject res = new JObject();
                    res.Add(new JProperty("id", id));
                    //if (hospitalService.getMedicineByCount(medicine, number))
                    //{
                    socket.Emit("web_pick_medicine_reply", res);
                    socket.Emit("java_drug_alert_reply", jobe);
                    //}
                    //else
                    //{
                    //	Console.WriteLine("fail");
                    //}

                    //查找病人和相关信息返回给JAVA
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_pick_medicine_apply: \n");
                    Console.WriteLine(e);
                }

            });

            socket.On("web_get_medicine_list_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string id = (string)jobe.GetValue("id");
                    string email = (string)jobe.GetValue("patient");
                    socket.Emit("web_get_medicine_list_reply", hospitalService.getDrugByPatient(email, id));
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_get_medicine_list_apply: \n");
                    Console.WriteLine(e);
                }

            });


            socket.On("web_get_exam_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string id = (string)jobe.GetValue("id");
                    string email = (string)jobe.GetValue("patient");
                    JObject result = new JObject();
                    result.Add(new JProperty("id", id));
                    result.Add(new JProperty("result", hospitalService.getExam(email)));
                    socket.Emit("web_get_exam_reply", result);
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_get_exam_apply: \n");
                    Console.WriteLine(e);
                }

            });
            socket.On("web_get_fee_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string id = (string)jobe.GetValue("id");
                    string email = (string)jobe.GetValue("email");
                    socket.Emit("web_get_fee_reply", hospitalService.getFee(email, id));
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_get_fee_apply: \n");
                    Console.WriteLine(e);
                }

            });
            socket.On("web_get_patient_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string email = (string)jobe.GetValue("email");
                    string id = (string)jobe.GetValue("id");
                    socket.Emit("web_get_patient_reply", hospitalService.getPatientInfo(email, id));
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_get_patient_apply: \n");
                    Console.WriteLine(e);
                }

            });
            socket.On("web_get_all_patient_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string id = (string)jobe.GetValue("id");
                    socket.Emit("web_get_all_patient_reply", hospitalService.getAllPatient(id));
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_get_all_patient_apply: \n");
                    Console.WriteLine(e);
                }

            });
            /*socket.On("web_change_right_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string id = (string)jobe.GetValue("id");
                    string email = (string)jobe.GetValue("email");
                    string position = (string)jobe.GetValue("position");
                    string department = (string)jobe.GetValue("department");
                    socket.Emit("web_change_right_reply", hospitalService.changePatientToDoc(email, position, department, id));
                }catch(Exception e)
                {
                    Console.WriteLine("web_change_right_apply: \n");
                    Console.WriteLine(e);
                }
            });*/
            socket.On("web_get_all_special_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string id = (string)jobe.GetValue("id");
                    socket.Emit("web_get_all_special_reply", hospitalService.getAllSpecial(id));
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_get_all_special_apply: \n");
                    Console.WriteLine(e);
                }

            });
            socket.On("web_add_doctor_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string id = (string)jobe.GetValue("id");
                    string name = (string)jobe.GetValue("name");
                    string password = (string)jobe.GetValue("password");
                    string email = (string)jobe.GetValue("email");
                    string dept_name = (string)jobe.GetValue("dept_name");
                    string position = (string)jobe.GetValue("position");
                    string age = (string)jobe.GetValue("age");
                    string gender = (string)jobe.GetValue("gender");
                    var result = hospitalService.DoctorRegister(name, password, email, dept_name, position, gender, age);
                    // todo email may has been used
                    JObject jobj = new JObject();
                    jobj.Add(new JProperty("id", id));
                    jobj.Add(new JProperty("result", result ? "SUCCESS" : "FAIL"));
                    socket.Emit("web_add_doctor_reply", jobj);
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_add_doctor_apply: \n");
                    Console.WriteLine(e);
                }

            });
            socket.On("web_add_nurse_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string id = (string)jobe.GetValue("id");
                    string name = (string)jobe.GetValue("name");
                    string password = (string)jobe.GetValue("password");
                    string email = (string)jobe.GetValue("email");
                    string dept_name = (string)jobe.GetValue("dept_name");
                    string age = (string)jobe.GetValue("age");
                    string gender = (string)jobe.GetValue("gender");
                    var result = hospitalService.NurserRegister(name, password, email, dept_name, gender, age);
                    // todo email may has been used
                    JObject jobj = new JObject();
                    jobj.Add(new JProperty("id", id));
                    jobj.Add(new JProperty("result", result ? "SUCCESS" : "FAIL"));
                    socket.Emit("web_add_nurse_reply", jobj);
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_add_nurse_apply: \n");
                    Console.WriteLine(e);
                }

            });
            socket.On("web_rem_doctor_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string id = (string)jobe.GetValue("id");
                    string email = (string)jobe.GetValue("doctor");
                    JObject jobj = new JObject();
                    var result = hospitalService.RemoveDoctor(email);
                    jobj.Add(new JProperty("id", id));
                    jobj.Add(new JProperty("result", result ? "SUCCESS" : "FAIL"));
                    socket.Emit("web_rem_doctor_reply", jobj);
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_rem_doctor_apply: \n");
                    Console.WriteLine(e);
                }

            });
            socket.On("web_rem_nurse_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string id = (string)jobe.GetValue("id");
                    string email = (string)jobe.GetValue("nurse");
                    JObject jobj = new JObject();
                    var result = hospitalService.RemoveNurse(email);
                    jobj.Add(new JProperty("id", id));
                    jobj.Add(new JProperty("result", result ? "SUCCESS" : "FAIL"));
                    socket.Emit("web_rem_nurse_reply", jobj);
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_rem_nurse_apply: \n");
                    Console.WriteLine(e);
                }

            });
            socket.On("web_rem_doctor_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string id = (string)jobe.GetValue("id");
                    string email = (string)jobe.GetValue("doctor");
                    string password = (string)jobe.GetValue("password");
                    JObject jobj = new JObject();
                    var result = hospitalService.changeDoctorPW(email, password);
                    jobj.Add(new JProperty("id", id));
                    jobj.Add(new JProperty("result", result ? "SUCCESS" : "FAIL"));
                    socket.Emit("web_rem_doctor_reply", jobj);
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_rem_doctor_apply: \n");
                    Console.WriteLine(e);
                }

            });
            socket.On("web_rem_nurser_apply", (data) =>
            {
                try
                {
                    JObject jobe = (JObject)data;
                    string id = (string)jobe.GetValue("id");
                    string email = (string)jobe.GetValue("nurse");
                    string password = (string)jobe.GetValue("password");
                    JObject jobj = new JObject();
                    var result = hospitalService.changeNursePW(email, password);
                    jobj.Add(new JProperty("id", id));
                    jobj.Add(new JProperty("result", result ? "SUCCESS" : "FAIL"));
                    socket.Emit("web_rem_nurser_reply", jobj);
                }
                catch (Exception e)
                {
                    Console.WriteLine("web_rem_nurser_apply: \n");
                    Console.WriteLine(e);
                }

            });
        }

    }
}
