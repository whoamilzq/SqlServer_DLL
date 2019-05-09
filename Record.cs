using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace SqlServer_dll
{
    public class Record
    {
        private ArrayList l = new ArrayList();

        public String toString()
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < l.Count; i++)
            {
                NameValuePair p = (NameValuePair)l[i];
                result.Append(p.toString() + "\t");
            }
            return result.ToString();
        }

        public void add(String Name, object Value)
        {
            NameValuePair p = new NameValuePair(Name, Value);
            l.Add(p);
        }

        public void setValue(int serial, object Value)
        {
            if (serial <= l.Count)
            {
                NameValuePair p = (NameValuePair)l[serial - 1];
                p.setO(Value);
            }
        }

        public void setValue(String Name, object Value)
        {
            NameValuePair p = find(Name);
            if (p != null)
            {
                p.setO(Value);
            }
        }

        public void setAndAddValue(String Name, Object Value)
        {
            NameValuePair p = find(Name);
            if (p != null)
            {
                p.setO(Value);
            }
            else
            {
                add(Name,Value);
            }
        }

        public Object getValue(int serial)
        {
            if (serial <= l.Count)
            {
                NameValuePair p = (NameValuePair)l[serial - 1];
                return p.getO();
            }
            else
            {
                return null;
            }
        }

        public Object getValue(String Name)
        {
            Object result = null;
            NameValuePair p = find(Name);
            if (p != null)
            {
                result = p.getO();
            }
            return result;
        }

        public String getStrValue(String Name)
        {
            String result = "";
            NameValuePair p = find(Name);
            if (p != null)
            {
                if (p.getO() != null)
                {
                    result = p.getO().ToString();
                }
            }
            return result;
        }

        public String getStrValue(int serial)
        {
            String result = "";
            if (serial <= l.Count)
            {
                NameValuePair p = (NameValuePair)l[serial - 1];
                if (p.getO() != null)
                {
                    result = p.getO().ToString();
                }
            }
            return result;
        }

        public int getIntValue(String Name)
        {
            int result = -1;
            NameValuePair p = find(Name);
            if (p != null)
            {
                if (p.getO() != null)
                {
                    result = Convert.ToInt16(p.getO().ToString());
                }
            }
            return result;
        }

        public int getIntValue(int serial)
        {
            int result = -1;
            if (serial <= l.Count)
            {
                NameValuePair p = (NameValuePair)l[serial - 1];
                if (p.getO() != null)
                {
                    result = Convert.ToInt16(p.getO().ToString());
                }
            }
            return result;
        }


        public long getLongValue(String Name)
        {
            long result = -1;
            NameValuePair p = find(Name);
            if (p != null)
            {
                if (p.getO() != null)
                {
                    result = Convert.ToInt32(p.getO().ToString());
                }
            }
            return result;
        }

        public long getLongValue(int serial)
        {
            long result = -1;
            if (serial <= l.Count)
            {
                NameValuePair p = (NameValuePair)l[serial - 1];
                if (p.getO() != null)
                {
                    result = Convert.ToInt32(p.getO().ToString());
                }
            }
            return result;
        }

        public ArrayList getList()
        {
            return this.l;
        }

        private NameValuePair find(String Name)
        {
            NameValuePair np = null;
            for (int i = 0; i < l.Count; i++)
            {
                NameValuePair p = (NameValuePair)l[i];               
                if (p.getName().ToLower().Equals(Name.ToLower()))
                {
                    np = p;
                    break;
                }
               
            }
            return np;
        }
    }
}
