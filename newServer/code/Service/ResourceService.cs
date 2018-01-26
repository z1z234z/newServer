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
    class ResourceService
    {
        private Dao dao;
        public ResourceService()
        {
            dao = new Dao();
        }
        public JArray getAllInstruments()
        {
            ArrayList allInstruments = dao.getAllInstruments();
            JArray result = new JArray(allInstruments.Count);
            for (int i = 0; i < allInstruments.Count; i++)
            {
                JObject temp = new JObject();
                temp.Add(new JProperty("name", ((MEDICAL_INSTRUMENT)allInstruments[i]).NAME));
                temp.Add(new JProperty("number", ((MEDICAL_INSTRUMENT)allInstruments[i]).AVAILABLE.ToString()));
                result.Add(temp);
            }
            return result;
        }
        public bool addInstruments(string name, int number)
        {
            return dao.addInstrument(name, number);
        }
        public bool updInstruments(string oldname, string newname, int number)
        {
            return dao.updInstrument(oldname, newname, number);
        }
        public bool delInstruments(string newname, int number)
        {
            return dao.delInstrument(newname, number);
        }
        public JArray getAllBlood()
        {
            ArrayList allBloods = dao.getAllBloods();
            JArray result = new JArray(allBloods.Count);
            for (int i = 0; i < allBloods.Count; i++)
            {
                JObject temp = new JObject();
                temp.Add(new JProperty("name", ((BLOOD_BANK)allBloods[i]).BLOOD_TYPE));
                temp.Add(new JProperty("number", ((BLOOD_BANK)allBloods[i]).SURPLUS.ToString()));
                result.Add(temp);
            }
            return result;
        }
        public bool addBlood(string name, int number)
        {
            return dao.addBlood(name, number);
        }
        public bool updBlood(string oldname, string newname, int number)
        {
            return dao.updBlood(oldname, newname, number);
        }
        public bool delBlood(string newname, int number)
        {
            return dao.delBlood(newname, number);
        }
        public JArray getAllDrug()
        {
            ArrayList allDrugss = dao.getAllDrugs();
            JArray result = new JArray(allDrugss.Count);
            for (int i = 0; i < allDrugss.Count; i++)
            {
                DRUG drug = dao.getdrugByID((int)((DRUG_INVENTORY)allDrugss[i]).DRUG_ID);
                JObject temp = new JObject();
                temp.Add(new JProperty("ID", ((DRUG_INVENTORY)allDrugss[i]).ID.ToString()));
                temp.Add(new JProperty("name", drug.NAME));
                temp.Add(new JProperty("code", drug.CODE));
                temp.Add(new JProperty("specification", drug.STANDARD));
                temp.Add(new JProperty("factory", drug.MANUFACTOR));
                temp.Add(new JProperty("price_in", drug.PAURCH_PRICE.ToString()));
                temp.Add(new JProperty("price_out", ((DRUG_INVENTORY)allDrugss[i]).PRICE.ToString()));
                temp.Add(new JProperty("number", ((DRUG_INVENTORY)allDrugss[i]).SURPLUS.ToString()));
                temp.Add(new JProperty("time", drug.PRIME.ToString()));
                result.Add(temp);
            }
            return result;
        }
        public JArray getPatientDrug(int id)
        {
            ArrayList list = dao.getMedicalRecordByPat(id);
            MEDICAL_RECORD record = null;
            foreach(MEDICAL_RECORD temp in list)
            {
                if(temp.DRUG_STATE == "取药中")
                {
                    record = temp;
                }
            }
            if (record != null)
            {
                ArrayList allDrugss = dao.getPrescribeByMedicalRecord((int)record.ID);
                JArray result = new JArray(allDrugss.Count);
                for (int i = 0; i < allDrugss.Count; i++)
                {
                    DRUG drug = dao.getdrugByID((int)((PRESCRIBE)allDrugss[i]).DRUG_ID);
                    DRUG_INVENTORY druginventory = ((PRESCRIBE)allDrugss[i]).DRUG_INVENTORY;
                    JObject temp = new JObject();
                    temp.Add(new JProperty("ID", drug.ID.ToString()));
                    temp.Add(new JProperty("name", drug.NAME));
                    temp.Add(new JProperty("code", drug.CODE));
                    temp.Add(new JProperty("specification", drug.STANDARD));
                    temp.Add(new JProperty("factory", drug.MANUFACTOR));
                    temp.Add(new JProperty("price_in", drug.PAURCH_PRICE.ToString()));
                    temp.Add(new JProperty("price_out", druginventory.PRICE.ToString()));
                    temp.Add(new JProperty("number", ((int)((PRESCRIBE)allDrugss[i]).QUANTITY).ToString()));
                    temp.Add(new JProperty("time", drug.PRIME.ToString()));
                    result.Add(temp);
                }
                return result;
            }
            else
                return null;
        }
        public JObject finishtPatientDrug(int id)
        {
            ArrayList list = dao.getMedicalRecordByPat(id);
            MEDICAL_RECORD record = null;
            JObject result = new JObject();
            foreach (MEDICAL_RECORD temp in list)
            {
                if (temp.DRUG_STATE == "取药中")
                {
                    record = temp;
                }
            }
            ArrayList allDrugss = dao.getPrescribeByMedicalRecord((int)record.ID);
            for (int i = 0; i < allDrugss.Count; i++)
            {
                DRUG_INVENTORY drug = ((PRESCRIBE)allDrugss[i]).DRUG_INVENTORY;
                int quantity = (int)drug.SURPLUS - (int)((PRESCRIBE)allDrugss[i]).QUANTITY;
                dao.changeDurgInventory((int)drug.ID, (int)drug.ID, (int)drug.DRUG_ID, (int)drug.PRICE, quantity);
            }
            if (dao.changeMedicalRecord((int)record.ID, (int)record.DOCTOR_ID, (int)record.PATIENT_ID, record.TREAT_STATE, record.TIME, record.DISEASE, record.DESCRIPTION, record.DIAGNOSIS, record.CLIN_STATE, record.INFU_STATE, "全部完成"))
            {
                result.Add(new JProperty("res", "success"));
            }
            else
            {
                result.Add(new JProperty("res", "fail"));
            }
            return result;
        }
        public DRUG getDrugbyName(string name)
        {
            int id = dao.getdrugIDByName(name);
            return dao.getdrugByID(id);
        }
        public DRUG_INVENTORY getDrugInventorybyID(int id)
        {
            return dao.getdrugInventoryByID(id);
        }
        public bool addDrugInventory(int id, string name, int price, int quantity)
        {
            return dao.addDrugInventory(id, name, price, quantity); ;
        }
        public bool updDrugInventory(int oldID, int id, int drug_iD, int price, int quantity)
        {
            return dao.updDrugInventory(oldID, id, drug_iD, price, quantity); ;
        }
        public bool delDrugInventory(int id, string name, int? price, int? quantity)
        {
            return dao.delDrugInventory(id, name, price, quantity);
        }
        public bool addDrug(int id, string name, string standard, int price, string manufactor, int prime)
        {
            return dao.addDrug(id, name, standard, price, manufactor, prime); ;
        }
        public bool updDrug(int oldID, int id, string name, string standard, int price, string manufactor, int prime)
        {
            return dao.updDrug(oldID, id, name, standard, price, manufactor, prime);
        }
        public bool delDrug(int id, string name, string standard, int? price, string manufactor, int? prime)
        {
            return dao.delDrug(id, name, standard, price, manufactor, prime);
        }
        public JArray getAllInventory()
        {
            ArrayList allInventory = dao.getInventory(null, null, null, null);
            JArray result = new JArray(allInventory.Count);
            for (int i = 0; i < allInventory.Count; i++)
            {
                JObject temp = new JObject();
                temp.Add(new JProperty("ID", ((INVENTORY)allInventory[i]).LIST_NUMBRE.ToString()));
                temp.Add(new JProperty("person", ((INVENTORY)allInventory[i]).INVENTORY_PEOPLE));
                temp.Add(new JProperty("year", ((INVENTORY)allInventory[i]).INVENTORY_DATE.Year));
                temp.Add(new JProperty("month", ((INVENTORY)allInventory[i]).INVENTORY_DATE.Month));
                temp.Add(new JProperty("day", ((INVENTORY)allInventory[i]).INVENTORY_DATE.Day));
                temp.Add(new JProperty("hour", ((INVENTORY)allInventory[i]).INVENTORY_DATE.Hour));
                temp.Add(new JProperty("minute", ((INVENTORY)allInventory[i]).INVENTORY_DATE.Minute));
                temp.Add(new JProperty("second", ((INVENTORY)allInventory[i]).INVENTORY_DATE.Second));
                temp.Add(new JProperty("memo", ((INVENTORY)allInventory[i]).REMARK));
                result.Add(temp);
            }
            return result;
        }

        public INVENTORY getInventory(int? Number, string People, string remark, DateTime? Date)
        {
            ArrayList inventory = dao.getInventory(Number, People, remark, Date);
            INVENTORY result = (INVENTORY)inventory[0];
            return result;
        }
        public bool addInventory(int? Number, string People, string remark, DateTime? Date)
        {
            return dao.addInventory(Number, People, remark, Date);
        }
        public bool delInventory(int? Number, string People, string remark, DateTime? Date)
        {
            return dao.delInventory(Number, People, remark, Date);
        }

        public bool addInventoryExample(int? ID, int? Number, int? quantity_old, int? quantity_new)
        {
            return dao.addInventoryExample(ID, Number, quantity_old, quantity_new);
        }
        public JArray getInventExampleByID(int id)
        {
            ArrayList allInventExample = dao.getInventoryExample(null, id, null, null);
            JArray result = new JArray(allInventExample.Count);
            for (int i = 0; i < allInventExample.Count; i++)
            {
                DRUG drug = dao.getdrugByID((int)((INVENTORY_EXAMPLE)allInventExample[i]).ID);
                JObject temp = new JObject();
                temp.Add(new JProperty("ID", ((INVENTORY_EXAMPLE)allInventExample[i]).ID.ToString()));
                temp.Add(new JProperty("code", drug.CODE));
                temp.Add(new JProperty("name", drug.NAME));
                temp.Add(new JProperty("time", drug.PRIME.ToString()));
                temp.Add(new JProperty("recordNumber", ((INVENTORY_EXAMPLE)allInventExample[i]).QUANTITY_OLD));
                temp.Add(new JProperty("realNumber", ((INVENTORY_EXAMPLE)allInventExample[i]).QUANTITY_NEW));
                result.Add(temp);
            }
            return result;
        }
    }
}
