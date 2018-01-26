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

    class ResourceController
    {

        private Socket socket;
        private ResourceService resourceService;
        public ResourceController(Socket sockets)
        {
            socket = sockets;
            resourceService = new ResourceService();

        }
        public void startListening()
        {
            socket.On("java_instrument_apply", (data) =>
            {
                try
                {
                    var result = resourceService.getAllInstruments();
                    socket.Emit("java_instrument_reply", result);
                }
                catch (Exception e)
                {
                    Console.WriteLine("java_instrument_apply: \n");
                    Console.WriteLine(e);
                }

            });
            socket.On("java_instrument_add_apply", (data) =>
            {
                try
                {
                    JArray temp = (JArray)data;
                    JObject Data = (JObject)temp[0];
                    string name = (string)Data.GetValue("name");
                    string num = (string)Data.GetValue("number");
                    int number = int.Parse(num);
                    bool result = resourceService.addInstruments(name, number);
                    if (result)
                    {
                        Console.WriteLine("java_instrument_add_apply: \n");
                        socket.Emit("java_instrument_add_reply", temp);
                    }
                    else
                    {

                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            });
            socket.On("java_instrument_rm_apply", (data) =>
            {
                try
                {
                    JArray temp = (JArray)data;
                    JObject Data = (JObject)temp[0];
                    string name = (string)Data.GetValue("name");
                    string num = (string)Data.GetValue("number");
                    int number = int.Parse(num);
                    bool result = resourceService.delInstruments(name, number);
                    JObject res = new JObject();
                    if (result)
                    {
                        res.Add(new JProperty("res", "success"));
                    }
                    else
                    {
                        res.Add(new JProperty("res", "fail"));
                    }
                    socket.Emit("java_instrument_rm_reply", res);
                }
                catch (Exception e)
                {
                    Console.WriteLine("java_instrument_rm_apply: \n");
                    Console.WriteLine(e);
                }

            });
            socket.On("java_instrument_change_apply", (data) =>
            {
                try
                {
                    JArray temp = (JArray)data;
                    JObject Data = (JObject)temp[0];
                    string oldname = (string)Data.GetValue("old_name");
                    string newname = (string)Data.GetValue("new_name");
                    string num = (string)Data.GetValue("number");
                    int number = int.Parse(num);
                    bool result = resourceService.updInstruments(oldname, newname, number);
                    JObject res = new JObject();
                    if (result)
                    {
                        res.Add(new JProperty("res", "success"));
                    }
                    else
                    {
                        res.Add(new JProperty("res", "fail"));
                    }
                    socket.Emit("java_instrument_change_reply", res);
                }
                catch (Exception e)
                {
                    Console.WriteLine("java_instrument_change_apply: \n");
                    Console.WriteLine(e);
                }

            });
            socket.On("java_blood_apply", (data) =>
            {
                try
                {
                    var result = resourceService.getAllBlood();
                    socket.Emit("java_blood_reply", result);
                }
                catch (Exception e)
                {
                    Console.WriteLine("java_blood_apply: \n");
                    Console.WriteLine(e);
                }
            });
            socket.On("java_blood_add_apply", (data) =>
            {
                try
                {
                    JArray temp = (JArray)data;
                    JObject Data = (JObject)temp[0];
                    string name = (string)Data.GetValue("name");
                    string num = (string)Data.GetValue("number");
                    int number = int.Parse(num);
                    bool result = resourceService.addBlood(name, number);
                    if (result)
                    {
                        socket.Emit("java_blood_add_reply", temp);
                    }
                    else
                    {

                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("java_blood_add_apply: \n");
                    Console.WriteLine(e);
                }
            });
            socket.On("java_blood_rm_apply", (data) =>
            {
                try
                {
                    JArray temp = (JArray)data;
                    JObject Data = (JObject)temp[0];
                    string name = (string)Data.GetValue("name");
                    string num = (string)Data.GetValue("number");
                    int number = int.Parse(num);
                    bool result = resourceService.delBlood(name, number);
                    JObject res = new JObject();
                    if (result)
                    {
                        res.Add(new JProperty("res", "success"));
                    }
                    else
                    {
                        res.Add(new JProperty("res", "fail"));
                    }
                    socket.Emit("java_blood_rm_reply", res);
                }
                catch (Exception e)
                {
                    Console.WriteLine("java_blood_rm_apply: \n");
                    Console.WriteLine(e);
                }
            });
            socket.On("java_blood_change_apply", (data) =>
            {
                try
                {
                    JArray temp = (JArray)data;
                    JObject Data = (JObject)temp[0];
                    string oldname = (string)Data.GetValue("old_name");
                    string newname = (string)Data.GetValue("new_name");
                    string num = (string)Data.GetValue("number");
                    int number = int.Parse(num);
                    bool result = resourceService.updBlood(oldname, newname, number);
                    JObject res = new JObject();
                    if (result)
                    {
                        res.Add(new JProperty("res", "success"));
                    }
                    else
                    {
                        res.Add(new JProperty("res", "fail"));
                    }
                    socket.Emit("java_blood_change_reply", res);
                }
                catch (Exception e)
                {
                    Console.WriteLine("java_blood_change_apply: \n");
                    Console.WriteLine(e);
                }
            });
            socket.On("java_drug_apply", (data) =>
            {
                try
                {
                    var result = resourceService.getAllDrug();
                    socket.Emit("java_drug_reply", result);
                }
                catch (Exception e)
                {
                    Console.WriteLine("java_drug_apply: \n");
                    Console.WriteLine(e);
                }
            });

            socket.On("java_drug_add_apply", (data) =>
            {
                try
                {
                    long add = 0; bool result = true;
                    JArray temp = (JArray)data;
                    JObject res = new JObject();
                    foreach (var temp_date in temp)
                    {
                        JObject Data = (JObject)temp_date;
                        string i = (DateTime.Now.ToFileTime() % 100000 + add).ToString();
                        string name = (string)Data.GetValue("name");
                        string specification = (string)Data.GetValue("specification");
                        string factory = (string)Data.GetValue("factory");
                        int pricein = (int)Data.GetValue("price_in");
                        int priceout = (int)Data.GetValue("price_out");
                        int number = (int)Data.GetValue("number");
                        string time = (string)Data.GetValue("time");
                        int id = int.Parse(i);
                        if (resourceService.getDrugbyName(name) == null)
                            resourceService.addDrug(id, name, specification, pricein, factory, int.Parse(time));
                        if (!resourceService.addDrugInventory(id, name, priceout, number))
                            result = false;
                        add++;
                    }
                    if (result)
                    {
                        res.Add(new JProperty("res", "success"));
                    }
                    else
                    {
                        res.Add(new JProperty("res", "fail"));
                    }
                    socket.Emit("java_drug_add_reply", res);
                }
                catch (Exception e)
                {
                    Console.WriteLine("java_drug_add_apply: \n");
                    Console.WriteLine(e);
                }
            });
            socket.On("java_drug_rm_apply", (data) =>
            {
                try
                {
                    bool result = true;
                    JArray temp = (JArray)data;
                    JObject res = new JObject();
                    foreach (var temp_date in temp)
                    {
                        JObject Data = (JObject)temp[0];
                        string i = (string)Data.GetValue("ID");
                        int id = int.Parse(i);
                        int drugid = (int)resourceService.getDrugInventorybyID(id).DRUG_ID;
                        if ((!resourceService.delDrugInventory(id, null, null, null)))
                            result = false;
                        if ((!resourceService.delDrug(drugid, null, null, null, null, null)))
                            result = false;
                    }
                    if (result)
                    {
                        res.Add(new JProperty("res", "success"));
                    }
                    else
                    {
                        res.Add(new JProperty("res", "fail"));
                    }
                    socket.Emit("java_drug_rm_reply", res);
                }
                catch (Exception e)
                {
                    Console.WriteLine("java_drug_rm_apply: \n");
                    Console.WriteLine(e);
                }
            });
            socket.On("java_drug_change_apply", (data) =>
            {
                try
                {
                    JArray temp = (JArray)data;
                    JObject Data = (JObject)temp[0];
                    string oi = (string)Data.GetValue("old_id");
                    string ni = (string)Data.GetValue("new_id");
                    string name = (string)Data.GetValue("name");
                    string pri = (string)Data.GetValue("price");
                    string num = (string)Data.GetValue("number");
                    int newid = int.Parse(ni);
                    int oldid = int.Parse(oi);
                    int price = int.Parse(pri);
                    int number = int.Parse(num);
                    int drugID = (int)resourceService.getDrugbyName(name).ID;
                    bool result = resourceService.updDrugInventory(oldid, newid, drugID, price, number);
                    JObject res = new JObject();
                    if (result)
                    {
                        res.Add(new JProperty("res", "success"));
                    }
                    else
                    {
                        res.Add(new JProperty("res", "fail"));
                    }
                    socket.Emit("java_drug_change_reply", res);
                }
                catch (Exception e)
                {
                    Console.WriteLine("java_drug_change_apply: \n");
                    Console.WriteLine(e);
                }
            });
            socket.On("java_inventory_apply", (data) =>
            {
                try
                {
                    var result = resourceService.getAllInventory();
                    socket.Emit("java_inventory_reply", result);
                }
                catch (Exception e)
                {
                    Console.WriteLine("java_inventory_apply: \n");
                    Console.WriteLine(e);
                }
            });
            socket.On("java_inventory_add_apply", (data) =>
            {
                try
                {
                    bool result = true;
                    JObject Data = (JObject)data;
                    JObject res = new JObject();
                    string id = (DateTime.Now.ToFileTime() % 100000).ToString();
                    string person = (string)Data.GetValue("person");
                    string remark = (string)Data.GetValue("memo");
                    int year = (int)Data.GetValue("year");
                    int day = (int)Data.GetValue("day");
                    int month = (int)Data.GetValue("month");
                    int hour = (int)Data.GetValue("hour");
                    int minute = (int)Data.GetValue("minute");
                    int second = (int)Data.GetValue("second");
                    DateTime date = new DateTime(year, month, day, month, minute, second);
                    JArray drug = (JArray)Data.GetValue("drug");
                    if (!resourceService.addInventory(int.Parse(id), person, remark, date))
                        result = false;
                    else
                    {
                        Thread.Sleep(2000);
                        INVENTORY inventory = resourceService.getInventory(null, person, remark, date);
                        foreach (JObject temp in drug)
                        {
                            int Number = (int)temp.GetValue("ID");
                            int quantity_new = (int)temp.GetValue("number");
                            DRUG_INVENTORY drug_inventory = resourceService.getDrugInventorybyID(Number);
                            int quantity_old = (int)drug_inventory.SURPLUS;
                            if (!resourceService.addInventoryExample(Number, (int)inventory.LIST_NUMBRE, quantity_old, quantity_new))
                                result = false;
                            else
                                resourceService.updDrugInventory((int)drug_inventory.ID, (int)drug_inventory.ID, (int)drug_inventory.DRUG_ID, (int)drug_inventory.PRICE, quantity_new);
                        }
                    }
                    if (result)
                    {
                        res.Add(new JProperty("res", "success"));
                    }
                    else
                    {
                        res.Add(new JProperty("res", "fail"));
                    }
                    socket.Emit("java_inventory_add_reply", res);
                }
                catch (Exception e)
                {
                    Console.WriteLine("java_inventory_add_apply: \n");
                    Console.WriteLine(e);
                }
            });
            socket.On("java_inventory_content_apply", (data) =>
            {
                try
                {
                    JObject Data = (JObject)data;
                    int id = int.Parse(Data.GetValue("ID").ToString());
                    var result = resourceService.getInventExampleByID(id);
                    socket.Emit("java_inventory_content_reply", result);
                }
                catch (Exception e)
                {
                    Console.WriteLine("java_inventory_content_apply: \n");
                    Console.WriteLine(e);
                }
            });
            socket.On("java_patientdrug_apply", (data) =>
            {
                try
                {
                    JObject Data = (JObject)data;
                    int pat_id = int.Parse(Data.GetValue("ID").ToString());
                    var result = resourceService.getPatientDrug(pat_id);
                    socket.Emit("java_patientdrug_reply", result);
                }
                catch (Exception e)
                {
                    Console.WriteLine("java_patientdrug_apply: \n");
                    Console.WriteLine(e);
                }
            });
            socket.On("java_patientdrug_clear_apply", (data) =>
            {
                try
                {
                    JObject Data = (JObject)data;
                    int pat_id = int.Parse(Data.GetValue("ID").ToString());
                    var result = resourceService.finishtPatientDrug(pat_id);
                    socket.Emit("java_patientdrug_clear_reply", result);
                }
                catch (Exception e)
                {
                    Console.WriteLine("java_patientdrug_clear_apply: \n");
                    Console.WriteLine(e);
                }
            });
        }
    }
}
