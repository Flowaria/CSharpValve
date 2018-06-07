using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TF2.Population
{
    public class Attributes
    {
        public const float NULL_VALUE = -999.999f;
        private int[] attrid;
        private float[] attrvalue;
        private bool[] isstr;
        private string[] attrvalue_str;
        private int size;
        public Attributes()
        {
            attrid.Initialize();
            attrvalue.Initialize();
            attrvalue_str.Initialize();
            isstr.Initialize();
        }

        public void Set(int id, float value)
        {
            if (IsExist(id))
            {
                if (!IsStr(id))
                {
                    int idx = Array.FindIndex(attrid, p => p == id);
                    attrid[idx] = id;
                    attrvalue[idx] = value;
                }
            }
            else
            {
                int p = attrid.GetLength(1);
                attrid[p] = id;
                attrvalue[p] = value;
                isstr[p] = false;
            }
        }

        public float Get(int id)
        {
            if (IsExist(id))
            {
                if (!IsStr(id))
                {
                    int idx = Array.FindIndex(attrid, p => p == id);
                    return attrvalue[idx];
                }
            }
            return NULL_VALUE;
        }

        public void SetString(int id, string value)
        {
            if (IsExist(id))
            {
                if (IsStr(id))
                {
                    int idx = Array.FindIndex(attrid, p => p == id);
                    attrid[idx] = id;
                    attrvalue_str[idx] = value;
                }
            }
            else
            {
                int p = attrid.GetLength(1);
                attrid[p] = id;
                attrvalue_str[p] = value;
                isstr[p] = true;
            }
        }

        public string GetString(int id)
        {
            if (IsStr(id))
            {
                if (IsExist(id))
                {
                    int idx = Array.FindIndex(attrid, p => p == id);
                    return attrvalue_str[idx];
                }
            }
            return null;
        }

        public bool Remove(int id)
        {
            try
            {
                if (IsExist(id))
                {
                    int idx = Array.FindIndex(attrid, p => p == id);
                    var list = attrid.ToList();
                    list.RemoveAt(idx);
                    attrid = list.ToArray();

                    if (isstr[idx])
                    {
                        var ilist = attrvalue_str.ToList();
                        ilist.RemoveAt(idx);
                        attrvalue_str = ilist.ToArray();
                    }
                    else
                    {
                        var ilist = attrvalue.ToList();
                        ilist.RemoveAt(idx);
                        attrvalue = ilist.ToArray();
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Clear()
        {
            Array.Clear(attrid, 0, attrid.Length);
            Array.Clear(attrvalue, 0, attrvalue.Length);
        }

        public bool IsExist(int id)
        {
            try
            {
                Array.Find(attrid, p => p == id);
                return true;
            }
            catch (ArgumentNullException)
            {
                return false;
            }
        }

        public bool IsStr(int id)
        {
            try
            {
                int idx = Array.FindIndex(attrid, p => p == id);
                return isstr[idx];
            }
            catch (ArgumentNullException)
            {
                return false;
            }
        }
    }
}
